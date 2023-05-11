using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Reports.RadicacionCuentas;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blazor.BusinessLogic
{
    public class JobsBusinessLogic : GenericBusinessLogic<Job>
    {
        public JobsBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public JobsBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public void SaveJobLog(string nameClass, bool isSuccess, string error = null)
        {
            try
            {
                BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
                var idJob = unitOfWork.Repository<Job>().FindById(x => string.Equals(x.Class, nameClass), false).Id;
                JobLog jobLog = new JobLog
                {
                    Id = 0,
                    JobId = idJob,
                    DateExecution = DateTime.Now,
                    IsSuccess = isSuccess,
                    Error = error,
                    CreatedBy = "Rutina",
                    UpdatedBy = "Rutina",
                    CreationDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };

                unitOfWork.Repository<JobLog>().Add(jobLog);
            }
            catch (Exception ex)
            {
                Console.Write(ex.GetFullErrorMessage());
            }
        }

        public void PruebaDeRutina()
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            RadicacionCuentas data = unitOfWork.Repository<RadicacionCuentas>().Table.FirstOrDefault();

            if (data == null)
            {
                throw new Exception("No se encontro datos en RadicacionCuentas");
            }

            EmailModelConfig envioEmailConfig = new EmailModelConfig();
            envioEmailConfig.Origen = DApp.Util.EmailOrigen_PorDefecto;
            envioEmailConfig.Asunto = $"Test rutina - envia un formato de radicacion cuentas";
            envioEmailConfig.MetodoUso = "Test rutina";
            envioEmailConfig.Template = "EmailPruebaEnvioCorreo";
            envioEmailConfig.Destinatarios.Add("edwin.aguiar@outlook.com");
            envioEmailConfig.Destinatarios.Add("anderson.toromuriel@gmail.com");
            envioEmailConfig.ArchivosAdjuntos.Add($"RadicacionCuentasReporte-{data.Consecutivo}.pdf", GetPdfRadicacionCuentasReporte(data));
            envioEmailConfig.Datos = new Dictionary<string, string>
                {
                    {"nombreCia", DApp.Util.UserSystem }
                };

            new ConfiguracionEnvioEmailBusinessLogic(this.UnitOfWork).EnviarEmail(envioEmailConfig);

        }

        private Stream GetPdfRadicacionCuentasReporte(RadicacionCuentas data)
        {
            XtraReport xtraReport = ReportExtentions.Report<RadicacionCuentasReporte>(this.BusinessLogic, data.Id);
            string pathPdf = Path.Combine(Path.GetTempPath(), $"RadicacionCuentasReporte-{data.Consecutivo}.pdf");
            PdfExportOptions pdfOptions = new PdfExportOptions();
            pdfOptions.ConvertImagesToJpeg = false;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;
            pdfOptions.PdfACompatibility = PdfACompatibility.PdfA2b;
            xtraReport.ExportToPdf(pathPdf, pdfOptions);
            return new MemoryStream(File.ReadAllBytes(pathPdf));
        }
    }
}
