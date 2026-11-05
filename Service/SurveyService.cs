using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface SurveyService
    {
        int SetOrderIndex(string id, int index);
        int SetServiceOrderIndex(string id, int index);
        string GetImageType(string code);
        int UpdateCost(string lstid, string cost);
        string EditAgentNumber(string uid, string number);
        string EditFullName(string uid, string name);
        AnswerClass SaveChange(string id, string code, string type);
        Feedback SaveCallback(string id, string status, string comment);
        PhoneInfo GetByPhone(string phone);
        string UpdateStatus(string id, string status, string date, string note);
    }
}
