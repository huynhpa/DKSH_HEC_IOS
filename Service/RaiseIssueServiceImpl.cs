using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class RaiseIssueServiceImpl : RaiseIssueService
    {
        RaiseIssueDao issueDao = new RaiseIssueDao();

        /// <summary>
        /// Delete Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteIssue(RaiseIssueParam jsonParam)
        {
            ResultMessageDto dto = new ResultMessageDto();
            try
            {
                dto.number = issueDao.DeleteIssue(jsonParam);
                if (dto.number > 0)
                {
                    dto.code = Constant.DELETE;
                    dto.message = Constant.MESSAGE_DELELE;
                }
            }
            catch(Exception ex)
            {
                dto.code = Constant.ERROR;
                dto.message = ex.Message;
            }
            return dto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Issue Detail
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public RaiseIssueDto GetIssueDetail(RaiseIssueParam jsonParam)
        {
            RaiseIssueDto dto = null;
            if (jsonParam != null && jsonParam.Id != 0)
            {
                dto = issueDao.GetIssueDetail(jsonParam);
            }
            return dto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get IssueNo By Ticket
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<RaiseIssueDto> GetIssueNoByTicket(TicketNoDto dto)
        {
            IEnumerable<RaiseIssueDto> IssueNoList = issueDao.GetIssueNoByTicket(dto);
            return IssueNoList;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get IssueNo List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RaiseIssueDto> GetIssueNoList()
        {
            IEnumerable<RaiseIssueDto> lst = issueDao.GetIssueNoList();
            return lst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto InsertIssue(RaiseIssueParam jsonParam)
        {
            ResultMessageDto rs = new ResultMessageDto();
            rs = issueDao.InsertIssue(jsonParam);
            if (rs.number > 0)
            {
                rs.code = Constant.INSERT;
                rs.message = Constant.MESSAGE_SUCCESS;
            }
            else
            {
                rs.code = Constant.ERROR;
                rs.message = Constant.MESSAGE_ERROR;
            }
            return rs;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<RaiseIssueDto> SearchIssue(RaiseIssueParam jsonParam)
        {
            IEnumerable<RaiseIssueDto> issueList = issueDao.SearchIssue(jsonParam);
            return issueList;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateIssue(RaiseIssueParam jsonParam)
        {
            ResultMessageDto rs = new ResultMessageDto();
            try
            {
                rs = issueDao.UpdateIssue(jsonParam);
                if (rs.number > 0)
                {
                    rs.code = Constant.UPDATE;
                    rs.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    rs.code = Constant.ERROR;
                    rs.message = Constant.MESSAGE_ERROR;
                }
            }
            catch(Exception ex)
            {
                rs.code = Constant.ERROR;
                rs.message = ex.Message;
            }            
            return rs;
            throw new NotImplementedException();
        }
    }
}