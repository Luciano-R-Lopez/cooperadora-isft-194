using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class CajaService
    {
        private readonly IRepository<Caja> repositorio;
        private readonly IRepository<Movimiento> movimientoRepositorio;
        private readonly IRepository<CajaTotalMedioPago> totalRepositorio;
        private readonly AuditoriaService auditoria;
        private readonly Func<DateTime> reloj;

        public CajaService()
            : this(Repositorios.Cajas, Repositorios.Movimientos, Repositorios.TotalesMedioPago, new AuditoriaService())
        {
        }

        public CajaService(IRepository<Caja> repositorio, IRepository<Movimiento> movimientoRepositorio,
            IRepository<CajaTotalMedioPago> totalRepositorio, AuditoriaService auditoria, Func<DateTime>? reloj = null)
        {
            this.repositorio = repositorio;
            this.movimientoRepositorio = movimientoRepositorio;
            this.totalRepositorio = totalRepositorio;
            this.auditoria = auditoria;
            this.reloj = reloj ?? (() => DateTime.Now);
        }

        public CajaDTO? ObtenerActiva()
        {
            Caja? caja = BuscarAbierta();
            return caja == null ? null : MapearADto(caja);
        }

        // De la más reciente a la más antigua.
        public List<CajaDTO> ObtenerTodas()
        {
            return repositorio.ObtenerTodos()
                              .OrderByDescending(c => c.FechaApertura)
                              .Select(MapearADto)
                              .ToList();
        }

        public CajaDTO Abrir(decimal montoInicial, int usuarioId)
        {
            if (BuscarAbierta() != null)
            {
                throw new NegocioException("Ya hay una caja abierta.");
            }

            Caja caja = new Caja(repositorio.ObtenerProximoId(), montoInicial, reloj());
            repositorio.Agregar(caja);
            auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Caja), caja.Id,
                "Apertura con " + Formato.Monto(caja.MontoInicial));
            return MapearADto(caja);
        }

        // montoContado: el dinero que se contó al cerrar.
        public ResumenCajaDTO Cerrar(decimal montoContado, int usuarioId)
        {
            Caja caja = ObtenerAbierta();
            caja.Cerrar(reloj(), montoContado);
            repositorio.Actualizar(caja);

            ResumenCajaDTO resumen = CalcularResumen(caja, montoContado);
            auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Caja), caja.Id,
                "Cierre: esperado " + Formato.Monto(resumen.MontoEsperado) + ", contado " + Formato.Monto(montoContado)
                + ", diferencia " + Formato.Monto(resumen.Diferencia ?? 0));
            return resumen;
        }

        // Resumen de la caja abierta; montoDeclarado es lo que se está contando (null si todavía no se informó).
        public ResumenCajaDTO ObtenerResumenActivo(decimal? montoDeclarado)
        {
            return CalcularResumen(ObtenerAbierta(), montoDeclarado);
        }

        public ResumenCajaDTO ObtenerResumen(int cajaId)
        {
            Caja caja = repositorio.ObtenerExistente(cajaId);
            return CalcularResumen(caja, caja.EstaAbierta ? null : caja.MontoTotal);
        }

        private Caja? BuscarAbierta()
        {
            return repositorio.ObtenerTodos().FirstOrDefault(c => c.EstaAbierta);
        }

        private Caja ObtenerAbierta()
        {
            Caja? caja = BuscarAbierta();
            if (caja == null)
            {
                throw new NoEncontradoException("No hay una caja abierta.");
            }
            return caja;
        }

        // Diferencia = monto declarado - monto esperado (inicial + saldo neto de cada medio de pago).
        private ResumenCajaDTO CalcularResumen(Caja caja, decimal? montoDeclarado)
        {
            List<Movimiento> movimientos = movimientoRepositorio.ObtenerTodos().Where(m => ReferenceEquals(m.Caja, caja)).ToList();
            List<Movimiento> vigentes = movimientos.Where(m => !m.Anulado).ToList();

            // Los totales por medio ya tienen revertidos los movimientos anulados.
            List<CajaTotalMedioPago> totales = totalRepositorio.ObtenerTodos().Where(t => ReferenceEquals(t.Caja, caja)).ToList();

            ResumenCajaDTO resumen = new ResumenCajaDTO();
            resumen.Caja = MapearADto(caja);
            resumen.MontoInicial = caja.MontoInicial;
            resumen.TotalIngresos = vigentes.Where(m => m.Tipo == TipoMovimiento.Ingreso).Sum(m => m.Monto);
            resumen.TotalEgresos = vigentes.Where(m => m.Tipo == TipoMovimiento.Egreso).Sum(m => m.Monto);
            resumen.MontoEsperado = caja.MontoInicial + totales.Sum(t => t.Monto);
            resumen.MontoDeclarado = montoDeclarado;
            resumen.Diferencia = montoDeclarado.HasValue ? montoDeclarado.Value - resumen.MontoEsperado : null;
            resumen.CantidadMovimientos = vigentes.Count;
            resumen.CantidadAnulados = movimientos.Count - vigentes.Count;
            foreach (CajaTotalMedioPago total in totales.OrderBy(t => t.MedioPago.Nombre))
            {
                TotalMedioPagoDTO fila = new TotalMedioPagoDTO();
                fila.MedioPago = total.MedioPago.Nombre;
                fila.Monto = total.Monto;
                resumen.TotalesPorMedio.Add(fila);
            }
            return resumen;
        }

        internal static CajaDTO MapearADto(Caja caja)
        {
            CajaDTO dto = new CajaDTO();
            dto.Id = caja.Id;
            dto.FechaApertura = caja.FechaApertura;
            dto.FechaCierre = caja.FechaCierre;
            dto.MontoInicial = caja.MontoInicial;
            dto.MontoTotal = caja.MontoTotal;
            dto.Estado = caja.Estado;
            return dto;
        }
    }
}
