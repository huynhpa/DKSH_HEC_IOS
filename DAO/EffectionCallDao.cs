using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using DKSHApplication.Common;

namespace DKSHApplication.DAO
{
    public class EffectionCallDao
    {
        private GenericRepository<EffectionCallDto> effectionCallRepo;
        private OBSEntity obsEntity;

        /// <summary>
        /// INIT
        /// </summary>
        public EffectionCallDao()
        {
            obsEntity = new OBSEntity();
            effectionCallRepo = new GenericRepository<EffectionCallDto>(new OBSEntity());
        }

        /// <summary>
        /// Get Effection By Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<EffectionCallDto> GetEffectionByQuestion(EffectionCallDto dto)
        {
            object[] param = { dto.QuestionID };
            string spQuery = @"EXEC sp_GetEffectionByQuestion {0}";
            return effectionCallRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Save EffectionCall
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveEffectionCall(EffectionCallDto dto)
        {
            object[] param = {  dto.EffectId
                                , dto.AnswerId
                                , dto.QuestionID
                                , dto.CampaignName
                                , dto.AgentExt
                                , dto.AgentTeam
                                , dto.IsEffective
                                , dto.CreateBy
                                , dto.UpdateBy
                                , dto.SONumber };
            string spQuery = @"EXEC sp_SaveEffectionCall {0}";
            return effectionCallRepo.ExecuteCommandOBS(spQuery, param);
        }

    }
}