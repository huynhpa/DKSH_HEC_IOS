using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface CampaignDetailService
    {
        CampaignDetailDto GetCampaignDetailById(CampaignDetailDto json);
    }
}
