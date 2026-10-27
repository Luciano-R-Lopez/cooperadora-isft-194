using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    internal class ValorCuotaPersistencia : IPersistencia<ValorCuota>
    {
        public void Insertar(ValorCuota valor)
        {
            BaseDatos.Ejecutar("INSERT INTO valor_cuota (id_valor_cuota, anio, monto) VALUES (@id, @anio, @monto)",
                ("@id", valor.Id), ("@anio", valor.Anio), ("@monto", valor.Monto));
        }

        public void Actualizar(ValorCuota valor)
        {
            BaseDatos.Ejecutar("UPDATE valor_cuota SET monto = @monto WHERE id_valor_cuota = @id",
                ("@monto", valor.Monto), ("@id", valor.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los valores de cuota no se eliminan: se cambian.");
        }
    }
}
