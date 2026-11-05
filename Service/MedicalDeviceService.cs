using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface MedicalDeviceService
    {        

        ResultMessageDto UploadVA05(HttpPostedFileBase file);
        IWorkbook ExportResult();
        ResultMessageDto UploadSellingQuota(HttpPostedFileBase file, string userId, string UpdateDate);
        IEnumerable<SellingQuotaDto> SearchSellingQuota(SellingQuotaDto jsonParam);
        IWorkbook ExportResult(SellingQuotaDto jsonParam);
        ResultMessageDto FetchingEmail(ReadMailDto rm);
    }
}
