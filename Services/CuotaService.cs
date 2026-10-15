using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class CuotaService : ICuotaService
    {
        private static readonly Lazy<CuotaService> _instancia = new(() =>
            new CuotaService(CuotaRepository.Instancia, SocioRepository.Instancia, UsuarioRepository.Instancia,
                             MedioPagoRepository.Instancia, DetalleIngresoCuotaRepository.Instancia,
                             MovimientoRegistrador.Instancia, AuditoriaService.Instancia));
        public static CuotaService Instancia => _instancia.Value;

        private readonly ICuotaRepository _cuotas;
        private readonly ISocioRepository _socios;
        private readonly IUsuarioRepository _usuarios;
        private readonly IMedioPagoRepository _medios;
        private readonly IDetalleIngresoCuotaRepository _detalles;
        private readonly IMovimientoRegistrador _registrador;
        private readonly IAuditoriaService _auditoria;

        private CuotaService(ICuotaRepository cuotas, ISocioRepository socios, IUsuarioRepository usuarios,
                             IMedioPagoRepository medios, IDetalleIngresoCuotaRepository detalles,
                             IMovimientoRegistrador registrador, IAuditoriaService auditoria)
        {
            _cuotas = cuotas;
            _socios = socios;
            _usuarios = usuarios;
            _medios = medios;
            _detalles = detalles;
            _registrador = registrador;
            _auditoria = auditoria;
        }

        public IReadOnlyList<CuotaDto> ObtenerPorSocio(int socioId)
        {
            var socio = _socios.ObtenerExistente(socioId);
            return _cuotas.ObtenerPorSocio(socio).Select(CuotaMapper.ADto).ToList();
        }

        public CuotaDto Generar(CuotaCrearDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var socio = _socios.ObtenerExistente(dto.SocioId);

            // La Cuota valida monto y año; acá solo se controla lo que depende de las otras cuotas del socio.
            var cuota = new Cuota(socio, dto.Monto, dto.Anio);

            if (_cuotas.ExisteParaAnio(socio, cuota.Anio))
                throw new ReglaNegocioException("El socio ya tiene una cuota para ese año.");

            _cuotas.Agregar(cuota);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Cuota), cuota.Id);

            return CuotaMapper.ADto(cuota);
        }

        public decimal? ObtenerMontoSugerido(int anio) =>
            _cuotas.ObtenerUltimaDelAnio(anio)?.Monto;

        public CuotaDto RegistrarPago(int cuotaId, int medioPagoId, int usuarioId)
        {
            var cuota = _cuotas.ObtenerExistente(cuotaId);
            cuota.ValidarPagable();

            var usuario = _usuarios.ObtenerExistente(usuarioId);
            var medio = _medios.ObtenerExistente(medioPagoId);

            // Todo lo que puede fallar por los datos se construye antes de tocar la caja;
            // el registro del ingreso (que exige caja abierta) va último, así si falla la cuota queda como estaba.
            var ingreso = new Ingreso(usuario, DateTime.Now, cuota.Monto, medio, CategoriaIngreso.CuotaSocial);
            var detalle = new DetalleIngresoCuota(ingreso, cuota, cuota.Monto);

            _registrador.Registrar(ingreso);
            _detalles.Agregar(detalle);

            cuota.CambiarEstado(EstadoCuota.Pagada);
            _cuotas.Actualizar(cuota);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Cuota), cuota.Id);

            return CuotaMapper.ADto(cuota);
        }
    }
}
