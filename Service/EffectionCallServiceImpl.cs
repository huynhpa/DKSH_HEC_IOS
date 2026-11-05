using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;

namespace DKSHApplication.Service
{
    public class EffectionCallServiceImpl : EffectionCallService
    {
        /// <summary>
        /// Effection Call
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EffectionCallDto EffectionCall(OBSurveyParam para)
        {
            EffectionCallDto effectionCall = new EffectionCallDto();
            return effectionCall;
        }

        /// <summary>
        /// Save EffectionCall
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ResultMessageDto SaveEffectionCall(EffectionCallDto para)
        {
            ResultMessageDto result = new ResultMessageDto();
            return result;
        }
    }
}