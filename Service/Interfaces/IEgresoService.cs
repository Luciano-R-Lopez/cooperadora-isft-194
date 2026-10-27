using CooperadoraISFT94.Dto;

namespace CooperadoraISFT94.Service.Interfaces
{
    public interface IEgresoService
    {
        /// <summary>Registra un egreso en la caja abierta a nombre del usuario indicado.</summary>
        EgresoDto Registrar(EgresoCrearDto dto, int usuarioId);
    }
}
