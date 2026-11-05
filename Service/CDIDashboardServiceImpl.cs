using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using DKSHApplication.Common;
using System.Timers;
using System.Globalization;

namespace DKSHApplication.Service
{
    public class CDIDashboardServiceImpl : CDIDashboardService
    {
        CDIDao cdiDao = new CDIDao();
        UserDao userDao = new UserDao();
        CustomerDao customerDao = new CustomerDao();
        AutoScheduleTasksDao autoScheduleTasksDao = new AutoScheduleTasksDao();
        static System.Timers.Timer timer;
        private static string DD_MM_YYYY = "dd-MM-yyyy";

        /// <summary>
        /// Export Licenses Summary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook ExportLicensesSummary(CdiDashboardDto dto)
        {
            IWorkbook workbook = ExportSummaryCDI(dto);
            return workbook;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Export Summary CDI
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private IWorkbook ExportSummaryCDI(CdiDashboardDto dto)
        {
            IEnumerable<CDIDto> LicenseSummary = cdiDao.CDIDashboardReport(dto);
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + Constant.LICENSES_SUMMARY;
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
                ISheet sheet = workbook.GetSheet("Licenses");
                int rowIdx = 2;
                foreach (CDIDto item in LicenseSummary)
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
                    if (!string.IsNullOrEmpty(item.LicenseGPKD))
                    {
                        licenseTypeMissing += item.LicenseGPKD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseTaxCode))
                    {
                        licenseTypeMissing += item.LicenseTaxCode + ";  ";
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
                    if (!string.IsNullOrEmpty(item.LicenseGPP))
                    {
                        licenseTypeMissing += item.LicenseGPP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseGDP))
                    {
                        licenseTypeMissing += item.LicenseGDP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseCOETTBYT))
                    {
                        licenseTypeMissing += item.LicenseCOETTBYT + ";  ";
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
                    if (item.RX != null)
                    {
                        if (item.RX == true)
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
                    if (item.OTC != null)
                    {
                        if (item.OTC == true)
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
                    if (item.EssentialDrugs != null)
                    {
                        if (item.EssentialDrugs == true)
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
                    if (item.DDKXB_TDPHCCDCGN != null)
                    {
                        if (item.DDKXB_TDPHCCDCGN == true)
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
                    if (item.DDKXB_TDNLDLT != null)
                    {
                        if (item.DDKXB_TDNLDLT == true)
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

                    if (item.DDKXB_TDDMCSD != null)
                    {
                        if (item.DDKXB_TDDMCSD == true)
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
                    if (item.DDKXB_SinhPham != null)
                    {
                        if (item.DDKXB_SinhPham == true)
                        {
                            row.CreateCell(35).SetCellValue("YES"); // Sinh phẩm
                            row.GetCell(32).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(35).SetCellValue("NO"); // Sinh phẩm
                            row.GetCell(35).CellStyle = style_red;
                        }
                    }
                    if (item.DDKXB_Vaccin != null)
                    {
                        if (item.DDKXB_Vaccin == true)
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
                    if (item.DDKXB_RX != null)
                    {
                        if (item.DDKXB_RX == true)
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
                    if (item.DDKXB_OTC != null)
                    {
                        if (item.DDKXB_OTC == true)
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
                    if (item.DDKXB_EssentialDrugs != null)
                    {
                        if (item.DDKXB_EssentialDrugs == true)
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
                    // IsManual
                    if (item.IsManual != null)
                    {
                        if (item.IsManual == true)
                        {
                            row.CreateCell(40).SetCellValue("X");
                            row.GetCell(40).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(40).SetCellValue("");
                        }
                    }

                    // CUSTOMER INFORMATION
                    row.CreateCell(41).SetCellValue(item.Remark);
                    row.CreateCell(42).SetCellValue(item.Address);
                    row.CreateCell(43).SetCellValue(item.District);
                    row.CreateCell(44).SetCellValue(item.Province);
                    row.CreateCell(45).SetCellValue(item.Region);
                    row.CreateCell(46).SetCellValue(item.Channel);
                    row.CreateCell(47).SetCellValue(item.CGrpCode);
                    // EXCLUDED Thuoc gay nghien
                    if (item.Excluded_TDPHCCDCGN != null)
                    {
                        if (item.Excluded_TDPHCCDCGN == true)
                        {
                            row.CreateCell(48).SetCellValue("X");
                            row.GetCell(48).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(48).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc doc
                    if (item.Excluded_TDNLDLT != null)
                    {
                        if (item.Excluded_TDNLDLT == true)
                        {
                            row.CreateCell(49).SetCellValue("X");
                            row.GetCell(49).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(49).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc cam
                    if (item.Excluded_TDDMCSD != null)
                    {
                        if (item.Excluded_TDDMCSD == true)
                        {
                            row.CreateCell(50).SetCellValue("X");
                            row.GetCell(50).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(50).SetCellValue("");
                        }
                    }
                    // Remark thuoc kiem soat dac biet
                    row.CreateCell(51).SetCellValue(item.RemarkKSDB);

                    // EXCLUDED Thuoc gay nghien
                    if (item.ExcludedGPP_TDPHCCDCGN != null)
                    {
                        if (item.ExcludedGPP_TDPHCCDCGN == true)
                        {
                            row.CreateCell(52).SetCellValue("X");
                            row.GetCell(52).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(52).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc doc
                    if (item.ExcludedGPP_TDNLDLT != null)
                    {
                        if (item.ExcludedGPP_TDNLDLT == true)
                        {
                            row.CreateCell(53).SetCellValue("X");
                            row.GetCell(53).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(53).SetCellValue("");
                        }
                    }
                    // EXCLUDED Thuoc cam
                    if (item.ExcludedGPP_TDDMCSD != null)
                    {
                        if (item.ExcludedGPP_TDDMCSD == true)
                        {
                            row.CreateCell(54).SetCellValue("X");
                            row.GetCell(54).CellStyle = style_x;
                        }
                        else
                        {
                            row.CreateCell(54).SetCellValue("");
                        }
                    }
                    // Remark thuoc kiem soat dac biet
                    row.CreateCell(55).SetCellValue(item.RemarkGPP_KSDB);
                    row.CreateCell(56).SetCellValue(item.CreateBy);
                    row.CreateCell(57).SetCellValue(item.CreateDate);
                    row.CreateCell(58).SetCellValue(item.FullNameCrt);
                    row.CreateCell(59).SetCellValue(item.UserId);
                    row.CreateCell(60).SetCellValue(item.UpdateDate);
                    row.CreateCell(61).SetCellValue(item.FullNameUdt);
                    rowIdx += 1;
                }
                file.Close();
                return workbook;
            }
        }

        /// <summary>
        /// CustCode Registered By DAY MONTH YEAR
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CdiDashboardDto GetCDIDashboard(CDIDashboardParam dto)
        {
            CdiDashboardDto obj = new CdiDashboardDto();
            obj.LicensesSummaryDto = cdiDao.DashboardCDI();
            IEnumerable<CustomerDto> channel = customerDao.GetChannelCustomer();
            obj.Channel = channel;
            return obj;
            throw new NotImplementedException();
        }


        /// <summary>
        /// Schedule Timer
        /// </summary>
        public void ScheduleTimerAutoLarmExpired(int hour)
        {
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, hour, 0, 0, 0); //Specify your scheduled time HH,MM,SS [17 and 00 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = Math.Abs((scheduledTime - DateTime.Now).TotalMilliseconds);
            timer = new Timer(tickTime);
            //timer.Elapsed += new ElapsedEventHandler();
            timer.Start();
        }

        /// <summary>
        /// Schedule Timer
        /// </summary>
        public void ScheduleTimerAutoLarmComingExpire(int hour)
        {
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, hour, 0, 0, 0); //Specify your scheduled time HH,MM,SS [17 and 00 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = Math.Abs((scheduledTime - DateTime.Now).TotalMilliseconds);
            timer = new Timer(tickTime);
            //timer.Elapsed += new ElapsedEventHandler();
            timer.Start();
        }

        /// <summary>
        /// Auto Alarm Certification Expired
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoAlarmCertificationExpired()
        {
            //timer.Stop();
            // Process customer
            CDIDto cdiDto = new CDIDto();
            int process = cdiDao.ProcessCustomer(cdiDto);
            CdiDashboardDto dto = new CdiDashboardDto();
            List<string> ReportType = new List<string>();
            List<string> files = new List<string>();
            List<string> attachmentFiles = new List<string>();
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            ReportType.Add(Constant.ALARM_CERTIFICATION_EXPIRED_DAILY);
            ReportType.Add(Constant.ALARM_CERTIFICATION_COMING_EXPIRE_DAILY);
            files.Add(Constant.CERTIFICATION_EXPIRED_DAILY);
            files.Add(Constant.CERTIFICATION_COMING_EXPIRE_DAILY);
            int i = 0;
            foreach (string item in ReportType)
            {
                dto.LicenseType = item;
                IWorkbook workbook = ExportSummaryCDI(dto);
                string strFile = files[i];
                string fileName = string.Format(strFile, DateTime.Now.ToString("dd-MM-yyyy"));
                string excelFile = filePath + fileName;
                if (File.Exists(excelFile))
                {
                    File.Delete(excelFile);
                }
                FileStream fs = new FileStream(excelFile, FileMode.CreateNew);
                workbook.Write(fs);
                fs.Close();
                attachmentFiles.Add(excelFile);
                i++;
            }
            // SEND MAIL 
            IEnumerable<UserDto> userList = userDao.GetUserByTeamCDI();
            string[] param = new string[2];
            param[0] = "SYSTEM [AUTO ALARM]";
            param[1] = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string title = string.Format(Constant.ALARM_CERTIFICATION_EXPIRED_AND_COMING_EXPIRE_TITLE, DateTime.Now.ToString("dd-MM-yyyy"));
            string content = string.Format(Constant.CERTIFICATION_EXPIRED_COMING_EXPIRE_DAILY_REPORT_CONTENT, param);
            List<string> ccAdd = new List<string>();
            ccAdd.Add("truc.thanh.ngo@dksh.com");
            foreach (UserDto item in userList)
            {
                //if (!item.Email.Equals("cam.thimong.tran.dksh@gmail.com") && !item.Email.Equals("hien.ngoc.nguyen@dksh.com")
                //    && !item.Email.Equals("tran.bao.duong@dksh.com") && !item.Email.Equals("ngocnu.cdi.dksh@gmail.com")
                //    && !item.Email.Equals("ngoc.hong.luong@dksh.com"))
                if (item.IsUsing == true)
                {
                    ccAdd.Add(item.Email);
                }
            }
            bool flag = Utils.SendMailMultipleAttacment(title, content, Constant.HUYNH_ANH_PHAM_DKSH, ccAdd, attachmentFiles);
            //ScheduleTimerAutoLarmExpired(8); // Begin back up at 08:00:00 AM

        }
        
        /// <summary>
        /// Auto Alarm CDI Report Daily
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ResultMessageDto AutoAlarmCDIReportDaily()
        {
            ResultMessageDto result = new ResultMessageDto();
            AutoScheduleTasksDto parameters = new AutoScheduleTasksDto();
            parameters.SentDatetime = DateTime.Now.ToString("dd-MM-yyyy");
            parameters.TaskName = Constant.ALARM_CERTIFICATION_DAILY;
            IEnumerable<AutoScheduleTasksDto> scheduleList = autoScheduleTasksDao.SearchAutoScheduleTasks(parameters);
            if (scheduleList.Count() == 0)
            {
                AutoScheduleTasksDto param = new AutoScheduleTasksDto();
                param.ScheduleId = Guid.NewGuid();
                param.TaskName = Constant.ALARM_CERTIFICATION_DAILY;
                param.SentStatus = Constant.SENT;
                result.number = autoScheduleTasksDao.AutoScheduleDailyTask(param);
                result.code = Constant.SUCCESS;
                result.message = Constant.MESSAGE_AUTO_ALARM;
                AutoAlarmCertificationExpired();
            }
            else
            {
                result.code = Constant.SENT;
                result.message = string.Empty;
            }
            return result;
        }


        /// <summary>
        /// Send Mail To Supervisor
        /// </summary>
        /// <returns></returns>
        private ResultMessageDto SendMailToGTSTele(SetupTimelineDto timelineDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            SendMailDto sendMailDto = new SendMailDto();
            sendMailDto.title = Constant.SUPERVISOR_GTS_TITLE;
            sendMailDto.content = string.Format(Constant.SUPERVISOR_GTS_BODY, timelineDto.Timeline);
            sendMailDto.fromAddress = Constant.CSKH_VIMEDIMEX_BD_MAIL_ADDRESS;
            sendMailDto.fromPassword = Constant.CSKH_VIMEDIMEX_BD_PASSWORD;
            //sendMailDto.recipient = Constant.PHARMACY_VIME_MAIL;
            //sendMailDto.recipient = Constant.HUYNH_ANH_PHAM_DKSH;
            sendMailDto.recipient = Constant.HUYNH_ANH_PHAM_DKSH;
            sendMailDto.cc = false;
            sendMailDto.bcc = true;
            List<string> bccMailList = new List<string>();
            //IEnumerable<CustCodeInchargeDto> listBCC = cdiDao.GetEmailCustCodeSE_ASM();
            //foreach (CustCodeInchargeDto item in listBCC)
            //{
            //    bccMailList.Add(item.Email);
            //}
            bccMailList.Add(Constant.NGUYEN_THUY_TUYET_VAN_MAIL);
            bccMailList.Add(Constant.CDI_EMAIL);
            //bccMailList.Add(Constant.THINH_HOANG_VO_EMAIL);
            bccMailList.Add(Constant.THO_THI_NGUYEN_MAIL);
            //bccMailList.Add(Constant.TRIET_VIET_TRAN_MAIL);
            //bccMailList.Add(Constant.TELESALE_HECA_MAIL);
            //bccMailList.Add(Constant.UYEN_XUANPHUONG_NGUYEN_MAIL);
            //bccMailList.Add(Constant.PHAN_THANH_KINH_MAIL);
            //bccMailList.Add(Constant.KIM_THI_THIEN_TRAN_EMAIL);
            //bccMailList.Add(Constant.TELESALE_TEAM1_EMAIL);
            //bccMailList.Add(Constant.TELESALE_TEAM_EMAIL);
            //bccMailList.Add(Constant.TELESALE_TEAM2_EMAIL);
            //bccMailList.Add(Constant.TELESALE_TEAM3_EMAIL);
            //bccMailList.Add(Constant.THUY_THITHANH2_LE_EMAIL);
            //bccMailList.Add(Constant.THUY_THITHANH2_NGUYEN_EMAIL);
            //bccMailList.Add(Constant.TRAM_VUAI_TRA_EMAIL);
            //bccMailList.Add(Constant.VAN_THIBICH_NGO_EMAIL);
            //bccMailList.Add("noreplydkshtools@gmail.com");
            sendMailDto.bccAddress = bccMailList;
            IEnumerable<CDIDto> LicenseExpired = cdiDao.TotalLicenseExpired();
            string fileAtt = LicenseExpiredToSale(LicenseExpired);
            List<string> f = new List<string>();
            f.Add(fileAtt);
            sendMailDto.attachment = f;
            // Check if wherther sent
            //AutoScheduleTasksDto parameters = new AutoScheduleTasksDto();
            //parameters.SentDatetime = DateTime.Now.ToString("dd-MM-yyyy");
            //parameters.TaskName = Constant.SEND_MAIL_GTS_TELESALE;
            //IEnumerable<AutoScheduleTasksDto> scheduleList = autoScheduleTasksDao.SearchAutoScheduleTasks(parameters);
            //if (scheduleList.Count() == 0)
            //{
            //    AutoScheduleTasksDto param = new AutoScheduleTasksDto();
            //    param.ScheduleId = Guid.NewGuid();
            //    param.TaskName = Constant.SEND_MAIL_GTS_TELESALE;
            //    param.SentStatus = Constant.SENT;
            //    result.number = autoScheduleTasksDao.AutoScheduleDailyTask(param);
            //}
            result = Utils.SendMailOutlookWithSMTP(sendMailDto);
            return result;
        }

        /// <summary>
        /// License Expired To Sale
        /// </summary>
        /// <param name="LicenseExpired"></param>
        /// <returns></returns>
        private string LicenseExpiredToSale(IEnumerable<CDIDto> LicenseExpired)
        {
            string wbName = Constant.LICENSE_EXPIRED_TO_SALE;
            string templateLocation = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            var filePath = templateLocation + wbName;
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
                var color_status = new XSSFColor(new byte[] { 10, 221, 71 });
                var color_missing = new XSSFColor(new byte[] { 202, 202, 8 });
                var backcolor_status = (XSSFFont)workbook.CreateFont();
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
                backcolor_status.Color = IndexedColors.Red.Index;
                backcolor_status.IsBold = true;
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
                ISheet sheet = workbook.GetSheet("Licenses_Expired");
                // Set date to column status and licenses type missing
                string now = DateTime.Now.ToString("dd/MM/yyyy");
                IRow x = sheet.GetRow(0);
                sheet.AutoSizeColumn(11);
                var status_style = (XSSFCellStyle)workbook.CreateCellStyle();
                status_style.SetFont(backcolor_status);
                status_style.VerticalAlignment = VerticalAlignment.Center;
                status_style.Alignment = HorizontalAlignment.Center;
                x.CreateCell(11).SetCellValue("Status Licenses Updated " + now);
                x.GetCell(11).CellStyle = status_style;
                x.CreateCell(12).SetCellValue("Licenses Type Missing Updated " + now);
                x.GetCell(12).CellStyle = status_style;
                int rowIdx = 2;
                foreach (CDIDto item in LicenseExpired)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.CustCode); // CustCode
                    row.CreateCell(1).SetCellValue(item.CustName); // CustName
                    row.CreateCell(2).SetCellValue(item.Address); // Address
                    row.CreateCell(3).SetCellValue(item.District); // District
                    row.CreateCell(4).SetCellValue(item.Province); // Province
                    row.CreateCell(5).SetCellValue(item.Region); // Region
                    row.CreateCell(6).SetCellValue(item.Channel); // Channel
                    row.CreateCell(7).SetCellValue(item.CGrpCode); // CGrpCode
                    row.CreateCell(8).SetCellValue(item.Mapping); // Mapping
                    row.CreateCell(9).SetCellValue(item.Salesman); // Salesman
                    row.CreateCell(10).SetCellValue(item.MustWin); // MustWin
                    row.GetCell(10).CellStyle = style_red;
                    // Status licenses
                    if (item.LicensesYESNO != null && item.LicensesYESNO == 1)
                    {
                        row.CreateCell(11).SetCellValue(Constant.FULL_LICENSE);
                        row.GetCell(11).CellStyle = style_x;
                    }
                    else if (item.LicensesYESNO != null && item.LicensesYESNO == 2)
                    {
                        row.CreateCell(11).SetCellValue(Constant.NO_CONTROL_LICENSE);
                        row.GetCell(11).CellStyle = style_info;
                    }
                    else
                    {
                        row.CreateCell(11).SetCellValue(Constant.LACK_OF_LICENSE);
                        row.GetCell(11).CellStyle = style_warning;
                    }
                    // Licenses Type Missing
                    string licenseTypeMissing = string.Empty;
                    if (!string.IsNullOrEmpty(item.LicenseGPKD))
                    {
                        licenseTypeMissing += item.LicenseGPKD + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseTaxCode))
                    {
                        licenseTypeMissing += item.LicenseTaxCode + ";  ";
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
                    if (!string.IsNullOrEmpty(item.LicenseGPP))
                    {
                        licenseTypeMissing += item.LicenseGPP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseGDP))
                    {
                        licenseTypeMissing += item.LicenseGDP + ";  ";
                    }
                    if (!string.IsNullOrEmpty(item.LicenseCOETTBYT))
                    {
                        licenseTypeMissing += item.LicenseCOETTBYT + ";  ";
                    }
                    row.CreateCell(12).SetCellValue(licenseTypeMissing); // License type missing
                    row.GetCell(12).CellStyle = style_red;
                    // GPP
                    if (!string.IsNullOrEmpty(item.GPPFromDate))
                    {
                        row.CreateCell(13).SetCellValue(new DateTime(int.Parse(item.GPPFromDate.Substring(6, 4)),
                            int.Parse(item.GPPFromDate.Substring(3, 2)), int.Parse(item.GPPFromDate.Substring(0, 2))));
                        row.GetCell(13).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(13).SetCellValue(item.GPPFromDate);
                    }
                    if (!string.IsNullOrEmpty(item.GPPToDate))
                    {
                        row.CreateCell(14).SetCellValue(new DateTime(int.Parse(item.GPPToDate.Substring(6, 4)),
                            int.Parse(item.GPPToDate.Substring(3, 2)), int.Parse(item.GPPToDate.Substring(0, 2))));
                        row.GetCell(14).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(14).SetCellValue(item.GPPToDate);
                    }
                    if (item.GPPRemainingDays != null)
                    {
                        row.CreateCell(15).SetCellType(CellType.String);
                        if (item.GPPRemainingDays <= 0)
                        {
                            row.CreateCell(15).SetCellValue(Constant.EXPIRED); // GPP Remaining date
                            row.GetCell(15).CellStyle = style_expired;
                        }
                        else if (item.GPPRemainingDays > 0 && item.GPPRemainingDays <= 90)
                        {
                            row.CreateCell(15).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.GPPRemainingDays.ToString())); // GPP Remaining date
                            row.GetCell(15).CellStyle = style_warning;
                        }
                    }

                    // GDP
                    if (!string.IsNullOrEmpty(item.GDPFromDate))
                    {
                        row.CreateCell(16).SetCellValue(new DateTime(int.Parse(item.GDPFromDate.Substring(6, 4)),
                            int.Parse(item.GDPFromDate.Substring(3, 2)), int.Parse(item.GDPFromDate.Substring(0, 2))));
                        row.GetCell(16).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(16).SetCellValue(item.GDPFromDate);
                        row.GetCell(16).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(item.GDPToDate))
                    {
                        row.CreateCell(17).SetCellValue(new DateTime(int.Parse(item.GDPToDate.Substring(6, 4)),
                            int.Parse(item.GDPToDate.Substring(3, 2)), int.Parse(item.GDPToDate.Substring(0, 2))));
                        row.GetCell(17).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(17).SetCellValue(item.GDPToDate);
                        row.GetCell(17).CellStyle = cellStyle;
                    }
                    if (item.GDPRemainingDays != null)
                    {
                        row.CreateCell(18).SetCellType(CellType.String);

                        if (item.GDPRemainingDays <= 0)
                        {
                            row.CreateCell(18).SetCellValue(Constant.EXPIRED); // GCN GDP Remaining date
                            row.GetCell(18).CellStyle = style_expired;
                        }
                        else if (item.GDPRemainingDays > 0 && item.GDPRemainingDays <= 90)
                        {
                            row.CreateCell(18).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.GDPRemainingDays.ToString())); // GCN GDP Remaining date
                            row.GetCell(18).CellStyle = style_warning;
                        }
                    }

                    // GCN đủ điều kiện kinh doanh dược
                    if (!string.IsNullOrEmpty(item.GCNDDKKDDFromDate))
                    {
                        row.CreateCell(19).SetCellValue(new DateTime(int.Parse(item.GCNDDKKDDFromDate.Substring(6, 4)),
                            int.Parse(item.GCNDDKKDDFromDate.Substring(3, 2)), int.Parse(item.GCNDDKKDDFromDate.Substring(0, 2))));
                        row.GetCell(19).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(19).SetCellValue(item.GCNDDKKDDFromDate);
                        row.GetCell(19).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(item.GCNDDKKDDToDate))
                    {
                        row.CreateCell(20).SetCellValue(new DateTime(int.Parse(item.GCNDDKKDDToDate.Substring(6, 4)),
                            int.Parse(item.GCNDDKKDDToDate.Substring(3, 2)), int.Parse(item.GCNDDKKDDToDate.Substring(0, 2))));
                        row.GetCell(20).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(20).SetCellValue(item.GCNDDKKDDToDate);
                        row.GetCell(20).CellStyle = cellStyle;
                    }
                    if (item.GCNDDKKDDRemainingDays != null)
                    {
                        row.CreateCell(21).SetCellType(CellType.String);

                        if (item.GCNDDKKDDRemainingDays <= 0)
                        {
                            row.CreateCell(21).SetCellValue(Constant.EXPIRED); // GCN DDKKDD Remaining date
                            row.GetCell(21).CellStyle = style_expired;
                        }
                        else if (item.GCNDDKKDDRemainingDays > 0 && item.GCNDDKKDDRemainingDays <= 90)
                        {
                            row.CreateCell(21).SetCellValue(string.Format(Constant.COMMING_EXPIRE, item.GCNDDKKDDRemainingDays.ToString())); // GCN DDKKDD Remaining date
                            row.GetCell(21).CellStyle = style_warning;
                        }
                    }
                    // GPKD
                    if (item.GPKD != null && item.GPKD == true)
                    {
                        row.CreateCell(22).SetCellValue("X");
                        row.GetCell(22).CellStyle = style_x;
                    }
                    // Tax code
                    row.CreateCell(23).SetCellValue(item.TaxCode);

                    rowIdx += 1;
                }
                sheet.AutoSizeColumn(12);
                file.Close();
                string fileName = string.Format("LicenseExpiredToSale-{0}", DateTime.Now.ToString("dd-MM-yyyy"));
                string excelFile = templateLocation + fileName + ".xlsx";
                if (File.Exists(excelFile))
                {
                    File.Delete(excelFile);
                }
                FileStream fs = new FileStream(excelFile, FileMode.CreateNew);
                workbook.Write(fs);
                fs.Close();
                return excelFile;
            }
        }

        /// <summary>
        /// Send Mail To SuperVisor
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto SendMailToSuperVisor()
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                SetupTimelineDto dto = cdiDao.GetTimelineIsRun();
                result = SendMailToGTSTele(dto);
                if (dto != null)
                {
                    string today = DateTime.Now.ToShortDateString();
                    DateTime fromDate = Utils.ConvertStringToDate(dto.UpdateDate, DD_MM_YYYY);
                    DateTime toDate = Utils.ConvertStringToDate(dto.Timeline, DD_MM_YYYY);
                    List<string> mondayList = Utils.GetMondayInRangeDates(fromDate, toDate);
                    foreach (string monday in mondayList)
                    {
                        if (monday.Equals(today))
                        {
                            result = SendMailToGTSTele(dto);
                            result.code = Constant.SUCCESS;
                            result.message = string.Format(Constant.TIMLINE_SENT_SUCCESS_MESSAGE, dto.Timeline);
                            break;
                        }
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_TIMELINE_IS_RUN_MESSAGE;
                }
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// View Detail Channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook ViewDetailChannel(CdiDashboardDto dto)
        {
            IEnumerable<ChannelHistoryDto> result = customerDao.ViewDetailChannel(dto);
            IWorkbook workbook = ChannelHistory(result);
            return workbook;
        }

        /// <summary>
        /// Channel History Report Monthly
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook ChannelHistoryReport(CdiDashboardDto dto)
        {
            IEnumerable<ChannelHistoryDto> channelHistory = customerDao.CompareChannelHistory(dto);
            IWorkbook workbook = ChannelHistory(channelHistory);
            return workbook;
        }

        /// <summary>
        /// Channel History
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public IWorkbook ChannelHistory(IEnumerable<ChannelHistoryDto> list)
        {
            string wbName = Constant.CHANNEL_HISTORY;
            string templateLocation = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            var filePath = templateLocation + wbName;
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
                var color_status = new XSSFColor(new byte[] { 10, 221, 71 });
                var color_missing = new XSSFColor(new byte[] { 202, 202, 8 });
                var backcolor_status = (XSSFFont)workbook.CreateFont();
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
                backcolor_status.Color = IndexedColors.Red.Index;
                backcolor_status.IsBold = true;
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
                ISheet sheet = workbook.GetSheet("Channel_History");
                int rowIdx = 1;
                foreach (ChannelHistoryDto item in list)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.CustCode); // CustCode
                    row.CreateCell(1).SetCellValue(item.CustName); // CustName
                    row.CreateCell(2).SetCellValue(item.Addr); // Address
                    row.CreateCell(3).SetCellValue(item.District); // District
                    row.CreateCell(4).SetCellValue(item.Province); // Province
                    row.CreateCell(5).SetCellValue(item.Region); // Region
                    row.CreateCell(6).SetCellValue(item.Channel); // Channel
                    row.CreateCell(7).SetCellValue(item.CGrpCode); // CGrpCode
                    row.CreateCell(8).SetCellValue(item.NewChannel); // New Channel
                    row.CreateCell(9).SetCellValue(item.NewCGrpCode); // New CGrpCode
                    rowIdx += 1;
                }
                return workbook;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Summary Channel History
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SummaryChannelHistoryDto> SummaryChannelHistory(CDIDashboardParam dto)
        {
            IEnumerable<SummaryChannelHistoryDto> result = cdiDao.SummaryChannelHistory(dto);
            return result;
        }

        /// <summary>
        /// View Total Every Month
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook ViewTotalEveryMonth(CdiDashboardDto dto)
        {
            IEnumerable<ChannelHistoryDto> result = customerDao.ViewTotalEveryMonth(dto);
            IWorkbook workbook = ChannelHistory(result);
            return workbook;
        }

        /// <summary>
        /// Channel History Report Monthly
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook CompareAnyMonth(CdiDashboardDto dto)
        {
            IEnumerable<ChannelHistoryDto> compare2Month = customerDao.CompareAnyMonth(dto);
            IWorkbook workbook = ChannelHistory(compare2Month);
            return workbook;
        }

    }
}