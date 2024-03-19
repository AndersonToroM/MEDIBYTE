using Blazor.BusinessLogic.Models;
using Blazor.BusinessLogic.Models.Enums;
using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using Blazor.Reports.Facturas;
using DevExpress.Compression;
using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Backend.Security;
using Dominus.Frontend.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Blazor.BusinessLogic
{
    public class FacturasBusinessLogic : GenericBusinessLogic<Facturas>
    {
        private const string _urlLoginSISPRO = "/api/Auth/LoginSISPRO";
        private const string _urlCargarFevRips = "/api/PaquetesFevRips/CargarFevRips";

        public FacturasBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public FacturasBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        private async Task<RespuestaLoginRips> GetTokenRips(string user, string host)
        {
            var services = DApp.GetTenant(host).Services;
            var urlRips = services[DApp.Util.ServiceRips];
            HttpClient http = new HttpClient();
            http.BaseAddress = new Uri(urlRips);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            var empresa = unitOfWork.Repository<Empresas>().Table.FirstOrDefault();
            var usuario = unitOfWork.Repository<User>().Table
                .Include(x => x.IdentificationType)
                .FirstOrDefault(x => x.UserName == user);

            if (empresa == null)
            {
                throw new Exception($"Empresa no encontrada.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(empresa.NumeroIdentificacion))
                {
                    throw new Exception($"El numero de identificación de la empresa no esta diligenciado en su maestra.");
                }
            }


            if (usuario == null)
            {
                throw new Exception($"Usuario {user} no encontrado.");
            }
            else
            {
                if (usuario.IdentificationType == null || string.IsNullOrWhiteSpace(usuario.IdentificationType.Codigo) || 
                    string.IsNullOrWhiteSpace(usuario.IdentificationNumber) || usuario.IdentificationNumber.Length < 5)
                {
                    throw new Exception($"El usuario {user} no tiene el numero de identificación o el tipo de identificación correctamente diligenciados en su maestro.");
                }
            }

            LoginIntegracionRips loginIntegracionRips = new LoginIntegracionRips();
            loginIntegracionRips.Persona.Identificacion.Tipo = usuario.IdentificationType.Codigo;
            loginIntegracionRips.Persona.Identificacion.Numero = usuario.IdentificationNumber;
            loginIntegracionRips.Nit = empresa.NumeroIdentificacion;
            loginIntegracionRips.Clave = Cryptography.Decrypt(usuario.PasswordRips);
            loginIntegracionRips.Reps = true;

            var jsonLogin = JsonConvert.SerializeObject(loginIntegracionRips, Newtonsoft.Json.Formatting.Indented);
            var content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            var httpResult = await http.PostAsync(_urlLoginSISPRO, content);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            RespuestaLoginRips resultadoLoginRips = new RespuestaLoginRips();
            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                resultadoLoginRips = JsonConvert.DeserializeObject<RespuestaLoginRips>(jsonResult);
            }
            else
            {
                throw new Exception($"Error en LoginSISPRO. Estado: {httpResult.StatusCode.ToString()}. Error: {jsonResult}");
            }

            return resultadoLoginRips;
        }


        public async Task<ResultadoIntegracionRips> EnviarRips(long facturaId, string user, string host)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            var fac = unitOfWork.Repository<Facturas>().Table.FirstOrDefault(x => x.Id == facturaId);

            ResultadoIntegracionRips resultado = new ResultadoIntegracionRips();
            resultado.CreatedBy = user;
            resultado.UpdatedBy = user;
            resultado.CreationDate = DateTime.Now;
            resultado.LastUpdate = DateTime.Now;
            resultado.Tipo = (int)TipoDocumento.Factura;
            resultado.IdTipo = facturaId;
            try
            {
                var token = await GetTokenRips(user, host);

                var services = DApp.GetTenant(host).Services;
                var urlRips = services[DApp.Util.ServiceRips];
                var json = await GetRipsJson(facturaId);

                HttpClient http = new HttpClient();
                http.BaseAddress = new Uri(urlRips);
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResult = await http.PostAsync(_urlCargarFevRips, content);
                var jsonResult = await httpResult.Content.ReadAsStringAsync();
                resultado.HttpStatus = (int)httpResult.StatusCode;
                resultado.JsonResult = jsonResult;
                resultado.HuboError = false;

                if (httpResult.StatusCode == HttpStatusCode.OK) {
                    var ripsResult = JsonConvert.DeserializeObject<RespuestaIntegracionRips>(jsonResult);
                    fac.CodigoUnicoValidacionRips = ripsResult.CodigoUnicoValidacion;
                    fac.FechaRadicacionRips = ripsResult.FechaRadicacion;
                    unitOfWork.Repository<Facturas>().Modify(fac);
                }
            }
            catch (Exception ex)
            {
                resultado.HuboError = true;
                resultado.Error = ex.GetFullErrorMessage();
            }

            unitOfWork.Repository<ResultadoIntegracionRips>().Add(resultado);
            
            return resultado;
        }

        public async Task<ArchivoDescargaModel> DescargarXMLDIAN(long facturaId)
        {
            ArchivoDescargaModel archivoDescargaModel = new ArchivoDescargaModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            var fac = unitOfWork.Repository<Facturas>().Table
                .Include(x=>x.Documentos)
                .FirstOrDefault(x => x.Id == facturaId);

            string nombre = $"{fac.IssueDate:yyyyMMddHHmmss}-JsonRips-{fac.Documentos.Prefijo}{fac.NroConsecutivo}";

            var pathXML = await GetFileXMLDIAN(fac.XmlUrl, archivoDescargaModel.Nombre);

            archivoDescargaModel.Nombre = $"{nombre}.xml";
            archivoDescargaModel.ContentType = "application/json";
            archivoDescargaModel.Archivo = File.ReadAllBytes(pathXML);

            return archivoDescargaModel;
        }

        public ArchivoDescargaModel GetJsonRipsFile(long facturaId)
        {
            ArchivoDescargaModel archivoDescargaModel = new ArchivoDescargaModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            var fac = unitOfWork.Repository<Facturas>().Table
                .Include(x => x.Documentos)
                .FirstOrDefault(x => x.Id == facturaId);

            var query = unitOfWork.Repository<ResultadoIntegracionRips>().Table
                .OrderBy(x => x.Id)
                .Where(x => x.Tipo == (int)TipoDocumento.Factura && x.IdTipo == facturaId);

            if (!string.IsNullOrWhiteSpace(fac.CodigoUnicoValidacionRips) && fac.FechaRadicacionRips.HasValue)
            {
                query.Where(x => x.HttpStatus == (int)HttpStatusCode.OK);
            }

            var resultadoRips = query.OrderBy(x => x.Id).LastOrDefault();

            if (resultadoRips == null)
            {
                throw new Exception("No se ha realizado ninguna validación de los RIPS de esta factura.");
            }

            archivoDescargaModel.Nombre = $"{DateTime.Now:yyyyMMddHHmmss}-JsonRips-{fac.Documentos.Prefijo}{fac.NroConsecutivo}.json";
            archivoDescargaModel.ContentType = "application/json";
            archivoDescargaModel.Archivo = Encoding.ASCII.GetBytes(resultadoRips.JsonResult);

            return archivoDescargaModel;
        }

        /// <summary>
        /// https://localhost:44333/empresas/ObtenerJsonRips?id=68282
        /// </summary>
        /// <param name="idFactura"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetRipsJson(long facturaId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var fac = unitOfWork.Repository<Facturas>().Table
                .Include(x => x.Empresas)
                .Include(x => x.Documentos)
                .FirstOrDefault(x => x.Id == facturaId);

            if (fac == null || fac.PacientesId.HasValue)
            {
                throw new Exception("La factura no es de tipo institucional.");
            }

            if (string.IsNullOrWhiteSpace(fac.XmlUrl))
            {
                throw new Exception("La factura no tiene registro en la dian o no se le ha generado XML.");
            }

            var facturasDetalles = unitOfWork.Repository<FacturasDetalles>().Table
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Empleados)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Empleados.TiposIdentificacion)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.FinalidadConsulta)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.ZonaTerritorialResidencia)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.TiposIdentificacion)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.Generos)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.Ciudades.Departamentos.Paises)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Diagnosticos)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ModalidadAtencion)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora.Documentos)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.CoberturaPlanBeneficios)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ViaIngresoServicioSalud)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ModalidadAtencion)
                .Include(x => x.Servicios.Cups)
                .Include(x => x.Servicios.HabilitacionServciosRips)
                .Include(x => x.Servicios.GrupoServciosRips)
                .Include(x => x.Facturas)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.PaisesOrigen)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstados)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.FinalidadProcedimiento)

                .Where(x => x.FacturasId == fac.Id)
                .ToList();

            if (facturasDetalles == null || !facturasDetalles.Any())
            {
                throw new Exception("La factura no tiene servicios facturados.");
            }

            var admisiones = facturasDetalles.Select(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones).Distinct().ToList();

            RipsRootJson ripsRootJson = new RipsRootJson();

            var xmlfile = await GetFileXMLDIAN(fac.XmlUrl, $"{DateTime.Now:yyyyMMddHHmmssfff}");
            ripsRootJson.XmlFevFile = Convert.ToBase64String(File.ReadAllBytes(xmlfile));

            ripsRootJson.Rips.NumDocumentoIdObligado = fac.Empresas.NumeroIdentificacion;
            ripsRootJson.Rips.NumFactura = $"{fac.Documentos.Prefijo}{fac.NroConsecutivo}";

            int consecutivoUsuario = 1;
            foreach (var admision in admisiones)
            {
                UsuarioRips usuarioRips = new UsuarioRips();
                usuarioRips.Consecutivo = consecutivoUsuario;
                usuarioRips.TipoDocumentoIdentificacion = admision.Pacientes.TiposIdentificacion.Codigo;
                usuarioRips.NumDocumentoIdentificacion = admision.Pacientes.NumeroIdentificacion;
                usuarioRips.TipoUsuario = admision.Pacientes.NumeroIdentificacion;
                usuarioRips.FechaNacimiento = admision.Pacientes.FechaNacimiento.ToString("yyyy-MM-dd");
                usuarioRips.CodPaisOrigen = admision.Pacientes.PaisesOrigen?.CodigoISO3166Num;
                usuarioRips.CodSexo = admision.Pacientes.Generos?.Codigo;
                usuarioRips.CodPaisResidencia = admision.Pacientes.Ciudades?.Departamentos?.Paises?.CodigoISO3166Num;
                usuarioRips.CodMunicipioResidencia = admision.Pacientes.Ciudades?.Codigo;
                usuarioRips.CodZonaTerritorialResidencia = admision.Pacientes.ZonaTerritorialResidencia?.Codigo;
                usuarioRips.TipoUsuario = admision.CoberturaPlanBeneficios.CodigoRips;

                var tieneIncapacidad = unitOfWork.Repository<Incapacidades>().Table.Any(a => a.HIstoriasClinicas.AtencionesId == admision.Atenciones.Id);
                usuarioRips.Incapacidad = tieneIncapacidad ? "SI" : "NO";

                int consecutivoProcedimiento = 1;
                var serviciosFacturados = facturasDetalles.Where(x => x.AdmisionesServiciosPrestados.Atenciones.AdmisionesId == admision.Id).ToList();
                foreach (var servicio in serviciosFacturados)
                {
                    ProcedimientoRips procedimientoRips = new ProcedimientoRips();
                    procedimientoRips.Consecutivo = consecutivoProcedimiento;
                    procedimientoRips.CodPrestador = fac.Empresas?.CodigoReps;
                    procedimientoRips.CodComplicacion = null;
                    procedimientoRips.CodDiagnosticoPrincipal = servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones?.Diagnosticos?.Codigo;
                    procedimientoRips.CodDiagnosticoRelacionado = null;
                    if(servicio.Servicios.CupsId != null)
                    {
                        procedimientoRips.CodProcedimiento = servicio.Servicios?.Cups?.Codigo;
                    }else
                    {
                        procedimientoRips.CodProcedimiento = servicio.Servicios?.Codigo;
                    }
                    procedimientoRips.CodServicio = servicio.Servicios?.HabilitacionServciosRips?.Codigo;
                    procedimientoRips.TipoPagoModerador = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.ValorPagoEstados?.CodigoRips;
                    procedimientoRips.FechaInicioAtencion = servicio.AdmisionesServiciosPrestados.Atenciones?.FechaAtencion.ToString("yyyy-MM-dd HH:mm");
                    procedimientoRips.FinalidadTecnologiaSalud = servicio.AdmisionesServiciosPrestados.Atenciones?.FinalidadProcedimiento?.CodigoRips;
                    procedimientoRips.GrupoServicios = servicio.Servicios?.GrupoServciosRips?.Codigo;
                    procedimientoRips.IdMIPRES = null;
                    procedimientoRips.ModalidadGrupoServicioTecSal = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.ModalidadAtencion?.Codigo;
                    procedimientoRips.NumAutorizacion = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.NroAutorizacion;
                    procedimientoRips.NumDocumentoIdentificacion = servicio.AdmisionesServiciosPrestados.Atenciones?.Empleados?.NumeroIdentificacion;
                    procedimientoRips.TipoDocumentoIdentificacion = servicio.AdmisionesServiciosPrestados.Atenciones.Empleados?.TiposIdentificacion?.Codigo;
                    procedimientoRips.ValorPagoModerador = Convert.ToInt32(servicio.Facturas.ValorTotal);
                    procedimientoRips.ViaIngresoServicioSalud = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.ViaIngresoServicioSalud?.Codigo;
                    procedimientoRips.VrServicio = Convert.ToInt32(servicio.ValorServicio);
                    procedimientoRips.NumFEVPagoModerador = null;

                    if (servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 58 || servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 59)
                    {
                        var facturaCopagoCuotaModeradora = servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora;
                        procedimientoRips.NumFEVPagoModerador = $"{facturaCopagoCuotaModeradora.Documentos.Prefijo}{facturaCopagoCuotaModeradora.NroConsecutivo}";
                    }

                    consecutivoProcedimiento++;
                    usuarioRips.Servicios.Procedimientos.Add(procedimientoRips);

                }

                consecutivoUsuario++;
                ripsRootJson.Rips.Usuarios.Add(usuarioRips);
            }

            return JsonConvert.SerializeObject(ripsRootJson, Newtonsoft.Json.Formatting.Indented);
        }

        private string GetPdfFacturaReporte(Facturas factura, string nameFile, string user)
        {
            XtraReport xtraReport = null;
            if (factura.AdmisionesId != null)
            {
                xtraReport = ReportExtentions.Report<FacturasParticularReporte>(this.BusinessLogic, factura.Id, user);
            }
            else
            {
                xtraReport = ReportExtentions.Report<FacturasReporte>(this.BusinessLogic, factura.Id, user);
            }

            string pathPdf = Path.Combine(Path.GetTempPath(), $"fv{nameFile}.pdf");
            PdfExportOptions pdfOptions = new PdfExportOptions();
            pdfOptions.ConvertImagesToJpeg = false;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;
            pdfOptions.PdfACompatibility = PdfACompatibility.PdfA2b;
            xtraReport.ExportToPdf(pathPdf, pdfOptions);
            return pathPdf;
        }

        private async Task<string> GetFileXMLDIAN(string xmlUrl, string nameFile)
        {
            byte[] contentarray = null;
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(xmlUrl);
            if (response.IsSuccessStatusCode)
                contentarray = await response.Content.ReadAsByteArrayAsync();
            else
                throw new Exception($"Error en descargar XMl desde acepta. | {response.StatusCode} - {response.ReasonPhrase}");
            string content = Encoding.UTF8.GetString(contentarray);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            content = @"<?xml version='1.0' encoding='UTF-8'?>";
            content += doc.DocumentElement.ChildNodes[3].InnerXml;

            string pathXml = Path.Combine(Path.GetTempPath(), $"ad{nameFile}.xml");
            File.WriteAllText(pathXml, content, Encoding.UTF8);
            return pathXml;
        }

        public async Task EnviarEmail(long facturaId, string eventoEnvio, string user)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var factura = unitOfWork.Repository<Facturas>().FindById(x => x.Id == facturaId, true);

            if (string.IsNullOrWhiteSpace(factura.DIANResponse))
            {
                throw new Exception("La factura no ha sido aceptada por la dian.");
            }
            else if (!factura.DIANResponse.Contains("DIAN Aceptado"))
            {
                throw new Exception("La factura no ha sido aceptada por la dian.");
            }

            try
            {
                string correo = null;
                if (factura.AdmisionesId != null)
                    correo = unitOfWork.Repository<Pacientes>().GetTable().FirstOrDefault(x => x.Id == factura.PacientesId)?.CorreoElectronico;
                else
                    correo = unitOfWork.Repository<Entidades>().GetTable().FirstOrDefault(x => x.Id == factura.EntidadesId)?.CorreoElectronico;

                string consecutivoEnvioFE = factura.ConsecutivoFE;
                if (string.IsNullOrWhiteSpace(consecutivoEnvioFE))
                {
                    consecutivoEnvioFE = new GenericBusinessLogic<Facturas>(unitOfWork).GetConsecutivoParaEnvioFE();
                    factura.ConsecutivoFE = consecutivoEnvioFE;
                    unitOfWork.Repository<Facturas>().Modify(factura);
                }

                string pathXml = await GetFileXMLDIAN(factura.XmlUrl, consecutivoEnvioFE);

                ZipArchive archive = new ZipArchive();
                archive.FileName = $"z{consecutivoEnvioFE}.zip";
                archive.AddFile(GetPdfFacturaReporte(factura, consecutivoEnvioFE, user), "/");
                archive.AddFile(pathXml, "/");
                MemoryStream msZip = new MemoryStream();
                archive.Save(msZip);
                msZip = new MemoryStream(msZip.ToArray());

                Empresas empresas = unitOfWork.Repository<Empresas>().FindById(x => x.Id == factura.EmpresasId, false);

                EmailModelConfig envioEmailConfig = new EmailModelConfig();
                envioEmailConfig.Origen = DApp.Util.EmailOrigen_Facturacion;
                envioEmailConfig.Asunto = $"{factura.Empresas.NumeroIdentificacion};{factura.Empresas.RazonSocial};{factura.Documentos.Prefijo}{factura.NroConsecutivo};01;{factura.Empresas.RazonSocial}";
                envioEmailConfig.MetodoUso = eventoEnvio;
                envioEmailConfig.Template = "EmailEnvioFacturaElectronica";
                envioEmailConfig.Destinatarios.Add(correo);
                envioEmailConfig.ArchivosAdjuntos.Add($"z{consecutivoEnvioFE}.zip", msZip);
                envioEmailConfig.Datos = new Dictionary<string, string>
                {
                    {"nombreCia",$"{empresas.RazonSocial}" }
                };

                new ConfiguracionEnvioEmailBusinessLogic(this.UnitOfWork).EnviarEmail(envioEmailConfig);

                var job = unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Table
                    .FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Factura && x.IdTipo == factura.Id && !x.Exitoso);
                if (job != null)
                {
                    job.Ejecutado = true;
                    job.Exitoso = true;
                    job.LastUpdate = DateTime.Now;
                    job.UpdatedBy = user;
                    job.Intentos++;
                    job.Detalle += $"Intento {job.Intentos}: {eventoEnvio}. ";
                    unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Modify(job);
                }
            }
            catch (Exception e)
            {
                DApp.LogToFile($"{e.GetFullErrorMessage()} | {e.StackTrace}");
                throw new Exception(e.GetFullErrorMessage());
            }
        }

        public void DisminuirSaldo(long idFactura, decimal valor)
        {
            var factura = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<Facturas>().FindById(x => x.Id == idFactura, false);
            factura.Saldo = factura.Saldo - valor;
            new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<Facturas>().Modify(factura);

        }

        public string FacturaIndividual(long admisionId, long empresaId, long userId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            Admisiones admision = unitOfWork.Repository<Admisiones>().Table
                .Include(x => x.FormasPagos)
                .Include(x => x.FacturaCopagoCuotaModeradora)
                .Include(x => x.FacturaCopagoCuotaModeradora.Documentos)
                .Include(x => x.ProgramacionCitas)
                .FirstOrDefault(x => x.Id == admisionId);

            User usuario = unitOfWork.Repository<User>().FindById(x => x.Id == userId, false);
            CiclosCajas cicloCaja = unitOfWork.Repository<CiclosCajas>().FindById(x => x.OpenUsersId == userId && x.CloseUsersId == null, false);

            if (admision.Facturado)
                throw new Exception($"Esta admision ya fue facturada. Factura {admision.FacturaCopagoCuotaModeradora?.Documentos?.Prefijo}-{admision.FacturaCopagoCuotaModeradora?.NroConsecutivo}");

            if (admision.FormasPagosId == null)
                throw new Exception($"La forma de pago es obligatoria.");
            if (admision.MedioPagosId == null)
                throw new Exception($"El medio de pago es obligatorio.");
            if (admision.DocumentosId == null)
                throw new Exception($"El documento es obligatorio.");

            if (admision.FormasPagos.Codigo == "1")
            {
                if (cicloCaja == null)
                {
                    throw new Exception($"No existe un ciclo de caja abierto para el usuario {usuario.NombreCompleto}.");
                }
            }

            if (admision.ValorPagoEstadosId == 10067)
            {
                return FacturaIndividualParticular(admision, empresaId, cicloCaja.Id, usuario.UserName, unitOfWork);
            }
            else if (admision.ValorPagoEstadosId == 58 || admision.ValorPagoEstadosId == 59 || admision.ValorPagoEstadosId == 68 | admision.ValorPagoEstadosId == 69)
            {
                return FacturaIndividualCopagoCuotaModeradora(admision, empresaId, cicloCaja.Id, usuario.UserName, unitOfWork);
            }
            else
            {
                throw new Exception($"Solo se permite facturar valores de COPAGO, CUOTA MODERADORA, CUOTA DE RECUPERACIÓN, PAGOS COMPARTIDOS o PARTICULAR.");
            }

        }

        private string FacturaIndividualCopagoCuotaModeradora(Admisiones admision, long empresaId, long cicloCajaId, string userName, BlazorUnitWork unitOfWork)
        {
            try
            {
                unitOfWork.BeginTransaction();

                if (admision.ValorCopago <= 0)
                {
                    throw new Exception($"El valor a facturar de COPAGO o CUOTA MODERADORA deben ser superiores a cero (0)");
                }

                Facturas factura = new Facturas();
                factura.EmpresasId = empresaId;
                factura.SedesId = admision.ProgramacionCitas.SedesId;
                factura.PacientesId = admision.PacientesId;
                factura.EntidadesId = null;
                factura.ConvenioId = null;
                factura.OrdenCompra = null;
                factura.ReferenciaFactura = null;
                factura.EsCopagoModeradora = true;
                factura.AdmisionesId = admision.Id;
                factura.Fecha = DateTime.Now;
                factura.DocumentosId = admision.DocumentosId.GetValueOrDefault(0);

                var documento = unitOfWork.Repository<Documentos>().FindById(x => x.Id == factura.DocumentosId, false);
                long consecutivo = 0;
                try
                {
                    consecutivo = new GenericBusinessLogic<Documentos>(unitOfWork).GetSecuence($"{documento.Prefijo}");
                }
                catch (Exception e)
                {
                    throw new Exception($"Error obteniendo consecutivo para {factura.SedesId}-{documento.Prefijo}. | {e.Message}");
                }

                if (documento.ResolucionDian != 0)
                {
                    if (consecutivo < documento.ConsecutivoDesde || consecutivo > documento.ConsecutivoHasta)
                    {
                        throw new Exception($"El consecutivo supero la resolucion de la DIAN del documento {documento.Prefijo}.");
                    }
                    if (DateTime.Now < documento.FechaDesde || DateTime.Now > documento.FechaHasta)
                    {
                        throw new Exception($"La fecha supero la resolucion de la DIAN del documento {documento.Prefijo}.");
                    }
                }

                factura.FormasPagosId = admision.FormasPagosId.GetValueOrDefault(0);
                factura.Observaciones = admision.ObservacionFactura;
                factura.NroConsecutivo = consecutivo;
                factura.LastUpdate = DateTime.Now;
                factura.UpdatedBy = userName;
                factura.CreationDate = DateTime.Now;
                factura.CreatedBy = userName;

                factura.ValorCopagoCuotaModeradora = 0;
                factura.ValorSubtotal = admision.ValorCopago;
                factura.ValorDescuentos = 0;
                factura.ValorImpuestos = 0;
                factura.ValorTotal = admision.ValorCopago;
                factura.Saldo = 0;
                factura.FehaInicial = DateTime.Now;
                factura.FechaFinal = DateTime.Now;
                factura.MontoEscrito = DApp.Util.NumeroEnLetras(factura.ValorTotal);
                if (admision.FormasPagos.Codigo == "1")
                    factura.Estadosid = 16;
                else
                    factura.Estadosid = 14;

                factura.MediosPagoId = admision.MedioPagosId;

                if (factura.ValorTotal < 0)
                {
                    throw new Exception("Valor total a pagar es negativo. Por favor verificar saldos.");
                }

                var newFact = unitOfWork.Repository<Facturas>().Add(factura);

                //Detalle si es copago o cuota
                if (!admision.Facturado)
                {
                    FacturasDetalles facturasDetalles = new FacturasDetalles();
                    facturasDetalles.Id = 0;
                    facturasDetalles.LastUpdate = DateTime.Now;
                    facturasDetalles.UpdatedBy = userName;
                    facturasDetalles.CreationDate = DateTime.Now;
                    facturasDetalles.CreatedBy = userName;
                    facturasDetalles.FacturasId = newFact.Id;
                    facturasDetalles.NroLinea = 1;
                    facturasDetalles.Cantidad = 1;
                    facturasDetalles.AdmisionesServiciosPrestadosId = null;
                    facturasDetalles.PorcDescuento = 0;
                    facturasDetalles.PorcImpuesto = 0;
                    facturasDetalles.ValorServicio = admision.ValorCopago;
                    facturasDetalles.SubTotal = facturasDetalles.Cantidad * facturasDetalles.ValorServicio;
                    facturasDetalles.ValorTotal = facturasDetalles.ValorServicio;

                    if (admision.ValorPagoEstadosId == 58)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("COP"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado COPAGO con codigo COP.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else if (admision.ValorPagoEstadosId == 59)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("CM"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado CUOTA MODERADORA con codigo CM.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else if (admision.ValorPagoEstadosId == 68)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("CR"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado CUOTA DE RECUPERACIÓN con codigo CR.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else if (admision.ValorPagoEstadosId == 69)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("PC"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado PAGOS COMPARTIDOS con codigo PC.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else
                    {
                        throw new Exception($"Admisión sin estado de pago registrado en el sistema. Comuniquese con el administrador.");
                    }

                    unitOfWork.Repository<FacturasDetalles>().Add(facturasDetalles);
                }

                if (admision.FormasPagos.Codigo == "1")
                {
                    Recaudos recaudos = new Recaudos();
                    recaudos.Id = 0;
                    try
                    {
                        recaudos.Consecutivo = unitOfWork.Repository<Recaudos>().Table.Max(x => x.Consecutivo) + 1;
                    }
                    catch
                    {
                        recaudos.Consecutivo = 1;
                    }
                    recaudos.IsNew = true;
                    recaudos.LastUpdate = DateTime.Now;
                    recaudos.UpdatedBy = userName;
                    recaudos.CreationDate = DateTime.Now;
                    recaudos.CreatedBy = userName;

                    recaudos.FechaRecaudo = DateTime.Now;
                    recaudos.CiclosCajasId = cicloCajaId;
                    recaudos.MediosPagoId = admision.MedioPagosId.GetValueOrDefault(0);
                    recaudos.EmpresasId = empresaId;
                    recaudos.PacientesId = factura.PacientesId;
                    recaudos.EntidadesId = factura.EntidadesId;
                    recaudos.SedesId = factura.SedesId;
                    recaudos.ValorTotalRecibido = factura.ValorTotal;
                    recaudos = unitOfWork.Repository<Recaudos>().Add(recaudos);

                    RecaudosDetalles recaudosDetalles = new RecaudosDetalles();
                    recaudosDetalles.Id = 0;
                    recaudosDetalles.IsNew = true;
                    recaudosDetalles.LastUpdate = DateTime.Now;
                    recaudosDetalles.UpdatedBy = userName;
                    recaudosDetalles.CreationDate = DateTime.Now;
                    recaudosDetalles.CreatedBy = userName;

                    recaudosDetalles.RecaudosId = recaudos.Id;
                    recaudosDetalles.ValorAplicado = factura.ValorTotal;
                    recaudosDetalles.ValorReteIca = 0;
                    recaudosDetalles.ValorRetencion = 0;
                    recaudosDetalles.FacturasId = factura.Id;
                    recaudosDetalles = unitOfWork.Repository<RecaudosDetalles>().Add(recaudosDetalles);
                }

                admision.Facturado = true;
                admision.FacturaCopagoCuotaModeradoraId = newFact.Id;
                admision = unitOfWork.Repository<Admisiones>().Modify(admision);

                unitOfWork.CommitTransaction();
                return $"{documento.Prefijo}-{newFact.NroConsecutivo}";
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw (e);
            }
        }

        private string FacturaIndividualParticular(Admisiones admision, long empresaId, long cicloCajaId, string userName, BlazorUnitWork unitOfWork)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var serviciosAFacturar = unitOfWork.Repository<ServiciosFacturar>().Table.Where(x => x.AdmisionesId == admision.Id).ToList();
                if (serviciosAFacturar == null)
                {
                    throw new Exception($"No existen servicios a facturar en esa admisión.");
                }

                if (serviciosAFacturar.Exists(x => x.AdmisionFacturada))
                {
                    throw new Exception($"Esta admisión ya fue facturada.");
                }

                Facturas factura = new Facturas();
                factura.EmpresasId = empresaId;
                factura.SedesId = admision.ProgramacionCitas.SedesId;
                factura.PacientesId = admision.PacientesId;
                factura.EntidadesId = null;
                factura.ConvenioId = null;
                factura.OrdenCompra = null;
                factura.ReferenciaFactura = null;
                factura.EsCopagoModeradora = false;
                factura.AdmisionesId = admision.Id;
                factura.Fecha = DateTime.Now;
                factura.DocumentosId = admision.DocumentosId.GetValueOrDefault(0);

                var documento = unitOfWork.Repository<Documentos>().FindById(x => x.Id == factura.DocumentosId, false);
                long consecutivo = 0;
                try
                {
                    consecutivo = new GenericBusinessLogic<Documentos>(unitOfWork).GetSecuence($"{documento.Prefijo}");
                }
                catch (Exception e)
                {
                    throw new Exception($"Error obteniendo consecutivo para {factura.SedesId}-{documento.Prefijo}. | {e.Message}");
                }

                if (documento.ResolucionDian != 0)
                {
                    if (consecutivo < documento.ConsecutivoDesde || consecutivo > documento.ConsecutivoHasta)
                    {
                        throw new Exception($"El consecutivo supero la resolucion de la DIAN del documento {documento.Prefijo}.");
                    }
                    if (DateTime.Now < documento.FechaDesde || DateTime.Now > documento.FechaHasta)
                    {
                        throw new Exception($"La fecha supero la resolucion de la DIAN del documento {documento.Prefijo}.");
                    }
                }

                factura.FormasPagosId = admision.FormasPagosId.GetValueOrDefault(0);
                factura.Observaciones = admision.ObservacionFactura;
                factura.NroConsecutivo = consecutivo;
                factura.LastUpdate = DateTime.Now;
                factura.UpdatedBy = userName;
                factura.CreationDate = DateTime.Now;
                factura.CreatedBy = userName;
                factura.ServiciosAfacturar.AddRange(serviciosAFacturar);

                factura.ValorCopagoCuotaModeradora = 0;
                factura.ValorSubtotal = factura.ServiciosAfacturar.Sum(x => x.SubTotal);
                factura.ValorDescuentos = factura.ServiciosAfacturar.Sum(x => x.ValorDescuento);
                factura.ValorImpuestos = factura.ServiciosAfacturar.Sum(x => x.ValorImpuesto);
                factura.ValorTotal = ((factura.ValorSubtotal - factura.ValorDescuentos) + factura.ValorImpuestos) - factura.ValorCopagoCuotaModeradora;
                factura.Saldo = 0;
                factura.FehaInicial = DateTime.Now;
                factura.FechaFinal = DateTime.Now;
                factura.MontoEscrito = DApp.Util.NumeroEnLetras(factura.ValorTotal);

                if (admision.FormasPagos.Codigo == "1")
                    factura.Estadosid = 16;
                else
                    factura.Estadosid = 14;

                factura.MediosPagoId = admision.MedioPagosId;
                var newFact = unitOfWork.Repository<Facturas>().Add(factura);

                List<long> idsServ = factura.ServiciosAfacturar.Select(x => x.Id).ToList();
                factura.ServicioFacturados = unitOfWork.Repository<AdmisionesServiciosPrestados>().FindAll(x => idsServ.Contains(x.Id), false);
                if (factura.ServicioFacturados != null && factura.ServicioFacturados.Count > 0)
                {
                    for (int i = 0; i < factura.ServicioFacturados.Count; i++)
                    {
                        if (!admision.Facturado)
                        {
                            FacturasDetalles facturasDetalles = new FacturasDetalles();
                            facturasDetalles.Id = 0;
                            facturasDetalles.LastUpdate = DateTime.Now;
                            facturasDetalles.UpdatedBy = userName;
                            facturasDetalles.CreationDate = DateTime.Now;
                            facturasDetalles.CreatedBy = userName;
                            facturasDetalles.FacturasId = newFact.Id;
                            facturasDetalles.NroLinea = i + 1;
                            facturasDetalles.AdmisionesServiciosPrestadosId = factura.ServicioFacturados[i].Id;
                            facturasDetalles.ServiciosId = factura.ServicioFacturados[i].ServiciosId;
                            facturasDetalles.Cantidad = factura.ServicioFacturados[i].Cantidad;
                            facturasDetalles.PorcDescuento = admision.PorcDescAutorizado;
                            facturasDetalles.PorcImpuesto = 0;
                            facturasDetalles.ValorServicio = factura.ServicioFacturados[i].ValorServicio;
                            facturasDetalles.SubTotal = facturasDetalles.Cantidad * facturasDetalles.ValorServicio;
                            facturasDetalles.ValorTotal = facturasDetalles.SubTotal - (factura.ServicioFacturados[i].ValorServicio * (facturasDetalles.PorcDescuento / 100));
                            unitOfWork.Repository<FacturasDetalles>().Add(facturasDetalles);
                        }
                        factura.ServicioFacturados[i].FacturasGeneracionId = null;
                        factura.ServicioFacturados[i].FacturasId = newFact.Id;
                        factura.ServicioFacturados[i].Facturado = true;
                        unitOfWork.Repository<AdmisionesServiciosPrestados>().Modify(factura.ServicioFacturados[i]);
                    }
                }

                if (admision.FormasPagos.Codigo == "1")
                {
                    Recaudos recaudos = new Recaudos();
                    recaudos.Id = 0;
                    try
                    {
                        recaudos.Consecutivo = unitOfWork.Repository<Recaudos>().Table.Max(x => x.Consecutivo) + 1;
                    }
                    catch
                    {
                        recaudos.Consecutivo = 1;
                    }
                    recaudos.IsNew = true;
                    recaudos.LastUpdate = DateTime.Now;
                    recaudos.UpdatedBy = userName;
                    recaudos.CreationDate = DateTime.Now;
                    recaudos.CreatedBy = userName;

                    recaudos.FechaRecaudo = DateTime.Now;
                    recaudos.CiclosCajasId = cicloCajaId;
                    recaudos.MediosPagoId = admision.MedioPagosId.GetValueOrDefault(0);
                    recaudos.EmpresasId = empresaId;
                    recaudos.PacientesId = factura.PacientesId;
                    recaudos.EntidadesId = factura.EntidadesId;
                    recaudos.SedesId = factura.SedesId;
                    recaudos.ValorTotalRecibido = factura.ValorTotal;

                    if (factura.ValorTotal < 0)
                    {
                        throw new Exception("Valor total a pagar es negativo. Por favor verificar saldos.");
                    }

                    recaudos = unitOfWork.Repository<Recaudos>().Add(recaudos);

                    RecaudosDetalles recaudosDetalles = new RecaudosDetalles();
                    recaudosDetalles.Id = 0;
                    recaudosDetalles.IsNew = true;
                    recaudosDetalles.LastUpdate = DateTime.Now;
                    recaudosDetalles.UpdatedBy = userName;
                    recaudosDetalles.CreationDate = DateTime.Now;
                    recaudosDetalles.CreatedBy = userName;

                    recaudosDetalles.RecaudosId = recaudos.Id;
                    recaudosDetalles.ValorAplicado = factura.ValorTotal;
                    recaudosDetalles.ValorReteIca = 0;
                    recaudosDetalles.ValorRetencion = 0;
                    recaudosDetalles.FacturasId = factura.Id;
                    recaudosDetalles = unitOfWork.Repository<RecaudosDetalles>().Add(recaudosDetalles);
                }

                admision.Facturado = true;
                admision.FacturaCopagoCuotaModeradoraId = newFact.Id;
                admision = unitOfWork.Repository<Admisiones>().Modify(admision);

                unitOfWork.CommitTransaction();
                return $"{documento.Prefijo}-{newFact.NroConsecutivo}";
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw (e);
            }
        }

        /// <summary>
        /// https://localhost:44333/empresas/ObtenerXMLFactura?id=74131
        /// </summary>
        /// <param name="idFactura"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public EInvoice GetInvoice(long idFactura)
        {
            Facturas factura = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<Facturas>()
                .Tabla().Include(x => x.Empresas)
                .Include(x => x.Empresas.TiposRegimenContable)
                .Include(x => x.Empresas.TiposPersonas)
                .Include(x => x.Empresas.TiposIdentificacion)
                .Include(x => x.Empresas.Ciudades)
                .Include(x => x.Empresas.Ciudades.Departamentos)
                .Include(x => x.Empresas.Ciudades.Departamentos.Paises)
                .Include(x => x.Convenio)
                .Include(x => x.Sedes)
                .Include(x => x.Sedes.Ciudades)
                .Include(x => x.Sedes.Ciudades.Departamentos)
                .Include(x => x.Sedes.Ciudades.Departamentos.Paises)
                .Include(x => x.Pacientes)
                .Include(x => x.Pacientes.TiposIdentificacion)
                .Include(x => x.Pacientes.Ciudades)
                .Include(x => x.Pacientes.Ciudades.Departamentos)
                .Include(x => x.Pacientes.Ciudades.Departamentos.Paises)
                .Include(x => x.Entidades)
                .Include(x => x.Entidades.Ciudades)
                .Include(x => x.Entidades.TiposPersonas)
                .Include(x => x.Entidades.TiposIdentificacion)
                .Include(x => x.Entidades.Ciudades.Departamentos)
                .Include(x => x.Entidades.Ciudades.Departamentos.Paises)
                .Include(x => x.Documentos)
                .FirstOrDefault(x => x.Id == idFactura);
            factura.FacturasDetalles = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<FacturasDetalles>().Tabla()
                .Include(x => x.Servicios)
                .Include(x => x.Servicios.Cups)
                .Include(x => x.Servicios.TiposImpuestos)

                .Where(x => x.FacturasId == idFactura).ToList();

            List<Admisiones> admisiones = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<AdmisionesServiciosPrestados>().Tabla()
                    .Include(x => x.Admisiones.TiposUsuarios)
                    .Include(x => x.Admisiones.Pacientes)
                    .Include(x => x.Admisiones.Pacientes.TiposIdentificacion)
                    .Include(x => x.Admisiones.Convenios)
                    .Include(x => x.Admisiones.Convenios.ModalidadesContratacion)
                    .Include(x => x.Admisiones.CoberturaPlanBeneficios)
                    .Include(x => x.Admisiones.ValorPagoEstados)
                    .Where(x => x.FacturasId == idFactura).Select(x => x.Admisiones).Distinct().ToList();


            TaxScheme taxSchemeClient = null;

            if (factura.Empresas != null)
            {
                factura.Empresas.EmpresasEsquemasImpuestos = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<EmpresasEsquemasImpuestos>().FindAll(x => x.EmpresasId == factura.EmpresasId, true);
                factura.Empresas.EmpresasResponsabilidadesFiscales = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<EmpresasResponsabilidadesFiscales>().FindAll(x => x.EmpresasId == factura.EmpresasId, true);
            }

            if (factura.Entidades != null)
            {
                factura.Entidades.EntidadesEsquemasImpuestos = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<EntidadesEsquemasImpuestos>().FindAll(x => x.EntidadesId == factura.EntidadesId, true);
                factura.Entidades.EntidadesResponsabilidadesFiscales = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<EntidadesResponsabilidadesFiscales>().FindAll(x => x.EntidadesId == factura.EntidadesId, true);
                taxSchemeClient = new TaxScheme { ID = factura.Entidades.GetCodigoImpuestoRecaudados(), Name = factura.Entidades.GetNombreImpuestoRecaudados() };
            }

            List<TotalByTaxScheme> InvoiceDetailsByTax = factura.FacturasDetalles.GroupBy(x => new { x.Servicios.TiposImpuestos.Codigo, x.PorcImpuesto }).Select(g => new TotalByTaxScheme { TaxSchemeCode = g.Key.Codigo, TaxPercentaje = g.Key.PorcImpuesto, Value = g.Sum(x => Math.Round(x.ValorServicio * x.Cantidad, 2)), DiscountValue = g.Sum(x => Math.Round(x.DiscountValue, 2)), TaxValue = g.Sum(x => Math.Round(x.TaxValue, 2)) }).ToList();

            EInvoice invoice = new EInvoice();

            invoice.IssueDate = factura.Fecha;
            invoice.IssueTime = factura.Fecha;
            if (factura.Documentos.Contingencia)
                invoice.InvoiceTypeCode = "03";
            else
                invoice.InvoiceTypeCode = "01";

            invoice.CustomizationID = "10";
            invoice.ID = factura.Documentos.Prefijo + "" + factura.NroConsecutivo.ToString();
            invoice.DocumentCurrencyCode = "COP";
            invoice.InvoicePeriod = new InvoicePeriod { StartDate = factura.FehaInicial, EndDate = factura.FechaFinal };

            invoice.AccountingSupplierParty = new AccountingSupplierParty();
            invoice.AccountingSupplierParty.AdditionalAccountID = factura.Empresas.TiposPersonas.Codigo;
            invoice.AccountingSupplierParty.Party = new Party();
            invoice.AccountingSupplierParty.Party.PartyIdentification = factura.Empresas.TiposIdentificacion.CodigoAlterno;
            invoice.AccountingSupplierParty.Party.ID = factura.Empresas.NumeroIdentificacion;
            invoice.AccountingSupplierParty.Party.schemeID = factura.Empresas.DV;
            invoice.AccountingSupplierParty.Party.PartyName = new PartyName { Name = factura.Empresas.RazonSocial };
            invoice.AccountingSupplierParty.Party.PhysicalLocation = new PhysicalLocation();
            invoice.AccountingSupplierParty.Party.PhysicalLocation.Address = new Address();
            invoice.AccountingSupplierParty.Party.PhysicalLocation.Address.ID = factura.Empresas.Ciudades.Codigo;
            invoice.AccountingSupplierParty.Party.PhysicalLocation.Address.CityName = factura.Empresas.Ciudades.Nombre;
            invoice.AccountingSupplierParty.Party.PhysicalLocation.Address.CountrySubentity = factura.Empresas.Ciudades.Departamentos.Nombre;
            invoice.AccountingSupplierParty.Party.PhysicalLocation.Address.CountrySubentityCode = factura.Empresas.Ciudades.Departamentos.Codigo;
            invoice.AccountingSupplierParty.Party.PhysicalLocation.Address.AddressLine = new AddressLine { Line = factura.Empresas.Direccion };

            invoice.AccountingSupplierParty.Party.PhysicalLocation.Address.Country = new CountryE { IdentificationCode = factura.Empresas.Ciudades.Departamentos.Paises.Codigo, Name = factura.Empresas.Ciudades.Departamentos.Paises.Nombre };

            invoice.AccountingSupplierParty.Party.PartyTaxScheme = new PartyTaxScheme { RegistrationName = factura.Empresas.RazonSocial, TaxLevelCode = factura.Empresas.GetResponsabilidadesFiscales(), TaxScheme = new TaxScheme { ID = factura.Empresas.GetCodigoImpuestoRecaudados(), Name = factura.Empresas.GetNombreImpuestoRecaudados() }, ListName = factura.Empresas.TiposRegimenContable.Codigo };

            invoice.AccountingSupplierParty.Party.SellerContact = new SellerContact { ElectronicMail = factura.Empresas.CorreoElectronico };

            invoice.SellerSupplierParty = new SellerSupplierParty();
            invoice.SellerSupplierParty.ID = factura.Sedes.Codigo;
            invoice.SellerSupplierParty.Party = new Party1();

            invoice.SellerSupplierParty.Party.PartyName = new PartyName { Name = factura.Sedes.Nombre };
            invoice.SellerSupplierParty.Party.PhysicalLocation = new PhysicalLocation();

            invoice.SellerSupplierParty.Party.PhysicalLocation.Address = new Address();
            invoice.SellerSupplierParty.Party.PhysicalLocation.Address.ID = factura.Sedes.Ciudades.Codigo;
            invoice.SellerSupplierParty.Party.PhysicalLocation.Address.CityName = factura.Sedes.Ciudades.Nombre;
            invoice.SellerSupplierParty.Party.PhysicalLocation.Address.CountrySubentity = factura.Sedes.Ciudades.Departamentos.Nombre;
            invoice.SellerSupplierParty.Party.PhysicalLocation.Address.CountrySubentityCode = factura.Sedes.Ciudades.Departamentos.Codigo;
            invoice.SellerSupplierParty.Party.PhysicalLocation.Address.AddressLine = new AddressLine { Line = factura.Sedes.Direccion };
            invoice.SellerSupplierParty.Party.PhysicalLocation.Address.Country = new CountryE { IdentificationCode = factura.Sedes.Ciudades.Departamentos.Paises.Codigo, Name = factura.Sedes.Ciudades.Departamentos.Paises.Nombre };

            if (factura.Pacientes != null)
            {
                invoice.AccountingCustomerParty = new AccountingCustomerParty();
                invoice.AccountingCustomerParty.AdditionalAccountID = "2";
                invoice.AccountingCustomerParty.Party = new Party2();
                invoice.AccountingCustomerParty.Party.PartyIdentification = factura.Pacientes.TiposIdentificacion.CodigoAlterno;
                invoice.AccountingCustomerParty.Party.ID = factura.Pacientes.NumeroIdentificacion.Trim();
                invoice.AccountingCustomerParty.Party.Person = new Person { FirstName = $"{factura.Pacientes.PrimerNombre} {factura.Pacientes.SegundoNombre}", LastName = $"{factura.Pacientes.PrimerApellido} {factura.Pacientes.SegundoApellido}" };

                invoice.AccountingCustomerParty.Party.PhysicalLocation = new PhysicalLocation { Address = new Address() };

                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.ID = factura.Pacientes.Ciudades.Codigo;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.CityName = factura.Pacientes.Ciudades.Nombre;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.CountrySubentity = factura.Pacientes.Ciudades.Departamentos.Nombre;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.CountrySubentityCode = factura.Pacientes.Ciudades.Departamentos.Codigo;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.AddressLine = new AddressLine { Line = factura.Pacientes.Direccion };

                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.Country = new CountryE { IdentificationCode = factura.Pacientes.Ciudades.Departamentos.Paises.Codigo, Name = factura.Pacientes.Ciudades.Departamentos.Paises.Nombre };
                invoice.PaymentMeans = new PaymentMeans { ID = ((factura.FormasPagos.Codigo == "1") ? "1" : "2"), PaymentDueDate = factura.Fecha.AddDays(0) };

                invoice.AccountingCustomerParty.Party.PartyTaxScheme = new PartyTaxScheme2();

                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationName = factura.Pacientes.NombreCompleto;
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.TaxLevelCode = "R-99-PN";
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.ListName = "No aplica";
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.TaxScheme = new TaxScheme { ID = "ZZ", Name = "No aplica" };

            }
            else
            {
                invoice.AccountingCustomerParty = new AccountingCustomerParty();
                invoice.AccountingCustomerParty.AdditionalAccountID = factura.Entidades.TiposPersonas.Codigo;

                invoice.AccountingCustomerParty.Party = new Party2();
                invoice.AccountingCustomerParty.Party.PartyIdentification = factura.Entidades.TiposIdentificacion.CodigoAlterno;
                invoice.AccountingCustomerParty.Party.ID = factura.Entidades.NumeroIdentificacion.Trim();
                invoice.AccountingCustomerParty.Party.schemeID = factura.Entidades.DV;
                invoice.AccountingCustomerParty.Party.PartyName = new PartyName { Name = factura.Entidades.Nombre };

                invoice.AccountingCustomerParty.Party.PhysicalLocation = new PhysicalLocation { Address = new Address() };

                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.ID = factura.Entidades.Ciudades.Codigo;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.CityName = factura.Entidades.Ciudades.Nombre;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.CountrySubentity = factura.Entidades.Ciudades.Departamentos.Nombre;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.CountrySubentityCode = factura.Entidades.Ciudades.Departamentos.Codigo;
                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.AddressLine = new AddressLine { Line = factura.Entidades.Direccion };

                invoice.AccountingCustomerParty.Party.PhysicalLocation.Address.Country = new CountryE { IdentificationCode = factura.Entidades.Ciudades.Departamentos.Paises.Codigo, Name = factura.Entidades.Ciudades.Departamentos.Paises.Nombre };

                invoice.AccountingCustomerParty.Party.PartyTaxScheme = new PartyTaxScheme2();

                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationName = factura.Entidades.Nombre;
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.TaxLevelCode = factura.Entidades.GetResponsabilidadesFiscales();
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.ListName = factura.Entidades.GetRegimenFiscal();
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.TaxScheme = new TaxScheme { ID = taxSchemeClient.ID, Name = taxSchemeClient.Name };
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationAddress = new Address();
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationAddress.ID = factura.Entidades.Ciudades.Codigo;
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationAddress.CityName = factura.Entidades.Ciudades.Nombre;
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationAddress.CountrySubentity = factura.Entidades.Ciudades.Departamentos.Nombre;
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationAddress.CountrySubentityCode = factura.Entidades.Ciudades.Departamentos.Codigo;
                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationAddress.AddressLine = new AddressLine { Line = factura.Entidades.Direccion };

                invoice.AccountingCustomerParty.Party.PartyTaxScheme.RegistrationAddress.Country = new CountryE { IdentificationCode = factura.Entidades.Ciudades.Departamentos.Paises.Codigo, Name = factura.Entidades.Ciudades.Departamentos.Paises.Nombre };

                invoice.CustomizationID = "SS-CUFE";

                invoice.PaymentMeans = new PaymentMeans { ID = factura.FormasPagos.Codigo, PaymentDueDate = factura.Fecha.AddDays(factura.Convenio.PeriodicidadPago.GetValueOrDefault(0)) };

                if (admisiones != null && admisiones.Count > 0)
                {
                    invoice.SectorSalud = new List<SectorSalud>();

                    foreach (var item in admisiones)
                    {
                        SectorSalud ss = new SectorSalud();
                        if (!string.IsNullOrWhiteSpace(factura.Entidades.CodigoReps))
                            ss.CodPrestadorServicio = factura.Entidades.CodigoReps.Trim();

                        if (!string.IsNullOrWhiteSpace(item.Pacientes.NumeroIdentificacion))
                            ss.NumIdentificacionUsuario = item.Pacientes.NumeroIdentificacion.Trim();

                        if (!string.IsNullOrWhiteSpace(item.Pacientes.TiposIdentificacion.Codigo))
                            ss.TipoIdentificacionUsuario = item.Pacientes.TiposIdentificacion.Codigo.Trim();

                        if (!string.IsNullOrWhiteSpace(item.Pacientes.PrimerApellido))
                            ss.PrimerApellido = NormalizeString.Normalize(item.Pacientes.PrimerApellido.Trim());

                        if (!string.IsNullOrWhiteSpace(item.Pacientes.SegundoApellido))
                            ss.SegundoApellido = NormalizeString.Normalize(item.Pacientes.SegundoApellido.Trim());

                        if (!string.IsNullOrWhiteSpace(item.Pacientes.PrimerNombre))
                            ss.PrimerNombre = NormalizeString.Normalize(item.Pacientes.PrimerNombre.Trim());

                        if (!string.IsNullOrWhiteSpace(item.Pacientes.SegundoNombre))
                            ss.SegundoNombre = NormalizeString.Normalize(item.Pacientes.SegundoNombre.Trim());

                        if (item.TiposUsuarios != null)
                            ss.TipoUsuario = item.TiposUsuarios.Id.ToString().Trim();

                        ss.ModContratoPago = item.Convenios.ModalidadesContratacion.Id.ToString("D2");
                        ss.Cobertura = item.CoberturaPlanBeneficios.Id.ToString("D2");

                        if (!string.IsNullOrWhiteSpace(item.NroAutorizacion))
                            ss.NumAutorizacion = item.NroAutorizacion.Trim();

                        if (!string.IsNullOrWhiteSpace(item.NumeroPrescripcion))
                            ss.NumPrescripcion = item.NumeroPrescripcion.Trim();
                        if (!string.IsNullOrWhiteSpace(item.NumeroSuministroPrescripcion))
                            ss.NumSuministroPrescripcion = item.NumeroSuministroPrescripcion.Trim();
                        if (!string.IsNullOrWhiteSpace(item.Convenios.NroContrato))
                            ss.Numcontrato = item.Convenios.NroContrato.Trim();
                        if (!string.IsNullOrWhiteSpace(item.NumeroPoliza))
                            ss.NumPoliza = item.NumeroPoliza.Trim();

                        ss.FechaInicio = factura.FehaInicial;
                        ss.FechaFinal = factura.FechaFinal;
                        if (item.ValorPagoEstadosId == 58)
                            ss.Copago = item.ValorCopago;
                        else
                            ss.Copago = 0;
                        if (item.ValorPagoEstadosId == 59)
                            ss.CuotaModeradora = item.ValorCopago;
                        else
                            ss.CuotaModeradora = 0;
                        if (item.ValorPagoEstadosId == 68)
                            ss.CoutaRecuperacion = item.ValorCopago;
                        else
                            ss.CoutaRecuperacion = 0;
                        if (item.ValorPagoEstadosId == 69)
                            ss.PagoVoluntario = item.ValorCopago;
                        else
                            ss.PagoVoluntario = 0;

                        invoice.SectorSalud.Add(ss);
                    }
                }
            }

            invoice.PrePaidPayment = new List<PrePaidPayment>();
            invoice.PrePaidPayment.Add(new PrePaidPayment { ID = 1, PaidAmount = factura.ValorCopagoCuotaModeradora, ReceivedDate = factura.Fecha });

            invoice.TaxTotal = new List<TaxTotal>();

            foreach (var item in InvoiceDetailsByTax)
            {
                if (item.TaxValue > 0)
                {
                    TaxTotal tax = new TaxTotal { TaxAmount = item.TaxValue };
                    tax.TaxSubtotal = new TaxSubtotal();
                    tax.TaxSubtotal.TaxableAmount = (item.TaxValue > 0) ? item.Value - item.DiscountValue : 0;
                    tax.TaxSubtotal.TaxAmount = item.TaxValue;
                    if (item.TaxSchemeCode == "22")
                    {
                        tax.TaxSubtotal.BaseUnitMeasure = "B%";
                        tax.TaxSubtotal.UnitCode = "NAR";
                        tax.TaxSubtotal.PerUnitAmount = new PerUnitAmountType { Value = 50 };
                    }
                    tax.TaxSubtotal.TaxCategory = new TaxCategory();
                    if (item.TaxSchemeCode != "22")
                        tax.TaxSubtotal.TaxCategory.Percent = new PercentType { Value = item.TaxPercentaje };
                    tax.TaxSubtotal.TaxCategory.TaxScheme = new TaxScheme { ID = item.TaxSchemeCode, Name = item.TaxSchemeName };

                    invoice.TaxTotal.Add(tax);
                }
            }

            if (invoice.TaxTotal.Count == 0)
                invoice.TaxTotal = null;

            invoice.LegalMonetaryTotal = new LegalMonetaryTotal();
            invoice.LegalMonetaryTotal.LineExtensionAmount = factura.ValorSubtotal - factura.ValorDescuentos;
            invoice.LegalMonetaryTotal.TaxExclusiveAmount = factura.ValorImpuestos;

            invoice.LegalMonetaryTotal.TaxExclusiveBaseAmount = factura.ValorImpuestos > 0 ? (factura.ValorSubtotal - factura.ValorDescuentos) : 0;

            invoice.LegalMonetaryTotal.TaxInclusiveAmount = (factura.ValorSubtotal - factura.ValorDescuentos + factura.ValorImpuestos);
            invoice.LegalMonetaryTotal.PrePaidAmount = new PrePaidAmountType { Value = factura.ValorCopagoCuotaModeradora };

            invoice.LegalMonetaryTotal.AllowanceLineAmount = new AllowanceLineAmountType { Value = factura.ValorDescuentos };
            invoice.LegalMonetaryTotal.PayableAmount = factura.ValorTotal;
            invoice.LegalMonetaryTotal.PayableExpectedAmount = factura.ValorTotal;
            invoice.LegalMonetaryTotal.TextAmount = DApp.Util.NumeroEnLetras(factura.ValorTotal);
            invoice.LegalMonetaryTotal.LineCountNumeric = new LineCountNumericType { Value = factura.FacturasDetalles.Count };
            invoice.InvoiceLine = new List<InvoiceLine>();

            for (int i = 0; i < factura.FacturasDetalles.Count; i++)
            {
                InvoiceLine data = new InvoiceLine();
                data.ID = (i + 1);
                data.StandardItemIdentification = new StandardItemIdentification { ID = factura.FacturasDetalles[i].Servicios.Codigo.Trim(), schemeID = "999" };
                data.InvoicedQuantity = factura.FacturasDetalles[i].Cantidad;
                data.UnitCode = "NAR";
                data.LineExtensionAmount = factura.FacturasDetalles[i].SubTotalValue - factura.FacturasDetalles[i].DiscountValue;
                //data.NetLineExtensionAmount = new NetLineExtensionAmountType { Value = InvoiceDetails[i].TotalValue }  ;
                data.Price = new Price { PriceAmount = factura.FacturasDetalles[i].ValorServicio, BaseQuantity = factura.FacturasDetalles[i].Cantidad };
                data.Item = new Item { Description = new List<string>() };
                data.Item.AddDescription(DApp.Util.QutarTildes(factura.FacturasDetalles[i].Servicios.Nombre));

                if (factura.FacturasDetalles[i].PorcDescuento > 0)
                {
                    data.AllowanceCharge = new List<AllowanceCharge>();
                    AllowanceCharge discount = new AllowanceCharge();
                    discount.ID = 1;
                    discount.ChargeIndicator = false;
                    discount.AllowanceChargeReason = "Descuento general";
                    discount.MultiplierFactorNumeric = factura.FacturasDetalles[i].PorcDescuento;
                    discount.BaseAmount = factura.FacturasDetalles[i].SubTotalValue;
                    discount.Amount = factura.FacturasDetalles[i].DiscountValue;
                    data.AllowanceCharge.Add(discount);
                }
                if (factura.FacturasDetalles[i].TaxValue > 0)
                {
                    data.TaxTotal = new TaxTotal();
                    data.TaxTotal.TaxAmount = factura.FacturasDetalles[i].TaxValue;
                    data.TaxTotal.TaxSubtotal = new TaxSubtotal { TaxableAmount = factura.FacturasDetalles[i].TaxValue > 0 ? factura.FacturasDetalles[i].SubTotalValue - factura.FacturasDetalles[i].DiscountValue : 0, TaxAmount = factura.FacturasDetalles[i].TaxValue };
                    data.TaxTotal.TaxSubtotal.TaxCategory = new TaxCategory();
                    data.TaxTotal.TaxSubtotal.TaxCategory.Percent = new PercentType { Value = factura.FacturasDetalles[i].PorcImpuesto };
                    data.TaxTotal.TaxSubtotal.TaxCategory.TaxScheme = new TaxScheme { ID = factura.FacturasDetalles[i].Servicios.TiposImpuestos.Codigo, Name = factura.FacturasDetalles[i].Servicios.TiposImpuestos.Descripcion };
                }
                invoice.InvoiceLine.Add(data);

            }

            invoice.InvoiceControl = new InvoiceControl();
            if (factura.Documentos.ResolucionDian == null || factura.Documentos.ResolucionDian == 0)
                throw new Exception("El documento no tiene una resolucion de la DIAN");

            invoice.InvoiceControl.InvoiceAuthorization = factura.Documentos.ResolucionDian.GetValueOrDefault(0);
            invoice.InvoiceControl.AuthorizationPeriod = new InvoicePeriod { StartDate = (DateTime)factura.Documentos.FechaDesde, EndDate = (DateTime)factura.Documentos.FechaHasta };
            invoice.InvoiceControl.AuthorizedInvoices = new AuthorizedInvoice { Prefix = factura.Documentos.Prefijo, To = factura.Documentos.ConsecutivoHasta.GetValueOrDefault(0), From = factura.Documentos.ConsecutivoDesde.GetValueOrDefault(0), TechnicalKey = factura.Documentos.LlaveUnica };

            return invoice;
        }

        public async Task<string> SendEInvioceAsync(long idFactura, string urlAcepta)
        {
            string content = "";
            EInvoice invoice = GetInvoice(idFactura);
            string xml = invoice.SerializeToXml();
            //.Replace(@"<?xml version=""1.0"" encoding=""utf-8""?>", "");
            HttpClient http = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("docid", invoice.ID));
            nvc.Add(new KeyValuePair<string, string>("comando", "emitir"));
            nvc.Add(new KeyValuePair<string, string>("parametros", ""));
            nvc.Add(new KeyValuePair<string, string>("datos", xml));

            //var con = new FormUrlEncodedContent(nvc);

            var encodedItems = nvc.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));
            var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

            var response = await http.PostAsync(urlAcepta, encodedContent);
            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error Acepta: {response.StatusCode} | {response.RequestMessage.RequestUri} | {response.Content?.ReadAsStringAsync()} ");
            }
            return content;
        }

        public async Task<string> ObtenerXMLFactura(long idFactura)
        {
            EInvoice invoice = GetInvoice(idFactura);
            string xml = invoice.SerializeToXml();
            return xml;
        }

        public string GenerateFileToCobol(List<long> ids)
        {
            string path = Path.Combine(Path.GetTempPath(), "FilesToCobol");
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            List<VContabilizacionRegistro> documentos = new GenericBusinessLogic<VContabilizacionRegistro>(this.UnitOfWork).FindAll(x => ids.Contains(x.Id)).OrderBy(x => x.Id).ToList();
            if (documentos.Count <= 0)
                throw new Exception("No existen datos a generar con las facturas seleccionadas.");

            //List<IGrouping<string, VContabilizacionRegistro>> result = documentos.GroupBy(x => x.Documento).ToList();
            //int i = 0;
            //foreach (var registro in result)
            //{
            //    string nombreArchivo = $"FIBATCH.{i.ToString("D3")}";
            //    List<string> data = registro.Select(x => Regex.Replace(x.Registro.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "") ).ToList();
            //    File.WriteAllLines(Path.Combine(path, nombreArchivo), data);
            //    i++;
            //}
            string nombreArchivo = $"FIBATCH.{documentos.Count.ToString("D3")}";
            List<string> data = new List<string>();
            //foreach (var item in documentos)
            //{
            //    data.Add(item.Registro);
            //}
            for (int i = 0; i < documentos.Count; i++)
            {
                var datareg = (i + 1).ToString("D9") + documentos[i].Registro.Substring(9);
                data.Add(DApp.Util.QutarTildes(datareg));
            }
            File.WriteAllLines(Path.Combine(path, nombreArchivo), data);

            ZipArchive archive = new ZipArchive();
            archive.AddDirectory(path, "/");
            string pathZip = Path.GetTempFileName();
            archive.Save(pathZip);
            return pathZip;
        }

        public byte[] ExcelInformeCartera(long entidadId, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.CreateNewDocument();
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet as Worksheet;
                worksheet.Name = "Informe de cartera por entidad";
                List<VReporteCartera> data = new GenericBusinessLogic<VReporteCartera>(this.UnitOfWork).Tabla()
                    .Where(x => x.EntidadId == entidadId && x.Fecha_Emision >= fechaDesde && x.Fecha_Emision <= fechaHasta)
                    .OrderBy(x => x.Fecha_Emision).ToList();


                //Titulos
                int column = 0;
                worksheet.Rows[0][column].SetValue("RAZON SOCIAL ENTIDAD"); column++;
                worksheet.Rows[0][column].SetValue("TIPO DOCUMENTO"); column++;
                worksheet.Rows[0][column].SetValue("NUMERO DE IDENTIFICACION"); column++;
                worksheet.Rows[0][column].SetValue("PREFIJO"); column++;
                worksheet.Rows[0][column].SetValue("CONSECUTIVO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA EMISION"); column++;
                worksheet.Rows[0][column].SetValue("FECHA RADICACION"); column++;
                worksheet.Rows[0][column].SetValue("PLAZO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA VENCIMIENTO"); column++;
                worksheet.Rows[0][column].SetValue("DIAS MORA"); column++;
                worksheet.Rows[0][column].SetValue("SUBTOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("COPAGOS Y CUOTAS MODERADORAS"); column++;
                worksheet.Rows[0][column].SetValue("TOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR ACEPTADO GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("SALDO POR COBRAR (Aplicados todos los recaudos)"); column++;
                worksheet.Rows[0][column].SetValue("PAGOS RECIBIDOS"); column++;

                for (int row = 0; row < data.Count; row++)
                {
                    column = 0;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Razon_Social_Entidad); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Tipo_Documento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].NumeroIdentificacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Prefijo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Consecutivo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Emision); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Radicacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Plazo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].FechaVencimiento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].DiasMora); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Subtotal_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Copagos_CuotasModeradoras); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Total_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteFuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Retefuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Aceptado_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Saldo_Por_Cobrar); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Pagos_Recibidos); column++;
                }
                worksheet.Columns.AutoFit(0, column);

                byte[] book = workbook.SaveDocument(DocumentFormat.Xlsx);
                workbook.Dispose();
                return book;
            }
            catch
            {
                throw;
            }
        }

        public byte[] ExcelInformeGeneralCartera(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.CreateNewDocument();
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet as Worksheet;
                worksheet.Name = "Informe de cartera general";
                List<VReporteCartera> data = new GenericBusinessLogic<VReporteCartera>(this.UnitOfWork).Tabla()
                    .Where(x => x.Fecha_Emision >= fechaDesde && x.Fecha_Emision <= fechaHasta)
                    .OrderBy(x => x.Fecha_Emision).ToList();


                //Titulos
                int column = 0;
                worksheet.Rows[0][column].SetValue("RAZON SOCIAL ENTIDAD"); column++;
                worksheet.Rows[0][column].SetValue("TIPO DOCUMENTO"); column++;
                worksheet.Rows[0][column].SetValue("NUMERO DE IDENTIFICACION"); column++;
                worksheet.Rows[0][column].SetValue("PREFIJO"); column++;
                worksheet.Rows[0][column].SetValue("CONSECUTIVO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA EMISION"); column++;
                worksheet.Rows[0][column].SetValue("FECHA RADICACION"); column++;
                worksheet.Rows[0][column].SetValue("PLAZO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA VENCIMIENTO"); column++;
                worksheet.Rows[0][column].SetValue("DIAS MORA"); column++;
                worksheet.Rows[0][column].SetValue("SUBTOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("COPAGOS Y CUOTAS MODERADORAS"); column++;
                worksheet.Rows[0][column].SetValue("TOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR ACEPTADO GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("SALDO POR COBRAR (Aplicados todos los recaudos)"); column++;
                worksheet.Rows[0][column].SetValue("PAGOS RECIBIDOS"); column++;

                for (int row = 0; row < data.Count; row++)
                {
                    column = 0;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Razon_Social_Entidad); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Tipo_Documento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].NumeroIdentificacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Prefijo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Consecutivo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Emision); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Radicacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Plazo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].FechaVencimiento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].DiasMora); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Subtotal_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Copagos_CuotasModeradoras); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Total_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteFuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Retefuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Aceptado_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Saldo_Por_Cobrar); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Pagos_Recibidos); column++;
                }
                worksheet.Columns.AutoFit(0, column);

                byte[] book = workbook.SaveDocument(DocumentFormat.Xlsx);
                workbook.Dispose();
                return book;
            }
            catch
            {
                throw;
            }
        }

        public byte[] ExcelExportarSiigo(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.CreateNewDocument();
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet as Worksheet;
                worksheet.Name = "Exportar SIIGO";
                List<VExportarSiigo> data = new GenericBusinessLogic<VExportarSiigo>(this.UnitOfWork).Tabla()
                    .Where(x => x.Fecha_Emision >= fechaDesde && x.Fecha_Emision <= fechaHasta)
                    .OrderBy(x => x.Fecha_Emision).ToList();


                //Titulos
                int column = 0;
                worksheet.Rows[0][column].SetValue("Tipo de comprobante"); column++;
                worksheet.Rows[0][column].SetValue("Consecutivo"); column++;
                worksheet.Rows[0][column].SetValue("Tipo identificación"); column++;
                worksheet.Rows[0][column].SetValue("Numero Identificación"); column++;
                worksheet.Rows[0][column].SetValue("Razón social (Obligatorio)"); column++;
                worksheet.Rows[0][column].SetValue("Nombres del tercero (Obligatorio)"); column++;
                worksheet.Rows[0][column].SetValue("Apellidos del tercero (Obligatorio)"); column++;
                worksheet.Rows[0][column].SetValue("Nit Entidad copago"); column++;
                worksheet.Rows[0][column].SetValue("Fecha de elaboración"); column++;
                worksheet.Rows[0][column].SetValue("Email Contacto"); column++;
                worksheet.Rows[0][column].SetValue("Código producto"); column++;
                worksheet.Rows[0][column].SetValue("Descripción producto"); column++;
                worksheet.Rows[0][column].SetValue("Cantidad producto"); column++;
                worksheet.Rows[0][column].SetValue("Valor unitario"); column++;
                worksheet.Rows[0][column].SetValue("Valor Descuento"); column++;
                worksheet.Rows[0][column].SetValue("Valor Copago"); column++;
                worksheet.Rows[0][column].SetValue("Valor Forma de Pago"); column++;
                worksheet.Rows[0][column].SetValue("Fecha Vencimiento"); column++;
                worksheet.Rows[0][column].SetValue("Observaciones"); column++;

                for (int row = 0; row < data.Count; row++)
                {
                    column = 0;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Prefijo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Consecutivo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Tipo_Documento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Numero_Identificacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Razon_Social); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Nombres_Tercero); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Apellidos_Tercero); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Nit_Entidad_Copago); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Emision); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Email_Contacto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Codigo_Producto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Descripcion_Producto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Cantidad_Producto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Unitario); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Descuento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Copago); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Subtotal_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].FechaVencimiento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Observaciones); column++;
                }
                worksheet.Columns.AutoFit(0, column);

                byte[] book = workbook.SaveDocument(DocumentFormat.Xlsx);
                workbook.Dispose();
                return book;
            }
            catch
            {
                throw;
            }
        }

    }
}