using CooperadoraISFT94.Common;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Repository.MySql;
using CooperadoraISFT94.Repository.MySql.Persistencias;

namespace CooperadoraISFT94.Repository
{
    // Único lugar donde se eligen las implementaciones de los repositorios: en memoria, o en memoria y además
    // guardados en MySQL (Configuracion.UsarBaseDeDatos). Los servicios no se enteran de cuál se usa.
    public static class Repositorios
    {
        public static readonly IRepository<Usuario> Usuarios =
            Elegir(UsuarioRepository.ObtenerInstancia(), new UsuarioPersistencia());
        public static readonly IRepository<Socio> Socios =
            Elegir(SocioRepository.ObtenerInstancia(), new SocioPersistencia());
        public static readonly IRepository<Cuota> Cuotas =
            Elegir(CuotaRepository.ObtenerInstancia(), new CuotaPersistencia());
        public static readonly IRepository<ValorCuota> ValoresCuota =
            Elegir(ValorCuotaRepository.ObtenerInstancia(), new ValorCuotaPersistencia());
        public static readonly IRepository<MedioPago> MediosPago =
            Elegir(MedioPagoRepository.ObtenerInstancia(), new MedioPagoPersistencia());
        public static readonly IRepository<MotivoEgreso> MotivosEgreso =
            Elegir(MotivoEgresoRepository.ObtenerInstancia(), new MotivoEgresoPersistencia());
        public static readonly IRepository<GrupoEgreso> GruposEgreso =
            Elegir(GrupoEgresoRepository.ObtenerInstancia(), new GrupoEgresoPersistencia());
        public static readonly IRepository<Caja> Cajas =
            Elegir(CajaRepository.ObtenerInstancia(), new CajaPersistencia());
        public static readonly IRepository<CajaTotalMedioPago> TotalesMedioPago =
            Elegir(CajaTotalMedioPagoRepository.ObtenerInstancia(), new CajaTotalMedioPagoPersistencia());
        public static readonly IRepository<Movimiento> Movimientos =
            Elegir(MovimientoRepository.ObtenerInstancia(), new MovimientoPersistencia());
        public static readonly IRepository<DetalleIngresoCuota> DetallesIngresoCuota =
            Elegir(DetalleIngresoCuotaRepository.ObtenerInstancia(), new DetalleIngresoCuotaPersistencia());
        public static readonly IRepository<Auditoria> Auditorias =
            Elegir(AuditoriaRepository.ObtenerInstancia(), new AuditoriaPersistencia());

        private static IRepository<T> Elegir<T>(IRepository<T> memoria, IPersistencia<T> mysql) where T : class, IEntidad
        {
            return Configuracion.UsarBaseDeDatos ? new RepositorioMySql<T>(memoria, mysql) : memoria;
        }
    }
}
