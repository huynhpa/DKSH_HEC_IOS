using DKSHApplication.DAO;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DKSHApplication.Common
{
    public class CallPlanDaily : IJob
    {        
        public void Execute(IJobExecutionContext context)
        {
            CallPlanDao callPlanDao = new CallPlanDao();
            PhoneNumberDao phoneNunberDao = new PhoneNumberDao();
            ResultMessageDto result = new ResultMessageDto();
            string path = @"\\VNSGNFS03P.vn.dksh.net\Public\SAIGON-GROUP\SGN-IT-APPS\EchoPlus\OUTBOUND\TS\EXP_PREPLAN.uni";
            try
            {
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                List<string> list = new List<string>();
                // delete to update call plan
                int del = callPlanDao.DeleteCallPlan();
                CallPlanDto callPlanDto = null;
                using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
                {
                    string line = string.Empty;
                    int ins = 0;
                    int idx = 0;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (idx > 0)
                        {
                            list = line.Split('\t').ToList();
                            foreach (string item in list)
                            {
                                callPlanDto = new CallPlanDto()
                                {
                                    callplan_id = Guid.NewGuid(),
                                    salesman_code = list.ElementAt(0),
                                    route_date = list.ElementAt(1),
                                    cust_code = list.ElementAt(2),
                                    txn_timestamp = list.ElementAt(3)
                                };
                            }
                            ins += callPlanDao.SaveCallPlanFromBlueOcean(callPlanDto);
                        }
                        idx += 1;
                    }
                    if (ins > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = string.Format(Constant.PROCESS_MESSAGE, idx);
                    }
                }
                // get call plan by sale rep and import into table [PhoneNumber] to route assign
                CallPlanDto dto = new CallPlanDto();
                IEnumerable<CallPlanDto> call_plan = callPlanDao.GetCallPlanDaily(dto);
                PhoneNumberDto phoneNumber = null;
                int n = 0;
                foreach (CallPlanDto item in call_plan)
                {
                    phoneNumber = new PhoneNumberDto()
                    {
                        ID = Guid.NewGuid(),
                        PhoneNumber = item.cust_code,
                        PhoneGroup = item.WindowsUser,
                        ServiceType = item.project_id,
                        ProjectId = (Guid)item.project_id,
                        CreatedBy = Constant.CALL_PLAN,
                        UpdatedBy = Constant.CALL_PLAN
                    };
                    n += phoneNunberDao.ImportPhoneNumber(phoneNumber);
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
        }

    }
}