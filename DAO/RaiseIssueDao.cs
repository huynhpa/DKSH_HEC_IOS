using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class RaiseIssueDao
    {
        private GenericRepository<RaiseIssueDto> issueRepo;

        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// INSTANCE
        /// </summary>
        public RaiseIssueDao()
        {
            issueRepo = new GenericRepository<RaiseIssueDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search Issue
        /// </summary>
        /// <param name="jsonPr"></param>
        /// <returns></returns>
        public IEnumerable<RaiseIssueDto> SearchIssue(RaiseIssueParam jsonPr)
        {
            object[] param = { jsonPr.TicketNo, jsonPr.Title, jsonPr.IssueStatus, jsonPr.IsDelete, jsonPr.RequestId, jsonPr.IssueNo };
            string spQuery = "EXEC [SearchIssue] {0}, {1}, {2}, {3}, {4}, {5}";
            return issueRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Insert Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto InsertIssue(RaiseIssueParam jsonParam)
        {
            ResultMessageDto rsDto = new ResultMessageDto();
            object[] param = {  jsonParam.Title, jsonParam.TicketNo, jsonParam.Description, jsonParam.RequestId,
                                jsonParam.CreateBy, jsonParam.UpdateBy, jsonParam.IssueStatus, jsonParam.Percentage, jsonParam.IssueNo };
            string spQuery = @"EXEC [InsertIssue] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            rsDto.number =  issueRepo.ExecuteCommand(spQuery, param);
            return rsDto;
        }

        /// <summary>
        /// Update Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateIssue(RaiseIssueParam jsonParam)
        {
            ResultMessageDto rsDto = new ResultMessageDto();
            object[] param = {  jsonParam.Id, jsonParam.TicketNo, jsonParam.Title, jsonParam.Description,
                                jsonParam.UpdateBy, jsonParam.IssueStatus, jsonParam.Percentage, jsonParam.IssueNo, jsonParam.IsDelete };
            string spQuery = @"EXEC [UpdateIssue] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            rsDto.number = issueRepo.ExecuteCommand(spQuery, param);
            return rsDto;
        }

        /// <summary>
        /// Get Issue Detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public RaiseIssueDto GetIssueDetail(RaiseIssueParam param)
        {
            object[] sqlParam = { param.Id};
            string spQuery = "EXEC [GetIssueDetail] {0}";
            return issueRepo.ExecuteQuerySingle(spQuery, sqlParam);
        }

        /// <summary>
        /// Delete Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public int DeleteIssue(RaiseIssueParam jsonParam)
        {
            int num = 0;
            var query = (from r in cccToolsEntity.CCCRaiseIssues
                         where r.Id == jsonParam.Id
                         select r).First();
            query.IsDelete = true;
            query.UpdateBy = jsonParam.UpdateBy;
            try
            {
                num = cccToolsEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return num;
        }

        /// <summary>
        /// Get IssueNo List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RaiseIssueDto> GetIssueNoList()
        {
            var query = from i in cccToolsEntity.CCCRaiseIssues
                        select new RaiseIssueDto()
                        {
                            Id = i.Id,
                            IssueNo = i.IssueNo,
                            Title = i.Title
                        };
            return query.ToList();
        }

        /// <summary>
        /// Get IssueNo By Ticket
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RaiseIssueDto> GetIssueNoByTicket(TicketNoDto param)
        {
            IEnumerable<RaiseIssueDto> issueNoList;
            try
            {
                var query = from i in cccToolsEntity.CCCRaiseIssues
                            where i.RequestId == param.TicketId
                            select new RaiseIssueDto()
                            {
                                Id = i.Id,
                                IssueNo = i.IssueNo,
                                Title = i.Title
                            };
                issueNoList = query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            return issueNoList;
        }
    }
}