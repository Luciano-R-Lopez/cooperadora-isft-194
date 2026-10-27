using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    // La baja es lógica: el socio queda con estado 'baja', la fecha y el usuario que la hizo.
    internal class SocioPersistencia : IPersistencia<Socio>
    {
        public void Insertar(Socio socio)
        {
            BaseDatos.Ejecutar(
                @"INSERT INTO socio (id_socio, dni, nombre, apellido, telefono, email, tipo, id_responsable, estado,
                                     fecha_alta, dado_de_alta_por)
                  VALUES (@id, @dni, @nombre, @apellido, @telefono, @email, @tipo, @responsable, @estado, CURDATE(), @usuario)",
                ("@id", socio.Id), ("@dni", socio.Dni), ("@nombre", socio.Nombre), ("@apellido", socio.Apellido),
                ("@telefono", BaseDatos.TextoONulo(socio.Telefono)), ("@email", BaseDatos.TextoONulo(socio.Email)),
                ("@tipo", BaseDatos.Nombre(socio.Categoria)), ("@responsable", socio.Responsable?.Id),
                ("@estado", Estado(socio)), ("@usuario", BaseDatos.ExigirUsuarioDeLaSesion()));
        }

        // La fecha y el usuario de la baja se guardan la primera vez; al reactivarlo se borran.
        public void Actualizar(Socio socio)
        {
            BaseDatos.Ejecutar(
                @"UPDATE socio
                  SET dni = @dni, nombre = @nombre, apellido = @apellido, telefono = @telefono, email = @email,
                      tipo = @tipo, id_responsable = @responsable, estado = @estado,
                      fecha_baja = IF(@estado = 'baja', COALESCE(fecha_baja, CURDATE()), NULL),
                      dado_de_baja_por = IF(@estado = 'baja', COALESCE(dado_de_baja_por, @usuario), NULL)
                  WHERE id_socio = @id",
                ("@dni", socio.Dni), ("@nombre", socio.Nombre), ("@apellido", socio.Apellido),
                ("@telefono", BaseDatos.TextoONulo(socio.Telefono)), ("@email", BaseDatos.TextoONulo(socio.Email)),
                ("@tipo", BaseDatos.Nombre(socio.Categoria)), ("@responsable", socio.Responsable?.Id),
                ("@estado", Estado(socio)), ("@usuario", BaseDatos.ExigirUsuarioDeLaSesion()), ("@id", socio.Id));
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los socios no se eliminan: se dan de baja.");
        }

        private static string Estado(Socio socio)
        {
            return socio.Activo ? "activo" : "baja";
        }
    }
}
