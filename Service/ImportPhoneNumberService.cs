using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DKSHApplication.Service
{
    public interface ImportPhoneNumberService
    {
        ResultMessageDto UploadPhoneNumber(HttpPostedFileBase aFile, PhoneNumberDto dto);
        IEnumerable<PhoneNumberDto> SearchPhoneNumber(PhoneNumberDto dto);
        IEnumerable<SummaryPhoneDto> SummaryPhoneNumber(PhoneNumberDto dto);
    }
}
