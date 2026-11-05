using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSHApplication.DTO;
using System.Web;
using NPOI.SS.UserModel;
using DKSHApplication.DB;

namespace DKSHApplication.Service
{
    public interface UpSaleCrossSaleService
    {
        ResultMessageDto UploadSalemanPOText(HttpPostedFileBase aFile, string Monthly);
        ResultMessageDto UploadDataCalls(HttpPostedFileBase aFile, string userId, DataCallsDto dto);
        IEnumerable<SalemanPOTextDto> GetGroupSalemanPOText();
        IEnumerable<SalemanPOTextDto> GetSalemanPO();
        ResultMessageDto SaveDataSale(HttpPostedFileBase file, DataSaleCSUSDto dto);
        IEnumerable<CrossSaleUpSaleReportDto> SearchUpSaleCrossSale(CrossSaleUpSaleReportDto dto);
        ResultMessageDto ProcessUpSaleCrossSale(CrossSaleUpSaleReportDto dto);
        IWorkbook ExportUpSaleCrossSales(IEnumerable<CrossSaleUpSaleReportDto> uscsResult);
        ResultMessageDto DeleteDataSale(DataSaleParam dto);
        IEnumerable<SalemanPOTextDto> GetAllSalemanPOText();
        IEnumerable<DataCallsDto> GetDataCallByMonth(DataCallSalemanParam pr);
    }
}
