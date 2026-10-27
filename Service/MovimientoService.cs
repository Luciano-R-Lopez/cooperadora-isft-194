using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class MovimientoService
    {
        private const string SinDetalle = "-";

        private readonly IRepository<Movimiento> repositorio;
        private readonly IRepository<DetalleIngresoCuota> detalleRepositorio;
        private readonly IRepository<Caja> cajaRepositorio;
        private readonly IRepository<Usuario> usuarioRepositorio;
        private readonly IRepository<Cuota> cuotaRepositorio;
        private readonly MovimientoRegistrador registrador;
        private readonly AuditoriaService auditoria;

        public MovimientoService()
            : this(Repositorios.Movimientos, Repositorios.DetallesIngresoCuota, Repositorios.Cajas, Repositorios.Usuarios,
                Repositorios.Cuotas, new MovimientoRegistrador(), new AuditoriaService())
        {
        }

        public MovimientoService(IRepository<Movimiento> repositorio, IRepository<DetalleIngresoCuota> detalleRepositorio,
            IRepository<Caja> cajaRepositorio, IRepository<Usuario> usuarioRepositorio, IRepository<Cuota> cuotaRepositorio,
            MovimientoRegistrador registrador, AuditoriaService auditoria)
        {
            this.repositorio = repositorio;
            this.detalleRepositorio = detalleRepositorio;
            this.cajaRepositorio = cajaRepositorio;
            this.usuarioRepositorio = usuarioRepositorio;
            this.cuotaRepositorio = cuotaRepositorio;
            this.registrador = registrador;
            this.auditoria = auditoria;
        }

        // Ingresos y egresos de una caja (incluidos los anulados), del más reciente al más antiguo.
        public List<MovimientoDTO> Obtener(MovimientoFiltroDTO filtro)
        {
            if (filtro == null)
            {
                throw new ArgumentException("El filtro es obligatorio.");
            }
            Caja caja = cajaRepositorio.ObtenerExistente(filtro.CajaId);

            IEnumerable<Movimiento> movimientos = repositorio.ObtenerTodos().Where(m => ReferenceEquals(m.Caja, caja));
            if (filtro.Tipo.HasValue)
            {
                movimientos = movimientos.Where(m => m.Tipo == filtro.Tipo.Value);
            }
            if (filtro.MedioPagoId.HasValue)
            {
                movimientos = movimientos.Where(m => m.MedioPago.Id == filtro.MedioPagoId.Value);
            }
            if (filtro.GrupoEgresoId.HasValue)
            {
                movimientos = movimientos.Where(m => m is Egreso egreso && egreso.Grupo.Id == filtro.GrupoEgresoId.Value);
            }

            ILookup<int, DetalleIngresoCuota> cuotasPorIngreso = detalleRepositorio.ObtenerTodos().ToLookup(d => d.Ingreso.Id);
            return movimientos.OrderByDescending(m => m.Fecha)
                              .ThenByDescending(m => m.Id)
                              .Select(m => MapearADto(m, cuotasPorIngreso[m.Id]))
                              .ToList();
        }

        // Si el movimiento era un cobro de cuotas, esas cuotas vuelven a quedar pendientes.
        public MovimientoDTO Anular(int movimientoId, string motivo, int usuarioId)
        {
            Movimiento movimiento = repositorio.ObtenerExistente(movimientoId);
            Usuario usuario = usuarioRepositorio.ObtenerExistente(usuarioId);
            List<DetalleIngresoCuota> detalles = detalleRepositorio.ObtenerTodos()
                .Where(d => ReferenceEquals(d.Ingreso, movimiento))
                .ToList();

            // Primero la caja: si la anulación falla (por ejemplo, saldo insuficiente), las cuotas quedan como estaban.
            registrador.Anular(movimiento, usuario, motivo);

            foreach (Cuota cuota in detalles.Select(d => d.Cuota).Where(c => c.EstaPagada))
            {
                cuota.RevertirPago();
                cuotaRepositorio.Actualizar(cuota);
                auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Cuota), cuota.Id,
                    "Estado: Pagada → Pendiente (se anuló el ingreso)");
            }
            return MapearADto(movimiento, detalles);
        }

        // Egresos vigentes de todas las cajas entre dos fechas (ambas incluidas), ordenados por grupo y fecha.
        public List<MovimientoDTO> ObtenerEgresos(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
            {
                throw new NegocioException("La fecha \"desde\" no puede ser posterior a la fecha \"hasta\".");
            }
            return repositorio.ObtenerTodos()
                              .OfType<Egreso>()
                              .Where(e => !e.Anulado && e.Fecha.Date >= desde.Date && e.Fecha.Date <= hasta.Date)
                              .OrderBy(e => e.Grupo.Nombre)
                              .ThenBy(e => e.Fecha)
                              .Select(e => MapearADto(e, Enumerable.Empty<DetalleIngresoCuota>()))
                              .ToList();
        }

        private static MovimientoDTO MapearADto(Movimiento movimiento, IEnumerable<DetalleIngresoCuota> detalles)
        {
            MovimientoDTO dto = new MovimientoDTO();
            dto.Id = movimiento.Id;
            dto.Fecha = movimiento.Fecha;
            dto.Tipo = movimiento.Tipo;
            dto.Clasificacion = movimiento.Clasificacion;
            dto.Grupo = movimiento is Egreso egreso ? egreso.Grupo.Nombre : SinDetalle;
            dto.Detalle = DescribirCuotas(detalles);
            dto.MedioPago = movimiento.MedioPago.Nombre;
            dto.Monto = movimiento.Monto;
            dto.Usuario = movimiento.Usuario.NombreCompleto;
            dto.Anulado = movimiento.Anulado;
            dto.MotivoAnulacion = movimiento.MotivoAnulacion;
            return dto;
        }

        // "Cuota 2026 - Pérez, Sofía (40111222)", y quién pagó si fue su responsable.
        private static string DescribirCuotas(IEnumerable<DetalleIngresoCuota> detalles)
        {
            List<string> cuotas = new List<string>();
            foreach (DetalleIngresoCuota detalle in detalles)
            {
                string texto = "Cuota " + detalle.Cuota.Anio + " - " + detalle.Cuota.Socio.ObtenerDescripcion();
                if (detalle.PagadaPor != null)
                {
                    texto += " (pagó " + detalle.PagadaPor.Apellido + ", " + detalle.PagadaPor.Nombre + ")";
                }
                cuotas.Add(texto);
            }
            return cuotas.Count == 0 ? SinDetalle : string.Join("; ", cuotas);
        }
    }
}
