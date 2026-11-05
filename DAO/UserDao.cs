using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class UserDao
    {
        private GenericRepository<UserDto> UserRepository;
        private GenericRepository<iMindUserDto> agentRepository;
        private CCCToolsEntity cccToolsEntity = null;
        private iMindEntity iMindEntity = null;

        public UserDao()
        {
            UserRepository = new GenericRepository<UserDto>(new CCCToolsEntity());
            agentRepository = new GenericRepository<iMindUserDto>(new iMindEntity());
            cccToolsEntity = new CCCToolsEntity();
            iMindEntity = new iMindEntity();
        }

        /// <summary>
        /// Search user
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<UserDto> SearchUser(UserDto dto)
        {
            object[] parameter = {
                                dto.UserId, dto.UserName, dto.RoleId, dto.Fullname, dto.Email,
                                dto.Team, dto.Department, dto.IsDelete, dto.IsActive, dto.JoinDateFrom,
                                dto.JoinDateTo
                            };
            string spQuery = @"[SearchUser] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}";
            return UserRepository.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// get user detail
        /// </summary>
        /// <returns></returns>
        public UserDto GetUserDetail(object[] parameter)
        {
            string spQuery = @"[GetUserDetail] {0}";
            return UserRepository.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int UnusedUser(int Id)
        {
            int num = 0;
            try
            {
                var user = (from u in cccToolsEntity.UserMngs
                            where u.Id == Id
                            select u).First();
                user.IsDelete = true;
                user.IsActive = false;
                num = cccToolsEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return num;
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int UpdateUser(object[] param)
        {
            string spQuery = "EXEC [UpdateUser] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}";
            int row = UserRepository.ExecuteCommand(spQuery, param);
            return row;
        }
        /// <summary>
        /// Add CDI
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int InsertUser(object[] param)
        {
            string spQuery = "EXEC [InsertUser] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}";
            int row = UserRepository.ExecuteCommand(spQuery, param);
            return row;
        }

        /// <summary>
        /// Check User Exist
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int CheckUserExist(UserDto param)
        {
            int num = 0;
            var userExist = (from u in cccToolsEntity.UserMngs
                              where u.UserId.Equals(param.UserId) || u.Email.Equals(param.Email)
                             select u).Count();
            num = userExist;
            return num;
        }

        /// <summary>
        /// Check User Update
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public UserDto CheckUserUpdate(object[] param)
        {
            string spQuery = "EXEC [CheckUserUpdate] {0}, {1}, {2}";
            return UserRepository.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Sign In
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public UserDto SignIn(object[] param)
        {
            string spQuery = @"EXEC [SignIn] {0}, {1}";
            return UserRepository.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// GetUserByTeamCDI
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserDto> GetUserByTeamCDI()
        {
            var query = from u in cccToolsEntity.UserMngs
                        where u.Team.Equals(Constant.TEAM_CDI) && u.IsActive == true
                        select new UserDto()
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Fullname = u.Fullname,
                            Email = u.Email,
                            IsUsing = u.IsUsing
                        };
            return query.ToList();
        }

        /// <summary>
        /// Registration a membership
        /// </summary>
        /// <param name="jsonPR"></param>
        /// <returns></returns>
        public int Registration(UserDto jsonPR)
        {
            object[] parameter = { jsonPR.UserId, jsonPR.UserName, jsonPR.Fullname, jsonPR.Email,
                                   jsonPR.Team, jsonPR.Department, jsonPR.JobTitle, jsonPR.JoinDate,
                                   jsonPR.CreateBy, jsonPR.UpdateBy, Utils.GetPasswordHashAndSalt(jsonPR.Pwd), jsonPR.IsManager,
                                   jsonPR.RoleId, jsonPR.IsActive };
            string spQuery = @"EXEC [Register] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}";
            int row = UserRepository.ExecuteCommand(spQuery, parameter);
            return row;
        }

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public int UpdatePasswordByEmail(ForgotPasswordDto dto)
        {
            try {
                var query = (from U in cccToolsEntity.UserMngs
                             where U.Email == dto.Email
                             select U).First();
                query.FlagChange = 1;
                query.Pwd = Utils.GetPasswordHashAndSalt(dto.ResetPassword);
                int cnt = cccToolsEntity.SaveChanges();
                return cnt;
            }
            catch(Exception ex)
            {
                throw ex;
            }                        
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public ResultMessageDto ResetPassword(ForgotPasswordDto pr)
        {
            ResultMessageDto rs = new ResultMessageDto();
            try
            {
                var query = (from U in cccToolsEntity.UserMngs
                             where U.Email == pr.Email
                             select U).First();
                query.FlagChange = 0; // update flag change default
                query.Pwd = Utils.GetPasswordHashAndSalt(pr.NewPassword);
                rs.number = cccToolsEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                rs.number = -1;
                rs.code = Constant.ERROR;
                rs.message = ex.Message;
            }
            return rs;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="jsonPR"></param>
        /// <returns></returns>
        public int UpdateProfile(UserDto jsonPR)
        {
            string pwd = string.Empty;
            if (!string.IsNullOrEmpty(jsonPR.Pwd))
            {
                pwd = Utils.GetPasswordHashAndSalt(jsonPR.Pwd);
            }
            else
            {
                pwd = GetCurrentPassword(jsonPR);
            }
            object[] parameter = { jsonPR.Id, jsonPR.UserName, jsonPR.Fullname, jsonPR.Email,
                                   jsonPR.Team, jsonPR.JobTitle, jsonPR.JoinDate, jsonPR.IsManager,
                                   pwd, jsonPR.ImagePath, jsonPR.Activity, jsonPR.Timeline,
                                   jsonPR.Experience, jsonPR.Skills, jsonPR.Location, jsonPR.Education,
                                   jsonPR.Facebook, jsonPR.Phone, jsonPR.UpdateBy };
            string spQuery = @"EXEC [UpdateProfile] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, 
                                {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}";
            int row = UserRepository.ExecuteCommand(spQuery, parameter);
            return row;
        }

        /// <summary>
        /// Get Current Password
        /// </summary>
        /// <param name="jsonPR"></param>
        /// <returns></returns>
        public string GetCurrentPassword(UserDto jsonPR)
        {
            var query = (from u in cccToolsEntity.UserMngs
                        where u.Id == jsonPR.Id
                        select u).First();
            string currentPassword = query.Pwd;
            return currentPassword;
        }

        /// <summary>
        /// Update Image Path
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public int UpdateImagePath(UserDto jsonObj)
        {
            object[] parameter = { jsonObj.UserId, jsonObj.ImagePath };
            string spQuery = @"EXEC [UpdateImagePath] {0}, {1}";
            int row = UserRepository.ExecuteCommand(spQuery, parameter);
            return row;
        }

        /// <summary>
        /// Personal profile
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public UserDto ViewProfile(string Email)
        {
            object[] param = { Email };
            string spQuery = "EXEC [PersonalProfile] {0}";
            return UserRepository.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Get Manager By Team
        /// </summary>
        /// <param name="Team"></param>
        /// <returns></returns>
        public IEnumerable<UserDto> GetManagerByTeam(string Team)
        {
            try
            {
                var query = (from u in cccToolsEntity.UserMngs
                             where u.Team.Equals(Team) && u.IsManager == true
                             select new UserDto()
                             {
                                 Id = u.Id,
                                 Email = u.Email,
                                 UserId = u.UserId,
                                 UserName = u.UserName,
                                 Fullname = u.Fullname
                             });
                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Email By User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserDto GetEmailByUserId(string userId)
        {
            try
            {
                var query = (from u in cccToolsEntity.UserMngs
                             where u.UserId.Equals(userId)
                             select new UserDto()
                             {
                                 Id = u.Id,
                                 UserId = u.UserId,
                                 Email = u.Email
                             });
                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Actived User
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public int ActivedUser(UserDto json)
        {
            try
            {
                int count = 0;
                var query = (from u in cccToolsEntity.UserMngs
                             where u.Id == json.Id
                             select u).SingleOrDefault();
                query.IsActive = true;
                count = cccToolsEntity.SaveChanges();
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Active Agent
        /// </summary>
        /// <returns></returns>
        public IEnumerable<iMindUserDto> GetActiveAgent()
        {
            object[] param = { };
            string spQuery = "EXEC [sp_GetAgentActive]";
            return agentRepository.ExecuteQueryiMind(spQuery, param);
        }
    }
}