using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Datos
{
    // Lo mínimo para usar la aplicación mientras no hay base de datos: los catálogos y un usuario por rol.
    // Con base de datos esto lo haría un script de alta.
    internal static class DatosIniciales
    {
        public static void Cargar()
        {
            CargarCatalogos();
            CargarUsuarios();
        }

        private static void CargarCatalogos()
        {
            if (Repositorios.MediosPago.ObtenerTodos().Count == 0)
            {
                foreach (string nombre in new[] { "Efectivo", "Transferencia", "Tarjeta", "Cheque" })
                {
                    Repositorios.MediosPago.Agregar(new MedioPago(Repositorios.MediosPago.ObtenerProximoId(), nombre));
                }
            }

            if (Repositorios.MotivosEgreso.ObtenerTodos().Count == 0)
            {
                foreach (string nombre in new[] { "Materiales", "Servicios", "Mantenimiento", "Eventos", "Otro" })
                {
                    Repositorios.MotivosEgreso.Agregar(new MotivoEgreso(Repositorios.MotivosEgreso.ObtenerProximoId(), nombre));
                }
            }

            if (Repositorios.GruposEgreso.ObtenerTodos().Count == 0)
            {
                foreach (string nombre in new[] { "Estudiantes", "Administración", "Otros" })
                {
                    Repositorios.GruposEgreso.Agregar(new GrupoEgreso(Repositorios.GruposEgreso.ObtenerProximoId(), nombre));
                }
            }
        }

        private static void CargarUsuarios()
        {
            if (Repositorios.Usuarios.ObtenerTodos().Count > 0)
            {
                return;
            }
            AgregarUsuario("admin", "Ana", "Admin", "admin123", Rol.Administrador);
            AgregarUsuario("operador", "Omar", "Operador", "operador123", Rol.Operador);
            AgregarUsuario("contador", "Carla", "Contadora", "contador123", Rol.Contador);
        }

        private static void AgregarUsuario(string nombreUsuario, string nombre, string apellido, string password, Rol rol)
        {
            Usuario usuario = new Usuario(Repositorios.Usuarios.ObtenerProximoId(), nombreUsuario, nombre, apellido,
                PasswordHasher.Hashear(password), true, rol);
            Repositorios.Usuarios.Agregar(usuario);
        }
    }
}
