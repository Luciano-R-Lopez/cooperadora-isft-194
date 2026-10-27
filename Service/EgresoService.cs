using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class EgresoService
    {
        private readonly IRepository<Usuario> usuarioRepositorio;
        private readonly IRepository<MedioPago> medioRepositorio;
        private readonly IRepository<MotivoEgreso> motivoRepositorio;
        private readonly IRepository<GrupoEgreso> grupoRepositorio;
        private readonly MovimientoRegistrador registrador;

        public EgresoService()
            : this(Repositorios.Usuarios, Repositorios.MediosPago, Repositorios.MotivosEgreso, Repositorios.GruposEgreso,
                new MovimientoRegistrador())
        {
        }

        public EgresoService(IRepository<Usuario> usuarioRepositorio, IRepository<MedioPago> medioRepositorio,
            IRepository<MotivoEgreso> motivoRepositorio, IRepository<GrupoEgreso> grupoRepositorio,
            MovimientoRegistrador registrador)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.medioRepositorio = medioRepositorio;
            this.motivoRepositorio = motivoRepositorio;
            this.grupoRepositorio = grupoRepositorio;
            this.registrador = registrador;
        }

        public EgresoDTO Registrar(EgresoDTO dto, int usuarioId)
        {
            if (dto == null)
            {
                throw new ArgumentException("Los datos del egreso son obligatorios.");
            }

            Usuario usuario = usuarioRepositorio.ObtenerExistente(usuarioId);
            MedioPago medio = medioRepositorio.ObtenerExistente(dto.MedioPagoId);
            medio.ExigirActivo();
            MotivoEgreso motivo = motivoRepositorio.ObtenerExistente(dto.MotivoEgresoId);
            motivo.ExigirActivo();
            GrupoEgreso grupo = grupoRepositorio.ObtenerExistente(dto.GrupoEgresoId);
            grupo.ExigirActivo();

            Egreso egreso = new Egreso(registrador.ObtenerProximoId(), usuario, registrador.Ahora(), dto.Monto, medio, motivo, grupo);
            registrador.Registrar(egreso);
            return MapearADto(egreso);
        }

        private static EgresoDTO MapearADto(Egreso egreso)
        {
            EgresoDTO dto = new EgresoDTO();
            dto.Id = egreso.Id;
            dto.Fecha = egreso.Fecha;
            dto.Monto = egreso.Monto;
            dto.MotivoEgresoId = egreso.Motivo.Id;
            dto.Motivo = egreso.Motivo.Nombre;
            dto.GrupoEgresoId = egreso.Grupo.Id;
            dto.Grupo = egreso.Grupo.Nombre;
            dto.MedioPagoId = egreso.MedioPago.Id;
            dto.MedioPago = egreso.MedioPago.Nombre;
            return dto;
        }
    }
}
