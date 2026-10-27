using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository
{
    public interface IValorCuotaRepository : IRepository<ValorCuota>
    {
        /// <summary>Valor fijado para ese año; null si no se fijó.</summary>
        ValorCuota? ObtenerPorAnio(int anio);
    }
}
