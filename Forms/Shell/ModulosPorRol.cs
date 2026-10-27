using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Forms.Admin;
using CooperadoraISFT94.Forms.Contador;
using CooperadoraISFT94.Forms.Operador;

namespace CooperadoraISFT94.Forms.Shell
{
    /// <summary>Un módulo de la ventana principal: el título de su pestaña y cómo construir su vista.</summary>
    public record Modulo(string Titulo, Func<Control> CrearVista);

    /// <summary>
    /// Registro de qué módulos ve cada rol. Para sumar un módulo (o dárselo a otro rol)
    /// se agrega una línea acá; la ventana principal no cambia. Un usuario con varios roles ve los módulos de todos.
    /// </summary>
    public static class ModulosPorRol
    {
        private static readonly IReadOnlyDictionary<Rol, IReadOnlyList<Modulo>> Registro =
            new Dictionary<Rol, IReadOnlyList<Modulo>>
            {
                [Rol.Administrador] = new[]
                {
                    new Modulo("Administración", () => new UsuariosView()),
                    new Modulo("Catálogos", () => new CatalogosView()),
                    new Modulo("Auditoría", () => new AuditoriaView())
                },
                [Rol.Operador] = new[]
                {
                    new Modulo("Socios y cuotas", () => new OperadorView()),
                    new Modulo("Cuotas y morosos", () => new CuotasView()),
                    new Modulo("Ingresos", () => new IngresoView())
                },
                [Rol.Contador] = new[]
                {
                    new Modulo("Caja y movimientos", () => new ContadorView())
                }
            };

        /// <summary>Los módulos de cada rol, en el orden de los roles (Administrador, Operador, Contador).</summary>
        public static IReadOnlyList<Modulo> De(IEnumerable<Rol> roles) =>
            roles.Distinct()
                 .Order()
                 .SelectMany(rol => Registro.TryGetValue(rol, out var modulos) ? modulos : Array.Empty<Modulo>())
                 .ToList();
    }
}
