using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DKSHApplication.Service
{
    public interface CustomerService
    {
        /// <summary>
        /// Search Customer
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<CustomerDto> SearchCustomer(CustomerDto param);

    }
}
