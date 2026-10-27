using CooperadoraISFT94.Common;
using System.Drawing.Printing;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Forms.Utils
{
    /// <summary>
    /// Reportes impresos (recibo de cuota, resumen de caja). Se abren en la vista previa de impresión:
    /// desde ahí se imprimen en papel o en PDF eligiendo "Microsoft Print to PDF".
    /// </summary>
    public static class Reportes
    {
        private const string Institucion = Configuracion.NombreInstitucion;

        public static void ImprimirRecibo(ReciboPagoDTO recibo)
        {
            var documento = new DocumentoSimple($"Recibo {recibo.Numero}")
                .Titulo(Institucion)
                .Subtitulo($"Recibo de pago de cuota social N° {recibo.Numero:D6}")
                .Espacio()
                .Dato("Fecha", recibo.Fecha.ToString("dd/MM/yyyy HH:mm"))
                .Dato("Socio", recibo.Socio);

            if (recibo.PagadoPor.Length > 0)
                documento.Dato("Pagó", $"{recibo.PagadoPor} (responsable)");

            documento.Dato("Medio de pago", recibo.MedioPago)
                     .Dato("Atendió", recibo.Usuario)
                     .Espacio()
                     .Tabla(new[] { "Concepto", "Socio", "Importe" }, new[] { 2f, 4f, 1.6f },
                            recibo.Cuotas.Select(c => new[] { $"Cuota social {c.Anio}", c.Socio, c.Monto.ToString("C2") }))
                     .Dato("Total", recibo.Monto.ToString("C2"))
                     .Espacio()
                     .Texto("Recibimos el importe indicado en concepto de cuota social.");

            documento.MostrarVistaPrevia();
        }

        public static void ImprimirResumenCaja(ResumenCajaDTO resumen, IReadOnlyList<MovimientoDTO> movimientos)
        {
            var caja = resumen.Caja;
            var documento = new DocumentoSimple($"Caja {caja.Id}")
                .Titulo(Institucion)
                .Subtitulo($"Resumen de caja #{caja.Id} ({caja.Estado.ToString().ToLowerInvariant()})")
                .Espacio()
                .Dato("Apertura", caja.FechaApertura.ToString("dd/MM/yyyy HH:mm"))
                .Dato("Cierre", caja.FechaCierre?.ToString("dd/MM/yyyy HH:mm") ?? "-")
                .Dato("Saldo inicial", resumen.MontoInicial.ToString("C2"))
                .Dato("Total ingresos", resumen.TotalIngresos.ToString("C2"))
                .Dato("Total egresos", resumen.TotalEgresos.ToString("C2"))
                .Dato("Saldo esperado", resumen.MontoEsperado.ToString("C2"))
                .Dato("Saldo contado", resumen.MontoDeclarado?.ToString("C2") ?? "-")
                .Dato("Diferencia", resumen.Diferencia?.ToString("C2") ?? "-")
                .Dato("Movimientos", $"{resumen.CantidadMovimientos} vigentes, {resumen.CantidadAnulados} anulados")
                .Espacio()
                .Subtitulo("Saldo por medio de pago")
                .Tabla(new[] { "Medio de pago", "Saldo" }, new[] { 3f, 2f },
                       resumen.TotalesPorMedio.Select(t => new[] { t.MedioPago, t.Monto.ToString("C2") }))
                .Espacio()
                .Subtitulo("Egresos por grupo")
                .Tabla(new[] { "Grupo", "Egresos", "Total" }, new[] { 3f, 1f, 2f },
                       movimientos.Where(m => m.Tipo == TipoMovimiento.Egreso && !m.Anulado)
                                  .GroupBy(m => m.Grupo)
                                  .OrderBy(g => g.Key)
                                  .Select(g => new[] { g.Key, g.Count().ToString(), g.Sum(m => m.Monto).ToString("C2") }))
                .Espacio()
                .Subtitulo("Movimientos")
                .Tabla(new[] { "Fecha", "Tipo", "Categoría / motivo", "Grupo", "Detalle", "Medio", "Monto", "Estado" },
                       new[] { 2.2f, 1.3f, 2f, 1.8f, 3.5f, 1.8f, 1.8f, 1.4f },
                       movimientos.OrderBy(m => m.Fecha).Select(m => new[]
                       {
                           m.Fecha.ToString("dd/MM HH:mm"), m.Tipo.Texto(), m.Clasificacion, m.Grupo, m.Detalle,
                           m.MedioPago, m.MontoConSigno.ToString("C2"), m.Anulado ? "Anulado" : "Vigente"
                       }));

            documento.MostrarVistaPrevia();
        }

        /// <summary>Documento de texto en blanco y negro, con paginado automático.</summary>
        private sealed class DocumentoSimple
        {
            private const float Margen = 60;
            private const float Interlineado = 4;

            private readonly string _nombre;
            private readonly List<Action<Pagina>> _bloques = new();

            public DocumentoSimple(string nombre) => _nombre = nombre;

            public DocumentoSimple Titulo(string texto) => Agregar(p => p.Linea(texto, p.FuenteTitulo));
            public DocumentoSimple Subtitulo(string texto) => Agregar(p => p.Linea(texto, p.FuenteSubtitulo));
            public DocumentoSimple Texto(string texto) => Agregar(p => p.Linea(texto, p.FuenteNormal));
            public DocumentoSimple Espacio() => Agregar(p => p.Saltar(12));
            public DocumentoSimple Dato(string clave, string valor) => Agregar(p => p.Dato(clave, valor));

            public DocumentoSimple Tabla(string[] titulos, float[] pesos, IEnumerable<string[]> filas)
            {
                var lista = filas.ToList();
                Agregar(p => p.Fila(titulos, pesos, p.FuenteNegrita));
                foreach (var fila in lista)
                    Agregar(p => p.Fila(fila, pesos, p.FuenteChica));
                if (lista.Count == 0)
                    Agregar(p => p.Linea("(sin datos)", p.FuenteChica));
                return this;
            }

            private DocumentoSimple Agregar(Action<Pagina> bloque)
            {
                _bloques.Add(bloque);
                return this;
            }

            public void MostrarVistaPrevia()
            {
                using var impresion = new PrintDocument { DocumentName = _nombre };
                var siguiente = 0;

                impresion.BeginPrint += (_, _) => siguiente = 0;
                impresion.PrintPage += (_, e) =>
                {
                    using var pagina = new Pagina(e);
                    while (siguiente < _bloques.Count)
                    {
                        if (!pagina.HayLugar && pagina.YaEscribio)
                            break;
                        _bloques[siguiente](pagina);
                        siguiente++;
                    }
                    e.HasMorePages = siguiente < _bloques.Count;
                };

                using var vista = new PrintPreviewDialog
                {
                    Document = impresion,
                    WindowState = FormWindowState.Maximized,
                    UseAntiAlias = true
                };
                vista.ShowDialog();
            }

            /// <summary>Estado de la página que se está imprimiendo: posición actual y fuentes.</summary>
            private sealed class Pagina : IDisposable
            {
                private readonly PrintPageEventArgs _e;
                private float _y;

                public Font FuenteTitulo { get; } = new("Segoe UI", 16f, FontStyle.Bold);
                public Font FuenteSubtitulo { get; } = new("Segoe UI", 12f, FontStyle.Bold);
                public Font FuenteNormal { get; } = new("Segoe UI", 10f);
                public Font FuenteNegrita { get; } = new("Segoe UI", 9f, FontStyle.Bold);
                public Font FuenteChica { get; } = new("Segoe UI", 9f);

                public Pagina(PrintPageEventArgs e)
                {
                    _e = e;
                    _y = Margen;
                }

                private float Izquierda => Margen;
                private float Ancho => _e.PageBounds.Width - 2 * Margen;
                private float Fondo => _e.PageBounds.Height - Margen;

                public bool YaEscribio => _y > Margen;
                public bool HayLugar => _y + 40 < Fondo;

                public void Saltar(float alto) => _y += alto;

                public void Linea(string texto, Font fuente)
                {
                    var area = new RectangleF(Izquierda, _y, Ancho, Fondo - _y);
                    var alto = _e.Graphics!.MeasureString(texto, fuente, (int)Ancho).Height;
                    _e.Graphics.DrawString(texto, fuente, Brushes.Black, area);
                    _y += alto + Interlineado;
                }

                public void Dato(string clave, string valor)
                {
                    var anchoClave = 170f;
                    _e.Graphics!.DrawString(clave + ":", FuenteNegrita, Brushes.Black, Izquierda, _y);
                    var area = new RectangleF(Izquierda + anchoClave, _y, Ancho - anchoClave, Fondo - _y);
                    var alto = _e.Graphics.MeasureString(valor, FuenteNormal, (int)area.Width).Height;
                    _e.Graphics.DrawString(valor, FuenteNormal, Brushes.Black, area);
                    _y += Math.Max(alto, FuenteNegrita.GetHeight(_e.Graphics)) + Interlineado;
                }

                public void Fila(string[] celdas, float[] pesos, Font fuente)
                {
                    var total = pesos.Sum();
                    var x = Izquierda;
                    var alto = 0f;

                    for (var i = 0; i < celdas.Length; i++)
                    {
                        var ancho = Ancho * pesos[i] / total;
                        var area = new RectangleF(x, _y, ancho - 6, Fondo - _y);
                        alto = Math.Max(alto, _e.Graphics!.MeasureString(celdas[i], fuente, (int)area.Width).Height);
                        _e.Graphics.DrawString(celdas[i], fuente, Brushes.Black, area);
                        x += ancho;
                    }

                    _y += alto + 2;
                    _e.Graphics!.DrawLine(Pens.LightGray, Izquierda, _y, Izquierda + Ancho, _y);
                    _y += 3;
                }

                public void Dispose()
                {
                    FuenteTitulo.Dispose();
                    FuenteSubtitulo.Dispose();
                    FuenteNormal.Dispose();
                    FuenteNegrita.Dispose();
                    FuenteChica.Dispose();
                }
            }
        }
    }
}
