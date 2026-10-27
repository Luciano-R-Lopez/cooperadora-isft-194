using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    internal class DetalleIngresoCuotaPersistencia : IPersistencia<DetalleIngresoCuota>
    {
        public void Insertar(DetalleIngresoCuota detalle)
        {
            BaseDatos.Ejecutar(
                @"INSERT INTO detalle_ingreso_cuota (id_detalle_ingreso_cuota, id_ingreso, id_cuota, monto_aplicado, pagada_por)
                  VALUES (@id, @ingreso, @cuota, @monto, @pagadaPor)",
                ("@id", detalle.Id), ("@ingreso", detalle.Ingreso.Id), ("@cuota", detalle.Cuota.Id),
                ("@monto", detalle.Monto), ("@pagadaPor", detalle.PagadaPor?.Id));
        }

        public void Actualizar(DetalleIngresoCuota detalle)
        {
            throw new NotSupportedException("Los detalles de pago no se modifican.");
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los detalles de pago no se eliminan.");
        }
    }
}
