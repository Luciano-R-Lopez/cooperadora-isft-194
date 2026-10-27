using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    // Registro de quién hizo qué sobre qué dato. No se modifica.
    public class Auditoria : IEntidad
    {
        public const int LargoMaximoEntidad = 50;
        public const int LargoMaximoDetalle = 500;

        public Auditoria(int id, Usuario usuario, AccionAuditoria accion, string entidadAfectada, int entidadId,
            DateTime fecha, string? detalle)
        {
            if (usuario == null)
            {
                throw new NegocioException("La auditoría debe registrar al usuario que hizo la acción.");
            }
            if (!Enum.IsDefined(accion))
            {
                throw new NegocioException("La acción de auditoría no es válida.");
            }
            if (entidadId <= 0)
            {
                throw new NegocioException("La auditoría debe referirse a un dato guardado.");
            }

            Id = id;
            Usuario = usuario;
            Accion = accion;
            EntidadAfectada = Validaciones.ExigirTexto(entidadAfectada, "nombre del dato auditado", LargoMaximoEntidad);
            EntidadId = entidadId;
            Fecha = fecha;

            // El detalle es informativo: si es muy largo se recorta en lugar de impedir la operación.
            string texto = (detalle ?? string.Empty).Trim();
            Detalle = texto.Length > LargoMaximoDetalle ? texto.Substring(0, LargoMaximoDetalle) : texto;
        }

        public int Id { get; private set; }

        public Usuario Usuario { get; private set; }

        public AccionAuditoria Accion { get; private set; }

        public string EntidadAfectada { get; private set; }

        public int EntidadId { get; private set; }

        public DateTime Fecha { get; private set; }

        // Qué cambió, por ejemplo "Roles: Operador → Operador, Contador"; vacío si no hace falta.
        public string Detalle { get; private set; }
    }
}
