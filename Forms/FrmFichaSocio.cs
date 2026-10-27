using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Datos de un socio y sus cuotas. Si es responsable, desde acá cobra juntas sus cuotas y las de sus socios a cargo
    // eligiendo cuáles.
    public partial class FrmFichaSocio : Form
    {
        private readonly int socioId;
        private readonly SocioController socios;
        private readonly CuotaController cuotas;
        private readonly CatalogoController catalogos;
        private SocioDTO? socio;
        private List<SocioDTO> aCargo = new List<SocioDTO>();

        // Para que el diseñador de Visual Studio pueda abrir el formulario.
        public FrmFichaSocio()
            : this(0)
        {
        }

        public FrmFichaSocio(int socioId)
        {
            InitializeComponent();
            Tema.Aplicar(this);
            this.socioId = socioId;
            socios = new SocioController();
            cuotas = new CuotaController();
            catalogos = new CatalogoController();
        }

        private void FrmFichaSocio_Load(object? sender, EventArgs e)
        {
            CargarDatos();
            CargarCuotas();
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void CargarDatos()
        {
            try
            {
                socio = socios.ObtenerPorId(socioId);
                aCargo = socio.Categoria == Categoria.Responsable ? socios.ObtenerACargo(socio.Id) : new List<SocioDTO>();
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }

            Text = "Ficha de " + socio.Apellido + ", " + socio.Nombre;
            lblTituloSocio.Text = socio.Apellido + ", " + socio.Nombre + (socio.Activo ? string.Empty : "  (dado de baja)");
            lblDatoSocio.Text = "DNI " + socio.Dni + "   |   " + socio.Categoria.Texto()
                + "   |   Teléfono: " + (socio.Telefono.Length > 0 ? socio.Telefono : "-")
                + "   |   Email: " + (socio.Email.Length > 0 ? socio.Email : "-");

            if (socio.Categoria != Categoria.Responsable)
            {
                lblDatoVinculo.Text = "Responsable: " + (socio.Responsable.Length > 0 ? socio.Responsable : "no tiene");
            }
            else
            {
                lblDatoVinculo.Text = aCargo.Count == 0
                    ? "Socios a cargo: ninguno"
                    : "Socios a cargo: " + string.Join("; ", aCargo.Select(s => s.Apellido + ", " + s.Nombre));
            }

            // A un socio dado de baja no se le generan cuotas nuevas (sí se le cobran las pendientes).
            btnGenerarCuota.Visible = socio.Activo;
            btnCobrar.Text = aCargo.Count > 0 ? "Cobrar cuotas (suyas y de sus socios a cargo)" : "Cobrar cuotas";
            btnCobrar.Width = aCargo.Count > 0 ? 360 : 200;
        }

        private void CargarCuotas()
        {
            List<CuotaDTO> lista = new List<CuotaDTO>();
            try
            {
                lista = cuotas.ObtenerPorSocio(socioId);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }

            dgvCuotas.MensajeSinFilas("El socio todavía no tiene cuotas. Use Generar cuota.");
            dgvCuotas.DataSource = null;
            dgvCuotas.DataSource = lista;
            dgvCuotas.MostrarColumnas(("Anio", "Año"), ("Monto", "Monto"), ("Estado", "Estado"));
            dgvCuotas.FormatoMoneda("Monto");
            dgvCuotas.MostrarEnumsConTexto();
            dgvCuotas.QuitarSeleccion();
        }

        private void dgvCuotas_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvCuotas.Rows)
            {
                if (fila.DataBoundItem is CuotaDTO cuota && cuota.Estado == EstadoCuota.Pagada)
                {
                    Tema.ResaltarFila(fila);
                }
            }
        }

        private void btnGenerarCuota_Click(object? sender, EventArgs e)
        {
            using (FrmGenerarCuota dialogo = new FrmGenerarCuota(socioId))
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
            }
            CargarCuotas();
            Aviso.Mostrar(lblAviso, "Cuota generada.");
        }

        // Cuotas pendientes del socio y, si es responsable, también las de sus socios a cargo.
        private void btnCobrar_Click(object? sender, EventArgs e)
        {
            if (socio == null)
            {
                return;
            }

            List<CuotaDTO> pendientes = new List<CuotaDTO>();
            List<ItemCatalogoDTO> medios;
            try
            {
                pendientes.AddRange(Pendientes(socio.Id));
                foreach (SocioDTO socioACargo in aCargo)
                {
                    pendientes.AddRange(Pendientes(socioACargo.Id));
                }
                medios = catalogos.ObtenerMediosPago(false);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }

            if (pendientes.Count == 0)
            {
                Mensajes.Informacion(aCargo.Count > 0
                    ? "Ni el socio ni sus socios a cargo tienen cuotas pendientes."
                    : "El socio no tiene cuotas pendientes.");
                return;
            }
            if (medios.Count == 0)
            {
                Mensajes.Advertencia("No hay medios de pago activos.");
                return;
            }

            bool variosSocios = pendientes.Select(c => c.SocioId).Distinct().Count() > 1;
            string titulo = variosSocios ? "Paga " + socio.Descripcion + " (responsable)" : socio.Descripcion;
            using (FrmCobrarCuotas dialogo = new FrmCobrarCuotas(titulo, pendientes, medios, socio.Responsable, variosSocios))
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                // Paga el responsable si cobra para sus socios a cargo, o si se marcó que paga el responsable del socio.
                int? pagadorId = aCargo.Count > 0 ? socio.Id : dialogo.PagaResponsable ? socio.ResponsableId : null;
                Cobrar(dialogo.CuotaIds, dialogo.MedioPagoId, pagadorId);
            }
        }

        private List<CuotaDTO> Pendientes(int id)
        {
            return cuotas.ObtenerPorSocio(id).Where(c => c.Estado == EstadoCuota.Pendiente).ToList();
        }

        private void Cobrar(List<int> cuotaIds, int medioPagoId, int? pagadorId)
        {
            ReciboPagoDTO recibo;
            try
            {
                recibo = cuotas.RegistrarPagos(cuotaIds, medioPagoId, pagadorId);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }

            CargarCuotas();
            Aviso.Mostrar(lblAviso, "Cobro registrado: " + Formato.Monto(recibo.Monto) + ".");
            if (Mensajes.Confirmar("Se cobraron " + recibo.Cuotas.Count + " cuota(s) por " + Formato.Monto(recibo.Monto)
                + ". Se registró el ingreso en la caja.\n\n¿Desea imprimir el recibo?"))
            {
                Reportes.ImprimirRecibo(recibo);
            }
        }
    }
}
