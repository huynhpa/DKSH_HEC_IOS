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
    public interface AssignmentService
    {
        IEnumerable<AssignmentDto> ViewAssignment(AssignmentDto dto);
        ResultMessageDto RemoveAssignment(AssignmentDto dto);
        ResultMessageDto SaveStatus(AssignDto param);
        List<ProjectDto> GetProjectByWindowUser(ProjectDto project);
    }
}
