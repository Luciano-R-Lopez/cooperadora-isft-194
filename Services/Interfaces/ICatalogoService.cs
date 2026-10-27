using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Services.Interfaces
{
    /// <summary>Listas de opciones configurables (medios de pago, motivos y grupos de egreso) y su mantenimiento.</summary>
    public interface ICatalogoService
    {
        /// <param name="incluirInactivos">True para filtros sobre datos históricos; false para cargar movimientos nuevos.</param>
        IReadOnlyList<MedioPagoDto> ObtenerMediosPago(bool incluirInactivos = false);

        IReadOnlyList<MotivoEgresoDto> ObtenerMotivosEgreso(bool incluirInactivos = false);

        IReadOnlyList<GrupoEgresoDto> ObtenerGruposEgreso(bool incluirInactivos = false);

        /// <summary>Todos los ítems del catálogo (activos e inactivos), por nombre.</summary>
        IReadOnlyList<ItemCatalogoDto> ObtenerItems(TipoCatalogo tipo);

        /// <exception cref="Common.Exceptions.ReglaNegocioException">Nombre inválido o repetido.</exception>
        ItemCatalogoDto Agregar(TipoCatalogo tipo, string nombre, int usuarioId);

        ItemCatalogoDto Renombrar(TipoCatalogo tipo, int id, string nombre, int usuarioId);

        ItemCatalogoDto CambiarEstado(TipoCatalogo tipo, int id, bool activo, int usuarioId);
    }
}
