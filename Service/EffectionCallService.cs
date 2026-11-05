using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSHApplication.DTO;

namespace DKSHApplication.Service
{
    public interface EffectionCallService
    {
        EffectionCallDto EffectionCall(OBSurveyParam para);
        ResultMessageDto SaveEffectionCall(EffectionCallDto para);
    }
}
