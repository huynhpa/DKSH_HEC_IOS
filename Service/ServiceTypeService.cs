using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSHApplication.DTO;

namespace DKSHApplication.Service
{
    public interface ServiceTypeService
    {
        IEnumerable<ServiceTypeDto> GetActiveServiceType(ServiceTypeDto dto);
        ServiceTypeDto GetServiceTypeById(ServiceTypeDto dto);
    }
}
