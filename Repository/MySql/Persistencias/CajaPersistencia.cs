using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    // Con la caja abierta, efectivo_esperado va guardando el MontoTotal. Al cerrarla, efectivo_declarado guarda
    // lo que se contó y diferencia_efectivo la diferencia con lo esperado.
    internal class CajaPersistencia : IPersistencia<Caja>
    {
        public void Insertar(Caja caja)
        {
            BaseDatos.Ejecutar(
                @"INSERT INTO caja (id_caja, fecha_apertura, efectivo_inicial, efectivo_esperado, estado, abierta_por)
                  VALUES (@id, @apertura, @inicial, @total, 'abierta', @usuario)",
                ("@id", caja.Id), ("@apertura", caja.FechaApertura), ("@inicial", caja.MontoInicial),
                ("@total", caja.MontoTotal), ("@usuario", BaseDatos.ExigirUsuarioDeLaSesion()));
        }

        public void Actualizar(Caja caja)
        {
            if (caja.EstaAbierta)
            {
                BaseDatos.Ejecutar("UPDATE caja SET efectivo_esperado = @total WHERE id_caja = @id",
                    ("@total", caja.MontoTotal), ("@id", caja.Id));
                return;
            }

            // Solo la primera vez que se guarda cerrada: después ya no cambia.
            BaseDatos.Ejecutar(
                @"UPDATE caja
                  SET estado = 'cerrada', fecha_cierre = @cierre, efectivo_declarado = @contado,
                      diferencia_efectivo = @contado - efectivo_esperado, cerrada_por = @usuario
                  WHERE id_caja = @id AND estado = 'abierta'",
                ("@cierre", caja.FechaCierre), ("@contado", caja.MontoTotal),
                ("@usuario", BaseDatos.ExigirUsuarioDeLaSesion()), ("@id", caja.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Las cajas no se eliminan.");
        }
    }
}
