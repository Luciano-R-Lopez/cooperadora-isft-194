using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.Interfaces
{
    public interface ICuotaRepository : IRepository<Cuota>
    {
        /// <summary>Cuotas de un socio, de la más reciente a la más antigua.</summary>
        IReadOnlyList<Cuota> ObtenerPorSocio(Socio socio);

        /// <summary>True si el socio ya tiene una cuota para ese año.</summary>
        bool ExisteParaAnio(Socio socio, int anio);

        /// <summary>Última cuota generada para ese año (de cualquier socio); null si no hay ninguna.</summary>
        Cuota? ObtenerUltimaDelAnio(int anio);

        /// <summary>Cuotas pendientes (de un año, o de todos con null), de la más vieja a la más nueva.</summary>
        IReadOnlyList<Cuota> ObtenerPendientes(int? anio);
    }
}
