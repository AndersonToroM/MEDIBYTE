using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.Reports.Facturas;
using Blazor.Reports.Notas;
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
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

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

                    var job = new ConfiguracionEnvioEmailJob
                    {
                        Id = 0,
                        Ejecutado = false,
                        Exitoso = false,
                        UpdatedBy = "Acepta",
                        LastUpdate = DateTime.Now,
                        CreatedBy = "Acepta",
                        CreationDate = DateTime.Now,
                        Intentos = 0
                    };

                    var invoice = manager.GetBusinessLogic<Facturas>().FindById(x => (x.Documentos.Prefijo + x.NroConsecutivo.ToString()) == eventR.NroId, true);
                    if (invoice != null && (string.IsNullOrWhiteSpace(invoice.DIANResponse) || string.IsNullOrWhiteSpace(invoice.DIANResponse) || !invoice.DIANResponse.Contains("Aceptado")))
                    {
                        invoice.CUFE = eventR.Uuid;
                        invoice.IssueDate = eventR.FechaEvento;
                        invoice.DIANResponse = eventR.TipoEvento + " " + eventR.Descripcion;
                        invoice.XmlUrl = eventR.XmlDoc;
                        invoice = manager.GetBusinessLogic<Facturas>().Modify(invoice);

                        job.Tipo = 1;
                        job.IdTipo = invoice.Id;
                        manager.GetBusinessLogic<ConfiguracionEnvioEmailJob>().Add(job);

                        //try
                        //{
                        //    await manager.FacturasBusinessLogic().EnviarEmail(invoice, "Envio Factura Evento DIAN", User.Identity.Name);
                        //}
                        //catch (Exception e)
                        //{
                        //    Console.WriteLine($"Error Enviando el correo. | {e.Message}", Console.ForegroundColor = ConsoleColor.Red);
                        //}

                    }

                    var note = manager.GetBusinessLogic<Notas>().FindById(x => (x.Documentos.Prefijo + x.Consecutivo.ToString()) == eventR.NroId, true);
                    if (note != null && (string.IsNullOrWhiteSpace(note.DIANResponse) || string.IsNullOrEmpty(note.DIANResponse) || !note.DIANResponse.Contains("Aceptado")))
                    {
                        note.CUFE = eventR.Uuid;
                        note.IssueDate = eventR.FechaEvento;
                        note.DIANResponse = eventR.TipoEvento + " " + eventR.Descripcion;
                        note.XmlUrl = eventR.XmlDoc;
                        manager.GetBusinessLogic<Notas>().Modify(note);

                        job.Tipo = 2;
                        job.IdTipo = note.Id;
                        manager.GetBusinessLogic<ConfiguracionEnvioEmailJob>().Add(job);

                        //try
                        //{
                        //    await manager.NotasBusinessLogic().EnviarEmail(note, "Envio Nota Evento DIAN", User.Identity.Name);
                        //}
                        //catch (Exception e)
                        //{
                        //    Console.WriteLine($"Error Enviando el correo. | {e.Message}", Console.ForegroundColor = ConsoleColor.Red);
                        //}
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
