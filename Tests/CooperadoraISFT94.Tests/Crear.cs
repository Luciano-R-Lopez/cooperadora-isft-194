using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Tests
{
    // Entidades válidas de ejemplo para no repetir datos en cada test. Cada una recibe un id distinto.
    internal static class Crear
    {
        private static int ultimoId;

        public static int Id()
        {
            return Interlocked.Increment(ref ultimoId);
        }

        public static Usuario Usuario(Rol rol = Rol.Operador)
        {
            return new Usuario(Id(), "operador", "Omar", "Operador", "100.c2FsdA==.aGFzaA==", true, rol);
        }

        public static Socio Socio(Categoria categoria = Categoria.Socio, string dni = "30123456")
        {
            return new Socio(Id(), "Lucía", "Fernández", dni, "", "", categoria);
        }

        public static MedioPago Efectivo()
        {
            return new MedioPago(Id(), "Efectivo");
        }

        public static Caja Caja(decimal montoInicial)
        {
            return new Caja(Id(), montoInicial, DateTime.Now);
        }

        public static CajaTotalMedioPago Total(Caja caja, MedioPago medio)
        {
            return new CajaTotalMedioPago(Id(), caja, medio);
        }

        public static Cuota Cuota(int anio = 2026, decimal monto = 5000m)
        {
            return new Cuota(Id(), Socio(), monto, anio);
        }

        public static Ingreso Ingreso(decimal monto, MedioPago medio, CategoriaIngreso categoria = CategoriaIngreso.Donacion)
        {
            return new Ingreso(Id(), Usuario(), DateTime.Now, monto, medio, categoria);
        }

        public static Egreso Egreso(decimal monto, MedioPago medio)
        {
            return new Egreso(Id(), Usuario(Rol.Contador), DateTime.Now, monto, medio, new MotivoEgreso(Id(), "Materiales"),
                new GrupoEgreso(Id(), "Estudiantes"));
        }
    }
}
