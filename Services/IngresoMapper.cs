using CooperadoraISFT94.DTOs.Ingresos;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    public static class IngresoMapper
    {
        public static IngresoDto ADto(Ingreso i) =>
            new(i.Id, i.Fecha, i.Monto, i.CategoriaIngreso, i.MedioPago.Nombre);
    }
}
