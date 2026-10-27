using CooperadoraISFT94.DTOs.Egresos;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    public static class EgresoMapper
    {
        public static EgresoDto ADto(Egreso e) =>
            new(e.Id, e.Fecha, e.Monto, e.Motivo.Nombre, e.Grupo.Nombre, e.MedioPago.Nombre);
    }
}
