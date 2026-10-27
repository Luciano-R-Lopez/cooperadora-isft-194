using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Service
{
    public static class CuotaMapper
    {
        public static CuotaDto ADto(Cuota c) =>
            new(c.Id, c.Socio.Id, c.Anio, c.Monto, c.Estado);
    }
}
