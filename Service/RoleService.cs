using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface RoleService
    {
        IEnumerable<RoleDto> GetRoleList();
        IEnumerable<RoleDto> SearchRole(RoleDto dto);
        ResultMessageDto InsertRole(RoleDto dto);
        ResultMessageDto UpdateRole(RoleDto dto);
        ResultMessageDto DelRole(RoleDto dto);
        RoleDto GetRoleDetail(RoleDto dto);
        IEnumerable<RoleDto> GetUsersInRoles(RoleDto dto);
    }
}
