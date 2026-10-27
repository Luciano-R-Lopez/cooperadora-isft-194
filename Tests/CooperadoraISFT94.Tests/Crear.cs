using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Tests
{
    /// <summary>Entidades válidas de ejemplo para no repetir datos en cada test.</summary>
    internal static class Crear
    {
        public static Usuario Usuario(Rol rol = Rol.Operador) =>
            new("operador", "Omar", "Operador", "100.c2FsdA==.aGFzaA==", true, rol);

        public static Socio Socio(Categoria categoria = Categoria.Socio, string dni = "30123456") =>
            new("Lucía", "Fernández", dni, "", "", categoria);

        public static MedioPago Efectivo() => new("Efectivo");

        public static Ingreso Ingreso(decimal monto, MedioPago medio, CategoriaIngreso categoria = CategoriaIngreso.Donacion) =>
            new(Usuario(), DateTime.Now, monto, medio, categoria);

        public static Egreso Egreso(decimal monto, MedioPago medio) =>
            new(Usuario(Rol.Contador), DateTime.Now, monto, medio, new MotivoEgreso("Materiales"), new GrupoEgreso("Estudiantes"));
    }
}
