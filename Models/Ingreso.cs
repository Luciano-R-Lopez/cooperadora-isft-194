using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
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

        public override void AfectarCaja(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            ValidarDestino(caja, totalMedioPago);

            caja.Acreditar(Monto);
            totalMedioPago.Acreditar(Monto);
        }
    }
}
