using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class SellingQuotaDao
    {
        private GenericRepository<SellingQuotaDto> sellingQuota;
        private CCCToolsEntity cccToolsEntity = null;

        public SellingQuotaDao()
        {
            sellingQuota = new GenericRepository<SellingQuotaDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search Selling Quota
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SellingQuotaDto> SearchSellingQuota(SellingQuotaDto dto)
        {
            if (!string.IsNullOrEmpty(dto.UpdateDateFrom) && string.IsNullOrEmpty(dto.UpdateDateTo))
            {
                dto.UpdateDateTo = dto.UpdateDateFrom;
            }
            else if (string.IsNullOrEmpty(dto.UpdateDateFrom) && !string.IsNullOrEmpty(dto.UpdateDateTo))
            {
                dto.UpdateDateFrom = dto.UpdateDateTo;
            }
            object[] parameter = { dto.CustCode, dto.CustName, Utils.StringDateFormat(dto.UpdateDateFrom)
                    , Utils.StringDateFormat(dto.UpdateDateTo), dto.ProductCode, dto.ProductName, dto.District
                    , dto.Contract, dto.Quota, dto.SAPContractID };
            string spQuery = @"EXEC [sp_SearchSellingQuota] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
            return sellingQuota.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Insert Selling Quota
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertSellingQuota(SellingQuotaDto dto)
        {
            object[] parameter = { dto.CustCode,dto.CustName,dto.AddressLocation,dto.District,dto.Province,dto.Region,dto.Client
                                    ,dto.ProductCode,dto.ProductName,dto.QuotaUploadUnit,dto.QuotaBox,dto.ContractNo
                                    ,dto.SAPContractID,dto.ContractValidOn,dto.ContractValidTo,dto.QuotaValidOn
                                    ,dto.QuotaValidTo,dto.SaleOrg,dto.DC,dto.Remark,dto.UpdateBy,Utils.StringDateFormat(dto.UpdateDate) };
            string spQuery = @"EXEC [sp_InsertSellingQuota] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13},
                                       {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}";
            return sellingQuota.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Delete Selling Quota
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int DeleteSellingQuota(SellingQuotaDto dto)
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_DeleteSellingQuota]";
            return sellingQuota.ExecuteCommand(spQuery, parameter);
        }
    }
}