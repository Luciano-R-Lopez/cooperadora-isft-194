using CooperadoraISFT94.Dto;

namespace CooperadoraISFT94.Service.Interfaces
{
    public interface IIngresoService
    {
        /// <summary>Registra un ingreso en la caja abierta a nombre del usuario indicado.</summary>
        IngresoDto Registrar(IngresoCrearDto dto, int usuarioId);
    }
}
