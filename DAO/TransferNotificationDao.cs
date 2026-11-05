using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class TransferNotificationDao
    {
        private GenericRepository<InvoiceDetailDto> InvoiceDetailRepo;

        private GenericRepository<InvoiceZZ24Dto> InvoiceZZ24Repo;

        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// Instance
        /// </summary>
        public TransferNotificationDao()
        {
            InvoiceDetailRepo = new GenericRepository<InvoiceDetailDto>(new CCCToolsEntity());
            InvoiceZZ24Repo = new GenericRepository<InvoiceZZ24Dto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Get Invoice ZZ24 By Delivery
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public InvoiceZZ24Dto GetInvoiceZZ24ByDelivery(InvoiceZZ24Dto dto)
        {
            object[] param = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = "EXEC [GetInvoiceZZ24ByDelivery] {0}, {1}, {2}";
            return InvoiceZZ24Repo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Get InvoiceZZ24 ByDelivery IsDelete
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public InvoiceZZ24Dto GetInvoiceZZ24ByDeliveryIsDelete(InvoiceZZ24Dto dto)
        {
            object[] param = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = "EXEC [sp_GetInvoiceZZ24ByDeliveryIsDelete] {0}, {1}, {2}";
            return InvoiceZZ24Repo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Search Invoice ZZ24
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceZZ24Dto> SearchInvoice(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.InvoiceNo, dto.DeliveryNo, dto.CustCode, dto.CreateDate };
            string spQuery = @"EXEC [SearchInvoiceZZ24] {0}, {1}, {2}, {3}";
            return InvoiceZZ24Repo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Search Invoice Non Group By Customer And Plant
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceZZ24Dto> SearchInvoiceNonGroupByCustomerAndPlant(InvoiceZZ24Dto dto)
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_SearchInvoiceNonGroupByCustomerAndPlant]";
            return InvoiceZZ24Repo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Search Invoice Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceDetailDto> SearchInvoiceDetail(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant , dto.CreateDate };
            string spQuery = @"EXEC [SearchInvoiceDetail] {0}, {1}, {2}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceDetailDto> SearchInvoiceDetailIsDelete(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = @"EXEC [SearchInvoiceDetailIsDelete] {0}, {1}, {2}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }


        /// <summary>
        /// Search Invoice Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceDetailDto> SearchInvoiceDetailUngroup(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = @"EXEC [sp_SearchInvoiceDetailUngroup] {0}, {1}, {2}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }

        public IEnumerable<InvoiceDetailDto> SearchInvoiceDetailUngroupIsDelete(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = @"EXEC [sp_SearchInvoiceDetailUngroupIsDelete] {0}, {1}, {2}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Sum Invoice Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public InvoiceDetailDto SumInvoiceDetail(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = @"EXEC [SumInvoiceDetail] {0}, {1}, {2}";
            return InvoiceDetailRepo.ExecuteQuerySingle(spQuery, parameter);
        }


        /// <summary>
        /// Search Invoice Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceDetailDto> CountInvoiceByCustomerAndPlant(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = @"EXEC [sp_CountInvoiceByCustomerAndPlant] {0}, {1}, {2}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }


        public IEnumerable<InvoiceDetailDto> CountInvoiceByCustomerAndPlantIsDelete(InvoiceZZ24Dto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate };
            string spQuery = @"EXEC [sp_CountInvoiceByCustomerAndPlantIsDelete] {0}, {1}, {2}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }


        /// <summary>
        /// Merge Invoice By Delivery
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceDetailDto> MergeInvoiceByDelivery(InvoiceDetailDto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate, dto.DeliveryNo };
            string spQuery = @"EXEC [sp_MergeInvoiceByDelivery] {0}, {1}, {2}, {3}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }


        public IEnumerable<InvoiceDetailDto> MergeInvoiceByDeliveryIsDelete(InvoiceDetailDto dto)
        {
            object[] parameter = { dto.CustCode, dto.Plant, dto.CreateDate, dto.DeliveryNo };
            string spQuery = @"EXEC [sp_MergeInvoiceByDeliveryIsDelete] {0}, {1}, {2}, {3}";
            return InvoiceDetailRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Insert Invoice ZZ24
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int InsertInvoiceZZ24(InvoiceZZ24Dto jsonParam)
        {
            object[] parameter = { jsonParam.Id, jsonParam.CustCode, jsonParam.CustName, jsonParam.Addrs, jsonParam.InvoiceNo,
                                   jsonParam.InvoiceDate, jsonParam.Amount, jsonParam.PaymentTerm, jsonParam.Plant,
                                   jsonParam.UpdateBy, jsonParam.CreateBy, jsonParam.TransferNumber, jsonParam.DeliveryNo,
                                   jsonParam.RedInvoice };
            string spQuery = @"EXEC [InsertInvoiceZZ24] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}";
            return InvoiceZZ24Repo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Delete invoice to prevent duplicate
        /// </summary>
        /// <returns></returns>
        public int DeleteAllInvoiceToPreventDuplicate(UploadStatusDto dto)
        {
            object[] parameter = { dto.Status, dto.CurrentDate };
            string spQuery = @"EXEC [DeleteAllInvoiceToPreventDuplicate] {0}, {1}";
            return InvoiceZZ24Repo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Invoice Detail
        /// </summary>
        /// <returns></returns>
        public int InsertInvoiceDetail(InvoiceDetailDto jsonParam)
        {
            object[] param = { jsonParam.Id, jsonParam.Material, jsonParam.DeliveryNo, jsonParam.Descriptions, jsonParam.DeliveryQty,
                               jsonParam.UpdateBy, jsonParam.CreateBy, jsonParam.TransferNumber };
            string spQuery = @"EXEC [InsertInvoiceDetail] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return InvoiceDetailRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update Share Status
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public int UpdateShareStatus(InvoiceZZ24Dto invoice)
        {
            object[] param = { invoice.CustCode, invoice.Plant, invoice.ShareStatus, invoice.Flag, invoice.UpdateBy, invoice.CreateDate };
            string spQuery = @"EXEC [UpdateShareStatus] {0}, {1}, {2}, {3}, {4}, {5}";
            return InvoiceZZ24Repo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Get Invoice By Delivery
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceZZ24Dto> GetInvoiceByDelivery(InvoiceZZ24Dto invoice)
        {
            object[] parameter = { invoice.DeliveryNoList };
            string spQuery = @"EXEC [GetInvoiceByDelivery] {0}";
            return InvoiceZZ24Repo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Delete Invoice
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public int DeleteInvoice(InvoiceZZ24Dto jsonParam)
        {
            object[] parameter = { jsonParam.CreateDate };
            string spQuery = @"EXEC [DeleteInvoice] {0}";
            return InvoiceZZ24Repo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// UpdateInvoiceNoAndCustCodeDetails
        /// </summary>
        /// <returns></returns>
        public int UpdateInvoiceNoAndCustCodeDetails()
        {
            object[] parameter = {};
            string spQuery = @"EXEC [sp_UpdateInvoiceNoAndCustCodeDetails]";
            return InvoiceZZ24Repo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Transfer NotifiLog
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<InvoiceZZ24Dto> TransferNotifiLog(InvoiceZZ24Dto json)
        {
            object[] parameter = { json.InvoiceNo, json.DeliveryNo, json.CustCode, json.CreateDate };
            string spQuery = @"EXEC [sp_SearchInvoiceZZ24IsDelete] {0}, {1}, {2}, {3}";
            return InvoiceZZ24Repo.ExecuteQuery(spQuery, parameter);
        }
    }
}