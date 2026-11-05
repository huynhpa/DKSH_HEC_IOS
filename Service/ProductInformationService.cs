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
    public interface ProductInformationService
    {
        IEnumerable<ProductInformationDto> GetSuppliers(ProductInformationDto jsonParam);
        IEnumerable<ProductInformationDto> SearchProduct(ProductInformationDto jsonParam);
    }
}
