using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Service
{
    public static class CajaMapper
    {
        public static CajaDto ADto(Caja c) =>
            new(c.Id, c.FechaApertura, c.FechaCierre, c.MontoInicial, c.MontoTotal, c.Estado);
    }
}
