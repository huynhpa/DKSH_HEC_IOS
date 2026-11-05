using DKSHApplication.DAO;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DKSHApplication.Common
{
    public class GetRecordIssabel : IJob
    {        
        public void Execute(IJobExecutionContext context)
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
                int del = recordsDao.DeleteRecord46(param);
                DataTable dt = issabel.GetRecordingIssabel(param);
                
                int rowCount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Records46Dto record_dksh = new Records46Dto()
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
                    rowCount += recordsDao.SaveRecordFromServer46(record_dksh);
                }
                DataTable dt_gigamed = issabel.GetRecordingIssabelGigamed(param);
                for (int i = 0; i < dt_gigamed.Rows.Count; i++)
                {
                    Records46Dto record_ggm = new Records46Dto()
                    {
                        calldate = dt_gigamed.Rows[i]["calldate"].ToString(),
                        destination = dt_gigamed.Rows[i]["dst"].ToString(),
                        duration = dt_gigamed.Rows[i]["duration"].ToString(),
                        disposition = dt_gigamed.Rows[i]["disposition"].ToString(),
                        uniqueid = dt_gigamed.Rows[i]["uniqueid"].ToString(),
                        recordingfile = dt_gigamed.Rows[i]["recordingfile"].ToString(),
                        cnum = dt_gigamed.Rows[i]["cnum"].ToString(),
                        contracts = "GIGAMED"
                    };
                    rowCount += recordsDao.SaveRecordFromServer46(record_ggm);
                }
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
        }

    }
}