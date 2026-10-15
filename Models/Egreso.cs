using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    public class Egreso : Movimiento
    {
        public Egreso(Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago, MotivoEgreso motivo)
            : base(usuario, fecha, monto, medioPago)
        {
            Motivo = motivo ?? throw new ReglaNegocioException("El motivo del egreso es obligatorio.");
        }

        public MotivoEgreso Motivo { get; }

        public override TipoMovimiento Tipo => TipoMovimiento.Egreso;

        public override string Clasificacion => Motivo.Nombre;

        protected override void Aplicar(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            caja.Debitar(Monto);
            totalMedioPago.Debitar(Monto);
        }
    }
}
