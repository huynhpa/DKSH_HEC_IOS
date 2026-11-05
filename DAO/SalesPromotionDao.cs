using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class SalesPromotionDao
    {
        private GenericRepository<SalesPromotionDto> SalesPromotionRepo;
        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// INITIALIZE
        /// </summary>
        public SalesPromotionDao()
        {
            SalesPromotionRepo = new GenericRepository<SalesPromotionDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search SalesPromotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SalesPromotionDto> SearchSalesPromotion(SalesPromotionDto dto)
        {
            object[] param = { dto.Suppliers, dto.StartDate, dto.EndDate, dto.CreateDateFrom, dto.CreateDateTo,
                dto.NhaThuoc, dto.NhaThuocBV, dto.CongtyDuoc, dto.SalesPromotionId };
            string spQuery = "exec sp_SearchSalesPromotion {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            return SalesPromotionRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Save SalesPromotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveSalesPromotion(SalesPromotionDto dto)
        {
            object[] param = { dto.Id, dto.SalesPromotionId, dto.Title , dto.ProductCode, dto.ProductName, dto.SalesContent, dto.ApplyTime,
                               dto.EntityApply, dto.ApplyArea, dto.Remark, dto.CreateBy, dto.UpdateBy, dto.StartDate, dto.EndDate,
                               dto.StartDate1, dto.EndDate1 };
            string spQuery = "exec sp_SaveSalesPromotion {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}";
            return SalesPromotionRepo.ExecuteCommand(spQuery, param);
        }
        

        /// <summary>
        /// Save SalesPromotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateSalesPromotion(SalesPromotionDto dto)
        {
            object[] param = { dto.Id, dto.SalesContent, dto.UpdateBy, dto.NhaThuoc, dto.NhaThuocBV, dto.CongtyDuoc };
            string spQuery = "exec sp_UpdateSalesPromotion {0}, {1}, {2}, {3}, {4}, {5}";
            return SalesPromotionRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// GetSalesPromotionDetail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal SalesPromotionDto GetSalesPromotionDetail(SalesPromotionDto dto)
        {
            object[] param = { dto.Id };
            string spQuery = "exec sp_GetSalesPromotionDetail {0}";
            return SalesPromotionRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// SetEntity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SetEntity(SalesPromotionDto dto)
        {
            object[] param = { dto.SPList, dto.NhaThuoc, dto.NhaThuocBV, dto.CongtyDuoc };
            string spQuery = "exec sp_SetEntity {0}, {1}, {2}, {3}";
            return SalesPromotionRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Sales Promotion Id List
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<SalesPromotionDto> SalesPromotionIdList()
        {
            object[] param = { };
            string spQuery = "exec sp_SalesPromotionIdList";
            return SalesPromotionRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Delete Sales Promotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteSalesPromotion(SalesPromotionDto dto)
        {
            object[] param = { dto.SPList };
            string spQuery = "exec sp_DeleteSalesPromotion {0}";
            return SalesPromotionRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Export By Entity Apply
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SalesPromotionDto> ExportByEntityApply(SalesPromotionDto dto)
        {
            object[] param = { dto.Entity };
            string spQuery = "exec sp_ExportByEntityApply {0}";
            return SalesPromotionRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Count Product By Sales Promotion
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<SalesPromotionDto> CountSalesPromotion()
        {
            object[] param = { };
            string spQuery = "exec sp_CountProductBySalesPromotion";
            return SalesPromotionRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Count Product By Sales PromotionNumber
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SalesPromotionDto> CountProductBySalesPromotionNumber(SalesPromotionDto dto)
        {
            object[] param = { dto.SalesPromotionId };
            string spQuery = "exec sp_CountProductBySalesPromotionNumber {0}";
            return SalesPromotionRepo.ExecuteQuery(spQuery, param);
        }
    }
}