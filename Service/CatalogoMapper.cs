using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Service
{
    public static class CatalogoMapper
    {
        public static MedioPagoDto ADto(MedioPago m) => new(m.Id, m.Nombre);

        public static MotivoEgresoDto ADto(MotivoEgreso m) => new(m.Id, m.Nombre);

        public static GrupoEgresoDto ADto(GrupoEgreso g) => new(g.Id, g.Nombre);
    }
}
