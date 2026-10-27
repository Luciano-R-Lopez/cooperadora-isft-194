using CooperadoraISFT94.DTOs.Egresos;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class EgresoService : IEgresoService
    {
        private static readonly Lazy<EgresoService> _instancia = new(() =>
            new EgresoService(UsuarioRepository.Instancia, MedioPagoRepository.Instancia,
                              MotivoEgresoRepository.Instancia, GrupoEgresoRepository.Instancia,
                              MovimientoRegistrador.Instancia));
        public static EgresoService Instancia => _instancia.Value;

        private readonly IUsuarioRepository _usuarios;
        private readonly IMedioPagoRepository _medios;
        private readonly IMotivoEgresoRepository _motivos;
        private readonly IGrupoEgresoRepository _grupos;
        private readonly IMovimientoRegistrador _registrador;

        private readonly Func<DateTime> _reloj;

        internal EgresoService(IUsuarioRepository usuarios, IMedioPagoRepository medios,
                               IMotivoEgresoRepository motivos, IGrupoEgresoRepository grupos,
                               IMovimientoRegistrador registrador,
                               Func<DateTime>? reloj = null)
        {
            _reloj = reloj ?? (() => DateTime.Now);
            _usuarios = usuarios;
            _medios = medios;
            _motivos = motivos;
            _grupos = grupos;
            _registrador = registrador;
        }

        public EgresoDto Registrar(EgresoCrearDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // Monto lo valida el Egreso; acá solo se buscan las entidades relacionadas.
            var usuario = _usuarios.ObtenerExistente(usuarioId);
            var medio = _medios.ObtenerExistente(dto.MedioPagoId);
            medio.ExigirActivo();
            var motivo = _motivos.ObtenerExistente(dto.MotivoEgresoId);
            motivo.ExigirActivo();
            var grupo = _grupos.ObtenerExistente(dto.GrupoEgresoId);
            grupo.ExigirActivo();

            var egreso = new Egreso(usuario, _reloj(), dto.Monto, medio, motivo, grupo);
            _registrador.Registrar(egreso);

            return EgresoMapper.ADto(egreso);
        }
    }
}
