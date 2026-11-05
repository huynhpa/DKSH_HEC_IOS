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
    public class CCCRequestDao
    {
        private GenericRepository<CCCRequestDto> requestRepo;

        private GenericRepository<HomeDashboardDto> homeDashboardRepo;

        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// instance
        /// </summary>
        public CCCRequestDao()
        {
            this.requestRepo = new GenericRepository<CCCRequestDto>(new CCCToolsEntity());
            this.homeDashboardRepo = new GenericRepository<HomeDashboardDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// SEARCH REQUEST
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CCCRequestDto> SearchRequest(CCCRequestParam dto)
        {
            object[] parameter = { dto.TicketNo, dto.Title, dto.ReqStatus, dto.CreateDate, dto.IsDelete, dto.Team };
            string spQuery = @"EXEC [SearchRequest] {0}, {1}, {2}, {3}, {4}, {5}";
            return requestRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// GetTicketNoList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketNoDto> GetTicketNoList()
        {
            var query = (from c in cccToolsEntity.CCCRequests
                         select new TicketNoDto()
                            { TicketId = c.Id,
                              TicketName = c.TicketNo,
                              Title = c.Title }).ToList();
            return query;
        }

        /// <summary>
        /// Detail
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public CCCRequestDto GetRequestDetail(CCCRequestParam pr)
        {
            object[] param = { pr.Id };
            string spQuery = @"EXEC [GetRequestDetail] {0}";
            CCCRequestDto dto = (CCCRequestDto)requestRepo.ExecuteQuerySingle(spQuery, param);
            return dto;
        }

        /// <summary>
        /// insert request
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int InsertRequest(CCCRequestDto param)
        {
            object[] parameters = { param.TicketNo, param.Title, param.Description, param.FileLocation, param.CreateBy, param.UpdateBy,
                                    param.Remark, param.CompleteDate, param.ReqStatus, param.Approval };
            string sqQuery = @"EXEC [InsertRequest] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
            int rs = requestRepo.ExecuteCommand(sqQuery, parameters);
            return rs;
        }

        /// <summary>
        /// update request
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int UpdateRequest(CCCRequestDto param)
        {
            object[] parameters = { param.Id, param.TicketNo, param.Title, param.Description, param.FileLocation, param.ReqStatus, param.UpdateBy,
                                    param.IsDelete, param.Remark, param.Percentage, param.CompleteDate,
                                    param.EstimateDate };
            string sqQuery = @"EXEC [UpdateRequest] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}";
            int rs = requestRepo.ExecuteCommand(sqQuery, parameters);
            return rs;
        }

        /// <summary>
        /// Delete Request
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int DeleteRequest(CCCRequestDto param)
        {
            int count = 0;
            try
            {
                var query = (from c in cccToolsEntity.CCCRequests
                             where c.Id == param.Id
                             select c).First();
                query.IsDelete = true;
                count = cccToolsEntity.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return count;
        }

        /// <summary>
        /// Get Ticket Doing List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CCCRequestDto> GetTicketDoingList()
        {
            object[] param = { };
            string spQuery = @"EXEC [GetTicketDoingList]";
            return requestRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Summary Dashboard
        /// </summary>
        /// <returns></returns>
        public HomeDashboardDto GetSummaryDashboard()
        {
            object[] param = { };
            string spQuery = @"EXEC [HomeDashboard]";
            return homeDashboardRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// approval request
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int ApprovalRequest(CCCRequestDto dto)
        {
            try
            {
                var query = (from r in cccToolsEntity.CCCRequests
                             where r.Id == dto.Id
                             select r).SingleOrDefault();
                query.Approval = dto.Approval;
                int count = cccToolsEntity.SaveChanges();
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}