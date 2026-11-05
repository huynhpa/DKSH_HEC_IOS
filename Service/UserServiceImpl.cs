using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DKSHApplication.Service
{
    public class UserServiceImpl : UserService
    {
        UserDao userDao = new UserDao();
        /// <summary>
        /// Search User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<UserDto> SearchUser(UserDto dto)
        {            
            IEnumerable<UserDto> listUser = userDao.SearchUser(dto);
            return listUser;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertUser(UserDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            object[] param = {
                                dto.UserId, dto.UserName, dto.RoleId, dto.Fullname, dto.Email,
                                Utils.GetPasswordHashAndSalt(dto.Pwd), dto.Team, dto.Department, false, dto.IsActive,
                                dto.JobTitle, dto.JoinDate, dto.IsManager, dto.CreateBy, dto.UpdateBy, 
                                dto.ScreenCode, dto.IsUsing
                             };
            if (!CheckUserExist(dto))
            {
                rs.number = userDao.InsertUser(param);
                if (rs.number > 0)
                {
                    rs.code = Constant.INSERT;
                    rs.message = Constant.MESSAGE_SUCCESS;
                    dto = new UserDto();
                    rs.UserList = userDao.SearchUser(dto);
                }
                else
                {
                    rs.code = Constant.ERROR;
                    rs.message = Constant.MESSAGE_ERROR;
                }
            }
            else
            {
                object[] str = { dto.Email };
                rs.code = Constant.EXISTED;
                rs.message = Constant.USER_EXIST;
            }
            return rs;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update user 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateUser(UserDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            object[] param = { 
                                dto.Id, dto.UserId, dto.UserName, dto.RoleId, dto.Fullname, dto.Email,
                                dto.Team, dto.Department, dto.IsDelete, dto.IsActive, dto.JobTitle,
                                dto.JoinDate, dto.IsManager, dto.UpdateBy, dto.ScreenCode, dto.IsUsing
                             };
            object[] pr = { dto.Id, dto.UserId, dto.Email };
            UserDto userDto = userDao.CheckUserUpdate(pr);
            object[] msg = new object[2];
            if (userDto != null && userDto.Id != dto.Id)
            {
                result.number = 0;
                result.code = Constant.EXISTED;
                msg[0] = dto.Email;
                msg[1] = userDto.UserName;
                result.message = string.Format(Constant.REGISTERED, msg);
            }
            else
            {
                result.number = userDao.UpdateUser(param);
                if (result.number == 1)
                {
                    result.code = Constant.UPDATE;
                    result.message = Constant.MESSAGE_SUCCESS;
                    dto = new UserDto();
                    result.UserList = userDao.SearchUser(dto);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.MESSAGE_ERROR;
                }

            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDto GetUserDetail(UserDto user)
        {
            UserDto userDto = new UserDto();
            if (user != null && user.Id > 0)
            {
                object[] param = { user.Id };
                userDto = userDao.GetUserDetail(param);
            }
            return userDto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteUser(int Id)
        {
            ResultMessageDto result = new ResultMessageDto();
            int cnt = userDao.UnusedUser(Id);
            if (cnt > 0)
            {
                result.code = Constant.DELETE;
                result.message = Constant.MESSAGE_DELELE;
                UserDto dto = new UserDto();
                result.UserList = userDao.SearchUser(dto);
            }            
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetUserByTeamCDI
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserDto> GetUserByTeamCDI()
        {
            IEnumerable<UserDto> UserCDIList = userDao.GetUserByTeamCDI();
            return UserCDIList;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto Registration(UserDto jsonParam)
        {
            ResultMessageDto resultDto = new ResultMessageDto();
            jsonParam.Department = Constant.DEPARTMENT;
            jsonParam.CreateBy = jsonParam.UserId;
            jsonParam.UpdateBy = jsonParam.UserId;
            if (jsonParam.Team.Equals(Constant.MANAGER) && jsonParam.Team.Equals(Constant.SUPPORT))
            {
                jsonParam.IsManager = true;
                jsonParam.IsActive = true;
                jsonParam.RoleId = Constant.ROLE_ADMIN;
            }
            else
            {
                jsonParam.IsActive = false;
                jsonParam.RoleId = Constant.ROLE_USER;
            }
            if (!CheckUserExist(jsonParam))
            {
                resultDto.number = userDao.Registration(jsonParam);
                // GET MANAGER BY TEAM
                IEnumerable<UserDto> userDto = userDao.GetManagerByTeam(jsonParam.Team);
                if (resultDto.number > 0)
                {
                    resultDto.code = Constant.INSERT;
                    resultDto.message = Constant.MESSAGE_SUCCESS;
                    object[] format = new object[5];
                    format[0] = jsonParam.UserId;
                    format[1] = jsonParam.UserName;
                    format[2] = jsonParam.Fullname;
                    format[3] = jsonParam.Email;
                    format[4] = jsonParam.Team;
                    foreach(UserDto u in userDto)
                    {
                        Utils.SendMail(Constant.TITLE_USER_REGISTER, 
                            string.Format(Constant.CONTENT_USER_REGISTER, format), u.Email, true, string.Empty);
                    }
                }
                else
                {
                    resultDto.code = Constant.ERROR;
                    resultDto.message = Constant.MESSAGE_ERROR;
                }
            }
            else
            {
                object[] str = { jsonParam.Email };
                resultDto.code = Constant.EXISTED;
                resultDto.message = String.Format(Constant.USER_EXIST, str);
            }
            
            return resultDto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// CheckUserExist
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        private bool CheckUserExist(UserDto pr)
        {
            bool exist = false;
            int cnt = userDao.CheckUserExist(pr);
            if (cnt > 0)
            {
                exist = true;
            }
            return exist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public ForgotPasswordDto ForgotPassword(ForgotPasswordDto dto)
        {
            string RandomPassword = Utils.CreateRandomPassword(8);            
            // SEND NEW PASSWORD FOR EMAIL
            StringBuilder contentMail = new StringBuilder();
            contentMail.Append("<p>We heard you need a password reset</p>");
            contentMail.Append("<p>Your password reset is : ");
            contentMail.Append("<b>" + RandomPassword + "</b></p>");
            string content = contentMail.ToString();
            bool send = Utils.SendMail("Your password reset", content, dto.Email, true, string.Empty);
            // UPDATE PASSWORD TEMP
            dto.ResetPassword = RandomPassword;
            int count = userDao.UpdatePasswordByEmail(dto);
            if (send)
            {
                dto.code = Constant.SUCCESS;
                dto.message = Constant.SEND_PASSWORD;
            }
            return dto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reset password after send mail
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto ResetPassword(ForgotPasswordDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (!string.IsNullOrEmpty(dto.Email))
            {
                result = userDao.ResetPassword(dto);
                if (result.number > 0)
                {
                    result.code = Constant.RESET_PASSWORD;
                    result.message = Constant.RESET_PASSWORD_MESSAGE;
                }
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.NOT_EXIST;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateProfile(UserDto jsonObj)
        {
            ResultMessageDto result = new ResultMessageDto();
            
            object[] pr = { jsonObj.Id, jsonObj.UserId, jsonObj.Email };
            UserDto userDto = userDao.CheckUserUpdate(pr);
            object[] msg = new object[2];
            if (userDto != null && userDto.Id != jsonObj.Id)
            {
                result.number = 0;
                result.code = Constant.EXISTED;
                msg[0] = jsonObj.Email;
                msg[1] = userDto.UserId;
                result.message = string.Format(Constant.REGISTERED, msg);
            }
            else
            {
                result.number = userDao.UpdateProfile(jsonObj);
                if (result.number == 1)
                {
                    result.code = Constant.UPDATE;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.MESSAGE_ERROR;
                }
            }            
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Image Path
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateImagePath(UserDto jsonObj)
        {
            ResultMessageDto result = new ResultMessageDto();
            object[] param = { jsonObj.Id };
            result.number = userDao.UpdateImagePath(jsonObj);
            result.UserObject = userDao.GetUserDetail(param);
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// view profile
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public UserDto ViewProfile(string Email)
        {
            UserDto dto = new UserDto();
            dto = userDao.ViewProfile(Email);
            return dto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actived User
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto ActivedUser(UserDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (json != null)
            {
                object[] pr = { json.Id };
                UserDto user = userDao.GetUserDetail(pr);
                int count = userDao.ActivedUser(user);
                // GET MANAGER BY TEAM
                IEnumerable<UserDto> obj = userDao.GetManagerByTeam(user.Team);
                if (count == 1)
                {
                    result.code = Constant.ACTIVED_USER;
                    result.message = Constant.MESSAGE_ACTIVED_USER;
                    object[] format = new object[4];
                    foreach (UserDto item in obj)
                    {
                        format[0] += item.Email + ";";
                    }
                    format[1] = user.UserId;
                    format[2] = user.UserName;
                    format[3] = user.Email;
                    Utils.SendMail(Constant.TITLE_USER_ACTIVED, string.Format(Constant.CONTENT_USER_ACTIVED, format)
                        , user.Email, true, string.Empty);
                }
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            
            return result;
            throw new NotImplementedException();
        }
    }
}