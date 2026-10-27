using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    public class Ingreso : Movimiento
    {
        public Ingreso(int id, Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago, CategoriaIngreso categoriaIngreso)
            : base(id, usuario, fecha, monto, medioPago)
        {
            if (!Enum.IsDefined(categoriaIngreso))
            {
                throw new NegocioException("La categoría del ingreso no es válida.");
            }
            CategoriaIngreso = categoriaIngreso;
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
