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
    public class MemberShipDao
    {
        private GenericRepository<MemberShipDto> MemberShipRepo;
        private GenericRepository<UsersDto> UserRepo;
        private GenericRepository<iMindUserDto> agentRepository;
        private OBSEntity OBSEntity = null;
        private iMindEntity iMindEntity = null;

        /// <summary>
        /// INITIALIZE
        /// </summary>
        public MemberShipDao()
        {
            MemberShipRepo = new GenericRepository<MemberShipDto>(new OBSEntity());
            UserRepo = new GenericRepository<UsersDto>(new OBSEntity());
            agentRepository = new GenericRepository<iMindUserDto>(new iMindEntity());
            OBSEntity = new OBSEntity();
            iMindEntity = new iMindEntity();
        }

        internal int ChangePassword(MemberShipDto dto)
        {
            object[] param = { dto.UserId, Utils.GetPasswordHashAndSalt(dto.NewPassword) };
            string query = @"EXEC sp_ChangePassword {0}, {1}";
            return MemberShipRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal MemberShipDto Login(LoginParams loginDto)
        {
            object[] param = { loginDto.UserName, Utils.GetPasswordHashAndSalt(loginDto.Password) };
            string spQuery = @"EXEC [sp_Login] {0}, {1}";
            return MemberShipRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Get Member List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<UsersDto> GetMemberList(UsersDto dto)
        {
            object[] param = { dto.UserName, dto.EmployeeID, dto.AgentNumber, dto.FullName };
            string spQuery = @"EXEC sp_GetMemberList {0}, {1}, {2}, {3}";
            return UserRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Insert Member Ship
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertMemberShip(MemberShipDto dto)
        {
            object[] param = { dto.UserId, Utils.GetPasswordHashAndSalt(dto.Password), Utils.GetPasswordHashAndSalt(dto.PasswordSalt),
                                dto.UserName, dto.AgentNumber };
            string query = @"EXEC sp_InsertMemberShip {0}, {1}, {2}, {3}, {4}";
            return MemberShipRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// Delete Member Ship
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteMemberShip(MemberShipDto dto)
        {
            object[] param = { dto.UserId };
            string query = @"EXEC sp_DeleteMemberShip {0}";
            return MemberShipRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int ResetPassword(hrEmployeeDto dto)
        {
            object[] param = { dto.UserId, Utils.GetPasswordHashAndSalt(Constant.RESET_PASSWORD_AGENT) };
            string query = @"EXEC sp_ChangePassword {0}, {1}";
            return MemberShipRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// update profile
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateProfile(MemberShipDto dto)
        {
            string NewPassword = null;
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                NewPassword = Utils.GetPasswordHashAndSalt(dto.NewPassword);
            }
            object[] param = { dto.UserId, dto.FullName, dto.JobTitle, dto.Email, NewPassword };
            string query = @"EXEC sp_UpdateProfile {0}, {1}, {2}, {3}, {4}";
            return MemberShipRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// update image path
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal int UpdateImagePath(MemberShipDto obj)
        {
            object[] param = { obj.UserId, obj.ImagePath };
            string query = @"EXEC sp_UpdateImagePath {0}, {1}";
            return MemberShipRepo.ExecuteCommandOBS(query, param);
        }
    }
}