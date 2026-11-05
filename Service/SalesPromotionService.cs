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
    public interface SalesPromotionService
    {
        IEnumerable<SalesPromotionDto> SearchSalesPromotion(SalesPromotionDto dto);

        ResultMessageDto SaveSalesPromotion(HttpPostedFileBase aFile, SalesPromotionDto dto);

        ResultMessageDto UpdateSalesPromotion(SalesPromotionDto dto);

        ResultMessageDto GetSuppliers();

        SalesPromotionDto GetSalesPromotionDetail(SalesPromotionDto dto);

        ResultMessageDto SetEntity(SalesPromotionDto dto);

        IEnumerable<SalesPromotionDto> SalesPromotionIdList();

        ResultMessageDto DeleteSalesPromotion(SalesPromotionDto dto);

        IWorkbook Export(IEnumerable<SalesPromotionDto> lst);
        string ExportByEntityApply(SalesPromotionDto dto);
    }
}
