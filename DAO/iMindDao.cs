using DKSHApplication.Common;
using DKSHApplication.DAOBusiness;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class iMindDao
    {
        private GenericRepository<contact> contactRepo;
        private GenericRepository<ContactsDto> contDtoRepo;
        private GenericRepository<contact_phones> contactPhoneRepo;
        private GenericRepository<contact_persons> contactPersonRepo;
        private GenericRepository<iMindContactInfoDto> iMindRepo;
        private GenericRepository<iMindCustomerInfoDto> iMindCustRepo;
        private GenericRepository<ContactCustomerIMindDto> contactCustomerRepo;
        private GenericRepository<ContactInformationDto> contactInfoRepo;
        private iMindEntity iMindEntity = null;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// initialize
        /// </summary>
        public iMindDao()
        {
            contactRepo = new GenericRepository<contact>(new OBSEntity());
            contactPhoneRepo = new GenericRepository<contact_phones>(new OBSEntity());
            contactPersonRepo = new GenericRepository<contact_persons>(new OBSEntity());
            iMindRepo = new GenericRepository<iMindContactInfoDto>(new OBSEntity());
            contDtoRepo = new GenericRepository<ContactsDto>(new OBSEntity());
            iMindCustRepo = new GenericRepository<iMindCustomerInfoDto>(new OBSEntity());
            contactCustomerRepo = new GenericRepository<ContactCustomerIMindDto>(new OBSEntity());
            contactInfoRepo = new GenericRepository<ContactInformationDto>(new OBSEntity());
            iMindEntity = new iMindEntity();
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Customer iMind Info
        /// </summary>
        /// <param name="ContactsDto">code</param>
        /// <returns>IEnumerable(iMindContactInfoDto)</returns>
        internal IEnumerable<iMindCustomerInfoDto> CustomeriMindInfo(ContactsDto dto)
        {
            object[] param = { dto.code };
            string procQuery = @"exec sp_CustomeriMindInfo {0}";
            return iMindCustRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// Contact phone number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<iMindContactInfoDto> ContactPhoneiMind(ContactsDto dto)
        {
            object[] param = { dto.code };
            string procQuery = @"exec sp_ContactPhoneiMind {0}";
            return iMindRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// ContactiMind
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<iMindContactInfoDto> ContactiMind()
        {
            object[] param = { };
            string procQuery = @"exec sp_ContactiMind";
            return iMindRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// select top 1 contact & phone iMind
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<iMindContactInfoDto> Top1ContactPhoneiMind(ContactsDto dto)
        {
            object[] param = { dto.code };
            string procQuery = @"exec sp_Top1ContactPhoneiMind {0}";
            return iMindRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// Table Contact
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ContactsDto> Contacts(ContactsDto dto)
        {
            object[] param = { dto.code };
            string procQuery = @"exec sp_Contacts {0}";
            return contDtoRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// Contact Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<iMindContactInfoDto> ContactCustomer(ContactsDto dto)
        {
            object[] param = { dto.code };
            string procQuery = @"exec sp_ContactCustomer {0}";
            return iMindRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// TABLE contacts & M_Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ContactCustomerIMindDto> Top1ContactCustomeriMind(OBSurveyParam dto)
        {
            object[] param = { dto.CustCode };
            string procQuery = @"exec sp_Top1ContactCustomeriMind {0}";
            return contactCustomerRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ContactInformationDto> iMindPhonenumber()
        {
            object[] param = { };
            string procQuery = @"exec sp_iMindPhonenumber";
            return contactInfoRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal ContactInformationDto GetContactNameByCustCodePhone(contact_phone_obs dto)
        {
            object[] param = { dto.cust_code, dto.phone_number };
            string procQuery = @"exec sp_GetContactNameByCustCodePhone {0},{1}";
            return contactInfoRepo.ExecuteQuerySingleOBS(procQuery, param);
        }
    }
}