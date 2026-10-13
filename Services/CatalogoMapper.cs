using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    public static class CatalogoMapper
    {
        public static MedioPagoDto ADto(MedioPago m) => new(m.Id, m.Nombre);

        public static MotivoEgresoDto ADto(MotivoEgreso m) => new(m.Id, m.Nombre);
    }
}
