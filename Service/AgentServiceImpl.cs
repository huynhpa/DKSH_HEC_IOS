using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using MP.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;

namespace DKSHApplication.Service
{
    public class AgentServiceImpl : AgentService
    {
        AgentDao agentDao = new AgentDao();
        RoleDao roleDao = new RoleDao();
        MemberShipDao memberShipDao = new MemberShipDao();
        HrEmployeeDao hrEmployeeDao = new HrEmployeeDao();
        CampaignDao campaignDao = new CampaignDao();

        /// <summary>
        /// change password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto ChangePassword(MemberShipDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = memberShipDao.ChangePassword(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.CHANGE_PASSWORD_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_PASSWORD_CHANGED;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }            
            return result;
        }

        /// <summary>
        /// CheckUserInRole
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto CheckUserInRole(UsersDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                RoleDto role = new RoleDto();
                role.UserId = dto.UserId;
                result.RoleByUser = roleDao.GetRoleByUser(role);
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Delete Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteAgent(hrEmployeeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                MemberShipDto member = new MemberShipDto();
                result.number = hrEmployeeDao.DeleteAgent(dto);
                member.UserId = dto.ID;
                int delmem = memberShipDao.DeleteMemberShip(member);
                // GET ROLE                
                RoleDto delRole = new RoleDto();
                delRole.UserId = dto.ID;
                int del = roleDao.DeleteUserInRole(delRole);
                if (result.number > 0)
                {
                    result.code = Constant.DELETE;
                    result.message = string.Format(Constant.MESSAGE_DELELE, dto.WindowsUser);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Get Agent Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto GetAgentDetail(UsersDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            result.UserDetail = agentDao.GetAgentDetail(dto);
            hrEmployeeDto hrEmp = new hrEmployeeDto();
            hrEmp.ID = dto.UserId;
            result.hrEmployee = hrEmployeeDao.GetEmployeeDetail(hrEmp);
            RoleDto role = new RoleDto();
            role.UserId = dto.UserId;
            result.RoleByUser = roleDao.GetRoleByUser(role);
            return result;
        }

        /// <summary>
        /// GetMemberList
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<UsersDto> GetMemberList(UsersDto dto)
        {
            IEnumerable<UsersDto> result = memberShipDao.GetMemberList(dto);
            return result;
        }


        /// <summary>
        /// get user profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public MemberShipDto GetUserProfile(MemberShipDto userProfile)
        {
            hrEmployeeDto hrEmp = new hrEmployeeDto();
            hrEmp.ID = (Guid)userProfile.UserId;
            hrEmp = hrEmployeeDao.GetEmployeeDetail(hrEmp);
            userProfile.FullName = hrEmp.FullName;
            userProfile.JobTitle = hrEmp.JobTitle;
            userProfile.Email = hrEmp.Email;
            userProfile.UserLogin = hrEmp.UserLogin;
            userProfile.ImagePath = hrEmp.ImagePath;
            return userProfile;
        }

        /// <summary>
        /// InsertAgent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertAgent(hrEmployeeDto hrEmpDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {                
                UsersDto dto = new UsersDto();                
                Guid userId = Guid.NewGuid();
                dto.UserId = userId;
                dto.UserName = hrEmpDto.WindowsUser;
                IEnumerable<UsersDto> existed = agentDao.CheckUserExisted(dto);
                if (existed.Count() == 0)
                {
                    result.number = agentDao.InsertAgent(dto);
                    hrEmpDto.ID = userId;
                    result.number += hrEmployeeDao.InsertHrEmployee(hrEmpDto);
                    MemberShipDto membership = new MemberShipDto();
                    membership.UserId = userId;
                    membership.Password = hrEmpDto.Password;
                    membership.PasswordSalt = hrEmpDto.PasswordSalt;
                    membership.UserName = hrEmpDto.WindowsUser;
                    result.number += memberShipDao.InsertMemberShip(membership);
                    // GET ROLE
                    List<string> roles = new List<string>();
                    if (!string.IsNullOrEmpty(hrEmpDto.RoleId))
                    {
                        roles = hrEmpDto.RoleId.Split(',').ToList();
                    }                    
                    RoleDto delRole = new RoleDto();
                    delRole.UserId = userId;
                    int del = roleDao.DeleteUserInRole(delRole);
                    int idx = 0;
                    RoleDto roleDto = null;
                    foreach (string item in roles)
                    {
                        roleDto = new RoleDto();
                        roleDto.UserId = userId;
                        roleDto.RoleId = FGuid.ToGuid(item);
                        idx += roleDao.SetUserInRoles(roleDto);
                    }
                    if (result.number > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.NO_ROW_SAVE;
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = string.Format(Constant.USER_NAME_EXIST, dto.UserName);
                }
                
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto ResetPassword(hrEmployeeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = memberShipDao.ResetPassword(dto);
                string[] format = new string[2];
                format[0] = dto.WindowsUser;
                format[1] = Constant.RESET_PASSWORD_AGENT;
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.RESET_PASSWORD_AGENT_MESSAGE, format);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_PASSWORD_CHANGED;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Update Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateAgent(hrEmployeeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {                
                MemberShipDto member = new MemberShipDto();
                result.number = hrEmployeeDao.UpdateAgent(dto);                
                // GET ROLE
                List<string> roles = new List<string>();
                if (!string.IsNullOrEmpty(dto.RoleId))
                {
                    roles = dto.RoleId.Split(',').ToList();
                }                
                RoleDto delRole = new RoleDto();
                delRole.UserId = dto.ID;
                int del = roleDao.DeleteUserInRole(delRole);
                int idx = 0;
                RoleDto roleDto = null;
                foreach (string item in roles)
                {
                    roleDto = new RoleDto();
                    roleDto.UserId = dto.ID;
                    roleDto.RoleId = FGuid.ToGuid(item);
                    idx += roleDao.SetUserInRoles(roleDto);
                }
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateImagePath(MemberShipDto obj)
        {
            ResultMessageDto result = new ResultMessageDto();            
            result.number = memberShipDao.UpdateImagePath(obj);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateProfile(MemberShipDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {                
                result.number = memberShipDao.UpdateProfile(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<AgentGroupDto> AgentGroup(AgentGroupDto dto)
        {
            IEnumerable<AgentGroupDto> result = hrEmployeeDao.AgentGroup(dto);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadAssignAgentProject(HttpPostedFileBase aFile, MemberShipDto memberShipDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    IWorkbook workbook;
                    mem.SetLength(aFile.ContentLength);
                    aFile.InputStream.Read(mem.GetBuffer(), 0, aFile.ContentLength);
                    string extension = Path.GetExtension(aFile.FileName).Replace(".", "");
                    if (Constant.EXCEL_XLSX.Equals(extension))
                    {
                        workbook = new XSSFWorkbook(mem);
                    }
                    else if (Constant.EXCEL_XLS.Equals(extension))
                    {
                        workbook = new HSSFWorkbook(mem);
                    }
                    else
                    {
                        throw new Exception("This format is not supported");
                    }

                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = 0;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        AgentDto agentDto = new AgentDto()
                        {
                            Extension = Utils.GetFormattedCellValue(row.GetCell(0)),
                            ProjectCode = Utils.GetFormattedCellValue(row.GetCell(1)),
                            UpdatedBy = memberShipDto.UserId
                        };
                        if (agentDao.AssignAgentProject(agentDto) >= 1)
                        {
                            rowCount += 1;
                        }
                    }
                    if (rowCount > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.number = rowCount;
                        result.message = string.Format(Constant.PROCESS_MESSAGE, rowCount);
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.MESSAGE_ERROR;
                    }
                    mem.Close();
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook AssignAgentProjectTemplate(AgentDto dto)
        {
            IWorkbook workbook;
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "AssignAgentProject.xlsx";
            List<ProjectDto> ProjectList = new List<ProjectDto>();
            ProjectList = campaignDao.ProjectActive().ToList();
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(1);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                var font = (XSSFFont)workbook.CreateFont();
                var style = (XSSFCellStyle)workbook.CreateCellStyle();
                font.FontName = "Segoe UI";
                font.FontHeightInPoints = 9;
                style.SetFont(font);
                int rowIdx = 1;
                foreach (ProjectDto item in ProjectList)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.ProjectCode); // ProjectCode
                    sheet.GetRow(rowIdx).GetCell(0).CellStyle = style;
                    row.CreateCell(1).SetCellValue(item.ProjectName); // Project Name
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }
    }
}