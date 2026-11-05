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
    public class TransferNotificationProvinceServiceImpl : TransferNotificationProvinceService
    {
        private static string VN61_DKSH_BD = "DKSH_BD";
        private static string VN64_VIME = "Vime";
        TransferNotificationProvinceDao transferDao = new TransferNotificationProvinceDao();

        public string ExportPDF(TransferNotificationProvinceDto dto)
        {
            IEnumerable<TransferNotificationProvinceDto> invoiceDetail = transferDao.GetInvoinceDetailProvince(dto);
            if (invoiceDetail != null)
            {
                string wb = GetTemplateByEntity(dto);
                return wb;
            }
            return string.Empty;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search Invoice
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<TransferNotificationProvinceDto> SearchInvoice(TransferNotificationProvinceDto dto)
        {
            if (string.IsNullOrEmpty(dto.CreateDateFrom))
            {
                dto.CreateDateFrom = DateTime.Now.ToString("dd-MM-yyyy");
            }
            if (string.IsNullOrEmpty(dto.CreateDateTo))
            {
                dto.CreateDateTo = dto.CreateDateFrom;
            }
            IEnumerable<TransferNotificationProvinceDto> tfProvince = transferDao.SearchInvoice(dto);
            return tfProvince;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Invoince Detail Province
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<TransferNotificationProvinceDto> GetInvoinceDetailProvince(TransferNotificationProvinceDto dto)
        {
            IEnumerable<TransferNotificationProvinceDto> tfProvince = transferDao.GetInvoinceDetailProvince(dto);
            return tfProvince;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload And Process
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadAndProcess(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook;
            int count = 0;
            string extension = Path.GetExtension(aFile.FileName).Replace(".", "");
            string transferNumber = Utils.GenerateRandomNo().ToString();
            MemoryStream mem = new MemoryStream();
            mem.SetLength(aFile.ContentLength);
            aFile.InputStream.Read(mem.GetBuffer(), 0, aFile.ContentLength);
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
            int delCnt = transferDao.DeleteTransferNotificationProvince();
            ISheet sheet_va05 = workbook.GetSheet("VA05"); // VA05
            TransferNotificationProvinceDto tfProvinceDto = null;
            for (int i = 1; i <= sheet_va05.LastRowNum; i++)
            {
                tfProvinceDto = new TransferNotificationProvinceDto();
                IRow row = sheet_va05.GetRow(i);
                ICell cell_SONumber = row.GetCell(0) != null ? row.GetCell(0) : row.CreateCell(0); // SO Number
                ICell cell_CustCode = row.GetCell(1) != null ? row.GetCell(1) : row.CreateCell(1); // CustCode
                ICell cell_ProductName = row.GetCell(3) != null ? row.GetCell(3) : row.CreateCell(3); // Product name
                ICell cell_OrderQty = row.GetCell(4) != null ? row.GetCell(4) : row.CreateCell(4); // Order Qty
                ICell cell_Amount = row.GetCell(5) != null ? row.GetCell(5) : row.CreateCell(5); // Total amount (VAT)
                ICell cell_OrderDate = row.GetCell(6) != null ? row.GetCell(6) : row.CreateCell(6); // Order Date
                ICell cell_Plant = row.GetCell(7) != null ? row.GetCell(7) : row.CreateCell(7); // Plant

                tfProvinceDto.SONumber = Utils.GetFormattedCellValue(cell_SONumber);
                tfProvinceDto.CustCode = Utils.GetFormattedCellValue(cell_CustCode);
                tfProvinceDto.ProductName = Utils.GetFormattedCellValue(cell_ProductName);
                tfProvinceDto.OrderQty = Utils.GetFormattedCellValue(cell_OrderQty);
                tfProvinceDto.Amount = Utils.GetFormattedCellValue(cell_Amount);
                tfProvinceDto.OrderDate = Utils.GetFormattedCellValue(cell_OrderDate);
                tfProvinceDto.Plant = Utils.GetFormattedCellValue(cell_Plant);
                tfProvinceDto.Id = Guid.NewGuid();
                // Save into Tranfer Notification Province
                count += transferDao.InsertTransferNotificationProvince(tfProvinceDto);
            }
            // close memory stream
            mem.Close();
            // RETURN RESULT MESSAGE
            if (count > 0)
            {
                result.code = Constant.SUCCESS;
                result.number = count;
                result.message = string.Format(Constant.PROCESS_MESSAGE, count);
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
        /// Upload Email
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadEmail(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook;
            int count = 0;
            string extension = Path.GetExtension(aFile.FileName).Replace(".", "");
            string transferNumber = Utils.GenerateRandomNo().ToString();
            MemoryStream mem = new MemoryStream();
            mem.SetLength(aFile.ContentLength);
            aFile.InputStream.Read(mem.GetBuffer(), 0, aFile.ContentLength);
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
            CustomerEmailDto custEmailDto = null;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                custEmailDto = new CustomerEmailDto();
                IRow row = sheet.GetRow(i);
                ICell cell_CustCode = row.GetCell(1) != null ? row.GetCell(1) : row.CreateCell(1); // CustCode
                ICell cell_Email1 = row.GetCell(5) != null ? row.GetCell(5) : row.CreateCell(5); // 
                ICell cell_Email2 = row.GetCell(6) != null ? row.GetCell(6) : row.CreateCell(6); // 
                ICell cell_Email3 = row.GetCell(7) != null ? row.GetCell(7) : row.CreateCell(7); // 
                ICell cell_Phone = row.GetCell(8) != null ? row.GetCell(8) : row.CreateCell(8); // 

                custEmailDto.CustCode = Utils.GetFormattedCellValue(cell_CustCode);
                custEmailDto.Email1 = Utils.GetFormattedCellValue(cell_Email1);
                custEmailDto.Email2 = Utils.GetFormattedCellValue(cell_Email2);
                custEmailDto.Email3 = Utils.GetFormattedCellValue(cell_Email3);
                custEmailDto.Phone = Utils.GetFormattedCellValue(cell_Phone);
                custEmailDto.Id = Guid.NewGuid();

                // Save into Tranfer Notification Province
                count += transferDao.InsertCustomerEmails(custEmailDto);

            }

            mem.Close();
            // RETURN RESULT MESSAGE
            if (count > 0)
            {
                result.code = Constant.SUCCESS;
                result.number = count;
                result.message = string.Format(Constant.PROCESS_MESSAGE, count);
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
        /// Binding data to template by entity
        /// </summary>
        private string GetTemplateByEntity(TransferNotificationProvinceDto entity)
        {
            IWorkbook workbook;
            string fileName = string.Empty;
            string extension = Constant.EXCEL_XLSX;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            var fileStream = filePath + entity.Plant.ToUpper() + Constant.PROVINCE + ".xlsx";
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
                IEnumerable<TransferNotificationProvinceDto> invoiceDetailList = transferDao.GetInvoinceDetailProvince(entity);
                IRow row = sheet.CreateRow(rowIdx);
                IEnumerable<TransferNotificationProvinceDto> CountInvoiceBySONumberList = transferDao.CountInvoiceBySONumber(entity);
                TransferNotificationProvinceDto sumAmountByCustCode = transferDao.SumAmountByCustCode(entity);
                // Binding invoice details
                if (invoiceDetailList.Count() > 0)
                {
                    //mergeRow = invoiceDetailList.Count() - 1;
                    foreach (TransferNotificationProvinceDto item in invoiceDetailList)
                    {
                        if (rowIdx == 17)
                        {
                            sheet.GetRow(rowIdx).CreateCell(1).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(2).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(3).SetCellValue(item.ProductName); // Tên sản phẩm
                            sheet.GetRow(rowIdx).GetCell(3).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(4).SetCellValue(item.OrderQty); // Số lượng
                            sheet.GetRow(rowIdx).GetCell(4).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(5).SetCellValue(item.Amount); // Số tiền
                            sheet.GetRow(rowIdx).GetCell(5).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(6).CellStyle = border;
                        }
                        else
                        {
                            sheet.CreateRow(rowIdx).CreateCell(3).SetCellValue(item.ProductName); // Tên sản phẩm
                            sheet.GetRow(rowIdx).CreateCell(1).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(2).CellStyle = border;
                            sheet.GetRow(rowIdx).GetCell(3).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(4).SetCellValue(item.OrderQty); // Số lượng
                            sheet.GetRow(rowIdx).GetCell(4).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(5).SetCellValue(item.Amount); // Số tiền
                            sheet.GetRow(rowIdx).GetCell(5).CellStyle = border;
                            sheet.GetRow(rowIdx).CreateCell(6).CellStyle = border;
                        }

                        rowIdx += 1;
                    }
                    //Merge rows by invoice
                    int idx = 0;
                    foreach (TransferNotificationProvinceDto obj in CountInvoiceBySONumberList)
                    {
                        if (idx == 0)
                        {
                            CellRangeAddress mergeInvoiceNo = new CellRangeAddress(17, 17 + obj.CountInvoiceBySONumber, 1, 1);
                            CellRangeAddress mergeDate = new CellRangeAddress(17, 17 + obj.CountInvoiceBySONumber, 2, 2);
                            CellRangeAddress mergeAmount = new CellRangeAddress(17, 17 + obj.CountInvoiceBySONumber, 6, 6);
                            sheet.AddMergedRegion(mergeInvoiceNo);
                            sheet.AddMergedRegion(mergeDate);
                            sheet.AddMergedRegion(mergeAmount);
                            row.GetCell(1).SetCellValue(obj.SONumber); // Số hóa đơn
                            row.GetCell(2).SetCellValue(obj.OrderDate); // Ngày hóa đơn
                            row.GetCell(6).SetCellValue(obj.Amount); // Amount
                            row.GetCell(1).CellStyle = border;
                            row.GetCell(2).CellStyle = border;
                            row.GetCell(6).CellStyle = border;
                        }
                        else
                        {
                            CellRangeAddress mergeInvoiceNo = new CellRangeAddress(17 + idx, 17 + idx + obj.CountInvoiceBySONumber, 1, 1);
                            CellRangeAddress mergeDate = new CellRangeAddress(17 + idx, 17 + idx + obj.CountInvoiceBySONumber, 2, 2);
                            CellRangeAddress mergeAmount = new CellRangeAddress(17 + idx, 17 + idx + obj.CountInvoiceBySONumber, 6, 6);
                            sheet.AddMergedRegion(mergeInvoiceNo);
                            sheet.AddMergedRegion(mergeDate);
                            sheet.AddMergedRegion(mergeAmount);
                            sheet.GetRow(17 + idx).CreateCell(1).SetCellValue(obj.SONumber); // Số hóa đơn                            
                            sheet.GetRow(17 + idx).CreateCell(2).SetCellValue(obj.OrderDate); // Ngày hóa đơn
                            sheet.GetRow(17 + idx).CreateCell(6).SetCellValue(obj.Amount); // Amount
                            sheet.GetRow(17 + idx).GetCell(1).CellStyle = border;
                            sheet.GetRow(17 + idx).GetCell(2).CellStyle = border;
                            sheet.GetRow(17 + idx).GetCell(6).CellStyle = border;
                        }
                        idx += obj.CountInvoiceBySONumber + 1;
                    }
                    int noRowMerge = rowIdx;
                    CellRangeAddress mergeTotal = new CellRangeAddress(noRowMerge, noRowMerge, 1, 5);
                    sheet.CreateRow(noRowMerge);
                    sheet.AddMergedRegion(mergeTotal);
                    sheet.GetRow(noRowMerge).CreateCell(1).SetCellValue("Tổng tiền");
                    sheet.GetRow(noRowMerge).CreateCell(6).SetCellValue(sumAmountByCustCode.Amount);
                    sheet.GetRow(noRowMerge).GetCell(1).CellStyle = border;
                    sheet.GetRow(noRowMerge).CreateCell(2).CellStyle = border;
                    sheet.GetRow(noRowMerge).CreateCell(3).CellStyle = border;
                    sheet.GetRow(noRowMerge).CreateCell(4).CellStyle = border;
                    sheet.GetRow(noRowMerge).CreateCell(5).CellStyle = border;
                    sheet.GetRow(noRowMerge).GetCell(6).CellStyle = border;
                }
                string address = entity.Addr + " " + entity.District;
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
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE03_PROVINCE);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE04);
                rowIdx += 1;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.NOTE_LINE05);
                rowIdx += 2;
                sheet.CreateRow(rowIdx).CreateCell(1).SetCellValue(Constant.THANK_YOU);
                sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                sheet.AutoSizeColumn(3);
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
                File.Delete(excelFile);
            }
            return fileName;
        }

        /// <summary>
        /// Download All Invoices
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto DownloadInvoices(TransferNotificationProvinceDto jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            List<string> listPath = new List<string>();
            IEnumerable<TransferNotificationProvinceDto> invoiceList = transferDao.SearchInvoice(jsonParam);
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            foreach (TransferNotificationProvinceDto item in invoiceList)
            {
                string fileName = filePath + ExportPDF(item) + Constant.PDF_EXTENSION;
                listPath.Add(fileName);
            }
            result.code = invoiceList.Count().ToString();
            result.filePath = Utils.CreateZipFile(invoiceList.Count(), filePath, listPath);
            foreach (string lst in listPath)
            {
                File.Delete(lst);
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send Invoice
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto SendInvoice(TransferNotificationProvinceDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            return SendInvoiceByMethod(dto);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send Invoice By Method
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        private ResultMessageDto SendInvoiceByMethod(TransferNotificationProvinceDto pr)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                SendMailDto mailDto = new SendMailDto();
                string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
                string filePdf = ExportPDF(pr);
                // SET MAIL OBJECT TO
                mailDto.title = Constant.TRANSFER_NOTIFI_SUBJECT;
                mailDto.attachment = new List<string>();
                mailDto.attachment.Add(filePath + filePdf + Constant.PDF_EXTENSION);
                //mailDto.content = SetBankInfo(pr.Plant);
                if (string.IsNullOrEmpty(pr.Content))
                {
                    mailDto.content = Constant.MAIL_CONTENT_PROVINCE_TRANSFER_NOTIFICATION;
                }
                else
                {
                    mailDto.content = pr.Content;
                }
                mailDto.fromAddress = pr.Sender;
                mailDto.fromPassword = pr.Password;
                List<string> ccList = new List<string>();
                ccList.Add(Constant.NGOC_NHATHANH_NGUYEN_MAIL);
                ccList.Add(Constant.DAO_THIANH_NGUYEN_MAIL);
                ccList.Add(Constant.CCC_PROVINCE_SGN_MAIL);
                ccList.Add(Constant.CCC_PROVINCE_VIME_MAIL);
                mailDto.ccAddress = ccList;
                List<string> receiverList = pr.Receiver.Split(';').ToList();
                mailDto.recipientList = new List<string>();
                if (pr.SendMethod.Equals("email"))
                {
                    foreach (string email in receiverList)
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
            }
            catch(UnauthorizedAccessException authex)
            {
                result.code = Constant.ERROR;
                result.message = authex.Message;
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }
    }
}