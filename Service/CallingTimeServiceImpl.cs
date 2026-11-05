using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using MP.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class CallingTimeServiceImpl : CallingTimeService
    {
        CallingTimeDao callingTimeDao = new CallingTimeDao();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CallingTimeDto> SearchCallingTime(CallingTimeDto dto)
        {
            IEnumerable<CallingTimeDto> result = callingTimeDao.SearchCallingTime(dto);
            return result;
        }

        /// <summary>
        /// SetCallingTime
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto SetCallingTime(CallingTimeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = callingTimeDao.SetCallingTime(dto);
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
    }
}