using CooperadoraISFT94.Common;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.InMemory
{
    /// <summary>
    /// Datos de prueba mientras no haya base de datos. Se elimina al migrar a MySQL
    /// (el primer admin y los catálogos se crearían con un script de alta).
    /// </summary>
    public static class DatosIniciales
    {
        public static void Cargar()
        {
            CargarCatalogos();
            CargarUsuarios();
            CargarSociosYCuotas();
        }

        private static void CargarCatalogos()
        {
            var medios = MedioPagoRepository.Instancia;
            if (medios.ObtenerTodos().Count == 0)
            {
                foreach (var nombre in new[] { "Efectivo", "Transferencia", "Tarjeta", "Cheque" })
                    medios.Agregar(new MedioPago(nombre));
            }

            var motivos = MotivoEgresoRepository.Instancia;
            if (motivos.ObtenerTodos().Count == 0)
            {
                foreach (var nombre in new[] { "Materiales", "Servicios", "Mantenimiento", "Eventos", "Otro" })
                    motivos.Agregar(new MotivoEgreso(nombre));
            }
        }

        private static void CargarUsuarios()
        {
            var usuarios = UsuarioRepository.Instancia;
            if (usuarios.ObtenerTodos().Count > 0) return;

            usuarios.Agregar(CrearUsuario("admin", "Ana", "Admin", "admin123", Rol.Administrador));
            usuarios.Agregar(CrearUsuario("operador", "Omar", "Operador", "operador123", Rol.Operador));
            usuarios.Agregar(CrearUsuario("contador", "Carla", "Contadora", "contador123", Rol.Contador));
        }

        private static void CargarSociosYCuotas()
        {
            var socios = SocioRepository.Instancia;
            var cuotas = CuotaRepository.Instancia;
            if (socios.ObtenerTodos().Count > 0) return;

            var lucia = socios.Agregar(new Socio("Lucía", "Fernández", "30123456", "1155550101", "lucia@mail.com", Categoria.Socio));
            var martin = socios.Agregar(new Socio("Martín", "Gómez", "28456789", "1155550202", "martin@mail.com", Categoria.Responsable));
            var sofia = new Socio("Sofía", "Pérez", "40111222", "", "", Categoria.Socio);
            sofia.AsignarResponsable(martin);
            socios.Agregar(sofia);

            // Las cuotas nacen pendientes: pasan a pagadas cuando se cobran (con su ingreso en caja).
            cuotas.Agregar(new Cuota(lucia, 5000m, 2025));
            cuotas.Agregar(new Cuota(lucia, 5000m, 2026));
            cuotas.Agregar(new Cuota(martin, 5000m, 2026));

            // Valor de la cuota del año: se propone al generar cuotas nuevas.
            ValorCuotaRepository.Instancia.Agregar(new ValorCuota(2026, 5000m));
        }

        private static Usuario CrearUsuario(string nombreUsuario, string nombre, string apellido, string password, Rol rol) =>
            new(nombreUsuario, nombre, apellido, PasswordHasher.Hashear(password), true, rol);
    }
}
