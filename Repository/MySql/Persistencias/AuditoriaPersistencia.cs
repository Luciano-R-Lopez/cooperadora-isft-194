using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    internal class AuditoriaPersistencia : IPersistencia<Auditoria>
    {
        public void Insertar(Auditoria registro)
        {
            BaseDatos.Ejecutar(
                @"INSERT INTO auditoria (id_auditoria, id_usuario, entidad, id_registro, accion, detalle, fecha_hora)
                  VALUES (@id, @usuario, @entidad, @registro, @accion, @detalle, @fecha)",
                ("@id", registro.Id), ("@usuario", registro.Usuario.Id), ("@entidad", registro.EntidadAfectada),
                ("@registro", registro.EntidadId), ("@accion", BaseDatos.Nombre(registro.Accion)),
                ("@detalle", BaseDatos.TextoONulo(registro.Detalle)), ("@fecha", registro.Fecha));
        }

        public void Actualizar(Auditoria registro)
        {
            throw new NotSupportedException("La auditoría no se modifica.");
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("La auditoría no se elimina.");
        }
    }
}
