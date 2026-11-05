using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Common
{
    public class JobScheduler
    {
        public static void Start()
        {
            // Trigger : get call plan from SFA and import into table [CallPlan] start at 06:30 AM daily
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            IJobDetail job = JobBuilder.Create<TradeSales>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(8, 5))
                  )
                .Build();
            scheduler.ScheduleJob(job, trigger);

            // dashboard
            IScheduler scheduler_dashboard = StdSchedulerFactory.GetDefaultScheduler();
            scheduler_dashboard.Start();
            IJobDetail job_04 = JobBuilder.Create<Dashboard>().Build();
            ITrigger trigger_04 = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(8, 20))
                  )
                .Build();
            scheduler_dashboard.ScheduleJob(job_04, trigger_04);


            // TalkTime
            IScheduler scheduler_talktime = StdSchedulerFactory.GetDefaultScheduler();
            scheduler_talktime.Start();
            IJobDetail job_05 = JobBuilder.Create<TalkTime>().Build();
            ITrigger trigger_05 = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(8, 30))
                  )
                .Build();
            scheduler_talktime.ScheduleJob(job_05, trigger_05);

            // Get record_ggm from issabel server and import into table [Records46] start at 20:00 daily
            IScheduler scheduler_record = StdSchedulerFactory.GetDefaultScheduler();
            scheduler_record.Start();
            IJobDetail job_01 = JobBuilder.Create<GetRecordIssabel>().Build();
            ITrigger trigger_01 = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(18, 0))
                  )
                .Build();
            scheduler_record.ScheduleJob(job_01, trigger_01);

            // Campaign result report daily
            IScheduler scheduler_report = StdSchedulerFactory.GetDefaultScheduler();
            scheduler_report.Start();
            IJobDetail job_02 = JobBuilder.Create<CampaignResultReport>().Build();
            ITrigger trigger_02 = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(19, 0))
                  )
                .Build();
            scheduler_report.ScheduleJob(job_02, trigger_02);


            // Get contact iMind data
            IScheduler scheduler_contactiMind = StdSchedulerFactory.GetDefaultScheduler();
            scheduler_contactiMind.Start();
            IJobDetail job_03 = JobBuilder.Create<ContactiMind>().Build();
            ITrigger trigger_03 = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(20, 30))
                  )
                .Build();
            scheduler_contactiMind.ScheduleJob(job_03, trigger_03);

        }
    }
}