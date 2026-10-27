using CooperadoraISFT94.Dto;

namespace CooperadoraISFT94.Service.Interfaces
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

        /// <summary>Cobra una cuota: igual que <see cref="RegistrarPagos"/> con una sola.</summary>
        ReciboPagoDto RegistrarPago(int cuotaId, int medioPagoId, int usuarioId, int? pagadorId = null);

        /// <summary>
        /// Cobra una o varias cuotas juntas: registra un solo ingreso de cuota social por el total (con un detalle
        /// por cuota) en la caja abierta y las marca como pagadas. Sin <paramref name="pagadorId"/>, las cuotas tienen
        /// que ser del mismo socio; con él, de socios a cargo de ese responsable (o del propio responsable).
        /// Si algo falla (por ejemplo, no hay caja abierta) no se registra nada.
        /// </summary>
        ReciboPagoDto RegistrarPagos(IReadOnlyCollection<int> cuotaIds, int medioPagoId, int usuarioId, int? pagadorId = null);

        /// <summary>Cuotas pendientes (de un año, o de todos con null) con los datos de contacto del socio.</summary>
        IReadOnlyList<CuotaPendienteDto> ObtenerPendientes(int? anio);

        /// <summary>
        /// Socios con cuotas pendientes, uno por fila con sus años y el total. Las familias quedan juntas
        /// (ordenadas por quien paga: el responsable o el propio socio).
        /// </summary>
        IReadOnlyList<DeudorDto> ObtenerDeudores(DeudoresFiltroDto filtro);
    }
}
