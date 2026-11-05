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
    public interface TransferNotificationService
    {
        ResultMessageDto UploadAndProcess(HttpPostedFileBase curDate, string userId, UploadStatusDto UploadStatus);
        
        IEnumerable<InvoiceZZ24Dto> TransferNotificationList(InvoiceZZ24Dto param);

        IEnumerable<InvoiceDetailDto> ViewInvoiceDetail(InvoiceZZ24Dto json);

        string ExportPDF(InvoiceZZ24Dto param);

        ResultMessageDto ShareAllInvoice(InvoiceZZ24Dto invoice);

        ResultMessageDto SendInvoice(InvoiceZZ24Dto json);

        ResultMessageDto DownloadInvoices(InvoiceZZ24Dto json);

        ResultMessageDto DeleteInvoice(InvoiceZZ24Dto json);

        string ExportExcel(InvoiceZZ24Dto invoice);
        ResultMessageDto ConvertPDF(string fileLocation, string fileNm);
        IEnumerable<InvoiceZZ24Dto> TransferNotifiLog(InvoiceZZ24Dto json);
        string ExportPDFIsDelete(InvoiceZZ24Dto invoice);
        string ExportExcelTransferLog(InvoiceZZ24Dto invoice);
        IEnumerable<InvoiceDetailDto> ViewInvoiceDetailTransferLog(InvoiceZZ24Dto json);
    }
}
