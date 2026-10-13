using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    public static class CuotaMapper
    {
        public static CuotaDto ADto(Cuota c) =>
            new(c.Id, c.Socio.Id, c.Anio, c.Monto, c.Estado);
    }
}
