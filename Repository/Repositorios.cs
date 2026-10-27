using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;

namespace CooperadoraISFT94.Repository
{
    // Único lugar donde se eligen las implementaciones de los repositorios. Para pasar a una base de datos,
    // cada uno se cambia por su versión (por ejemplo, Repository.MySql) y los servicios no cambian.
    public static class Repositorios
    {
        public static readonly IRepository<Usuario> Usuarios = UsuarioRepository.ObtenerInstancia();
        public static readonly IRepository<Socio> Socios = SocioRepository.ObtenerInstancia();
        public static readonly IRepository<Cuota> Cuotas = CuotaRepository.ObtenerInstancia();
        public static readonly IRepository<ValorCuota> ValoresCuota = ValorCuotaRepository.ObtenerInstancia();
        public static readonly IRepository<MedioPago> MediosPago = MedioPagoRepository.ObtenerInstancia();
        public static readonly IRepository<MotivoEgreso> MotivosEgreso = MotivoEgresoRepository.ObtenerInstancia();
        public static readonly IRepository<GrupoEgreso> GruposEgreso = GrupoEgresoRepository.ObtenerInstancia();
        public static readonly IRepository<Caja> Cajas = CajaRepository.ObtenerInstancia();
        public static readonly IRepository<CajaTotalMedioPago> TotalesMedioPago = CajaTotalMedioPagoRepository.ObtenerInstancia();
        public static readonly IRepository<Movimiento> Movimientos = MovimientoRepository.ObtenerInstancia();
        public static readonly IRepository<DetalleIngresoCuota> DetallesIngresoCuota = DetalleIngresoCuotaRepository.ObtenerInstancia();
        public static readonly IRepository<Auditoria> Auditorias = AuditoriaRepository.ObtenerInstancia();
    }
}
