using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class ContactiMindServiceImpl : ContactiMindService
    {
        CustomerLicensesDao dao = new CustomerLicensesDao();

        /// <summary>
        /// SearchContactiMind
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<contact_phone_obs> SearchContactiMind(contact_phone_obs json)
        {
            IEnumerable<contact_phone_obs> result = dao.SearchContactiMind(json);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<contact_phone_obs> ContactPhoneNumber(contact_phone_obs json)
        {
            IEnumerable<contact_phone_obs> result = dao.GetContactPhoneList(json).DistinctBy(x => x.phone_number);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto DeletePhoneNumber(contact_phone_obs json)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = dao.DeletePhoneNumber(json);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.MESSAGE_DELELE, result.number);
                }
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }
    }
}