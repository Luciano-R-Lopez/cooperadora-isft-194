using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    public class Egreso : Movimiento
    {
        // El grupo se elige aparte del motivo: un mismo motivo (por ejemplo, un arreglo de computadora)
        // puede ser un gasto de los estudiantes o de la administración.
        public Egreso(int id, Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago, MotivoEgreso motivo,
            GrupoEgreso grupo)
            : base(id, usuario, fecha, monto, medioPago)
        {
            if (motivo == null)
            {
                throw new NegocioException("El motivo del egreso es obligatorio.");
            }
            if (grupo == null)
            {
                throw new NegocioException("El grupo del egreso es obligatorio.");
            }
            Motivo = motivo;
            Grupo = grupo;
        }

        public MotivoEgreso Motivo { get; private set; }

        public GrupoEgreso Grupo { get; private set; }

        public override TipoMovimiento Tipo
        {
            get { return TipoMovimiento.Egreso; }
        }

        public override string Clasificacion
        {
            get { return Motivo.Nombre; }
        }

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
