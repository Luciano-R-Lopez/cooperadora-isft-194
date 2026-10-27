namespace CooperadoraISFT94.Common
{
    public static class Formato
    {
        public const string FechaHora = "dd/MM/yyyy HH:mm";
        public const string Fecha = "dd/MM/yyyy";
        public const string Moneda = "$ #,##0.00";

        public static string Monto(decimal monto)
        {
            return monto.ToString(Moneda);
        }

        public static string Saludo(DateTime ahora)
        {
            if (ahora.Hour < 13)
            {
                return "Buen día";
            }
            return ahora.Hour < 20 ? "Buenas tardes" : "Buenas noches";
        }

        public static string DiaSemana(DateTime fecha)
        {
            string texto = fecha.ToString("dddd");
            return texto.Length == 0 ? texto : char.ToUpper(texto[0]) + texto.Substring(1);
        }
    }
}
