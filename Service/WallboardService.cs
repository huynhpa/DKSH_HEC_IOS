using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DKSHApplication.Service
{
    public interface WallboardService
    {
        WallboardDto InitializeWallboard();
        ResultMessageDto SaveRecords();
        ResultMessageDto SaveVASCampaign();
    }
}
