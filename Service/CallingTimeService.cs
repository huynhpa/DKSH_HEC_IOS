using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface CallingTimeService
    {
        IEnumerable<CallingTimeDto> SearchCallingTime(CallingTimeDto dto);
        ResultMessageDto SetCallingTime(CallingTimeDto dto);
    }
}
