using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    // Lo comparten los servicios que registran o anulan movimientos (ingresos, egresos y cobro de cuotas):
    // aplica el movimiento a la caja abierta y a su total por medio de pago, y lo audita.
    public class MovimientoRegistrador
    {
        private readonly IRepository<Movimiento> repositorio;
        private readonly IRepository<Caja> cajaRepositorio;
        private readonly IRepository<CajaTotalMedioPago> totalRepositorio;
        private readonly AuditoriaService auditoria;
        private readonly Func<DateTime> reloj;

        public MovimientoRegistrador()
            : this(Repositorios.Movimientos, Repositorios.Cajas, Repositorios.TotalesMedioPago, new AuditoriaService())
        {
        }

        public MovimientoRegistrador(IRepository<Movimiento> repositorio, IRepository<Caja> cajaRepositorio,
            IRepository<CajaTotalMedioPago> totalRepositorio, AuditoriaService auditoria, Func<DateTime>? reloj = null)
        {
            this.repositorio = repositorio;
            this.cajaRepositorio = cajaRepositorio;
            this.totalRepositorio = totalRepositorio;
            this.auditoria = auditoria;
            this.reloj = reloj ?? (() => DateTime.Now);
        }

        public int ObtenerProximoId()
        {
            return repositorio.ObtenerProximoId();
        }

        public DateTime Ahora()
        {
            return reloj();
        }

        public void Registrar(Movimiento movimiento)
        {
            Caja? caja = cajaRepositorio.ObtenerTodos().FirstOrDefault(c => c.EstaAbierta);
            if (caja == null)
            {
                throw new NegocioException(Caja.MensajeSinCajaAbierta);
            }

            CajaTotalMedioPago? total = BuscarTotal(caja, movimiento.MedioPago);
            bool totalEsNuevo = total == null;
            if (total == null)
            {
                total = new CajaTotalMedioPago(totalRepositorio.ObtenerProximoId(), caja, movimiento.MedioPago);
            }

            // El movimiento valida y modifica caja y total; recién después se guarda todo.
            movimiento.AfectarCaja(caja, total);
            repositorio.Agregar(movimiento);
            if (totalEsNuevo)
            {
                totalRepositorio.Agregar(total);
            }
            else
            {
                totalRepositorio.Actualizar(total);
            }
            cajaRepositorio.Actualizar(caja);

            string detalle = movimiento.Clasificacion + " - ";
            if (movimiento is Egreso egreso)
            {
                detalle += egreso.Grupo.Nombre + " - ";
            }
            detalle += movimiento.MedioPago.Nombre + " - " + Formato.Monto(movimiento.Monto);
            auditoria.Registrar(movimiento.Usuario.Id, AccionAuditoria.Alta, movimiento.GetType().Name, movimiento.Id, detalle);
        }

        public void Anular(Movimiento movimiento, Usuario usuario, string motivo)
        {
            if (movimiento.Caja == null)
            {
                throw new NegocioException("El movimiento no se registró en ninguna caja.");
            }
            CajaTotalMedioPago? total = BuscarTotal(movimiento.Caja, movimiento.MedioPago);
            if (total == null)
            {
                throw new NoEncontradoException("No se encontró el total por medio de pago del movimiento.");
            }

            // El movimiento valida (caja abierta, no anulado, saldo) y revierte; después se guarda.
            movimiento.Anular(usuario, reloj(), motivo, total);
            repositorio.Actualizar(movimiento);
            totalRepositorio.Actualizar(total);
            cajaRepositorio.Actualizar(movimiento.Caja);

            auditoria.Registrar(usuario.Id, AccionAuditoria.Baja, movimiento.GetType().Name, movimiento.Id,
                "Anulado (" + Formato.Monto(movimiento.Monto) + "): " + movimiento.MotivoAnulacion);
        }

        private CajaTotalMedioPago? BuscarTotal(Caja caja, MedioPago medio)
        {
            return totalRepositorio.ObtenerTodos().FirstOrDefault(t =>
                ReferenceEquals(t.Caja, caja) && ReferenceEquals(t.MedioPago, medio));
        }
    }
}
