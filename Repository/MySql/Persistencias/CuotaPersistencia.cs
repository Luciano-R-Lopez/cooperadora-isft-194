using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    // En la base la cuota tiene período y vencimiento; en la aplicación solo el año.
    // Se guarda como período 1/1/año y vencimiento 31/12/año.
    internal class CuotaPersistencia : IPersistencia<Cuota>
    {
        public void Insertar(Cuota cuota)
        {
            BaseDatos.Ejecutar(
                @"INSERT INTO cuota (id_cuota, id_socio, periodo, importe, fecha_vencimiento, estado)
                  VALUES (@id, @socio, @periodo, @importe, @vencimiento, @estado)",
                ("@id", cuota.Id), ("@socio", cuota.Socio.Id), ("@periodo", new DateTime(cuota.Anio, 1, 1)),
                ("@importe", cuota.Monto), ("@vencimiento", new DateTime(cuota.Anio, 12, 31)),
                ("@estado", BaseDatos.Nombre(cuota.Estado)));
        }

        // Lo único que cambia de una cuota es si está pagada.
        public void Actualizar(Cuota cuota)
        {
            BaseDatos.Ejecutar("UPDATE cuota SET estado = @estado WHERE id_cuota = @id",
                ("@estado", BaseDatos.Nombre(cuota.Estado)), ("@id", cuota.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Las cuotas no se eliminan.");
        }
    }
}
