using Blazor.BusinessLogic.Models;
using Blazor.BusinessLogic.Models.Enums;
using Blazor.BusinessLogic.ServiciosExternos;
using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using Blazor.Reports.Notas;
using DevExpress.Compression;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static DevExpress.Data.Filtering.Helpers.SubExprHelper.ThreadHoppingFiltering;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace Blazor.BusinessLogic
{
    public class NotasBusinessLogic : GenericBusinessLogic<Notas>
    {
        public NotasBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public NotasBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public async Task<IntegracionEnviarFEModel> EnviarFacturaDian(long notaId, string user, string host)
        {
            ResultadoIntegracionFE enviarDocumento_FE = new ResultadoIntegracionFE();
            enviarDocumento_FE.CreatedBy = user;
            enviarDocumento_FE.UpdatedBy = user;
            enviarDocumento_FE.CreationDate = DateTime.Now;
            enviarDocumento_FE.LastUpdate = DateTime.Now;

            IntegracionEnviarFEModel enviarDocumento_IFE = new IntegracionEnviarFEModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            unitOfWork.BeginTransaction();
            try
            {
                var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
                var nota = unitOfWork.Repository<Notas>().Table
                    .Include(x => x.Documentos)
                    .FirstOrDefault(x => x.Id == notaId);

                if (nota.IdDocumentoFE.HasValue &&
                    !string.IsNullOrWhiteSpace(nota.DIANResponse) &&
                    nota.DIANResponse.Equals(DApp.Util.Dian.StatusStaged, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Esta factura ya fue enviada a la DIAN pero no ha sido ceritifcada. Por favor consulte su estado.");
                }

                if (nota.IdDocumentoFE.HasValue &&
                    !string.IsNullOrWhiteSpace(nota.DIANResponse) &&
                    nota.DIANResponse.Equals(DApp.Util.Dian.StatusCertified, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Esta factura ya fue enviada a la DIAN.");
                }

                if (nota.Documentos.Transaccion != 3 && nota.Documentos.Transaccion != 4)
                {
                    throw new Exception($"El documento no pertenece a la transacción para nota cretido o debito. Por favor verifique la configuración del documento {nota.Documentos.Prefijo}");
                }

                IntegracionFE integracionFE = new IntegracionFE(parametros, host);

                enviarDocumento_FE.Tipo = (int)TipoDocumento.Nota;
                enviarDocumento_FE.IdTipo = notaId;

                if (nota.Documentos.Transaccion == 3) // Nota Credito
                {
                    var json = GetFENotaCreditoJson(notaId);
                    //enviarDocumento_IFE = await integracionFE.EnviarNotaCredito(json);
                }
                else if (nota.Documentos.Transaccion == 4) // Nota Debito
                {
                    var json = GetFENotaDebitoJson(notaId);
                    //enviarDocumento_IFE = await integracionFE.EnviarNotaDebito(json);
                }



                enviarDocumento_FE.Api = enviarDocumento_IFE.Api;
                enviarDocumento_FE.HttpStatus = enviarDocumento_IFE.HttpStatus;
                enviarDocumento_FE.JsonResult = enviarDocumento_IFE.JsonResult;
                enviarDocumento_FE.HuboError = enviarDocumento_IFE.HuboErrorFE || enviarDocumento_IFE.HuboErrorIntegracion;
                enviarDocumento_FE.Error = string.Join(", ", enviarDocumento_IFE.Errores);

                if (!enviarDocumento_FE.HuboError)
                {
                    nota.IdDocumentoFE = enviarDocumento_IFE.IdDocumentFE;
                    nota.UpdatedBy = user;
                    unitOfWork.Repository<Notas>().Modify(nota);
                    unitOfWork.CommitTransaction();
                    enviarDocumento_IFE.IdDocumentFE = nota.IdDocumentoFE;

                    //enviarDocumento_IFE = await ConsultarEstadoDocumento(fac.Id, user, host);
                }
            }
            catch (Exception ex)
            {
                enviarDocumento_FE.HuboError = true;
                enviarDocumento_FE.Error = ex.GetFullErrorMessage();
                enviarDocumento_IFE.Errores.Add(ex.GetFullErrorMessage());
                enviarDocumento_IFE.HuboErrorFE = true;
            }

            unitOfWork.Repository<ResultadoIntegracionFE>().Add(enviarDocumento_FE);
            unitOfWork.CommitTransaction();
            return enviarDocumento_IFE;
        }

        /// <summary>
        /// https://localhost:44333/empresas/ObtenerJsonNotaDebitoFE?id=10021
        /// </summary>
        /// <param name="idNota"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetFENotaDebitoJson(long notaId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var nota = unitOfWork.Repository<Notas>().Table
                .Include(x => x.NotasConceptos)
                .Include(x => x.Facturas.Documentos)
                .Include(x => x.Empresas.TiposIdentificacion)
                .Include(x => x.Empresas.Ciudades.Departamentos.Paises)
                .Include(x => x.Documentos)
                .Include(x => x.Entidades)
                .Include(x => x.Entidades.TiposPersonas)
                .Include(x => x.Entidades.Ciudades.Departamentos.Paises)
                .Include(x => x.Entidades.TiposIdentificacion)
                .Include(x => x.Entidades.EntidadesResponsabilidadesFiscales).ThenInclude(x => x.ResponsabilidadesFiscales)
                .Include(x => x.Pacientes)
                .Include(x => x.Pacientes.Ciudades.Departamentos.Paises)
                .Include(x => x.Pacientes.TiposIdentificacion)
                .Include(x => x.MediosPago)
                .Include(x => x.FormasPagos)
                .FirstOrDefault(x => x.Id == notaId);

            if (nota == null)
            {
                throw new Exception($"La nota con el Id {notaId} no se encuentra registrada en el sistema.");
            }

            var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(parametros.EmailRecepcionFE))
            {
                throw new Exception($"El email para recepcionar los documentos de la facturación electrónica no se encuentra parametrizado correctamente.");
            }
            if (!DApp.Util.EsEmailValido(parametros.EmailRecepcionFE))
            {
                throw new Exception($"El email para recepcionar los documentos de la facturación electrónica no es válido.");
            }

            var notasDetalles = unitOfWork.Repository<NotasDetalles>().Table
                .Include(x => x.Servicios)
                .Where(x => x.NotasId == nota.Id)
                .ToList();

            if (nota.Documentos.Transaccion != 4)
            {
                throw new Exception($"El documento {nota.Documentos.Prefijo} no figura como transacción de nota debito.");
            }

            FeNotaDebitoJson feRootJson = new FeNotaDebitoJson();
            feRootJson.Currency = DApp.Util.Dian.Currency;
            feRootJson.ReasonDebit = nota.NotasConceptos.Codigo;
            feRootJson.SeriePrefix = nota.Documentos.Prefijo;
            feRootJson.SerieNumber = nota.Consecutivo.ToString();
            feRootJson.OperationType = DApp.Util.Dian.OperationTypeNotaDebito;
            feRootJson.IssueDate = nota.Fecha;
            feRootJson.DueDate = nota.FechaVencimiento;
            feRootJson.DeliveryDate = nota.Fecha;
            feRootJson.CorrelationDocumentId = nota.ConsecutivoFE;
            feRootJson.SerieExternalKey = nota.Documentos.ExternalKey;
            feRootJson.IssuerParty.Identification.DocumentNumber = nota.Empresas.NumeroIdentificacion;
            feRootJson.IssuerParty.Identification.DocumentType = nota.Empresas.TiposIdentificacion.CodigoFE;
            feRootJson.IssuerParty.Identification.CountryCode = nota.Empresas.Ciudades.Departamentos.Paises.Codigo;
            feRootJson.IssuerParty.Identification.CheckDigit = nota.Empresas.DV;

            FePaymentMean fEPaymentMeans = new FePaymentMean();
            fEPaymentMeans.Code = nota.MediosPago.Codigo;
            fEPaymentMeans.Mean = nota.FormasPagos.Codigo;
            fEPaymentMeans.DueDate = nota.FechaVencimiento.ToString("yyyy-MM-dd");
            feRootJson.PaymentMeans.Add(fEPaymentMeans);

            feRootJson.Total.GrossAmount = nota.ValorSubtotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.TotalBillableAmount = nota.ValorSubtotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.PayableAmount = nota.ValorTotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.TaxableAmount = "0";

            feRootJson.DocumentReferences.Add(new FeDocumentReference
            {
                DocumentReferred = $"{nota.Facturas.Documentos.Prefijo}{nota.Facturas.NroConsecutivo}",
                IssueDate = nota.Facturas.Fecha.ToString("yyyy-MM-dd"),
                Type = DApp.Util.Dian.InvoiceReference,
                DocumentReferredCUFE = nota.Facturas.CUFE
            });

            feRootJson.CustomerParty.Email = parametros.EmailRecepcionFE; // Es el correo al que van a llegar las notificaciones del provedor de FE
            if (nota.EsNotaInstitucional)
            {
                feRootJson.CustomerParty.LegalType = nota.Entidades.TiposPersonas.NombreFE;
                feRootJson.CustomerParty.TaxScheme = "ZZ"; //Identificador del Régimen Fiscal del adquirente ???
                feRootJson.CustomerParty.ResponsabilityTypes.AddRange(nota.Entidades.EntidadesResponsabilidadesFiscales.Select(x => x.ResponsabilidadesFiscales.Codigo));
                feRootJson.CustomerParty.Identification.DocumentNumber = nota.Entidades.NumeroIdentificacion;
                feRootJson.CustomerParty.Identification.DocumentType = nota.Empresas.TiposIdentificacion.CodigoFE;
                feRootJson.CustomerParty.Identification.CountryCode = nota.Entidades.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.CustomerParty.Identification.CheckDigit = nota.Entidades.DV;
                feRootJson.CustomerParty.Name = nota.Entidades.Nombre;
                feRootJson.CustomerParty.Address.DepartmentCode = nota.Entidades.Ciudades.Departamentos.Codigo;
                feRootJson.CustomerParty.Address.CityCode = nota.Entidades.Ciudades.Codigo;
                feRootJson.CustomerParty.Address.AddressLine = nota.Entidades.Direccion;
                feRootJson.CustomerParty.Address.Country = nota.Entidades.Ciudades.Departamentos.Paises.Codigo;
            }
            else
            {
                feRootJson.CustomerParty.LegalType = "Natural"; // agregar tipopersona a la tabla pacientes
                feRootJson.CustomerParty.TaxScheme = "ZZ";
                feRootJson.CustomerParty.ResponsabilityTypes.Add("R-99-PN");
                feRootJson.CustomerParty.Identification.DocumentNumber = nota.Pacientes.NumeroIdentificacion;
                feRootJson.CustomerParty.Identification.DocumentType = nota.Pacientes.TiposIdentificacion.CodigoFE;
                feRootJson.CustomerParty.Identification.CountryCode = nota.Pacientes.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.CustomerParty.Person.FirstName = nota.Pacientes.PrimerNombre;
                feRootJson.CustomerParty.Person.MiddleName = nota.Pacientes.SegundoNombre;
                feRootJson.CustomerParty.Person.FamilyName = nota.Pacientes.PrimerApellido;
                feRootJson.CustomerParty.Address.DepartmentCode = nota.Pacientes.Ciudades.Departamentos.Codigo;
                feRootJson.CustomerParty.Address.CityCode = nota.Pacientes.Ciudades.Codigo;
                feRootJson.CustomerParty.Address.AddressLine = nota.Pacientes.Direccion;
                feRootJson.CustomerParty.Address.Country = nota.Pacientes.Ciudades.Departamentos.Paises.Codigo;
            }

            int numberLine = 1;
            foreach (var notaDetalle in notasDetalles)
            {
                FeLine feLine = new FeLine();

                feLine.Number = numberLine.ToString();
                feLine.Quantity = notaDetalle.Cantidad.ToString("F2", CultureInfo.InvariantCulture);
                feLine.QuantityUnitOfMeasure = DApp.Util.Dian.QuantityUnitOfMeasure;
                feLine.ExcludeVat = "true";
                feLine.UnitPrice = notaDetalle.ValorServicio.ToString(CultureInfo.InvariantCulture);
                feLine.GrossAmount = notaDetalle.SubTotal.ToString(CultureInfo.InvariantCulture);
                feLine.NetAmount = notaDetalle.ValorTotal.ToString(CultureInfo.InvariantCulture);
                feLine.Item.Gtin = notaDetalle.Servicios.Codigo;
                feLine.Item.Description = notaDetalle.Servicios.Nombre;

                feRootJson.Lines.Add(feLine);
                numberLine++;
            }

            return JsonConvert.SerializeObject(feRootJson, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// https://localhost:44333/empresas/ObtenerJsonNotaDebitoFE?id=10021
        /// </summary>
        /// <param name="idNota"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetFENotaCreditoJson(long notaId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var nota = unitOfWork.Repository<Notas>().Table
                .Include(x => x.NotasConceptos)
                .Include(x => x.Facturas.Documentos)
                .Include(x => x.Empresas.TiposIdentificacion)
                .Include(x => x.Empresas.Ciudades.Departamentos.Paises)
                .Include(x => x.Documentos)
                .Include(x => x.Entidades)
                .Include(x => x.Entidades.TiposPersonas)
                .Include(x => x.Entidades.Ciudades.Departamentos.Paises)
                .Include(x => x.Entidades.TiposIdentificacion)
                .Include(x => x.Entidades.EntidadesResponsabilidadesFiscales).ThenInclude(x => x.ResponsabilidadesFiscales)
                .Include(x => x.Pacientes)
                .Include(x => x.Pacientes.Ciudades.Departamentos.Paises)
                .Include(x => x.Pacientes.TiposIdentificacion)
                .Include(x => x.MediosPago)
                .Include(x => x.FormasPagos)
                .FirstOrDefault(x => x.Id == notaId);

            if (nota == null)
            {
                throw new Exception($"La nota con el Id {notaId} no se encuentra registrada en el sistema.");
            }

            var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(parametros.EmailRecepcionFE))
            {
                throw new Exception($"El email para recepcionar los documentos de la facturación electrónica no se encuentra parametrizado correctamente.");
            }
            if (!DApp.Util.EsEmailValido(parametros.EmailRecepcionFE))
            {
                throw new Exception($"El email para recepcionar los documentos de la facturación electrónica no es válido.");
            }

            var notasDetalles = unitOfWork.Repository<NotasDetalles>().Table
                .Include(x => x.Servicios)
                .Where(x => x.NotasId == nota.Id)
                .ToList();

            if (nota.Documentos.Transaccion != 3)
            {
                throw new Exception($"El documento {nota.Documentos.Prefijo} no figura como transacción de nota credito.");
            }

            FeNotaCreditoJson feRootJson = new FeNotaCreditoJson();
            feRootJson.Currency = DApp.Util.Dian.Currency;
            feRootJson.ReasonCredit = nota.NotasConceptos.Codigo;
            feRootJson.SeriePrefix = nota.Documentos.Prefijo;
            feRootJson.SerieNumber = nota.Consecutivo.ToString();
            feRootJson.OperationType = DApp.Util.Dian.OperationTypeNotaDebito;
            feRootJson.IssueDate = nota.Fecha;
            feRootJson.DueDate = nota.FechaVencimiento;
            feRootJson.DeliveryDate = nota.Fecha;
            feRootJson.CorrelationDocumentId = nota.ConsecutivoFE;
            feRootJson.SerieExternalKey = nota.Documentos.ExternalKey;
            feRootJson.IssuerParty.Identification.DocumentNumber = nota.Empresas.NumeroIdentificacion;
            feRootJson.IssuerParty.Identification.DocumentType = nota.Empresas.TiposIdentificacion.CodigoFE;
            feRootJson.IssuerParty.Identification.CountryCode = nota.Empresas.Ciudades.Departamentos.Paises.Codigo;
            feRootJson.IssuerParty.Identification.CheckDigit = nota.Empresas.DV;

            FePaymentMean fEPaymentMeans = new FePaymentMean();
            fEPaymentMeans.Code = nota.MediosPago.Codigo;
            fEPaymentMeans.Mean = nota.FormasPagos.Codigo;
            fEPaymentMeans.DueDate = nota.FechaVencimiento.ToString("yyyy-MM-dd");
            feRootJson.PaymentMeans.Add(fEPaymentMeans);

            feRootJson.Total.GrossAmount = nota.ValorSubtotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.TotalBillableAmount = nota.ValorSubtotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.PayableAmount = nota.ValorTotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.TaxableAmount = "0";

            feRootJson.DocumentReferences.Add(new FeDocumentReference
            {
                DocumentReferred = $"{nota.Facturas.Documentos.Prefijo}{nota.Facturas.NroConsecutivo}",
                IssueDate = nota.Facturas.Fecha.ToString("yyyy-MM-dd"),
                Type = DApp.Util.Dian.InvoiceReference,
                DocumentReferredCUFE = nota.Facturas.CUFE
            });

            feRootJson.CustomerParty.Email = parametros.EmailRecepcionFE; // Es el correo al que van a llegar las notificaciones del provedor de FE
            if (nota.EsNotaInstitucional)
            {
                feRootJson.CustomerParty.LegalType = nota.Entidades.TiposPersonas.NombreFE;
                feRootJson.CustomerParty.TaxScheme = "ZZ"; //Identificador del Régimen Fiscal del adquirente ???
                feRootJson.CustomerParty.ResponsabilityTypes.AddRange(nota.Entidades.EntidadesResponsabilidadesFiscales.Select(x => x.ResponsabilidadesFiscales.Codigo));
                feRootJson.CustomerParty.Identification.DocumentNumber = nota.Entidades.NumeroIdentificacion;
                feRootJson.CustomerParty.Identification.DocumentType = nota.Empresas.TiposIdentificacion.CodigoFE;
                feRootJson.CustomerParty.Identification.CountryCode = nota.Entidades.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.CustomerParty.Identification.CheckDigit = nota.Entidades.DV;
                feRootJson.CustomerParty.Name = nota.Entidades.Nombre;
                feRootJson.CustomerParty.Address.DepartmentCode = nota.Entidades.Ciudades.Departamentos.Codigo;
                feRootJson.CustomerParty.Address.CityCode = nota.Entidades.Ciudades.Codigo;
                feRootJson.CustomerParty.Address.AddressLine = nota.Entidades.Direccion;
                feRootJson.CustomerParty.Address.Country = nota.Entidades.Ciudades.Departamentos.Paises.Codigo;
            }
            else
            {
                feRootJson.CustomerParty.LegalType = "Natural"; // agregar tipopersona a la tabla pacientes
                feRootJson.CustomerParty.TaxScheme = "ZZ";
                feRootJson.CustomerParty.ResponsabilityTypes.Add("R-99-PN");
                feRootJson.CustomerParty.Identification.DocumentNumber = nota.Pacientes.NumeroIdentificacion;
                feRootJson.CustomerParty.Identification.DocumentType = nota.Pacientes.TiposIdentificacion.CodigoFE;
                feRootJson.CustomerParty.Identification.CountryCode = nota.Pacientes.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.CustomerParty.Person.FirstName = nota.Pacientes.PrimerNombre;
                feRootJson.CustomerParty.Person.MiddleName = nota.Pacientes.SegundoNombre;
                feRootJson.CustomerParty.Person.FamilyName = nota.Pacientes.PrimerApellido;
                feRootJson.CustomerParty.Address.DepartmentCode = nota.Pacientes.Ciudades.Departamentos.Codigo;
                feRootJson.CustomerParty.Address.CityCode = nota.Pacientes.Ciudades.Codigo;
                feRootJson.CustomerParty.Address.AddressLine = nota.Pacientes.Direccion;
                feRootJson.CustomerParty.Address.Country = nota.Pacientes.Ciudades.Departamentos.Paises.Codigo;
            }

            int numberLine = 1;
            foreach (var notaDetalle in notasDetalles)
            {
                FeLine feLine = new FeLine();

                feLine.Number = numberLine.ToString();
                feLine.Quantity = notaDetalle.Cantidad.ToString("F2", CultureInfo.InvariantCulture);
                feLine.QuantityUnitOfMeasure = DApp.Util.Dian.QuantityUnitOfMeasure;
                feLine.ExcludeVat = "true";
                feLine.UnitPrice = notaDetalle.ValorServicio.ToString(CultureInfo.InvariantCulture);
                feLine.GrossAmount = notaDetalle.SubTotal.ToString(CultureInfo.InvariantCulture);
                feLine.NetAmount = notaDetalle.ValorTotal.ToString(CultureInfo.InvariantCulture);
                feLine.Item.Gtin = notaDetalle.Servicios.Codigo;
                feLine.Item.Description = notaDetalle.Servicios.Nombre;

                feRootJson.Lines.Add(feLine);
                numberLine++;
            }


            return JsonConvert.SerializeObject(feRootJson, Newtonsoft.Json.Formatting.Indented);
        }

        private string GetPdfNotaReporte(Notas nota, string nameFaile, string user)
        {
            XtraReport xtraReport = ReportExtentions.Report<NotasReporte>(this.BusinessLogic, nota.Id, user);

            string pathPdf = Path.Combine(Path.GetTempPath(), $"{(nota.Documentos.Transaccion == 3 ? "nc" : "nd")}{nameFaile}.pdf");
            PdfExportOptions pdfOptions = new PdfExportOptions();
            pdfOptions.ConvertImagesToJpeg = false;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;
            pdfOptions.PdfACompatibility = PdfACompatibility.PdfA2b;
            xtraReport.ExportToPdf(pathPdf, pdfOptions);
            return pathPdf;
        }

        public async Task EnviarEmail(long notaId, string eventoEnvio, string user)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var nota = unitOfWork.Repository<Notas>().FindById(x => x.Id == notaId, true);

            if (string.IsNullOrWhiteSpace(nota.DIANResponse))
            {
                throw new Exception("La nota no ha sido aceptada por la dian.");
            }
            else if (!nota.DIANResponse.Contains("DIAN Aceptado"))
            {
                throw new Exception("La nota no ha sido aceptada por la dian.");
            }

            try
            {
                string correo = null;
                if (nota.Facturas.AdmisionesId != null)
                    correo = unitOfWork.Repository<Pacientes>().GetTable().FirstOrDefault(x => x.Id == nota.PacientesId)?.CorreoElectronico;
                else
                    correo = unitOfWork.Repository<Entidades>().GetTable().FirstOrDefault(x => x.Id == nota.EntidadesId)?.CorreoElectronico;

                byte[] contentarray = null;
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(nota.XmlUrl);
                if (response.IsSuccessStatusCode)
                    contentarray = await response.Content.ReadAsByteArrayAsync();
                else
                    throw new Exception($"Error en descargar XMl desde acepta. | {response.StatusCode} - {response.ReasonPhrase}");
                string content = Encoding.UTF8.GetString(contentarray);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(content);
                content = @"<?xml version='1.0' encoding='UTF-8'?>";
                content += doc.DocumentElement.ChildNodes[3].InnerXml;

                string pathXml = Path.Combine(Path.GetTempPath(), $"ad{nota.ConsecutivoFE}.xml");
                File.WriteAllText(pathXml, content, Encoding.UTF8);

                ZipArchive archive = new ZipArchive();
                archive.FileName = $"z{nota.ConsecutivoFE}.zip";
                archive.AddFile(GetPdfNotaReporte(nota, nota.ConsecutivoFE, user), "/");
                archive.AddFile(pathXml, "/");
                MemoryStream msZip = new MemoryStream();
                archive.Save(msZip);
                msZip = new MemoryStream(msZip.ToArray());

                Empresas empresas = unitOfWork.Repository<Empresas>().FindById(x => x.Id == nota.EmpresasId, false);

                EmailModelConfig envioEmailConfig = new EmailModelConfig();
                envioEmailConfig.Origen = DApp.Util.EmailOrigen_Facturacion;
                envioEmailConfig.Asunto = $"{nota.Empresas.NumeroIdentificacion};{nota.Empresas.RazonSocial};{nota.Documentos.Prefijo}{nota.Consecutivo};{(nota.Documentos.Transaccion == 3 ? 91 : 92)};{nota.Empresas.RazonSocial}";
                envioEmailConfig.MetodoUso = eventoEnvio;
                envioEmailConfig.Template = "EmailEnvioNotaElectronica";
                envioEmailConfig.Destinatarios.Add(correo);
                envioEmailConfig.ArchivosAdjuntos.Add($"z{nota.ConsecutivoFE}.zip", msZip);
                envioEmailConfig.Datos = new Dictionary<string, string>
                {
                    {"nombreCia",$"{empresas.RazonSocial}" }
                };

                new ConfiguracionEnvioEmailBusinessLogic(this.UnitOfWork).EnviarEmail(envioEmailConfig);

                var job = unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Table
                    .FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Nota && x.IdTipo == nota.Id && !x.Exitoso);
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

        public override Notas Add(Notas data)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            var documento = unitOfWork.Repository<Documentos>().FindById(x => x.Id == data.DocumentosId, false);

            var factura = unitOfWork.Repository<Facturas>().FindById(x => x.Id == data.FacturasId, true);
            if (factura != null && factura.Estadosid == 1087)
            {
                throw new Exception($"La factura {factura.Documentos.Prefijo}-{factura.NroConsecutivo} se encuentra en estado anulada.");
            }

            try
            {
                data.Consecutivo = new GenericBusinessLogic<Documentos>(unitOfWork).GetSecuence($"{documento.Prefijo}");
                data.ConsecutivoFE = new GenericBusinessLogic<Notas>(unitOfWork).GetConsecutivoParaEnvioFE();
                data = unitOfWork.Repository<Notas>().Add(data);
                unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception($"Error obteniendo consecutivo para {data.SedesId}-{documento.Prefijo}. | {e.Message}");
            }
            return data;
        }

        public Notas AprobarNota(Notas nota)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                nota.Documentos = unitOfWork.Repository<Documentos>().FindById(x => x.Id == nota.DocumentosId, false);
                nota.NotasConceptos = unitOfWork.Repository<NotasConceptos>().FindById(x => x.Id == nota.NotasConceptosId, false);
                nota.Facturas = unitOfWork.Repository<Facturas>().FindById(x => x.Id == nota.FacturasId, false);

                var detalles = unitOfWork.Repository<NotasDetalles>().Table.Where(x => x.NotasId == nota.Id);
                nota.ValorTotal = detalles.Sum(x => x.ValorTotal);
                nota.ValorSubtotal = detalles.Sum(x => x.SubTotal);
                nota.ValorDescuentos = detalles.Sum(x => x.SubTotal * (x.PorcDescuento / 100));
                nota.ValorImpuestos = detalles.Sum(x => (x.SubTotal - (x.SubTotal * (x.PorcDescuento / 100))) * (x.PorcImpuesto / 100));
                nota.MontoEscrito = DApp.Util.NumeroEnLetras(nota.ValorTotal);
                if (nota.Documentos.Transaccion == 3 && nota.ValorTotal > nota.Facturas.ValorTotal)
                {
                    throw new Exception($"El valor total de la nota crédito no puede ser superior al valor total de la factura. ( ${nota.Facturas.ValorTotal} )");
                }
                nota.Estadosid = 10085;
                nota = unitOfWork.Repository<Notas>().Modify(nota);

                if (nota.FacturasId != null && nota.Documentos.Transaccion == 3)
                {
                    nota.Facturas.Saldo -= nota.ValorTotal;
                }
                else if (nota.FacturasId != null && nota.Documentos.Transaccion == 4)
                {
                    nota.Facturas.Saldo += nota.ValorTotal;
                }
                if (nota.Facturas.Saldo < 0)
                    nota.Facturas.Saldo = 0;

                nota.Facturas.TieneNotas = true;
                if (nota.Documentos.Transaccion == 3 && nota.NotasConceptos.Codigo == "2")
                {
                    nota.Facturas.Estadosid = unitOfWork.Repository<Estados>().FindById(x => x.Tipo == "FACTURA" && x.Nombre == "ANULADA", false).Id;
                }
                unitOfWork.Repository<Facturas>().Modify(nota.Facturas);

                if (nota.Documentos.Transaccion == 3 && nota.NotasConceptos.Codigo == "1")
                {
                    var pacientesId = detalles.Select(x => x.PacientesId).Distinct().ToList();
                    if (pacientesId.Count > 0)
                    {
                        var listServices = new BlazorUnitWork(UnitOfWork.Settings).Repository<AdmisionesServiciosPrestados>().Table.Include(x => x.Admisiones).Where(x => (x.FacturasId == nota.FacturasId || x.Admisiones.FacturaCopagoCuotaModeradoraId == nota.FacturasId) && pacientesId.Contains(x.Admisiones.PacientesId)).ToList();
                        var listAdminsiones = listServices.Select(x => x.Admisiones).Distinct().ToList();

                        if (listAdminsiones != null && listAdminsiones.Count > 0)
                            foreach (var adm in listAdminsiones)
                            {
                                adm.Facturado = false;
                                adm.FacturaCopagoCuotaModeradoraId = null;
                                unitOfWork.Repository<Admisiones>().Modify(adm);
                            }

                        if (listServices != null && listServices.Count > 0)
                            foreach (var ser in listServices)
                            {
                                ser.Facturado = false;
                                ser.FacturasId = null;
                                ser.FacturasGeneracionId = null;
                                unitOfWork.Repository<AdmisionesServiciosPrestados>().Modify(ser);
                            }
                    }
                }

                if (nota.Documentos.Transaccion == 3 && nota.NotasConceptos.Codigo == "2")
                {
                    var listServices = new BlazorUnitWork(UnitOfWork.Settings).Repository<AdmisionesServiciosPrestados>().Table.Include(x => x.Admisiones).Where(x => x.FacturasId == nota.FacturasId || x.Admisiones.FacturaCopagoCuotaModeradoraId == nota.FacturasId).ToList();
                    var listAdminsiones = listServices.Select(x => x.Admisiones).Distinct().ToList();

                    if (listAdminsiones != null && listAdminsiones.Count > 0)
                        foreach (var adm in listAdminsiones)
                        {
                            adm.Facturado = false;
                            adm.FacturaCopagoCuotaModeradoraId = null;
                            unitOfWork.Repository<Admisiones>().Modify(adm);
                        }

                    if (listServices != null && listServices.Count > 0)
                        foreach (var ser in listServices)
                        {
                            ser.Facturado = false;
                            ser.FacturasId = null;
                            ser.FacturasGeneracionId = null;
                            unitOfWork.Repository<AdmisionesServiciosPrestados>().Modify(ser);
                        }
                }

                unitOfWork.CommitTransaction();
                return nota;
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw e;
            }
        }

        public Notas AnularNota(Notas nota)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                nota.Documentos = unitOfWork.Repository<Documentos>().FindById(x => x.Id == nota.DocumentosId, false);
                nota.Facturas = unitOfWork.Repository<Facturas>().FindById(x => x.Id == nota.FacturasId, false);

                var detalles = unitOfWork.Repository<NotasDetalles>().Table.Where(x => x.NotasId == nota.Id);
                nota.ValorTotal = detalles.Sum(x => x.ValorTotal);
                nota.ValorSubtotal = detalles.Sum(x => x.SubTotal);
                nota.ValorDescuentos = detalles.Sum(x => x.SubTotal * (x.PorcDescuento / 100));
                nota.ValorImpuestos = detalles.Sum(x => (x.SubTotal - (x.SubTotal * (x.PorcDescuento / 100))) * (x.PorcImpuesto / 100));
                nota.MontoEscrito = DApp.Util.NumeroEnLetras(nota.ValorTotal);
                nota.Estadosid = 10086;
                nota = unitOfWork.Repository<Notas>().Modify(nota);

                if (nota.FacturasId != null && nota.Documentos.Transaccion == 3)
                {
                    nota.Facturas.Saldo += nota.ValorTotal;
                }
                else if (nota.FacturasId != null && nota.Documentos.Transaccion == 4)
                {
                    nota.Facturas.Saldo -= nota.ValorTotal;
                }
                var totalFacturasConNotas = unitOfWork.Repository<Notas>().Table.Where(x => x.FacturasId == nota.FacturasId && x.Estadosid == 10085).Count();
                if (totalFacturasConNotas == 0)
                    nota.Facturas.TieneNotas = false;
                unitOfWork.Repository<Facturas>().Modify(nota.Facturas);
                unitOfWork.CommitTransaction();
                return nota;
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw e;
            }
        }
    }
}
