using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Tests
{
    public class CajaYMovimientoTests
    {
        [Fact]
        public void Ingreso_acredita_la_caja_y_su_medio_y_queda_ligado_a_la_caja()
        {
            var caja = new Caja(100m, DateTime.Now);
            var medio = Crear.Efectivo();
            var total = new CajaTotalMedioPago(caja, medio);
            var ingreso = Crear.Ingreso(50m, medio);

            ingreso.AfectarCaja(caja, total);

            Assert.Equal(150m, caja.MontoTotal);
            Assert.Equal(50m, total.Monto);
            Assert.Same(caja, ingreso.Caja);
        }

        [Fact]
        public void Egreso_debita_la_caja_y_su_medio()
        {
            var caja = new Caja(100m, DateTime.Now);
            var medio = Crear.Efectivo();
            var total = new CajaTotalMedioPago(caja, medio);

            Crear.Egreso(30m, medio).AfectarCaja(caja, total);

            Assert.Equal(70m, caja.MontoTotal);
            Assert.Equal(-30m, total.Monto);
        }

        [Fact]
        public void Egreso_mayor_al_saldo_de_la_caja_falla_sin_modificar_nada()
        {
            var caja = new Caja(100m, DateTime.Now);
            var medio = Crear.Efectivo();
            var total = new CajaTotalMedioPago(caja, medio);
            var egreso = Crear.Egreso(100.01m, medio);

            Assert.Throws<ReglaNegocioException>(() => egreso.AfectarCaja(caja, total));
            Assert.Equal(100m, caja.MontoTotal);
            Assert.Equal(0m, total.Monto);
            Assert.Null(egreso.Caja);
        }

        [Fact]
        public void Egreso_por_todo_el_saldo_deja_la_caja_en_cero()
        {
            var caja = new Caja(100m, DateTime.Now);
            var medio = Crear.Efectivo();

            Crear.Egreso(100m, medio).AfectarCaja(caja, new CajaTotalMedioPago(caja, medio));

            Assert.Equal(0m, caja.MontoTotal);
        }

        [Fact]
        public void Un_movimiento_no_se_puede_aplicar_dos_veces()
        {
            var caja = new Caja(0m, DateTime.Now);
            var medio = Crear.Efectivo();
            var total = new CajaTotalMedioPago(caja, medio);
            var ingreso = Crear.Ingreso(10m, medio);
            ingreso.AfectarCaja(caja, total);

            Assert.Throws<ReglaNegocioException>(() => ingreso.AfectarCaja(caja, total));
            Assert.Equal(10m, caja.MontoTotal);
        }

        [Fact]
        public void No_se_registran_movimientos_en_una_caja_cerrada()
        {
            var caja = new Caja(0m, DateTime.Now);
            var medio = Crear.Efectivo();
            var total = new CajaTotalMedioPago(caja, medio);
            caja.CerrarCaja(DateTime.Now, 0m);

            Assert.Throws<ReglaNegocioException>(() => Crear.Ingreso(10m, medio).AfectarCaja(caja, total));
        }

        [Fact]
        public void El_total_debe_corresponder_al_medio_del_movimiento()
        {
            var caja = new Caja(0m, DateTime.Now);
            var total = new CajaTotalMedioPago(caja, new MedioPago("Transferencia"));

            Assert.Throws<ReglaNegocioException>(() => Crear.Ingreso(10m, Crear.Efectivo()).AfectarCaja(caja, total));
            Assert.Equal(0m, caja.MontoTotal);
        }

        [Fact]
        public void Cerrar_reemplaza_el_monto_total_por_el_contado()
        {
            var caja = new Caja(100m, DateTime.Now);

            caja.CerrarCaja(DateTime.Now, 90m);

            Assert.Equal(EstadoCaja.Cerrada, caja.Estado);
            Assert.Equal(90m, caja.MontoTotal);
            Assert.Throws<ReglaNegocioException>(() => caja.CerrarCaja(DateTime.Now, 90m));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10.001)]
        public void Monto_inicial_invalido_se_rechaza(decimal monto)
        {
            Assert.Throws<ReglaNegocioException>(() => new Caja(monto, DateTime.Now));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(1.234)]
        public void Movimiento_con_monto_invalido_se_rechaza(decimal monto)
        {
            Assert.Throws<ReglaNegocioException>(() => Crear.Ingreso(monto, Crear.Efectivo()));
        }
    }
}
