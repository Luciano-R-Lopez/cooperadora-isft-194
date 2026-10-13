using CooperadoraISFT94.DTOs.Catalogos;

namespace CooperadoraISFT94.Services.Interfaces
{
    /// <summary>Listas de opciones configurables (medios de pago, motivos de egreso) para desplegables y filtros.</summary>
    public interface ICatalogoService
    {
        IReadOnlyList<MedioPagoDto> ObtenerMediosPago();
        IReadOnlyList<MotivoEgresoDto> ObtenerMotivosEgreso();
    }
}
