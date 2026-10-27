using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Valor de la cuota de cada año, generación de las cuotas para todos los socios y lista de morosos
    // (un socio por fila, con los años que debe) para cobrar o avisarles.
    public partial class FrmCuotas : Form, IVuelveDeSubpantalla
    {
        private readonly CuotaController cuotas;
        private readonly CatalogoController catalogos;
        private List<DeudorDTO> deudores = new List<DeudorDTO>();
        private bool cargando;

        public FrmCuotas()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            cuotas = new CuotaController();
            catalogos = new CatalogoController();
        }

        private void FrmCuotas_Load(object? sender, EventArgs e)
        {
            cargando = true;
            numAnio.Value = DateTime.Today.Year;
            cargando = false;
            ProponerValor();
            CargarAniosMorosos();
            CargarMorosos();
        }

        public void AlVolverDeSubpantalla(Form subpantalla)
        {
            // En la ficha se pudo cobrar o generar cuotas.
            CargarAniosMorosos();
            CargarMorosos();
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void numAnio_ValueChanged(object? sender, EventArgs e)
        {
            if (!cargando)
            {
                ProponerValor();
            }
        }

        // Muestra el valor fijado para el año elegido (o el de la última cuota de ese año).
        private void ProponerValor()
        {
            try
            {
                decimal sugerido = cuotas.ObtenerMontoSugerido((int)numAnio.Value) ?? 0m;
                numValor.Value = Math.Min(sugerido, numValor.Maximum);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void btnGuardarValor_Click(object? sender, EventArgs e)
        {
            try
            {
                cuotas.FijarValorCuota((int)numAnio.Value, numValor.Value);
                Aviso.Mostrar(lblAviso, "Valor de la cuota " + numAnio.Value + " guardado: " + Formato.Monto(numValor.Value) + ".");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void btnGenerar_Click(object? sender, EventArgs e)
        {
            int anio = (int)numAnio.Value;
            if (!Mensajes.Confirmar("¿Generar la cuota " + anio + " de " + Formato.Monto(numValor.Value)
                + " a todos los socios activos que todavía no la tienen?"))
            {
                return;
            }

            try
            {
                GeneracionCuotasDTO resultado = cuotas.GenerarParaTodos(anio, numValor.Value);
                CargarAniosMorosos();
                CargarMorosos();
                Mensajes.Informacion("Se generaron " + resultado.Generadas + " cuotas " + resultado.Anio + " de "
                    + Formato.Monto(resultado.Monto) + ".\n" + resultado.YaTenian + " socios ya tenían la cuota de ese año.");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        // Años con cuotas pendientes, más "Todos los años". Se mantiene el elegido si sigue en la lista.
        private void CargarAniosMorosos()
        {
            int? elegido = (cmbAnioMorosos.SelectedItem as Opcion<int?>)?.Valor;
            List<int> anios = new List<int>();
            try
            {
                DeudoresFiltroDTO todos = new DeudoresFiltroDTO();
                todos.IncluirDadosDeBaja = true;
                foreach (DeudorDTO deudor in cuotas.ObtenerDeudores(todos))
                {
                    anios.AddRange(deudor.AniosQueDebe.Split(", ").Select(int.Parse));
                }
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }

            cargando = true;
            cmbAnioMorosos.Items.Clear();
            cmbAnioMorosos.Items.Add(new Opcion<int?>("Todos los años", null));
            cmbAnioMorosos.SelectedIndex = 0;
            foreach (int anio in anios.Distinct().OrderByDescending(a => a))
            {
                cmbAnioMorosos.Items.Add(new Opcion<int?>(anio.ToString(), anio));
                if (anio == elegido)
                {
                    cmbAnioMorosos.SelectedIndex = cmbAnioMorosos.Items.Count - 1;
                }
            }
            cargando = false;
        }

        private void cmbAnioMorosos_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!cargando)
            {
                CargarMorosos();
            }
        }

        private void chkDadosDeBaja_CheckedChanged(object? sender, EventArgs e)
        {
            CargarMorosos();
        }

        private void txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            MostrarGrilla();
        }

        private void CargarMorosos()
        {
            DeudoresFiltroDTO filtro = new DeudoresFiltroDTO();
            filtro.Anio = (cmbAnioMorosos.SelectedItem as Opcion<int?>)?.Valor;
            filtro.IncluirDadosDeBaja = chkDadosDeBaja.Checked;
            try
            {
                deudores = cuotas.ObtenerDeudores(filtro);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
            MostrarGrilla();
        }

        private void MostrarGrilla()
        {
            int? seleccionado = Seleccionado()?.SocioId;
            List<DeudorDTO> visibles = Visibles();

            dgvMorosos.MensajeSinFilas(deudores.Count == 0
                ? "No hay socios con cuotas pendientes."
                : "Ningún socio coincide con la búsqueda.");
            dgvMorosos.DataSource = null;
            dgvMorosos.DataSource = visibles;
            dgvMorosos.MostrarColumnas(("Socio", "Socio"), ("Dni", "DNI"), ("Paga", "Paga"), ("AniosQueDebe", "Años que debe"),
                ("Total", "Total"), ("Telefono", "Teléfono"), ("Email", "Email"), ("Estado", "Estado"));
            dgvMorosos.FormatoMoneda("Total");
            dgvMorosos.Columna("Paga").FillWeight = 170;
            dgvMorosos.Columna("Email").FillWeight = 140;
            dgvMorosos.Columna("Estado").FillWeight = 60;
            dgvMorosos.QuitarSeleccion();
            if (seleccionado.HasValue)
            {
                dgvMorosos.SeleccionarFila("SocioId", seleccionado.Value.ToString());
            }

            lblTotalMorosos.Text = visibles.Count + " socios deben " + visibles.Sum(d => d.CantidadCuotas) + " cuotas: "
                + Formato.Monto(visibles.Sum(d => d.Total));
        }

        private List<DeudorDTO> Visibles()
        {
            List<DeudorDTO> visibles = new List<DeudorDTO>();
            foreach (DeudorDTO deudor in deudores)
            {
                if (Busqueda.Coincide(txtBuscar.Text, deudor.Socio, deudor.Dni, deudor.Paga))
                {
                    visibles.Add(deudor);
                }
            }
            return visibles;
        }

        private void dgvMorosos_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvMorosos.Rows)
            {
                if (fila.DataBoundItem is DeudorDTO deudor && !deudor.SocioActivo)
                {
                    Tema.AtenuarFila(fila);
                }
            }
        }

        private DeudorDTO? Seleccionado()
        {
            return dgvMorosos.Seleccionado<DeudorDTO>();
        }

        private void dgvMorosos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnCobrar_Click(sender, e);
            }
        }

        // Cobra en un solo pago las cuotas pendientes que se elijan del socio seleccionado.
        private void btnCobrar_Click(object? sender, EventArgs e)
        {
            DeudorDTO? deudor = Seleccionado();
            if (deudor == null)
            {
                Mensajes.Advertencia("Seleccione un socio de la lista para cobrarle.");
                return;
            }

            List<CuotaDTO> pendientes;
            List<ItemCatalogoDTO> medios;
            try
            {
                pendientes = cuotas.ObtenerPorSocio(deudor.SocioId).Where(c => c.Estado == EstadoCuota.Pendiente).ToList();
                medios = catalogos.ObtenerMediosPago(false);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }
            if (medios.Count == 0)
            {
                Mensajes.Advertencia("No hay medios de pago activos.");
                return;
            }

                        using (FrmCobrarCuotas dialogo = new FrmCobrarCuotas(deudor.Socio + " (DNI " + deudor.Dni + ")", pendientes, medios,
                deudor.Responsable, false))
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                ReciboPagoDTO recibo;
                try
                {
                    recibo = cuotas.RegistrarPagos(dialogo.CuotaIds, dialogo.MedioPagoId,
                        dialogo.PagaResponsable ? deudor.ResponsableId : null);
                }
                catch (NegocioException ex)
                {
                    Mensajes.Advertencia(ex.Message);
                    return;
                }

                CargarAniosMorosos();
                CargarMorosos();
                Aviso.Mostrar(lblAviso, "Cobro registrado: " + Formato.Monto(recibo.Monto) + ".");
                if (Mensajes.Confirmar("Se cobraron " + recibo.Cuotas.Count + " cuota(s) por " + Formato.Monto(recibo.Monto)
                    + ".\n\n¿Desea imprimir el recibo?"))
                {
                    Reportes.ImprimirRecibo(recibo);
                }
            }
        }

        private void btnVerFicha_Click(object? sender, EventArgs e)
        {
            DeudorDTO? deudor = Seleccionado();
            if (deudor == null)
            {
                Mensajes.Advertencia("Seleccione un socio de la lista para ver su ficha.");
                return;
            }
            Navegacion.AbrirSubpantalla(this, new FrmFichaSocio(deudor.SocioId));
        }

        // Emails de los socios de la lista (o de su responsable, que es quien paga), separados por ";" para el campo CCO.
        private void btnCopiarEmails_Click(object? sender, EventArgs e)
        {
            List<DeudorDTO> visibles = Visibles();
            List<string> emails = visibles.Select(d => d.EmailDeContacto)
                                          .Where(email => email.Length > 0)
                                          .Distinct(StringComparer.OrdinalIgnoreCase)
                                          .ToList();
            if (emails.Count == 0)
            {
                Mensajes.Informacion("Ninguno de los socios de la lista tiene email cargado.");
                return;
            }

            Clipboard.SetText(string.Join("; ", emails));
            int sinEmail = visibles.Count(d => d.EmailDeContacto.Length == 0);
            Mensajes.Informacion("Se copiaron " + emails.Count + " emails. Péguelos en el campo \"CCO\" de su correo para enviar un recordatorio."
                + (sinEmail > 0 ? "\n\n" + sinEmail + " socio(s) de la lista no tienen email: habrá que llamarlos." : string.Empty));
        }

        private void btnExportar_Click(object? sender, EventArgs e)
        {
            ExportarCsv.Grilla(dgvMorosos, "Morosos");
        }
    }
}
