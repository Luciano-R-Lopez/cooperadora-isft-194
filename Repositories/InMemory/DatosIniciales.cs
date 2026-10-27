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
            CargarDatosDeEjemplo();
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

            var grupos = GrupoEgresoRepository.Instancia;
            if (grupos.ObtenerTodos().Count == 0)
            {
                foreach (var nombre in new[] { "Estudiantes", "Administración", "Otros" })
                    grupos.Agregar(new GrupoEgreso(nombre));
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

        /// <summary>Socios, cuotas, cajas y movimientos simulados (ver <see cref="DatosDeEjemplo"/>).</summary>
        private static void CargarDatosDeEjemplo()
        {
            if (SocioRepository.Instancia.ObtenerTodos().Count > 0) return;

            var usuarios = UsuarioRepository.Instancia;
            DatosDeEjemplo.Cargar(
                usuarios.ObtenerPorNombreUsuario("admin")!.Id,
                usuarios.ObtenerPorNombreUsuario("operador")!.Id,
                usuarios.ObtenerPorNombreUsuario("contador")!.Id);
        }

        private static Usuario CrearUsuario(string nombreUsuario, string nombre, string apellido, string password, Rol rol) =>
            new(nombreUsuario, nombre, apellido, PasswordHasher.Hashear(password), true, rol);
    }
}
