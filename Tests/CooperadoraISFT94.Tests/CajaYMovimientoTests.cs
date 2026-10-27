using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Tests
{
    public class CajaYMovimientoTests
    {
        [Fact]
        public void Ingreso_acredita_la_caja_y_su_medio_y_queda_ligado_a_la_caja()
        {
            var caja = Crear.Caja(100m);
            var medio = Crear.Efectivo();
            var total = Crear.Total(caja, medio);
            var ingreso = Crear.Ingreso(50m, medio);

            ingreso.AfectarCaja(caja, total);

            Assert.Equal(150m, caja.MontoTotal);
            Assert.Equal(50m, total.Monto);
            Assert.Same(caja, ingreso.Caja);
        }

        [Fact]
        public void Egreso_debita_la_caja_y_su_medio()
        {
            var caja = Crear.Caja(100m);
            var medio = Crear.Efectivo();
            var total = Crear.Total(caja, medio);

            Crear.Egreso(30m, medio).AfectarCaja(caja, total);

            Assert.Equal(70m, caja.MontoTotal);
            Assert.Equal(-30m, total.Monto);
        }

        [Fact]
        public void Egreso_mayor_al_saldo_de_la_caja_falla_sin_modificar_nada()
        {
            var caja = Crear.Caja(100m);
            var medio = Crear.Efectivo();
            var total = Crear.Total(caja, medio);
            var egreso = Crear.Egreso(100.01m, medio);

            Assert.Throws<NegocioException>(() => egreso.AfectarCaja(caja, total));
            Assert.Equal(100m, caja.MontoTotal);
            Assert.Equal(0m, total.Monto);
            Assert.Null(egreso.Caja);
        }

        [Fact]
        public void Egreso_por_todo_el_saldo_deja_la_caja_en_cero()
        {
            var caja = Crear.Caja(100m);
            var medio = Crear.Efectivo();

            Crear.Egreso(100m, medio).AfectarCaja(caja, Crear.Total(caja, medio));

            Assert.Equal(0m, caja.MontoTotal);
        }

        [Fact]
        public void Un_movimiento_no_se_puede_aplicar_dos_veces()
        {
            var caja = Crear.Caja(0m);
            var medio = Crear.Efectivo();
            var total = Crear.Total(caja, medio);
            var ingreso = Crear.Ingreso(10m, medio);
            ingreso.AfectarCaja(caja, total);

            Assert.Throws<NegocioException>(() => ingreso.AfectarCaja(caja, total));
            Assert.Equal(10m, caja.MontoTotal);
        }

        [Fact]
        public void No_se_registran_movimientos_en_una_caja_cerrada()
        {
            var caja = Crear.Caja(0m);
            var medio = Crear.Efectivo();
            var total = Crear.Total(caja, medio);
            caja.Cerrar(DateTime.Now, 0m);

            Assert.Throws<NegocioException>(() => Crear.Ingreso(10m, medio).AfectarCaja(caja, total));
        }

        [Fact]
        public void El_total_debe_corresponder_al_medio_del_movimiento()
        {
            var caja = Crear.Caja(0m);
            var total = Crear.Total(caja, new MedioPago(Crear.Id(), "Transferencia"));

            Assert.Throws<NegocioException>(() => Crear.Ingreso(10m, Crear.Efectivo()).AfectarCaja(caja, total));
            Assert.Equal(0m, caja.MontoTotal);
        }

        [Fact]
        public void Cerrar_reemplaza_el_monto_total_por_el_contado()
        {
            var caja = Crear.Caja(100m);

            caja.Cerrar(DateTime.Now, 90m);

            Assert.Equal(EstadoCaja.Cerrada, caja.Estado);
            Assert.Equal(90m, caja.MontoTotal);
            Assert.Throws<NegocioException>(() => caja.Cerrar(DateTime.Now, 90m));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10.001)]
        public void Monto_inicial_invalido_se_rechaza(decimal monto)
        {
            Assert.Throws<NegocioException>(() => Crear.Caja(monto));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(1.234)]
        public void Movimiento_con_monto_invalido_se_rechaza(decimal monto)
        {
            Assert.Throws<NegocioException>(() => Crear.Ingreso(monto, Crear.Efectivo()));
        }
    }
}
