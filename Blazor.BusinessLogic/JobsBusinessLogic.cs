using Blazor.BusinessLogic.Jobs;
using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
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

        #region Internal methods

        public void ActualizarJob(Job job, string host)
        {
            try
            {
                BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
                var tenant = DApp.GetTenant(host);
                if (tenant == null)
                {
                    throw new Exception($"No existe tenant para el host {host}");
                }

                var jobBD = unitOfWork.Repository<Job>().FindById(x => x.Id == job.Id, false);
                if (jobBD == null)
                {
                    throw new Exception($"No existe rutina {job.Id}");
                }

                var jobQuartz = JobExecution.Jobs.FirstOrDefault(x => x.TenantCode.Equals(tenant.Code) && x.IdJob == jobBD.Id && x.Class.Equals(jobBD.Class));
                if (jobQuartz == null)
                {
                    throw new Exception($"No existe un job quartz para {tenant.Code}, {jobBD.Id}, {jobBD.Class}");
                }

                var jobKey = new JobKey(jobQuartz.JobKey, jobQuartz.Group);
                var triggerKey = new TriggerKey(jobQuartz.TriggerKey, jobQuartz.Group);

                if (!jobBD.CronSchedule.Equals(job.CronSchedule))
                {
                    jobQuartz.ITrigger = TriggerBuilder.Create()
                                    .WithIdentity(jobQuartz.TriggerKey, jobQuartz.Group)
                                    .WithCronSchedule(job.CronSchedule)
                                    .Build();

                    JobExecution.Scheduler.RescheduleJob(triggerKey, jobQuartz.ITrigger).GetAwaiter().GetResult();
                }

                if (job.Active && !jobBD.Active)
                {
                    JobExecution.Scheduler.ResumeJob(jobKey).GetAwaiter().GetResult();
                }
                else if (!job.Active && jobBD.Active)
                {
                    JobExecution.Scheduler.PauseJob(jobKey).GetAwaiter().GetResult();
                }

                unitOfWork.Repository<Job>().Modify(job);
            }
            catch (Exception ex)
            {
                Console.Write(ex.GetFullErrorMessage());
            }
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

        #endregion

        public async Task EnvioCorreoEventoAcepta()
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            ConfiguracionEnvioEmailJob jobEnvioEmail = unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Table
                .OrderBy(x => x.CreationDate)
                .FirstOrDefault(x => !x.Ejecutado);

            if (jobEnvioEmail == null)
            {
                return;
            }

            try
            {
                if (jobEnvioEmail.Tipo == 1) // Tipo factura
                {
                    Facturas factura = unitOfWork.Repository<Facturas>().Table
                        .Include(x => x.Empresas)
                        .Include(x => x.Documentos)
                        .FirstOrDefault(x => x.Id == jobEnvioEmail.IdTipo);
                    await new FacturasBusinessLogic(UnitOfWork.Settings).EnviarEmail(factura, "Envio Factura Evento DIAN", DApp.Util.UserSystem);
                }
                else if (jobEnvioEmail.Tipo == 2) // Tipo Nota
                {
                    Notas factura = unitOfWork.Repository<Notas>().Table.FirstOrDefault(x => x.Id == jobEnvioEmail.IdTipo);
                    await new NotasBusinessLogic(UnitOfWork.Settings).EnviarEmail(factura, "Envio Nota Evento DIAN", DApp.Util.UserSystem);
                }

            }
            catch (Exception ex)
            {
                jobEnvioEmail.Ejecutado = true;
                jobEnvioEmail.Exitoso = false;
                jobEnvioEmail.Error = ex.GetFullErrorMessage();
                unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Modify(jobEnvioEmail);
            }
        }
    }
}
