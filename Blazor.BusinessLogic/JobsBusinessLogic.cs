using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Reports.RadicacionCuentas;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        #region SaveLog

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

        #endregion

        public async Task EnvioCorreoEventoAcepta()
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            ConfiguracionEnvioEmailJob data = unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Table
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault(x => !x.Ejecutado);

            if (data == null)
            {
                return;
            }

            try
            {
                if (data.Tipo == 1) // Tipo factura
                {
                    Facturas factura = unitOfWork.Repository<Facturas>().Table
                        .Include(x=> x.Empresas)
                        .Include(x=> x.Documentos)
                        .FirstOrDefault(x => x.Id == data.IdTipo);
                    await new FacturasBusinessLogic(UnitOfWork.Settings).EnviarEmail(factura, "Envio Factura Evento DIAN", DApp.Util.UserSystem);
                }
                else if (data.Tipo == 2) // Tipo Nota
                {
                    Notas factura = unitOfWork.Repository<Notas>().Table.FirstOrDefault(x => x.Id == data.IdTipo);
                    await new NotasBusinessLogic(UnitOfWork.Settings).EnviarEmail(factura, "Envio Nota Evento DIAN", DApp.Util.UserSystem);
                }

                data.Ejecutado = true;
                data.Exitoso = true;
            }
            catch (Exception ex)
            {
                data.Ejecutado = true;
                data.Exitoso = false;
                data.Error = ex.GetFullErrorMessage();
            }

            unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Modify(data);

        }
    }
}
