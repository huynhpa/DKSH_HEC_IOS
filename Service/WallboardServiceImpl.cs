using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using MP.Core.Library;
using NPOI.HSSF.UserModel;

namespace DKSHApplication.Service
{
    public class WallboardServiceImpl : WallboardService
    {
        SearchCallDao searchCallDao = new SearchCallDao();
        CampaignDao campaignDao = new CampaignDao();
        WallboardDao wbDao = new WallboardDao();
        RecordsDao recordsDao = new RecordsDao();
        public const string Telesales = "TS";
        public const string TeleCSO = "TS_NEW_CSO";
        public const string VASCampaign = "VASCampaign";

        /// <summary>
        /// Initialize Wallboard
        /// </summary>
        /// <returns></returns>
        public WallboardDto InitializeWallboard()
        {
            WallboardDto wallboard = new WallboardDto();
            DailyCallDto dc = new DailyCallDto();
            dc.TeleTeam = Telesales;
            wallboard.TeleSale = wbDao.DailyCallTele(dc);
            dc.TeleTeam = TeleCSO;
            wallboard.TeleCSO = wbDao.DailyCallTele(dc);
            dc.TeleTeam = VASCampaign;
            wallboard.VAS = wbDao.DailyCallTele(dc);
            wallboard.FocusCampaign = wbDao.VASCampaign();
            wallboard.AchivedPerformance = wbDao.DailyCallMTDSalePerformance();
            return wallboard;
        }



        /// <summary>
        /// SaveRecords
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto SaveRecords()
        {
            MySqlConnectionRecords issabel = new MySqlConnectionRecords();
            RecordsDao recordsDao = new RecordsDao();
            Records46Dto param = new Records46Dto()
            {
                calldate = DateTime.Now.ToString("dd-MM-yyyy"),
                calldate_to = DateTime.Now.ToString("dd-MM-yyyy")
            };
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                DailyCallDto dc = new DailyCallDto()
                {
                    TeleTeam = Telesales
                };
                int del = recordsDao.DeleteRecord46(param);
                DataTable dt = issabel.GetRecordingIssabel(param);
                Records46Dto record = null;
                int rowCount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    record = new Records46Dto()
                    {
                        calldate = dt.Rows[i]["calldate"].ToString(),
                        destination = dt.Rows[i]["dst"].ToString(),
                        duration = dt.Rows[i]["duration"].ToString(),
                        disposition = dt.Rows[i]["disposition"].ToString(),
                        uniqueid = dt.Rows[i]["uniqueid"].ToString(),
                        recordingfile = dt.Rows[i]["recordingfile"].ToString(),
                        cnum = dt.Rows[i]["cnum"].ToString(),
                        contracts = "DKSH"
                    };
                    rowCount += recordsDao.SaveRecordFromServer46(record);
                }
                DataTable dt_gigamed = issabel.GetRecordingIssabelGigamed(param);
                for (int j = 0; j < dt_gigamed.Rows.Count; j++)
                {
                    record = new Records46Dto();
                    record.calldate = dt_gigamed.Rows[j]["calldate"].ToString();
                    record.destination = dt_gigamed.Rows[j]["dst"].ToString();
                    record.duration = dt_gigamed.Rows[j]["duration"].ToString();
                    record.disposition = dt_gigamed.Rows[j]["disposition"].ToString();
                    record.uniqueid = dt_gigamed.Rows[j]["uniqueid"].ToString();
                    record.recordingfile = dt_gigamed.Rows[j]["recordingfile"].ToString();
                    record.cnum = dt_gigamed.Rows[j]["cnum"].ToString();
                    record.contracts = "GIGAMED";
                    rowCount += recordsDao.SaveRecordFromServer46(record);
                }
                // Save dailycallperformance
                rowCount += wbDao.DeleteAchivedPerformance();
                rowCount += wbDao.DeleteDailyCallPerformance();
                DailyCallDto dc_1 = new DailyCallDto()
                { };
                rowCount += wbDao.SaveDailyCallPerformance(dc_1);
                if (rowCount > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.number = rowCount;
                    result.message = string.Format(Constant.PROCESS_MESSAGE, rowCount);
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
        /// SaveVASCampaign & DailyCallMTDSalePerformance
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto SaveVASCampaign()
        {
            ResultMessageDto result = new ResultMessageDto();
            result.number = wbDao.SaveVASCampaign();
            result.number += wbDao.SaveDailyCallMTDSalePerformance();
            return result;
        }
    }
}