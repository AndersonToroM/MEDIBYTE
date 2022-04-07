using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using DevExpress.Compression;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WebApp.Reportes.Facturas;
using WebApp.Reportes.Notas;

namespace Blazor.WebApp.Controllers
{


    [AllowAnonymous]
    public class EventController : BaseAppController
    {
        public EventController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] XElement evento)
        {
            try
            {
                var ser = new XmlSerializer(typeof(Evento));
                var retr = new XmlSerializer(typeof(Retorno));
                string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + evento.ToString();
                using (var sr = new StringReader(xml))
                {
                    var data = (Evento)ser.Deserialize(sr);

                    EventosDIAN eventR = new EventosDIAN();
                    eventR.NroId = data.Identificador.Id;
                    eventR.NumDocEmisor = data.Identificador.NumDocEmisor;
                    eventR.NumDocReceptor = data.Identificador.NumDocReceptor;

                    eventR.TipoEvento = data.TipoEvento;
                    eventR.CodigoEvento = data.CodigoEvento;
                    eventR.Descripcion = data.Descripcion;
                    eventR.Uri = data.Uri;

                    eventR.TipoDoc = data.Identificador.TipoDoc;
                    eventR.SubtipoDoc = data.Identificador.SubtipoDoc;
                    eventR.NroId = data.Identificador.Id;

                    eventR.Uuid = data.Identificador.Uuid;
                    eventR.FechaEmision = data.Identificador.FechaEmision;
                    eventR.FechaEvento = DateTime.Parse(data.Identificador.FechaEvento);
                    eventR.XmlDoc = data.XmlDoc;
                    eventR.Pdf = data.Pdf;

                    eventR.UpdatedBy = "Acepta";
                    eventR.LastUpdate = DateTime.Now;
                    eventR.CreatedBy = "Acepta";
                    eventR.CreationDate = DateTime.Now;

                    DataBaseSetting conexionTenant = DApp.GetTenantConnection(Request.Host.Value);
                    Dominus.Backend.DataBase.BusinessLogic manager = new Dominus.Backend.DataBase.BusinessLogic(conexionTenant);

                    manager.GetBusinessLogic<EventosDIAN>().Add(eventR);

                    var invoice = manager.GetBusinessLogic<Facturas>().FindById(x => (x.Documentos.Prefijo + x.NroConsecutivo.ToString()) == eventR.NroId, true);
                    if (invoice != null && (string.IsNullOrWhiteSpace(invoice.DIANResponse) || string.IsNullOrWhiteSpace(invoice.DIANResponse) || !invoice.DIANResponse.Contains("Aceptado")))
                    {
                        invoice.CUFE = eventR.Uuid;
                        invoice.IssueDate = eventR.FechaEvento;
                        invoice.DIANResponse = eventR.TipoEvento + " " + eventR.Descripcion;
                        invoice.XmlUrl = eventR.XmlDoc;
                        invoice = manager.GetBusinessLogic<Facturas>().Modify(invoice);
                        try
                        {
                            await manager.FacturasBusinessLogic().EnviarEmail(invoice, GetPdfFacturaReporte(invoice, manager), "Envio Factura Evento DIAN");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error Enviando el correo. | {e.Message}", Console.ForegroundColor = ConsoleColor.Red);
                        }

                    }

                    var note = manager.GetBusinessLogic<Notas>().FindById(x => (x.Documentos.Prefijo + x.Consecutivo.ToString()) == eventR.NroId, true);
                    if (note != null && (string.IsNullOrWhiteSpace(note.DIANResponse) || string.IsNullOrEmpty(note.DIANResponse) || !note.DIANResponse.Contains("Aceptado")))
                    {
                        note.CUFE = eventR.Uuid;
                        note.IssueDate = eventR.FechaEvento;
                        note.DIANResponse = eventR.TipoEvento + " " + eventR.Descripcion;
                        note.XmlUrl = eventR.XmlDoc;
                        manager.GetBusinessLogic<Notas>().Modify(note);
                        try
                        {
                            await manager.NotasBusinessLogic().EnviarEmail(note, GetPdfNotaReporte(note, manager), "Envio Nota Evento DIAN");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error Enviando el correo. | {e.Message}", Console.ForegroundColor = ConsoleColor.Red);
                        }
                    }

                    if (invoice == null && note == null)
                        throw new Exception($"El documento {eventR.NroId} no fue encontrado en el sistema.");

                    string xmlret = $@"<?xml version=""1.0"" encoding=""UTF-8""?><Retorno><CodRespuesta>1</CodRespuesta><DescRespuesta>Evento agregado para el documento {eventR.NroId}</DescRespuesta></Retorno>";
                    return new OkObjectResult(xmlret);
                }
            }
            catch (Exception e)
            {
                string xmlret = @"<?xml version=""1.0"" encoding=""UTF-8""?><Retorno><CodRespuesta>2</CodRespuesta><DescRespuesta>" + e.GetFullErrorMessage() + "</DescRespuesta></Retorno>";
                return new BadRequestObjectResult(xmlret);
            }
        }

        private string GetPdfFacturaReporte(Facturas factura, Dominus.Backend.DataBase.BusinessLogic manager)
        {
            InformacionReporte informacionReporte = new InformacionReporte();
            informacionReporte.Empresa = manager.GetBusinessLogic<Empresas>().FindById(x => x.Id == factura.EmpresasId, true);
            informacionReporte.BD = DApp.GetTenantConnection(Request.Host.Value);
            informacionReporte.Ids = new long[] { factura.Id };

            XtraReport xtraReport = null;
            if (factura.AdmisionesId != null)
            {
                FacturasParticularReporte report = new FacturasParticularReporte(informacionReporte);
                xtraReport = report;
            }
            else
            {
                FacturasReporte report = new FacturasReporte(informacionReporte);
                xtraReport = report;
            }

            string pathPdf = Path.Combine(Path.GetTempPath(), $"{factura.Documentos.Prefijo}-{factura.NroConsecutivo}.pdf");
            PdfExportOptions pdfOptions = new PdfExportOptions();
            pdfOptions.ConvertImagesToJpeg = false;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;
            pdfOptions.PdfACompatibility = PdfACompatibility.PdfA2b;
            xtraReport.ExportToPdf(pathPdf, pdfOptions);
            return pathPdf;
        }

        private string GetPdfNotaReporte(Notas nota, Dominus.Backend.DataBase.BusinessLogic manager)
        {
            InformacionReporte informacionReporte = new InformacionReporte();
            informacionReporte.Empresa = manager.GetBusinessLogic<Empresas>().FindById(x => x.Id == nota.EmpresasId, true);
            informacionReporte.BD = DApp.GetTenantConnection(Request.Host.Value);
            informacionReporte.Ids = new long[] { nota.Id };

            NotasReporte report = new NotasReporte(informacionReporte);
            XtraReport xtraReport = report;

            string pathPdf = Path.Combine(Path.GetTempPath(), $"{nota.Documentos.Prefijo}-{nota.Consecutivo}.pdf");
            PdfExportOptions pdfOptions = new PdfExportOptions();
            pdfOptions.ConvertImagesToJpeg = false;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;
            pdfOptions.PdfACompatibility = PdfACompatibility.PdfA2b;
            xtraReport.ExportToPdf(pathPdf, pdfOptions);
            return pathPdf;
        }
    }

    public class Evento
    {

        public string TipoEvento { get; set; }

        public string CodigoEvento { get; set; }

        public string Descripcion { get; set; }

        public string Uri { get; set; }

        [XmlElementAttribute("Identificador")]
        public Identificador1 Identificador { get; set; }

        public string XmlDoc { get; set; }

        public string Pdf { get; set; }

    }

    public class Retorno
    {
        public string CodRespuesta { get; set; }

        public string DescRespuesta { get; set; }
    }

    public class Identificador1
    {
        public string NumDocEmisor { get; set; }

        public string NumDocReceptor { get; set; }

        public string TipoDoc { get; set; }

        public string SubtipoDoc { get; set; }

        public string Id { get; set; }

        public string Uuid { get; set; }

        [XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime FechaEmision { get; set; }

        public string FechaEvento { get; set; }

        public string Observacion { get; set; }
    }

}
