using CooperadoraISFT94.DTOs.Cuotas;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface ICuotaService
    {
        IReadOnlyList<CuotaDto> ObtenerPorSocio(int socioId);

        /// <summary>Genera la cuota anual del socio. Un socio tiene una sola cuota por año.</summary>
        CuotaDto Generar(CuotaCrearDto dto, int usuarioId);

        /// <summary>
        /// Registra el ingreso de cuota social (con su detalle) en la caja abierta y marca la cuota como pagada.
        /// Si no hay caja abierta, no se registra nada.
        /// </summary>
        CuotaDto RegistrarPago(int cuotaId, int medioPagoId, int usuarioId);
    }
}
