using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Tests
{
    /// <summary>
    /// Servicios conectados a repositorios en memoria nuevos (no a los singletons de la app) y a un reloj
    /// que el test controla. Cada test arma el suyo, así no comparten datos.
    /// </summary>
    internal sealed class Contexto
    {
        public DateTime Ahora { get; set; } = new(2026, 3, 10, 10, 0, 0);

        public UsuarioRepository Usuarios { get; } = new();
        public SocioRepository Socios { get; } = new();
        public CuotaRepository Cuotas { get; } = new();
        public MedioPagoRepository Medios { get; } = new();
        public MotivoEgresoRepository Motivos { get; } = new();
        public GrupoEgresoRepository Grupos { get; } = new();
        public MovimientoRepository Movimientos { get; } = new();
        public CajaRepository Cajas { get; } = new();
        public CajaTotalMedioPagoRepository Totales { get; } = new();
        public DetalleIngresoCuotaRepository Detalles { get; } = new();
        public ValorCuotaRepository Valores { get; } = new();
        public AuditoriaRepository Auditorias { get; } = new();

        public AuditoriaService Auditoria { get; }
        public AuthService Auth { get; }
        public UsuarioService UsuariosServicio { get; }
        public SocioService SociosServicio { get; }
        public CatalogoService Catalogos { get; }
        public CajaService CajasServicio { get; }
        public MovimientoRegistrador Registrador { get; }
        public CuotaService CuotasServicio { get; }
        public IngresoService Ingresos { get; }
        public EgresoService Egresos { get; }
        public MovimientoService MovimientosServicio { get; }

        public Usuario Operador { get; }
        public Usuario Contador { get; }
        public MedioPago Efectivo { get; }
        public MedioPago Transferencia { get; }
        public MotivoEgreso Materiales { get; }
        public GrupoEgreso Estudiantes { get; }
        public GrupoEgreso Administracion { get; }

        public Contexto()
        {
            DateTime Reloj() => Ahora;

            Auditoria = new AuditoriaService(Auditorias, Usuarios, Reloj);
            Auth = new AuthService(Usuarios, Reloj);
            UsuariosServicio = new UsuarioService(Usuarios, Auditoria);
            SociosServicio = new SocioService(Socios, Auditoria);
            Catalogos = new CatalogoService(Medios, Motivos, Grupos, Auditoria);
            CajasServicio = new CajaService(Cajas, Movimientos, Totales, Auditoria, Reloj);
            Registrador = new MovimientoRegistrador(Movimientos, Cajas, Totales, Auditoria, Reloj);
            CuotasServicio = new CuotaService(Cuotas, Socios, Usuarios, Medios, Detalles, Valores, Registrador, Auditoria, Reloj);
            Ingresos = new IngresoService(Usuarios, Medios, Registrador, Reloj);
            Egresos = new EgresoService(Usuarios, Medios, Motivos, Grupos, Registrador, Reloj);
            MovimientosServicio = new MovimientoService(Movimientos, Detalles, Medios, Cajas, Usuarios, Cuotas, Grupos,
                                                        Registrador, Auditoria);

            Operador = Usuarios.Agregar(new Usuario("operador", "Omar", "Operador", PasswordHasher.Hashear("operador123"), true, Rol.Operador));
            Contador = Usuarios.Agregar(new Usuario("contador", "Carla", "Contadora", PasswordHasher.Hashear("contador123"), true, Rol.Contador));
            Efectivo = Medios.Agregar(new MedioPago("Efectivo"));
            Transferencia = Medios.Agregar(new MedioPago("Transferencia"));
            Materiales = Motivos.Agregar(new MotivoEgreso("Materiales"));
            Estudiantes = Grupos.Agregar(new GrupoEgreso("Estudiantes"));
            Administracion = Grupos.Agregar(new GrupoEgreso("Administración"));
        }

        public Socio Socio(string dni = "30123456", Categoria categoria = Categoria.Socio, Socio? responsable = null)
        {
            var socio = new Socio("Nombre", "Apellido" + dni, dni, "", "", categoria);
            socio.AsignarResponsable(responsable);
            return Socios.Agregar(socio);
        }

        public Cuota Cuota(Socio socio, decimal monto = 5000m, int anio = 2026) =>
            Cuotas.Agregar(new Cuota(socio, monto, anio));

        public void AbrirCaja(decimal montoInicial = 0m) =>
            CajasServicio.Abrir(new AbrirCajaDto(montoInicial), Contador.Id);
    }
}
