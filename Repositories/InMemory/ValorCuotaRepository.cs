using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class ValorCuotaRepository : InMemoryRepository<ValorCuota>, IValorCuotaRepository
    {
        private static readonly Lazy<ValorCuotaRepository> _instancia = new(() => new ValorCuotaRepository());
        public static ValorCuotaRepository Instancia => _instancia.Value;

        internal ValorCuotaRepository() : base("El valor de cuota seleccionado no existe.") { }

        public ValorCuota? ObtenerPorAnio(int anio) => Items.FirstOrDefault(v => v.Anio == anio);
    }
}
