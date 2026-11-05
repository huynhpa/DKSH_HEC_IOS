using DKSHApplication.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System.IO;
using DKSHApplication.Common;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace DKSHApplication.Service
{
    public class DataProcessServiceImpl : DataProcessService
    {
        DataProcessDao dataProcessDao = new DataProcessDao();

        public ResultMessageDto SaveClient()
        {
            ResultMessageDto result = new ResultMessageDto();
            IEnumerable<SupplierDto> lst = dataProcessDao.SaleByCustCode();
            List<SupplierDto> ls = lst.ToList();
            int cnt = 0;
            string client = string.Empty;
            string custCode = string.Empty;
            SupplierDto item = null;
            for (int i = 0; i < lst.Count(); i++)
            {
                custCode = ls[i].CustCode;
                if (ls[i].CustCode.Equals(ls[i+1].CustCode))
                {
                    client += "," + ls[i].Suppliers;
                }
                item = new SupplierDto();
                item.CustCode = custCode;
                item.Suppliers = client;
                cnt += dataProcessDao.UpdateSuppiers(item);               
            }
            return result;
        }

        

        /// <summary>
        /// Upload Customer ClassSFA
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadCustomerClassSFA(HttpPostedFileBase aFile)
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
                int rowCnt = 0;
                ISheet sheet = workbook.GetSheetAt(0);
                CustomerClassSFADto sfa = null;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    sfa = new CustomerClassSFADto();
                    IRow row = sheet.GetRow(i);
                    sfa.CustCode = Utils.GetFormattedCellValue(row.GetCell(0));
                    sfa.Class = Utils.GetFormattedCellValue(row.GetCell(1));
                    rowCnt += dataProcessDao.SaveCustomerClassSFA(sfa);
                }
                if (rowCnt > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.PROCESS_MESSAGE, rowCnt);
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
        /// Upload Data Process
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadDataProcess(HttpPostedFileBase aFile)
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
                int rowCnt = 0;
                ISheet sheet = workbook.GetSheetAt(0);
                DataProcessDto dp = null;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    dp = new DataProcessDto();
                    IRow row = sheet.GetRow(i);
                    dp.CustCode = Utils.GetFormattedCellValue(row.GetCell(0));
                    dp.CustName = Utils.GetFormattedCellValue(row.GetCell(1));
                    dp.Address = Utils.GetFormattedCellValue(row.GetCell(2));
                    dp.District = Utils.GetFormattedCellValue(row.GetCell(3));
                    dp.Province = Utils.GetFormattedCellValue(row.GetCell(4));
                    dp.Region = Utils.GetFormattedCellValue(row.GetCell(5));
                    dp.Channel = Utils.GetFormattedCellValue(row.GetCell(6));
                    dp.Client = Utils.GetFormattedCellValue(row.GetCell(7));
                    dp.ClientName = Utils.GetFormattedCellValue(row.GetCell(8));
                    dp.SKUCode = Utils.GetFormattedCellValue(row.GetCell(9));
                    dp.SKUName = Utils.GetFormattedCellValue(row.GetCell(10));
                    dp.Sales = Utils.GetFormattedCellValue(row.GetCell(11));
                    dp.IND = Utils.GetFormattedCellValue(row.GetCell(12));
                    rowCnt += dataProcessDao.SaveDataProcess(dp);
                }
                if (rowCnt > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.PROCESS_MESSAGE, rowCnt);
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
        /// Save Data Sales Inhouse
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto SaveDataSalesInhouse(HttpPostedFileBase aFile)
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
                int rowCnt = 0;
                ISheet sheet = workbook.GetSheetAt(0);
                CSSalesDto csSales = null;
                for (int i = 553; i <= sheet.LastRowNum; i++)
                {
                    csSales = new CSSalesDto();
                    IRow row = sheet.GetRow(i);
                    csSales.Plant = Utils.GetFormattedCellValue(row.GetCell(0));
                    csSales.PRINCIPALCODE = Utils.GetFormattedCellValue(row.GetCell(1));
                    csSales.PRINCIPAL = Utils.GetFormattedCellValue(row.GetCell(2));
                    csSales.PRODUCT = Utils.GetFormattedCellValue(row.GetCell(3));
                    csSales.PRODUCTNAME = Utils.GetFormattedCellValue(row.GetCell(4));
                    csSales.KUNNR = Utils.GetFormattedCellValue(row.GetCell(5));
                    csSales.NAME1 = Utils.GetFormattedCellValue(row.GetCell(6));
                    csSales.NAME2 = Utils.GetFormattedCellValue(row.GetCell(7));
                    csSales.STRAS1 = Utils.GetFormattedCellValue(row.GetCell(8));
                    csSales.STRAS2 = Utils.GetFormattedCellValue(row.GetCell(9));
                    csSales.BZIRK = Utils.GetFormattedCellValue(row.GetCell(10));
                    csSales.KDGRP = Utils.GetFormattedCellValue(row.GetCell(11));
                    csSales.KDGRPDESC = Utils.GetFormattedCellValue(row.GetCell(12));
                    csSales.REGIODESC = Utils.GetFormattedCellValue(row.GetCell(13));
                    csSales.BZIRKDESC = Utils.GetFormattedCellValue(row.GetCell(14));
                    csSales.VBELN = Utils.GetFormattedCellValue(row.GetCell(15));
                    csSales.BUDAT = Utils.GetFormattedCellValue(row.GetCell(16)).Trim();
                    csSales.REASON = Utils.GetFormattedCellValue(row.GetCell(17));
                    if (string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(18)).Trim()))
                    {
                        csSales.Qty = "0";
                    }
                    else
                    {
                        csSales.Qty = Utils.GetFormattedCellValue(row.GetCell(18)).Trim();
                    }
                    if (string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(19)).Trim()))
                    {
                        csSales.DGLE = "0";
                    }
                    else
                    {
                        csSales.DGLE = Utils.GetFormattedCellValue(row.GetCell(19)).Trim();
                    }
                    
                    csSales.PERNR = Utils.GetFormattedCellValue(row.GetCell(20));
                    if (string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(21)).Trim()))
                    {
                        csSales.VAT = "0";
                    }
                    else
                    {
                        csSales.VAT = Utils.GetFormattedCellValue(row.GetCell(21)).Trim();
                    }                    
                    csSales.SUOM = Utils.GetFormattedCellValue(row.GetCell(22));
                    csSales.CUSTPAYTO = Utils.GetFormattedCellValue(row.GetCell(23));
                    if (string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(24)).Trim()))
                    {
                        csSales.TT = "0";
                    }
                    else
                    {
                        csSales.TT = Utils.GetFormattedCellValue(row.GetCell(24)).Trim();
                    }
                    if (string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(25)).Trim()))
                    {
                        csSales.NGUYENGIA = "0";
                    }
                    else
                    {
                        csSales.NGUYENGIA = Utils.GetFormattedCellValue(row.GetCell(25)).Trim();
                    }

                    if (string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(26)).Trim()))
                    {
                        csSales.TTV = "0";
                    }
                    else
                    {
                        csSales.TTV = Utils.GetFormattedCellValue(row.GetCell(26)).Trim();
                    }

                    if (string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(27)).Trim()))
                    {
                        csSales.DISCOUNT = "0";
                    }
                    else
                    {
                        csSales.DISCOUNT = Utils.GetFormattedCellValue(row.GetCell(27)).Trim();
                    }                    
                    csSales.Code = Utils.GetFormattedCellValue(row.GetCell(28));
                    csSales.Device = Utils.GetFormattedCellValue(row.GetCell(29));
                    csSales.orginv = Utils.GetFormattedCellValue(row.GetCell(31));
                    csSales.BATCH = Utils.GetFormattedCellValue(row.GetCell(32));
                    csSales.CHARGE = Utils.GetFormattedCellValue(row.GetCell(33));
                    csSales.BNAME = Utils.GetFormattedCellValue(row.GetCell(34));
                    csSales.TEXT1 = Utils.GetFormattedCellValue(row.GetCell(35));
                    rowCnt += dataProcessDao.SaveDataSalesInhouse(csSales);
                }
                if (rowCnt > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.PROCESS_MESSAGE, rowCnt);
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
    }
}