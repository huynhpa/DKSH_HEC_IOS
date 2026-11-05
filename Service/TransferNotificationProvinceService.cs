using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DKSHApplication.Service
{
    public interface TransferNotificationProvinceService
    {
        /// <summary>
        /// Upload & Process
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        ResultMessageDto UploadAndProcess(HttpPostedFileBase aFile);

        /// <summary>
        /// Search Invoice
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<TransferNotificationProvinceDto> SearchInvoice(TransferNotificationProvinceDto dto);

        /// <summary>
        /// Upload Email
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        ResultMessageDto UploadEmail(HttpPostedFileBase aFile);

        /// <summary>
        /// export pdf
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        string ExportPDF(TransferNotificationProvinceDto invoice);

        /// <summary>
        /// Get Invoince Detail Province
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<TransferNotificationProvinceDto> GetInvoinceDetailProvince(TransferNotificationProvinceDto dto);

        /// <summary>
        /// Download Invoices
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto DownloadInvoices(TransferNotificationProvinceDto jsonParam);
        
        /// <summary>
        /// Send invoice
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto SendInvoice(TransferNotificationProvinceDto dto);
    }
}
