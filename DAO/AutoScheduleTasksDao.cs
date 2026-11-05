using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class AutoScheduleTasksDao
    {
        private GenericRepository<AutoScheduleTasksDto> autoScheduleRepo;

        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// instance
        /// </summary>
        public AutoScheduleTasksDao()
        {
            this.autoScheduleRepo = new GenericRepository<AutoScheduleTasksDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search Auto Schedule Tasks
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<AutoScheduleTasksDto> SearchAutoScheduleTasks(AutoScheduleTasksDto dto)
        {
            object[] parameter = { dto.SentDatetime, dto.TaskName };
            string spQuery = @"EXEC [sp_GetAutoScheduleTasks] {0}, {1}";
            return autoScheduleRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Auto Schedule Daily Task
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int AutoScheduleDailyTask(AutoScheduleTasksDto param)
        {
            object[] parameters = { param.ScheduleId, param.TaskName, param.SentStatus };
            string sqQuery = @"EXEC [sp_AutoScheduleDailyTask] {0}, {1}, {2}";
            int rs = autoScheduleRepo.ExecuteCommand(sqQuery, parameters);
            return rs;
        }
    }
}