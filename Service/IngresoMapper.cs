using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Service
{
    public static class IngresoMapper
    {
        public static IngresoDto ADto(Ingreso i) =>
            new(i.Id, i.Fecha, i.Monto, i.CategoriaIngreso, i.MedioPago.Nombre);
    }
}
