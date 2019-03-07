using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using Ninject.Activation;
using Quartz;
using Quartz.Impl;
using MVC.Service;
using System.Threading.Tasks;
namespace MVC.WEB.ScheduleJob
{
    public class JobScheduler : IJob
    {

        public async static void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            IScheduler sched = await schedFact.GetScheduler();
            sched.Start();

            IJobDetail job = JobBuilder.Create<JobScheduler>().Build();

            //Build a trigger that will fire daily at 10:42 am:
            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(11, 25))
            .Build();

            await sched.ScheduleJob(job, trigger);
        }

        async  System.Threading.Tasks.Task IJob.Execute(IJobExecutionContext context)
        {

            var result = await DeleteAllFile(System.Web.Hosting.HostingEnvironment.MapPath("/PdfFiles/"));

        }
        public async Task<string> DeleteAllFile(string path)
        {
            
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                await Task.Delay(1000);
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}