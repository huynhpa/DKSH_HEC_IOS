using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface CampaignService
    {
        CampaignDto GetCampaign(CampaignDto dto);
        List<ServiceTypeDto> Search(CampaignDto dto);
        ResultMessageDto DeleteCampaign(ServiceTypeDto dto);
        ResultMessageDto InsertCampaign(ServiceTypeDto dto);
        ResultMessageDto UpdateCampaign(ServiceTypeDto dto);
        ServiceTypeDto CampaignDetail(ServiceTypeDto param);
        IWorkbook ViewAgentInCampaign(SearchCallDto json);
    }
}
