using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.Interfaces
{
    public interface IValorCuotaRepository : IRepository<ValorCuota>
    {
        /// <summary>Valor fijado para ese año; null si no se fijó.</summary>
        ValorCuota? ObtenerPorAnio(int anio);
    }
}
