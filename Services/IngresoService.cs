using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Ingresos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class IngresoService : IIngresoService
    {
        private static readonly Lazy<IngresoService> _instancia = new(() =>
            new IngresoService(UsuarioRepository.Instancia, MedioPagoRepository.Instancia, MovimientoRegistrador.Instancia));
        public static IngresoService Instancia => _instancia.Value;

        private readonly IUsuarioRepository _usuarios;
        private readonly IMedioPagoRepository _medios;
        private readonly IMovimientoRegistrador _registrador;

        private IngresoService(IUsuarioRepository usuarios, IMedioPagoRepository medios, IMovimientoRegistrador registrador)
        {
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

            var ingreso = new Ingreso(usuario, DateTime.Now, dto.Monto, medio, dto.CategoriaIngreso);
            _registrador.Registrar(ingreso);

            return IngresoMapper.ADto(ingreso);
        }
    }
}
