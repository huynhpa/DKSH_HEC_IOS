using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.DAO;
using DKSHApplication.Common;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;

namespace DKSHApplication.Service
{
    public class ScheduleTimerServiceImpl : ScheduleTimerService
    {
        ScheduleTimerDao shiftScheduleDao = new ScheduleTimerDao();

        public ShiftAgentsDto CheckAgentExisted(ShiftAgentsDto dto)
        {
            ShiftAgentsDto result = shiftScheduleDao.CheckAgentExisted(dto);
            return result;
        }

        /// <summary>
        /// Export Schedule
        /// </summary>
        /// <param name="scheduleTimerDto"></param>
        /// <returns></returns>
        public IWorkbook ExportSchedule(ScheduleTimerDto scheduleTimerDto)
        {
            throw new NotImplementedException();
        }

        public ShiftAgentsDto GetAgentInfo(ShiftAgentsDto agent)
        {
            ShiftAgentsDto result = shiftScheduleDao.GetAgentInfo(agent);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public ResultMessageDto NewAgents(ShiftAgentsDto agent)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (agent.IsBackUp == null)
                {
                    agent.IsBackUp = false;
                }
                if (agent.IsActive == null)
                {
                    agent.IsActive = false;
                }
                if (agent.IsUsed == null)
                {
                    agent.IsUsed = false;
                }
                result.number = shiftScheduleDao.NewAgents(agent);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<ShiftAgentsDto> SearchAgents(ShiftAgentsDto dto)
        {
            IEnumerable<ShiftAgentsDto> result = shiftScheduleDao.SearchAgents(dto);
            return result;
        }

        /// <summary>
        /// Search Schedule Timer
        /// </summary>
        /// <param name="scheduleTimerDto"></param>
        /// <returns></returns>
        public IEnumerable<ScheduleTimerDto> SearchScheduleTimer(ScheduleTimerDto scheduleTimerDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Agents
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateAgents(ShiftAgentsDto agent)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (agent.IsBackUp == null)
                {
                    agent.IsBackUp = false;
                }
                if (agent.IsActive == null)
                {
                    agent.IsActive = false;
                }
                if (agent.IsUsed == null)
                {
                    agent.IsUsed = false;
                }
                result.number = shiftScheduleDao.UpdateAgents(agent);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Generate Date
        /// </summary>
        /// <param name="dto"></param>
        private ResultMessageDto GenerateDate(List<ScheduleTimerDto> dtos)
        {
            ResultMessageDto result = new ResultMessageDto();
            ShiftAgentsDto agent = new ShiftAgentsDto();
            agent.IsUsed = true;
            IEnumerable<ShiftAgentsDto> agents = shiftScheduleDao.SearchAgents(agent);
            foreach (ScheduleTimerDto item in dtos)
            {
                item.ShiftId = Guid.NewGuid();
                result.number += shiftScheduleDao.GenerateDate(item);
            }
            if (result.number > 0)
            {
                result.code = Constant.SUCCESS;
                result.message = Constant.MESSAGE_SUCCESS;
            }
            return result;
        }
    }
}