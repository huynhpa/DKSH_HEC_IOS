using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class CallingTimeDao
    {
        private GenericRepository<CallingTimeDto> callingTimeRepo;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// init
        /// </summary>
        public CallingTimeDao()
        {
            callingTimeRepo = new GenericRepository<CallingTimeDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Calling Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallingTimeDto> CallingTime(OBSurveyParam dto)
        {
            object[] parameter = { dto.CustCode };
            string spQuery = @"EXEC [sp_CustomerCallingTime] {0}";
            return callingTimeRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SetCallingTime(CallingTimeDto dto)
        {
            object[] parameter = { dto.CustomerCode, dto.PhoneNumber, dto.Priority1, dto.Priority2 };
            string spQuery = @"EXEC [sp_SetCallingTime] {0},{1},{2},{3}";
            return callingTimeRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Search Calling Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallingTimeDto> SearchCallingTime(CallingTimeDto dto)
        {
            object[] parameter = { dto.CustomerCode };
            string spQuery = @"EXEC [sp_CallingTime] {0}";
            return callingTimeRepo.ExecuteQueryOBS(spQuery, parameter);
        }
    }
}