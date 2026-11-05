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
    public class CustomerDao
    {
        private GenericRepository<CustomerDto> customerRepo;

        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// 
        /// </summary>
        public CustomerDao()
        {
            customerRepo = new GenericRepository<CustomerDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// SEARCH CUSTOMER
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerDto> SearchCustomer(CustomerDto dto)
        {
            object[] parameter = {  dto.CustCode, dto.CustName, dto.District, dto.ZTerm, dto.Province, 
                                    dto.Region, dto.Channel, dto.TaxCode, dto.CGrpCode, dto.Addr, dto.VNZ0Code, dto.BravoCode };
            string spQuery = @"EXEC [SearchCustomer] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}";
            return customerRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <returns></returns>
        public int DeleteCustomer()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [DeleteCustomer] ";
            return customerRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int InsertCustomer(CustomerDto customer)
        {
            object[] parameter = {  customer.CustCode, customer.CustName, customer.Addr, customer.District, customer.ZTerm, customer.Province,
                                    customer.Region, customer.Channel, customer.TaxCode, customer.Tel, customer.CGrpCode, customer.CGrpName,
                                    customer.SortItem };
            string spQuery = @"EXEC [InsertCustomer] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}";
            return customerRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert CustCode By CGrpCode
        /// </summary>
        /// <returns></returns>
        public int InsertCustCode()
        {
            object[] param = { };
            string spQuery = @"EXEC [InsertCustCode]";
            return customerRepo.ExecuteCommand(spQuery, param);
        }



        /// <summary>
        /// delete all from nha thuoc benh vien cong
        /// </summary>
        /// <returns></returns>
        public int DeleteCustomerHNine()
        {
            object[] param = { };
            string spQuery = @"EXEC [DeleteCustomerHNine]";
            return customerRepo.ExecuteCommand(spQuery, param);
        }


        /// <summary>
        /// Save channel history
        /// </summary>
        /// <returns></returns>
        public int SaveChannelHistory()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_SaveChannelHistory]";
            return customerRepo.ExecuteCommand(spQuery, param);
        }




        /// <summary>
        /// Get Channel
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerDto> GetChannelCustomer()
        {
            object[] parameter = {  };
            string spQuery = @"EXEC [sp_GetChannelCustomer]";
            return customerRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        public CustomerDto GetByCustCode(CustomerDto customerDto)
        {
            object[] parameter = { customerDto.CustCode };
            string spQuery = @"EXEC [sp_GetByCustCode] {0}";
            return customerRepo.ExecuteQuerySingle(spQuery, parameter);
        }

    }
}