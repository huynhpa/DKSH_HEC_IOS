using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.Common;
using DKSHApplication.DAOBusiness;
using DKSHApplication.DB;
using DKSHApplication.DTO;

namespace DKSHApplication.DAO
{
    public class ZaloTrackingDao
    {
        private GenericRepository<ZaloTrackingDto> zaloRepo;
        private OBSEntity obsEntity = null;
        public ZaloTrackingDao()
        {
            zaloRepo = new GenericRepository<ZaloTrackingDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Insert Zalo Tracking
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertZaloTracking(ZaloTrackingDto dto)
        {
            object[] param = { dto.Id
                               ,dto.ImagePath
                               ,dto.ImageName
                               ,dto.CustCode
                               ,dto.SurveyId
                               ,dto.ServiceTypeId
                               ,dto.EmployeeId
                               //,dto.ImageCode
                               ,dto.CreateBy };
            string spQuery = @"EXEC sp_SaveZaloTracking {0},{1},{2},{3},{4},{5},{6},{7}";
            //string spQuery = @"EXEC sp_SaveZaloTracking {0},{1},{2},{3},{4},{5},{6},{7},{8}";
            return zaloRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Update Zalo Tracking
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateZaloTracking(ZaloTrackingDto dto)
        {
            object[] param = { dto.Id
                               ,dto.ImagePath
                               ,dto.ImageName
                               ,dto.CustCode
                               ,dto.SurveyId
                               ,dto.ServiceTypeId
                               ,dto.EmployeeId
                               ,dto.UpdateBy
                               ,dto.ImageCode
                               ,dto.ImageStatus };
            string spQuery = @"EXEC sp_UpdateZaloTracking {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}";
            return zaloRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// GetZaloById
        /// </summary>
        /// <param name="zaloDto"></param>
        /// <returns></returns>
        internal ZaloTrackingDto GetZaloById(ZaloTrackingDto zaloDto)
        {
            object[] param = { zaloDto.SurveyId , zaloDto.ServiceTypeId };
            string spQuery = @"exec sp_GetZaloById {0},{1}";
            return zaloRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Update Image Code
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateImageCode(ZaloTrackingDto dto)
        {
            object[] parameter = { dto.Id, dto.ImageCode, dto.ImageStatus };
            string spQuery = @"EXEC [sp_UpdateImageCode] {0}, {1}, {2}";
            return zaloRepo.ExecuteCommandOBS(spQuery, parameter);
        }
        /// <summary>
        /// Zalo Tracking Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ZaloTrackingDto ZaloTrackingDetail(ZaloTrackingDto dto)
        {
            object[] parameter = { dto.Id };
            string spQuery = @"EXEC [sp_ZaloTrackingDetail] {0}";
            return zaloRepo.ExecuteQuerySingleOBS(spQuery, parameter);
        }

        /// <summary>
        /// Delete Zalo Tracking
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int DeleteZaloTracking(ZaloTrackingDto dto)
        {
            object[] parameter = { dto.Id, dto.SurveyId };
            string spQuery = @"EXEC [sp_DeleteZaloTracking] {0}, {1}";
            return zaloRepo.ExecuteCommandOBS(spQuery, parameter);
        }
    }
}