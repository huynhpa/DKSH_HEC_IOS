using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class QuickReportDao
    {
        private GenericRepository<QuickReportDto> quickReportRepo;

        private GenericRepository<QuickReportResult> quickReportResultRepo;

        private CCCToolsEntity cccToolsEntity = null;

        public QuickReportDao()
        {
            quickReportRepo = new GenericRepository<QuickReportDto>(new CCCToolsEntity());
            quickReportResultRepo = new GenericRepository<QuickReportResult>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<QuickReportDto> SearchQuickReport(QuickReportDto json)
        {
            object[] param = { json.RequestId, json.IssueId, json.Title, json.IsDelete };
            string spQuery = "EXEC [SearchQuickReport] {0}, {1}, {2}, {3}";
            return quickReportRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// GetQuickReportDetail
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public QuickReportDto GetQuickReportDetail(QuickReportDto json)
        {
            object[] param = { json.Id };
            string spQuery = "EXEC [GetQuickReportDetail] {0}";
            return quickReportRepo.ExecuteQuerySingle(spQuery, param);
        }


        /// <summary>
        /// Insert Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public int InsertQuickReport(QuickReportDto json)
        {
            object[] param = { json.RequestId, json.IssueId, json.Title, json.Descriptions, json.CreateBy, json.UpdateBy };
            string spQuery = "EXEC [InsertQuickReport] {0}, {1}, {2}, {3}, {4}, {5}";
            return quickReportResultRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public int UpdateQuickReport(QuickReportDto json)
        {
            object[] param = { json.Id, json.Title, json.Descriptions, json.UpdateBy };
            string spQuery = "EXEC [UpdateQuickReport] {0}, {1}, {2}, {3}";
            return quickReportResultRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Delete Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public int DeleteQuickReport(QuickReportDto json)
        {
            var query = (from q in cccToolsEntity.QuickReports
                        where q.Id == json.Id
                        select q).First();
            query.IsDelete = true;
            int rs = 0;
            try
            {
                rs = cccToolsEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                rs = -1;
                throw ex;
            }
            return rs;
        }
    }
}