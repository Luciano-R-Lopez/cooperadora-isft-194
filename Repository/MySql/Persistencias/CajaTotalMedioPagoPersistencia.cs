using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    // La tabla no tiene id propio (su clave es caja + medio de pago) y guarda ingresos y egresos por separado.
    // Los dos totales se recalculan desde las tablas ingreso y egreso (sin los anulados), así siempre
    // coinciden con los movimientos. Por eso el movimiento se guarda antes que su total.
    internal class CajaTotalMedioPagoPersistencia : IPersistencia<CajaTotalMedioPago>
    {
        private const string Recalcular =
            @"INSERT INTO caja_total_medio_pago (id_caja, id_medio_pago, total_ingresos, total_egresos)
              VALUES (@caja, @medio,
                      (SELECT COALESCE(SUM(importe), 0) FROM ingreso
                       WHERE id_caja = @caja AND id_medio_pago = @medio AND estado = 'registrado'),
                      (SELECT COALESCE(SUM(importe), 0) FROM egreso
                       WHERE id_caja = @caja AND id_medio_pago = @medio AND estado = 'registrado'))
              ON DUPLICATE KEY UPDATE total_ingresos = VALUES(total_ingresos), total_egresos = VALUES(total_egresos)";

        public void Insertar(CajaTotalMedioPago total)
        {
            Guardar(total);
        }

        public void Actualizar(CajaTotalMedioPago total)
        {
            Guardar(total);
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los totales por medio de pago no se eliminan.");
        }

        private static void Guardar(CajaTotalMedioPago total)
        {
            BaseDatos.Ejecutar(Recalcular, ("@caja", total.Caja.Id), ("@medio", total.MedioPago.Id));
        }
    }
}
