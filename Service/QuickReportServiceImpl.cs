using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class QuickReportServiceImpl : QuickReportService
    {
        QuickReportDao quickReportDao = new QuickReportDao();

        /// <summary>
        /// Delete Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public QuickReportResult DeleteQuickReport(QuickReportDto json)
        {
            QuickReportResult rs = new QuickReportResult();
            rs.number = quickReportDao.DeleteQuickReport(json);
            if (rs.number > 0)
            {                
                rs.code = Constant.DELETE;
                rs.message = Constant.MESSAGE_DELELE;
                json = new QuickReportDto();
                rs.QuickReportList = quickReportDao.SearchQuickReport(json);
            }
            return rs;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Quick Report Detail
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public QuickReportDto GetQuickReportDetail(QuickReportDto json)
        {
            QuickReportDto obj = quickReportDao.GetQuickReportDetail(json);
            return obj;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public QuickReportResult InsertQuickReport(QuickReportDto json)
        {
            QuickReportResult rs = new QuickReportResult();
            rs.number = quickReportDao.InsertQuickReport(json);
            if (rs.number == 1)
            {
                rs.code = Constant.INSERT;
                rs.message = Constant.MESSAGE_SUCCESS;
                json = new QuickReportDto();
                rs.QuickReportList = quickReportDao.SearchQuickReport(json);
            }
            return rs;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search quick report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<QuickReportDto> SearchQuickReport(QuickReportDto json)
        {
            IEnumerable<QuickReportDto> qrLst = quickReportDao.SearchQuickReport(json);
            return qrLst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public QuickReportResult UpdateQuickReport(QuickReportDto json)
        {
            QuickReportResult rs = new QuickReportResult();
            rs.number = quickReportDao.UpdateQuickReport(json);
            if (rs.number == 1)
            {
                rs.code = Constant.UPDATE;
                rs.message = Constant.MESSAGE_SUCCESS;
                json = new QuickReportDto();
                rs.QuickReportList = quickReportDao.SearchQuickReport(json);
            }
            return rs;
            throw new NotImplementedException();
        }
    }
}