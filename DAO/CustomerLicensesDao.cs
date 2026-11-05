using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class CustomerLicensesDao
    {
        private GenericRepository<CustomerLicensesDto> LicensesRepo;
        private GenericRepository<CustomerDto> CustomerRepo;
        private GenericRepository<contact_phone_obs> ContactPhoneRepo;
        private GenericRepository<contact> contactRepo;
        private OBSEntity obsEntity = null;
        private CCCToolsEntity cccEntity = null;

        /// <summary>
        /// initialize
        /// </summary>
        public CustomerLicensesDao()
        {
            LicensesRepo = new GenericRepository<CustomerLicensesDto>(new CCCToolsEntity());
            CustomerRepo = new GenericRepository<CustomerDto>(new CCCToolsEntity());
            ContactPhoneRepo = new GenericRepository<contact_phone_obs>(new OBSEntity());
            contactRepo = new GenericRepository<contact>(new OBSEntity());
            cccEntity = new CCCToolsEntity();
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Get Licenses Status
        /// </summary>
        /// <param name="licensesDto"></param>
        /// <returns></returns>
        internal CustomerLicensesDto GetLicenseStatus(CustomerLicensesDto licensesDto)
        {
            object[] parameter = { licensesDto.CustCode };
            string spQuery = @"EXEC [sp_GetLicenseStatusOBS] {0}";
            return LicensesRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal CustomerDto CustomerInfo(CustomerDto dto)
        {
            object[] parameter = { dto.CustCode };
            string spQuery = @"EXEC [sp_GetByCustCode] {0}";
            return CustomerRepo.ExecuteQuerySingle(spQuery, parameter);
        }


        /// <summary>
        /// Save Contact
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveContact(CustomerDto dto)
        {
            object[] parameter = { dto.CustCode, dto.CustName, dto.Addr, dto.District, 
                                dto.Province, dto.Region, dto.Channel, dto.CGrpCode, dto.CGrpName };
            string spQuery = @"EXEC [sp_SaveContact] {0},{1},{2},{3},{4},{5},{6},{7},{8}";
            return CustomerRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// SaveContactPhone
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveContactPhone(contact_phone_obs dto)
        {
            object[] parameter = { dto.cust_code, dto.phone_number, dto.email_addr };
            string spQuery = @"EXEC [sp_SaveContactPhone] {0},{1},{2}";
            return ContactPhoneRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// UpdateContactName
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateContactName(contact_phone_obs dto)
        {
            object[] parameter = { dto.person_name, dto.person_note, dto.cust_code, dto.phone_number };
            string spQuery = @"EXEC [sp_UpdateContactName] {0},{1},{2},{3}";
            return ContactPhoneRepo.ExecuteCommandOBS(spQuery, parameter);
        }


        internal IEnumerable<contact_phone_obs> SearchContactiMind(contact_phone_obs dto)
        {
            object[] parameter = { dto.cust_code, dto.cust_name };
            string spQuery = @"EXEC [sp_SearchContactiMind] {0},{1}";
            return ContactPhoneRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<contact_phone_obs> GetContactPhoneList(contact_phone_obs dto)
        {
            object[] parameter = { dto.cust_code };
            string spQuery = @"EXEC [sp_GetContactPhoneList] {0}";
            return ContactPhoneRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// DeletePhoneNumber
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeletePhoneNumber(contact_phone_obs dto)
        {
            object[] parameter = { dto.cust_code, dto.phone_number };
            string spQuery = @"DELETE FROM OB.dbo.contact_phone where cust_code = {0} and phone_number = {1}";
            return ContactPhoneRepo.ExecuteCommandOBS(spQuery, parameter);
        }
    }
}