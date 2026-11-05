using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using DKSHApplication.Common;
using NPOI.HSSF.UserModel;
using DKSHApplication.DAO;

namespace DKSHApplication.Service
{
    public class UpSaleCrossSaleServiceImpl : UpSaleCrossSaleService
    {
        CrossSaleUpSaleDao CSUSDao = new CrossSaleUpSaleDao();
        public IEnumerable<CrossSaleUpSaleReportDto> SearchUpSaleCrossSale(CrossSaleUpSaleReportDto dto)
        {
            IEnumerable<CrossSaleUpSaleReportDto> result = CSUSDao.SearchUpSaleCrossSale(dto);
            return result;
        }

        /// <summary>
        /// Get Group Saleman PO Text
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalemanPOTextDto> GetGroupSalemanPOText()
        {
            CrossSaleUpSaleReportDto dto = new CrossSaleUpSaleReportDto();
            IEnumerable<SalemanPOTextDto> result = CSUSDao.GetGroupSalemanPOText(dto).GroupBy(c => c.GroupCode).Select(g => g.First());
            return result;
        }

        /// <summary>
        /// Get Saleman PO
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalemanPOTextDto> GetSalemanPO()
        {
            CrossSaleUpSaleReportDto dto = new CrossSaleUpSaleReportDto();
            IEnumerable<SalemanPOTextDto> result = CSUSDao.GetGroupSalemanPOText(dto);
            return result;
        }

        /// <summary>
        /// Save Data Sale From Inhouse
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto SaveDataSale(HttpPostedFileBase file, DataSaleCSUSDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();            
            try
            {
                IWorkbook workbook;
                // delete all customers
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
                ISheet sheet = workbook.GetSheetAt(0); // Read first sheet
                int rowCount = 0;
                DataSaleCSUSDto dscsus = null;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    dscsus = new DataSaleCSUSDto();
                    IRow row = sheet.GetRow(i);
                    dscsus.Id = Guid.NewGuid();
                    dscsus.KUNNR = Utils.GetFormattedCellValue(row.GetCell(5));
                    dscsus.REGION = Utils.GetFormattedCellValue(row.GetCell(13));
                    dscsus.VBELN = Utils.GetFormattedCellValue(row.GetCell(15));
                    dscsus.BILLINGDATE = Utils.GetFormattedCellValue(row.GetCell(16));
                    dscsus.REASON = Utils.GetFormattedCellValue(row.GetCell(17));
                    dscsus.QTY = Utils.GetFormattedCellValue(row.GetCell(18));
                    dscsus.PERNR = Utils.GetFormattedCellValue(row.GetCell(20));
                    dscsus.TT = Utils.GetFormattedCellValue(row.GetCell(24));
                    dscsus.NGUYENGIA = Utils.GetFormattedCellValue(row.GetCell(25));
                    dscsus.TTV = Utils.GetFormattedCellValue(row.GetCell(26));
                    dscsus.ORDERNO = Utils.GetFormattedCellValue(row.GetCell(30));
                    dscsus.POTEXT = Utils.GetFormattedCellValue(row.GetCell(40));
                    dscsus.SODocDate = Utils.GetFormattedCellValue(row.GetCell(44));
                    dscsus.ItemNotes = Utils.GetFormattedCellValue(row.GetCell(45));                    
                    dscsus.CreateBy = dto.CreateBy;
                    dscsus.UpdateBy = dto.UpdateBy;
                    rowCount += CSUSDao.InsertDataSale(dscsus);
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
            }
            catch(OutOfMemoryException ex)
            {
                result.code = Constant.ERROR;
                result.message = string.Format(Constant.OUT_OFF_MEMORY, ex.Message);
            }
            catch(Exception exception)
            {
                result.code = Constant.ERROR;
                result.message = exception.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        

        /// <summary>
        /// Upload Saleman PO Text
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadSalemanPOText(HttpPostedFileBase aFile, string Monthly)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                IWorkbook workbook;
                int del = CSUSDao.DeleteSalemanPOText();
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
                    result.code = Constant.ERROR;
                    throw new Exception(Constant.FILE_INCORRECT_FORMAT);
                }
                ISheet sheet = workbook.GetSheetAt(0);
                int rowCount = 0;
                SalemanPOTextDto smpot = null;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    smpot = new SalemanPOTextDto();
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        smpot.PERNR = Utils.GetFormattedCellValue(row.GetCell(0));
                        smpot.Agent = Utils.GetFormattedCellValue(row.GetCell(1));
                        smpot.Team = Utils.GetFormattedCellValue(row.GetCell(2));
                        smpot.GroupCode = Utils.GetFormattedCellValue(row.GetCell(3));
                        smpot.POText = Utils.GetFormattedCellValue(row.GetCell(4));
                        smpot.SAPExtension = Utils.GetFormattedCellValue(row.GetCell(5));
                        smpot.AgentName = Utils.GetFormattedCellValue(row.GetCell(6));
                        smpot.Monthly = Monthly;
                        rowCount += CSUSDao.InsertSalemanPOText(smpot);
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
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }



        /// <summary>
        /// Process Up Sale Cross Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto ProcessUpSaleCrossSale(CrossSaleUpSaleReportDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                // Check data exist
                IEnumerable<DataSaleCSUSDto> lstDataSale = CSUSDao.GetListDataSalesByMonth(dto);
                CrossSaleUpSaleReportDto datacall = CSUSDao.CheckDataCall(dto);
                SalemanPOTextDto saleman = CSUSDao.CheckSalemanCode();
                if (lstDataSale != null && lstDataSale.Count() > 0 && datacall != null && saleman != null)
                {
                    /* SAVE DATA UP SALE CROSS SALE EXIST IN SALEMAN PO TEXT */
                    int inst = CSUSDao.InsertDataSaleExistSalemanPO(dto);

                    /* SUMMARY DATA SALE */
                    IEnumerable<CrossSaleUpSaleReportDto> lst = CSUSDao.SummaryDataSaleUSCS(dto);
                    foreach (CrossSaleUpSaleReportDto item in lst)
                    {
                        item.UpdateBy = dto.UpdateBy;
                        item.Monthly = Utils.ConvertToYYYYMM(dto.Monthly);
                        result.number += CSUSDao.ProcessDataUpSaleCrossSale(item);
                    }
                    /* UPDATE TOTAL INVOICE  */
                    IEnumerable<SalemanPOTextDto> salemanlist = CSUSDao.GetGroupSalemanPOText(dto);
                    int cnt = 0;
                    foreach (SalemanPOTextDto sm in salemanlist)
                    {
                        sm.Monthly = dto.Monthly;
                        cnt += CSUSDao.SummaryItemNoteTotalInvoice(sm);
                    }
                    /* UPDATE SALE VALUE ITEMNOTES */
                    IEnumerable<CrossSaleUpSaleReportDto> saleValueItemnote = CSUSDao.CalculateSaleValueItemNotes(dto);
                    CrossSaleUpSaleReportDto udt = null;
                    foreach (CrossSaleUpSaleReportDto svi in saleValueItemnote)
                    {
                        udt = new CrossSaleUpSaleReportDto();
                        udt.SalemanCode = svi.SalemanCode;
                        udt.SalesValueOrderItemnote = svi.SalesValueOrderItemnote;
                        udt.Monthly = svi.Monthly;
                        cnt += CSUSDao.UpdateSaleValueItemNotes(udt);
                    }

                    if (result.number > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.CSUS_PROCESS_MESSAGE;
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.NO_ROW_SAVE;
                    }
                }
                else
                {
                    
                    result.code = Constant.ERROR;
                    if (datacall == null)
                    {
                        result.number = 1;
                        result.message = string.Format(Constant.ERROR_NO_DATA_CALL, dto.Monthly);
                    }
                    if (saleman == null)
                    {
                        result.number = 0;
                        result.message = Constant.ERROR_NO_SALEMAN;
                    }
                    if (lstDataSale.Count() == 0)
                    {
                        result.number = 2;
                        result.message = string.Format(Constant.NO_DATA_SALE_FOUND, dto.Monthly);
                    }
                }
                
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// Export Up Sale Cross Sales
        /// </summary>
        /// <param name="uscsResult"></param>
        /// <returns></returns>
        public IWorkbook ExportUpSaleCrossSales(IEnumerable<CrossSaleUpSaleReportDto> uscsResult)
        {
            IWorkbook workbook;
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "USCSReport.xlsx";
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                try
                {
                    workbook = new XSSFWorkbook(file);
                    ISheet sheet_raw = workbook.GetSheet("Report_USCS");
                    int rowIdx = 1;
                    foreach (CrossSaleUpSaleReportDto item in uscsResult)
                    {
                        IRow row = sheet_raw.CreateRow(rowIdx);
                        row.CreateCell(0).SetCellValue(item.Monthly);
                        row.CreateCell(1).SetCellValue(item.Name);
                        row.CreateCell(2).SetCellValue(item.AgentUser);
                        row.CreateCell(3).SetCellValue(item.Team);
                        row.CreateCell(4).SetCellValue(item.GroupCode);
                        row.CreateCell(5).SetCellValue(item.TotalCallsHandle.ToString());
                        row.CreateCell(6).SetCellValue(item.TotalOrderCalls75.ToString());
                        row.CreateCell(7).SetCellValue(item.PhoneOrdersDirectCustomer51.ToString());
                        row.CreateCell(8).SetCellValue(item.ApproachedCSUP.ToString());
                        row.CreateCell(9).SetCellValue(item.SuccessfulCalls.ToString());
                        row.CreateCell(10).SetCellValue(item.SuccessfulOrderCSUP.ToString());
                        row.CreateCell(11).SetCellValue(item.SuccessfulInvoiceApproachedCSUS.ToString());
                        row.CreateCell(12).SetCellValue(item.SuccessfulCustomersApproachedCSUS.ToString());
                        row.CreateCell(13).SetCellValue(item.SalesValueCSUP.ToString());
                        row.CreateCell(14).SetCellValue(item.SalesValueOrderItemnote.ToString());
                        row.CreateCell(15).SetCellValue(item.TotalInvoices.ToString());
                        rowIdx += 1;
                    }
                    file.Close();
                    return workbook;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        /// <summary>
        /// Upload Data Calls
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadDataCalls(HttpPostedFileBase aFile, string userId, DataCallsDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                IWorkbook workbook;
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
                    result.code = Constant.ERROR;
                    throw new Exception(Constant.FILE_INCORRECT_FORMAT);
                }
                /*===== delete data existed =====*/
                int del = CSUSDao.DeleteDataCalls(dto);
                ISheet sheet = workbook.GetSheetAt(0);
                int rowCount = 0;
                DataCallsDto dc = null;
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    dc = new DataCallsDto();
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        if (!string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(0))))
                        {
                            dc.DataCallsID = Guid.NewGuid();
                            dc.AgentName = Utils.GetFormattedCellValue(row.GetCell(0));
                            dc.AgentId = Utils.GetFormattedCellValue(row.GetCell(1));
                            dc.Extension = Utils.GetFormattedCellValue(row.GetCell(2));
                            dc.IntervalStartTime = Utils.GetFormattedCellValue(row.GetCell(3));
                            dc.IntervalEndTime = Utils.GetFormattedCellValue(row.GetCell(4));
                            dc.TotalLoggedInTime = Utils.GetFormattedCellValue(row.GetCell(5));
                            dc.CallPresented = Utils.GetFormattedCellValue(row.GetCell(6));
                            dc.CallHandled = Utils.GetFormattedCellValue(row.GetCell(7));
                            dc.CallbackPresent = Utils.GetFormattedCellValue(row.GetCell(8));
                            dc.CallbackHandled = Utils.GetFormattedCellValue(row.GetCell(9));
                            dc.TotalCallsPresented = Utils.GetFormattedCellValue(row.GetCell(10));
                            dc.TotalCallsHandled = Utils.GetFormattedCellValue(row.GetCell(11));
                            dc.CallRatio = Utils.GetFormattedCellValue(row.GetCell(12));
                            dc.AverageReservedTime = Utils.GetFormattedCellValue(row.GetCell(13));
                            dc.ReadyTimeTalkTime = Utils.GetFormattedCellValue(row.GetCell(14));
                            dc.PercentReadyTimeTalkTime = Utils.GetFormattedCellValue(row.GetCell(15));
                            dc.CreateBy = userId;
                            dc.UpdateBy = userId;
                            dc.Monthly = dto.Monthly;
                            rowCount += CSUSDao.InsertDataCalls(dc);
                        }                        
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
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// Delete Data Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteDataSale(DataSaleParam dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = CSUSDao.DeleteDataSale(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.MESSAGE_DELELE_DATA_SALE, result.number);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.MESSAGE_NO_ROW_DELELE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// Get All Saleman POText
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalemanPOTextDto> GetAllSalemanPOText()
        {
            IEnumerable<SalemanPOTextDto> sm = CSUSDao.GetAllSalemanPOText();
            return sm;
        }

        /// <summary>
        /// Get Data Call By Month
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public IEnumerable<DataCallsDto> GetDataCallByMonth(DataCallSalemanParam pr)
        {
            IEnumerable<DataCallsDto> dc = CSUSDao.GetDataCallByMonth(pr);
            return dc;
        }
    }
}