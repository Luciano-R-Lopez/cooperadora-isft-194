using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class MovimientoRegistrador : IMovimientoRegistrador
    {
        private static readonly Lazy<MovimientoRegistrador> _instancia = new(() =>
            new MovimientoRegistrador(MovimientoRepository.Instancia, CajaRepository.Instancia,
                                      CajaTotalMedioPagoRepository.Instancia, AuditoriaService.Instancia));
        public static MovimientoRegistrador Instancia => _instancia.Value;

        private readonly IMovimientoRepository _movimientos;
        private readonly ICajaRepository _cajas;
        private readonly ICajaTotalMedioPagoRepository _totales;
        private readonly IAuditoriaService _auditoria;

        private readonly Func<DateTime> _reloj;

        internal MovimientoRegistrador(IMovimientoRepository movimientos, ICajaRepository cajas,
                                       ICajaTotalMedioPagoRepository totales, IAuditoriaService auditoria,
                                       Func<DateTime>? reloj = null)
        {
            _reloj = reloj ?? (() => DateTime.Now);
            _movimientos = movimientos;
            _cajas = cajas;
            _totales = totales;
            _auditoria = auditoria;
        }

        public void Registrar(Movimiento movimiento)
        {
            ArgumentNullException.ThrowIfNull(movimiento);

            var caja = _cajas.ObtenerActiva()
                ?? throw new ReglaNegocioException(Caja.MensajeSinCajaAbierta);

            var total = _totales.Obtener(caja, movimiento.MedioPago);
            var totalEsNuevo = total is null;
            total ??= new CajaTotalMedioPago(caja, movimiento.MedioPago);

            // El movimiento valida y modifica caja y total en memoria; recién después se persiste todo.
            movimiento.AfectarCaja(caja, total);

            _movimientos.Agregar(movimiento);
            if (totalEsNuevo)
                _totales.Agregar(total);
            else
                _totales.Actualizar(total);
            _cajas.Actualizar(caja);

            _auditoria.Registrar(movimiento.Usuario.Id, AccionAuditoria.Alta, movimiento.GetType().Name, movimiento.Id,
                                 $"{movimiento.Clasificacion} - {movimiento.MedioPago.Nombre} - {movimiento.Monto:C2}");
        }

        public void Anular(Movimiento movimiento, Usuario usuario, string motivo)
        {
            ArgumentNullException.ThrowIfNull(movimiento);

            var caja = movimiento.Caja
                ?? throw new ReglaNegocioException("El movimiento no se registró en ninguna caja.");
            var total = _totales.Obtener(caja, movimiento.MedioPago)
                ?? throw new NoEncontradoException("No se encontró el total por medio de pago del movimiento.");

            // El movimiento valida (caja abierta, no anulado, saldo) y revierte en memoria; después se persiste.
            movimiento.Anular(usuario, _reloj(), motivo, total);

            _movimientos.Actualizar(movimiento);
            _totales.Actualizar(total);
            _cajas.Actualizar(caja);

            _auditoria.Registrar(usuario.Id, AccionAuditoria.Baja, movimiento.GetType().Name, movimiento.Id,
                                 $"Anulado ({movimiento.Monto:C2}): {movimiento.MotivoAnulacion}");
        }
    }
}
