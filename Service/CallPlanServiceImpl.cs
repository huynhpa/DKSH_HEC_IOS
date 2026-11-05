using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using MP.Core.Library;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DKSHApplication.Service
{
    public class CallPlanServiceImpl : CallPlanService
    {
        CallPlanDao callPlanDao = new CallPlanDao();
        CampaignDao campaignDao = new CampaignDao();
        PhoneNumberDao phoneNunberDao = new PhoneNumberDao();

        /// <summary>
        /// what is the function to do?
        /// save the customer focus in campaign
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="WindowUser"></param>
        /// <returns></returns>
        public ResultMessageDto ImportCampaignByCustomer(HttpPostedFileBase aFile, string WindowUser, CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            CallPlanByCampaignDto n = new CallPlanByCampaignDto();
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    IWorkbook workbook;
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
                    // remove and replace
                    if (dto.action_type.Equals("1"))
                    {
                        callPlanDao.DeleteFocusCampaign(dto);
                    }
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = 0;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            CallPlanByCampaignDto campaignByCustomer = new CallPlanByCampaignDto()
                            {
                                CustCode = Utils.GetFormattedCellValue(row.GetCell(0)),
                                user_ext = Utils.GetFormattedCellValue(row.GetCell(1)),
                                salesman_code = Utils.GetFormattedCellValue(row.GetCell(2)),
                                GroupCode = Utils.GetFormattedCellValue(row.GetCell(3)),
                                ServiceCode = Utils.GetFormattedCellValue(row.GetCell(4)),
                                Remark = Utils.GetFormattedCellValue(row.GetCell(6)),
                                indicator = Constant.INDICATOR_FOCUS,
                                create_by = WindowUser
                            };
                            if (!string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(4))))
                            {
                                if (callPlanDao.SaveCallplanByCampaign(campaignByCustomer) >= 1)
                                {
                                    rowCount += 1;
                                }
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
        /// update oss call plan to tracking the customer whether call
        /// </summary>
        /// <param name="oss"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateOSSCallPlan(OSSCallPlanDto oss)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int udt = callPlanDao.UpdateOSSCallPlan(oss);
                if (udt > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
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
        /// search call plan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CallPlanDto> SearchCallPlan(CallPlanDto dto)
        {
            IEnumerable<CallPlanDto> rs = callPlanDao.SearchCallPlan(dto);
            return rs;
        }

        /// <summary>
        /// Upload Call Plan
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadCallPlan(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook;
            try
            {
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

                ISheet sheet = workbook.GetSheet(Constant.CALL_PLAN);
                int rowCount = 0;
                CallPlanDto callPlan = null;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    callPlan = new CallPlanDto();
                    IRow row = sheet.GetRow(i);
                    callPlan.callplan_id = Guid.NewGuid();
                    callPlan.salesman_code = Utils.GetFormattedCellValue(row.GetCell(1));
                    DateTime routeDate = DateTime.ParseExact(Utils.GetFormattedCellValue(row.GetCell(2)), "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    callPlan.route_date = routeDate.ToString("yyyy-MM-dd HH:mm:ss");
                    callPlan.cust_code = Utils.GetFormattedCellValue(row.GetCell(3));
                    callPlan.txn_timestamp = Utils.GetFormattedCellValue(row.GetCell(4));
                    rowCount += callPlanDao.InsertCallPlan(callPlan);
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
            return result;
        }

        /// <summary>
        /// Campaign By Customer Template
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public IWorkbook CampaignByCustomerTemplate(CampaignDto dto)
        {
            IWorkbook workbook;
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "CallPlanByCampaign.xlsx";
            dto.Status = "1"; // get campaign is active
            IEnumerable<ServiceTypeDto> result = campaignDao.SearchCampaign(dto).OrderBy(x=>x.ProjectName);
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(1);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                var font = (XSSFFont)workbook.CreateFont();
                var style = (XSSFCellStyle)workbook.CreateCellStyle();
                font.FontName = "Segoe UI";
                font.FontHeightInPoints = 9;
                style.SetFont(font);
                int rowIdx = 1;
                foreach (ServiceTypeDto item in result)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.ProjectName); // ProjectName
                    sheet.GetRow(rowIdx).GetCell(0).CellStyle = style;
                    row.CreateCell(1).SetCellValue(item.ServiceCode); // ServiceCode
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    row.CreateCell(2).SetCellValue(item.ServiceName); // ServiceName
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// all campaign active
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<ServiceTypeDto> AllCampaignActive(CampaignDto dto)
        {
            dto.Status = "1";
            IEnumerable<ServiceTypeDto> result = campaignDao.SearchCampaign(dto);
            return result;
        }

        /// <summary>
        /// Campaign By Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CallPlanByCampaignDto> SearchCallPlanByCampaign(CallPlanByCampaignDto dto)
        {
            IEnumerable<CallPlanByCampaignDto> result = callPlanDao.SearchCallPlanByCampaign(dto).DistinctBy(s => new { s.CustCode, s.ServiceCode });
            return result;
        }

        /// <summary>
        /// process call plan by campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto ProcessCallPlanByCampaign(CallPlanByCampaignDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                // to do something
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// get call plan from blue ocean 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto GetCallPlan(CallPlanDto dto)
        {
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
                        callPlanDto = new CallPlanDto();
                        if (idx > 0)
                        {
                            list = line.Split('\t').ToList();
                            foreach (string item in list)
                            {
                                callPlanDto.callplan_id = Guid.NewGuid();
                                callPlanDto.salesman_code = list.ElementAt(0);
                                callPlanDto.route_date = list.ElementAt(1);
                                callPlanDto.cust_code = list.ElementAt(2);
                                callPlanDto.txn_timestamp = list.ElementAt(3);
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
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Route Call Plan By SaleRep
        /// </summary>
        /// <returns></returns>
        public ResultMessageDto RouteCallPlanBySaleRep(CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                IEnumerable<CallPlanDto> call_plan = callPlanDao.GetCallPlanDaily(dto);
                PhoneNumberDto phoneNumber = null;
                int idx = 0;
                foreach (CallPlanDto item in call_plan)
                {
                    phoneNumber = new PhoneNumberDto()
                    {
                        ID = Guid.NewGuid(),
                        PhoneNumber = item.cust_code,
                        PhoneGroup = item.WindowsUser,
                        ServiceType = item.project_id,
                        CreatedBy = Constant.SYSTEM_AUTO,
                        UpdatedBy = Constant.CALL_PLAN
                    };
                    idx += phoneNunberDao.ImportPhoneNumber(phoneNumber);
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
        /// purepose route call plan by campaign into phonenumber
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto RouteToPhoneNumber(CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            CallPlanByCampaignDto c = new CallPlanByCampaignDto()
            {
                FocusMonth = dto.FocusMonth
            };
            IEnumerable<CallPlanByCampaignDto> callPlanByCampaign = callPlanDao.SearchCallPlanByCampaign(c);
            foreach (CallPlanByCampaignDto item in callPlanByCampaign)
            {
                PhoneNumberDto pn = new PhoneNumberDto()
                {
                    PhoneNumber = item.CustCode,
                    PhoneGroup = item.AgentNumber
                };
                result.number += phoneNunberDao.RouteToPhoneNumber(pn);
            }
            if (result.number > 0)
            {
                result.code = Constant.SUCCESS;
                result.message = Constant.MESSAGE_SUCCESS;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook ExportCampaignFocus(CallPlanDto dto)
        {
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "CampaignFocus.xlsx";
            IWorkbook workbook;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                string extentsion = Path.GetExtension(filePath).Replace(".", string.Empty);
                if (Constant.EXCEL_XLSX.Equals(extentsion))
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("This format is not supported");
                }                
                var color_fail = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_success = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_callback = new XSSFColor(new byte[] { 51, 153, 255 });
                var font_fail = (XSSFFont)workbook.CreateFont();
                var font_success = (XSSFFont)workbook.CreateFont();
                var font_callback = (XSSFFont)workbook.CreateFont();
                var font_x = (XSSFFont)workbook.CreateFont();
                font_x.FontName = "Arial";
                font_x.FontHeightInPoints = 10;
                var style_x = (XSSFCellStyle)workbook.CreateCellStyle();
                style_x.SetFont(font_x);

                font_fail.SetColor(color_fail);
                font_success.SetColor(color_success);
                font_callback.SetColor(color_callback);
                font_fail.IsBold = true;
                font_success.IsBold = true;
                font_callback.IsBold = true;
                // Fail
                var style_fail = (XSSFCellStyle)workbook.CreateCellStyle();
                style_fail.SetFont(font_fail);
                // Success
                var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                style_success.SetFont(font_success);
                // Call back
                var style_callback = (XSSFCellStyle)workbook.CreateCellStyle();
                style_callback.SetFont(font_callback);
                ISheet sheet = workbook.GetSheetAt(0); // Campaign Focus
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                int rowIdx = 1;
                CallPlanByCampaignDto cpn = new CallPlanByCampaignDto()
                {
                    CustCode = dto.cust_code, 
                    salesman_code = dto.salesman_code,
                    FocusMonth = dto.FocusMonth
                };
                IEnumerable<CallPlanByCampaignDto> result = callPlanDao.SearchCallPlanByCampaign(cpn).DistinctBy(s => new { s.CustCode, s.ServiceCode });
                foreach(CallPlanByCampaignDto item in result)
                {
                    IRow row = sheet.CreateRow(rowIdx);                    
                    row.CreateCell(0).SetCellValue(item.ServiceCode); // campaign code
                    row.GetCell(0).CellStyle = style_x;
                    row.CreateCell(1).SetCellValue(item.ServiceName); // campaign name
                    row.GetCell(1).CellStyle = style_x;
                    row.CreateCell(2).SetCellValue(item.CustCode); // custcode
                    row.GetCell(2).CellStyle = style_x;
                    row.CreateCell(3).SetCellValue(item.salesman_code); // saleman
                    row.GetCell(3).CellStyle = style_x;
                    row.CreateCell(4).SetCellValue(item.AgentNumber); // extension
                    row.GetCell(4).CellStyle = style_x;
                    row.CreateCell(5).SetCellValue(item.FullName); // full name
                    row.GetCell(5).CellStyle = style_x;
                    if (!string.IsNullOrEmpty(item.CreateDate))
                    {
                        row.CreateCell(6).SetCellValue(new DateTime(int.Parse(item.CreateDate.Substring(6, 4)), 
                            int.Parse(item.CreateDate.Substring(3, 2)), int.Parse(item.CreateDate.Substring(0, 2))));
                        row.GetCell(6).CellStyle = cellStyle;
                    }             
                    
                    rowIdx += 1;
                }

            }
            return workbook;
        }

        public ResultMessageDto ImportDailyInTimeSummary(HttpPostedFileBase aFile, string windowUser, CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            CallPlanByCampaignDto n = new CallPlanByCampaignDto();
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    IWorkbook workbook;
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
                    
                    int del = callPlanDao.DeleteDailyInTimeSummary(dto);
                    
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = 0;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            ICell visit = row.GetCell(4);
                            if (visit != null)
                            {
                                visit.SetCellType(CellType.String);
                            }                            
                            DailyInTimeSummaryDto dits = new DailyInTimeSummaryDto()
                            {
                                sales_area_code = Utils.GetFormattedCellValue(row.GetCell(0)),
                                sales_area_name = Utils.GetFormattedCellValue(row.GetCell(1)),
                                salesrep_code = Utils.GetFormattedCellValue(row.GetCell(2)),
                                salesrep_name = Utils.GetFormattedCellValue(row.GetCell(3)),
                                visit_id = Utils.GetFormattedCellValue(visit),
                                cust_code = Utils.GetFormattedCellValue(row.GetCell(5)),
                                cust_name = Utils.GetFormattedCellValue(row.GetCell(6)),
                                cont_code = Utils.GetFormattedCellValue(row.GetCell(7)),
                                cont_name = Utils.GetFormattedCellValue(row.GetCell(8)),
                                clas = Utils.GetFormattedCellValue(row.GetCell(9)),
                                call_date = Utils.GetFormattedCellValue(row.GetCell(10)),
                                time_in = Utils.GetFormattedCellValue(row.GetCell(11)),
                                time_out = Utils.GetFormattedCellValue(row.GetCell(12)),
                                time_spend = Utils.GetFormattedCellValue(row.GetCell(13)),
                                reason_code = Utils.GetFormattedCellValue(row.GetCell(14)),
                                reason_name = Utils.GetFormattedCellValue(row.GetCell(15)),
                                remarks = Utils.GetFormattedCellValue(row.GetCell(16)),
                                start_date = Utils.GetFormattedCellValue(row.GetCell(17)),
                                end_date = Utils.GetFormattedCellValue(row.GetCell(18)),
                                dummy_reason_code = Utils.GetFormattedCellValue(row.GetCell(19)),
                                txn_timestamp = Utils.GetFormattedCellValue(row.GetCell(20)),
                                sfms_ind = Utils.GetFormattedCellValue(row.GetCell(21)),
                                sales_ord_ind = Utils.GetFormattedCellValue(row.GetCell(22)),
                                tra_ind = Utils.GetFormattedCellValue(row.GetCell(23)),
                                coll_ind = Utils.GetFormattedCellValue(row.GetCell(24)),                                
                                drc_ind = Utils.GetFormattedCellValue(row.GetCell(25)),
                                mss_ind = Utils.GetFormattedCellValue(row.GetCell(26)),
                                visit_ind = Utils.GetFormattedCellValue(row.GetCell(27)),
                                in_latitude = Utils.GetFormattedCellValue(row.GetCell(28)),
                                in_longitude = Utils.GetFormattedCellValue(row.GetCell(29)),
                                in_gpstime = Utils.GetFormattedCellValue(row.GetCell(30)),
                                out_latitude = Utils.GetFormattedCellValue(row.GetCell(31)),
                                out_longitude = Utils.GetFormattedCellValue(row.GetCell(32)),
                                out_gpstime = Utils.GetFormattedCellValue(row.GetCell(33)),
                                call_ind = Utils.GetFormattedCellValue(row.GetCell(34)),
                                det_ind = Utils.GetFormattedCellValue(row.GetCell(35)),
                                summary_month = dto.summary_month
                            };
                            if (!string.IsNullOrEmpty(dits.visit_id))
                            {
                                rowCount = callPlanDao.DailyInTimeSummary(dits);
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
        /// SearchDailyInTimeSummary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<DailyInTimeSummaryDto> SearchDailyInTimeSummary(CallPlanDto dto)
        {
            IEnumerable<DailyInTimeSummaryDto> result = callPlanDao.SearchDailyInTimeSummary(dto);
            return result;
        }

        public ResultMessageDto ImportLimitedCustomer(HttpPostedFileBase aFile, LimitedCustomerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            CallPlanByCampaignDto n = new CallPlanByCampaignDto();
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    IWorkbook workbook;
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
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            LimitedCustomerDto lmc = new LimitedCustomerDto()
                            {
                                cust_code = Utils.GetFormattedCellValue(row.GetCell(0))
                            };
                        }                      
                        rowCount += callPlanDao.SaveLimitedCustomer(dto);
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
        /// SearchLimitedCustomer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<LimitedCustomerDto> SearchLimitedCustomer(LimitedCustomerDto dto)
        {
            IEnumerable<LimitedCustomerDto> result = callPlanDao.SearchLimitedCustomer(dto);
            return result;
        }

        /// <summary>
        /// CheckLimitedCustomer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto CheckLimitedCustomer(LimitedCustomerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                string[] para = new string[3];
                
                IEnumerable<LimitedCustomerDto> rs = callPlanDao.CheckLimitedCustomer(dto);                
                if (rs.Count() > 0)
                {
                    para[0] = rs.ElementAt(0).cust_code;
                    para[1] = rs.ElementAt(0).limit_from;
                    para[2] = rs.ElementAt(0).limit_to;
                    result.code = Constant.EXISTED;
                    result.message = string.Format(Constant.CUSTOMER_HAS_BEEN_LIMITED_INTIME, para);
                }
                else
                {
                    result.code = Constant.SUCCESS;
                }
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