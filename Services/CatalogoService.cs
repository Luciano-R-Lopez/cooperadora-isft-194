using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class CatalogoService : ICatalogoService
    {
        private static readonly Lazy<CatalogoService> _instancia = new(() =>
            new CatalogoService(MedioPagoRepository.Instancia, MotivoEgresoRepository.Instancia));
        public static CatalogoService Instancia => _instancia.Value;

        private readonly IMedioPagoRepository _medios;
        private readonly IMotivoEgresoRepository _motivos;

        private CatalogoService(IMedioPagoRepository medios, IMotivoEgresoRepository motivos)
        {
            _medios = medios;
            _motivos = motivos;
        }

        public IReadOnlyList<MedioPagoDto> ObtenerMediosPago() =>
            _medios.ObtenerTodos().OrderBy(m => m.Nombre).Select(CatalogoMapper.ADto).ToList();

        public IReadOnlyList<MotivoEgresoDto> ObtenerMotivosEgreso() =>
            _motivos.ObtenerTodos().OrderBy(m => m.Nombre).Select(CatalogoMapper.ADto).ToList();
    }
}
