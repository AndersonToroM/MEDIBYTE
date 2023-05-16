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

    public class JobExecution
    {
        public static IScheduler Scheduler = null;
        public static List<JobData> Jobs { get; set; } = new List<JobData>();

        public async Task RunJobs()
        {
            try
            {
                if (DApp.Tenants != null && DApp.Tenants.Count > 0)
                {
                    // Grab the Scheduler instance from the Factory
                    NameValueCollection props = new NameValueCollection { { "quartz.serializer.type", "binary" } };
                    StdSchedulerFactory factory = new StdSchedulerFactory(props);
                    Scheduler = await factory.GetScheduler();

                    foreach (var tenant in DApp.Tenants)
                    {
                        DataBaseSetting BD = tenant.DataBaseSetting;
                        if (!BD.TurnOnJobs)
                        {
                            return;
                        }

                        // and start it off
                        await Scheduler.Start();

                        List<Job> jobs = new GenericBusinessLogic<Job>(BD).FindAll(x => x.Active);
                        foreach (var job in jobs)
                        {
                            Type type = Type.GetType("Blazor.BusinessLogic.Jobs." + job.Class);
                            if (type != null)
                            {
                                JobData jobData = new JobData();
                                jobData.IdJob = job.Id;
                                jobData.Class = job.Class;
                                jobData.TenantCode = tenant.Code;
                                jobData.CronExpression = job.CronSchedule;

                                jobData.IJobDetail = JobBuilder.Create(type)
                                .WithIdentity(jobData.JobKey, jobData.Group)
                                .UsingJobData("TenantCode", tenant.Code)
                                .Build();

                                jobData.ITrigger = TriggerBuilder.Create()
                                    .WithIdentity(jobData.TriggerKey, jobData.Group)
                                    .WithCronSchedule(jobData.CronExpression)
                                    //.WithSimpleSchedule(x=> x.WithIntervalInSeconds(30).RepeatForever())
                                    .Build();

                                await Scheduler.ScheduleJob(jobData.IJobDetail, jobData.ITrigger);
                                Jobs.Add(jobData);
                                Console.WriteLine($"Rutina Id={jobData.JobKey} creada - " + DateTime.Now.ToLongTimeString() + " -> " + job.Description);
                            }
                            else
                            {
                                Console.WriteLine($" ::::::::::: Id={job.Id} Clase ({job.Class} para {tenant.Code}) no existe o esta mal ubicado en el proyecto ::::::::::: ");
                            }

                        }
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
        public long IdJob { get; set; }
        public string Class { get; set; }
        public string CronExpression { get; set; }
        public string TenantCode { get; set; }
        public string Group
        {
            get { return $"{TenantCode}_{Class}"; }
        }
        public string JobKey
        {
            get { return $"Job_{TenantCode}_{IdJob}_{Class}"; }
        }
        public string TriggerKey
        {
            get { return $"Trigger_{TenantCode}_{IdJob}_{Class}"; }
        }
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

}
