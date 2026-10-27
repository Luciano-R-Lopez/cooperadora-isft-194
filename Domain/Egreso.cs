using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;

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
            Motivo = Validaciones.ExigirDato(motivo, "El motivo del egreso es obligatorio.");
            Grupo = Validaciones.ExigirDato(grupo, "El grupo del egreso es obligatorio.");
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
