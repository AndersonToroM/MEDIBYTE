using Dominus.Backend.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominus.Backend.Application
{
    public class Util
    {
        public string UserSystem = "Sistema SIISO.";
        public string EmailOrigen_PorDefecto = "POR DEFECTO";
        public string EmailOrigen_Facturacion = "FACTURACION";
        public string ServiceRips = "Rips";
        public string ServiceFE = "Saphety";

        public Dian Dian = new Dian();

        public string QutarTildes(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            data = data.Replace("á", "a");
            data = data.Replace("Á", "A");
            data = data.Replace("é", "e");
            data = data.Replace("É", "E");
            data = data.Replace("í", "i");
            data = data.Replace("Í", "I");
            data = data.Replace("ó", "o");
            data = data.Replace("Ó", "O");
            data = data.Replace("ú", "u");
            data = data.Replace("Ú", "U");
            data = data.Replace("ñ", "n");
            data = data.Replace("Ñ", "N");
            data = data.Replace("ü", "u");
            data = data.Replace("Ü", "U");
            data = data.Trim();
            return data;
        }

        public string QuitarEspacios(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            data = data.Replace("  ", " ");
            data = data.Trim();
            return data;
        }

        public bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return new EmailAddressAttribute().IsValid(email);
        }

        public List<DateTime> FestivosSoloFecha(int anio)
        {
            return new FestivosColombia(anio).FestivosSoloFecha();
        }

        public List<DiaFestivo> Festivos(int anio)
        {
            return new FestivosColombia(anio).Festivos();
        }

        public int CalcularEdad(DateTime? fecha)
        {
            return CalcularEdad(fecha, DateTime.Now);
        }

        public int CalcularEdad(DateTime? fecha, DateTime? fechaCalcular)
        {
            if (!fecha.HasValue)
            {
                return 0;
            }
            else if (!fechaCalcular.HasValue)
            {
                return DateTime.Now.Year - fecha.Value.Year;
            }
            else
            {
                return fechaCalcular.Value.Year - fecha.Value.Year;
            }
        }

        public string CalcularPesoParaBytes(Int64 bytes)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            if (bytes < 0) { return "-" + CalcularPesoParaBytes(-bytes); }
            if (bytes == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(bytes, 1024);
            decimal adjustedSize = (decimal)bytes / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public string CalcularDigitoVerificacion(string Nit)
        {
            try
            {
                string Temp;
                int Contador;
                int Residuo;
                int Acumulador;
                int[] Vector = new int[15];

                Vector[0] = 3;
                Vector[1] = 7;
                Vector[2] = 13;
                Vector[3] = 17;
                Vector[4] = 19;
                Vector[5] = 23;
                Vector[6] = 29;
                Vector[7] = 37;
                Vector[8] = 41;
                Vector[9] = 43;
                Vector[10] = 47;
                Vector[11] = 53;
                Vector[12] = 59;
                Vector[13] = 67;
                Vector[14] = 71;

                Acumulador = 0;

                for (Contador = 0; Contador < Nit.Length; Contador++)
                {
                    Temp = Nit[(Nit.Length - 1) - Contador].ToString();
                    Acumulador = Acumulador + (Convert.ToInt32(Temp) * Vector[Contador]);
                }

                Residuo = Acumulador % 11;

                if (Residuo > 1)
                    return Convert.ToString(11 - Residuo);

                return Residuo.ToString();
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public string ArrayBytesToString(byte[] data)
        {
            try
            {
                if (data != null && data.Length > 0)
                    return Convert.ToBase64String(data);
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en ArrayBytesToString. | " + e.Message);
                return null;
            }
            
        }

        public byte[] StringToArrayBytes(string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data))
                    return null;
                else
                    return Convert.FromBase64String(data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en StringToArrayBytes. | " + e.Message);
                return null;
            }
            

        }

        public string NumeroEnLetras(string num)
        {
            decimal nro;

            try
            {
                nro = decimal.Parse(num, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return "";
            }

            long enteros = Convert.ToInt64(Math.Truncate(nro));
            long decimales = Convert.ToInt64(Math.Round((nro - enteros) * 100, 2));
            return Resultado(enteros, decimales);
        }

        public string NumeroEnLetras(double value)
        {
            long enteros = Convert.ToInt64(Math.Truncate(value));
            long decimales = Convert.ToInt64(Math.Round((value - enteros) * 100, 2));
            return Resultado(enteros, decimales);
        }

        public string NumeroEnLetras(int enteros)
        {
            return Resultado(enteros, 0);
        }

        public string NumeroEnLetras(long enteros)
        {
            return Resultado(enteros, 0);
        }

        public string NumeroEnLetras(decimal value)
        {
            long enteros = Convert.ToInt64(Math.Truncate(value));
            long decimales = Convert.ToInt64(Math.Round((value - enteros) * 100, 2));
            return Resultado(enteros, decimales);
        }

        private string Resultado(long enteros, long decimales)
        {
            string resultado = Conversion(Convert.ToDecimal(enteros)) + " PESOS";
            resultado += " CON " + Conversion(Convert.ToDecimal(decimales)) + " CENTAVOS M/CTE";

            return resultado;
        }

        private string Conversion(decimal value)
        {
            bool valorNegativo = false;
            if (value < 0)
            {
                value = value * -1;
                valorNegativo = true;
            }

            string numero = "";
            value = Math.Truncate(value);
            if (value == 0) numero = "CERO";
            else if (value == 1) numero = "UNO";
            else if (value == 2) numero = "DOS";
            else if (value == 3) numero = "TRES";
            else if (value == 4) numero = "CUATRO";
            else if (value == 5) numero = "CINCO";
            else if (value == 6) numero = "SEIS";
            else if (value == 7) numero = "SIETE";
            else if (value == 8) numero = "OCHO";
            else if (value == 9) numero = "NUEVE";
            else if (value == 10) numero = "DIEZ";
            else if (value == 11) numero = "ONCE";
            else if (value == 12) numero = "DOCE";
            else if (value == 13) numero = "TRECE";
            else if (value == 14) numero = "CATORCE";
            else if (value == 15) numero = "QUINCE";
            else if (value < 20) numero = "DIECI" + Conversion(value - 10);
            else if (value == 20) numero = "VEINTE";
            else if (value < 30) numero = "VEINTI" + Conversion(value - 20);
            else if (value == 30) numero = "TREINTA";
            else if (value == 40) numero = "CUARENTA";
            else if (value == 50) numero = "CINCUENTA";
            else if (value == 60) numero = "SESENTA";
            else if (value == 70) numero = "SETENTA";
            else if (value == 80) numero = "OCHENTA";
            else if (value == 90) numero = "NOVENTA";
            else if (value < 100) numero = Conversion(Math.Truncate(value / 10) * 10) + " Y " + Conversion(value % 10);
            else if (value == 100) numero = "CIEN";
            else if (value < 200) numero = "CIENTO " + Conversion(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) numero = Conversion(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) numero = "QUINIENTOS";
            else if (value == 700) numero = "SETECIENTOS";
            else if (value == 900) numero = "NOVECIENTOS";
            else if (value < 1000) numero = Conversion(Math.Truncate(value / 100) * 100) + " " + Conversion(value % 100);
            else if (value == 1000) numero = "MIL";
            else if (value < 2000) numero = "MIL " + Conversion(value % 1000);
            else if (value < 1000000)
            {
                numero = Conversion(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) numero = numero + " " + Conversion(value % 1000);
            }

            else if (value == 1000000) numero = "UN MILLON";
            else if (value < 2000000) numero = "UN MILLON " + Conversion(value % 1000000);
            else if (value < 1000000000000)
            {
                numero = Conversion(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) numero = numero + " " + Conversion(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) numero = "UN BILLON";
            else if (value < 2000000000000) numero = "UN BILLON " + Conversion(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                numero = Conversion(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) numero = numero + " " + Conversion(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }

            if (valorNegativo)
                numero = $"(-) {numero}";

            return numero;

        }

        public bool EsFechaCorrecta(DateTime fecha)
        {
            if (fecha < DApp.FechaMinima || fecha > DApp.FechaMaxima)
            {
                return false;
            }
            return true;
        }

        public string ObtenerContentTypePorExtension(string extension)
        {
            extension = extension.ToLower();
            if (extension.StartsWith("."))
                extension = extension.Replace(".", "");

            string contentType = "application/octet-stream";

            if (ContentTypes.ContainsKey(extension))
                contentType = ContentTypes[extension];
            return contentType;
        }

        private Dictionary<string, string> ContentTypes = new Dictionary<string, string>
        {
            {"3gp", "video/3gpp"},
            {"7z", "application/x-7z-compressed"},
            {"accdb", "application/msaccess"},
            {"ai", "application/illustrator"},
            {"apk", "application/vnd.android.package-archive"},
            {"arw", "image/x-dcraw"},
            {"avi", "video/x-msvideo"},
            {"bash", "text/x-shellscript"},
            {"bat", "application/x-msdos-program"},
            {"blend", "application/x-blender"},
            {"bin", "application/x-bin"},
            {"bmp", "image/bmp"},
            {"bpg", "image/bpg"},
            {"bz2", "application/x-bzip2"},
            {"cb7", "application/x-cbr"},
            {"cba", "application/x-cbr"},
            {"cbr", "application/x-cbr"},
            {"cbt", "application/x-cbr"},
            {"cbtc", "application/x-cbr"},
            {"cbz", "application/x-cbr"},
            {"cc", "text/x-c"},
            {"cdr", "application/coreldraw"},
            {"class", "application/java"},
            {"cnf", "text/plain"},
            {"conf", "text/plain"},
            {"cpp", "text/x-c++src"},
            {"cr2", "image/x-dcraw"},
            {"css", "text/css"},
            {"csv", "text/csv"},
            {"cvbdl", "application/x-cbr"},
            {"c", "text/x-c"},
            {"c++", "text/x-c++src"},
            {"dcr", "image/x-dcraw"},
            {"deb", "application/x-deb"},
            {"dng", "image/x-dcraw"},
            {"doc", "application/msword"},
            {"docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {"docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {"dot", "application/msword"},
            {"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {"dv", "video/dv"},
            {"eot", "application/vnd.ms-fontobject"},
            {"epub", "application/epub+zip"},
            {"eps", "application/postscript"},
            {"erf", "image/x-dcraw"},
            {"exe", "application/x-ms-dos-executable"},
            {"flac", "audio/flac"},
            {"flv", "video/x-flv"},
            {"gif", "image/gif"},
            {"gpx", "application/gpx+xml"},
            {"gz", "application/gzip"},
            {"gzip", "application/gzip"},
            {"h", "text/x-h"},
            {"heic", "image/heic"},
            {"heif", "image/heif"},
            {"hh", "text/x-h"},
            {"hpp", "text/x-h"},
            {"htaccess", "text/plain"},
            {"ical", "text/calendar"},
            {"ics", "text/calendar"},
            {"iiq", "image/x-dcraw"},
            {"impress", "text/impress"},
            {"java", "text/x-java-source"},
            {"jp2", "image/jp2"},
            {"jpeg", "image/jpeg"},
            {"jpg", "image/jpeg"},
            {"jps", "image/jpeg"},
            {"k25", "image/x-dcraw"},
            {"kdc", "image/x-dcraw"},
            {"key", "application/x-iwork-keynote-sffkey"},
            {"keynote", "application/x-iwork-keynote-sffkey"},
            {"kml", "application/vnd.google-earth.kml+xml"},
            {"kmz", "application/vnd.google-earth.kmz"},
            {"kra", "application/x-krita"},
            {"ldif", "text/x-ldif"},
            {"love", "application/x-love-game"},
            {"lwp", "application/vnd.lotus-wordpro"},
            {"m2t", "video/mp2t"},
            {"m3u", "audio/mpegurl"},
            {"m3u8", "audio/mpegurl"},
            {"m4a", "audio/mp4"},
            {"m4b", "audio/m4b"},
            {"m4v", "video/mp4"},
            {"mdb", "application/msaccess"},
            {"mef", "image/x-dcraw"},
            {"mkv", "video/x-matroska"},
            {"mobi", "application/x-mobipocket-ebook"},
            {"mov", "video/quicktime"},
            {"mp3", "audio/mpeg"},
            {"mp4", "video/mp4"},
            {"mpeg", "video/mpeg"},
            {"mpg", "video/mpeg"},
            {"mpo", "image/jpeg"},
            {"msi", "application/x-msi"},
            {"mts", "video/MP2T"},
            {"mt2s", "video/MP2T"},
            {"nef", "image/x-dcraw"},
            {"numbers", "application/x-iwork-numbers-sffnumbers"},
            {"odf", "application/vnd.oasis.opendocument.formula"},
            {"odg", "application/vnd.oasis.opendocument.graphics"},
            {"odp", "application/vnd.oasis.opendocument.presentation"},
            {"ods", "application/vnd.oasis.opendocument.spreadsheet"},
            {"odt", "application/vnd.oasis.opendocument.text"},
            {"oga", "audio/ogg"},
            {"ogg", "audio/ogg"},
            {"ogv", "video/ogg"},
            {"one", "application/msonenote"},
            {"opus", "audio/ogg"},
            {"orf", "image/x-dcraw"},
            {"otf", "application/font-sfnt"},
            {"pages", "application/x-iwork-pages-sffpages"},
            {"pdf", "application/pdf"},
            {"pfb", "application/x-font"},
            {"pef", "image/x-dcraw"},
            {"php", "application/x-php"},
            {"pl", "application/x-perl"},
            {"pls", "audio/x-scpls"},
            {"png", "image/png"},
            {"pot", "application/vnd.ms-powerpoint"},
            {"potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {"potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {"ppa", "application/vnd.ms-powerpoint"},
            {"ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {"pps", "application/vnd.ms-powerpoint"},
            {"ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {"ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {"ppt", "application/vnd.ms-powerpoint"},
            {"pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {"pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"ps", "application/postscript"},
            {"psd", "application/x-photoshop"},
            {"py", "text/x-python"},
            {"raf", "image/x-dcraw"},
            {"rar", "application/x-rar-compressed"},
            {"reveal", "text/reveal"},
            {"rss", "application/rss+xml"},
            {"rtf", "application/rtf"},
            {"rw2", "image/x-dcraw"},
            {"schema", "text/plain"},
            {"sgf", "application/sgf"},
            {"sh-lib", "text/x-shellscript"},
            {"sh", "text/x-shellscript"},
            {"srf", "image/x-dcraw"},
            {"sr2", "image/x-dcraw"},
            {"tar", "application/x-tar"},
            {"tar.bz2", "application/x-bzip2"},
            {"tar.gz", "application/x-compressed"},
            {"tbz2", "application/x-bzip2"},
            {"tcx", "application/vnd.garmin.tcx+xml"},
            {"tex", "application/x-tex"},
            {"tgz", "application/x-compressed"},
            {"tiff", "image/tiff"},
            {"tif", "image/tiff"},
            {"ttf", "application/font-sfnt"},
            {"txt", "text/plain"},
            {"vcard", "text/vcard"},
            {"vcf", "text/vcard"},
            {"vob", "video/dvd"},
            {"vsd", "application/vnd.visio"},
            {"vsdm", "application/vnd.ms-visio.drawing.macroEnabled.12"},
            {"vsdx", "application/vnd.ms-visio.drawing"},
            {"vssm", "application/vnd.ms-visio.stencil.macroEnabled.12"},
            {"vssx", "application/vnd.ms-visio.stencil"},
            {"vstm", "application/vnd.ms-visio.template.macroEnabled.12"},
            {"vstx", "application/vnd.ms-visio.template"},
            {"wav", "audio/wav"},
            {"webm", "video/webm"},
            {"woff", "application/font-woff"},
            {"wpd", "application/vnd.wordperfect"},
            {"wmv", "video/x-ms-wmv"},
            {"xcf", "application/x-gimp"},
            {"xla", "application/vnd.ms-excel"},
            {"xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {"xls", "application/vnd.ms-excel"},
            {"xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {"xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {"xlt", "application/vnd.ms-excel"},
            {"xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {"xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {"xrf", "image/x-dcraw"},
            {"yaml", "application/yaml"},
            {"yml", "application/yaml"},
            {"zip", "application/zip"},
            {"url", "application/internet-shortcut"},
            {"webloc", "application/internet-shortcut"},
            {"js", "application/javascript"},
            {"json", "application/json"},
            {"fb2", "application/x-fictionbook+xml"},
            {"html", "text/html"},
            {"htm", "text/html"},
            {"svg", "image/svg+xml"},
            {"swf", "application/x-shockwave-flash"},
            {"xml", "application/xml"}
        };
    }

    public class Dian
    {
        public string StatusCertified = "Certified";
        public string StatusStaged = "Staged";
        public string StatusWithErrors = "WithErrors";
        public string StatusBadRequest = "BadRequest";

        public string Currency = "COP";
        public string OperationTypeSSRecaudo = "SS-Recaudo";
        public string OperationTypeSSCUFE = "SS-CUFE";
        public string OtherReference = "OtherReference";
        public string OrderReference = "OrderReference";
        public string InvoiceReference = "InvoiceReference";
        public string QuantityUnitOfMeasure = "NAR";
        public string FeCollectionName = "Usuario";

        public string Reterenta = "RETERENTA";
        public string Reteica = "RETEICA";
        public string CodigoPrestador = "CODIGO_PRESTADOR";
        public string CodeListName = "salud_cobertura.gc";
        public string ModalidadPago = "MODALIDAD_PAGO";
        public string OberturaPlanBeneficios = "OBERTURA_PLAN_BENEFICIOS";
        public string NumeroContrato = "NUMERO_CONTRATO";
        public string NumeroPoliza = "NUMERO_POLIZA";
        public string Copago = "COPAGO";
        public string CuotaModeradora = "CUOTA_MODERADORA";
        public string CuotaRecuperacion = "CUOTA_RECUPERACION";
        public string PagosCompartidos = "PAGOS_COMPARTIDOS";

        public string OperationTypeNotaDebito = "30";
        public string OperationTypeNotaCredito = "20";


    }

    public enum LogType
    {
        Error = 1,
        Info = 2,
        Warning = 3,
    }
}
