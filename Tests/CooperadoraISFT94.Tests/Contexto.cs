using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Tests
{
    // Servicios conectados a repositorios en memoria nuevos (no a los de la aplicación) y a un reloj que el test
    // controla. Cada test arma el suyo, así no comparten datos.
    internal class Contexto
    {
        public DateTime Ahora { get; set; } = new DateTime(2026, 3, 10, 10, 0, 0);

        public UsuarioRepository Usuarios { get; } = new UsuarioRepository();
        public SocioRepository Socios { get; } = new SocioRepository();
        public CuotaRepository Cuotas { get; } = new CuotaRepository();
        public MedioPagoRepository Medios { get; } = new MedioPagoRepository();
        public MotivoEgresoRepository Motivos { get; } = new MotivoEgresoRepository();
        public GrupoEgresoRepository Grupos { get; } = new GrupoEgresoRepository();
        public MovimientoRepository Movimientos { get; } = new MovimientoRepository();
        public CajaRepository Cajas { get; } = new CajaRepository();
        public CajaTotalMedioPagoRepository Totales { get; } = new CajaTotalMedioPagoRepository();
        public DetalleIngresoCuotaRepository Detalles { get; } = new DetalleIngresoCuotaRepository();
        public ValorCuotaRepository Valores { get; } = new ValorCuotaRepository();
        public AuditoriaRepository Auditorias { get; } = new AuditoriaRepository();

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
            Func<DateTime> reloj = () => Ahora;

            Auditoria = new AuditoriaService(Auditorias, Usuarios, reloj);
            Auth = new AuthService(Usuarios, reloj);
            UsuariosServicio = new UsuarioService(Usuarios, Auditoria);
            SociosServicio = new SocioService(Socios, Auditoria);
            Catalogos = new CatalogoService(Medios, Motivos, Grupos, Auditoria);
            CajasServicio = new CajaService(Cajas, Movimientos, Totales, Auditoria, reloj);
            Registrador = new MovimientoRegistrador(Movimientos, Cajas, Totales, Auditoria, reloj);
            CuotasServicio = new CuotaService(Cuotas, Socios, Usuarios, Medios, Detalles, Valores, Registrador, Auditoria);
            Ingresos = new IngresoService(Usuarios, Medios, Registrador);
            Egresos = new EgresoService(Usuarios, Medios, Motivos, Grupos, Registrador);
            MovimientosServicio = new MovimientoService(Movimientos, Detalles, Cajas, Usuarios, Cuotas, Registrador, Auditoria);

            Operador = AgregarUsuario("operador", "operador123", Rol.Operador);
            Contador = AgregarUsuario("contador", "contador123", Rol.Contador);
            Efectivo = Agregar(Medios, new MedioPago(Medios.ObtenerProximoId(), "Efectivo"));
            Transferencia = Agregar(Medios, new MedioPago(Medios.ObtenerProximoId(), "Transferencia"));
            Materiales = Agregar(Motivos, new MotivoEgreso(Motivos.ObtenerProximoId(), "Materiales"));
            Estudiantes = Agregar(Grupos, new GrupoEgreso(Grupos.ObtenerProximoId(), "Estudiantes"));
            Administracion = Agregar(Grupos, new GrupoEgreso(Grupos.ObtenerProximoId(), "Administración"));
        }

        public Usuario AgregarUsuario(string nombreUsuario, string password, params Rol[] roles)
        {
            Usuario usuario = new Usuario(Usuarios.ObtenerProximoId(), nombreUsuario, "Nombre", "Apellido",
                PasswordHasher.Hashear(password), true, roles);
            return Agregar(Usuarios, usuario);
        }

        public Socio Socio(string dni = "30123456", Categoria categoria = Categoria.Socio, Socio? responsable = null,
            string apellido = "")
        {
            Socio socio = new Socio(Socios.ObtenerProximoId(), "Nombre", apellido.Length > 0 ? apellido : "Apellido" + dni, dni,
                "", "", categoria);
            socio.AsignarResponsable(responsable);
            return Agregar(Socios, socio);
        }

        public Cuota Cuota(Socio socio, decimal monto = 5000m, int anio = 2026)
        {
            return Agregar(Cuotas, new Cuota(Cuotas.ObtenerProximoId(), socio, monto, anio));
        }

        public void AbrirCaja(decimal montoInicial = 0m)
        {
            CajasServicio.Abrir(montoInicial, Contador.Id);
        }

        public Caja CajaAbierta()
        {
            return Cajas.ObtenerTodos().First(c => c.EstaAbierta);
        }

        public IngresoDTO Donacion(decimal monto)
        {
            IngresoDTO dto = new IngresoDTO();
            dto.Monto = monto;
            dto.CategoriaIngreso = CategoriaIngreso.Donacion;
            dto.MedioPagoId = Efectivo.Id;
            return Ingresos.Registrar(dto, Operador.Id);
        }

        public EgresoDTO Egreso(decimal monto, GrupoEgreso grupo, MedioPago? medio = null)
        {
            EgresoDTO dto = new EgresoDTO();
            dto.Monto = monto;
            dto.MotivoEgresoId = Materiales.Id;
            dto.GrupoEgresoId = grupo.Id;
            dto.MedioPagoId = (medio ?? Efectivo).Id;
            return Egresos.Registrar(dto, Contador.Id);
        }

        public SocioDTO DatosSocio(string dni, int id = 0, Categoria categoria = Categoria.Socio)
        {
            SocioDTO dto = new SocioDTO();
            dto.Id = id;
            dto.Nombre = "Nombre";
            dto.Apellido = "Apellido";
            dto.Dni = dni;
            dto.Categoria = categoria;
            return dto;
        }

        public static LoginDTO Login(string usuario, string password)
        {
            LoginDTO dto = new LoginDTO();
            dto.NombreUsuario = usuario;
            dto.Password = password;
            return dto;
        }

        private static T Agregar<T>(RepositorioBase<T> repositorio, T elemento) where T : class, IEntidad
        {
            repositorio.Agregar(elemento);
            return elemento;
        }
    }
}
