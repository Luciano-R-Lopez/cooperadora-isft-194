using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services.Interfaces
{
    /// <summary>
    /// Colaborador de los servicios que generan movimientos (ingresos manuales, cobro de cuotas, egresos):
    /// concentra en un solo lugar cómo un movimiento afecta a la caja abierta y queda guardado y auditado.
    /// </summary>
    public interface IMovimientoRegistrador
    {
        /// <summary>
        /// Aplica el movimiento a la caja abierta y a su total por medio de pago, y lo guarda.
        /// Si no hay caja abierta no se guarda nada.
        /// </summary>
        /// <exception cref="Common.Exceptions.ReglaNegocioException">No hay una caja abierta.</exception>
        void Registrar(Movimiento movimiento);

        /// <summary>
        /// Revierte el efecto del movimiento en la caja abierta, lo marca como anulado y lo audita.
        /// Si no se puede (caja cerrada, saldo insuficiente...) no se modifica nada.
        /// </summary>
        void Anular(Movimiento movimiento, Usuario usuario, string motivo);
    }
}
