using CooperadoraISFT94.DTOs.Cajas;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface ICajaService
    {
        /// <summary>La caja abierta, o null si no hay ninguna.</summary>
        CajaDto? ObtenerActiva();
        IReadOnlyList<CajaDto> ObtenerTodas();
        CajaDto Abrir(AbrirCajaDto dto, int usuarioId);

        /// <summary>Cierra la caja y devuelve el resumen final con la diferencia hallada.</summary>
        ResumenCajaDto Cerrar(CerrarCajaDto dto, int usuarioId);

        /// <summary>Resumen de la caja abierta. Si se informa el monto contado, calcula la diferencia.</summary>
        ResumenCajaDto ObtenerResumenActivo(decimal? montoDeclarado);
    }
}
