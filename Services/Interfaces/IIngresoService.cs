using CooperadoraISFT94.DTOs.Ingresos;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface IIngresoService
    {
        /// <summary>Registra un ingreso en la caja abierta a nombre del usuario indicado.</summary>
        IngresoDto Registrar(IngresoCrearDto dto, int usuarioId);
    }
}
