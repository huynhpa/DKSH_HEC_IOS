using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface DoCampaignService
    {
        OutboundSurveyDataResult GetGeneralSurveyData(OBSurveyParam param);
        ResultMessageDto LoginAsterisk(LoginAsteriskDto loginAsteriskDto);
        ResultMessageDto SaveSurvey(OBSurveyParam param);
        LoadPhoneNumberResult GetNextAssign(OBSurveyParam param);
        ResultMessageDto QuickCall(OBSurveyParam param);
        ResultMessageDto GetAgentByUserName(hrEmployeeDto param);
        ResultMessageDto DeleteSurvey(OBSurveyParam param);
        ResultMessageDto GetQuestionAnswerBySurvey(OBSurveyParam para);
        LoadPhoneNumberResult GetSurveyById(OBSurveyParam para);
        ResultMessageDto SaveSurveyByQuestion(DataSurvey param, OBSurveyParam outbound);
        ResultMessageDto SearchSurveyByAgent(SearchCallParam json);
        ResultMessageDto Click2Call(Click2CallDto param);
        DoctorInformationDto GetDoctorInfo(OBSurveyParam para);
        IEnumerable<DoctorInformationDto> GetDoctorCallplan(OBSurveyParam para);
        List<ServiceTypeDto> GetCampaignFocusByCustomer(OBSurveyParam para);
        ResultMessageDto Click2CallGigamed(Click2CallDto param);

        ResultMessageDto AddContactList(iMindContactInfoDto json);

        ResultMessageDto SaveSpecialNote(iMindContactInfoDto json);
    }
}
