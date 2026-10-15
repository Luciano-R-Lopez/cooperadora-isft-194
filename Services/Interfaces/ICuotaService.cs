using CooperadoraISFT94.DTOs.Cuotas;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface ICuotaService
    {
        IReadOnlyList<CuotaDto> ObtenerPorSocio(int socioId);

        /// <summary>Genera la cuota anual de un socio activo. Un socio tiene una sola cuota por año.</summary>
        CuotaDto Generar(CuotaCrearDto dto, int usuarioId);

        /// <summary>
        /// Fija el valor de la cuota del año y genera la cuota a cada socio activo que todavía no la tenga.
        /// </summary>
        GeneracionCuotasDto GenerarParaTodos(int anio, decimal monto, int usuarioId);

        /// <summary>Valor fijado para el año o, si no hay, el de la última cuota generada ese año; null si no hay ninguno.</summary>
        decimal? ObtenerMontoSugerido(int anio);

        /// <summary>Fija (o cambia) el valor de la cuota del año para las cuotas que se generen de ahora en más.</summary>
        void FijarValorCuota(int anio, decimal monto, int usuarioId);

        /// <summary>
        /// Registra el ingreso de cuota social (con su detalle) en la caja abierta y marca la cuota como pagada.
        /// Si paga el responsable del socio, se indica en <paramref name="pagadorId"/> y queda registrado.
        /// Si no hay caja abierta, no se registra nada.
        /// </summary>
        ReciboPagoDto RegistrarPago(int cuotaId, int medioPagoId, int usuarioId, int? pagadorId = null);

        /// <summary>Cuotas pendientes (de un año, o de todos con null) con los datos de contacto del socio.</summary>
        IReadOnlyList<CuotaPendienteDto> ObtenerPendientes(int? anio);
    }
}
