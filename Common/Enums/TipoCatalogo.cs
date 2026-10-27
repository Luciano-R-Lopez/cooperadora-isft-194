using System.ComponentModel;

namespace CooperadoraISFT94.Common.Enums
{
    public enum TipoCatalogo
    {
        [Description("Medios de pago")] MedioPago,
        [Description("Motivos de egreso")] MotivoEgreso,
        [Description("Grupos de egreso")] GrupoEgreso
    }
}
