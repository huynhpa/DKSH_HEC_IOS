using DKSHApplication.DAO;
using DKSHApplication.DTO;
using Quartz;
using System;
using System.IO;
using System.Text;

namespace DKSHApplication.Common
{
    public class TradeSales : IJob
    {        
        public void Execute(IJobExecutionContext context)
        {
            OBSDashboardDao dashboard = new OBSDashboardDao();
            var filePath = @"D:\OSS\Dev_Publish\ExcelTemplate\";
            string message = string.Empty;
            try
            {
                
                TradeSalesDto tradeSales = new TradeSalesDto()
                {
                    InvoiceDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
                };
                int trade_sale = dashboard.SaveTradeSales(tradeSales);
                if (trade_sale > 0)
                {
                    message = Constant.SUCCESS;
                }
                else
                {
                    message = Constant.NO_ROW_SAVE;
                }
                // write log
                StringBuilder sb = new StringBuilder();
                sb.Append("************** SAVE DATA SALE *****************\n");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt\n"));
                sb.Append("Number rows affect: ");
                sb.Append(trade_sale);
                File.AppendAllText(filePath + "log.txt", sb.ToString());
                sb.Clear();
            }
            catch (Exception ex)
            {
                StringBuilder error = new StringBuilder();
                error.Append("*************** ERROR SAVE DATA SALE ****************\n");
                error.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + "\n");
                error.Append(ex.Message + " AND " + message + "\n");
                File.AppendAllText(filePath + "log.txt", error.ToString());
                error.Clear();
            }

        }

    }
}