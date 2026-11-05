using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using MP.Core.Library;

namespace DKSHApplication.Service
{
    public class CampaignDetailServiceImpl : CampaignDetailService
    {
        QuestionDao questionDao = new QuestionDao();
        AnswerDao answerDao = new AnswerDao();
        ServiceQuestionsDao serviceQuestionsDao = new ServiceQuestionsDao();

        /// <summary>
        /// campaign detail
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public CampaignDetailDto GetCampaignDetailById(CampaignDetailDto json)
        {
            CampaignDetailDto result = answerDao.GetCampaignDetailById(json);
            return result;
        }
    }
}