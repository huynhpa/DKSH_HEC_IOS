using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.DAO;
using DKSHApplication.Common;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;

namespace DKSHApplication.Service
{
    public class SalesPromotionServiceImpl : SalesPromotionService
    {
        SalesPromotionDao salesPromotionDao = new SalesPromotionDao();
        ProductInformationDao productInformationDao = new ProductInformationDao();

        /// <summary>
        /// Get Sales Promotion Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public SalesPromotionDto GetSalesPromotionDetail(SalesPromotionDto dto)
        {
            SalesPromotionDto spDto = salesPromotionDao.GetSalesPromotionDetail(dto);
            return spDto;
        }

        /// <summary>
        /// Get Suppliers
        /// </summary>
        /// <returns>ResultMessageDto</returns>
        public ResultMessageDto GetSuppliers()
        {
            ResultMessageDto result = new ResultMessageDto();
            result.Suppliers = productInformationDao.GetSuppliers();
            return result;
        }

        /// <summary>
        /// Save SalesPromotion
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto SaveSalesPromotion(HttpPostedFileBase aFile, SalesPromotionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
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
                throw new Exception("This format is not supported");
            }
            ISheet sheet = workbook.GetSheetAt(0);
            SalesPromotionDto spDto = null;
            int rowCount = 0;
            int cnt = 0;
            int rowIdx = 2;
            try
            {
                string t = string.Empty;
                IRow title = sheet.GetRow(0);
                if (title != null)
                {
                    t = Utils.GetFormattedCellValue(sheet.GetRow(0).GetCell(0));
                }
                
                int spId = Utils.GenerateRandomNo();
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);                    
                    spDto = new SalesPromotionDto();
                    spDto.Id = Guid.NewGuid();                    
                    spDto.Title = t;
                    spDto.ProductCode = Utils.GetFormattedCellValue(row.GetCell(0));
                    spDto.ProductName = Utils.GetFormattedCellValue(row.GetCell(1));
                    if (!string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(2))) && i > rowIdx)
                    {
                        rowIdx = i;
                        spId = Utils.GenerateRandomNo();
                        ICell cellContent = sheet.GetRow(rowIdx).GetCell(2);
                        ICell cellTime = sheet.GetRow(rowIdx).GetCell(3);
                        ICell cellEntity = sheet.GetRow(rowIdx).GetCell(4);
                        ICell cellArea = sheet.GetRow(rowIdx).GetCell(5);
                        ICell cellRemark = sheet.GetRow(rowIdx).GetCell(6);
                        string contentHold = Utils.GetFormattedCellValue(cellContent);
                        string timeHold = Utils.GetFormattedCellValue(cellTime);
                        string entityHold = Utils.GetFormattedCellValue(cellEntity);
                        string areaHold = Utils.GetFormattedCellValue(cellArea);
                        string remarkHold = Utils.GetFormattedCellValue(cellRemark);
                        spDto.SalesContent = contentHold;
                        spDto.ApplyTime = timeHold;
                        spDto.EntityApply = entityHold;
                        spDto.ApplyArea = areaHold;
                        spDto.Remark = remarkHold;
                    }
                    else
                    {
                        spDto.SalesContent = Utils.GetFormattedCellValue(sheet.GetRow(rowIdx).GetCell(2));
                        spDto.ApplyTime = Utils.GetFormattedCellValue(sheet.GetRow(rowIdx).GetCell(3));
                        spDto.EntityApply = Utils.GetFormattedCellValue(sheet.GetRow(rowIdx).GetCell(4));
                        spDto.ApplyArea = Utils.GetFormattedCellValue(sheet.GetRow(rowIdx).GetCell(5));
                        spDto.Remark = Utils.GetFormattedCellValue(sheet.GetRow(rowIdx).GetCell(6));
                    }
                    List<string> applyTime = spDto.ApplyTime.Split(' ').ToList();
                    List<string> strDate = new List<string>();
                    foreach (string str in applyTime)
                    {
                        string s = str.Trim().ToLower().Replace("\nvà", "").Replace("\nđến","");
                        bool isDate = Utils.CheckStringIsDate(s);
                        if (isDate)
                        {
                            strDate.Add(s);
                        }
                    }
                    if (strDate.Count() == 2)
                    {
                        spDto.StartDate = Utils.StringDateFormat1(strDate[0]);
                        spDto.EndDate = Utils.StringDateFormat1(strDate[1]);
                    }
                    else if (strDate.Count() == 4)
                    {
                        spDto.StartDate = Utils.StringDateFormat1(strDate[0]);
                        spDto.EndDate = Utils.StringDateFormat1(strDate[1]);
                        spDto.StartDate1 = Utils.StringDateFormat1(strDate[2]);
                        spDto.EndDate1 = Utils.StringDateFormat1(strDate[3]);
                    }
                    spDto.SalesPromotionId = spId.ToString();
                    spDto.CreateBy = dto.CreateBy;
                    spDto.UpdateBy = dto.CreateBy;
                    cnt += salesPromotionDao.SaveSalesPromotion(spDto);
                    rowCount += 1;
                }
                if (cnt > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.PROCESS_MESSAGE, cnt);
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
        /// Search SalesPromotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SalesPromotionDto> SearchSalesPromotion(SalesPromotionDto dto)
        {
            try
            {
                List<string> entities = new List<string>();
                if (!string.IsNullOrEmpty(dto.EntityApply))
                {
                    entities = dto.EntityApply.Split(',').ToList();
                    if (entities.Count() > 0)
                    {
                        foreach (string item in entities)
                        {
                            if (item.Equals("NT"))
                            {
                                dto.NhaThuoc = true;
                            }
                            else if (item.Equals("NTBV"))
                            {
                                dto.NhaThuocBV = true;
                            }
                            else if (item.Equals("CTD"))
                            {
                                dto.CongtyDuoc = true;
                            }
                        }
                    }
                }
                IEnumerable<SalesPromotionDto> lst = salesPromotionDao.SearchSalesPromotion(dto);
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto SetEntity(SalesPromotionDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            try
            {
                rs.number = salesPromotionDao.SetEntity(dto);
                if (rs.number > 0)
                {
                    rs.code = Constant.SUCCESS;
                    rs.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    rs.code = Constant.ERROR;
                    rs.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                rs.code = Constant.ERROR;
                rs.message = ex.Message;
            }
            return rs;
        }

        /// <summary>
        /// Update SalesPromotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateSalesPromotion(SalesPromotionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int cnt = salesPromotionDao.UpdateSalesPromotion(dto);
                if (cnt > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
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
        /// SalesPromotionIdList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalesPromotionDto> SalesPromotionIdList()
        {
            IEnumerable<SalesPromotionDto> rs = salesPromotionDao.SalesPromotionIdList();
            return rs;
        }

        /// <summary>
        /// Export By Entity Apply
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string ExportByEntityApply(SalesPromotionDto dto)
        {
            string fileSave = string.Empty;
            string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/"));
            string fileTemplate = Constant.SALES_PROMOTION_BY_ENTITY;
            List<string> listPath = new List<string>();
            List<string> entities = new List<string>();
            entities.Add("NT");
            entities.Add("NTBV");
            entities.Add("CTD");
            foreach (string i in entities)
            {
                string fileName = "CTKM_" + i + "_" + DateTime.Now.ToString("MMyyyy");
                dto.Entity = i;
                IEnumerable<SalesPromotionDto> lst = salesPromotionDao.ExportByEntityApply(dto);
                var fileLocation = filePath + fileTemplate;
                using (FileStream file = new FileStream(fileLocation, FileMode.Open, FileAccess.ReadWrite))
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
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowIdx = 2;
                    
                    foreach (SalesPromotionDto obj in lst)
                    {
                        IRow row = sheet.CreateRow(rowIdx);
                        row.CreateCell(0).SetCellValue(obj.Suppliers);
                        row.GetCell(0).CellStyle = style_red;
                        row.CreateCell(1).SetCellValue(obj.ProductCode);
                        row.CreateCell(2).SetCellValue(obj.ProductName);
                        row.CreateCell(3).SetCellValue(obj.SalesContent);
                        row.CreateCell(4).SetCellValue(obj.StartDate);
                        row.CreateCell(5).SetCellValue(obj.EndDate);
                        row.CreateCell(6).SetCellValue(obj.StartDate1);
                        row.CreateCell(7).SetCellValue(obj.EndDate1);
                        if (obj.Status != null)
                        {
                            if (obj.Status.Equals("ACTIVE"))
                            {
                                row.CreateCell(8).SetCellValue(obj.Status);
                                row.GetCell(8).CellStyle = style_x;
                            }
                            else if (obj.Status.Equals("EXPIRED"))
                            {
                                row.CreateCell(8).SetCellValue(obj.Status);
                                row.GetCell(8).CellStyle = style_red;
                            }
                        }
                        row.CreateCell(9).SetCellValue(obj.ApplyArea);
                        row.CreateCell(10).SetCellValue(obj.EntityApply);
                        rowIdx += 1;
                    }                    

                    string excelFile = filePath + fileName + "." + Constant.EXCEL_XLSX;
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
            if (listPath.Count() > 0)
            {
                fileSave = Utils.CreateZipFile(listPath.Count(), filePath, listPath);
            }            
            return fileSave;
        }
        
        /// <summary>
        /// Delete Sales Promotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteSalesPromotion(SalesPromotionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = salesPromotionDao.DeleteSalesPromotion(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.MESSAGE_DELELE, result.number);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.MESSAGE_NO_ROW_DELELE;
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
        /// Export
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public IWorkbook Export(IEnumerable<SalesPromotionDto> lst)
        {
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + Constant.SALES_PROMOTION_REPORT;
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
                ISheet sheet = workbook.GetSheet("SalesPromotion");
                int rowIdx = 1;
                foreach (SalesPromotionDto obj in lst)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    //row.CreateCell(0).SetCellValue(obj.SalesPromotionId);
                    row.CreateCell(1).SetCellValue(obj.ProductCode);
                    row.CreateCell(2).SetCellValue(obj.ProductName);
                    row.CreateCell(3).SetCellValue(obj.Suppliers);
                    row.CreateCell(4).SetCellValue(obj.SalesContent);
                    row.CreateCell(5).SetCellValue(obj.ApplyTime);                    
                    if (obj.Status != null)
                    {
                        if (obj.Status.Equals("ACTIVE"))
                        {
                            row.CreateCell(6).SetCellValue(obj.Status);
                            row.GetCell(6).CellStyle = style_x;
                        }
                        else if (obj.Status.Equals("EXPIRED"))
                        {
                            row.CreateCell(6).SetCellValue(obj.Status);
                            row.GetCell(6).CellStyle = style_red;
                        }
                    }
                    if (obj.NhaThuoc != null && obj.NhaThuoc == true)
                    {
                        row.CreateCell(7).SetCellValue("X");
                        row.GetCell(7).CellStyle = style_x;
                    }
                    if (obj.NhaThuocBV != null && obj.NhaThuocBV == true)
                    {
                        row.CreateCell(8).SetCellValue("X"); 
                        row.GetCell(8).CellStyle = style_x;
                    }
                    if (obj.CongtyDuoc != null && obj.CongtyDuoc == true)
                    {
                        row.CreateCell(9).SetCellValue("X"); 
                        row.GetCell(9).CellStyle = style_x;
                    }
                    row.CreateCell(10).SetCellValue(obj.EntityApply);
                    row.CreateCell(11).SetCellValue(obj.ApplyArea);
                    row.CreateCell(12).SetCellValue(obj.Remark);
                    rowIdx += 1;
                }
                // Merge
                file.Close();
                return workbook;
            }
        }

    }
}