﻿using Blazor.BusinessLogic.Jobs;
using Blazor.BusinessLogic.Models.Enums;
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

        public void SaveJobLog(string nameClass, bool isSuccess, string descripcion = null , string error = null)
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
                    Descripcion = descripcion,
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

        public async Task<bool> EnvioCorreoEventoAcepta()
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            ConfiguracionEnvioEmailJob job = unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Table
                .OrderBy(x => x.CreationDate)
                .FirstOrDefault(x => !x.Exitoso && x.Intentos < 3);

            if (job == null)
            {
                return false;
            }

            try
            {
                if (job.Tipo == (int)TipoDocumento.Factura) // Tipo factura
                {
                    await new FacturasBusinessLogic(UnitOfWork.Settings).EnviarEmail(job.IdTipo, "Envio Factura Evento DIAN", DApp.Util.UserSystem);
                }
                else if (job.Tipo == (int)TipoDocumento.Nota) // Tipo Nota
                {
                    await new NotasBusinessLogic(UnitOfWork.Settings).EnviarEmail(job.IdTipo, "Envio Nota Evento DIAN", DApp.Util.UserSystem);
                }
            }
            catch (Exception ex)
            {
                job.Ejecutado = true;
                job.Exitoso = false;
                job.Intentos++;
                job.Detalle +=  $"Intento {job.Intentos}: {ex.GetFullErrorMessage()}. ";
                unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Modify(job);
            }

            return true;
        }
    }
}