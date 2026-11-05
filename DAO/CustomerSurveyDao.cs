using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class CustomerSurveyDao
    {
        private GenericRepository<QuestionChildDto> questionChildRepo;

        private GenericRepository<CustomerSurveyInfoDto> customerSurveyRepo;

        private GenericRepository<CustomerSurveyMarkDto> customerSurveyMarkRepo;

        private GenericRepository<QuestionnaireTitleDto> questionTitleRepo;

        private GenericRepository<ContactInformationDto> contactInfoRepo;

        private CCCToolsEntity cccToolsEntity = null;

        private iMindEntity iMindEntity = null;

        /// <summary>
        /// Initial
        /// </summary>
        public CustomerSurveyDao()
        {
            questionChildRepo = new GenericRepository<QuestionChildDto>(new CCCToolsEntity());
            customerSurveyRepo = new GenericRepository<CustomerSurveyInfoDto>(new CCCToolsEntity());
            questionTitleRepo = new GenericRepository<QuestionnaireTitleDto>(new CCCToolsEntity());
            customerSurveyMarkRepo = new GenericRepository<CustomerSurveyMarkDto>(new CCCToolsEntity());
            contactInfoRepo = new GenericRepository<ContactInformationDto>(new iMindEntity());
            cccToolsEntity = new CCCToolsEntity();
            iMindEntity = new iMindEntity();
        }

        /// <summary>
        /// Customer Care Center
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionChildDto> CustomerCareCenter()
        {
            object[] parameter = {  };
            string spQuery = @"EXEC [CustomerCareCenter]";
            return questionChildRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Transportation
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionChildDto> Transportation()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [Transportation]";
            return questionChildRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Credit
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionChildDto> Credit()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [Credit]";
            return questionChildRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Sales Rep
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionChildDto> SalesRep()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [SalesRep]";
            return questionChildRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// iMind get contact info
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ContactInformationDto GetContactInfo(CustomerSurveyInfoDto json)
        {
            object[] parameter = { json.CustCode };
            string spQuery = @"EXEC [sp_GetContactInfo] {0}";
            return contactInfoRepo.ExecuteQuerySingleIMind(spQuery, parameter);
        }

        /// <summary>
        /// Save Customer Survey Info
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int SaveCustomerSurveyMark(CustomerSurveyInfoDto pr)
        {
            object[] parameter = { Utils.StringDateFormat(pr.SurveyDate), pr.CustCode, pr.ContactName, pr.Tel, pr.CustomerComment,
                                    pr.UpdateBy, pr.CreateBy, pr.TotalMark, pr.QuestionnaireTitleId,
                                    pr.QuestionParentId, pr.QuestionChildId, pr.Remark, pr.UserGroup, pr.UserSurvey };
            string spQuery = @"EXEC [sp_SaveCustomerSurveyInfo] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}";
            return customerSurveyRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Get Question Title
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionnaireTitleDto> GetQuestionTitle()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_GetQuestionTitle]";
            return questionTitleRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Get Question Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CustomerSurveyInfoDto GetQuestionDetail(CustomerSurveyInfoDto dto)
        {
            object[] para = { dto.QuestionnaireTitleId, dto.QuestionParentId, dto.QuestionChildId };
            string spQuery = @"EXEC [sp_GetQuestionDetail] {0}, {1}, {2}";
            return customerSurveyRepo.ExecuteQuerySingle(spQuery, para);
        }

        /// <summary>
        /// New question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int NewQuestion(CustomerSurveyInfoDto pr)
        {
            object[] parameter = { pr.QuestionnaireTitleId, pr.QuestionParentId, pr.QuestionChildId, pr.QuestionVN, pr.QuestionEN, pr.RankMark };
            string spQuery = @"EXEC [NewQuestion] {0}, {1}, {2}, {3}, {4}, {5}";
            return customerSurveyRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Edit Question
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int EditQuestion(CustomerSurveyInfoDto pr)
        {
            object[] parameter = { pr.QuestionnaireTitleId, pr.QuestionParentId, pr.QuestionChildId, pr.QuestionVN, pr.QuestionEN, pr.RankMark };
            string spQuery = @"EXEC [EditQuestion] {0}, {1}, {2}, {3}, {4}, {5}";
            return customerSurveyRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int DeleteQuestion(CustomerSurveyInfoDto pr)
        {
            object[] parameter = { pr.QuestionnaireTitleId, pr.QuestionParentId, pr.QuestionChildId };
            string spQuery = @"EXEC [DeleteQuestion] {0}, {1}, {2}";
            return customerSurveyRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public int DeleteCustomerSurveyed(CustomerSurveyInfoDto json)
        {
            object[] parameter = { json.CustCode, json.SurveyDate };
            string spQuery = @"EXEC [sp_DeleteCustomerSurveyed] {0}, {1}";
            return customerSurveyRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Save Customer Survey Info
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int SaveCustomerSurveyInfo(CustomerSurveyInfoDto pr)
        {
            object[] parameter = { Utils.StringDateFormat(pr.SurveyDate), pr.CustCode, pr.ContactName, pr.Tel, pr.CustomerComment,
                                    pr.UpdateBy, pr.CreateBy, pr.TotalMark, pr.QuestionnaireTitleId,
                                    pr.QuestionParentId, pr.QuestionChildId, pr.Remark, pr.UserGroup, pr.UserSurvey };
            string spQuery = @"EXEC [sp_SaveCustomerSurvey] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}";
            return customerSurveyRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Check Question Existed
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public CustomerSurveyInfoDto CheckQuestionExisted(CustomerSurveyInfoDto pr)
        {
            object[] parameter = { pr.QuestionnaireTitleId, pr.QuestionParentId, pr.QuestionChildId };
            string spQuery = @"EXEC [CheckQuestionExisted] {0}, {1}, {2}";
            return customerSurveyRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Get Customer Survey Report
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public IEnumerable<CustomerSurveyInfoDto> GetCustomerSurveyReport(QuestionnaireTitleDto pr)
        {
            object[] parameter = { pr.QuestionnaireTitleId };
            string spQuery = @"EXEC [sp_GetCustomerSurveyReport] {0}";
            return customerSurveyRepo.ExecuteQuery(spQuery, parameter);
        }

        /// Get Customer Survey Mark
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerSurveyMarkDto> GetCustomerSurveyMark(CustomerSurveyInfoDto dto)
        {
            object[] parameter = { dto.CustCode };
            string spQuery = @"EXEC [sp_GetCustomerSurveyMark] {0}";
            return customerSurveyMarkRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Customer Survey Mark With Question Title
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerSurveyMarkDto> CustomerSurveyMarkWithQuestion(CustomerSurveyInfoDto dto)
        {
            object[] parameter = { dto.CustCode };
            string spQuery = @"EXEC [sp_CustomerSurveyMark] {0}, {1}";
            return customerSurveyMarkRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Get Customer Survey Mark
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CustomerSurveyInfoDto GetCustomerSurveyInfo(CustomerSurveyInfoDto dto)
        {
            object[] parameter = { dto.CustCode };
            string spQuery = @"EXEC [sp_GetCustomerSurveyInfo] {0}";
            return customerSurveyRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Customer Survey Info
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerSurveyInfoDto> CustomerSurveyInfo()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_CustomerSurveyInfo]";
            return customerSurveyRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Get Survey History
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<CustomerSurveyInfoDto> GetSurveyHistory(CustomerSurveyInfoDto json)
        {
            object[] parameter = { json.SurveyFromDate, json.SurveyToDate, json.CustCode, json.CustName, json.UserSurvey, json.UserGroup };
            string spQuery = @"EXEC [sp_GetSurveyHistory] {0}, {1}, {2}, {3}, {4}, {5}";
            return customerSurveyRepo.ExecuteQuery(spQuery, parameter);
        }
    }
}