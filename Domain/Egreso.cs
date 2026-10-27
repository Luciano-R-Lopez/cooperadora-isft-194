using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Domain
{
    public class Egreso : Movimiento
    {
        public Egreso(Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago, MotivoEgreso motivo,
                      GrupoEgreso grupo)
            : base(usuario, fecha, monto, medioPago)
        {
            Motivo = motivo ?? throw new ReglaNegocioException("El motivo del egreso es obligatorio.");
            Grupo = grupo ?? throw new ReglaNegocioException("El grupo del egreso es obligatorio.");
        }

        public MotivoEgreso Motivo { get; }

        /// <summary>A quién corresponde el gasto. Se elige aparte del motivo: un mismo motivo puede ser de distintos grupos.</summary>
        public GrupoEgreso Grupo { get; }

        public override TipoMovimiento Tipo => TipoMovimiento.Egreso;

        public override string Clasificacion => Motivo.Nombre;

        protected override void Aplicar(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            caja.Debitar(Monto);
            totalMedioPago.Debitar(Monto);
        }

        protected override void Revertir(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            caja.Acreditar(Monto);
            totalMedioPago.Acreditar(Monto);
        }
    }
}
