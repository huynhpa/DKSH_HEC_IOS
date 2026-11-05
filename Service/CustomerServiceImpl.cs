using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class CustomerServiceImpl : CustomerService
    {
        CustomerDao customerDao = new CustomerDao();

        /// <summary>
        /// SEARCH REQUEST
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<CustomerDto> SearchCustomer(CustomerDto param)
        {
            IEnumerable<CustomerDto> listCustomer = customerDao.SearchCustomer(param);
            return listCustomer;
            throw new NotImplementedException();
        }

 
    }
}