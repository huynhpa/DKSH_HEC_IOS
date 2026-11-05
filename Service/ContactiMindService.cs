using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface ContactiMindService
    {
        IEnumerable<contact_phone_obs> SearchContactiMind(contact_phone_obs json);
        IEnumerable<contact_phone_obs> ContactPhoneNumber(contact_phone_obs json);
        ResultMessageDto DeletePhoneNumber(contact_phone_obs json);
    }
}
