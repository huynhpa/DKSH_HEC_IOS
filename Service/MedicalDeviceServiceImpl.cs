using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class MedicalDeviceServiceImpl : MedicalDeviceService
    {
        SellingQuotaDao sellingQuotaDao = new SellingQuotaDao();
        OrderTypeDao orderTypeDao = new OrderTypeDao();
        /// <summary>
        /// Upload Customer
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadVA05(HttpPostedFileBase file)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook;
            
            MemoryStream mem = new MemoryStream();
            mem.SetLength(file.ContentLength);
            file.InputStream.Read(mem.GetBuffer(), 0, file.ContentLength);
            string extension = Path.GetExtension(file.FileName).Replace(".","");
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

            ISheet sheet = workbook.GetSheet("DATA");
            OrderTypeDto orderTypeDto = new OrderTypeDto();
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                string cell_VBELN = Utils.GetFormattedCellValue(row.GetCell(0));
                string cell_DEVICE = Utils.GetFormattedCellValue(row.GetCell(1));
                string cell_ORDERNO = Utils.GetFormattedCellValue(row.GetCell(2));
                string cell_POTEXT = Utils.GetFormattedCellValue(row.GetCell(3));
                string cell_RET_SO = Utils.GetFormattedCellValue(row.GetCell(4));
                if (!cell_VBELN.Substring(0, 3).Equals("219"))
                {
                    orderTypeDto.Id = Guid.NewGuid();
                    orderTypeDto.RetSo = cell_RET_SO;
                    orderTypeDto.Device = cell_DEVICE;
                    orderTypeDto.Potext = cell_POTEXT;
                    orderTypeDto.VBeln = cell_VBELN;
                    orderTypeDto.OrderNo = cell_ORDERNO;
                    int cnt = orderTypeDao.InsertOrderTypeCount(orderTypeDto);
                }
            }
            IWorkbook workbookResult = ExportResult();
            return result;
        }

        /// <summary>
        /// Export Result
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public IWorkbook ExportResult()
        {
            IWorkbook workbook;
            string extension = Constant.EXCEL_XLSX;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "OrderTypeCount.xlsx";
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                if (Constant.EXCEL_XLSX.Equals(extension))
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("This format is not supported");
                }

                ISheet sheet_OrderType = workbook.GetSheet("OrderType");
                ISheet sheet_OrderType_213 = workbook.GetSheet("OrderType_213");
                ISheet sheet_OrderType_TENDER = workbook.GetSheet("OrderType_TENDER");
            }
            return workbook;
        }

        public ResultMessageDto UploadSellingQuota(HttpPostedFileBase file, string userId, string UpdateDate)
        {
            ResultMessageDto result = new ResultMessageDto();
            SellingQuotaDto dtoDel = new SellingQuotaDto();
            int del = sellingQuotaDao.DeleteSellingQuota(dtoDel);
            IWorkbook workbook;
            MemoryStream mem = new MemoryStream();
            mem.SetLength(file.ContentLength);
            file.InputStream.Read(mem.GetBuffer(), 0, file.ContentLength);
            string extension = Path.GetExtension(file.FileName).Replace(".", "");
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
            SellingQuotaDto sellingQuota = null;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                sellingQuota = new SellingQuotaDto();
                IRow row = sheet.GetRow(i);
                sellingQuota.CustCode = Utils.GetFormattedCellValue(row.GetCell(4)); // CustCode
                sellingQuota.CustName = Utils.GetFormattedCellValue(row.GetCell(5)); // CustName
                sellingQuota.AddressLocation = Utils.GetFormattedCellValue(row.GetCell(6)); // Address
                sellingQuota.District = Utils.GetFormattedCellValue(row.GetCell(7)); // District
                sellingQuota.Province = Utils.GetFormattedCellValue(row.GetCell(8)); // Province
                sellingQuota.Region = Utils.GetFormattedCellValue(row.GetCell(9)); // Region
                sellingQuota.Client = Utils.GetFormattedCellValue(row.GetCell(10)); // Client
                sellingQuota.ProductCode = Utils.GetFormattedCellValue(row.GetCell(11)); // ProductCode
                sellingQuota.ProductName = Utils.GetFormattedCellValue(row.GetCell(12)); // ProductName
                sellingQuota.QuotaUploadUnit = Utils.GetFormattedCellValue(row.GetCell(14)); // QuotaUploadUnit
                sellingQuota.QuotaBox = Utils.GetFormattedCellValue(row.GetCell(15)); // QuotaBox
                sellingQuota.ContractNo = Utils.GetFormattedCellValue(row.GetCell(17)); // ContractNo
                sellingQuota.SAPContractID = Utils.GetFormattedCellValue(row.GetCell(18)); // SAPContractID
                sellingQuota.ContractValidOn = Utils.GetFormattedCellValue(row.GetCell(20)); // ContractValidOn
                sellingQuota.ContractValidTo = Utils.GetFormattedCellValue(row.GetCell(21)); // ContractValidTo
                sellingQuota.QuotaValidOn = Utils.GetFormattedCellValue(row.GetCell(22)); // QuotaValidOn
                sellingQuota.QuotaValidTo = Utils.GetFormattedCellValue(row.GetCell(23)); // QuotaValidTo
                sellingQuota.SaleOrg = Utils.GetFormattedCellValue(row.GetCell(24)); // SaleOrg
                sellingQuota.DC = Utils.GetFormattedCellValue(row.GetCell(26)); // DC
                sellingQuota.Remark = Utils.GetFormattedCellValue(row.GetCell(27)); // Remark
                sellingQuota.UpdateBy = userId;
                sellingQuota.UpdateDate = UpdateDate;
                int j = sellingQuotaDao.InsertSellingQuota(sellingQuota);
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
        /// Search Selling Quota
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<SellingQuotaDto> SearchSellingQuota(SellingQuotaDto jsonParam)
        {
            IEnumerable<SellingQuotaDto> SellingQuotaList = sellingQuotaDao.SearchSellingQuota(jsonParam);
            return SellingQuotaList;
        }

        /// <summary>
        /// Export Result
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IWorkbook ExportResult(SellingQuotaDto jsonParam)
        {
            IEnumerable<SellingQuotaDto> SellingQuotaList = sellingQuotaDao.SearchSellingQuota(jsonParam);
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + Constant.SELLING_QUOTA_XLSX;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY);
                var color_expired = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_warning = new XSSFColor(new byte[] { 255, 153, 0 });
                var color_success = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_info = new XSSFColor(new byte[] { 24, 213, 247 });
                var color_red = new XSSFColor(new byte[] { 255, 0, 0 });
                var font_expired = (XSSFFont)workbook.CreateFont();
                var font_warning = (XSSFFont)workbook.CreateFont();
                var font_success = (XSSFFont)workbook.CreateFont();
                var font_red = (XSSFFont)workbook.CreateFont();
                var font_info = (XSSFFont)workbook.CreateFont();
                font_expired.SetColor(color_expired);
                font_warning.SetColor(color_warning);
                font_success.SetColor(color_success);
                font_red.SetColor(color_red);
                font_expired.IsBold = true;
                font_warning.IsBold = true;
                font_success.IsBold = true;
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
                var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                style_success.SetFont(font_success);
                // 
                var style_red = (XSSFCellStyle)workbook.CreateCellStyle();
                style_red.SetFont(font_red);
                var style_info = (XSSFCellStyle)workbook.CreateCellStyle();
                style_info.SetFont(font_info);
                ISheet sheet = workbook.GetSheet("Selling_Quota");
                int rowIdx = 1;
                foreach(SellingQuotaDto item in SellingQuotaList)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.CustCode); // CustCode
                    row.CreateCell(1).SetCellValue(item.CustName); // CustName
                    row.CreateCell(2).SetCellValue(item.AddressLocation); // Address
                    row.CreateCell(3).SetCellValue(item.District); // District
                    row.CreateCell(4).SetCellValue(item.Province); // Province
                    row.CreateCell(5).SetCellValue(item.Region); // Region
                    row.CreateCell(6).SetCellValue(item.Client); // Client
                    row.CreateCell(7).SetCellValue(item.ProductCode); // ProductCode
                    row.CreateCell(8).SetCellValue(item.ProductName); // ProductName
                    row.CreateCell(9).SetCellValue(item.QuotaBox); // QuotaBox
                    row.CreateCell(10).SetCellValue(item.ContractNo); // ContractNo
                    row.CreateCell(11).SetCellValue(item.SAPContractID); // SAPContractID
                    row.CreateCell(12).SetCellValue(item.ContractValidOn); // ContractValidOn
                    row.CreateCell(13).SetCellValue(item.ContractValidTo); // ContractValidTo
                    row.CreateCell(14).SetCellValue(item.QuotaValidOn); // QuotaValidOn
                    row.CreateCell(15).SetCellValue(item.QuotaValidTo); // QuotaValidTo
                    row.CreateCell(16).SetCellValue(item.SaleOrg); // SaleOrg
                    row.CreateCell(17).SetCellValue(item.DC); // DC
                    row.CreateCell(18).SetCellValue(item.Remark); // Remark
                    row.CreateCell(19).SetCellValue(item.UpdateDate); // UpdateDate
                    rowIdx++;
                }
                return workbook;
            }
        }

        /// <summary>
        /// Fetching Email
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public ResultMessageDto FetchingEmail(ReadMailDto rm)
        {
            ResultMessageDto result = SendMailExchangeService.ReadMailSpecificate(rm);
            return result;
        }
    }
}