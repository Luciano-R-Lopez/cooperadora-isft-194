using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Repository;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Service
{
    public sealed class IngresoService : IIngresoService
    {
        private static readonly Lazy<IngresoService> _instancia = new(() =>
            new IngresoService(UsuarioRepository.Instancia, MedioPagoRepository.Instancia, MovimientoRegistrador.Instancia));
        public static IngresoService Instancia => _instancia.Value;

        private readonly IUsuarioRepository _usuarios;
        private readonly IMedioPagoRepository _medios;
        private readonly IMovimientoRegistrador _registrador;

        private readonly Func<DateTime> _reloj;

        internal IngresoService(IUsuarioRepository usuarios, IMedioPagoRepository medios, IMovimientoRegistrador registrador,
                                Func<DateTime>? reloj = null)
        {
            _reloj = reloj ?? (() => DateTime.Now);
            _usuarios = usuarios;
            _medios = medios;
            _registrador = registrador;
        }

        public IngresoDto Registrar(IngresoCrearDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // Las cuotas sociales se cobran desde la cuota, para que el ingreso quede ligado a ella.
            if (dto.CategoriaIngreso == CategoriaIngreso.CuotaSocial)
                throw new ReglaNegocioException("Las cuotas sociales se cobran desde la ficha del socio.");

            // Solo se controla lo que depende de otros datos; monto y categoría los valida el Ingreso.
            var usuario = _usuarios.ObtenerExistente(usuarioId);
            var medio = _medios.ObtenerExistente(dto.MedioPagoId);
            medio.ExigirActivo();

            var ingreso = new Ingreso(usuario, _reloj(), dto.Monto, medio, dto.CategoriaIngreso);
            _registrador.Registrar(ingreso);

            return IngresoMapper.ADto(ingreso);
        }
    }
}
