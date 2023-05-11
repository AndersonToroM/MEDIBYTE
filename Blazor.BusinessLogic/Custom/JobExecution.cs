using Blazor.Infrastructure.Entities;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;


namespace Blazor.BusinessLogic.Jobs
{

    // docs: https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontriggers.html#cron-expressions
    // cron: http://www.cronmaker.com/?1 or 

    #region JOB EXECUTION

    public class JobExecution
    {
        public static Dictionary<string, IScheduler> DicScheduler = new Dictionary<string, IScheduler>();
        public static List<JobData> Jobs { get; set; } = new List<JobData>();

        public async Task RunJobs()
        {
            try
            {
                if (DApp.Tenants != null && DApp.Tenants.Count > 0)
                {
                    foreach (var tenant in DApp.Tenants)
                    {
                        DataBaseSetting BD = tenant.DataBaseSetting;
                        if (!BD.TurnOnJobs)
                        {
                            return;
                        }

                        // Grab the Scheduler instance from the Factory
                        NameValueCollection props = new NameValueCollection { { "quartz.serializer.type", "binary" } };
                        StdSchedulerFactory factory = new StdSchedulerFactory(props);
                        IScheduler scheduler = await factory.GetScheduler();

                        // and start it off
                        await scheduler.Start();

                        List<Job> jobs = new GenericBusinessLogic<Job>(BD).FindAll(x => x.Active);
                        foreach (var job in jobs)
                        {
                            Type type = Type.GetType("Blazor.BusinessLogic.Jobs." + job.Class);
                            if (type != null)
                            {
                                JobData jobData = new JobData();
                                jobData.Class = job.Class;
                                jobData.CronExpression = job.CronSchedule;
                                jobData.ConnectionName = tenant.Code;

                                jobData.IJobDetail = JobBuilder.Create(type)
                                .WithIdentity($"Job_{job.Id}_{jobData.Class}", jobData.Class)
                                .UsingJobData("TenantCode", tenant.Code)
                                .Build();

                                jobData.ITrigger = TriggerBuilder.Create()
                                    .WithIdentity($"Trigger_{job.Id}_{jobData.Class}", jobData.Class)
                                    .WithCronSchedule(job.CronSchedule)
                                    //.WithSimpleSchedule(x=> x.WithIntervalInSeconds(30).RepeatForever())
                                    .Build();

                                await scheduler.ScheduleJob(jobData.IJobDetail, jobData.ITrigger);
                                Jobs.Add(jobData);
                                Console.WriteLine($"Rutina {jobData.Class} para {tenant.Code} creada - " + DateTime.Now.ToLongTimeString() + " -> " + job.Description);
                            }
                            else
                            {
                                Console.WriteLine($" ::::::::::: Clase ({job.Class} para {tenant.Code}) no existe o esta mal ubicado en el proyecto ::::::::::: ");
                            }

                        }
                        DicScheduler.Add(tenant.Code, scheduler);
                    }
                }

            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class JobData
    {
        public IJobDetail IJobDetail { get; set; }
        public ITrigger ITrigger { get; set; }
        public string Class { get; set; }
        public string CronExpression { get; set; }
        public string ConnectionName { get; set; }
    }

    public class ConsoleLogProvider : ILogProvider
    {
        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (level >= LogLevel.Info && func != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                }
                return true;
            };
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
