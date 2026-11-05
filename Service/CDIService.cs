using DKSHApplication.DB;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public interface CDIService
    {
        /// <summary>
        /// get all cdi list
        /// </summary>
        /// <returns></returns>
        IEnumerable<CDIDto> GetAllCDI(CDIDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        CDIDto GetCDIDetail(CDIDto dto);

        /// <summary>
        /// Unused CDI
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto UnusedCDI(CDIDto dto);

        /// <summary>
        /// Update Cdi
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto UpdateCdi(CDIDto dto);

        /// <summary>
        /// Insert Cdi
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto InsertCdi(CDIDto dto);

        /// <summary>
        /// Search Certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<CertificationDto> SearchCertification(CertificateParam dto);

        /// <summary>
        /// Update Certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto UpdateCertification(CertificationDto dto);


        /// <summary>
        /// Insert Certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto InsertCertification(CertificationDto dto);

        /// <summary>
        /// Get Certificate Detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        CertificationDto GetCertificateDetail(CertificateParam param);

        /// <summary>
        /// Update condition enough to buy
        /// </summary>
        /// <returns></returns>
        int IsCertification();

        /// <summary>
        /// Get is certification
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        IEnumerable<CertificationDto> GetIsCertification(CertificateParam jsonParam);
        IEnumerable<CustomerShipToDto> SearchShipTo(CustomerShipToDto shipto);

        /// <summary>
        /// Get Certificate Expired
        /// </summary>
        /// <returns></returns>
        CertificateInvalid GetCertificateExpired();

        /// <summary>
        /// Get Certificate Coming Expired Soon
        /// </summary>
        /// <returns></returns>
        CertificateInvalid GetCertificateComingExpiredSoon();

        /// <summary>
        /// Search Certificate Invalid
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        IEnumerable<CertificationDto> SearchCertificateInvalid(CertificateParam jsonParam);

        /// <summary>
        /// Get Cust Group
        /// </summary>
        /// <returns></returns>
        IEnumerable<CustGroupCode> GetCustGroup();

        /// <summary>
        /// ExportExcelBySearch
        /// </summary>
        /// <param name="cdiMngList"></param>
        /// <returns></returns>
        IWorkbook ExportExcelBySearch(IEnumerable<CDIDto> cdiMngList, string sheetName, string fileTemplate);

        /// <summary>
        /// Process Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto ProcessCustomer(CDIDto dto);

        /// <summary>
        /// Certification History
        /// </summary>
        /// <param name="jsonPr"></param>
        /// <returns></returns>
        IEnumerable<CertificationHistoryDto> CertificationHistory(CDIDto jsonPr);

        /// <summary>
        /// Check Cust Code Exist
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto CheckCustCodeExist(CDIDto json);

        /// <summary>
        /// Daily Report
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        IWorkbook DailyReport(CDIDto js);


        /// <summary>
        /// Required Licences
        /// </summary>
        /// <returns></returns>
        IEnumerable<CDIDto> RequiredLicences(CDIDto js);

        /// <summary>
        /// Export Required Licenses
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        IWorkbook ExportRequiredLicenses(CDIDto js);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResultMessageDto BackupData(CDIDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        ResultMessageDto UploadCustomerNTBVC(HttpPostedFileBase aFile);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hour"></param>
        void ScheduleTimer(int hour);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto SetupTimeline(SetupTimelineDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultMessageDto UploadCodeShipTo(HttpPostedFileBase aFile, string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        CustomerShipToDto GetDetailLicenseShipTo(CustomerShipToDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<CustomerShipToDto> GetShipToByCustCode(CustomerShipToDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ResultMessageDto UpdateShipToLicense(CustomerShipToDto obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<CustomerShipToDto> GetShipToBySoldTo(CDIDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<CustomerShipToDto> ShipToList(CustomerShipToDto dto);
        IWorkbook ExportShipTo(IEnumerable<CustomerShipToDto> shiptoList, string cERTIFICATION, string cERTIFICATION_TEMPLATE);
        IWorkbook ExportG950(CDIDto js);
        string ExportProductByFile(CDIDto js);
    }
}