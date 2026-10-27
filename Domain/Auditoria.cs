using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Domain
{
    /// <summary>Registro inmutable de quién hizo qué sobre qué entidad.</summary>
    public class Auditoria : Entidad
    {
        public const int LargoMaximoEntidad = 50;
        public const int LargoMaximoDetalle = 500;

        public Auditoria(Usuario usuario, AccionAuditoria accion, string entidadAfectada, int entidadId, DateTime fecha,
                         string? detalle = null)
        {
            Usuario = usuario ?? throw new ReglaNegocioException("La auditoría debe registrar al usuario que realizó la acción.");

            if (!Enum.IsDefined(accion))
                throw new ReglaNegocioException("La acción de auditoría no es válida.");
            if (entidadId <= 0)
                throw new ReglaNegocioException("La auditoría debe referirse a una entidad guardada.");

            Accion = accion;
            EntidadAfectada = Validaciones.ExigirTexto(entidadAfectada, "nombre de la entidad auditada", LargoMaximoEntidad);
            EntidadId = entidadId;
            Fecha = fecha;

            // El detalle es informativo: si es muy largo se recorta en lugar de impedir la operación auditada.
            var texto = (detalle ?? string.Empty).Trim();
            Detalle = texto.Length > LargoMaximoDetalle ? texto[..LargoMaximoDetalle] : texto;
        }

        public Usuario Usuario { get; }

        public AccionAuditoria Accion { get; }

        public string EntidadAfectada { get; }

        public int EntidadId { get; }

        public DateTime Fecha { get; }

        /// <summary>Qué cambió, en texto (por ejemplo "Rol: Operador → Contador"); vacío si no hace falta.</summary>
        public string Detalle { get; }
    }
}
