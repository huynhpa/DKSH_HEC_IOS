using System;
using System.Collections.Generic;
using System.Linq;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using DKSHApplication.Common;
using DKSHApplication.DB;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Web;
using System.Timers;

namespace DKSHApplication.Service
{
    public class CDIServiceImpl : CDIService
    {
        CDIDao cdiDao = new CDIDao();
        UserDao userDao = new UserDao();
        CustGroupCodeDao custGroupDao = new CustGroupCodeDao();
        CustomerDao customerDao = new CustomerDao();
        static System.Timers.Timer timer;
        /// <summary>
        /// Implement Get all data from CDIMng
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CDIDto> GetAllCDI(CDIDto dto)
        {
            //RunProcessAndBackUpDaily();
            if (!string.IsNullOrEmpty(dto.CustCode))
            {
                object[] pr = { dto.CustCode };
                CDIDto cdiDto = cdiDao.GetCDIDetail(pr);
                if (cdiDto != null && cdiDto.IsManual == false)
                {
                    cdiDao.CheckCustCodeInDDKXB(cdiDto);
                    cdiDao.ProcessByCustCode(cdiDto);
                }
            }
            if (!string.IsNullOrEmpty(dto.Certification))
            {
                List<string> listCertificateType = dto.Certification.Split(',').ToList();                
                foreach(string item in listCertificateType)
                {
                    if (item.Equals(Constant.CER_GCNDDKKDD))
                    {
                        dto.GCNDDKKDD = item;
                    }
                    if (item.Equals(Constant.CER_GPP))
                    {
                        dto.GPP = item;
                    }
                    if (item.Equals(Constant.CER_GDP))
                    {
                        dto.GDP = item;
                    }
                    if (item.Equals(Constant.CER_GSP))
                    {
                        dto.GSP = item;
                    }
                    if (item.Equals(Constant.CER_KCB))
                    {
                        dto.KCB = item;
                    }
                    if (item.Equals(Constant.CER_GCNDDKTC))
                    {
                        dto.GCNDDKTC = item;
                    }
                }                
            }
            if (!string.IsNullOrEmpty(dto.DDKXB))
            {
                List<string> listDDKXB = dto.DDKXB.Split(',').ToList();
                foreach (string str in listDDKXB)
                {
                    if (str.Equals(Constant.TDPHCCDCGN))
                    {
                        dto.TDPHCCDCGN_Search = str;
                    }
                    if (str.Equals(Constant.TDNLDLT))
                    {
                        dto.TDNLDLT_Search = str;
                    }
                    if (str.Equals(Constant.TDDMCSD))
                    {
                        dto.TDDMCSD_Search = str;
                    }
                    if (str.Equals(Constant.SINHPHAM))
                    {
                        dto.SinhPham_Search = str;
                    }
                    if (str.Equals(Constant.VACCIN))
                    {
                        dto.Vaccin_Search = str;
                    }
                    if (str.Equals(Constant.BOTOXXEOMIN))
                    {
                        dto.BotoxXeomin_Search = str;
                    }
                }
            }
            if (!string.IsNullOrEmpty(dto.DateExpiredFrom) && string.IsNullOrEmpty(dto.DateExpiredTo))
            {
                dto.DateExpiredTo = dto.DateExpiredFrom;
            }
            if (!string.IsNullOrEmpty(dto.CreateDateFrom) && string.IsNullOrEmpty(dto.CreateDateTo))
            {
                dto.CreateDateTo = dto.CreateDateFrom;
            }
            if (!string.IsNullOrEmpty(dto.UpdateDateFrom) && string.IsNullOrEmpty(dto.UpdateDateTo))
            {
                dto.UpdateDateTo = dto.UpdateDateFrom;
            }
            IEnumerable<CDIDto> lst = cdiDao.GetSoldToList(dto);
            return lst;

        }

        /// <summary>
        /// get CDI detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CDIDto GetCDIDetail(CDIDto dto)
        {
            object[] param = { dto.CustCode };
            CDIDto result = cdiDao.GetCDIDetail(param);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerShipToDto> GetShipToBySoldTo(CDIDto dto)
        {
            IEnumerable<CustomerShipToDto> shipto = cdiDao.GetShipToByCustCode(dto);
            return shipto;
        }

        /// <summary>
        /// Unused Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UnusedCDI(CDIDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (!String.IsNullOrEmpty(dto.CustCode))
            {
                object[] param = { dto.CustCode, dto.UserId };
                int cnt = cdiDao.UnusedCdi(param);
                if (cnt > 0)
                {
                    result.number = cnt;
                    result.code = Constant.DELETE;
                    result.message = Constant.MESSAGE_DELELE;
                }
                else
                {
                    result.number = cnt;
                    result.message = result.message;
                }
            }
            return result;
        }

        /// <summary>
        /// Update CDI
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateCdi(CDIDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            try
            {                
                // SAVE HISTORY CERTIFICATE
                object[] chParam = { dto.CustCode };
                CDIDto certificateHistory = cdiDao.GetCDIDetail(chParam);
                certificateHistory.CDINumber = Guid.NewGuid();
                int x = cdiDao.InsertCertificationHistory(certificateHistory);
                // Update new information
                int cntTaxcode = 0;
                if (dto != null)
                {
                    dto.IsManual = dto.IsManual == null ? false : dto.IsManual;
                    if (!string.IsNullOrEmpty(dto.TaxCode))
                    {
                        dto.IsUpdateTaxCode = true;
                        cntTaxcode = cdiDao.UpdateTaxCodeFromCDIMng(dto);
                    }
                    object[] param = {
                                dto.CustCode, dto.TaxCode, dto.GPKD, dto.GPKDNumber, dto.GPKDName,
                                Utils.StringDateFormat(dto.GCNDDKKDDFromDate), Utils.StringDateFormat(dto.GCNDDKKDDToDate),
                                dto.GCNDDKKDDStatus, Utils.StringDateFormat(dto.GPPFromDate),
                                Utils.StringDateFormat(dto.GPPToDate), dto.GPPStatus, Utils.StringDateFormat(dto.GDPFromDate),
                                Utils.StringDateFormat(dto.GDPToDate), dto.GDPStatus, Utils.StringDateFormat(dto.GSPFromDate),
                                Utils.StringDateFormat(dto.GSPToDate), dto.GSPStatus,
                                Utils.StringDateFormat(dto.KCBFromDate), Utils.StringDateFormat(dto.KCBToDate),
                                dto.KCBStatus, dto.Vaccin, dto.SinhPham, dto.Remark, dto.UserId, dto.IsDDKKDD,
                                dto.IsGPP, dto.IsGDP, dto.IsGSP, dto.IsKCB, dto.IsCOETTBYT, dto.TDDMCSD, dto.NNKDMBTTBYT,
                                dto.TDNLDLT, dto.TDPHCCDCGN , dto.DDKXB_TDPHCCDCGN, dto.DDKXB_TDNLDLT, dto.DDKXB_TDDMCSD,
                                dto.DDKXB_SinhPham, dto.DDKXB_Vaccin, dto.IsManual, dto.DMKTCTBotoxBotulinum, dto.DDKM_BotoxXeomin,
                                Utils.StringDateFormat(dto.GCNDDKTCFromDate), Utils.StringDateFormat(dto.GCNDDKTCToDate), dto.IsGCNDDKTC,
                                dto.IsUpdateTaxCode, dto.CMND, dto.Included, dto.Excluded_TDPHCCDCGN, dto.Excluded_TDNLDLT, 
                                dto.Excluded_TDDMCSD, dto.RemarkKSDB, dto.IncludedGPP, dto.ExcludedGPP_TDPHCCDCGN, dto.ExcludedGPP_TDNLDLT,
                                dto.ExcludedGPP_TDDMCSD, dto.RemarkGPP_KSDB, dto.RX, dto.OTC, dto.EssentialDrugs,
                                dto.DDKXB_RX, dto.DDKXB_OTC, dto.DDKXB_EssentialDrugs
                             };
                    int rsCnt = cdiDao.UpdateCdi(param);
                    if (dto.IsManual == false)
                    {
                        cdiDao.CheckCustCodeInDDKXB(dto);
                        int cnt = cdiDao.ProcessByCustCode(dto);
                    }
                    cdiDao.RequiredLicencesByCustomer(dto);
                    if (rsCnt > 0)
                    {
                        rs.code = Constant.SUCCESS;
                        rs.message = Constant.MESSAGE_SUCCESS;
                    }
                    else
                    {
                        rs.code = Constant.ERROR;
                        rs.message = Constant.MESSAGE_ERROR;
                    }
                }
            }
            catch(Exception ex)
            {
                rs.code = Constant.ERROR;
                rs.message = string.Format(Constant.MESSAGE_ERROR_CONVERT, ex.Message);
            }
            return rs;
        }

        /// <summary>
        /// Insert Cdi
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertCdi(CDIDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            try
            {
                int count = 0;
                int cntTaxcode = 0;
                if (dto.CustCode != null)
                {
                    dto.IsManual = dto.IsManual == null ? false : dto.IsManual;
                    if (!string.IsNullOrEmpty(dto.TaxCode))
                    {
                        dto.IsUpdateTaxCode = true;
                        cntTaxcode = cdiDao.UpdateTaxCodeFromCDIMng(dto);
                    }
                    object[] insParam = {
                                    dto.CustCode, dto.TaxCode, dto.GPKD, dto.GPKDNumber, dto.GPKDName,
                                    Utils.StringDateFormat(dto.GCNDDKKDDFromDate), Utils.StringDateFormat(dto.GCNDDKKDDToDate),
                                    dto.GCNDDKKDDStatus, Utils.StringDateFormat(dto.GPPFromDate),
                                    Utils.StringDateFormat(dto.GPPToDate), dto.GPPStatus, Utils.StringDateFormat(dto.GDPFromDate),
                                    Utils.StringDateFormat(dto.GDPToDate), dto.GDPStatus, Utils.StringDateFormat(dto.GSPFromDate),
                                    Utils.StringDateFormat(dto.GSPToDate), dto.GSPStatus,
                                    Utils.StringDateFormat(dto.KCBFromDate), Utils.StringDateFormat(dto.KCBToDate),
                                    dto.KCBStatus, dto.Vaccin, dto.SinhPham, dto.Remark, dto.UserId, dto.IsDDKKDD,
                                    dto.IsGPP, dto.IsGDP, dto.IsGSP, dto.IsKCB, dto.IsCOETTBYT, dto.TDDMCSD, dto.NNKDMBTTBYT,
                                    dto.TDNLDLT, dto.TDPHCCDCGN , dto.DDKXB_TDPHCCDCGN, dto.DDKXB_TDNLDLT, dto.DDKXB_TDDMCSD,
                                    dto.DDKXB_SinhPham, dto.DDKXB_Vaccin, dto.IsManual, dto.DMKTCTBotoxBotulinum, dto.DDKM_BotoxXeomin,
                                    dto.CreateBy,Utils.StringDateFormat(dto.GCNDDKTCFromDate), Utils.StringDateFormat(dto.GCNDDKTCToDate),
                                    dto.IsGCNDDKTC, dto.IsUpdateTaxCode, dto.CMND, dto.Included, dto.Excluded_TDPHCCDCGN, dto.Excluded_TDNLDLT,
                                    dto.Excluded_TDDMCSD, dto.RemarkKSDB, dto.IncludedGPP, dto.ExcludedGPP_TDPHCCDCGN, dto.ExcludedGPP_TDNLDLT,
                                    dto.ExcludedGPP_TDDMCSD, dto.RemarkGPP_KSDB, dto.RX, dto.OTC, dto.EssentialDrugs,
                                    dto.DDKXB_RX, dto.DDKXB_OTC, dto.DDKXB_EssentialDrugs
                                };
                    count = cdiDao.checkCustomerExist(dto);
                    if (count > 0)
                    {
                        rs.number = count;
                        rs.code = Constant.EXISTED;
                        rs.message = String.Format(Constant.CUSTOMER_CODE_EXIST, dto.CustCode);
                    }
                    else
                    {
                        rs.number = cdiDao.AddCdi(insParam);
                        if (dto.IsManual == false)
                        {
                            cdiDao.CheckCustCodeInDDKXB(dto);
                            int cnt = cdiDao.ProcessByCustCode(dto); // Process customer by custcode
                        }
                        cdiDao.RequiredLicencesByCustomer(dto);
                        if (rs.number == 1)
                        {
                            rs.code = Constant.INSERT;
                            rs.message = Constant.MESSAGE_SUCCESS;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                rs.code = Constant.ERROR;
                rs.message = string.Format(Constant.MESSAGE_ERROR_CONVERT, ex.Message);
            }
            
            return rs;
        }


        /// <summary>
        /// Search Certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CertificationDto> SearchCertification(CertificateParam dto)
        {
            IEnumerable<CertificationDto> certificateDto = cdiDao.SearchCertification(dto);
            return certificateDto;
        }

        /// <summary>
        /// Update Certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateCertification(CertificationDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            QueryResultDto queryRs = new QueryResultDto();
            object[] param = {
                                dto.CustCode, Utils.StringDateFormat(dto.GCNDDKKDDFromDate), Utils.StringDateFormat(dto.GCNDDKKDDToDate),
                                dto.GCNDDKKDDStatus, Utils.StringDateFormat(dto.GPPFromDate), Utils.StringDateFormat(dto.GPPToDate), dto.GPPStatus, Utils.StringDateFormat(dto.GDPFromDate),
                                Utils.StringDateFormat(dto.GDPToDate), dto.GDPStatus, Utils.StringDateFormat(dto.GSPFromDate), Utils.StringDateFormat(dto.GSPToDate), dto.GSPStatus,
                                Utils.StringDateFormat(dto.KCBFromDate), Utils.StringDateFormat(dto.KCBToDate), dto.KCBStatus, Utils.StringDateFormat(dto.KSDBFromDate),
                                Utils.StringDateFormat(dto.KSDBToDate), dto.KSDBStatus, Utils.StringDateFormat(dto.COETTBYTFromDate), Utils.StringDateFormat(dto.COETTBYTToDate),
                                dto.COETTBYTStatus, dto.UserId, dto.IsKSDB, dto.IsDDKKDD,
                                dto.IsGPP, dto.IsGDP, dto.IsGSP, dto.IsKCB, dto.IsCOETTBYT
                             };
            queryRs = cdiDao.UpdateCertification(param);
            if (queryRs.Code == 1)
            {
                rs.number = queryRs.Code;
                rs.code = Constant.UPDATE;
                rs.message = Constant.MESSAGE_SUCCESS;
            }
            else
            {
                rs.number = queryRs.Code;
                rs.code = Constant.ERROR;
                rs.message = Constant.MESSAGE_ERROR;
            }
            return rs;
        }

        /// <summary>
        /// Insert certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertCertification(CertificationDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            int cnt = 0;
            object[] param = {
                                dto.CustCode, Utils.StringDateFormat(dto.GCNDDKKDDFromDate), Utils.StringDateFormat(dto.GCNDDKKDDToDate),
                                dto.GCNDDKKDDStatus, Utils.StringDateFormat(dto.GPPFromDate), Utils.StringDateFormat(dto.GPPToDate), dto.GPPStatus, Utils.StringDateFormat(dto.GDPFromDate),
                                Utils.StringDateFormat(dto.GDPToDate), dto.GDPStatus, Utils.StringDateFormat(dto.GSPFromDate), Utils.StringDateFormat(dto.GSPToDate), dto.GSPStatus,
                                Utils.StringDateFormat(dto.KCBFromDate), Utils.StringDateFormat(dto.KCBToDate), dto.KCBStatus, Utils.StringDateFormat(dto.KSDBFromDate),
                                Utils.StringDateFormat(dto.KSDBToDate), dto.KSDBStatus, Utils.StringDateFormat(dto.COETTBYTFromDate), Utils.StringDateFormat(dto.COETTBYTToDate),
                                dto.COETTBYTStatus, dto.UserId, dto.CreateBy, dto.IsKSDB, dto.IsDDKKDD,
                                dto.IsGPP, dto.IsGDP, dto.IsGSP, dto.IsKCB, dto.IsCOETTBYT
                             };
            object[] checkExist = { dto.CustCode };
            CDIDto cdiObj = new CDIDto
            {
                CustCode = dto.CustCode
            };
            int count = cdiDao.checkCustomerExist(cdiObj);
            if (count > 0)
            {
                rs.number = count;
                rs.code = Constant.EXISTED;
                rs.message = String.Format(Constant.CUSTOMER_CODE_EXIST, dto.CustCode);                
            }
            else
            {
                cnt = cdiDao.InsertCertification(param);
                if (cnt > 0)
                {
                    rs.number = cnt;
                    rs.code = Constant.INSERT;
                    rs.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    rs.code = Constant.ERROR;
                    rs.message = Constant.MESSAGE_ERROR;
                }
            }
            return rs;
        }

        /// <summary>
        /// Get Certificate Detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public CertificationDto GetCertificateDetail(CertificateParam param)
        {
            object[] pr = { param.CustCode };
            CertificationDto obj = cdiDao.getCertificate(pr);
            return obj;
        }

        /// <summary>
        /// Is Certification
        /// </summary>
        /// <returns></returns>
        public int IsCertification()
        {
            int count = cdiDao.IsCertification();
            return count;
        }

        /// <summary>
        /// Get Is Certification
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<CertificationDto> GetIsCertification(CertificateParam jsonParam)
        {
            IEnumerable<CertificationDto> cerList = cdiDao.GetIsCertification(jsonParam);
            return cerList;
        }

        /// <summary>
        /// COUNT NUMBER CERTIFICATION EXPIRED
        /// </summary>
        /// <returns></returns>
        public CertificateInvalid GetCertificateExpired()
        {
            CertificateInvalid dto = cdiDao.GetCertificateExpired();
            return dto;
        }

        /// <summary>
        /// SEARCH CERTIFICATION EXPIRED
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<CertificationDto> SearchCertificateInvalid(CertificateParam jsonParam)
        {
            IEnumerable<CertificationDto> lstCer = cdiDao.SearchCertificateInvalid(jsonParam);
            return lstCer;
        }

        /// <summary>
        /// Get Certificate Coming Expired Soon
        /// </summary>
        /// <returns></returns>
        public CertificateInvalid GetCertificateComingExpiredSoon()
        {
            CertificateInvalid dto = cdiDao.GetCertificateComingExpiredSoon();
            return dto;
        }

        /// <summary>
        /// Get Cust Group
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustGroupCode> GetCustGroup()
        {
            IEnumerable<CustGroupCode> lst = custGroupDao.GetCustGroup();
            return lst;
        }

        /// <summary>
        /// Set Value To Cell Date Format
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cell"></param>
        public void SetValueToCellDateFormat(IWorkbook workbook, ICell cell, string dateValue)
        {
            if (!string.IsNullOrEmpty(dateValue))
            {
                cell.SetCellValue(dateValue);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY);
                cell.CellStyle = cellStyle;
            }
            else
            {
                cell.SetCellValue(dateValue);
            }
        }

        /// <summary>
        /// Export Excel By Search
        /// </summary>
        /// <param name="cdiMngList"></param>
        /// <returns></returns>
        public IWorkbook ExportExcelBySearch(IEnumerable<CDIDto> cdiMngList, string sheetName, string fileTemplate)
        {
            // Process customer
            CDIDto dto = new CDIDto();
            //int process = cdiDao.ProcessCustomer(dto);
            // Export data
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileTemplate;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY);
                var color_expired = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_warning = new XSSFColor(new byte[] { 255, 153, 0 });
                var color_x = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_red = new XSSFColor(new byte[] { 255, 0, 0});
                var color_info = new XSSFColor(new byte[] { 24, 213, 247 });
                var font_expired = (XSSFFont)workbook.CreateFont();
                var font_warning = (XSSFFont)workbook.CreateFont();
                var font_x = (XSSFFont)workbook.CreateFont();
                var font_red = (XSSFFont)workbook.CreateFont();
                var font_info = (XSSFFont)workbook.CreateFont();
                font_expired.SetColor(color_expired);
                font_warning.SetColor(color_warning);
                font_x.SetColor(color_x);
                font_red.SetColor(color_red);
                font_expired.IsBold = true;
                font_warning.IsBold = true;
                font_x.IsBold = true;
                font_red.IsBold = true;
                font_info.SetColor(color_info);
                font_info.IsBold = true;
                // Expired
                var style_expired = (XSSFCellStyle)workbook.CreateCellStyle(); 
                style_expired.SetFont(font_expired);
                // Coming expire
                var style_warning = (XSSFCellStyle)workbook.CreateCellStyle();
                style_warning.SetFont(font_warning);
                // Style for X value
                var style_x = (XSSFCellStyle)workbook.CreateCellStyle();
                style_x.SetFont(font_x);
                // style for red color
                var style_red = (XSSFCellStyle)workbook.CreateCellStyle();
                style_red.SetFont(font_red);
                // style for info
                var style_info = (XSSFCellStyle)workbook.CreateCellStyle();
                style_info.SetFont(font_info);
                // image insert
                string imgFile = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/")) + "QUAYTHUOCHONGTAM_GPP.jpg";
                byte[] data = File.ReadAllBytes(imgFile);
                ICellStyle cellLink = workbook.CreateCellStyle();                

                int pictureIndex = workbook.AddPicture(data, PictureType.JPEG);
                ICreationHelper helper = workbook.GetCreationHelper();
                // Create sheet
                ISheet sheet = workbook.GetSheet(sheetName);
                int rowIdx = 2;
                foreach (CDIDto obj in cdiMngList)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(obj.UserId); // Update By dd-mm-yyyy
                    // UpdateDate
                    if (!string.IsNullOrEmpty(obj.UpdateDate))
                    {
                        row.CreateCell(1).SetCellValue(new DateTime(int.Parse(obj.UpdateDate.Substring(6, 4)),
                            int.Parse(obj.UpdateDate.Substring(3, 2)), int.Parse(obj.UpdateDate.Substring(0, 2))));
                        row.GetCell(1).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(1).SetCellValue(obj.UpdateDate);
                    }
                    row.CreateCell(2).SetCellValue(obj.CreateBy); // CreateBy
                    // CreateDate
                    if (!string.IsNullOrEmpty(obj.CreateDate))
                    {
                        row.CreateCell(3).SetCellValue(new DateTime(int.Parse(obj.CreateDate.Substring(6, 4)),
                            int.Parse(obj.CreateDate.Substring(3, 2)), int.Parse(obj.CreateDate.Substring(0, 2))));
                        row.GetCell(3).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(3).SetCellValue(obj.CreateDate);
                    }
                    row.CreateCell(4).SetCellValue(obj.CustCode); // CustCode
                    row.CreateCell(5).SetCellValue(obj.CustName); // CustName
                    if (!string.IsNullOrEmpty(obj.GPPFromDate))
                    {
                        row.CreateCell(6).SetCellValue(new DateTime(int.Parse(obj.GPPFromDate.Substring(6, 4)),
                            int.Parse(obj.GPPFromDate.Substring(3, 2)), int.Parse(obj.GPPFromDate.Substring(0, 2))));
                        row.GetCell(6).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(6).SetCellValue(obj.GPPFromDate);
                    }
                    if (!string.IsNullOrEmpty(obj.GPPToDate))
                    {
                        row.CreateCell(7).SetCellValue(new DateTime(int.Parse(obj.GPPToDate.Substring(6, 4)),
                            int.Parse(obj.GPPToDate.Substring(3, 2)), int.Parse(obj.GPPToDate.Substring(0, 2))));
                        row.GetCell(7).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(obj.GPPToDate);
                    }
                    if (obj.GPPRemainingDays != null)
                    {
                        row.CreateCell(8).SetCellType(CellType.String);
                        if (obj.GPPRemainingDays <= 0)
                        {
                            row.CreateCell(8).SetCellValue(Constant.EXPIRED); // GCN GPP Remaining date
                            row.GetCell(8).CellStyle = style_expired;
                        }
                        else if (obj.GPPRemainingDays > 0 && obj.GPPRemainingDays <= 90)
                        {
                            row.CreateCell(8).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GPPRemainingDays.ToString())); // GCN GPP Remaining date
                            row.GetCell(8).CellStyle = style_warning;
                        }
                    }
                    // GDP
                    if (!string.IsNullOrEmpty(obj.GDPFromDate))
                    {
                        row.CreateCell(9).SetCellValue(new DateTime(int.Parse(obj.GDPFromDate.Substring(6, 4)),
                            int.Parse(obj.GDPFromDate.Substring(3, 2)), int.Parse(obj.GDPFromDate.Substring(0, 2))));
                        row.GetCell(9).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(9).SetCellValue(obj.GDPFromDate);
                        row.GetCell(9).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GDPToDate))
                    {
                        row.CreateCell(10).SetCellValue(new DateTime(int.Parse(obj.GDPToDate.Substring(6, 4)),
                            int.Parse(obj.GDPToDate.Substring(3, 2)), int.Parse(obj.GDPToDate.Substring(0, 2))));
                        row.GetCell(10).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(10).SetCellValue(obj.GDPToDate);
                        row.GetCell(10).CellStyle = cellStyle;
                    }
                    if (obj.GDPRemainingDays != null)
                    {
                        row.CreateCell(11).SetCellType(CellType.String);

                        if (obj.GDPRemainingDays <= 0)
                        {
                            row.CreateCell(11).SetCellValue(Constant.EXPIRED); // GCN GDP Remaining date
                            row.GetCell(11).CellStyle = style_expired;
                        }
                        else if (obj.GDPRemainingDays > 0 && obj.GDPRemainingDays <= 90)
                        {
                            row.CreateCell(11).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GDPRemainingDays.ToString())); // GCN GDP Remaining date
                            row.GetCell(11).CellStyle = style_warning;
                        }
                    }
                    // GSP
                    if (!string.IsNullOrEmpty(obj.GSPFromDate))
                    {
                        row.CreateCell(12).SetCellValue(new DateTime(int.Parse(obj.GSPFromDate.Substring(6, 4)),
                            int.Parse(obj.GSPFromDate.Substring(3, 2)), int.Parse(obj.GSPFromDate.Substring(0, 2))));
                        row.GetCell(12).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue(obj.GSPFromDate);
                        row.GetCell(12).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GSPToDate))
                    {
                        row.CreateCell(13).SetCellValue(new DateTime(int.Parse(obj.GSPToDate.Substring(6, 4)),
                            int.Parse(obj.GSPToDate.Substring(3, 2)), int.Parse(obj.GSPToDate.Substring(0, 2))));
                        row.GetCell(13).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(13).SetCellValue(obj.GSPToDate);
                        row.GetCell(13).CellStyle = cellStyle;
                    }
                    if (obj.GSPRemainingDays != null)
                    {
                        if (obj.GSPRemainingDays <= 0)
                        {
                            row.CreateCell(14).SetCellValue(Constant.EXPIRED); // GCN GSP Remaining date
                            row.GetCell(14).CellStyle = style_expired;
                        }
                        else if (obj.GSPRemainingDays > 0 && obj.GSPRemainingDays <= 90)
                        {
                            row.CreateCell(14).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GSPRemainingDays));
                            row.GetCell(14).CellStyle = style_warning;
                        }
                    }

                    row.CreateCell(15).SetCellValue(obj.TaxCode); // Tax code
                    if (obj.GPKD != null && obj.GPKD == true)
                    {
                        row.CreateCell(16).SetCellValue("X"); // GPKD
                        row.GetCell(16).CellStyle = style_x;
                    }
                    if (obj.NNKDMBTTBYT != null)
                    {
                        if (obj.NNKDMBTTBYT == true)
                        {
                            row.CreateCell(17).SetCellValue("YES"); // Ngành nghề kinh doanh có mua bán TTBYT (COE)
                            row.GetCell(17).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(17).SetCellValue("NO"); // Ngành nghề kinh doanh có mua bán TTBYT (COE)
                            row.GetCell(17).CellStyle = style_red;
                        }                        
                    }
                    row.CreateCell(18).SetCellValue(obj.GPKDNumber); // So GPKD
                    row.CreateCell(19).SetCellValue(obj.GPKDName); // GPKD Name
                    if (obj.IsCOETTBYT != null)
                    {
                        if (obj.IsCOETTBYT == true)
                        {
                            row.CreateCell(20).SetCellValue("YES"); // Công bố đủ điều kiện mua bán TTBYT (COE)
                            row.GetCell(20).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(20).SetCellValue("NO"); // Công bố đủ điều kiện mua bán TTBYT (COE)
                            row.GetCell(20).CellStyle = style_red;
                        }
                    }
                    // GCN đủ điều kiện kinh doanh dược
                    if (!string.IsNullOrEmpty(obj.GCNDDKKDDFromDate))
                    {
                        row.CreateCell(21).SetCellValue(new DateTime(int.Parse(obj.GCNDDKKDDFromDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKKDDFromDate.Substring(3, 2)), int.Parse(obj.GCNDDKKDDFromDate.Substring(0, 2))));
                        row.GetCell(21).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(21).SetCellValue(obj.GCNDDKKDDFromDate);
                        row.GetCell(21).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GCNDDKKDDToDate))
                    {
                        row.CreateCell(22).SetCellValue(new DateTime(int.Parse(obj.GCNDDKKDDToDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKKDDToDate.Substring(3, 2)), int.Parse(obj.GCNDDKKDDToDate.Substring(0, 2))));
                        row.GetCell(22).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(22).SetCellValue(obj.GCNDDKKDDToDate);
                        row.GetCell(22).CellStyle = cellStyle;
                    }
                    if (obj.GCNDDKKDDRemainingDays != null)
                    {
                        row.CreateCell(23).SetCellType(CellType.String);
                                                
                        if (obj.GCNDDKKDDRemainingDays <= 0)
                        {
                            row.CreateCell(23).SetCellValue(Constant.EXPIRED); // GCN DDKKDD Remaining date
                            row.GetCell(23).CellStyle = style_expired;
                        }
                        else if (obj.GCNDDKKDDRemainingDays > 0 && obj.GCNDDKKDDRemainingDays <= 90)
                        {
                            row.CreateCell(23).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GCNDDKKDDRemainingDays.ToString())); // GCN DDKKDD Remaining date
                            row.GetCell(23).CellStyle = style_warning;
                        }
                    }
                    // PHAM VI KINH DOANH
                    if (obj.TDPHCCDCGN != null)
                    {
                        if (obj.TDPHCCDCGN == true)
                        {
                            row.CreateCell(24).SetCellValue("YES"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(24).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(24).SetCellValue("NO"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(24).CellStyle = style_red;
                        }
                    }
                    if (obj.TDNLDLT != null)
                    {
                        if (obj.TDNLDLT == true)
                        {
                            row.CreateCell(25).SetCellValue("YES"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(25).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(25).SetCellValue("NO"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(25).CellStyle = style_red;
                        }
                    }

                    if (obj.TDDMCSD != null)
                    {
                        if (obj.TDDMCSD == true)
                        {
                            row.CreateCell(26).SetCellValue("YES"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(26).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(26).SetCellValue("NO"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(26).CellStyle = style_red;
                        }
                    }
                    if (obj.SinhPham != null)
                    {
                        if (obj.SinhPham == true)
                        {
                            row.CreateCell(27).SetCellValue("YES"); // Sinh phẩm
                            row.GetCell(27).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(27).SetCellValue("NO"); // Sinh phẩm
                            row.GetCell(27).CellStyle = style_red;
                        }
                    }
                    if (obj.Vaccin != null)
                    {
                        if (obj.Vaccin == true)
                        {
                            row.CreateCell(28).SetCellValue("YES"); // Vaccine
                            row.GetCell(28).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(28).SetCellValue("NO"); // Vaccine
                            row.GetCell(28).CellStyle = style_red;
                        }
                    }
                    if (obj.RX != null)
                    {
                        if (obj.RX == true)
                        {
                            row.CreateCell(29).SetCellValue("YES"); // RX
                            row.GetCell(29).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(29).SetCellValue("NO"); // RX
                            row.GetCell(29).CellStyle = style_red;
                        }
                    }
                    if (obj.OTC != null)
                    {
                        if (obj.OTC == true)
                        {
                            row.CreateCell(30).SetCellValue("YES"); // OTC
                            row.GetCell(30).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(30).SetCellValue("NO"); // OTC
                            row.GetCell(30).CellStyle = style_red;
                        }
                    }
                    if (obj.EssentialDrugs != null)
                    {
                        if (obj.EssentialDrugs == true)
                        {
                            row.CreateCell(31).SetCellValue("YES"); // Essential Drugs
                            row.GetCell(31).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(31).SetCellValue("NO"); // Essential Drugs
                            row.GetCell(31).CellStyle = style_red;
                        }
                    }
                    // DU DIEU KIEN XUAT BAN
                    if (obj.DDKXB_TDPHCCDCGN != null)
                    {
                        if (obj.DDKXB_TDPHCCDCGN == true)
                        {
                            row.CreateCell(32).SetCellValue("YES"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(32).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(32).SetCellValue("NO"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(32).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_TDNLDLT != null)
                    {
                        if (obj.DDKXB_TDNLDLT == true)
                        {
                            row.CreateCell(33).SetCellValue("YES"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(33).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(33).SetCellValue("NO"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(33).CellStyle = style_red;
                        }
                    }

                    if (obj.DDKXB_TDDMCSD != null)
                    {
                        if (obj.DDKXB_TDDMCSD == true)
                        {
                            row.CreateCell(34).SetCellValue("YES"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(34).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(34).SetCellValue("NO"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(34).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_SinhPham != null)
                    {
                        if (obj.DDKXB_SinhPham == true)
                        {
                            row.CreateCell(35).SetCellValue("YES"); // Sinh phẩm
                            row.GetCell(35).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(35).SetCellValue("NO"); // Sinh phẩm
                            row.GetCell(35).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_Vaccin != null)
                    {
                        if (obj.DDKXB_Vaccin == true)
                        {
                            row.CreateCell(36).SetCellValue("YES"); // Vaccine
                            row.GetCell(36).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(36).SetCellValue("NO"); // Vaccine
                            row.GetCell(36).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_RX != null)
                    {
                        if (obj.DDKXB_RX == true)
                        {
                            row.CreateCell(37).SetCellValue("YES"); // RX
                            row.GetCell(37).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(37).SetCellValue("NO"); // RX
                            row.GetCell(37).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_OTC != null)
                    {
                        if (obj.DDKXB_OTC == true)
                        {
                            row.CreateCell(38).SetCellValue("YES"); // OTC
                            row.GetCell(38).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(38).SetCellValue("NO"); // OTC
                            row.GetCell(38).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_EssentialDrugs != null)
                    {
                        if (obj.DDKXB_EssentialDrugs == true)
                        {
                            row.CreateCell(39).SetCellValue("YES"); // EssentialDrugs
                            row.GetCell(39).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(39).SetCellValue("NO"); // EssentialDrugs
                            row.GetCell(39).CellStyle = style_red;
                        }
                    }
                    // KCB
                    if (!string.IsNullOrEmpty(obj.KCBFromDate))
                    {
                        row.CreateCell(40).SetCellValue(new DateTime(int.Parse(obj.KCBFromDate.Substring(6, 4)),
                            int.Parse(obj.KCBFromDate.Substring(3, 2)), int.Parse(obj.KCBFromDate.Substring(0, 2))));
                        row.GetCell(40).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(40).SetCellValue(obj.KCBFromDate);
                        row.GetCell(40).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.KCBToDate))
                    {
                        row.CreateCell(41).SetCellValue(new DateTime(int.Parse(obj.KCBToDate.Substring(6, 4)),
                            int.Parse(obj.KCBToDate.Substring(3, 2)), int.Parse(obj.KCBToDate.Substring(0, 2))));
                        row.GetCell(41).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(41).SetCellValue(obj.KCBToDate);
                        row.GetCell(41).CellStyle = cellStyle;
                    }
                    if (obj.KCBRemainingDays != null)
                    {                                                
                        if (obj.KCBRemainingDays <= 0)
                        {
                            row.CreateCell(42).SetCellValue(Constant.EXPIRED); // GCN KCB Remaining date 
                            row.GetCell(42).CellStyle = style_expired;
                        }
                        else if (obj.KCBRemainingDays > 0 && obj.KCBRemainingDays <= 90)
                        {
                            row.CreateCell(42).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.KCBRemainingDays)); // GCN KCB Remaining date 
                            row.GetCell(42).CellStyle = style_warning;
                        }
                    }
                    // Danh mục kỹ thuật(có tiêm Botox/ Botulinum)
                    if (obj.DMKTCTBotoxBotulinum != null && obj.DMKTCTBotoxBotulinum == true)
                    {
                        row.CreateCell(43).SetCellValue("X");
                        row.GetCell(43).CellStyle = style_x;
                    }
                    // Đủ điều kiện mua Botox/ Xeomin
                    if (obj.DDKM_BotoxXeomin != null)
                    {
                        if (obj.DDKM_BotoxXeomin == true)
                        {
                            row.CreateCell(44).SetCellValue("YES");
                            row.GetCell(44).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(44).SetCellValue("NO");
                            row.GetCell(44).CellStyle = style_red;
                        }
                    }
                    // Giấy chứng nhận đủ điều kiện tiêm chủng
                    if (!string.IsNullOrEmpty(obj.GCNDDKTCFromDate))
                    {
                        row.CreateCell(45).SetCellValue(new DateTime(int.Parse(obj.GCNDDKTCFromDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKTCFromDate.Substring(3, 2)), int.Parse(obj.GCNDDKTCFromDate.Substring(0, 2))));
                        row.GetCell(45).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(45).SetCellValue(obj.GCNDDKTCFromDate);
                        row.GetCell(45).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GCNDDKTCToDate))
                    {
                        row.CreateCell(46).SetCellValue(new DateTime(int.Parse(obj.GCNDDKTCToDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKTCToDate.Substring(3, 2)), int.Parse(obj.GCNDDKTCToDate.Substring(0, 2))));
                        row.GetCell(46).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(46).SetCellValue(obj.GCNDDKTCToDate);
                        row.GetCell(46).CellStyle = cellStyle;
                    }
                    if (obj.GCNDDKTCRemainingDays != null)
                    {
                        if (obj.GCNDDKTCRemainingDays <= 0)
                        {
                            row.CreateCell(47).SetCellValue(Constant.EXPIRED); // GCNDDKTC Remaining date 
                            row.GetCell(47).CellStyle = style_expired;
                        }
                        else if (obj.GCNDDKTCRemainingDays > 0 && obj.GCNDDKTCRemainingDays <= 90)
                        {
                            row.CreateCell(47).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GCNDDKTCRemainingDays)); // GCNDDKTC Remaining date 
                            row.GetCell(47).CellStyle = style_warning;
                        }
                    }
                    // CUSTOMER INFORMATION
                    row.CreateCell(48).SetCellValue(obj.Remark);
                    row.CreateCell(49).SetCellValue(obj.Address);
                    row.CreateCell(50).SetCellValue(obj.District);
                    row.CreateCell(51).SetCellValue(obj.Province);
                    row.CreateCell(52).SetCellValue(obj.Region);
                    row.CreateCell(53).SetCellValue(obj.CGrpCode);
                    row.CreateCell(54).SetCellValue(obj.CGrpName);
                    row.CreateCell(55).SetCellValue(obj.Tel);
                    row.CreateCell(56).SetCellValue(obj.Channel);
                    if (obj.IsManual != null)
                    {
                        if (obj.IsManual == true)
                        {
                            row.CreateCell(57).SetCellValue("X");
                            row.GetCell(57).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(57).SetCellValue("");
                        }
                    }
                    // Status licenses
                    if (obj.LicensesYESNO != null && obj.LicensesYESNO == 1)
                    {
                        row.CreateCell(58).SetCellValue(Constant.FULL_LICENSE);
                        row.GetCell(58).CellStyle = style_x;
                    }
                    else if (obj.LicensesYESNO != null && obj.LicensesYESNO == 2)
                    {
                        row.CreateCell(58).SetCellValue(Constant.NO_CONTROL_LICENSE);
                        row.GetCell(58).CellStyle = style_info;
                    }
                    else
                    {
                        row.CreateCell(58).SetCellValue(Constant.LACK_OF_LICENSE);
                        row.GetCell(58).CellStyle = style_warning;
                    }
                    // Licenses Type Missing
                    string licenseTypeMissing = string.Empty;
                    if (!string.IsNullOrEmpty(obj.LicenseGPP))
                    {
                        licenseTypeMissing += obj.LicenseGPP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseGDP))
                    {
                        licenseTypeMissing += obj.LicenseGDP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseTaxCode))
                    {
                        licenseTypeMissing += obj.LicenseTaxCode + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseGPKD))
                    {
                        licenseTypeMissing += obj.LicenseGPKD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseCOETTBYT))
                    {
                        licenseTypeMissing += obj.LicenseCOETTBYT + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseGCNDDKKDD))
                    {
                        licenseTypeMissing += obj.LicenseGCNDDKKDD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseKCB))
                    {
                        licenseTypeMissing += obj.LicenseKCB + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseDMKTBotoxXeomin))
                    {
                        licenseTypeMissing += obj.LicenseDMKTBotoxXeomin;
                    }
                    row.CreateCell(59).SetCellValue(licenseTypeMissing);
                    row.GetCell(59).CellStyle = style_red;
                    row.CreateCell(60).SetCellValue(obj.ProvinceByUser);
                    
                    // EXCLUDED Thuoc gay nghien
                    if (obj.Excluded_TDPHCCDCGN != null)
                    {
                        if (obj.Excluded_TDPHCCDCGN == true)
                        {
                            row.CreateCell(61).SetCellValue("X");
                            row.GetCell(61).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(61).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc doc
                    if (obj.Excluded_TDNLDLT != null)
                    {
                        if (obj.Excluded_TDNLDLT == true)
                        {
                            row.CreateCell(62).SetCellValue("X");
                            row.GetCell(62).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(62).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc cam
                    if (obj.Excluded_TDDMCSD != null)
                    {
                        if (obj.Excluded_TDDMCSD == true)
                        {
                            row.CreateCell(63).SetCellValue("X");
                            row.GetCell(63).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(63).SetCellValue("");
                        }
                    }
                    // Remark thuoc kiem soat dac biet
                    row.CreateCell(64).SetCellValue(obj.RemarkKSDB);

                    // EXCLUDED Thuoc gay nghien
                    if (obj.ExcludedGPP_TDPHCCDCGN != null)
                    {
                        if (obj.ExcludedGPP_TDPHCCDCGN == true)
                        {
                            row.CreateCell(65).SetCellValue("X");
                            row.GetCell(65).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(65).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc doc
                    if (obj.ExcludedGPP_TDNLDLT != null)
                    {
                        if (obj.ExcludedGPP_TDNLDLT == true)
                        {
                            row.CreateCell(66).SetCellValue("X");
                            row.GetCell(66).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(66).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc cam
                    if (obj.ExcludedGPP_TDDMCSD != null)
                    {
                        if (obj.ExcludedGPP_TDDMCSD == true)
                        {
                            row.CreateCell(67).SetCellValue("X");
                            row.GetCell(67).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(67).SetCellValue("");
                        }
                    }
                    // Remark thuoc kiem soat dac biet
                    row.CreateCell(68).SetCellValue(obj.RemarkGPP_KSDB);
                    // insert picture license
                    //IDrawing drawing = sheet.CreateDrawingPatriarch();
                    //IClientAnchor anchor = helper.CreateClientAnchor();
                    //anchor.Col1 = 60;//0 index based column
                    //anchor.Row1 = rowIdx;//0 index based row
                    //IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
                    //picture.Resize(2,2);
                    // licenses location
                    //licensePath = Constant.CHUNG_TU_KHACH_HANG + obj.CustCode + " - " + obj.CustName;
                    //FileInfo myFile = new FileInfo(licensePath);
                    //row.CreateCell(60).CellFormula = licensePath;
                    //IFont link_font = workbook.CreateFont();
                    //link_font.Underline = FontUnderlineType.Single;
                    //cellLink.SetFont(link_font);
                    //link_font.Color = IndexedColors.BlueGrey.Index;                    
                    //row.GetCell(60).CellStyle = cellLink;
                    rowIdx += 1;
                }
                sheet.AutoSizeColumn(3);      
                file.Close();
                return workbook;
            }
        }


        /// <summary>
        /// Process Customer
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto ProcessCustomer(CDIDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            result.number = cdiDao.ProcessCustomer(dto);
            if (result.number > 0)
            {
                result.code = Constant.SUCCESS;
                result.message = Constant.PROCESS_CUSTOMER;
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.PROCESS_CUSTOMER_ERROR;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Certification History
        /// </summary>
        /// <param name="jsonPr"></param>
        /// <returns></returns>
        public IEnumerable<CertificationHistoryDto> CertificationHistory(CDIDto jsonPr)
        {
            IEnumerable<CertificationHistoryDto> historyList = cdiDao.CertificationHistory(jsonPr);
            return historyList;
            throw new NotImplementedException();
        }

        /// <summary>
        /// CheckCustCodeExist
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto CheckCustCodeExist(CDIDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            int count = cdiDao.checkCustomerExist(json);
            if (count > 0)
            {
                result.code = Constant.EXISTED;
                result.message = String.Format(Constant.CUSTOMER_CODE_EXIST, json.CustCode);
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public IWorkbook DailyReport(CDIDto js)
        {
            int process = cdiDao.ProcessCustomer(js);
            IWorkbook workbook = DDKXBDaily(js);
            return workbook;
        }

        /// <summary>
        /// Report Đủ điều kiện xuất bán một số loại thuốc
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private IWorkbook DDKXBDaily(CDIDto obj)
        {
            IEnumerable<CDIDto> allList = ReportDailyDDKXB(obj);
            IWorkbook wb = ExportExcelBySearch(allList, obj.SheetName, obj.FileTemplate);
            return wb;
        }

        /// <summary>
        /// Get list KSDB report daily
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private IEnumerable<CDIDto> ReportDailyDDKXB(CDIDto obj)
        {
            if (!string.IsNullOrEmpty(obj.DDKXB))
            {
                List<string> listDDKXB = obj.DDKXB.Split(',').ToList();
                foreach (string str in listDDKXB)
                {
                    if (str.Equals(Constant.TDPHCCDCGN))
                    {
                        obj.TDPHCCDCGN_Search = str;
                    }
                    if (str.Equals(Constant.TDNLDLT))
                    {
                        obj.TDNLDLT_Search = str;
                    }
                    if (str.Equals(Constant.TDDMCSD))
                    {
                        obj.TDDMCSD_Search = str;
                    }
                    if (str.Equals(Constant.SINHPHAM))
                    {
                        obj.SinhPham_Search = str;
                    }
                    if (str.Equals(Constant.VACCIN))
                    {
                        obj.Vaccin_Search = str;
                    }
                    if (str.Equals(Constant.BOTOXXEOMIN))
                    {
                        obj.BotoxXeomin_Search = str;
                    }
                }
            }
            /* GET LIST CUSTOMER */
            IEnumerable<CDIDto> rs = cdiDao.DDKXBDaily(obj);
            IEnumerable<CDIDto> lstNewCustomer = cdiDao.SelectCustomerNOTINDDKXB(obj);
            IEnumerable<CDIDto> allList = rs.Union(lstNewCustomer);
            return allList;
        }
        /// <summary>
        /// Required Licences
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CDIDto> RequiredLicences(CDIDto js)
        {
            IEnumerable<CDIDto> result = cdiDao.RequiredLicences(js);
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Export Required Licenses
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public IWorkbook ExportRequiredLicenses(CDIDto js)
        {
            IEnumerable<CDIDto> requiredLicensesList = cdiDao.RequiredLicences(js);
            // Export data
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + Constant.REQUIRED_LICENSES;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY);
                var color_expired = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_warning = new XSSFColor(new byte[] { 255, 153, 0 });
                var color_x = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_info = new XSSFColor(new byte[] { 24, 213, 247 });
                var color_red = new XSSFColor(new byte[] { 255, 0, 0 });
                var font_expired = (XSSFFont)workbook.CreateFont();
                var font_warning = (XSSFFont)workbook.CreateFont();
                var font_x = (XSSFFont)workbook.CreateFont();
                var font_red = (XSSFFont)workbook.CreateFont();
                var font_info = (XSSFFont)workbook.CreateFont();
                font_expired.SetColor(color_expired);
                font_warning.SetColor(color_warning);
                font_x.SetColor(color_x);
                font_red.SetColor(color_red);
                font_expired.IsBold = true;
                font_warning.IsBold = true;
                font_x.IsBold = true;
                font_red.IsBold = true;
                font_info.SetColor(color_info);
                font_info.IsBold = true;
                // Expired
                var style_expired = (XSSFCellStyle)workbook.CreateCellStyle();
                style_expired.SetFont(font_expired);
                // Coming expire
                var style_warning = (XSSFCellStyle)workbook.CreateCellStyle();
                style_warning.SetFont(font_warning);
                // 
                var style_x = (XSSFCellStyle)workbook.CreateCellStyle();
                style_x.SetFont(font_x);
                //
                var style_red = (XSSFCellStyle)workbook.CreateCellStyle();
                style_red.SetFont(font_red);
                var style_info = (XSSFCellStyle)workbook.CreateCellStyle();
                style_info.SetFont(font_info);
                ISheet sheet = workbook.GetSheet("RequiredLicenses");
                int rowIdx = 2;
                foreach (CDIDto item in requiredLicensesList)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.CustCode); // CustCode
                    row.CreateCell(1).SetCellValue(item.CustName); // CustName
                    // Status licenses
                    if (item.LicensesYESNO != null && item.LicensesYESNO == 1)
                    {
                        row.CreateCell(2).SetCellValue(Constant.FULL_LICENSE);
                        row.GetCell(2).CellStyle = style_x;
                    }
                    else if (item.LicensesYESNO != null && item.LicensesYESNO == 2)
                    {
                        row.CreateCell(2).SetCellValue(Constant.NO_CONTROL_LICENSE);
                        row.GetCell(2).CellStyle = style_info;
                    }
                    else
                    {
                        row.CreateCell(2).SetCellValue(Constant.LACK_OF_LICENSE);
                        row.GetCell(2).CellStyle = style_warning;
                    }
                    // Licenses Type Missing
                    string licenseTypeMissing = string.Empty;
                    if (!string.IsNullOrEmpty(item.LicenseGPP))
                    {
                        licenseTypeMissing += item.LicenseGPP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseGDP))
                    {
                        licenseTypeMissing += item.LicenseGDP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseTaxCode))
                    {
                        licenseTypeMissing += item.LicenseTaxCode + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseGPKD))
                    {
                        licenseTypeMissing += item.LicenseGPKD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseCOETTBYT))
                    {
                        licenseTypeMissing += item.LicenseCOETTBYT + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseGCNDDKKDD))
                    {
                        licenseTypeMissing += item.LicenseGCNDDKKDD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseKCB))
                    {
                        licenseTypeMissing += item.LicenseKCB + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseDMKTBotoxXeomin))
                    {
                        licenseTypeMissing += item.LicenseDMKTBotoxXeomin;
                    }
                    row.CreateCell(3).SetCellValue(licenseTypeMissing); // License type missing
                    row.GetCell(3).CellStyle = style_red;
                    // GPP
                    if (!string.IsNullOrEmpty(item.GPPFromDate))
                    {
                        row.CreateCell(4).SetCellValue(new DateTime(int.Parse(item.GPPFromDate.Substring(6, 4)),
                            int.Parse(item.GPPFromDate.Substring(3, 2)), int.Parse(item.GPPFromDate.Substring(0, 2))));
                        row.GetCell(4).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(4).SetCellValue(item.GPPFromDate);
                    }
                    if (!string.IsNullOrEmpty(item.GPPToDate))
                    {
                        row.CreateCell(5).SetCellValue(new DateTime(int.Parse(item.GPPToDate.Substring(6, 4)),
                            int.Parse(item.GPPToDate.Substring(3, 2)), int.Parse(item.GPPToDate.Substring(0, 2))));
                        row.GetCell(5).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(5).SetCellValue(item.GPPToDate);
                    }
                    if (item.GPPRemainingDays != null)
                    {
                        row.CreateCell(6).SetCellType(CellType.String);
                        if (item.GPPRemainingDays <= 0)
                        {
                            row.CreateCell(6).SetCellValue(Constant.EXPIRED); // GPP Remaining date
                            row.GetCell(6).CellStyle = style_expired;
                        }
                        else if (item.GPPRemainingDays > 0 && item.GPPRemainingDays <= 90)
                        {
                            row.CreateCell(6).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.GPPRemainingDays.ToString())); // GPP Remaining date
                            row.GetCell(6).CellStyle = style_warning;
                        }
                    }

                    // GDP
                    if (!string.IsNullOrEmpty(item.GDPFromDate))
                    {
                        row.CreateCell(7).SetCellValue(new DateTime(int.Parse(item.GDPFromDate.Substring(6, 4)),
                            int.Parse(item.GDPFromDate.Substring(3, 2)), int.Parse(item.GDPFromDate.Substring(0, 2))));
                        row.GetCell(7).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(item.GDPFromDate);
                        row.GetCell(7).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(item.GDPToDate))
                    {
                        row.CreateCell(8).SetCellValue(new DateTime(int.Parse(item.GDPToDate.Substring(6, 4)),
                            int.Parse(item.GDPToDate.Substring(3, 2)), int.Parse(item.GDPToDate.Substring(0, 2))));
                        row.GetCell(8).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(8).SetCellValue(item.GDPToDate);
                        row.GetCell(8).CellStyle = cellStyle;
                    }
                    if (item.GDPRemainingDays != null)
                    {
                        row.CreateCell(9).SetCellType(CellType.String);

                        if (item.GDPRemainingDays <= 0)
                        {
                            row.CreateCell(9).SetCellValue(Constant.EXPIRED); // GCN GDP Remaining date
                            row.GetCell(9).CellStyle = style_expired;
                        }
                        else if (item.GDPRemainingDays > 0 && item.GDPRemainingDays <= 90)
                        {
                            row.CreateCell(9).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.GDPRemainingDays.ToString())); // GCN GDP Remaining date
                            row.GetCell(9).CellStyle = style_warning;
                        }
                    }

                    // GSP
                    if (!string.IsNullOrEmpty(item.GSPFromDate))
                    {
                        row.CreateCell(10).SetCellValue(new DateTime(int.Parse(item.GSPFromDate.Substring(6, 4)),
                            int.Parse(item.GSPFromDate.Substring(3, 2)), int.Parse(item.GSPFromDate.Substring(0, 2))));
                        row.GetCell(10).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(10).SetCellValue(item.GSPFromDate);
                        row.GetCell(10).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(item.GSPToDate))
                    {
                        row.CreateCell(11).SetCellValue(new DateTime(int.Parse(item.GSPToDate.Substring(6, 4)),
                            int.Parse(item.GSPToDate.Substring(3, 2)), int.Parse(item.GSPToDate.Substring(0, 2))));
                        row.GetCell(11).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(11).SetCellValue(item.GSPToDate);
                        row.GetCell(11).CellStyle = cellStyle;
                    }
                    if (item.GSPRemainingDays != null)
                    {
                        if (item.GSPRemainingDays <= 0)
                        {
                            row.CreateCell(12).SetCellValue(Constant.EXPIRED); // GSP Remaining date
                            row.GetCell(12).CellStyle = style_expired;
                        }
                        else if (item.GSPRemainingDays > 0 && item.GSPRemainingDays <= 90)
                        {
                            row.CreateCell(12).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.GSPRemainingDays));
                            row.GetCell(12).CellStyle = style_warning;
                        }
                    }
                    row.CreateCell(13).SetCellValue(item.TaxCode); // Tax code
                    // GPKD
                    if (item.GPKD != null && item.GPKD == true)
                    {
                        row.CreateCell(14).SetCellValue("X");
                        row.GetCell(14).CellStyle = style_x;
                    }
                    // Ngành nghề kinh doanh có mua bán TTBYT(COE)
                    if (item.NNKDMBTTBYT != null)
                    {
                        if (item.NNKDMBTTBYT == true)
                        {
                            row.CreateCell(15).SetCellValue("YES");
                            row.GetCell(15).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(15).SetCellValue("NO");
                            row.GetCell(15).CellStyle = style_red;
                        }
                    }
                    // Công bố đủ điều kiện mua bán TTBYT (COE)
                    if (item.IsCOETTBYT != null)
                    {
                        if (item.IsCOETTBYT == true)
                        {
                            row.CreateCell(16).SetCellValue("YES"); 
                            row.GetCell(16).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(16).SetCellValue("NO");
                            row.GetCell(16).CellStyle = style_red;
                        }
                    }
                    // GCN đủ điều kiện kinh doanh dược
                    if (!string.IsNullOrEmpty(item.GCNDDKKDDFromDate))
                    {
                        row.CreateCell(17).SetCellValue(new DateTime(int.Parse(item.GCNDDKKDDFromDate.Substring(6, 4)),
                            int.Parse(item.GCNDDKKDDFromDate.Substring(3, 2)), int.Parse(item.GCNDDKKDDFromDate.Substring(0, 2))));
                        row.GetCell(17).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(17).SetCellValue(item.GCNDDKKDDFromDate);
                        row.GetCell(17).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(item.GCNDDKKDDToDate))
                    {
                        row.CreateCell(18).SetCellValue(new DateTime(int.Parse(item.GCNDDKKDDToDate.Substring(6, 4)),
                            int.Parse(item.GCNDDKKDDToDate.Substring(3, 2)), int.Parse(item.GCNDDKKDDToDate.Substring(0, 2))));
                        row.GetCell(18).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(18).SetCellValue(item.GCNDDKKDDToDate);
                        row.GetCell(18).CellStyle = cellStyle;
                    }
                    if (item.GCNDDKKDDRemainingDays != null)
                    {
                        row.CreateCell(19).SetCellType(CellType.String);

                        if (item.GCNDDKKDDRemainingDays <= 0)
                        {
                            row.CreateCell(19).SetCellValue(Constant.EXPIRED); // GCN DDKKDD Remaining date
                            row.GetCell(19).CellStyle = style_expired;
                        }
                        else if (item.GCNDDKKDDRemainingDays > 0 && item.GCNDDKKDDRemainingDays <= 90)
                        {
                            row.CreateCell(19).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.GCNDDKKDDRemainingDays.ToString())); // GCN DDKKDD Remaining date
                            row.GetCell(19).CellStyle = style_warning;
                        }
                    }

                    // KCB
                    if (!string.IsNullOrEmpty(item.KCBFromDate))
                    {
                        row.CreateCell(20).SetCellValue(new DateTime(int.Parse(item.KCBFromDate.Substring(6, 4)),
                            int.Parse(item.KCBFromDate.Substring(3, 2)), int.Parse(item.KCBFromDate.Substring(0, 2))));
                        row.GetCell(20).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(20).SetCellValue(item.KCBFromDate);
                        row.GetCell(20).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(item.KCBToDate))
                    {
                        row.CreateCell(21).SetCellValue(new DateTime(int.Parse(item.KCBToDate.Substring(6, 4)),
                            int.Parse(item.KCBToDate.Substring(3, 2)), int.Parse(item.KCBToDate.Substring(0, 2))));
                        row.GetCell(21).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(21).SetCellValue(item.KCBToDate);
                        row.GetCell(21).CellStyle = cellStyle;
                    }
                    if (item.KCBRemainingDays != null)
                    {
                        if (item.KCBRemainingDays <= 0)
                        {
                            row.CreateCell(22).SetCellValue(Constant.EXPIRED); // GCN KCB Remaining date 
                            row.GetCell(22).CellStyle = style_expired;
                        }
                        else if (item.KCBRemainingDays > 0 && item.KCBRemainingDays <= 90)
                        {
                            row.CreateCell(22).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.KCBRemainingDays)); // GCN KCB Remaining date 
                            row.GetCell(22).CellStyle = style_warning;
                        }
                    }
                    // Danh mục kỹ thuật(có tiêm Botox | Xeomin (Botulinum))
                    if (item.DMKTCTBotoxBotulinum != null && item.DMKTCTBotoxBotulinum == true)
                    {
                        row.CreateCell(23).SetCellValue("X");
                        row.GetCell(23).CellStyle = style_x;
                    }

                    // PHAM VI KINH DOANH
                    if (item.TDPHCCDCGN != null)
                    {
                        if (item.TDPHCCDCGN == true)
                        {
                            row.CreateCell(24).SetCellValue("YES"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(24).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(24).SetCellValue("NO"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(24).CellStyle = style_red;
                        }
                    }
                    if (item.TDNLDLT != null)
                    {
                        if (item.TDNLDLT == true)
                        {
                            row.CreateCell(25).SetCellValue("YES"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(25).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(25).SetCellValue("NO"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(25).CellStyle = style_red;
                        }
                    }

                    if (item.TDDMCSD != null)
                    {
                        if (item.TDDMCSD == true)
                        {
                            row.CreateCell(26).SetCellValue("YES"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(26).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(26).SetCellValue("NO"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(26).CellStyle = style_red;
                        }
                    }
                    if (item.SinhPham != null)
                    {
                        if (item.SinhPham == true)
                        {
                            row.CreateCell(27).SetCellValue("YES"); // Sinh phẩm
                            row.GetCell(27).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(27).SetCellValue("NO"); // Sinh phẩm
                            row.GetCell(27).CellStyle = style_red;
                        }
                    }
                    if (item.Vaccin != null)
                    {
                        if (item.Vaccin == true)
                        {
                            row.CreateCell(28).SetCellValue("YES"); // Vaccine
                            row.GetCell(28).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(28).SetCellValue("NO"); // Vaccine
                            row.GetCell(28).CellStyle = style_red;
                        }
                    }
                    // DU DIEU KIEN XUAT BAN
                    if (item.DDKXB_TDPHCCDCGN != null)
                    {
                        if (item.DDKXB_TDPHCCDCGN == true)
                        {
                            row.CreateCell(29).SetCellValue("YES"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(29).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(29).SetCellValue("NO"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(29).CellStyle = style_red;
                        }
                    }
                    if (item.DDKXB_TDNLDLT != null)
                    {
                        if (item.DDKXB_TDNLDLT == true)
                        {
                            row.CreateCell(30).SetCellValue("YES"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(30).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(30).SetCellValue("NO"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(30).CellStyle = style_red;
                        }
                    }

                    if (item.DDKXB_TDDMCSD != null)
                    {
                        if (item.DDKXB_TDDMCSD == true)
                        {
                            row.CreateCell(31).SetCellValue("YES"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(31).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(31).SetCellValue("NO"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(31).CellStyle = style_red;
                        }
                    }
                    if (item.DDKXB_SinhPham != null)
                    {
                        if (item.DDKXB_SinhPham == true)
                        {
                            row.CreateCell(32).SetCellValue("YES"); // Sinh phẩm
                            row.GetCell(32).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(32).SetCellValue("NO"); // Sinh phẩm
                            row.GetCell(32).CellStyle = style_red;
                        }
                    }
                    if (item.DDKXB_Vaccin != null)
                    {
                        if (item.DDKXB_Vaccin == true)
                        {
                            row.CreateCell(33).SetCellValue("YES"); // Vaccine
                            row.GetCell(33).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(33).SetCellValue("NO"); // Vaccine
                            row.GetCell(33).CellStyle = style_red;
                        }
                    }
                    // IsManual
                    if (item.IsManual != null)
                    {
                        if (item.IsManual == true)
                        {
                            row.CreateCell(34).SetCellValue("X");
                            row.GetCell(34).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(34).SetCellValue("");
                        }
                    }

                    // CUSTOMER INFORMATION
                    row.CreateCell(35).SetCellValue(item.Remark);
                    row.CreateCell(36).SetCellValue(item.Address);
                    row.CreateCell(37).SetCellValue(item.District);
                    row.CreateCell(38).SetCellValue(item.Province);
                    row.CreateCell(39).SetCellValue(item.Region);
                    row.CreateCell(40).SetCellValue(item.Channel);
                    row.CreateCell(41).SetCellValue(item.CGrpCode);
                    rowIdx += 1;
                }
                file.Close();
                return workbook;
            }
        }

        /// <summary>
        /// Backup Data
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto BackupData(CDIDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                string[] param = new string[2];
                param[0] = dto.UserId;
                param[1] = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                string title = Constant.BACKUP_DATA_DAILY_REPORT_TITLE;
                string content = string.Format(Constant.BACKUP_DATA_DAILY_REPORT_CONTENT, param);
                int cnt = cdiDao.BackUpCDIMngToDDKXBDaily(dto);
                if (cnt > 0)
                {
                    // GET USER CDI TEAM
                    IEnumerable<UserDto> userList = userDao.GetUserByTeamCDI();
                    List<string> userEmail = new List<string>();
                    foreach(UserDto item in userList)
                    {
                        //if (!item.Email.Equals("cam.thimong.tran.dksh@gmail.com") && !item.Email.Equals("hien.ngoc.nguyen@dksh.com")
                        //    && !item.Email.Equals("tran.bao.duong@dksh.com") && !item.Email.Equals("ngocnu.cdi.dksh@gmail.com")
                        //    && !item.Email.Equals("trinh.thihanh.tran@dksh.com") && !item.Email.Equals("tran.ngoc.duong@dksh.com"))
                        if (item.IsUsing == true)
                        {
                            userEmail.Add(item.Email);
                        }
                    }
                    bool flag = Utils.SendMailMultipleAttacment(title, content, Constant.HUYNH_ANH_PHAM_DKSH, userEmail, null);
                    if (flag)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.BACKUP_DDKXB;
                    }
                }
                else
                {
                    result.code = Constant.FAILED;
                    result.message = Constant.BACKUP_FAILED;
                }
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            
            return result;
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// upload customer nha thuoc benh vien cong
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadCustomerNTBVC(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook;
            // delete all customers
            int delCustomer = customerDao.DeleteCustomerHNine();
            MemoryStream mem = new MemoryStream();
            mem.SetLength(aFile.ContentLength);
            aFile.InputStream.Read(mem.GetBuffer(), 0, aFile.ContentLength);
            string extension = Path.GetExtension(aFile.FileName).Replace(".", "");
            if (Constant.EXCEL_XLSX.Equals(extension))
            {
                workbook = new XSSFWorkbook(mem);
            }
            else if (Constant.EXCEL_XLS.Equals(extension))
            {
                workbook = new HSSFWorkbook(mem);
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet = workbook.GetSheet("Customer");
            int rowCount = 0;
            CustomerNTBVCDto customerDto = null;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                customerDto = new CustomerNTBVCDto();
                IRow row = sheet.GetRow(i);
                customerDto.CustCode = Utils.GetFormattedCellValue(row.GetCell(0)); // cust code
                customerDto.CustomerName = Utils.GetFormattedCellValue(row.GetCell(1)); // cust name
                customerDto.Provine = Utils.GetFormattedCellValue(row.GetCell(2)); // provine
                customerDto.Region = Utils.GetFormattedCellValue(row.GetCell(3)); // region
                customerDto.CGrpCode = Utils.GetFormattedCellValue(row.GetCell(4)); // cgrp code
                customerDto.Note = Utils.GetFormattedCellValue(row.GetCell(5)); // note
                int j = customerDao.InsertCustomerNTBVC(customerDto);
                if (j >= 1)
                {
                    rowCount += 1;
                }
            }
            if (rowCount > 0)
            {
                result.code = Constant.SUCCESS;
                result.number = rowCount;
                result.message = string.Format(Constant.PROCESS_MESSAGE, rowCount);
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.MESSAGE_ERROR;
            }
            mem.Close();
            return result;
        }


        /// <summary>
        /// Schedule Timer
        /// </summary>
        public void ScheduleTimer(int hour)
        {
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, hour, 0, 0, 0); //Specify your scheduled time HH,MM,SS [17 and 00 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = Math.Abs((scheduledTime - DateTime.Now).TotalMilliseconds);
            timer = new System.Timers.Timer(tickTime);
            timer.Elapsed += new ElapsedEventHandler(AutoBackUpDailyReport);
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// Auto BackUp Daily Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoBackUpDailyReport(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            int cnt = cdiDao.AutoBackUpDailyReport();
            if (cnt > 0)
            {
                IEnumerable<UserDto> userList = userDao.GetUserByTeamCDI();
                string[] param = new string[2];
                param[0] = "SYSTEM [AUTO BACK UP]";
                param[1] = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                string title = Constant.BACKUP_DATA_DAILY_REPORT_TITLE;
                string content = string.Format(Constant.BACKUP_DATA_DAILY_REPORT_CONTENT, param);
                List<string> ccMail = new List<string>();
                foreach (UserDto item in userList)
                {
                    //if (!item.Email.Equals("cam.thimong.tran.dksh@gmail.com") && !item.Email.Equals("hien.ngoc.nguyen@dksh.com")
                    //    && !item.Email.Equals("tran.bao.duong@dksh.com") && !item.Email.Equals("ngocnu.cdi.dksh@gmail.com")
                    //    && !item.Email.Equals("trinh.thihanh.tran@dksh.com") && !item.Email.Equals("tran.ngoc.duong@dksh.com"))
                    if (item.IsUsing == true)
                    {
                        ccMail.Add(item.Email);
                    }
                }
                bool flag = Utils.SendMailMultipleAttacment(title, content, Constant.HUYNH_ANH_PHAM_DKSH, ccMail, null);
            }
            ScheduleTimer(16); // Begin back up at 16:00:00 PM
        }

        /// <summary>
        /// Setup Timeline
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto SetupTimeline(SetupTimelineDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                dto.Id = Guid.NewGuid();
                dto.TaskName = Constant.TIMELINE;
                dto.IsRun = true;
                result.number = cdiDao.SetupTimeline(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// update code ship to
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultMessageDto UploadCodeShipTo(HttpPostedFileBase aFile, string userId)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook;
            // delete all customers ship to
            int delCustomer = customerDao.DeleteHECShipTo();
            MemoryStream mem = new MemoryStream();
            mem.SetLength(aFile.ContentLength);
            aFile.InputStream.Read(mem.GetBuffer(), 0, aFile.ContentLength);
            string extension = Path.GetExtension(aFile.FileName).Replace(".", "");
            if (Constant.EXCEL_XLSX.Equals(extension))
            {
                workbook = new XSSFWorkbook(mem);
            }
            else if (Constant.EXCEL_XLS.Equals(extension))
            {
                workbook = new HSSFWorkbook(mem);
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet = workbook.GetSheetAt(0);
            int rowCount = 0;
            CustomerShipToDto shipto = null;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                shipto = new CustomerShipToDto();
                IRow row = sheet.GetRow(i);
                shipto.CustCode = Utils.GetFormattedCellValue(row.GetCell(0)); // cust code
                shipto.SalOrg = Utils.GetFormattedCellValue(row.GetCell(1)); // 
                shipto.DC = Utils.GetFormattedCellValue(row.GetCell(2)); // 
                shipto.DV = Utils.GetFormattedCellValue(row.GetCell(3)); // 
                shipto.Name1 = Utils.GetFormattedCellValue(row.GetCell(4)); //
                shipto.ShipTo = Utils.GetFormattedCellValue(row.GetCell(5));
                shipto.ShipToName = Utils.GetFormattedCellValue(row.GetCell(6));
                shipto.CreateBy = userId;
                shipto.UpdateBy = userId;
                int j = cdiDao.InsertHECShipTo(shipto);
                if (j >= 1)
                {
                    rowCount += 1;
                }
            }
            // Update to CustomerShipTo table
            int udt = cdiDao.UpdateHECShipToIntoCustomerShipTo();
            if (rowCount > 0)
            {
                result.code = Constant.SUCCESS;
                result.number = rowCount;
                result.message = string.Format(Constant.PROCESS_MESSAGE, rowCount);
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.MESSAGE_ERROR;
            }
            mem.Close();
            return result;
        }


        /// <summary>
        /// Get Detail License ShipTo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CustomerShipToDto GetDetailLicenseShipTo(CustomerShipToDto dto)
        {
            CustomerShipToDto customerShipTo = cdiDao.GetDetailLicenseShipTo(dto);
            if (customerShipTo != null && customerShipTo.CustCode.Equals(customerShipTo.ShipTo))
            {
                object[] pr = { customerShipTo.CustCode };
                CDIDto soldto = cdiDao.GetCDIDetail(pr);
                soldto.ShipTo = dto.ShipTo;
                PropertyCopy.Copy(soldto, customerShipTo);
                int udt = cdiDao.UpdateShipToLicenses(customerShipTo);
            }
            return customerShipTo;
        }

        /// <summary>
        /// Get ShipTo By CustCode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerShipToDto> GetShipToByCustCode(CustomerShipToDto dto)
        {
            CDIDto cdidto = new CDIDto();
            cdidto.CustCode = dto.CustCode;
            IEnumerable<CustomerShipToDto> result = cdiDao.GetShipToByCustCode(cdidto);
            return result;
        }

        /// <summary>
        /// Update Ship To License
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateShipToLicense(CustomerShipToDto obj)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (obj != null)
                {
                    obj.IsManual = obj.IsManual == null ? false : obj.IsManual;
                    int rsCnt = cdiDao.UpdateShipToLicense(obj);
                    if (!string.IsNullOrEmpty(obj.CustCode) && !string.IsNullOrEmpty(obj.ShipTo))
                    {
                        if (obj.CustCode.Equals(obj.ShipTo))
                        {
                            int process = cdiDao.ProcessCustomerByShipTo(obj);
                            int ls = cdiDao.RequiredLicencesShipTo(obj);
                        }
                        else
                        {
                            int rs = cdiDao.ProcessCustomerByShiptoCGrpCode(obj);
                            int ls = cdiDao.RequiredLicensesShipToCGrpCode(obj);
                        }
                    }
                    
                    if (rsCnt > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.MESSAGE_ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = string.Format(Constant.MESSAGE_ERROR_CONVERT, ex.Message);
            }
            return result;
        }


        /****************************************************************************/

        /// <summary>
        /// Search ShipTo
        /// </summary>
        /// <param name="shipto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerShipToDto> SearchShipTo(CustomerShipToDto shipto)
        {
            IEnumerable<CustomerShipToDto> result = cdiDao.SearchShipTo(shipto);
            return result;
        }


        /// <summary>
        /// Get ship to list
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerShipToDto> ShipToList(CustomerShipToDto dto)
        {
            if (!string.IsNullOrEmpty(dto.UpdateDateFrom) && string.IsNullOrEmpty(dto.UpdateDateTo))
            {
                dto.UpdateDateTo = dto.UpdateDateFrom;
            }
            IEnumerable<CustomerShipToDto> result = cdiDao.ShipToList(dto);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shiptoList"></param>
        /// <param name="cERTIFICATION"></param>
        /// <param name="templateNm"></param>
        /// <returns></returns>
        public IWorkbook ExportShipTo(IEnumerable<CustomerShipToDto> shiptoList, string sheetName, string templateNm)
        {
            int process = cdiDao.ProcessShiptoLicenses();
            // Export data
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + templateNm;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY);
                var color_expired = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_warning = new XSSFColor(new byte[] { 255, 153, 0 });
                var color_x = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_red = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_info = new XSSFColor(new byte[] { 24, 213, 247 });
                var font_expired = (XSSFFont)workbook.CreateFont();
                var font_warning = (XSSFFont)workbook.CreateFont();
                var font_x = (XSSFFont)workbook.CreateFont();
                var font_red = (XSSFFont)workbook.CreateFont();
                var font_info = (XSSFFont)workbook.CreateFont();
                font_expired.SetColor(color_expired);
                font_warning.SetColor(color_warning);
                font_x.SetColor(color_x);
                font_red.SetColor(color_red);
                font_expired.IsBold = true;
                font_warning.IsBold = true;
                font_x.IsBold = true;
                font_red.IsBold = true;
                font_info.SetColor(color_info);
                font_info.IsBold = true;
                // Expired
                var style_expired = (XSSFCellStyle)workbook.CreateCellStyle();
                style_expired.SetFont(font_expired);
                // Coming expire
                var style_warning = (XSSFCellStyle)workbook.CreateCellStyle();
                style_warning.SetFont(font_warning);
                // Style for X value
                var style_x = (XSSFCellStyle)workbook.CreateCellStyle();
                style_x.SetFont(font_x);
                // style for red color
                var style_red = (XSSFCellStyle)workbook.CreateCellStyle();
                style_red.SetFont(font_red);
                // style for info
                var style_info = (XSSFCellStyle)workbook.CreateCellStyle();
                style_info.SetFont(font_info);
                // Create sheet
                ISheet sheet = workbook.GetSheet(sheetName);
                int rowIdx = 2;
                foreach (CustomerShipToDto obj in shiptoList)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(obj.UpdateBy); // Update By dd-mm-yyyy
                    // UpdateDate
                    if (!string.IsNullOrEmpty(obj.UpdateDate))
                    {
                        row.CreateCell(1).SetCellValue(new DateTime(int.Parse(obj.UpdateDate.Substring(6, 4)),
                            int.Parse(obj.UpdateDate.Substring(3, 2)), int.Parse(obj.UpdateDate.Substring(0, 2))));
                        row.GetCell(1).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(1).SetCellValue(obj.UpdateDate);
                    }
                    row.CreateCell(2).SetCellValue(obj.CustCode); // SOLD TO
                    // CreateDate
                    row.CreateCell(3).SetCellValue(obj.ShipTo);
                    
                    row.CreateCell(4).SetCellValue(obj.CustName); // SHIP TO NAME
                    if (!string.IsNullOrEmpty(obj.GPPFromDate))
                    {
                        row.CreateCell(5).SetCellValue(new DateTime(int.Parse(obj.GPPFromDate.Substring(6, 4)),
                            int.Parse(obj.GPPFromDate.Substring(3, 2)), int.Parse(obj.GPPFromDate.Substring(0, 2))));
                        row.GetCell(5).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(5).SetCellValue(obj.GPPFromDate);
                    }
                    if (!string.IsNullOrEmpty(obj.GPPToDate))
                    {
                        row.CreateCell(6).SetCellValue(new DateTime(int.Parse(obj.GPPToDate.Substring(6, 4)),
                            int.Parse(obj.GPPToDate.Substring(3, 2)), int.Parse(obj.GPPToDate.Substring(0, 2))));
                        row.GetCell(6).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(6).SetCellValue(obj.GPPToDate);
                    }
                    if (obj.GPPRemainingDays != null)
                    {
                        row.CreateCell(7).SetCellType(CellType.String);
                        if (obj.GPPRemainingDays <= 0)
                        {
                            row.CreateCell(7).SetCellValue(Constant.EXPIRED); // GCN GPP Remaining date
                            row.GetCell(7).CellStyle = style_expired;
                        }
                        else if (obj.GPPRemainingDays > 0 && obj.GPPRemainingDays <= 90)
                        {
                            row.CreateCell(7).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GPPRemainingDays.ToString())); // GCN GPP Remaining date
                            row.GetCell(7).CellStyle = style_warning;
                        }
                    }
                    // GDP
                    if (!string.IsNullOrEmpty(obj.GDPFromDate))
                    {
                        row.CreateCell(8).SetCellValue(new DateTime(int.Parse(obj.GDPFromDate.Substring(6, 4)),
                            int.Parse(obj.GDPFromDate.Substring(3, 2)), int.Parse(obj.GDPFromDate.Substring(0, 2))));
                        row.GetCell(8).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(8).SetCellValue(obj.GDPFromDate);
                        row.GetCell(8).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GDPToDate))
                    {
                        row.CreateCell(9).SetCellValue(new DateTime(int.Parse(obj.GDPToDate.Substring(6, 4)),
                            int.Parse(obj.GDPToDate.Substring(3, 2)), int.Parse(obj.GDPToDate.Substring(0, 2))));
                        row.GetCell(9).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(9).SetCellValue(obj.GDPToDate);
                        row.GetCell(9).CellStyle = cellStyle;
                    }
                    if (obj.GDPRemainingDays != null)
                    {
                        row.CreateCell(10).SetCellType(CellType.String);

                        if (obj.GDPRemainingDays <= 0)
                        {
                            row.CreateCell(10).SetCellValue(Constant.EXPIRED); // GCN GDP Remaining date
                            row.GetCell(10).CellStyle = style_expired;
                        }
                        else if (obj.GDPRemainingDays > 0 && obj.GDPRemainingDays <= 90)
                        {
                            row.CreateCell(10).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GDPRemainingDays.ToString())); // GCN GDP Remaining date
                            row.GetCell(10).CellStyle = style_warning;
                        }
                    }
                    // GSP
                    if (!string.IsNullOrEmpty(obj.GSPFromDate))
                    {
                        row.CreateCell(11).SetCellValue(new DateTime(int.Parse(obj.GSPFromDate.Substring(6, 4)),
                            int.Parse(obj.GSPFromDate.Substring(3, 2)), int.Parse(obj.GSPFromDate.Substring(0, 2))));
                        row.GetCell(11).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(11).SetCellValue(obj.GSPFromDate);
                        row.GetCell(11).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GSPToDate))
                    {
                        row.CreateCell(12).SetCellValue(new DateTime(int.Parse(obj.GSPToDate.Substring(6, 4)),
                            int.Parse(obj.GSPToDate.Substring(3, 2)), int.Parse(obj.GSPToDate.Substring(0, 2))));
                        row.GetCell(12).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue(obj.GSPToDate);
                        row.GetCell(12).CellStyle = cellStyle;
                    }
                    if (obj.GSPRemainingDays != null)
                    {
                        if (obj.GSPRemainingDays <= 0)
                        {
                            row.CreateCell(13).SetCellValue(Constant.EXPIRED); // GCN GSP Remaining date
                            row.GetCell(13).CellStyle = style_expired;
                        }
                        else if (obj.GSPRemainingDays > 0 && obj.GSPRemainingDays <= 90)
                        {
                            row.CreateCell(13).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GSPRemainingDays));
                            row.GetCell(13).CellStyle = style_warning;
                        }
                    }

                    row.CreateCell(14).SetCellValue(obj.TaxCode); // Tax code
                    if (obj.GPKD != null && obj.GPKD == true)
                    {
                        row.CreateCell(15).SetCellValue("X"); // GPKD
                        row.GetCell(15).CellStyle = style_x;
                    }
                    if (obj.NNKDMBTTBYT != null)
                    {
                        if (obj.NNKDMBTTBYT == true)
                        {
                            row.CreateCell(16).SetCellValue("YES"); // Ngành nghề kinh doanh có mua bán TTBYT (COE)
                            row.GetCell(16).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(16).SetCellValue("NO"); // Ngành nghề kinh doanh có mua bán TTBYT (COE)
                            row.GetCell(16).CellStyle = style_red;
                        }
                    }
                    row.CreateCell(17).SetCellValue(obj.GPKDNumber); // So GPKD
                    row.CreateCell(18).SetCellValue(obj.GPKDName); // GPKD Name
                    if (obj.IsCOETTBYT != null)
                    {
                        if (obj.IsCOETTBYT == true)
                        {
                            row.CreateCell(19).SetCellValue("YES"); // Công bố đủ điều kiện mua bán TTBYT (COE)
                            row.GetCell(19).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(19).SetCellValue("NO"); // Công bố đủ điều kiện mua bán TTBYT (COE)
                            row.GetCell(19).CellStyle = style_red;
                        }
                    }
                    // GCN đủ điều kiện kinh doanh dược
                    if (!string.IsNullOrEmpty(obj.GCNDDKKDDFromDate))
                    {
                        row.CreateCell(20).SetCellValue(new DateTime(int.Parse(obj.GCNDDKKDDFromDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKKDDFromDate.Substring(3, 2)), int.Parse(obj.GCNDDKKDDFromDate.Substring(0, 2))));
                        row.GetCell(20).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(20).SetCellValue(obj.GCNDDKKDDFromDate);
                        row.GetCell(20).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GCNDDKKDDToDate))
                    {
                        row.CreateCell(21).SetCellValue(new DateTime(int.Parse(obj.GCNDDKKDDToDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKKDDToDate.Substring(3, 2)), int.Parse(obj.GCNDDKKDDToDate.Substring(0, 2))));
                        row.GetCell(21).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(21).SetCellValue(obj.GCNDDKKDDToDate);
                        row.GetCell(21).CellStyle = cellStyle;
                    }
                    if (obj.GCNDDKKDDRemainingDays != null)
                    {
                        row.CreateCell(22).SetCellType(CellType.String);

                        if (obj.GCNDDKKDDRemainingDays <= 0)
                        {
                            row.CreateCell(22).SetCellValue(Constant.EXPIRED); // GCN DDKKDD Remaining date
                            row.GetCell(22).CellStyle = style_expired;
                        }
                        else if (obj.GCNDDKKDDRemainingDays > 0 && obj.GCNDDKKDDRemainingDays <= 90)
                        {
                            row.CreateCell(22).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GCNDDKKDDRemainingDays.ToString())); // GCN DDKKDD Remaining date
                            row.GetCell(22).CellStyle = style_warning;
                        }
                    }
                    // PHAM VI KINH DOANH
                    if (obj.TDPHCCDCGN != null)
                    {
                        if (obj.TDPHCCDCGN == true)
                        {
                            row.CreateCell(23).SetCellValue("YES"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(23).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(23).SetCellValue("NO"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(23).CellStyle = style_red;
                        }
                    }
                    if (obj.TDNLDLT != null)
                    {
                        if (obj.TDNLDLT == true)
                        {
                            row.CreateCell(24).SetCellValue("YES"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(24).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(24).SetCellValue("NO"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(24).CellStyle = style_red;
                        }
                    }

                    if (obj.TDDMCSD != null)
                    {
                        if (obj.TDDMCSD == true)
                        {
                            row.CreateCell(25).SetCellValue("YES"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(25).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(25).SetCellValue("NO"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(25).CellStyle = style_red;
                        }
                    }
                    if (obj.SinhPham != null)
                    {
                        if (obj.SinhPham == true)
                        {
                            row.CreateCell(26).SetCellValue("YES"); // Sinh phẩm
                            row.GetCell(26).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(26).SetCellValue("NO"); // Sinh phẩm
                            row.GetCell(26).CellStyle = style_red;
                        }
                    }
                    if (obj.Vaccin != null)
                    {
                        if (obj.Vaccin == true)
                        {
                            row.CreateCell(27).SetCellValue("YES"); // Vaccine
                            row.GetCell(27).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(27).SetCellValue("NO"); // Vaccine
                            row.GetCell(27).CellStyle = style_red;
                        }
                    }
                    // DU DIEU KIEN XUAT BAN
                    if (obj.DDKXB_TDPHCCDCGN != null)
                    {
                        if (obj.DDKXB_TDPHCCDCGN == true)
                        {
                            row.CreateCell(28).SetCellValue("YES"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(28).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(28).SetCellValue("NO"); // Thuốc dạng phối hợp có chứa dược chất gây nghiện
                            row.GetCell(28).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_TDNLDLT != null)
                    {
                        if (obj.DDKXB_TDNLDLT == true)
                        {
                            row.CreateCell(29).SetCellValue("YES"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(29).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(29).SetCellValue("NO"); // Thuốc độc, nguyên liệu độc làm thuốc
                            row.GetCell(29).CellStyle = style_red;
                        }
                    }

                    if (obj.DDKXB_TDDMCSD != null)
                    {
                        if (obj.DDKXB_TDDMCSD == true)
                        {
                            row.CreateCell(30).SetCellValue("YES"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(30).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(30).SetCellValue("NO"); // Thuốc dược chất thuộc danh mục bị cấm sử dụng
                            row.GetCell(30).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_SinhPham != null)
                    {
                        if (obj.DDKXB_SinhPham == true)
                        {
                            row.CreateCell(31).SetCellValue("YES"); // Sinh phẩm
                            row.GetCell(31).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(31).SetCellValue("NO"); // Sinh phẩm
                            row.GetCell(31).CellStyle = style_red;
                        }
                    }
                    if (obj.DDKXB_Vaccin != null)
                    {
                        if (obj.DDKXB_Vaccin == true)
                        {
                            row.CreateCell(32).SetCellValue("YES"); // Vaccine
                            row.GetCell(32).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(32).SetCellValue("NO"); // Vaccine
                            row.GetCell(32).CellStyle = style_red;
                        }
                    }
                    // KCB
                    if (!string.IsNullOrEmpty(obj.KCBFromDate))
                    {
                        row.CreateCell(33).SetCellValue(new DateTime(int.Parse(obj.KCBFromDate.Substring(6, 4)),
                            int.Parse(obj.KCBFromDate.Substring(3, 2)), int.Parse(obj.KCBFromDate.Substring(0, 2))));
                        row.GetCell(33).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(33).SetCellValue(obj.KCBFromDate);
                        row.GetCell(33).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.KCBToDate))
                    {
                        row.CreateCell(34).SetCellValue(new DateTime(int.Parse(obj.KCBToDate.Substring(6, 4)),
                            int.Parse(obj.KCBToDate.Substring(3, 2)), int.Parse(obj.KCBToDate.Substring(0, 2))));
                        row.GetCell(34).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(34).SetCellValue(obj.KCBToDate);
                        row.GetCell(34).CellStyle = cellStyle;
                    }
                    if (obj.KCBRemainingDays != null)
                    {
                        if (obj.KCBRemainingDays <= 0)
                        {
                            row.CreateCell(35).SetCellValue(Constant.EXPIRED); // GCN KCB Remaining date 
                            row.GetCell(35).CellStyle = style_expired;
                        }
                        else if (obj.KCBRemainingDays > 0 && obj.KCBRemainingDays <= 90)
                        {
                            row.CreateCell(35).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.KCBRemainingDays)); // GCN KCB Remaining date 
                            row.GetCell(35).CellStyle = style_warning;
                        }
                    }
                    // Danh mục kỹ thuật(có tiêm Botox/ Botulinum)
                    if (obj.DMKTCTBotoxBotulinum != null && obj.DMKTCTBotoxBotulinum == true)
                    {
                        row.CreateCell(36).SetCellValue("X");
                        row.GetCell(36).CellStyle = style_x;
                    }
                    // Đủ điều kiện mua Botox/ Xeomin
                    if (obj.DDKM_BotoxXeomin != null)
                    {
                        if (obj.DDKM_BotoxXeomin == true)
                        {
                            row.CreateCell(37).SetCellValue("YES"); // Vaccine
                            row.GetCell(37).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(37).SetCellValue("NO"); // Vaccine
                            row.GetCell(37).CellStyle = style_red;
                        }
                    }
                    // Giấy chứng nhận đủ điều kiện tiêm chủng
                    if (!string.IsNullOrEmpty(obj.GCNDDKTCFromDate))
                    {
                        row.CreateCell(38).SetCellValue(new DateTime(int.Parse(obj.GCNDDKTCFromDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKTCFromDate.Substring(3, 2)), int.Parse(obj.GCNDDKTCFromDate.Substring(0, 2))));
                        row.GetCell(38).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(38).SetCellValue(obj.GCNDDKTCFromDate);
                        row.GetCell(38).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(obj.GCNDDKTCToDate))
                    {
                        row.CreateCell(39).SetCellValue(new DateTime(int.Parse(obj.GCNDDKTCToDate.Substring(6, 4)),
                            int.Parse(obj.GCNDDKTCToDate.Substring(3, 2)), int.Parse(obj.GCNDDKTCToDate.Substring(0, 2))));
                        row.GetCell(39).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(39).SetCellValue(obj.GCNDDKTCToDate);
                        row.GetCell(39).CellStyle = cellStyle;
                    }
                    if (obj.GCNDDKTCRemainingDays != null)
                    {
                        if (obj.GCNDDKTCRemainingDays <= 0)
                        {
                            row.CreateCell(40).SetCellValue(Constant.EXPIRED); // GCNDDKTC Remaining date 
                            row.GetCell(40).CellStyle = style_expired;
                        }
                        else if (obj.GCNDDKTCRemainingDays > 0 && obj.GCNDDKTCRemainingDays <= 90)
                        {
                            row.CreateCell(40).SetCellValue(string.Format(Constant.COMMING_EXPIRE, obj.GCNDDKTCRemainingDays)); // GCNDDKTC Remaining date 
                            row.GetCell(40).CellStyle = style_warning;
                        }
                    }
                    // CUSTOMER INFORMATION
                    row.CreateCell(41).SetCellValue(obj.Remark);
                    row.CreateCell(42).SetCellValue(obj.Address);
                    row.CreateCell(43).SetCellValue(obj.District);
                    row.CreateCell(44).SetCellValue(obj.Province);
                    row.CreateCell(45).SetCellValue(obj.Region);
                    row.CreateCell(46).SetCellValue(obj.CGrpCode);
                    row.CreateCell(47).SetCellValue(obj.CGrpName);
                    row.CreateCell(48).SetCellValue(obj.Tel);
                    row.CreateCell(49).SetCellValue(obj.Channel);
                    if (obj.IsManual != null)
                    {
                        if (obj.IsManual == true)
                        {
                            row.CreateCell(50).SetCellValue("X");
                            row.GetCell(50).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(50).SetCellValue("");
                        }
                    }
                    // Status licenses
                    if (obj.LicensesYESNO != null && obj.LicensesYESNO == 1)
                    {
                        row.CreateCell(51).SetCellValue(Constant.FULL_LICENSE);
                        row.GetCell(51).CellStyle = style_x;
                    }
                    else if (obj.LicensesYESNO != null && obj.LicensesYESNO == 2)
                    {
                        row.CreateCell(51).SetCellValue(Constant.NO_CONTROL_LICENSE);
                        row.GetCell(51).CellStyle = style_info;
                    }
                    else
                    {
                        row.CreateCell(51).SetCellValue(Constant.LACK_OF_LICENSE);
                        row.GetCell(51).CellStyle = style_warning;
                    }
                    // Licenses Type Missing
                    string licenseTypeMissing = string.Empty;
                    if (!string.IsNullOrEmpty(obj.LicenseGPP))
                    {
                        licenseTypeMissing += obj.LicenseGPP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseGDP))
                    {
                        licenseTypeMissing += obj.LicenseGDP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseTaxCode))
                    {
                        licenseTypeMissing += obj.LicenseTaxCode + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseGPKD))
                    {
                        licenseTypeMissing += obj.LicenseGPKD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseCOETTBYT))
                    {
                        licenseTypeMissing += obj.LicenseCOETTBYT + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseGCNDDKKDD))
                    {
                        licenseTypeMissing += obj.LicenseGCNDDKKDD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseKCB))
                    {
                        licenseTypeMissing += obj.LicenseKCB + ";  ";
                    }
                    if (!string.IsNullOrEmpty(obj.LicenseDMKTBotoxXeomin))
                    {
                        licenseTypeMissing += obj.LicenseDMKTBotoxXeomin;
                    }
                    row.CreateCell(52).SetCellValue(licenseTypeMissing);
                    row.GetCell(52).CellStyle = style_red;
                    rowIdx += 1;
                }

                sheet.AutoSizeColumn(4);
                file.Close();
                return workbook;
            }
        }

        /// <summary>
        /// Export G950
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public IWorkbook ExportG950(CDIDto js)
        {
            int process = cdiDao.ProcessCustomer(js);
            IEnumerable<CDIDto> list = ReportDailyDDKXB(js);
            IEnumerable<ProductKSDBDto> products = cdiDao.GetListProductKSDB();
            IWorkbook workbook = BindToG950File(list, products, js);            
            return workbook;
        }

        /// <summary>
        /// Bind to G950 File
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IWorkbook BindToG950File(IEnumerable<CDIDto> list, IEnumerable<ProductKSDBDto> products, CDIDto js)
        {
            IWorkbook workbook = null;
            string now = DateTime.Now.ToString("dd.MM.yyyy");
            string fiveYearLater = DateTime.Now.AddYears(5).ToString("dd.MM.yyyy");
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + js.FileTemplate;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheet(js.SheetName);
                int rowIdx = 2;
                foreach (CDIDto obj in list)
                {
                    foreach(ProductKSDBDto pro in products)
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row == null)
                        {
                            row = sheet.CreateRow(rowIdx);
                        }
                        row.CreateCell(5).SetCellValue(obj.CustCode);
                        row.CreateCell(6).SetCellValue(now);
                        row.CreateCell(7).SetCellValue(fiveYearLater);
                        row.CreateCell(8).SetCellValue(pro.ProductCode);
                        rowIdx += 1;
                    }
                }
                file.Close();
            }
            return workbook;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public string ReportProductFileByFile(IEnumerable<CDIDto> list, IEnumerable<ProductKSDBDto> products)
        {
            string zipFile = string.Empty;
            IWorkbook workbook = null;
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/CDIReportDaily/"));
            var excelTemplate = "ProductKSDB.xlsx";
            string fl = filePath + excelTemplate;
            List<string> listPath = new List<string>();
            foreach (ProductKSDBDto pro in products)
            {
                using (FileStream file = new FileStream(fl, FileMode.Open, FileAccess.ReadWrite))
                {
                    workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowIdx = 1;
                    foreach(CDIDto dto in list)
                    {
                        IRow row = sheet.CreateRow(rowIdx);
                        row.CreateCell(0).SetCellValue(dto.CustCode);
                        row.CreateCell(1).SetCellValue(pro.ProductCode);
                        rowIdx += 1;
                    }
                    string excelFile = filePath + "ProductKSDB_" + pro.ProductCode + "." + Constant.EXCEL_XLSX;
                    if (File.Exists(excelFile))
                    {
                        File.Delete(excelFile);
                    }
                    FileStream fs = new FileStream(excelFile, FileMode.CreateNew);
                    workbook.Write(fs);
                    fs.Close();
                    file.Close();
                    listPath.Add(excelFile);                    
                }
            }
            zipFile = Utils.CreateZipFile(products.Count(), filePath, listPath);
            return zipFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public string ExportProductByFile(CDIDto js)
        {
            IEnumerable<CDIDto> list = ReportDailyDDKXB(js);
            IEnumerable<ProductKSDBDto> products = cdiDao.GetListProductKSDB();
            return ReportProductFileByFile(list, products);
        }
    }
}