using Dominus.Backend.Data;
using Dominus.Backend.DataBase;
using Dominus.Backend.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Dominus.Backend.Application
{
    public static class DApp
    {
        public static string PathDirectoryLogs = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Files", "Logs");

        public static List<Tenant> Tenants { get; set; }

        public static string UrlService { get; set; }
        public static string CloudStorageAccount { get; set; }

        public static bool IsHostDevelopment { get; set; }

        //public static List<DataBaseSetting> DataBaseSettings { get; set; }

        //public static List<Menu> Menus { get; set; }

        public static Language DefaultLanguage { get; set; }

        public static BusinessRule BusinessRules { get; set; }

        private static readonly string ImageFormat = "data:{0};base64,{1}";

        public static Util Util { get; set; } = new Util();
        public static InfoApp InfoApp { get; set; }

        public static DateTime FechaMinima { get; private set; }
        public static DateTime FechaMaxima { get; private set; }

        static DApp()
        {
            try
            {
                IsHostDevelopment = false;
                Tenants = new List<Tenant>();
                //Menus = new List<Menu>();
                DefaultLanguage = new Language { Id = "2", Code = "ESP", Culture = "es", Name = "Español", DateFormat = "dd/MM/yyyy", TimeFormat = "HH:mm", DateTimeFormat = "dd/MM/yyyy HH:mm" };
                BusinessRules = new BusinessRule();
                BusinessRules.Rules = new List<RuleModel>();
                FechaMinima = new DateTime(1800, 1, 1);
                FechaMaxima = new DateTime(2900, 12, 31);
                GetInfoApp();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void GetInfoApp()
        {
            try
            {
                InfoApp = JsonConvert.DeserializeObject<InfoApp>(File.ReadAllText(Path.Combine("Utils", "infoApp.json")));
            }
            catch
            {
                InfoApp = new InfoApp { VersionApp = "00000000", ParcheApp = "0" };
            }
        }

        public static void LogToFile(string text)
        {
            try
            {
                string pathFile = Path.Combine(PathDirectoryLogs, $"ErrorFile{DateTime.Now:yyyyMMdd}.log");
                File.AppendAllText(pathFile, $"{text}{Environment.NewLine}");
            }
            catch
            {

            }
        }

        public static void LoadTenants(IConfiguration conf)
        {
            DApp.Tenants = conf.GetSection("Tenants").Get<List<Tenant>>();
        }

        private static string CleanName(string name)
        {
            return name.Replace("https://", "").Replace("http://", "");
        }

        public static DataBaseSetting GetTenantConnection(string host)
        {
            return Tenants.FirstOrDefault(x => CleanName(x.Name).StartsWith(host)).DataBaseSetting;
        }

        public static string GetTenantService(string host, string service)
        {
            return Tenants.FirstOrDefault(x => CleanName(x.Name).StartsWith(host)).Services[service];
        }

        public static string GetTenantEnvironment(string host)
        {
            return Tenants.FirstOrDefault(x => CleanName(x.Name).StartsWith(host)).Environment;
        }

        public static Tenant GetTenant(string host)
        {
            return Tenants.FirstOrDefault(x => CleanName(x.Name).StartsWith(host));
        }

        public static void LoadRules(string dataDir)
        {
            if (File.Exists(dataDir))
            {
                if (DApp.BusinessRules.Rules == null)
                    DApp.BusinessRules.Rules = new List<RuleModel>();
                DApp.BusinessRules.Rules.AddRange(JsonSerialize.StringToObject<List<RuleModel>>(File.ReadAllText(dataDir)));
            }
        }

        public static string GetResource(string llave)
        {
            return DefaultLanguage.GetResource(llave);
        }

        public static string GetFullDomain(HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/";
        }

        public static string GetStringLengthResource(string resourceKey, int maximumLength)
        {
            return String.Format(DefaultLanguage.GetResource("DLCOLUMNMAXLENGTH"), DefaultLanguage.GetResource(resourceKey), maximumLength);
        }

        public static string GetRequiredResource(string resourceKey)
        {
            return String.Format(DefaultLanguage.GetResource("DLCOLUMNISREQUIRED"), DefaultLanguage.GetResource(resourceKey));
        }

        public static string GetIncorrectResource(string resourceKey)
        {
            return String.Format(DefaultLanguage.GetResource("DLCOLUMNINCORRECT"), DefaultLanguage.GetResource(resourceKey));
        }

        public static bool ActionViewSecurity(HttpContext HttpContext, string UrlAction)
        {
            if (UrlAction.Equals("/") || SecurityNavigation.ControllerExclude.Exists(x => UrlAction.ToUpper().Contains(x.ToUpper())))
                return false;

            //Se quitan los parametros que vienen por url y se obtiene la accion que va a ejecutar
            string rutaAction; ;
            string[] dataUrl = UrlAction.Split("/");
            //string menu = null;
            string menuAction = null;
            if (dataUrl.Count() >= 3)
            {
                //menu = dataUrl[1];
                menuAction = dataUrl[2].Split("?")[0];

                rutaAction = "/" + dataUrl[1] + "/";
                rutaAction += dataUrl[2].Split("?")[0];
            }
            else
                rutaAction = UrlAction;

            Claim CConnectionName = HttpContext.User.FindFirst("ConnectionName");
            Claim CProfilesId = HttpContext.User.FindFirst("ProfilesId");

            //Se evalua si en la cookie estan los parametros asignados
            if (CConnectionName == null || CProfilesId == null)
            {
                throw new System.Exception("El claim en el contexto no esta completo, faltan parametros ConnectionName y ProfilesId");
            }

            string ConnectionName = CConnectionName.Value;

            //Se evalua si la accion existe entre las acciones registradas en la base de datos la cual se carga en el arranque de la api
            var asd = DApp.GetTenantConnection(HttpContext.Request.Host.Value);
            bool existMenuAction = DApp.GetTenantConnection(HttpContext.Request.Host.Value).MenuActionName.Exists(x => x.Equals(menuAction));
            if (!existMenuAction)
                return false;

            //se deserializa los perfiles guadados en la cookie
            List<long> ProfilesId = new List<long>();
            JsonConvert.PopulateObject(CProfilesId.Value, ProfilesId);

            //Se obtiene la seguridad de esta conexion 
            //List<SecurityNavigation> ListSecurityNavigation = DApp.DataBaseSettings.Find(x => x.Name == ConnectionName).ListSecurityNavigation.Where(x => ProfilesId.Contains(x.ProfileId)).ToList();
            List<SecurityNavigation> ListSecurityNavigation = DApp.GetTenantConnection(HttpContext.Request.Host.Value).ListSecurityNavigation.Where(x => ProfilesId.Contains(x.ProfileId)).ToList();

            if (ListSecurityNavigation != null && ListSecurityNavigation.Count > 0)
            {

                //Se evalua la politica de los perfiles (simplemente si existe una politica de permitir todo esta regira sobre la otra)
                bool PoliticaPerfil = true;
                List<bool> Politicas = ListSecurityNavigation.Select(x => x.SecurityPolicy).Distinct().ToList();
                if (Politicas.Count == 1)
                {
                    PoliticaPerfil = Politicas[0];
                }

                //Se mira si existe dicha ruta de navegacion donde la politica sea igual a la evaluada anteriormente
                bool PermisoRuta = ListSecurityNavigation.Exists(x =>
                x.SecurityPolicy == PoliticaPerfil && x.ListNavegation.Exists(j => j.Path.Trim() == rutaAction.Trim()));


                if (PoliticaPerfil == false)
                {
                    return !PermisoRuta;
                }
                else
                {
                    return PermisoRuta;
                }
            }
            else
            {
                Console.WriteLine("El usuario no tiene un perfil asociado.");
                throw new System.Exception(DApp.DefaultLanguage.GetResource("SECURITY.NOESTABLISHED"));
            }

        }

        public static string GetImageSource(string Base64String, string defaultSource = null)
        {
            if (string.IsNullOrWhiteSpace(Base64String))
                return defaultSource ?? "";

            return string.Format(ImageFormat, "image/png", Base64String);
        }
        public static byte[] GetImageBytes(string Base64String)
        {
            return !string.IsNullOrWhiteSpace(Base64String) ? Convert.FromBase64String(Base64String) : null;
        }
        public static string GetImageString(byte[] Base64Bytes)
        {
            return Base64Bytes != null ? Convert.ToBase64String(Base64Bytes) : null;
        }

        public static void UpdateSecurityNavigation(SecurityNavigation seguridad, string host)
        {
            var tenant = DApp.Tenants.Find(x => x.Name.Contains(host));
            if (tenant != null)
            {
                tenant.DataBaseSetting.ListSecurityNavigation.RemoveAll(x => x.ProfileId == seguridad.ProfileId);
                tenant.DataBaseSetting.ListSecurityNavigation.Add(seguridad);
            }
        }
    }
}