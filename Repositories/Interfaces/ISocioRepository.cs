using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.Interfaces
{
    public interface ISocioRepository : IRepository<Socio>
    {
        Socio? ObtenerPorDni(string dni);

        /// <summary>Busca por DNI, nombre o apellido. Un filtro vacío devuelve todos.</summary>
        IReadOnlyList<Socio> Buscar(string? filtro);
    }
}
