using DKSHApplication.Common;
using DKSHApplication.DAOBusiness;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class PhoneNumberDao
    {
        private GenericRepository<ServiceTypeDto> serviceTypeRepo;
        private GenericRepository<hrEmployeeDto> hrEmpRepo;
        private GenericRepository<StatusDto> statusRepo;
        private GenericRepository<AgeRankDto> ageRankRepo;
        private GenericRepository<PhoneNumberDto> phoneNumberRepo;
        private GenericRepository<SummaryPhoneDto> SummaryPhoneRepo;
        private OBSEntity obsEntity = null;

        
        /// <summary>
        /// initialize
        /// </summary>
        public PhoneNumberDao()
        {
            serviceTypeRepo = new GenericRepository<ServiceTypeDto>(new OBSEntity());
            hrEmpRepo = new GenericRepository<hrEmployeeDto>(new OBSEntity());
            statusRepo = new GenericRepository<StatusDto>(new OBSEntity());
            ageRankRepo = new GenericRepository<AgeRankDto>(new OBSEntity());
            phoneNumberRepo = new GenericRepository<PhoneNumberDto>(new OBSEntity());
            SummaryPhoneRepo = new GenericRepository<SummaryPhoneDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// search phone number assigned
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<PhoneNumberDto> SearchPhoneNumber(PhoneNumberDto dto)
        {
            object[] param = { dto.CreatedDate, dto.ProjectId, dto.PhoneNumber, dto.PhoneGroup };
            string procQuery = @"EXEC sp_SearchPhoneNumber {0}, {1}, {2}, {3}";
            return phoneNumberRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SummaryPhoneDto> SummaryPhoneNumber(PhoneNumberDto dto)
        {
            object[] param = { };
            string procQuery = @"EXEC sp_SummaryPhoneNumber";
            return SummaryPhoneRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// Insert Phone Number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertPhoneNumber(PhoneNumberDto dto)
        {
            object[] param = { dto.ID, dto.PhoneNumber, dto.ServiceType, dto.PhoneGroup,
                                dto.CreatedBy, dto.UpdatedBy, dto.IsDeleted };
            string procQuery = @"EXEC sp_InsertPhoneNumber {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return phoneNumberRepo.ExecuteCommandOBS(procQuery, param);
        }

        /// <summary>
        /// Quick call
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int QuickCall(PhoneNumberDto dto)
        {
            object[] param = { dto.ID, dto.PhoneNumber, dto.ServiceType, dto.PhoneGroup,
                                dto.CreatedBy, dto.UpdatedBy, dto.IsDeleted, dto.ProjectId };
            string procQuery = @"EXEC sp_QuickCall {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return phoneNumberRepo.ExecuteCommandOBS(procQuery, param);
        }

        /// <summary>
        /// Get Phone Number Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal PhoneNumberDto GetPhoneNumberDetail(PhoneNumberDto dto)
        {
            object[] param = { dto.ID };
            string procQuery = @"EXEC sp_GetPhoneNumberDetail {0}";
            return phoneNumberRepo.ExecuteQuerySingleOBS(procQuery, param);
        }

        /// <summary>
        /// Count Phone Number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<PhoneNumberDto> CountPhoneNumber(PhoneNumberDto dto)
        {
            //object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber, dto.ProjectId };
            //string procQuery = @"EXEC sp_GetPhoneNumber {0}, {1}, {2}, {3}";
            object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber };            
            string procQuery = @"EXEC sp_GetPhoneNumber {0}, {1}, {2}";
            return phoneNumberRepo.ExecuteQueryOBS(procQuery, param);
        }

        /// <summary>
        /// top 1 phone number by current date
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal PhoneNumberDto Top1PhoneNumberCurrentDate(PhoneNumberDto dto)
        {
            object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber, dto.ProjectId };
            string procQuery = @"EXEC sp_Top1PhoneNumberCurrentDate {0}, {1}, {2}, {3}";
            //object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber };
            //string procQuery = @"EXEC sp_Top1PhoneNumberCurrentDate {0}, {1}, {2}";
            return phoneNumberRepo.ExecuteQuerySingleOBS(procQuery, param);
        }

        /// <summary>
        /// Top1 Phone Number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal PhoneNumberDto Top1PhoneNumber(PhoneNumberDto dto)
        {
            object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber, dto.ProjectId };
            string procQuery = @"EXEC sp_Top1PhoneNumber {0}, {1}, {2}, {3}";
            //object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber };
            //string procQuery = @"EXEC sp_Top1PhoneNumber {0}, {1}, {2}";
            return phoneNumberRepo.ExecuteQuerySingleOBS(procQuery, param);
        }

        /// <summary>
        /// Top1 Phone Number AppointmentDate
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal PhoneNumberDto Top1PhoneNumberAppointmentDate(PhoneNumberDto dto)
        {
            //object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber, dto.ProjectId };
            //string procQuery = @"EXEC sp_Top1PhoneNumberAppointmentDate {0}, {1}, {2}, {3}";
            object[] param = { dto.ServiceType, dto.WindowsUser, dto.PhoneNumber };
            string procQuery = @"EXEC sp_Top1PhoneNumberAppointmentDate {0}, {1}, {2}";
            return phoneNumberRepo.ExecuteQuerySingleOBS(procQuery, param);
        }

        /// <summary>
        /// Update Is Assigned
        /// </summary>
        /// <param name="pnEntity"></param>
        /// <returns></returns>
        internal int UpdateIsAssigned(PhoneNumberDto pnEntity)
        {
            object[] param = { pnEntity.ID, pnEntity.IsAssigned };
            string procQuery = @"EXEC sp_UpdatePhoneNumber {0}, {1}";
            return phoneNumberRepo.ExecuteQueryCountOBS(procQuery, param);
        }

        /// <summary>
        /// Import Phone Number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int ImportPhoneNumber(PhoneNumberDto dto)
        {
            //object[] param = { dto.ID, dto.PhoneNumber, dto.CustName, dto.PhoneGroup, dto.CreatedDate,
            //                dto.ServiceType, dto.CreatedBy, dto.UpdatedBy, dto.ProjectId, dto.CustomField1 };
            //string procQuery = @"EXEC sp_ImportPhoneNumber {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
            object[] param = { dto.ID, dto.PhoneNumber, dto.CustName, dto.PhoneGroup, dto.CreatedDate,
                            dto.ProjectId, dto.CreatedBy };
            string procQuery = @"EXEC sp_ImportPhoneNumber {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return phoneNumberRepo.ExecuteCommandOBS(procQuery, param);
        }

        /// <summary>
        /// route to phone number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int RouteToPhoneNumber(PhoneNumberDto dto)
        {
            object[] param = { dto.PhoneNumber, dto.PhoneGroup };
            string procQuery = @"EXEC sp_RouteToPhoneNumber {0}, {1}";
            return phoneNumberRepo.ExecuteCommandOBS(procQuery, param);
        }

        /// <summary>
        /// Remove Assignment
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int RemoveAssignment(AssignmentDto dto)
        {
            object[] param = { dto.ProjectID, dto.UserId };
            string procQuery = @"EXEC sp_RemoveAssignment {0}, {1}";
            return phoneNumberRepo.ExecuteCommandOBS(procQuery, param);
        }

        /// <summary>
        /// GetPhoneNumberFromCallPlan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal PhoneNumberDto GetPhoneNumberFromCallPlan(OBSurveyParam dto)
        {
            object[] param = { dto.ServiceTypeId, dto.WindowsUser, dto.ProjectID, Constant.CALL_PLAN };
            string procQuery = @"EXEC sp_GetPhoneNumberFromCallPlan {0}, {1}, {2}, {3}";
            return phoneNumberRepo.ExecuteQuerySingleOBS(procQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal PhoneNumberDto GetCustomerVime(OBSurveyParam obj)
        {
            string cust_code = obj.CustCode;
            if (string.IsNullOrEmpty(cust_code))
            {
                cust_code = obj.PhoneNumber;
            }
            object[] param = { cust_code };
            string procQuery = @"EXEC sp_GetCustomerVime {0}";
            return phoneNumberRepo.ExecuteQuerySingleOBS(procQuery, param);
        }
    }
}