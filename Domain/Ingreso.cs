using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Domain
{
    public class Ingreso : Movimiento
    {
        public Ingreso(Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago, CategoriaIngreso categoriaIngreso)
            : base(usuario, fecha, monto, medioPago)
        {
            if (!Enum.IsDefined(categoriaIngreso))
                throw new ReglaNegocioException("La categoría del ingreso no es válida.");

            CategoriaIngreso = categoriaIngreso;
        }

        public CategoriaIngreso CategoriaIngreso { get; }

        public override TipoMovimiento Tipo => TipoMovimiento.Ingreso;

        public override string Clasificacion => CategoriaIngreso.ToString();

        protected override void Aplicar(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            caja.Acreditar(Monto);
            totalMedioPago.Acreditar(Monto);
        }

        protected override void Revertir(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            caja.Debitar(Monto);
            totalMedioPago.Debitar(Monto);
        }
    }
}
