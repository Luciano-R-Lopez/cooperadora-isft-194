using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    // Donaciones y subsidios. Las cuotas sociales se cobran desde CuotaService, ligadas a la cuota.
    public class IngresoService
    {
        private readonly IRepository<Usuario> usuarioRepositorio;
        private readonly IRepository<MedioPago> medioRepositorio;
        private readonly MovimientoRegistrador registrador;

        public IngresoService()
            : this(Repositorios.Usuarios, Repositorios.MediosPago, new MovimientoRegistrador())
        {
        }

        public IngresoService(IRepository<Usuario> usuarioRepositorio, IRepository<MedioPago> medioRepositorio,
            MovimientoRegistrador registrador)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.medioRepositorio = medioRepositorio;
            this.registrador = registrador;
        }

        public IngresoDTO Registrar(IngresoDTO dto, int usuarioId)
        {
            if (dto == null)
            {
                throw new ArgumentException("Los datos del ingreso son obligatorios.");
            }
            if (dto.CategoriaIngreso == CategoriaIngreso.CuotaSocial)
            {
                throw new NegocioException("Las cuotas sociales se cobran desde la ficha del socio o desde la lista de morosos.");
            }

            Usuario usuario = usuarioRepositorio.ObtenerExistente(usuarioId);
            MedioPago medio = medioRepositorio.ObtenerExistente(dto.MedioPagoId);
            medio.ExigirActivo();

            Ingreso ingreso = new Ingreso(registrador.ObtenerProximoId(), usuario, registrador.Ahora(), dto.Monto, medio,
                dto.CategoriaIngreso);
            registrador.Registrar(ingreso);
            return MapearADto(ingreso);
        }

        private static IngresoDTO MapearADto(Ingreso ingreso)
        {
            IngresoDTO dto = new IngresoDTO();
            dto.Id = ingreso.Id;
            dto.Fecha = ingreso.Fecha;
            dto.Monto = ingreso.Monto;
            dto.CategoriaIngreso = ingreso.CategoriaIngreso;
            dto.MedioPagoId = ingreso.MedioPago.Id;
            dto.MedioPago = ingreso.MedioPago.Nombre;
            return dto;
        }
    }
}
