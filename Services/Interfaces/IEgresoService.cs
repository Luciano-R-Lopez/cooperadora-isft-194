using CooperadoraISFT94.DTOs.Egresos;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface IEgresoService
    {
        /// <summary>Registra un egreso en la caja abierta a nombre del usuario indicado.</summary>
        EgresoDto Registrar(EgresoCrearDto dto, int usuarioId);
    }
}
