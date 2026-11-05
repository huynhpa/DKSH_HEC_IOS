using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace DKSHApplication.Service
{
    public class TransferNotificationServiceImpl : TransferNotificationService
    {
        private static string VN61_DKSH_BD = "DKSH_BD";
        private static string VN62_DKSH_DN = "DKSH_DN";
        private static string VN63_DKSH_HN = "DKSH_HN";
        private static string VN64_VIME = "Vime";
        private static string VN65_DAPHA = "Dapharco";
        private static string VN66 = "VN66";
        TransferNotificationDao transferDao = new TransferNotificationDao();

        /// <summary>
        /// Export PDF
        /// </summary>
        /// <param name="DeliveryNo"></param>
        public string ExportPDF(InvoiceZZ24Dto param)
        {
            InvoiceZZ24Dto invoice = transferDao.GetInvoiceZZ24ByDelivery(param);
            if (invoice != null)
            {
                string wb = GetTemplateByEntity(invoice);
                return wb;
            }
            return null;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Binding data to template by entity
        /// </summary>
        private string GetTemplateByEntity(InvoiceZZ24Dto entity)
        {
            IWorkbook workbook;
            string fileName = string.Empty;
            string extension = Constant.EXCEL_XLSX;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            var fileStream = filePath + entity.Plant.ToUpper() + ".xlsx";
            using (FileStream file = new FileStream(fileStream, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (Constant.EXCEL_XLSX.Equals(extension))
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("This format is not supported");
                }
                string[] d = new string[3];
                d[0] = DateTime.Now.Day.ToString();
                d[1] = DateTime.Now.Month.ToString();
                d[2] = DateTime.Now.Year.ToString();
                
                ISheet sheet = workbook.GetSheet(entity.Plant.ToUpper());
                var color = new XSSFColor(new byte[] { 3, 15, 1 });
                var font = (XSSFFont)workbook.CreateFont();
                font.SetColor(color);
                font.IsBold = true;
                font.FontHeightInPoints = 14;
                var style = (XSSFCellStyle)workbook.CreateCellStyle();
                var border = (XSSFCellStyle)workbook.CreateCellStyle();
                var mergeStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                mergeStyle.Alignment = HorizontalAlignment.CenterSelection;
                mergeStyle.VerticalAlignment = VerticalAlignment.Center;
                style.SetFont(font);
                border.BorderRight = BorderStyle.Thin;
                border.BorderLeft = BorderStyle.Thin;
                border.BorderBottom = BorderStyle.Thin;
                border.BorderTop = BorderStyle.Thin;
                border.VerticalAlignment = VerticalAlignment.Center;
                border.Alignment = HorizontalAlignment.Center;
                border.SetFont(font);
                /* Binding detail invoice */
                int rowIdx = 17;
                //int mergeRow = 0;
                IEnumerable<InvoiceDetailDto> invoiceDetailUngroup = transferDao.SearchInvoiceDetailUngroup(entity);
                IEnumerable<InvoiceDetailDto> invoiceDetailList = transferDao.SearchInvoiceDetail(entity);
                double SumAmount = 0;
                IRow row = sheet.CreateRow(rowIdx);
                IEnumerable<InvoiceDetailDto> CountInvoiceByCustomerAndPlant = transferDao.CountInvoiceByCustomerAndPlant(entity);
                // Sum invoice detail
                if (CountInvoiceByCustomerAndPlant != null && CountInvoiceByCustomerAndPlant.Count() > 0)
                {
                    foreach(InvoiceDetailDto invoice in CountInvoiceByCustomerAndPlant)
                    {
                        SumAmount += Convert.ToDouble(invoice.Amt);
                    }
                    
                }
                // Binding invoice details
                if (invoiceDetailUngroup.Count() > 0)
                {
                    //mergeRow = invoiceDetailList.Count() - 1;
                    foreach (InvoiceDetailDto item in invoiceDetailUngroup)
                    {
                        if (rowIdx == 17)
                        {
                            sheet.GetRow(rowIdx).CreateCell(1).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(2).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(3).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(4).SetCellValue(item.Descriptions); // Tên sản phẩm
                            sheet.GetRow(rowIdx).GetCell(4).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(5).SetCellValue(item.DeliveryQty); // Số lượng
                            sheet.GetRow(rowIdx).GetCell(5).CellStyle = border;
                        }
                        else
                        {
                            sheet.CreateRow(rowIdx).CreateCell(4).SetCellValue(item.Descriptions); // Tên sản phẩm
                            sheet.GetRow(rowIdx).CreateCell(1).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(2).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(3).CellStyle = border;
                            sheet.GetRow(rowIdx).GetCell(4).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(5).SetCellValue(item.DeliveryQty); // Số lượng                            
                            sheet.GetRow(rowIdx).GetCell(5).CellStyle = border;
                        }
                        
                        rowIdx += 1;
                    }
                    // Merge rows by invoice
                    int idx = 0;
                    foreach(InvoiceDetailDto obj in CountInvoiceByCustomerAndPlant)
                    {
                        obj.Plant = entity.Plant;
                        obj.CreateDate = entity.CreateDate;
                        IEnumerable<InvoiceDetailDto> mergeRegionInvoice = transferDao.MergeInvoiceByDelivery(obj);
                        if (idx == 0)
                        {
                            CellRangeAddress mergeInvoiceNo = new CellRangeAddress(17, 17 + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 1, 1);
                            CellRangeAddress mergeDate = new CellRangeAddress(17, 17 + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 2, 2);
                            CellRangeAddress mergeTotal = new CellRangeAddress(17, 17 + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 3, 3);
                            sheet.AddMergedRegion(mergeInvoiceNo);
                            sheet.AddMergedRegion(mergeDate);
                            sheet.AddMergedRegion(mergeTotal);
                            row.GetCell(1).SetCellValue(!string.IsNullOrEmpty(obj.RedInvoice) ? obj.RedInvoice + " - " + obj.InvoiceNo : "" + obj.InvoiceNo); // Số hóa đơn
                            row.GetCell(2).SetCellValue(obj.InvoiceDate); // Ngày hóa đơn
                            row.GetCell(3).SetCellValue(obj.Amount); // Tổng tiền
                            //row.GetCell(1).CellStyle = mergeStyle;
                            //row.GetCell(2).CellStyle = mergeStyle;
                            //row.GetCell(3).CellStyle = mergeStyle;
                        }
                        else
                        {
                            CellRangeAddress mergeInvoiceNo = new CellRangeAddress(17 + idx, 17 + idx + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 1, 1);
                            CellRangeAddress mergeDate = new CellRangeAddress(17 + idx, 17 + idx + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 2, 2);
                            CellRangeAddress mergeTotal = new CellRangeAddress(17 + idx, 17 + idx + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 3, 3);
                            sheet.AddMergedRegion(mergeInvoiceNo);
                            sheet.AddMergedRegion(mergeDate);
                            sheet.AddMergedRegion(mergeTotal);
                            sheet.GetRow(17 + idx).CreateCell(1).SetCellValue(!string.IsNullOrEmpty(obj.RedInvoice) ? obj.RedInvoice + " - " + obj.InvoiceNo : "" + obj.InvoiceNo); // Số hóa đơn
                            sheet.GetRow(17 + idx).CreateCell(2).SetCellValue(obj.InvoiceDate); // Ngày hóa đơn
                            sheet.GetRow(17 + idx).CreateCell(3).SetCellValue(obj.Amount); // Tổng tiền                            
                            sheet.GetRow(17 + idx).GetCell(1).CellStyle = border;
                            sheet.GetRow(17 + idx).GetCell(2).CellStyle = border;
                            sheet.GetRow(17 + idx).GetCell(3).CellStyle = border;
                            //sheet.GetRow(17 + idx).GetCell(1).CellStyle = mergeStyle;
                            //sheet.GetRow(17 + idx).GetCell(2).CellStyle = mergeStyle;
                            //sheet.GetRow(17 + idx).GetCell(3).CellStyle = mergeStyle;
                        }
                        idx += mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant + 1;
                        mergeRegionInvoice = null;
                    }
                    // Total row
                    sheet.CreateRow(17 + idx).CreateCell(1).SetCellValue("Tổng tiền");
                    sheet.GetRow(17 + idx).GetCell(1).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(2).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(4).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(5).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(3).SetCellValue(string.Format("{0:n0}", SumAmount));
                    sheet.GetRow(17 + idx).GetCell(3).CellStyle = border;
                }
                string address = entity.Addrs + " " + entity.District;
                if (entity.Plant.Equals(Constant.VN61))
                {
                    fileName = entity.CustName + "_" + VN61_DKSH_BD;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    string currDateHCM = string.Format(Constant.HCMC_DATETIME, d);
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHCM);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN61_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN61_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN61_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN62))
                {
                    fileName = entity.CustName + "_" + VN62_DKSH_DN;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    string currDateDaNang = string.Format(Constant.DA_NANG_DATETIME, d);
                    sheet.CreateRow(6).CreateCell(4).SetCellValue(currDateDaNang);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN62_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN62_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN62_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.VN62_TOLL_FREE);
                }
                else if (entity.Plant.Equals(Constant.VN63))
                {
                    fileName = entity.CustName + "_" + VN63_DKSH_HN;
                    string currDateHN = string.Format(Constant.HANOI_DATETIME, d);
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHN);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.VN63_ADDRESS);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_COMPANY_ADDRESS);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN64))
                {
                    fileName = entity.CustName + "_" + VN64_VIME;
                    string currDateHCM = string.Format(Constant.HCMC_DATETIME, d);
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHCM);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN64_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN64_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN64_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN65))
                {
                    fileName = entity.CustName + "_" + VN65_DAPHA;
                    sheet.GetRow(11).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(11).GetCell(2).CellStyle = style;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    string currDate = string.Format(Constant.DA_NANG_DATETIME, d);
                    sheet.CreateRow(5).CreateCell(4).SetCellValue(currDate);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN65_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN65_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN65_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN66))
                {
                    fileName = entity.CustName + "_" + VN66;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    string currDateHCM = string.Format(Constant.HCMC_DATETIME, d);
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHCM);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN66_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN66_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN66_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.ANNOUNCEMENT_LINE01);
                //sheet.CreateRow(rowIdx + 1).CreateCell(1).SetCellValue(Constant.ANNOUNCEMENT_LINE02);
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE01);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_INFO);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE02);
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE03);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE04);
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE05);
                rowIdx += 2;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.THANK_YOU);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                sheet.AutoSizeColumn(4);
                fileName = Utils.ReplaceSpecialChar(fileName);
                string pdfFile = filePath + fileName + Constant.PDF_EXTENSION;
                string excelFile = filePath + fileName + "." + Constant.EXCEL_XLSX;
                if (File.Exists(excelFile))
                {
                    File.Delete(excelFile);
                }
                FileStream fs = new FileStream(excelFile, FileMode.CreateNew);
                workbook.Write(fs);
                fs.Close();
                file.Close();
                ExcelToPdf excelToPdf = new ExcelToPdf();
                excelToPdf.ConvertFile(excelFile, pdfFile);
                //File.Delete(excelFile);
            }
            
            return fileName;
        }

        /// <summary>
        /// Transfer Notification List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceZZ24Dto> TransferNotificationList(InvoiceZZ24Dto param)
        {
            IEnumerable<InvoiceZZ24Dto> lst = transferDao.SearchInvoice(param);
            return lst;
        }

        /// <summary>
        /// Upload And Process
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto UploadAndProcess(HttpPostedFileBase file, string userId, UploadStatusDto uploadStatus)
        {
            ResultMessageDto result = new ResultMessageDto();            
            IWorkbook workbook;
            int count = 0;
            string extension = Path.GetExtension(file.FileName).Replace(".", "");
            string transferNumber = Utils.GenerateRandomNo().ToString();
            MemoryStream mem = new MemoryStream();
            mem.SetLength(file.ContentLength);
            file.InputStream.Read(mem.GetBuffer(), 0, file.ContentLength);
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
            int del = transferDao.DeleteAllInvoiceToPreventDuplicate(uploadStatus);
            ISheet sheet_zz24 = workbook.GetSheet("ZZ24"); // ZZ24
            InvoiceZZ24Dto invoiceZZ24Dto = null;
            for (int i = 1; i <= sheet_zz24.LastRowNum; i++)
            {
                invoiceZZ24Dto = new InvoiceZZ24Dto();
                IRow row = sheet_zz24.GetRow(i);
                ICell cell_CustCode = row.GetCell(1) != null ? row.GetCell(1) : row.CreateCell(1); // Custcode
                ICell cell_CustName = row.GetCell(2) != null ? row.GetCell(2) : row.CreateCell(2); // Cust name
                ICell cell_Address = row.GetCell(3) != null ? row.GetCell(3) : row.CreateCell(3); // Cust name
                ICell cell_DeliveryNo = row.GetCell(5) != null ? row.GetCell(5) : row.CreateCell(5); // Delivery No
                ICell cell_InvoiceNo = row.GetCell(6) != null ? row.GetCell(6) : row.CreateCell(6); // Invoice No
                ICell cell_InvoiceDate = row.GetCell(7) != null ? row.GetCell(7) : row.CreateCell(7); // Invoice Date
                ICell cell_Amount = row.GetCell(9) != null ? row.GetCell(9) : row.CreateCell(9); // Amount
                ICell cell_PaymentTerm = row.GetCell(10) != null ? row.GetCell(10) : row.CreateCell(10); // Payment Term
                ICell cell_Plant = row.GetCell(12) != null ? row.GetCell(12) : row.CreateCell(12); // Plant
                ICell cell_RedInvoice = row.GetCell(19) != null ? row.GetCell(19) : row.CreateCell(19); // Plant
                string PaymentTerm = Utils.GetFormattedCellValue(cell_PaymentTerm);
                if (!string.IsNullOrEmpty(PaymentTerm) && PaymentTerm.ToLower().Equals(Constant.PAYMENT_TERM_01))
                {
                    // binding Invoice ZZ24 Dto
                    invoiceZZ24Dto.CustCode = Utils.GetFormattedCellValue(cell_CustCode);
                    invoiceZZ24Dto.CustName = Utils.GetFormattedCellValue(cell_CustName);
                    invoiceZZ24Dto.Addrs = Utils.GetFormattedCellValue(cell_Address);
                    invoiceZZ24Dto.DeliveryNo = Utils.GetFormattedCellValue(cell_DeliveryNo);
                    invoiceZZ24Dto.InvoiceNo = Utils.GetFormattedCellValue(cell_InvoiceNo);
                    invoiceZZ24Dto.InvoiceDate = Utils.GetFormattedCellValue(cell_InvoiceDate);
                    invoiceZZ24Dto.Amount = !string.IsNullOrEmpty(Utils.GetFormattedCellValue(cell_Amount)) ? Utils.GetFormattedCellValue(cell_Amount).Trim().Replace(".","") : "0";
                    invoiceZZ24Dto.PaymentTerm = Utils.GetFormattedCellValue(cell_PaymentTerm);
                    invoiceZZ24Dto.Plant = Utils.GetFormattedCellValue(cell_Plant);
                    invoiceZZ24Dto.RedInvoice = Utils.GetFormattedCellValue(cell_RedInvoice);
                    invoiceZZ24Dto.Id = Guid.NewGuid();
                    invoiceZZ24Dto.TransferNumber = transferNumber;
                    invoiceZZ24Dto.CreateBy = userId;
                    invoiceZZ24Dto.UpdateBy = userId;
                    count += transferDao.InsertInvoiceZZ24(invoiceZZ24Dto);
                }
            }
            InvoiceZZ24Dto SearchDto = new InvoiceZZ24Dto();
            IEnumerable<InvoiceZZ24Dto> lst = transferDao.SearchInvoiceNonGroupByCustomerAndPlant(SearchDto);
            ISheet sheet_vl06f = workbook.GetSheet("VL06F"); // VL06F 
            foreach (InvoiceZZ24Dto item in lst)
            {
                InvoiceDetailDto invoiceDetailDto = null;
                for (int j = 1; j <= sheet_vl06f.LastRowNum; j++)
                {
                    invoiceDetailDto = new InvoiceDetailDto();
                    IRow row = sheet_vl06f.GetRow(j);
                    ICell cell_DeliveryNo = row != null ? row.GetCell(0) : null; // DeliveryNo
                    ICell cell_Material = row != null ? row.GetCell(7) : null; // Material
                    ICell cell_Description = row != null ? row.GetCell(8) : null; // Description
                    ICell cell_DeliveryQty = row != null ? row.GetCell(19) : null; // DeliveryQty
                    if (!string.IsNullOrEmpty(item.DeliveryNo) && item.DeliveryNo.Equals(Utils.GetFormattedCellValue(cell_DeliveryNo)))
                    {
                        /* Binding Invoice Invoice Detail Dto */
                        invoiceDetailDto.DeliveryNo = Utils.GetFormattedCellValue(cell_DeliveryNo);
                        invoiceDetailDto.Material = Utils.GetFormattedCellValue(cell_Material);
                        invoiceDetailDto.Descriptions = Utils.GetFormattedCellValue(cell_Description);
                        invoiceDetailDto.DeliveryQty = !string.IsNullOrEmpty(Utils.GetFormattedCellValue(cell_DeliveryQty)) ? Utils.GetFormattedCellValue(cell_DeliveryQty).Trim().Replace(".","") : "0";
                        invoiceDetailDto.Id = Guid.NewGuid();
                        invoiceDetailDto.TransferNumber = transferNumber;
                        invoiceDetailDto.CreateBy = userId;
                        invoiceDetailDto.UpdateBy = userId;
                        if (int.Parse(invoiceDetailDto.DeliveryQty) > 0)
                        {
                            transferDao.InsertInvoiceDetail(invoiceDetailDto);
                        }
                    }
                }
            }
            int udt = transferDao.UpdateInvoiceNoAndCustCodeDetails();
            mem.Close();
            // RETURN RESULT MESSAGE
            if (count > 0)
            {
                result.code = Constant.SUCCESS;
                result.number = count;
                result.message = Constant.MESSAGE_SUCCESS;
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.NO_ROW_SAVE;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search Invoice Detail
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceDetailDto> ViewInvoiceDetail(InvoiceZZ24Dto json)
        {
            IEnumerable<InvoiceDetailDto> lst = transferDao.SearchInvoiceDetail(json);
            return lst;
        }

        /// <summary>
        /// Share All Delivery
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto ShareAllInvoice(InvoiceZZ24Dto invoice)
        {
            ResultMessageDto rs = new ResultMessageDto();            
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));            
            int countTotal = 0;
            string[] bankInfo = new string[3];
            string bodyHtml = string.Empty;
            IEnumerable<InvoiceZZ24Dto> invoiceList = transferDao.GetInvoiceByDelivery(invoice);
            foreach (InvoiceZZ24Dto item in invoiceList)
            {
                string plant = string.Empty;
                bodyHtml = SetBankInfo(item.Plant);
                if (item.Plant.Equals(Constant.VN61))
                {
                    plant = VN61_DKSH_BD;                    
                }
                else if (item.Plant.Equals(Constant.VN62))
                {
                    plant = VN62_DKSH_DN;
                }
                else if (item.Plant.Equals(Constant.VN63))
                {
                    plant = VN63_DKSH_HN;
                }
                else if (item.Plant.Equals(Constant.VN64))
                {
                    plant = VN64_VIME;
                }
                else if (item.Plant.Equals(Constant.VN65))
                {
                    plant = VN65_DAPHA;
                }
                else if (item.Plant.Equals(Constant.VN66))
                {
                    plant = VN66;
                }
                string filePdf = filePath + item.CustName + "_" + plant + Constant.PDF_EXTENSION;
                try
                {
                    bool flag = false;
                    ExportPDF(item);
                    invoice.SendMethod = "email";
                    flag = Utils.SendMailOutlookWithInterop(item.Email, filePdf, Constant.TRANSFER_NOTIFI_SUBJECT, bodyHtml);
                    if (flag) { item.ShareStatus = "1"; } else { item.ShareStatus = "0"; }
                    countTotal += transferDao.UpdateShareStatus(item);
                }
                catch(Exception e)
                {
                    rs.code = Constant.ERROR;
                    rs.message = e.Message;
                    return rs;
                }                
            }
            if (countTotal > 0)
            {
                rs.number = countTotal;
                rs.code = Constant.SUCCESS;
                rs.message = string.Format(Constant.SEND_MESSAGE, countTotal);
            }
            return rs;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send Invoice
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto SendInvoice(InvoiceZZ24Dto json)
        {
            ResultMessageDto rs = new ResultMessageDto();
            return SendInvoiceByMethod(json);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send Invoice By Method
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        private ResultMessageDto SendInvoiceByMethod(InvoiceZZ24Dto pr)
        {
            ResultMessageDto result = new ResultMessageDto();
            SendMailDto mailDto = new SendMailDto();
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            string filePdf = ExportPDF(pr);
            string name = pr.CustName + "_" + SetPlantName(pr.Plant);
            // SET MAIL OBJECT TO
            mailDto.title = string.Format(Constant.TRANSFER_NOTIFI_SUBJECT, name);
            mailDto.attachment = new List<string>();
            mailDto.attachment.Add(filePath + filePdf + Constant.PDF_EXTENSION);
            if (string.IsNullOrEmpty(pr.Content))
            {
                mailDto.content = Constant.MAIL_CONTENT_TRANSFER_NOTIFICATION;
            }
            else
            {
                mailDto.content = pr.Content;
            }
            mailDto.fromAddress = pr.Sender;
            mailDto.fromPassword = pr.Password;
            List<string> CCList = null;
            if (pr.CC != null && pr.CC.Count() > 0)
            {
                CCList = pr.CC.Split(';').ToList();
            }          
            mailDto.ccAddress = new List<string>();
            if (!string.IsNullOrEmpty(pr.CC) && CCList != null)
            {
                foreach (string item in CCList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        mailDto.ccAddress.Add(item.Trim());
                    }
                }
            }            
            List <string> receiverList = pr.Receiver.Split(';').ToList();
            mailDto.recipientList = new List<string>();
            if (pr.SendMethod.Equals("email"))
            {
                foreach(string email in receiverList)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        mailDto.recipientList.Add(email.Trim());
                    }          
                }
                result = SendMailExchangeService.SendMailSMTP(mailDto);
            }
            if (result.flag) { pr.ShareStatus = "1"; } else { pr.ShareStatus = "0"; }
            transferDao.UpdateShareStatus(pr);
            return result;
        }

        /// <summary>
        /// Set Bank Info
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        private string SetBankInfo(string plant)
        {
            string[] bankInfo = new string[3];
            if (plant.Equals(Constant.VN61))
            {
                bankInfo[0] = Constant.VN61_COMPANY;
                bankInfo[1] = Constant.VN61_BANK_ACCOUNT;
                bankInfo[2] = Constant.VN61_BANK_NAME;
            }
            else if (plant.Equals(Constant.VN62))
            {
                bankInfo[0] = Constant.VN62_COMPANY;
                bankInfo[1] = Constant.VN62_BANK_ACCOUNT;
                bankInfo[2] = Constant.VN62_BANK_NAME;
            }
            else if (plant.Equals(Constant.VN63))
            {
                bankInfo[0] = Constant.VN63_COMPANY;
                bankInfo[1] = Constant.VN63_BANK_ACCOUNT;
                bankInfo[2] = Constant.VN63_BANK_NAME;
            }
            else if (plant.Equals(Constant.VN64))
            {
                bankInfo[0] = Constant.VN64_COMPANY;
                bankInfo[1] = Constant.VN64_BANK_ACCOUNT;
                bankInfo[2] = Constant.VN64_BANK_NAME;
            }
            else if (plant.Equals(Constant.VN65))
            {
                bankInfo[0] = Constant.VN65_COMPANY;
                bankInfo[1] = Constant.VN65_BANK_ACCOUNT;
                bankInfo[2] = Constant.VN65_BANK_NAME;
            }
            else if (plant.Equals(Constant.VN66))
            {
                bankInfo[0] = Constant.VN66_COMPANY;
                bankInfo[1] = Constant.VN66_BANK_ACCOUNT;
                bankInfo[2] = Constant.VN66_BANK_NAME;
            }
            string bodyHtml = string.Format(Constant.TRANSFER_NOTIFI_BODY, bankInfo);
            return bodyHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        private string SetPlantName(string plant)
        {
            string str = string.Empty;
            if (plant.Equals(Constant.VN61))
            {
                str = VN61_DKSH_BD;
            }
            else if (plant.Equals(Constant.VN62))
            {
                str = VN62_DKSH_DN;
            }
            else if (plant.Equals(Constant.VN63))
            {
                str = VN63_DKSH_HN;
            }
            else if (plant.Equals(Constant.VN64))
            {
                str = VN64_VIME;
            }
            else if (plant.Equals(Constant.VN65))
            {
                str = VN65_DAPHA;
            }
            else if (plant.Equals(Constant.VN66))
            {
                str = VN66;
            }
            return str;
        }

        /// <summary>
        /// Download Invoices
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto DownloadInvoices(InvoiceZZ24Dto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            List<string> listPath = new List<string>();
            json.IsVN66 = true;
            IEnumerable <InvoiceZZ24Dto> invoiceList = transferDao.SearchInvoice(json);
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            foreach (InvoiceZZ24Dto item in invoiceList)
            {
                string fileName = filePath + ExportPDF(item);
                string fileNamePdf = fileName + Constant.PDF_EXTENSION;
                string fileNameXLSX = fileName + Constant.XLSX_EXTENSION;
                listPath.Add(fileNamePdf);
                listPath.Add(fileNameXLSX);
            }
            result.code = invoiceList.Count().ToString();
            result.filePath = Utils.CreateZipFile(invoiceList.Count(), filePath, listPath);
            foreach(string lst in listPath)
            {
                File.Delete(lst);
            }
            return result;
        }

        /// <summary>
        /// Delete Invoice
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteInvoice(InvoiceZZ24Dto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result.number = transferDao.DeleteInvoice(json);
            if (result.number > 0)
            {
                result.code = Constant.SUCCESS;
                result.message = Constant.MESSAGE_DELELE;
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.MESSAGE_NO_ROW_DELELE;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public string ExportExcel(InvoiceZZ24Dto invoice)
        {
            InvoiceZZ24Dto ivc = transferDao.GetInvoiceZZ24ByDelivery(invoice);
            if (invoice != null)
            {
                string wb = GetTemplateByEntity(invoice);
                return wb;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert PDF
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="fileNm"></param>
        /// <returns></returns>
        public ResultMessageDto ConvertPDF(string fileLocation, string fileNm)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                ExcelToPdf excelToPdf = new ExcelToPdf();
                string outPath = HttpContext.Current.Server.MapPath("~/FilesUpload/InvoiceToBank/") + fileNm + Constant.PDF_EXTENSION;
                excelToPdf.ConvertFile(fileLocation, outPath);
                result.filePath = outPath;
                result.FileNm = fileNm;
                result.code = Constant.SUCCESS;
                result.message = Constant.MESSAGE_CONVERT_SUCCESS;
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Transfer Notifi Log
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceZZ24Dto> TransferNotifiLog(InvoiceZZ24Dto json)
        {
            IEnumerable<InvoiceZZ24Dto> lst = transferDao.TransferNotifiLog(json);
            return lst;
        }

        /// <summary>
        /// Export PDF IsDelete
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string ExportPDFIsDelete(InvoiceZZ24Dto param)
        {
            string fileNm = string.Empty;
            InvoiceZZ24Dto invoiceObj = transferDao.GetInvoiceZZ24ByDeliveryIsDelete(param);
            if (invoiceObj != null)
            {
                fileNm = GetTemplateByEntityTransferLog(invoiceObj);
            }
            return fileNm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GetTemplateByEntityTransferLog(InvoiceZZ24Dto entity)
        {
            IWorkbook workbook;
            string fileName = string.Empty;
            string extension = Constant.EXCEL_XLSX;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            var fileStream = filePath + entity.Plant.ToUpper() + ".xlsx";
            using (FileStream file = new FileStream(fileStream, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (Constant.EXCEL_XLSX.Equals(extension))
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("This format is not supported");
                }
                string[] d = new string[3];
                d[0] = DateTime.Now.Day.ToString();
                d[1] = DateTime.Now.Month.ToString();
                d[2] = DateTime.Now.Year.ToString();

                ISheet sheet = workbook.GetSheet(entity.Plant.ToUpper());
                var color = new XSSFColor(new byte[] { 3, 15, 1 });
                var font = (XSSFFont)workbook.CreateFont();
                font.SetColor(color);
                font.IsBold = true;
                font.FontHeightInPoints = 14;
                var style = (XSSFCellStyle)workbook.CreateCellStyle();
                var border = (XSSFCellStyle)workbook.CreateCellStyle();
                var mergeStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                mergeStyle.Alignment = HorizontalAlignment.CenterSelection;
                mergeStyle.VerticalAlignment = VerticalAlignment.Center;
                style.SetFont(font);
                border.BorderRight = BorderStyle.Thin;
                border.BorderLeft = BorderStyle.Thin;
                border.BorderBottom = BorderStyle.Thin;
                border.BorderTop = BorderStyle.Thin;
                border.VerticalAlignment = VerticalAlignment.Center;
                border.Alignment = HorizontalAlignment.Center;
                border.SetFont(font);
                /* Binding detail invoice */
                int rowIdx = 17;
                //int mergeRow = 0;
                IEnumerable<InvoiceDetailDto> invoiceDetailUngroup = transferDao.SearchInvoiceDetailUngroupIsDelete(entity);
                IEnumerable<InvoiceDetailDto> invoiceDetailList = transferDao.SearchInvoiceDetailIsDelete(entity);
                double SumAmount = 0;
                IRow row = sheet.CreateRow(rowIdx);
                IEnumerable<InvoiceDetailDto> CountInvoiceByCustomerAndPlant = transferDao.CountInvoiceByCustomerAndPlantIsDelete(entity);
                // Sum invoice detail
                if (CountInvoiceByCustomerAndPlant != null && CountInvoiceByCustomerAndPlant.Count() > 0)
                {
                    foreach (InvoiceDetailDto invoice in CountInvoiceByCustomerAndPlant)
                    {
                        SumAmount += Convert.ToDouble(invoice.Amt);
                    }

                }
                // Binding invoice details
                if (invoiceDetailUngroup.Count() > 0)
                {
                    //mergeRow = invoiceDetailList.Count() - 1;
                    foreach (InvoiceDetailDto item in invoiceDetailUngroup)
                    {
                        if (rowIdx == 17)
                        {
                            sheet.GetRow(rowIdx).CreateCell(1).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(2).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(3).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(4).SetCellValue(item.Descriptions); // Tên sản phẩm
                            sheet.GetRow(rowIdx).GetCell(4).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(5).SetCellValue(item.DeliveryQty); // Số lượng
                            sheet.GetRow(rowIdx).GetCell(5).CellStyle = border;
                        }
                        else
                        {
                            sheet.CreateRow(rowIdx).CreateCell(4).SetCellValue(item.Descriptions); // Tên sản phẩm
                            sheet.GetRow(rowIdx).CreateCell(1).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(2).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(3).CellStyle = border;
                            sheet.GetRow(rowIdx).GetCell(4).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(5).SetCellValue(item.DeliveryQty); // Số lượng                            
                            sheet.GetRow(rowIdx).GetCell(5).CellStyle = border;
                        }

                        rowIdx += 1;
                    }
                    // Merge rows by invoice
                    int idx = 0;
                    foreach (InvoiceDetailDto obj in CountInvoiceByCustomerAndPlant)
                    {
                        obj.Plant = entity.Plant;
                        obj.CreateDate = entity.CreateDate;
                        IEnumerable<InvoiceDetailDto> mergeRegionInvoice = transferDao.MergeInvoiceByDeliveryIsDelete(obj);
                        if (idx == 0)
                        {
                            CellRangeAddress mergeInvoiceNo = new CellRangeAddress(17, 17 + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 1, 1);
                            CellRangeAddress mergeDate = new CellRangeAddress(17, 17 + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 2, 2);
                            CellRangeAddress mergeTotal = new CellRangeAddress(17, 17 + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 3, 3);
                            sheet.AddMergedRegion(mergeInvoiceNo);
                            sheet.AddMergedRegion(mergeDate);
                            sheet.AddMergedRegion(mergeTotal);
                            row.GetCell(1).SetCellValue(!string.IsNullOrEmpty(obj.RedInvoice) ? obj.RedInvoice + " - " + obj.InvoiceNo : "" + obj.InvoiceNo); // Số hóa đơn
                            row.GetCell(2).SetCellValue(obj.InvoiceDate); // Ngày hóa đơn
                            row.GetCell(3).SetCellValue(obj.Amount); // Tổng tiền
                            //row.GetCell(1).CellStyle = mergeStyle;
                            //row.GetCell(2).CellStyle = mergeStyle;
                            //row.GetCell(3).CellStyle = mergeStyle;
                        }
                        else
                        {
                            CellRangeAddress mergeInvoiceNo = new CellRangeAddress(17 + idx, 17 + idx + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 1, 1);
                            CellRangeAddress mergeDate = new CellRangeAddress(17 + idx, 17 + idx + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 2, 2);
                            CellRangeAddress mergeTotal = new CellRangeAddress(17 + idx, 17 + idx + mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant, 3, 3);
                            sheet.AddMergedRegion(mergeInvoiceNo);
                            sheet.AddMergedRegion(mergeDate);
                            sheet.AddMergedRegion(mergeTotal);
                            sheet.GetRow(17 + idx).CreateCell(1).SetCellValue(!string.IsNullOrEmpty(obj.RedInvoice) ? obj.RedInvoice + " - " + obj.InvoiceNo : "" + obj.InvoiceNo); // Số hóa đơn
                            sheet.GetRow(17 + idx).CreateCell(2).SetCellValue(obj.InvoiceDate); // Ngày hóa đơn
                            sheet.GetRow(17 + idx).CreateCell(3).SetCellValue(obj.Amount); // Tổng tiền                            
                            sheet.GetRow(17 + idx).GetCell(1).CellStyle = border;
                            sheet.GetRow(17 + idx).GetCell(2).CellStyle = border;
                            sheet.GetRow(17 + idx).GetCell(3).CellStyle = border;
                            //sheet.GetRow(17 + idx).GetCell(1).CellStyle = mergeStyle;
                            //sheet.GetRow(17 + idx).GetCell(2).CellStyle = mergeStyle;
                            //sheet.GetRow(17 + idx).GetCell(3).CellStyle = mergeStyle;
                        }
                        idx += mergeRegionInvoice.ElementAt(0).CountByCustomerAndPlant + 1;
                        mergeRegionInvoice = null;
                    }
                    // Total row
                    sheet.CreateRow(17 + idx).CreateCell(1).SetCellValue("Tổng tiền");
                    sheet.GetRow(17 + idx).GetCell(1).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(2).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(4).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(5).CellStyle = border;
                    sheet.GetRow(17 + idx).CreateCell(3).SetCellValue(string.Format("{0:n0}", SumAmount));
                    sheet.GetRow(17 + idx).GetCell(3).CellStyle = border;
                }
                string address = entity.Addrs + " " + entity.District;
                if (entity.Plant.Equals(Constant.VN61))
                {
                    fileName = entity.CustName + "_" + VN61_DKSH_BD;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    string currDateHCM = string.Format(Constant.HCMC_DATETIME, d);
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHCM);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN61_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN61_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN61_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN62))
                {
                    fileName = entity.CustName + "_" + VN62_DKSH_DN;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    string currDateDaNang = string.Format(Constant.DA_NANG_DATETIME, d);
                    sheet.CreateRow(6).CreateCell(4).SetCellValue(currDateDaNang);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN62_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN62_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN62_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.VN62_TOLL_FREE);
                }
                else if (entity.Plant.Equals(Constant.VN63))
                {
                    fileName = entity.CustName + "_" + VN63_DKSH_HN;
                    string currDateHN = string.Format(Constant.HANOI_DATETIME, d);
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHN);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.VN63_ADDRESS);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN63_COMPANY_ADDRESS);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN64))
                {
                    fileName = entity.CustName + "_" + VN64_VIME;
                    string currDateHCM = string.Format(Constant.HCMC_DATETIME, d);
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    sheet.GetRow(13).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(13).GetCell(2).CellStyle = style;
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHCM);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN64_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN64_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN64_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN65))
                {
                    fileName = entity.CustName + "_" + VN65_DAPHA;
                    sheet.GetRow(11).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(11).GetCell(2).CellStyle = style;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(address);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    string currDate = string.Format(Constant.DA_NANG_DATETIME, d);
                    sheet.CreateRow(5).CreateCell(4).SetCellValue(currDate);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN65_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN65_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN65_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                else if (entity.Plant.Equals(Constant.VN66))
                {
                    fileName = entity.CustName + "_" + VN66;
                    sheet.GetRow(12).CreateCell(2).SetCellValue(entity.CustName);
                    sheet.GetRow(12).GetCell(2).CellStyle = style;
                    string currDateHCM = string.Format(Constant.HCMC_DATETIME, d);
                    sheet.CreateRow(8).CreateCell(4).SetCellValue(currDateHCM);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TITLE_TRANSFER);
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TEN_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN66_COMPANY);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.SO_TAI_KHOAN);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN66_BANK_ACCOUNT);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NGAN_HANG);
                    sheet.GetRow(rowIdx).CreateCell(2).SetCellValue(Constant.VN66_BANK_NAME);
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.UY_NHIEM_CHI);
                    rowIdx += 2;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.FREE_FAX);
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    rowIdx += 1;
                    sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.TOLL_FREE_1800545402);
                }
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.ANNOUNCEMENT_LINE01);
                //sheet.CreateRow(rowIdx + 1).CreateCell(1).SetCellValue(Constant.ANNOUNCEMENT_LINE02);
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE01);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_INFO);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE02);
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE03);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE04);
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE05);
                rowIdx += 2;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.THANK_YOU);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                sheet.AutoSizeColumn(4);
                fileName = Utils.ReplaceSpecialChar(fileName);
                string pdfFile = filePath + fileName + Constant.PDF_EXTENSION;
                string excelFile = filePath + fileName + "." + Constant.EXCEL_XLSX;
                if (File.Exists(excelFile))
                {
                    File.Delete(excelFile);
                }
                FileStream fs = new FileStream(excelFile, FileMode.CreateNew);
                workbook.Write(fs);
                fs.Close();
                file.Close();
                ExcelToPdf excelToPdf = new ExcelToPdf();
                excelToPdf.ConvertFile(excelFile, pdfFile);
                //File.Delete(excelFile);
            }

            return fileName;
        }

        /// <summary>
        /// Export Excel Transfer Log
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public string ExportExcelTransferLog(InvoiceZZ24Dto invoice)
        {
            string wb = string.Empty;
            InvoiceZZ24Dto ivc = transferDao.GetInvoiceZZ24ByDeliveryIsDelete(invoice);
            if (invoice != null)
            {
                wb = GetTemplateByEntityTransferLog(invoice);                
            }
            return wb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceDetailDto> ViewInvoiceDetailTransferLog(InvoiceZZ24Dto json)
        {
            IEnumerable<InvoiceDetailDto> lst = transferDao.SearchInvoiceDetailIsDelete(json);
            return lst;
        }
    }
}