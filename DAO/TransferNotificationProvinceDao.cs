using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class TransferNotificationProvinceDao
    {
        private GenericRepository<TransferNotificationProvinceDto> tfProvinceRepo;

        private GenericRepository<CustomerEmailDto> custEmailRepo;

        private CCCToolsEntity cccToolsEntity = null;

        public TransferNotificationProvinceDao()
        {
            tfProvinceRepo = new GenericRepository<TransferNotificationProvinceDto>(new CCCToolsEntity());
            custEmailRepo = new GenericRepository<CustomerEmailDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Insert Transfer Notification Province
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public int InsertTransferNotificationProvince(TransferNotificationProvinceDto jsonParam)
        {
            object[] parameter = { jsonParam.Id, jsonParam.SONumber, jsonParam.CustCode, jsonParam.ProductName,
                                   jsonParam.OrderQty, jsonParam.Amount, jsonParam.OrderDate, jsonParam.Plant };
            string spQuery = @"EXEC [sp_InsertTransferNotificationProvince] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return tfProvinceRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Delete Transfer Notification Province
        /// </summary>
        /// <param name="dto">SO NUMBER</param>
        /// <returns></returns>
        public int DeleteTransferNotificationProvince()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_DeleteTransferNotificationProvince]";
            return tfProvinceRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Customer Emails
        /// </summary>
        /// <param name="para">CustomerEmailDto</param>
        /// <returns></returns>
        public int InsertCustomerEmails(CustomerEmailDto para)
        {
            object[] parameter = { para.Id, para.CustCode, para.Email1, para.Email2, para.Email3, para.Phone };
            string spQuery = @"EXEC [sp_InsertCustomerEmails] {0}, {1}, {2}, {3}, {4}, {5}";
            return custEmailRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Search Invoice
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IEnumerable<TransferNotificationProvinceDto> SearchInvoice(TransferNotificationProvinceDto para)
        {
            object[] parameter = { para.CustCode, Utils.StringDateFormat(para.CreateDateFrom), Utils.StringDateFormat(para.CreateDateTo) };
            string spQuery = @"EXEC [sp_SearchInvoice] {0}, {1}, {2}";
            return tfProvinceRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Count Invoice By SONumber
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IEnumerable<TransferNotificationProvinceDto> CountInvoiceBySONumber(TransferNotificationProvinceDto para)
        {
            object[] parameter = { para.CustCode, para.CreateDate, para.Plant };
            string spQuery = @"EXEC [sp_CountInvoiceBySONumber] {0}, {1}, {2}";
            return tfProvinceRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Update Share Status
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateShareStatus(TransferNotificationProvinceDto dto)
        {
            object[] param = { dto.CustCode, dto.Plant, dto.ShareStatus, dto.SONumber, dto.CreateDate };
            string spQuery = @"EXEC [UpdateShareStatusProvince] {0}, {1}, {2}, {3}, {4}";
            return tfProvinceRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Count Invoice By SONumber
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IEnumerable<TransferNotificationProvinceDto> GetInvoinceDetailProvince(TransferNotificationProvinceDto para)
        {
            object[] parameter = { para.CustCode, para.CreateDate, para.Plant };
            string spQuery = @"EXEC [sp_GetInvoinceDetailProvince] {0}, {1}, {2}";
            return tfProvinceRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Get Email By Customer
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>
        public IEnumerable<CustomerEmailDto> GetEmailByCustomer(CustomerEmailDto cust)
        {
            object[] parameter = { cust.CustCode };
            string spQuery = @"EXEC [sp_GetEmailByCustomer] {0}";
            return custEmailRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Sum Amount By CustCode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public TransferNotificationProvinceDto SumAmountByCustCode(TransferNotificationProvinceDto dto)
        {
            object[] parameter = { dto.CustCode, dto.CreateDate, dto.Plant };
            string spQuery = @"EXEC [sp_SumAmountByCustCode] {0}, {1}, {2}";
            return tfProvinceRepo.ExecuteQuerySingle(spQuery, parameter);
        }


    }
}