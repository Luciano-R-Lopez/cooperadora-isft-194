using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    // Medios de pago, motivos y grupos de egreso: el administrador los agrega, renombra y desactiva.
    public class CatalogoService
    {
        private readonly IRepository<MedioPago> medioRepositorio;
        private readonly IRepository<MotivoEgreso> motivoRepositorio;
        private readonly IRepository<GrupoEgreso> grupoRepositorio;
        private readonly AuditoriaService auditoria;

        public CatalogoService()
            : this(Repositorios.MediosPago, Repositorios.MotivosEgreso, Repositorios.GruposEgreso, new AuditoriaService())
        {
        }

        public CatalogoService(IRepository<MedioPago> medioRepositorio, IRepository<MotivoEgreso> motivoRepositorio,
            IRepository<GrupoEgreso> grupoRepositorio, AuditoriaService auditoria)
        {
            this.medioRepositorio = medioRepositorio;
            this.motivoRepositorio = motivoRepositorio;
            this.grupoRepositorio = grupoRepositorio;
            this.auditoria = auditoria;
        }

        // incluirInactivos: true para filtrar datos ya registrados; false para cargar movimientos nuevos.
        public List<ItemCatalogoDTO> ObtenerItems(TipoCatalogo tipo, bool incluirInactivos)
        {
            return Items(tipo).Where(i => incluirInactivos || i.Activo)
                              .OrderBy(i => i.Nombre)
                              .Select(i => MapearADto(tipo, i))
                              .ToList();
        }

        // Agrega el ítem si dto.Id es 0; si no, lo renombra.
        public ItemCatalogoDTO Guardar(ItemCatalogoDTO dto, int usuarioId)
        {
            if (dto == null)
            {
                throw new ArgumentException("Los datos del ítem son obligatorios.");
            }
            return dto.Id == 0 ? Agregar(dto, usuarioId) : Renombrar(dto, usuarioId);
        }

        private ItemCatalogoDTO Agregar(ItemCatalogoDTO dto, int usuarioId)
        {
            ItemCatalogo nuevo;
            switch (dto.Tipo)
            {
                case TipoCatalogo.MedioPago:
                    nuevo = new MedioPago(medioRepositorio.ObtenerProximoId(), dto.Nombre);
                    break;
                case TipoCatalogo.MotivoEgreso:
                    nuevo = new MotivoEgreso(motivoRepositorio.ObtenerProximoId(), dto.Nombre);
                    break;
                case TipoCatalogo.GrupoEgreso:
                    nuevo = new GrupoEgreso(grupoRepositorio.ObtenerProximoId(), dto.Nombre);
                    break;
                default:
                    throw new NegocioException("El catálogo no es válido.");
            }
            ExigirNombreLibre(dto.Tipo, nuevo.Nombre, 0);

            switch (nuevo)
            {
                case MedioPago medio:
                    medioRepositorio.Agregar(medio);
                    break;
                case MotivoEgreso motivo:
                    motivoRepositorio.Agregar(motivo);
                    break;
                case GrupoEgreso grupo:
                    grupoRepositorio.Agregar(grupo);
                    break;
            }
            auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nuevo.GetType().Name, nuevo.Id, nuevo.Nombre);
            return MapearADto(dto.Tipo, nuevo);
        }

        private ItemCatalogoDTO Renombrar(ItemCatalogoDTO dto, int usuarioId)
        {
            ItemCatalogo item = ObtenerExistente(dto.Tipo, dto.Id);
            string anterior = item.Nombre;

            // El setter valida el nombre; si está repetido, el ítem vuelve a su nombre anterior.
            item.Nombre = dto.Nombre;
            try
            {
                ExigirNombreLibre(dto.Tipo, item.Nombre, item.Id);
            }
            catch (NegocioException)
            {
                item.Nombre = anterior;
                throw;
            }
            Guardar(item);
            auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, item.GetType().Name, item.Id,
                new DescripcionCambios().Campo("Nombre", anterior, item.Nombre).ToString());
            return MapearADto(dto.Tipo, item);
        }

        // Un ítem desactivado no se ofrece para movimientos nuevos; los ya registrados no cambian.
        public ItemCatalogoDTO CambiarEstado(TipoCatalogo tipo, int id, bool activo, int usuarioId)
        {
            ItemCatalogo item = ObtenerExistente(tipo, id);
            item.Activo = activo;
            Guardar(item);
            auditoria.Registrar(usuarioId, activo ? AccionAuditoria.Modificacion : AccionAuditoria.Baja,
                item.GetType().Name, item.Id, activo ? "Activado" : "Desactivado");
            return MapearADto(tipo, item);
        }

        private IEnumerable<ItemCatalogo> Items(TipoCatalogo tipo)
        {
            switch (tipo)
            {
                case TipoCatalogo.MedioPago:
                    return medioRepositorio.ObtenerTodos();
                case TipoCatalogo.MotivoEgreso:
                    return motivoRepositorio.ObtenerTodos();
                case TipoCatalogo.GrupoEgreso:
                    return grupoRepositorio.ObtenerTodos();
                default:
                    throw new NegocioException("El catálogo no es válido.");
            }
        }

        private ItemCatalogo ObtenerExistente(TipoCatalogo tipo, int id)
        {
            switch (tipo)
            {
                case TipoCatalogo.MedioPago:
                    return medioRepositorio.ObtenerExistente(id);
                case TipoCatalogo.MotivoEgreso:
                    return motivoRepositorio.ObtenerExistente(id);
                case TipoCatalogo.GrupoEgreso:
                    return grupoRepositorio.ObtenerExistente(id);
                default:
                    throw new NegocioException("El catálogo no es válido.");
            }
        }

        private void Guardar(ItemCatalogo item)
        {
            switch (item)
            {
                case MedioPago medio:
                    medioRepositorio.Actualizar(medio);
                    break;
                case MotivoEgreso motivo:
                    motivoRepositorio.Actualizar(motivo);
                    break;
                case GrupoEgreso grupo:
                    grupoRepositorio.Actualizar(grupo);
                    break;
            }
        }

        // No puede haber dos ítems con el mismo nombre en un catálogo (sin distinguir mayúsculas).
        private void ExigirNombreLibre(TipoCatalogo tipo, string nombre, int idAIgnorar)
        {
            if (Items(tipo).Any(i => i.Id != idAIgnorar && string.Equals(i.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
            {
                throw new NegocioException("Ya existe \"" + nombre + "\" en el catálogo.");
            }
        }

        private static ItemCatalogoDTO MapearADto(TipoCatalogo tipo, ItemCatalogo item)
        {
            ItemCatalogoDTO dto = new ItemCatalogoDTO();
            dto.Id = item.Id;
            dto.Tipo = tipo;
            dto.Nombre = item.Nombre;
            dto.Activo = item.Activo;
            return dto;
        }
    }
}
