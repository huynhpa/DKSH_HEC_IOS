using DKSHApplication.DAO;
using DKSHApplication.DTO;
using DKSHApplication.DB;
using DKSHApplication.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DKSHApplication.Common
{
    public class ContactiMind : IJob
    {        
        public void Execute(IJobExecutionContext context)
        {
            iMindDao iMind = new iMindDao();
            CustomerLicensesDao customerInfoDao = new CustomerLicensesDao();
            ResultMessageDto result = new ResultMessageDto();            
            try
            {
                IEnumerable<iMindContactInfoDto> contact = iMind.ContactiMind();
                foreach(iMindContactInfoDto item in contact)
                {
                    CustomerDto cust = new CustomerDto()
                    {
                        CustCode = item.code
                    };
                    cust = customerInfoDao.CustomerInfo(cust);
                    if (cust != null)
                    {
                        customerInfoDao.SaveContact(cust);
                    }
                }

                IEnumerable<ContactInformationDto> imindPhonenumber = iMind.iMindPhonenumber();
                foreach(ContactInformationDto obj in imindPhonenumber)
                {
                    contact_phone_obs cp = new contact_phone_obs()
                    {
                        cust_code = obj.code,
                        phone_number = obj.phone_number,
                        email_addr = obj.email_addr
                    };
                    customerInfoDao.SaveContactPhone(cp);
                    ContactInformationDto ci = iMind.GetContactNameByCustCodePhone(cp);
                    if (ci != null)
                    {
                        contact_phone_obs cpo = new contact_phone_obs()
                        {
                            cust_code = ci.cust_code,
                            person_name = ci.person_name,
                            person_note = ci.person_note,
                            phone_number = ci.phone_number
                        };
                        customerInfoDao.UpdateContactName(cpo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
        }

    }
}