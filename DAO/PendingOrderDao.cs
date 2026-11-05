using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.Common;
using DKSHApplication.DB;

namespace DKSHApplication.DAO
{
    public class PendingOrderDao
    {
        public GenericRepository<PendingOrderDto> pendingOrderRepo;
        public GenericRepository<CustomerHospitalDto> customerHospitalRepo;
        public GenericRepository<ProductMedicalDto> productMedicalRepo;
        public GenericRepository<PendingOrderReceiverDto> receiverDataRepo;
        public GenericRepository<HospitalEmailDto> HospitalEmailRepo;

        private CCCToolsEntity cccToolsEntity = null;
        public PendingOrderDao()
        {
            pendingOrderRepo = new GenericRepository<PendingOrderDto>(new CCCToolsEntity());
            customerHospitalRepo = new GenericRepository<CustomerHospitalDto>(new CCCToolsEntity());
            productMedicalRepo = new GenericRepository<ProductMedicalDto>(new CCCToolsEntity());
            receiverDataRepo = new GenericRepository<PendingOrderReceiverDto>(new CCCToolsEntity());
            HospitalEmailRepo = new GenericRepository<HospitalEmailDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Pending Order List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> PendingOrderList(PendingOrderDto dto)
        {
            object[] parameter = { dto.Reject, dto.HospitalMedical,
                                   Utils.StringDateFormat(dto.CreateDateFrom), Utils.StringDateFormat(dto.CreateDateTo),
                                   dto.SoldToPT, dto.SendStatus, dto.EmailYesNo };
            string spQuery = @"EXEC [sp_GetPendingOrder] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return pendingOrderRepo.ExecuteQuery(spQuery, parameter);
        }


        /// <summary>
        /// Send Mail By Reject
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> SendMailByReject(PendingOrderDto dto)
        {
            object[] parameter = { dto.Reject, dto.CreateDate };
            string spQuery = @"EXEC [sp_SendMailByReject] {0}, {1}";
            return pendingOrderRepo.ExecuteQuery(spQuery, parameter);
        }


        /// <summary>
        /// Insert Pending Order
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertPendingOrder(PendingOrderDto dto)
        {
            object[] parameter = {  dto.PendingOrderId, dto.Documents, dto.SoldToPT, dto.CustName, dto.Description,
                                    dto.Material, dto.OrderQty, dto.SU, dto.Reject, dto.DocDate, dto.DeliveryDate, dto.PONumber,
                                    dto.Plant, dto.HospitalMedical, dto.CreateBy, dto.Created, dto.NetPrice };
            string spQuery = @"EXEC [sp_InsertPendingOrder] {0}, {1}, {2}, {3}, {4}, {5}, {6}, 
                            {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}";
            return pendingOrderRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Customer Hospital
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertCustomerHospital(CustomerHospitalDto dto)
        {
            object[] parameter = { dto.CustCode, dto.CustName, dto.Addr, dto.District, dto.ZTerm,
                                   dto.Province, dto.Region, dto.Channel, dto.TaxCode, dto.Tel,
                                   dto.CGrpCode, dto.CGrpName };
            string spQuery = @"EXEC [sp_InsertCustomerHospital] 
                            {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}";
            return customerHospitalRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Product Medical
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertProductMedical(ProductMedicalDto dto)
        {
            object[] parameter = { dto.Supplier, dto.PendingNotify, dto.ProductID, dto.ProductName, dto.UpdateBy };
            string spQuery = @"EXEC [sp_InsertProductMedical] {0}, {1}, {2}, {3}, {4}";
            return productMedicalRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Delete Product Medical
        /// </summary>
        /// <returns></returns>
        public int DeleteProductMedical()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_DeleteProductMedical]";
            return productMedicalRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Delete Customer Hospital
        /// </summary>
        /// <returns></returns>
        public int DeleteCustomerHospital()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_DeleteCustomerHospital]";
            return productMedicalRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Get Customer Hospital
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerHospitalDto> GetCustomerHospital(PendingOrderDto dto)
        {
            object[] parameter = { dto.SoldToPT };
            string spQuery = @"EXEC [sp_GetCustomerHospital] {0}";
            return customerHospitalRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Get Product Medical
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductMedicalDto> GetProductMedical(PendingOrderDto dto)
        {
            object[] parameter = { dto.Material };
            string spQuery = @"EXEC [sp_GetProductMedical] {0}";
            return productMedicalRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Insert Receiver Data
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertReceiverData(PendingOrderReceiverDto dto)
        {
            object[] parameter = { dto.CustCode, dto.SalesRep,
                                    dto.PhoneNo, dto.Email, dto.UpdateBy };
            string spQuery = @"EXEC [sp_InsertReceiverData] {0}, {1}, {2}, {3}, {4}";
            return receiverDataRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Check Hospital In Material
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PendingOrderDto CheckHospitalInMaterial(PendingOrderDto dto)
        {
            object[] parameter = { dto.Material };
            string spQuery = @"EXEC [sp_CheckHospitalInMaterial] {0}";
            return pendingOrderRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Delete Receiver Data
        /// </summary>
        /// <param name="pendingOrderReceiverDto"></param>
        /// <returns></returns>
        public int DeleteReceiverData()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_DeleteReceiverData]";
            return receiverDataRepo.ExecuteCommand(spQuery, parameter);
        }


        /// <summary>
        /// Get Pending Order By Reject
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetPendingOrderByReject(PendingOrderDto dto)
        {
            object[] param = { dto.HospitalMedical, dto.CreateDateFrom, dto.CreateDateTo, dto.Created };
            string spQuery = @"EXEC [sp_GetPendingOrderByReject] {0}, {1}, {2}, {3}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Detail Document
        /// </summary>
        /// <param name="poDto"></param>
        /// <returns></returns>
        public PendingOrderDto GetDetailDocument(PendingOrderDto poDto)
        {
            object[] param = { poDto.CreateDate, poDto.SoldToPT, poDto.Reject, poDto.HospitalMedical };
            string spQuery = @"EXEC [sp_GetDetailDocument] {0}, {1}, {2}, {3}";
            return pendingOrderRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Get Detail By Hospital
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetDetailByHospital(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate, pendingOrderDto.SoldToPT, pendingOrderDto.Reject };
            string spQuery = @"EXEC [sp_GetDetailByHospital] {0}, {1}, {2}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Detail By Medical
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetDetailByMedical(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate, pendingOrderDto.SoldToPT, pendingOrderDto.Reject };
            string spQuery = @"EXEC [sp_GetDetailByMedical] {0}, {1}, {2}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Delete Email
        /// </summary>
        /// <returns></returns>
        public int DeleteEmail()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_DeleteHospitalEmail]";
            return HospitalEmailRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update Send Status
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateSendStatus(PendingOrderDto dto)
        {
            object[] param = { dto.CreateDate, dto.SoldToPT, dto.SendStatus, dto.Reject, dto.HospitalMedical };
            string spQuery = @"EXEC [sp_UpdateSendStatus] {0}, {1}, {2}, {3}, {4}";
            return pendingOrderRepo.ExecuteCommand(spQuery, param);
        }


        /// <summary>
        /// SCRIPT 01
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetHospitalPIADMIN(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetHospitalPIADMIN] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 02
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetHospitalNONPIADMIN(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetHospitalNONPIADMIN] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 03
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetHospitalPIADMIND4(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetHospitalPIADMIND4] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 04
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetHospitalNONPIADMIND4(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetHospitalNONPIADMIND4] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 05
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetHospitalPIADMINF5(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetHospitalPIADMINF5] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 06
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetHospitalNONPIADMINF5(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetHospitalNONPIADMINF5] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 07
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetMedicalPIADMIN(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetMedicalPIADMIN] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 08
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetMedicalNONPIADMIN(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetMedicalNONPIADMIN] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 09
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetMedicalPIADMINF5(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetMedicalPIADMINF5] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// SCRIPT 10
        /// </summary>
        /// <param name="pendingOrderDto"></param>
        /// <returns></returns>
        public IEnumerable<PendingOrderDto> GetMedicalNONPIADMINF5(PendingOrderDto pendingOrderDto)
        {
            object[] param = { pendingOrderDto.CreateDate };
            string spQuery = @"EXEC [sp_GetMedicalNONPIADMINF5] {0}";
            return pendingOrderRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Lastest PendingOrder
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PendingOrderDto GetLastestPendingOrder(PendingOrderDto dto)
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_GetLastestPendingOrder]";
            return pendingOrderRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int UpdateProductNonNotifyPending()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_UpdateProductNonNotifyPending]";
            return pendingOrderRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update Pending Order
        /// </summary>
        /// <param name="pod"></param>
        /// <returns></returns>
        public int UpdatePendingOrder(PendingOrderDto pod)
        {
            object[] parameter = {  pod.PendingOrderId, pod.Documents, pod.SoldToPT, pod.CustName, pod.Description,
                                    pod.Material, pod.OrderQty, pod.SU, pod.Reject, pod.DocDate, pod.DeliveryDate, pod.PONumber,
                                    pod.Plant, pod.HospitalMedical, pod.CreateBy, pod.Created, pod.NetPrice, Utils.StringDateFormat(pod.CreateDate) };
            string spQuery = @"EXEC [sp_PendingOrderBackup] {0}, {1}, {2}, {3}, {4}, {5}, {6}, 
                                                            {7}, {8}, {9}, {10}, {11}, {12}, 
                                                            {13}, {14}, {15}, {16}, {17}";
            return pendingOrderRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Backup PendingOrder
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal int BackupPendingOrder(PendingOrderDto obj)
        {
            object[] param = { obj.CreateDate };
            string spQuery = @"EXEC [sp_BackupPendingOrder] {0}";
            return pendingOrderRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteDataPending()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_DeleteDataPending]";
            return pendingOrderRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Customer Hospital
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<CustomerHospitalDto> CustomerHospital()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_CustomerHospital]";
            return customerHospitalRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Product Medical
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ProductMedicalDto> ProductMedical()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_ProductMedical]";
            return productMedicalRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Receiver Data
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<PendingOrderReceiverDto> ReceiverData()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_ReceiverData]";
            return receiverDataRepo.ExecuteQuery(spQuery, parameter);
        }

    }
}