using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface CustomerSurveyService
    {
        /// <summary>
        /// Get Customer By CustCode
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto GetCustomerByCustCode(CustomerSurveyInfoDto json);

        /// <summary>
        /// Get Questionnaire
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        QuestionnaireDto GetQuestionnaire(CustomerSurveyInfoDto json);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto SaveCustomerSurveyInfo(CustomerSurveyInfoDto json);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<QuestionnaireTitleDto> GetQuestionTitle();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        CustomerSurveyInfoDto GetQuestionDetail(CustomerSurveyInfoDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto DeleteQuestion(CustomerSurveyInfoDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto EditQuestion(CustomerSurveyInfoDto dto);
        ResultMessageDto NewQuestion(CustomerSurveyInfoDto dto);
        IWorkbook CustomerSurveyReport();
        IEnumerable<CustomerSurveyInfoDto> GetSurveyHistory(CustomerSurveyInfoDto json);
        IEnumerable<iMindUserDto> GetiMindAgents();
    }
}
