using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    // Medios de pago, motivos y grupos de egreso: nombre y si está activo.
    internal class MedioPagoPersistencia : IPersistencia<MedioPago>
    {
        public void Insertar(MedioPago medio)
        {
            BaseDatos.Ejecutar("INSERT INTO medio_pago (id_medio_pago, nombre, activo) VALUES (@id, @nombre, @activo)",
                ("@id", medio.Id), ("@nombre", medio.Nombre), ("@activo", medio.Activo));
        }

        public void Actualizar(MedioPago medio)
        {
            BaseDatos.Ejecutar("UPDATE medio_pago SET nombre = @nombre, activo = @activo WHERE id_medio_pago = @id",
                ("@nombre", medio.Nombre), ("@activo", medio.Activo), ("@id", medio.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los medios de pago no se eliminan: se desactivan.");
        }
    }

    // El motivo de egreso de la aplicación es la tabla categoria_egreso.
    internal class MotivoEgresoPersistencia : IPersistencia<MotivoEgreso>
    {
        public void Insertar(MotivoEgreso motivo)
        {
            BaseDatos.Ejecutar(
                @"INSERT INTO categoria_egreso (id_categoria_egreso, nombre, activa, creada_por)
                  VALUES (@id, @nombre, @activo, @usuario)",
                ("@id", motivo.Id), ("@nombre", motivo.Nombre), ("@activo", motivo.Activo),
                ("@usuario", BaseDatos.ExigirUsuarioDeLaSesion()));
        }

        public void Actualizar(MotivoEgreso motivo)
        {
            BaseDatos.Ejecutar("UPDATE categoria_egreso SET nombre = @nombre, activa = @activo WHERE id_categoria_egreso = @id",
                ("@nombre", motivo.Nombre), ("@activo", motivo.Activo), ("@id", motivo.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los motivos de egreso no se eliminan: se desactivan.");
        }
    }

    internal class GrupoEgresoPersistencia : IPersistencia<GrupoEgreso>
    {
        public void Insertar(GrupoEgreso grupo)
        {
            BaseDatos.Ejecutar(
                @"INSERT INTO grupo_egreso (id_grupo_egreso, nombre, activo, creado_por)
                  VALUES (@id, @nombre, @activo, @usuario)",
                ("@id", grupo.Id), ("@nombre", grupo.Nombre), ("@activo", grupo.Activo),
                ("@usuario", BaseDatos.ExigirUsuarioDeLaSesion()));
        }

        public void Actualizar(GrupoEgreso grupo)
        {
            BaseDatos.Ejecutar("UPDATE grupo_egreso SET nombre = @nombre, activo = @activo WHERE id_grupo_egreso = @id",
                ("@nombre", grupo.Nombre), ("@activo", grupo.Activo), ("@id", grupo.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los grupos de egreso no se eliminan: se desactivan.");
        }
    }
}
