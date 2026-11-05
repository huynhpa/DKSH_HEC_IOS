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
using System.Web;

namespace DKSHApplication.Service
{
    public class ImportPhoneNumberServiceImpl : ImportPhoneNumberService
    {
        PhoneNumberDao phoneNumberDao = new PhoneNumberDao();

        /// <summary>
        /// Search Phone Number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<PhoneNumberDto> SearchPhoneNumber(PhoneNumberDto dto)
        {
            IEnumerable<PhoneNumberDto> rs = phoneNumberDao.SearchPhoneNumber(dto);
            return rs;
        }

        /// <summary>
        /// Summary Phone Number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SummaryPhoneDto> SummaryPhoneNumber(PhoneNumberDto dto)
        {
            IEnumerable<SummaryPhoneDto> rs = phoneNumberDao.SummaryPhoneNumber(dto);
            return rs;
        }

        /// <summary>
        /// Upload Call Plan
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadPhoneNumber(HttpPostedFileBase aFile, PhoneNumberDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook;
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
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
                    PhoneNumberDto phonenumber = null;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (!string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(0))))
                        {
                            phonenumber = new PhoneNumberDto();
                            phonenumber.ID = Guid.NewGuid();                            
                            phonenumber.PhoneNumber = Utils.GetFormattedCellValue(row.GetCell(0)); // customer code
                            phonenumber.PhoneGroup = Utils.GetFormattedCellValue(row.GetCell(1)); // user agent (ex:220user)
                            phonenumber.CreatedDate = Utils.GetFormattedCellValue(row.GetCell(2)); // call date 
                            phonenumber.CustName = Utils.GetFormattedCellValue(row.GetCell(3)); // remark
                            phonenumber.UpdatedBy = Constant.OUT_CALL_PLAN; // out call plan
                            phonenumber.ProjectId = dto.ProjectId; // project id
                            phonenumber.ServiceType = dto.ProjectId; // campaign
                            rowCount += phoneNumberDao.ImportPhoneNumber(phonenumber);
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
            return result;
        }
    }
}