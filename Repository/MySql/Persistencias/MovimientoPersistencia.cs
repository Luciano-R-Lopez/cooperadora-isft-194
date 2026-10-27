using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    // Ingresos y egresos son tablas distintas, pero en la aplicación comparten la numeración (son Movimiento):
    // cada uno se guarda con su id y nunca hay un ingreso y un egreso con el mismo id.
    // concepto: la aplicación no lo pide, se guarda la categoría o el motivo.
    internal class MovimientoPersistencia : IPersistencia<Movimiento>
    {
        public void Insertar(Movimiento movimiento)
        {
            if (movimiento.Caja == null)
            {
                throw new InvalidOperationException("El movimiento se guarda después de aplicarlo a la caja.");
            }

            if (movimiento is Ingreso ingreso)
            {
                BaseDatos.Ejecutar(
                    @"INSERT INTO ingreso (id_ingreso, fecha_hora, importe, concepto, estado, id_medio_pago,
                                           id_categoria_ingreso, id_caja, registrado_por)
                      VALUES (@id, @fecha, @importe, @concepto, 'registrado', @medio,
                              (SELECT id_categoria_ingreso FROM categoria_ingreso WHERE nombre = @categoria), @caja, @usuario)",
                    ("@id", ingreso.Id), ("@fecha", ingreso.Fecha), ("@importe", ingreso.Monto),
                    ("@concepto", ingreso.Clasificacion), ("@medio", ingreso.MedioPago.Id),
                    ("@categoria", ingreso.CategoriaIngreso.ToString()), ("@caja", movimiento.Caja.Id),
                    ("@usuario", ingreso.Usuario.Id));
            }
            else if (movimiento is Egreso egreso)
            {
                BaseDatos.Ejecutar(
                    @"INSERT INTO egreso (id_egreso, fecha_hora, importe, concepto, estado, id_medio_pago,
                                          id_categoria_egreso, id_grupo_egreso, id_caja, registrado_por)
                      VALUES (@id, @fecha, @importe, @concepto, 'registrado', @medio, @motivo, @grupo, @caja, @usuario)",
                    ("@id", egreso.Id), ("@fecha", egreso.Fecha), ("@importe", egreso.Monto),
                    ("@concepto", egreso.Clasificacion), ("@medio", egreso.MedioPago.Id), ("@motivo", egreso.Motivo.Id),
                    ("@grupo", egreso.Grupo.Id), ("@caja", movimiento.Caja.Id), ("@usuario", egreso.Usuario.Id));
            }
        }

        // Lo único que cambia de un movimiento ya registrado es su anulación.
        public void Actualizar(Movimiento movimiento)
        {
            string tabla = movimiento is Ingreso ? "ingreso" : "egreso";
            BaseDatos.Ejecutar(
                "UPDATE " + tabla + @"
                 SET estado = @estado, motivo_anulacion = @motivo, fecha_anulacion = @fecha, anulado_por = @usuario
                 WHERE id_" + tabla + " = @id",
                ("@estado", movimiento.Anulado ? "anulado" : "registrado"),
                ("@motivo", BaseDatos.TextoONulo(movimiento.MotivoAnulacion)), ("@fecha", movimiento.FechaAnulacion),
                ("@usuario", movimiento.AnuladoPor?.Id), ("@id", movimiento.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los movimientos no se eliminan: se anulan.");
        }
    }
}
