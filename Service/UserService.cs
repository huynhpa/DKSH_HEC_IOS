using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface UserService
    {
        /// <summary>
        /// Search user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<UserDto> SearchUser(UserDto dto);

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto InsertUser(UserDto dto);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto UpdateUser(UserDto dto);

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserDto GetUserDetail(UserDto user);

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResultMessageDto DeleteUser(int Id);

        /// <summary>
        /// Get User By Team CDI
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserDto> GetUserByTeamCDI();

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto Registration(UserDto jsonParam);

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        ForgotPasswordDto ForgotPassword(ForgotPasswordDto dto);

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto ResetPassword(ForgotPasswordDto dto);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        ResultMessageDto UpdateProfile(UserDto jsonObj);

        /// <summary>
        /// Update Image Path
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        ResultMessageDto UpdateImagePath(UserDto jsonObj);

        /// <summary>
        /// View profile
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        UserDto ViewProfile(string Email);

        /// <summary>
        /// Actived user
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto ActivedUser(UserDto json);

    }
}
