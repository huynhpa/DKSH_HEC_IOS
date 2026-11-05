using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.Common;
using DKSHApplication.DB;

namespace DKSHApplication.DAO
{
    public class ProductInformationDao
    {
        public GenericRepository<ProductInformationDto> productInfoRepo;

        private CCCToolsEntity cccToolsEntity = null;
        public ProductInformationDao()
        {
            productInfoRepo = new GenericRepository<ProductInformationDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public IEnumerable<ProductInformationDto> SearchProduct(ProductInformationDto productInfo)
        {
            object[] param = { productInfo.Supplier, productInfo.ProductID, productInfo.ProductName, productInfo.Category, productInfo.ProductGroup };
            string spQuery = @"EXEC [sp_SearchProduct] {0}, {1}, {2}, {3}, {4}";
            return productInfoRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Suppliers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductInformationDto> GetSuppliers()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_GetSuppliers]";
            return productInfoRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// Delete Product Information
        /// </summary>
        /// <returns></returns>
        public int DeleteProductInformation()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_DeleteProductInformation]";
            return productInfoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Insert Product Info
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public int InsertProductInfo(ProductInformationDto productInfo)
        {
            object[] param = {  productInfo.Supplier, productInfo.ProductID, productInfo.ProductName, productInfo.PrdHierarchy, productInfo.Category,
                                productInfo.Brand, productInfo.ProductGroup, productInfo.ProductSubGroup, productInfo.VAT,
                                productInfo.SellingPrice, productInfo.GenericName, productInfo.UnitOfMeasure, productInfo.SalesUnit };
            string spQuery = @"EXEC [sp_InsertProductInformation] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}";
            return productInfoRepo.ExecuteCommand(spQuery, param);
        }
    }
}