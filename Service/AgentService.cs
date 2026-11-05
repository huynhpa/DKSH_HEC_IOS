using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface AgentService
    {
        IEnumerable<UsersDto> GetMemberList(UsersDto dto);
        ResultMessageDto InsertAgent(hrEmployeeDto hrEmployeeDto);
        ResultMessageDto UpdateAgent(hrEmployeeDto dto);
        ResultMessageDto GetAgentDetail(UsersDto dto);
        ResultMessageDto DeleteAgent(hrEmployeeDto dto);
        ResultMessageDto UpdateProfile(MemberShipDto dto);
        ResultMessageDto ChangePassword(MemberShipDto dto);
        ResultMessageDto ResetPassword(hrEmployeeDto dto);
        ResultMessageDto CheckUserInRole(UsersDto dto);
        ResultMessageDto UpdateImagePath(MemberShipDto obj);
        MemberShipDto GetUserProfile(MemberShipDto userProfile);
        IEnumerable<AgentGroupDto> AgentGroup(AgentGroupDto dto);
        ResultMessageDto UploadAssignAgentProject(HttpPostedFileBase aFile, MemberShipDto memberShipDto);
        IWorkbook AssignAgentProjectTemplate(AgentDto dto);
    }
}
