using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DKSHApplication.Service
{
    public class ProductInformationServiceImpl : ProductInformationService
    {
        ProductInformationDao productInfoDao = new ProductInformationDao();

        /// <summary>
        /// Get Suppliers
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<ProductInformationDto> GetSuppliers(ProductInformationDto jsonParam)
        {
            IEnumerable<ProductInformationDto> result = productInfoDao.GetSuppliers();
            return result;
        }

        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<ProductInformationDto> SearchProduct(ProductInformationDto jsonParam)
        {
            IEnumerable<ProductInformationDto> result = productInfoDao.SearchProduct(jsonParam);
            return result;
        }

    }
}