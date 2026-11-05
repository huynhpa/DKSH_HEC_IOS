using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class MarketingDataRawDao
    {
        private GenericRepository<MarketingDataRawDto> dataRawRepo;

        private GenericRepository<SalemanCodeDto> salemanCodeRepo;

        private GenericRepository<DataRawResult> dataRawResultRepo;

        private GenericRepository<CustomerDto> customerRepo;

        private GenericRepository<ProductsGroupDto> productGroupRepo;

        private CCCToolsEntity cccToolsEntity = null;

        public MarketingDataRawDao()
        {
            dataRawRepo = new GenericRepository<MarketingDataRawDto>(new CCCToolsEntity());
            salemanCodeRepo = new GenericRepository<SalemanCodeDto>(new CCCToolsEntity());
            dataRawResultRepo = new GenericRepository<DataRawResult>(new CCCToolsEntity());
            productGroupRepo = new GenericRepository<ProductsGroupDto>(new CCCToolsEntity());
            customerRepo = new GenericRepository<CustomerDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Get Marketing Project By Project Number
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public IEnumerable<MarketingDataRawDto> GetMarketingProjectByProjectNumber(MarketingProjectDto projectDto)
        {
            object[] param = { projectDto.ProjectNumber };
            string spQuery = @"EXEC [GetMarketingProjectByProjectNumber] {0}";
            return dataRawRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Insert data raw from Processing
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertDataRaw (MarketingDataRawDto dto)
        {
            object[] param = { dto.MarketingProjectId, dto.Product, dto.ProductName, dto.CustCode,
                               dto.CustName, dto.Addr, dto.Provine, dto.CustGroup,
                               dto.Region, dto.District, dto.BillingDate, dto.Qty,
                               dto.SalemanCode, dto.TT, dto.TTV, dto.ORDER_NO, dto.SODoc_Date, dto.Id, dto.PRINCIPAL, dto.Reason,
                               dto.CGrpCode, dto.PickedOriginal, dto.Channel, dto.CreateBy, dto.UpdateBy };
            string spQuery = @"EXEC [InsertMarketingDataRaw] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10},
                                                             {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19},
                                                             {20}, {21}, {22}, {23}, {24}";
            return dataRawRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Delete Marketing Data Raw
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public int DeleteMarketingDataRaw(MarketingProjectDto projectDto)
        {
            object[] param = { projectDto.ProjectNumber, projectDto.LastDateProcess };
            string spQuery = @"EXEC [DeleteMarketingDataRaw] {0}, {1}";
            return dataRawRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Count Data Raw By Project Number
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public IEnumerable<DataRawResult> CountDataRawByProjectNumber(ProjectProcessParam ProjectList)
        {
            object[] param = { ProjectList.ProjectNumberList };
            string spQuery = @"EXEC [CountDataRawByProjectNumber] {0}";
            return dataRawResultRepo.ExecuteQuery(spQuery, param);
            //try
            //{
            //    var query = (from p in cccToolsEntity.MarketingDataRaws
            //                 join c in cccToolsEntity.MarketingProjects on p.MarketingProjectId equals c.ProjectNumber
            //                 where p.MarketingProjectId.Contains(ProjectNumberList)
            //                 group new { p,c } by new { p.MarketingProjectId, c.ProjectName } into gr                           
            //                 select new DataRawResult
            //                 {
            //                     MarketingProjectId = gr.Key.MarketingProjectId,
            //                     ProjectName = gr.Key.ProjectName,
            //                     DataRawCount = gr.Count()
            //                 }).ToList();                
            //    return query;
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// Check OrderNo VA05
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public MarketingDataRawDto CheckOrderNoVA05(MarketingDataRawDto dto)
        {
            object[] param = { dto.ORDER_NO, dto.MarketingProjectId };
            string spQuery = @"EXEC [CheckOrderNoVA05] {0}, {1}";
            return dataRawRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Get Customer By Cust Code
        /// </summary>
        /// <param name="CustCode"></param>
        /// <returns></returns>
        public CustomerDto GetCustomerByCustCode(string CustCode)
        {
            object[] param = { CustCode };
            string spQuery = @"EXEC [GetCustomerByCustCode] {0}";
            return customerRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// GET MARKETING DATA RAW DETAIL
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public IEnumerable<MarketingDataRawDto> MarketingDataRawDetail(MarketingProjectDto projectDto)
        {
            object[] param = { projectDto.ProjectNumber };
            string spQuery = @"EXEC [MarketingDataRawDetail] {0}";
            return dataRawRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// GET SALE MAN CODE HAS DATA SALE
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public IEnumerable<SalemanCodeDto> GetSalemanDataRaw(MarketingProjectDto projectDto)
        {
            object[] param = { projectDto.ProjectNumber };
            string spQuery = @"EXEC [GetSalemanDataRaw] {0}";
            return salemanCodeRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Find customer code sum quantity > 0
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public IEnumerable<MarketingDataRawDto> GetCustCodeLargerThanZero(MarketingProjectDto projectDto)
        {
            object[] param = { projectDto.ProjectNumber };
            string spQuery = @"EXEC [GetCustCodeLargerThanZero] {0}";
            return dataRawRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Channel By CGrpCode
        /// </summary>
        /// <param name="custgroup"></param>
        /// <returns></returns>
        public string GetChannelByCGrpCode(string custgroup)
        {
            try
            {
                string channel = string.Empty;
                var query = (from c in cccToolsEntity.CGrpCodes
                             where c.CGroupCode.ToUpper().Equals(custgroup.ToUpper())
                             select c.Channel).FirstOrDefault();
                channel = query;
                return channel;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete All Data By Project Number
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public int DeleteAllDataByProjectNumber(MarketingProjectDto projectDto)
        {
            object[] param = { projectDto.ProjectNumber };
            string spQuery = @"EXEC [DeleteAllDataByProjectNumber] {0}";
            return dataRawRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// SUM QUANTITY BY REGION
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public IEnumerable<MarketingDataRawDto> SumByRegion(MarketingProjectDto projectDto)
        {
            object[] param = { projectDto.ProjectNumber };
            string spQuery = @"EXEC [SumByRegion] {0}";
            return dataRawRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Sum Amount By Sale Man Code
        /// </summary>
        /// <param name="projectObj"></param>
        /// <returns></returns>
        public IEnumerable<MarketingDataRawDto> SumAmountBySaleManCode(MarketingProjectDto projectObj)
        {
            object[] param = { projectObj.ProjectNumber };
            string spQuery = @"EXEC [SumAmountBySaleManCode] {0}";
            return dataRawRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Sum amount by region
        /// </summary>
        /// <param name="projectObj"></param>
        /// <returns></returns>
        public IEnumerable<MarketingDataRawDto> SumAmountByRegion(MarketingProjectDto projectObj)
        {
            object[] param = { projectObj.ProjectNumber };
            string spQuery = @"EXEC [SumAmountByRegion] {0}";
            return dataRawRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Summary By Product Group
        /// </summary>
        /// <param name="prj"></param>
        /// <returns></returns>
        public ProductsGroupDto SummaryByProductGroup(ProductsGroupDto prj)
        {
            object[] param = { prj.ProjectNumber };
            string spQuery = "EXEC [sp_SummaryByProductGroup] {0}";
            return productGroupRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Get Product Group By Project
        /// </summary>
        /// <param name="prj"></param>
        /// <returns></returns>
        public IEnumerable<ProductsGroupDto> GetProductGroupByProject(MarketingProjectDto prj)
        {
            object[] param = { prj.ProjectNumber };
            string spQuery = @"EXEC [sp_GetProductGroupByProject] {0}";
            return productGroupRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Insert Summary Product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateSummaryProduct(ProductsGroupDto dto)
        {
            object[] param = { dto.ProjectNumber, dto.ProductGroup, dto.ProductCode,
                               Convert.ToInt32(dto.SumQuantity), Convert.ToDouble(dto.SumAmount) };
            string spQuery = @"EXEC [sp_UpdateSummaryProduct] {0}, {1}, {2}, {3}, {4}";
            return productGroupRepo.ExecuteCommand(spQuery, param);
        }


        /// <summary>
        /// Get Product By Group
        /// </summary>
        /// <param name="prj"></param>
        /// <returns></returns>
        public IEnumerable<ProductsGroupDto> GetProductByGroup(ProductsGroupDto prj)
        {
            object[] param = { prj.ProjectNumber };
            string spQuery = "EXEC [sp_GetProductByGroup] {0}";
            return productGroupRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Summary Data By Product Group
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ProductsGroupDto SummaryDataByProductGroup(ProductsGroupDto item)
        {
            object[] param = { item.ProjectNumber, item.ProductCode };
            string spQuery = "EXEC [sp_SummaryDataByProductGroup] {0}, {1}";
            return productGroupRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Summary Product Group
        /// </summary>
        /// <param name="prj"></param>
        /// <returns></returns>
        public IEnumerable<ProductsGroupDto> SummaryProductGroup(MarketingProjectDto prj)
        {
            object[] param = { prj.ProjectNumber };
            string spQuery = "EXEC [sp_SummaryProductGroup] {0}";
            return productGroupRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// UpdateDataPick
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal int UpdateDataPick(MarketingDataRawDto obj)
        {
            object[] param = { obj.Id, obj.PRINCIPAL, obj.Product, obj.CustCode, obj.CustName, obj.Provine , obj.CustGroup, obj.Region, obj.District,
                            obj.VBELN, obj.BillingDate, obj.Reason, obj.Qty, obj.SalemanCode, obj.ORDER_NO, obj.VAT, obj.TT, obj.TTV };
            string spQuery = @"EXEC [sp_UpdateDataPick] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10},
                                                        {11}, {12}, {13}, {14}, {15}, {16}, {17}";
            return productGroupRepo.ExecuteCommand(spQuery, param);
        }
    }
}