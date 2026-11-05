using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.DAO;
using DKSHApplication.Common;

namespace DKSHApplication.Service
{
    public class HumanAllocationServiceImpl : HumanAllocationService
    {
        HumanAllocationDao humanAllocationDao = new HumanAllocationDao();

        public IEnumerable<AgentJobsDto> GetAgentJob()
        {
            IEnumerable<AgentJobsDto> rs = humanAllocationDao.GetAgentJob();
            return rs;
        }

        

        /// <summary>
        /// Get Agent List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AgentsDto> GetAgentList()
        {
            IEnumerable<AgentsDto> result = humanAllocationDao.GetAgentList();
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Campaign List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HumanAllocationDto> GetCampaignList()
        {
            IEnumerable<HumanAllocationDto> result = humanAllocationDao.GetCampaignList();
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Login Or Logout By Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public HumanAllocationDto GetLoginOrLogoutByAgent(HumanAllocationDto dto)
        {
            try
            {
                HumanAllocationDto humanDto = humanAllocationDao.GetLoginOrLogoutByAgent(dto);
                return humanDto;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        

        /// <summary>
        /// LogIn And LogOut
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto LogInCallTime(HumanAllocationDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                
                dto.Id = Guid.NewGuid();
                dto.LoginStatus = "1"; // Login
                result.number = humanAllocationDao.LogInCallTime(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Log Out Call Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto LogOutCallTime(HumanAllocationDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                HumanAllocationDto detail = humanAllocationDao.GetHumanAllocationDetail(dto);
                dto.LogoutStatus = "1"; // Logout
                if (string.IsNullOrEmpty(dto.SignInTime))
                {
                    dto.SignInTime = detail.SignInTime_AMPM;
                }
                if (string.IsNullOrEmpty(dto.SignOutTime))
                {
                    dto.SignOutTime = detail.SignOutTime_AMPM;
                }
                result.number = humanAllocationDao.LogOutCallTime(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
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
        /// Search Human Allocation
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<HumanAllocationDto> SearchHumanAllocation(HumanAllocationDto dto)
        {
            if (string.IsNullOrEmpty(dto.CreateDate))
            {
                dto.CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
            }
            IEnumerable<HumanAllocationDto> result = humanAllocationDao.SearchHumanAllocation(dto);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<LogAgentJobDto> SearchLogAgentJob(LogAgentJobDto param)
        {
            IEnumerable<LogAgentJobDto> rs = humanAllocationDao.SearchLogAgentJob(param);
            return rs;
        }

        /// <summary>
        /// Get Agent Job Detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public LogAgentJobDto GetAgentJobDetail(LogAgentJobDto param)
        {
            LogAgentJobDto logAgentJobs = humanAllocationDao.GetAgentJobDetail(param);
            return logAgentJobs;
        }

        /// <summary>
        /// Insert Log Agent Job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto InsertLogAgentJob(LogAgentJobDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                param.Id = Guid.NewGuid();
                result.number = humanAllocationDao.InsertLogAgentJob(param);
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
        /// Update Log Agent Job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateLogAgentJob(LogAgentJobDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                LogAgentJobDto detail = humanAllocationDao.GetAgentJobDetail(param);
                if (string.IsNullOrEmpty(param.SignInTime))
                {
                    param.SignInTime = detail.SignInTime_AMPM;
                }
                if (string.IsNullOrEmpty(param.SignOutTime))
                {
                    param.SignOutTime = detail.SignOutTime_AMPM;
                }
                result.number = humanAllocationDao.UpdateLogAgentJob(param);
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
    }
}