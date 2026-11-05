using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class LoginServiceImpl : LoginService
    {
        MemberShipDao memberShipDao = new MemberShipDao();

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public MemberShipDto Login(LoginParams loginDto)
        {
            MemberShipDto memberInfo = memberShipDao.Login(loginDto);
            return memberInfo;
        }

    }
}