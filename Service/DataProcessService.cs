using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DKSHApplication.Service
{
    public interface DataProcessService
    {
        ResultMessageDto UploadDataProcess(HttpPostedFileBase aFile);

        ResultMessageDto UploadCustomerClassSFA(HttpPostedFileBase aFile);
        ResultMessageDto SaveClient();
        ResultMessageDto SaveDataSalesInhouse(HttpPostedFileBase aFile);
    }
}
