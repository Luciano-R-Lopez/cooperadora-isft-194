using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Domain
{
    public class Ingreso : Movimiento
    {
        public Ingreso(int id, Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago, CategoriaIngreso categoriaIngreso)
            : base(id, usuario, fecha, monto, medioPago)
        {
            CategoriaIngreso = Validaciones.ExigirEnum(categoriaIngreso, "categoría del ingreso");
        }

        public CategoriaIngreso CategoriaIngreso { get; private set; }

        public override TipoMovimiento Tipo
        {
            get { return TipoMovimiento.Ingreso; }
        }

        public override string Clasificacion
        {
            get { return CategoriaIngreso.Texto(); }
        }

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
