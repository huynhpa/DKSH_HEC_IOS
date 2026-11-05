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
    public interface ProjectService
    {
        IEnumerable<ProjectDto> SearchProject(ProjectDto dto);
        ProjectDto GetProjectDetail(ProjectDto dto);
        ResultMessageDto UpdateProject(ProjectDto dto);
        ResultMessageDto InsertProject(ProjectDto dto);
        ResultMessageDto DeleteProject(ProjectDto dto);
    }
}
