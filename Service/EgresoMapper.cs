using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Service
{
    public static class EgresoMapper
    {
        public static EgresoDto ADto(Egreso e) =>
            new(e.Id, e.Fecha, e.Monto, e.Motivo.Nombre, e.Grupo.Nombre, e.MedioPago.Nombre);
    }
}
