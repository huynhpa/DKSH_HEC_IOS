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
    public class SurveyServiceImpl : SurveyService
    {
        SurveyVoteDao surveyVoteDao = new SurveyVoteDao();
        HrEmployeeDao hrEmployeeDao = new HrEmployeeDao();       
        public string EditAgentNumber(string uid, string number)
        {
            throw new NotImplementedException();
        }

        public string EditFullName(string uid, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetByPhone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public PhoneInfo GetByPhone(string phone)
        {
            OBSurveyParam obsParam = new OBSurveyParam();
            obsParam.PhoneNumber = phone;

            PhoneInfo Pobj = new PhoneInfo();
            SurveyVoteDto temp = surveyVoteDao.GetByPhoneNumber(obsParam);
            if (temp != null)
            {
                hrEmployeeDto emp = new hrEmployeeDto();
                emp.ID = FGuid.ToGuid(temp.EmployeeID);
                if (temp.SurveyDate == DateTime.Now.ToString("dd-MM-yyyy") && phone != "Không tìm thấy phân công")
                {
                    Pobj.type = 5;
                    Pobj.date = temp.SurveyDate;                    
                    hrEmployeeDto hrObj = hrEmployeeDao.GetEmployeeDetail(emp);
                    if (hrObj != null) {
                        Pobj.agentNumber = hrObj.AgentNumber;
                    }
                }
                else
                {
                    Pobj.type = 2;
                    Pobj.date = temp.SurveyDate;
                    hrEmployeeDto hrObj = hrEmployeeDao.GetEmployeeDetail(emp);
                    if (hrObj != null)
                    {
                        Pobj.agentNumber = hrObj.AgentNumber;
                    }
                }
            }
            else
            {
                Pobj.type = 0;
            }
            return Pobj;
        }

        public string GetImageType(string code)
        {
            throw new NotImplementedException();
        }

        public Feedback SaveCallback(string id, string status, string comment)
        {
            throw new NotImplementedException();
        }

        public AnswerClass SaveChange(string id, string code, string type)
        {
            throw new NotImplementedException();
        }

        public int SetOrderIndex(string id, int index)
        {
            throw new NotImplementedException();
        }

        public int SetServiceOrderIndex(string id, int index)
        {
            throw new NotImplementedException();
        }

        public int UpdateCost(string lstid, string cost)
        {
            throw new NotImplementedException();
        }

        public string UpdateStatus(string id, string status, string date, string note)
        {
            throw new NotImplementedException();
        }
    }
}