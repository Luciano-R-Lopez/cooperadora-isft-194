using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    public static class CajaMapper
    {
        public static CajaDto ADto(Caja c) =>
            new(c.Id, c.FechaApertura, c.FechaCierre, c.MontoInicial, c.MontoTotal, c.Estado);
    }
}
