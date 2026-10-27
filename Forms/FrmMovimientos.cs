using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Ingresos y egresos de cada caja, con filtros. Desde acá se anulan movimientos de la caja abierta, se imprime
    // el resumen de caja y se exportan los movimientos o los egresos de un período organizados por grupo.
    public partial class FrmMovimientos : Form
    {
        private readonly MovimientoController movimientos;
        private readonly CajaController cajas;
        private readonly CatalogoController catalogos;
        private List<MovimientoDTO> lista = new List<MovimientoDTO>();
        private bool cargando;

        public FrmMovimientos()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            movimientos = new MovimientoController();
            cajas = new CajaController();
            catalogos = new CatalogoController();
        }

        private void FrmMovimientos_Load(object? sender, EventArgs e)
        {
            cargando = true;
            cmbTipo.Items.Add(new Opcion<TipoMovimiento?>("Todos", null));
            cmbTipo.Items.Add(new Opcion<TipoMovimiento?>("Ingresos", TipoMovimiento.Ingreso));
            cmbTipo.Items.Add(new Opcion<TipoMovimiento?>("Egresos", TipoMovimiento.Egreso));
            cmbTipo.SelectedIndex = 0;
            try
            {
                // Las cajas vienen de la más reciente a la más antigua: se elige la abierta o, si no hay, la última.
                List<CajaDTO> todas = cajas.ObtenerTodas();
                foreach (CajaDTO caja in todas)
                {
                    cmbCaja.Items.Add(caja);
                }
                int abierta = todas.FindIndex(c => c.Abierta);
                cmbCaja.SelectedIndex = abierta >= 0 ? abierta : (todas.Count > 0 ? 0 : -1);

                LlenarConTodos(cmbMedio, catalogos.ObtenerMediosPago(true));
                LlenarConTodos(cmbGrupo, catalogos.ObtenerGruposEgreso(true));
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
            cargando = false;
            CargarMovimientos();
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private static void LlenarConTodos(ComboBox combo, List<ItemCatalogoDTO> items)
        {
            combo.Items.Add(new Opcion<int?>("Todos", null));
            foreach (ItemCatalogoDTO item in items)
            {
                combo.Items.Add(new Opcion<int?>(item.Nombre, item.Id));
            }
            combo.SelectedIndex = 0;
        }

        private void Filtro_Cambio(object? sender, EventArgs e)
        {
            if (!cargando)
            {
                CargarMovimientos();
            }
        }

        private void txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            MostrarGrilla();
        }

        private CajaDTO? CajaElegida()
        {
            return cmbCaja.SelectedItem as CajaDTO;
        }

        private void CargarMovimientos()
        {
            CajaDTO? caja = CajaElegida();
            lista = new List<MovimientoDTO>();
            if (caja != null)
            {
                MovimientoFiltroDTO filtro = new MovimientoFiltroDTO();
                filtro.CajaId = caja.Id;
                filtro.Tipo = (cmbTipo.SelectedItem as Opcion<TipoMovimiento?>)?.Valor;
                filtro.MedioPagoId = (cmbMedio.SelectedItem as Opcion<int?>)?.Valor;
                filtro.GrupoEgresoId = (cmbGrupo.SelectedItem as Opcion<int?>)?.Valor;
                try
                {
                    lista = movimientos.Obtener(filtro);
                }
                catch (NegocioException ex)
                {
                    Mensajes.Advertencia(ex.Message);
                }
            }
            MostrarGrilla();
        }

        private void MostrarGrilla()
        {
            List<MovimientoDTO> visibles = new List<MovimientoDTO>();
            foreach (MovimientoDTO movimiento in lista)
            {
                if (Busqueda.Coincide(txtBuscar.Text, movimiento.Clasificacion, movimiento.Grupo, movimiento.Detalle,
                    movimiento.Usuario, movimiento.MedioPago, movimiento.Estado))
                {
                    visibles.Add(movimiento);
                }
            }

            dgvMovimientos.MensajeSinFilas(CajaElegida() == null
                ? "Todavía no se abrió ninguna caja."
                : "No hay movimientos con esos filtros.");
            dgvMovimientos.DataSource = null;
            dgvMovimientos.DataSource = visibles;
            dgvMovimientos.MostrarColumnas(("Fecha", "Fecha y hora"), ("Tipo", "Tipo"), ("Clasificacion", "Categoría / motivo"),
                ("Grupo", "Grupo"), ("Detalle", "Detalle"), ("MedioPago", "Medio de pago"), ("MontoConSigno", "Monto"),
                ("Usuario", "Usuario"), ("Estado", "Estado"));
            dgvMovimientos.FormatoFecha("Fecha", Formato.FechaHora);
            dgvMovimientos.FormatoMoneda("MontoConSigno");
            dgvMovimientos.Columna("Detalle").FillWeight = 200;
            dgvMovimientos.MostrarEnumsConTexto();
            dgvMovimientos.QuitarSeleccion();
        }

        private void dgvMovimientos_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvMovimientos.Rows)
            {
                if (fila.DataBoundItem is MovimientoDTO movimiento && movimiento.Anulado)
                {
                    Tema.AtenuarFila(fila);
                }
            }
        }

        private void btnAnular_Click(object? sender, EventArgs e)
        {
            MovimientoDTO? movimiento = dgvMovimientos.Seleccionado<MovimientoDTO>();
            if (movimiento == null)
            {
                Mensajes.Advertencia("Seleccione un movimiento de la grilla para anularlo.");
                return;
            }

            string pregunta = "Se va a anular el " + movimiento.Tipo.Texto().ToLowerInvariant() + " de " + Formato.Monto(movimiento.Monto)
                + " (" + movimiento.Clasificacion + "). Su efecto en la caja se revierte, pero queda registrado.\n\nMotivo de la anulación:";
            string? motivo = FrmTexto.Pedir(this, "Anular movimiento", pregunta, "Anular");
            if (motivo == null)
            {
                return;
            }

            try
            {
                movimientos.Anular(movimiento.Id, motivo);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }
            CargarMovimientos();
            Aviso.Mostrar(lblAviso, "Movimiento anulado.");
        }

        private void btnImprimir_Click(object? sender, EventArgs e)
        {
            CajaDTO? caja = CajaElegida();
            if (caja == null)
            {
                Mensajes.Advertencia("Elija una caja.");
                return;
            }

            try
            {
                ResumenCajaDTO resumen = cajas.ObtenerResumen(caja.Id);
                MovimientoFiltroDTO filtro = new MovimientoFiltroDTO();
                filtro.CajaId = caja.Id;
                Reportes.ImprimirResumenCaja(resumen, movimientos.Obtener(filtro));
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void btnExportar_Click(object? sender, EventArgs e)
        {
            ExportarCsv.Grilla(dgvMovimientos, "Movimientos");
        }

        private void btnExportarEgresos_Click(object? sender, EventArgs e)
        {
            using (FrmExportarEgresos dialogo = new FrmExportarEgresos())
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    List<MovimientoDTO> egresos = movimientos.ObtenerEgresos(dialogo.Desde, dialogo.Hasta);
                    if (egresos.Count == 0)
                    {
                        Mensajes.Informacion("No hay egresos en ese período.");
                        return;
                    }
                    if (ExportarCsv.EgresosPorGrupo(egresos, dialogo.Desde, dialogo.Hasta))
                    {
                        Aviso.Mostrar(lblAviso, "Se exportaron " + egresos.Count + " egresos.");
                    }
                }
                catch (NegocioException ex)
                {
                    Mensajes.Advertencia(ex.Message);
                }
            }
        }
    }
}
