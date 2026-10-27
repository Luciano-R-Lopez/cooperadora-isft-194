using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class CatalogoService : ICatalogoService
    {
        private static readonly Lazy<CatalogoService> _instancia = new(() =>
            new CatalogoService(MedioPagoRepository.Instancia, MotivoEgresoRepository.Instancia,
                                GrupoEgresoRepository.Instancia, AuditoriaService.Instancia));
        public static CatalogoService Instancia => _instancia.Value;

        private readonly IMedioPagoRepository _medios;
        private readonly IMotivoEgresoRepository _motivos;
        private readonly IGrupoEgresoRepository _grupos;
        private readonly IAuditoriaService _auditoria;

        internal CatalogoService(IMedioPagoRepository medios, IMotivoEgresoRepository motivos,
                                 IGrupoEgresoRepository grupos, IAuditoriaService auditoria)
        {
            _medios = medios;
            _motivos = motivos;
            _grupos = grupos;
            _auditoria = auditoria;
        }

        public IReadOnlyList<MedioPagoDto> ObtenerMediosPago(bool incluirInactivos = false) =>
            _medios.ObtenerTodos()
                   .Where(m => incluirInactivos || m.Activo)
                   .OrderBy(m => m.Nombre)
                   .Select(CatalogoMapper.ADto)
                   .ToList();

        public IReadOnlyList<MotivoEgresoDto> ObtenerMotivosEgreso(bool incluirInactivos = false) =>
            _motivos.ObtenerTodos()
                    .Where(m => incluirInactivos || m.Activo)
                    .OrderBy(m => m.Nombre)
                    .Select(CatalogoMapper.ADto)
                    .ToList();

        public IReadOnlyList<GrupoEgresoDto> ObtenerGruposEgreso(bool incluirInactivos = false) =>
            _grupos.ObtenerTodos()
                   .Where(g => incluirInactivos || g.Activo)
                   .OrderBy(g => g.Nombre)
                   .Select(CatalogoMapper.ADto)
                   .ToList();

        public IReadOnlyList<ItemCatalogoDto> ObtenerItems(TipoCatalogo tipo) =>
            Items(tipo).OrderBy(i => i.Nombre).Select(i => ADto(tipo, i)).ToList();

        public ItemCatalogoDto Agregar(TipoCatalogo tipo, string nombre, int usuarioId)
        {
            ItemCatalogo item = tipo switch
            {
                TipoCatalogo.MedioPago => new MedioPago(nombre),
                TipoCatalogo.MotivoEgreso => new MotivoEgreso(nombre),
                TipoCatalogo.GrupoEgreso => new GrupoEgreso(nombre),
                _ => throw new ReglaNegocioException("El catálogo no es válido.")
            };
            ExigirNombreLibre(tipo, item.Nombre, itemId: null);

            item = item switch
            {
                MedioPago medio => _medios.Agregar(medio),
                MotivoEgreso motivo => _motivos.Agregar(motivo),
                GrupoEgreso grupo => _grupos.Agregar(grupo),
                _ => throw new ReglaNegocioException("El catálogo no es válido.")
            };

            _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, item.GetType().Name, item.Id, item.Nombre);
            return ADto(tipo, item);
        }

        public ItemCatalogoDto Renombrar(TipoCatalogo tipo, int id, string nombre, int usuarioId)
        {
            var item = ObtenerExistente(tipo, id);
            var anterior = item.Nombre;

            item.Renombrar(nombre);
            try
            {
                ExigirNombreLibre(tipo, item.Nombre, item.Id);
            }
            catch (ReglaNegocioException)
            {
                item.Renombrar(anterior);
                throw;
            }

            Guardar(item);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, item.GetType().Name, item.Id,
                                 new DescripcionCambios().Campo("Nombre", anterior, item.Nombre).ToString());
            return ADto(tipo, item);
        }

        public ItemCatalogoDto CambiarEstado(TipoCatalogo tipo, int id, bool activo, int usuarioId)
        {
            var item = ObtenerExistente(tipo, id);

            if (activo)
                item.Activar();
            else
                item.Desactivar();

            Guardar(item);
            _auditoria.Registrar(usuarioId, activo ? AccionAuditoria.Modificacion : AccionAuditoria.Baja,
                                 item.GetType().Name, item.Id, activo ? "Activado" : "Desactivado");
            return ADto(tipo, item);
        }

        private IEnumerable<ItemCatalogo> Items(TipoCatalogo tipo) =>
            tipo switch
            {
                TipoCatalogo.MedioPago => _medios.ObtenerTodos(),
                TipoCatalogo.MotivoEgreso => _motivos.ObtenerTodos(),
                TipoCatalogo.GrupoEgreso => _grupos.ObtenerTodos(),
                _ => throw new ReglaNegocioException("El catálogo no es válido.")
            };

        private ItemCatalogo ObtenerExistente(TipoCatalogo tipo, int id) =>
            tipo switch
            {
                TipoCatalogo.MedioPago => _medios.ObtenerExistente(id),
                TipoCatalogo.MotivoEgreso => _motivos.ObtenerExistente(id),
                TipoCatalogo.GrupoEgreso => _grupos.ObtenerExistente(id),
                _ => throw new ReglaNegocioException("El catálogo no es válido.")
            };

        private void Guardar(ItemCatalogo item)
        {
            switch (item)
            {
                case MedioPago medio: _medios.Actualizar(medio); break;
                case MotivoEgreso motivo: _motivos.Actualizar(motivo); break;
                case GrupoEgreso grupo: _grupos.Actualizar(grupo); break;
            }
        }

        /// <summary>No puede haber dos ítems con el mismo nombre en un catálogo (sin distinguir mayúsculas).</summary>
        private void ExigirNombreLibre(TipoCatalogo tipo, string nombre, int? itemId)
        {
            if (Items(tipo).Any(i => i.Id != itemId && string.Equals(i.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
                throw new ReglaNegocioException($"Ya existe \"{nombre}\" en el catálogo.");
        }

        private static ItemCatalogoDto ADto(TipoCatalogo tipo, ItemCatalogo item) =>
            new(item.Id, tipo, item.Nombre, item.Activo);
    }
}
