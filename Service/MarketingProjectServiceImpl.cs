using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DB;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using System.Text;
using System.Globalization;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;

namespace DKSHApplication.Service
{
    public class MarketingProjectServiceImpl : MarketingProjectService
    {
        MarketingProjectDao projectDao = new MarketingProjectDao();
        MarketingDataRawDao dataRawDao = new MarketingDataRawDao();
        CustomerDao customerDao = new CustomerDao();

        private static string EXT_XLS = ".xls";

        private static string EXT_XLSX = ".xlsx";

        private static string EXT_TXT = ".txt";

        private static string ddMMyyyy = "dd-MM-yyyy";


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto Delete(MarketingProjectDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result.number = dataRawDao.DeleteAllDataByProjectNumber(json);
            object[] param = new object[2];
            param[0] = json.ProjectName;
            param[1] = result.number;
            if (result.number > 0)
            {
                result.code = Constant.DELETE;
                result.message = string.Format("Project {0}, {1} data picked records have been deleted successfully", param);
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.MESSAGE_ERROR;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Channel
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ChannelDto> GetChannel()
        {
            IEnumerable<ChannelDto> lst = projectDao.GetChannel();
            return lst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetProjectById
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public MarketingProjectDto GetProjectById(MarketingProjectDto json)
        {
            MarketingProjectDto result = new MarketingProjectDto();
            result = projectDao.GetProjectById(json);
            //result.FromTime = !string.IsNullOrEmpty(result.FromTime) ? result.FromTime.Substring(0, 5) + " " +  result.FromTime.Substring(5, 2) : string.Empty;
            //result.ToTime = !string.IsNullOrEmpty(result.ToTime) ? result.ToTime.Substring(0, 5) + " " + result.ToTime.Substring(5, 2) : string.Empty;
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Region
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RegionDto> GetRegion()
        {
            IEnumerable<RegionDto> lst = projectDao.GetRegion();
            return lst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Saleman Code
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalemanCodeDto> GetSalemanCode(SalemanCodeDto json)
        {
            IEnumerable<SalemanCodeDto> lst = projectDao.GetSalemanCode(json);
            return lst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto Insert(MarketingProjectDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (json != null)
            {
                /* Handle marketing project dto */
                MarketingProjectDto obj = HandleMarketingProjectDto(json);
                if (!string.IsNullOrEmpty(obj.ProductList) ||
                        (!obj.InExCludedCust.Equals(Constant.ALL) && !string.IsNullOrEmpty(obj.CustomerList)))
                {
                    result.number = projectDao.Insert(obj);
                    if (result.number == 1)
                    {
                        result.code = Constant.INSERT;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.MESSAGE_ERROR;
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.MESSAGE_PRODUCT_CUSTOMER_NULL;
                }

            }

            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search Marketing Project
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> Search(MarketingProjectDto pr)
        {
            IEnumerable<MarketingProjectDto> lst = projectDao.SearchMarketingProject(pr);
            return lst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// team list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> TeamList()
        {
            IEnumerable<string> team = projectDao.TeamList();
            return team;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto Update(MarketingProjectDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (json != null)
            {
                try
                {
                    /* Handle marketing project dto */
                    MarketingProjectDto obj = HandleMarketingProjectDto(json);
                    if (!string.IsNullOrEmpty(obj.ProductList) ||
                            (!obj.InExCludedCust.Equals(Constant.ALL) && !string.IsNullOrEmpty(obj.CustomerList)))
                    {
                        result.number = projectDao.Update(obj);
                        if (result.number == 1)
                        {
                            result.code = Constant.UPDATE;
                            result.message = Constant.MESSAGE_SUCCESS;
                        }
                        else
                        {
                            result.code = Constant.ERROR;
                            result.message = Constant.MESSAGE_ERROR;
                        }
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.MESSAGE_PRODUCT_CUSTOMER_NULL;
                    }
                }
                catch(Exception ex)
                {
                    result.code = Constant.ERROR;
                    result.message = ex.Message;
                }

                

            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handle MarketingProjectDto object before insert or update
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public MarketingProjectDto HandleMarketingProjectDto(MarketingProjectDto jsonParam)
        {
            StringBuilder productList = new StringBuilder();
            StringBuilder customerList = new StringBuilder();
            StringBuilder resourceList = new StringBuilder();
            List<string> productGroup = new List<string>();
            StringBuilder channel = new StringBuilder();
            if (jsonParam.InExCludedChannel.Equals(Constant.ALL))
            {
                IEnumerable<ChannelDto> channels = projectDao.GetChannel();
                int idx = 0;
                foreach (ChannelDto cn in channels)
                {
                    channel.Append(cn.Channel);
                    if (idx < channels.Count() - 1)
                    {
                        channel.Append(Constant.COMMA);
                    }
                    idx += 1;
                }
                jsonParam.Channel = channel.ToString();
            }
            
            /* ASSGIN TO DTO */
            string filePathCustomer = Path.Combine(HttpContext.Current.Server.MapPath("~/FilesUpload/")) + jsonParam.ProjectNumber
                                + "_Customers." + jsonParam.CustomerExtension;
            string filePathProduct = Path.Combine(HttpContext.Current.Server.MapPath("~/FilesUpload/")) + jsonParam.ProjectNumber
                                + "_Products." + jsonParam.ProductExtension;
            string filePathResources = Path.Combine(HttpContext.Current.Server.MapPath("~/FilesUpload/")) + jsonParam.ProjectNumber
                                + "_Resources.xlsx";
            string extCustomer = Path.GetExtension(filePathCustomer);
            string extProduct = Path.GetExtension(filePathCustomer);
            if (extCustomer.Equals(EXT_XLS) || extCustomer.Equals(EXT_XLSX))
            {
                customerList = ReadWorkbook(jsonParam, "Customers");
            }
            else if (extCustomer.Equals(EXT_TXT))
            {
                customerList = ReadTextFile(jsonParam, filePathCustomer);
            }
            if (extProduct.Equals(EXT_XLS) || extProduct.Equals(EXT_XLSX))
            {
                productList = ReadWorkbook(jsonParam, "Products");
            }
            else if (extProduct.Equals(EXT_TXT))
            {
                productList = ReadTextFile(jsonParam, filePathProduct);
            }
            resourceList = ReadWorkbook(jsonParam, "Resources");
            jsonParam.ProductList = productList.ToString();
            jsonParam.Customers = customerList.ToString();
            jsonParam.ResourceSale = resourceList.ToString();
            List<ProductsGroupDto> prList = ReadWorkbookProductGroup(jsonParam, "Products");
            int i = 0;
            int del = projectDao.DeleteProductGroup(jsonParam);
            foreach (ProductsGroupDto item in prList)
            {
                productGroup.Add(item.ProductGroup);
                projectDao.InsertProductGroup(item);
                i += 1;
            }
            StringBuilder pg = new StringBuilder();
            productGroup = productGroup.Distinct().ToList();
            int j = 0;
            foreach(string str in productGroup)
            {
                pg.Append(str);
                if (j < productGroup.Count() - 1)
                {
                    pg.Append(Constant.COMMA);
                }
                j += 1;
            }
            jsonParam.ProductGroup = pg.ToString();
            return jsonParam;
        }

        /// <summary>
        /// Read Sheet
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private StringBuilder ReadWorkbook(MarketingProjectDto jsonParam, string sheetName)
        {
            StringBuilder data = new StringBuilder();
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/FilesUpload/")) + jsonParam.ProjectNumber
                                + "_" + sheetName + ".xlsx";
            if (File.Exists(filePath))
            {
                string ext = Path.GetExtension(filePath);
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheet(sheetName);
                    int rowIdx = 1;
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row != null && row.GetCell(0) != null)
                        {
                            data.Append(Utils.GetFormattedCellValue(row.GetCell(0)));
                            if (rowIdx < sheet.LastRowNum)
                            {
                                data.Append(Constant.COMMA);
                            }
                        }
                        rowIdx += 1;
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// read workbook product group
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private List<ProductsGroupDto> ReadWorkbookProductGroup(MarketingProjectDto jsonParam, string sheetName)
        {
            List<ProductsGroupDto> pgList = new List<ProductsGroupDto>();
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/FilesUpload/")) + jsonParam.ProjectNumber
                                + "_" + sheetName + ".xlsx";
            if (File.Exists(filePath))
            {
                string ext = Path.GetExtension(filePath);
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheet(sheetName);
                    int rowIdx = 1;
                    ProductsGroupDto pg = null;
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row != null)
                        {
                            pg = new ProductsGroupDto();
                            pg.ProductCode = Utils.GetFormattedCellValue(row.GetCell(0));
                            pg.ProductName = Utils.GetFormattedCellValue(row.GetCell(1));
                            pg.ProductGroup = Utils.GetFormattedCellValue(row.GetCell(2));
                            pg.ProjectNumber = jsonParam.ProjectNumber;
                            pgList.Add(pg);
                        }
                        rowIdx += 1;
                    }
                }
            }
            return pgList;
        }

        /// <summary>
        /// Read Text File
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private StringBuilder ReadTextFile(MarketingProjectDto jsonParam, string filePath)
        {
            StringBuilder data = new StringBuilder();
            data = Utils.ReadFileTXTLineByLine(filePath);
            return data;
        }

        /// <summary>
        /// Check OrderNo VA05
        /// </summary>
        private bool CheckOrderNoVA05(MarketingDataRawDto dto)
        {
            bool va05 = false;
            MarketingDataRawDto exists = dataRawDao.CheckOrderNoVA05(dto);
            if (exists == null)
            {
                va05 = true;
            }
            return va05;
        }

        /// <summary>
        /// Get Customer By CustCode
        /// </summary>
        /// <param name="CustCode"></param>
        /// <returns></returns>
        private CustomerDto GetCustomerByCustCode(string CustCode)
        {
            CustomerDto customer = dataRawDao.GetCustomerByCustCode(CustCode);
            return customer;
        }

        /// <summary>
        /// Process data from inhouse
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto ProcessProject(IEnumerable<MarketingProjectDto> jsonArr, string currDate, string userId)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (jsonArr != null && jsonArr.Any())
            {
                string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/")) + currDate + "_" + Constant.INHOUSE + "." + Constant.EXCEL_XLSX;
                /*=========== LOOP PROJECT SELECTED BY OBJECT ===========*/
                int countRow = 0;
                string projectNumber = string.Empty;
                string lastDateProc = string.Empty;

                foreach (MarketingProjectDto projDto in jsonArr)
                {
                    projectNumber += projDto.ProjectNumber + ",";
                    projDto.LastDateProcess = !string.IsNullOrEmpty(projDto.LastDateProcess) ? projDto.LastDateProcess.Substring(6, 4) +
                                            projDto.LastDateProcess.Substring(3, 2) + projDto.LastDateProcess.Substring(0, 2) : string.Empty;
                    lastDateProc += projDto.LastDateProcess;
                    // DELETE DATA BEFORE PROCESS
                    MarketingProjectDto item = projectDao.GetProjectById(projDto);
                    if (!string.IsNullOrEmpty(item.LastDateProcess))
                    {
                        item.ProjectNumber = projectNumber;
                        item.LastDateProcess = lastDateProc;
                        int deleteRow = dataRawDao.DeleteMarketingDataRaw(item);
                    }
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook;
                        workbook = new XSSFWorkbook(file);
                        ISheet sheet_Inhouse = workbook.GetSheet(Constant.INHOUSE); // GET SHEET INHOUSE
                        ISheet sheet_VA05 = workbook.GetSheet(Constant.VA05); // GET SHEET VA05
                        ProjectInExcludedDto InExcludedDto = new ProjectInExcludedDto();
                        if (!string.IsNullOrEmpty(item.Customers))
                        {
                            InExcludedDto.CustCodeList = item.Customers.Split(',').ToList();
                        }
                        if (!string.IsNullOrEmpty(item.ProductList))
                        {
                            InExcludedDto.ProductList = item.ProductList.Split(',').ToList();
                        }
                        if (!string.IsNullOrEmpty(item.Channel))
                        {
                            InExcludedDto.ChannelList = item.Channel.Split(',').ToList();
                        }
                        if (!string.IsNullOrEmpty(item.Region))
                        {
                            InExcludedDto.RegionList = item.Region.Split(',').ToList();
                            for (int n = 0; n < InExcludedDto.RegionList.Count(); n++)
                            {
                                if (InExcludedDto.RegionList[n].Equals(string.Empty))
                                {
                                    InExcludedDto.RegionList.RemoveAt(n);
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(item.District))
                        {
                            InExcludedDto.DistrictList = item.District.Split(',').ToList();
                        }
                        if (!string.IsNullOrEmpty(item.ResourceSale))
                        {
                            InExcludedDto.ResourceList = item.ResourceSale.Split(',').ToList();
                        }
                        if (!string.IsNullOrEmpty(item.Province))
                        {
                            InExcludedDto.ProvinceList = item.Province.Split(',').ToList();
                        }
                        /* 
                         * get custgroup in channel 
                         * For case channel Phar.Company include custgroup
                         * Four CustGroup
                         */
                        ChannelDto channel = new ChannelDto();
                        channel.Channel = item.Channel;
                        IEnumerable<CGrpCodeDto> custgroupCode = projectDao.GetCustGroupCodeByChannel(channel);
                        // Read and save from INHOUSE
                        for (int i = 1; i <= sheet_Inhouse.LastRowNum; i++)
                        {
                            IRow row = sheet_Inhouse.GetRow(i);
                            ICell cell_cust = row.GetCell(5) != null ? row.GetCell(5) : row.CreateCell(5); // Cust Code
                            ICell cell_cgrpcode = row.GetCell(11) != null ? row.GetCell(11) : row.CreateCell(11); // CGrpCode - CustGroupName
                            ICell cell_product = row.GetCell(3) != null ? row.GetCell(3) : row.CreateCell(3); // Products
                            ICell cell_province = row.GetCell(3) != null ? row.GetCell(3) : row.CreateCell(3); // Province
                            ICell cell_region = row.GetCell(13) != null ? row.GetCell(13) : row.CreateCell(13); // Region
                            ICell cell_district = row.GetCell(14) != null ? row.GetCell(14) : row.CreateCell(14); // District
                            ICell cell_reason = row.GetCell(17) != null ? row.GetCell(17) : row.CreateCell(17); // Reason
                            ICell cell_salemancode = row.GetCell(20) != null ? row.GetCell(20) : row.CreateCell(20); // Sale man code
                            ICell cell_soDocDate = row.GetCell(44) != null ? row.GetCell(44) : row.CreateCell(44); // Sale date
                            ICell cell_channel = row.GetCell(12) != null ? row.GetCell(12) : row.CreateCell(12); // Channel
                            ICell cell_tt = row.GetCell(24) != null ? row.GetCell(24) : row.CreateCell(24); // TT
                            bool flagChannel = false;
                            bool flagRegion = false;
                            bool flagDistrict = false;
                            bool flagProduct = false;
                            bool flagResource = false;
                            bool flagProvince = false;
                            bool flagCustomer = LoopInExCludedCustomers(InExcludedDto.CustCodeList, Utils.GetFormattedCellValue(cell_cust), item.InExCludedCust);
                            if (Utils.GetFormattedCellValue(cell_reason).Trim().Equals(Constant.REASON))
                            {
                                if (flagCustomer)
                                {
                                    flagChannel = LoopInExCludedChannel(custgroupCode, Utils.GetFormattedCellValue(cell_cgrpcode), item.InExCludedChannel);
                                    if (flagChannel)
                                    {
                                        flagProvince = LoopMathProvince(InExcludedDto.ProvinceList, Utils.GetFormattedCellValue(cell_province), item.InExCludedProvince);
                                        if (flagProvince)
                                        {
                                            flagRegion = LoopInExcludedRegion(InExcludedDto.RegionList, Utils.GetFormattedCellValue(cell_region), item.InExCludedRegion);
                                            if (flagRegion)
                                            {
                                                flagDistrict = LoopInExcludedDistrict(InExcludedDto.DistrictList, Utils.GetFormattedCellValue(cell_district), item.InExCludedDistrict);
                                                if (flagDistrict)
                                                {
                                                    flagProduct = LoopMatchProducts(InExcludedDto.ProductList, Utils.GetFormattedCellValue(cell_product));
                                                    if (flagProduct)
                                                    {
                                                        flagResource = LoopResourceSale(InExcludedDto.ResourceList, Utils.GetFormattedCellValue(cell_salemancode), item.IsSelectedResource);
                                                        if (flagResource)
                                                        {
                                                            if (LoopDatetimeValid(item, Utils.GetFormattedCellValue(cell_soDocDate), "yyyymmdd"))
                                                            {
                                                                result = GetRowValue(row, item.ProjectNumber, userId);
                                                                if (result.number == 1)
                                                                {
                                                                    countRow += result.number;
                                                                }
                                                                else
                                                                {
                                                                    countRow += 0;
                                                                }
                                                            }
                                                        } // resource
                                                    } // product
                                                } // district
                                            } // region
                                        } // province
                                    } // channel
                                } // customer
                            }
                        }
                        // END LOOP SHEET INHOUSE
                        // Read and save sheet VA05
                        for (int j = 1; j <= sheet_VA05.LastRowNum; j++)
                        {
                            MarketingDataRawDto dataRawDto = new MarketingDataRawDto();
                            // Check order no whether exists in table MarketingProjectDataRaw
                            IRow row = sheet_VA05.GetRow(j);
                            ICell cell_orderNo = row.GetCell(0) != null ? row.GetCell(0) : row.CreateCell(0); // ORDER NO
                            ICell cell_custCode = row.GetCell(1) != null ? row.GetCell(1) : row.CreateCell(1); // Cust Code
                            ICell cell_ProductName = row.GetCell(3) != null ? row.GetCell(3) : row.CreateCell(3); // Cust Code
                            ICell cell_saty = row.GetCell(4) != null ? row.GetCell(4) : row.CreateCell(4); // Saty
                            ICell cell_docDate = row.GetCell(6) != null ? row.GetCell(6) : row.CreateCell(6); // Doc date
                            ICell cell_poNumber = row.GetCell(9) != null ? row.GetCell(9) : row.CreateCell(9); // Sale man
                            ICell cell_orderQty = row.GetCell(12) != null ? row.GetCell(12) : row.CreateCell(12); // Order Qty
                            ICell cell_confirmQty = row.GetCell(17) != null ? row.GetCell(17) : row.CreateCell(17); // Confirm Qty
                            ICell cell_product = row.GetCell(25) != null ? row.GetCell(25) : row.CreateCell(25); // Material
                            ICell cell_netValue = row.GetCell(33) != null ? row.GetCell(33) : row.CreateCell(33); // Net value
                            ICell cell_net = row.GetCell(34) != null ? row.GetCell(34) : row.CreateCell(34); // Net
                            ICell cell_rj = row.GetCell(49) != null ? row.GetCell(49) : row.CreateCell(49); // RJ                            
                            dataRawDto.ORDER_NO = Utils.GetFormattedCellValue(cell_orderNo);
                            dataRawDto.CustCode = Utils.GetFormattedCellValue(cell_custCode);
                            dataRawDto.PONumber = Utils.GetFormattedCellValue(cell_poNumber);
                            dataRawDto.SalemanCode = Utils.GetFormattedCellValue(cell_poNumber);
                            dataRawDto.ConfirmQty = int.Parse(Utils.GetFormattedCellValue(cell_confirmQty));
                            dataRawDto.OrderQty = int.Parse(Utils.GetFormattedCellValue(cell_orderQty));
                            dataRawDto.RJ = Utils.GetFormattedCellValue(cell_rj);
                            dataRawDto.Product = Utils.GetFormattedCellValue(cell_product);
                            dataRawDto.ProductName = Utils.GetFormattedCellValue(cell_ProductName);
                            dataRawDto.SODoc_Date = Utils.GetFormattedCellValue(cell_docDate);
                            dataRawDto.Saty = Utils.GetFormattedCellValue(cell_saty);
                            dataRawDto.NetValue = double.Parse(Utils.GetFormattedCellValue(cell_netValue));
                            dataRawDto.Net = double.Parse(Utils.GetFormattedCellValue(cell_net));
                            dataRawDto.MarketingProjectId = item.ProjectNumber; // Assign Project Number
                            dataRawDto.PickedOriginal = Constant.VA05; // Picked original
                            // get province from custcode
                            CustomerDto customerDto = new CustomerDto();
                            customerDto.CustCode = Utils.GetFormattedCellValue(cell_custCode);
                            customerDto = customerDao.GetByCustCode(customerDto);
                            bool orderNo = CheckOrderNoVA05(dataRawDto);
                            if (orderNo)
                            {
                                // Check Sale man whether OB or TELE
                                if (dataRawDto.PONumber.ToUpper().IndexOf(Constant.VA05_OB) > 0
                                    || dataRawDto.PONumber.ToUpper().IndexOf(Constant.VA05_TELE) > 0)
                                {
                                    // Check Confirm Qty # 0 or RJ # Blank, # CA, # I8, # I9
                                    bool rj = !dataRawDto.RJ.Equals("CA") &&
                                            !dataRawDto.RJ.Equals("I8") &&
                                            !dataRawDto.RJ.Equals("I9");
                                    if (rj)
                                    {
                                        if ((dataRawDto.ConfirmQty != 0 && string.IsNullOrEmpty(dataRawDto.RJ))
                                            || (dataRawDto.ConfirmQty == 0 && !string.IsNullOrEmpty(dataRawDto.RJ))
                                            || (dataRawDto.ConfirmQty != 0 && !string.IsNullOrEmpty(dataRawDto.RJ)))
                                        {
                                            if (dataRawDto.Net != 0)
                                            {
                                                CustomerDto customer = dataRawDao.GetCustomerByCustCode(dataRawDto.CustCode);
                                                if (customer != null)
                                                {
                                                    dataRawDto.CustCode = customer.CustCode;
                                                    dataRawDto.CustName = customer.CustName;
                                                    dataRawDto.Channel = customer.Channel;
                                                    dataRawDto.CGrpCode = customer.CGrpCode;
                                                    dataRawDto.CustGroup = customer.CGrpName;
                                                    dataRawDto.Region = customer.Region;
                                                    dataRawDto.Addr = customer.Addr;
                                                    dataRawDto.Provine = customer.Province;
                                                    dataRawDto.District = customer.District;
                                                    dataRawDto.CreateBy = userId;
                                                    bool flagChannel = false;
                                                    bool flagRegion = false;
                                                    bool flagDistrict = false;
                                                    bool flagProduct = false;
                                                    bool flagProvince = false;
                                                    bool flagCustomer = LoopInExCludedCustomers(InExcludedDto.CustCodeList, dataRawDto.CustCode,
                                                                                                item.InExCludedCust);
                                                    if (flagCustomer)
                                                    {
                                                        flagChannel = LoopInExCludedChannel(custgroupCode,
                                                                                dataRawDto.CGrpCode, item.InExCludedChannel);
                                                        if (flagChannel)
                                                        {
                                                            flagProvince = LoopMathProvince(InExcludedDto.ProvinceList, customerDto.Province, item.InExCludedProvince);
                                                            if (flagProvince)
                                                            {
                                                                flagRegion = LoopInExcludedRegion(InExcludedDto.RegionList,
                                                                                dataRawDto.Region, item.InExCludedRegion);
                                                                if (flagRegion)
                                                                {
                                                                    flagDistrict = LoopInExcludedDistrict(InExcludedDto.DistrictList,
                                                                                    dataRawDto.District, item.InExCludedDistrict);
                                                                    if (flagDistrict)
                                                                    {
                                                                        flagProduct = LoopMatchProducts(InExcludedDto.ProductList, dataRawDto.Product);
                                                                        if (flagProduct)
                                                                        {
                                                                            if (LoopDatetimeValid(item, dataRawDto.SODoc_Date, "dd.MM.yyyy"))
                                                                            {
                                                                                // check saty whether ZVRE then * -1
                                                                                if (dataRawDto.Saty.ToUpper().Equals("ZVRE"))
                                                                                {
                                                                                    dataRawDto.Qty = -1 * dataRawDto.OrderQty;
                                                                                    dataRawDto.TT = -1 * dataRawDto.Net;
                                                                                }
                                                                                else
                                                                                {
                                                                                    dataRawDto.Qty = dataRawDto.OrderQty;
                                                                                    dataRawDto.TT = dataRawDto.Net;
                                                                                }
                                                                                // Save data into table MarketingDataRaw
                                                                                // New Guid
                                                                                dataRawDto.Id = Guid.NewGuid();
                                                                                dataRawDto.SODoc_Date = dataRawDto.SODoc_Date.Substring(6, 4) +
                                                                                        dataRawDto.SODoc_Date.Substring(3, 2) + dataRawDto.SODoc_Date.Substring(0, 2); // yyyymmdd
                                                                                int cnt = dataRawDao.InsertDataRaw(dataRawDto);
                                                                                if (cnt == 1)
                                                                                {
                                                                                    countRow += cnt;
                                                                                }
                                                                            } // Doc Date
                                                                        } // Flag Product
                                                                    } // Flag District
                                                                } // Flag Region
                                                            } // Flag Province
                                                        } // Flag Channel
                                                    } // Flag Customer
                                                } // Customer information
                                            } // Net != 0                      
                                        } // Confirm Qty & RJ
                                    } // Check Confirm Qty # 0 and RJ # 0, # CA, # I8, # I9
                                } // Check Sale man whether OB or TELE
                            } // Order No VS Document
                        } // End loop
                        // END LOOP SHEET VA05
                        // UPDATE PROJECT STATUS                        
                        projDto.ProjectStatus = Constant.INPROGRESS;
                        int cntPrjStatus = projectDao.UpdateProjectStatus(projDto);
                        if (countRow > 0)
                        {
                            result.number = countRow;
                            result.code = Constant.INSERT;
                            result.message = string.Format(Constant.PROCESS_MESSAGE, countRow);
                        }
                        else
                        {
                            result.code = Constant.ERROR;
                        }
                    }
                }
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check included and exclude
        /// </summary>
        /// <param name="InExClude"></param>
        /// <returns></returns>
        private bool LoopInExCludedCustomers(List<String> customers, string cellCustomer, string inexcluded)
        {
            bool flag = true;
            if (inexcluded.Equals(Constant.ALL))
            {
                flag = true;
            }
            else if (customers != null && customers.Count() > 0)
            {
                foreach (string customer in customers)
                {
                    if (inexcluded.Equals(Constant.INCLUDED))
                    {
                        if (customer.Equals(cellCustomer))
                        {
                            flag = true;
                            break;
                        }
                    }
                    else if (inexcluded.Equals(Constant.EXCLUDED))
                    {
                        if (customer.Equals(cellCustomer))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// Loop InExCluded Channel
        /// </summary>
        /// <param name="channelList"></param>
        /// <returns></returns>
        private bool LoopInExCludedChannel(IEnumerable<CGrpCodeDto> channel, string cellChannel, string inexcluded)
        {
            bool flag = true;
            foreach (CGrpCodeDto custGroup in channel)
            {
                if (inexcluded.Equals(Constant.INCLUDED))
                {
                    if (custGroup.CGroupCode.ToUpper().Equals(cellChannel))
                    {
                        flag = true;
                        break;
                    }
                }
                else if (inexcluded.Equals(Constant.EXCLUDED))
                {
                    if (custGroup.CGroupCode.ToUpper().Equals(cellChannel))
                    {
                        flag = false;
                        break;
                    }
                }
                else if (inexcluded.Equals(Constant.ALL))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// Loop Match Products
        /// </summary>
        /// <returns></returns>
        private bool LoopMatchProducts(List<String> products, string cellProduct)
        {
            bool flag = false;
            if (products != null && products.Count() > 0)
            {
                foreach (string product in products)
                {
                    if (product.Equals(cellProduct))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            
            return flag;
        }

        /// <summary>
        /// Loop included / excluded province
        /// </summary>
        /// <param name="provinces"></param>
        /// <param name="cellProvince"></param>
        /// <param name="inexcluded"></param>
        /// <returns></returns>
        private bool LoopMathProvince(List<String> provinces, string cellProvince, string inexcluded)
        {
            bool flag = true;
            if (provinces != null && provinces.Count() > 0)
            {
                foreach (string province in provinces)
                {
                    if (inexcluded.Equals(Constant.INCLUDED))
                    {
                        if (province.Equals(cellProvince))
                        {
                            flag = true;
                            break;
                        }
                    }
                    else if (inexcluded.Equals(Constant.EXCLUDED))
                    {
                        if (province.Equals(cellProvince))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (inexcluded.Equals(Constant.ALL))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// Loop InExcluded Region
        /// </summary>
        /// <param name="RegionList"></param>
        /// <param name="cellRegion"></param>
        /// <param name="inexcluded"></param>
        /// <returns></returns>
        private bool LoopInExcludedRegion(List<String> RegionList, string cellRegion, string inexcluded)
        {
            bool flag = true;
            if (RegionList != null && RegionList.Count() > 0)
            {
                foreach (string region in RegionList)
                {
                    if (inexcluded.Equals(Constant.INCLUDED))
                    {
                        if (region.Equals(cellRegion))
                        {
                            flag = true;
                            break;
                        }
                    }
                    else if (inexcluded.Equals(Constant.EXCLUDED))
                    {
                        if (region.Equals(cellRegion))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (inexcluded.Equals(Constant.ALL))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// Loop InExcluded District
        /// </summary>
        /// <param name="DistrictList"></param>
        /// <param name="cellDistrict"></param>
        /// <param name="inexcluded"></param>
        /// <returns></returns>
        private bool LoopInExcludedDistrict(List<String> DistrictList, string cellDistrict, string inexcluded)
        {
            bool flag = true;
            if (DistrictList != null && DistrictList.Count > 0 && !inexcluded.Equals(Constant.ALL))
            {
                foreach (string district in DistrictList)
                {
                    if (inexcluded.Equals(Constant.INCLUDED))
                    {
                        if (district.ToUpper().Equals(cellDistrict.ToUpper()))
                        {
                            flag = true;
                            break;
                        }
                    }
                    else if (inexcluded.Equals(Constant.EXCLUDED))
                    {
                        if (district.ToUpper().Equals(cellDistrict.ToUpper()))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            else if (inexcluded.Equals(Constant.ALL))
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Loop match resourse selected
        /// </summary>
        /// <param name="ResourceList"></param>
        /// <param name="cellResource"></param>
        /// <returns></returns>
        private bool LoopResourceSale(List<String> ResourceList, string cellResource, string IsSelectedResource)
        {
            bool flag = false;
            if (Constant.ALL.Equals(IsSelectedResource))
            {
                flag = true; // GET ALL RESOURCE                  
            }
            else if (Constant.SELECTED.Equals(IsSelectedResource))
            {
                foreach (string resource in ResourceList)
                {
                    if (resource.Equals(cellResource))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// Check datetime in range valid start and the end of marketing project
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        private bool LoopDatetimeValid(MarketingProjectDto projectDto, string cellSODoc_Date, string format)
        {
            bool flag = false;
            try
            {
                DateTime validFrom = new DateTime(int.Parse(projectDto.ValidFrom.Substring(6, 4)),
                                                  int.Parse(projectDto.ValidFrom.Substring(3, 2)),
                                                  int.Parse(projectDto.ValidFrom.Substring(0, 2)));
                DateTime validTo = new DateTime(int.Parse(projectDto.ValidTo.Substring(6, 4)),
                                                  int.Parse(projectDto.ValidTo.Substring(3, 2)),
                                                  int.Parse(projectDto.ValidTo.Substring(0, 2)));
                if (!string.IsNullOrEmpty(cellSODoc_Date) && cellSODoc_Date.Length == 8)
                {
                    DateTime soDocDate = new DateTime(int.Parse(cellSODoc_Date.Substring(0, 4)),
                                                  int.Parse(cellSODoc_Date.Substring(4, 2)),
                                                  int.Parse(cellSODoc_Date.Substring(6, 2)));
                    if (soDocDate >= validFrom && soDocDate <= validTo)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// Read Row Value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="projectNumber"></param>
        /// <returns></returns>
        private ResultMessageDto GetRowValue(IRow row, string projectNumber, string userId)
        {
            ResultMessageDto result = new ResultMessageDto();
            string cellPosition = string.Empty;
            string rowNumber = string.Empty;
            try
            {
                DataFormatter dataFormatter = new DataFormatter(CultureInfo.CurrentCulture);
                MarketingDataRawDto dataRawDto = new MarketingDataRawDto();
                dataRawDto.Id = Guid.NewGuid(); // Marketing project Id
                dataRawDto.PickedOriginal = Constant.INHOUSE; // Picked original
                dataRawDto.MarketingProjectId = projectNumber; // SET PROJECT NUMBER
                dataRawDto.PRINCIPAL = Utils.GetFormattedCellValue(row.GetCell(2)); // PRINCIPAL
                dataRawDto.Product = Utils.GetFormattedCellValue(row.GetCell(3)); // PRODUCT
                dataRawDto.ProductName = Utils.GetFormattedCellValue(row.GetCell(4)); // PRODUCT NAME
                dataRawDto.CustCode = Utils.GetFormattedCellValue(row.GetCell(5)); // CUSTOMER CODE
                dataRawDto.CustName = Utils.GetFormattedCellValue(row.GetCell(6)); // CUSTOMER NAME
                dataRawDto.Addr = Utils.GetFormattedCellValue(row.GetCell(8)); // ADDRESS
                dataRawDto.Provine = Utils.GetFormattedCellValue(row.GetCell(9)); // PROVINE
                dataRawDto.CGrpCode = Utils.GetFormattedCellValue(row.GetCell(11)); // CGrpCode
                dataRawDto.CustGroup = Utils.GetFormattedCellValue(row.GetCell(12)); // CUSTGROUP
                dataRawDto.Region = Utils.GetFormattedCellValue(row.GetCell(13)); // REGION
                dataRawDto.District = Utils.GetFormattedCellValue(row.GetCell(14)); // DISTRICT
                if (row.GetCell(16) != null)
                {
                    dataRawDto.BillingDate = Utils.GetFormattedCellValue(row.GetCell(16));
                    cellPosition = "BILLINGDATE " + dataRawDto.BillingDate;
                    rowNumber = row.GetCell(16).RowIndex.ToString();
                }
                dataRawDto.Reason = Utils.GetFormattedCellValue(row.GetCell(17)); // Reason
                if (row.GetCell(18) != null)
                {
                    dataRawDto.Qty = int.Parse(Utils.GetFormattedCellValue(row.GetCell(18))); // QTY
                    cellPosition = "QTY " + dataRawDto.Qty;
                    rowNumber = row.GetCell(18).RowIndex.ToString();
                }
                dataRawDto.SalemanCode = Utils.GetFormattedCellValue(row.GetCell(20)); // SALE MAN CODE | PERNR
                if (row.GetCell(24) != null)
                {
                    dataRawDto.TT = double.Parse(Utils.GetFormattedCellValue(row.GetCell(24))); // TT
                    cellPosition = "TT " + dataRawDto.TT;
                    rowNumber = row.GetCell(24).RowIndex.ToString();
                }
                if (row.GetCell(26) != null)
                {
                    dataRawDto.TTV = double.Parse(Utils.GetFormattedCellValue(row.GetCell(26))); // TTV
                    cellPosition = "TTV " + dataRawDto.TTV;
                    rowNumber = row.GetCell(26).RowIndex.ToString();
                }
                if (row.GetCell(30) != null)
                {
                    dataRawDto.ORDER_NO = Utils.GetFormattedCellValue(row.GetCell(30)); // ORDER_NO
                    cellPosition = "ORDER NO " + dataRawDto.ORDER_NO;
                    rowNumber = row.GetCell(30).RowIndex.ToString();
                }
                if (row.GetCell(44) != null)
                {
                    dataRawDto.SODoc_Date = Utils.GetFormattedCellValue(row.GetCell(44)); // SODoc_Date
                    cellPosition = "SODoc_Date " + dataRawDto.SODoc_Date;
                    rowNumber = row.GetCell(44).RowIndex.ToString();
                }
                dataRawDto.Channel = Utils.GetFormattedCellValue(row.GetCell(46)); // Channel
                dataRawDto.CreateBy = userId;
                result.number = dataRawDao.InsertDataRaw(dataRawDto); // SAVE INTO TABLE MarketingDataRaw
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = string.Format(Constant.GET_CELL_ERROR, cellPosition + "-" + ex.Message);
                //throw ex;
            }
            return result;
        }

        /// <summary>
        /// Get Project Selected
        /// </summary>
        /// <param name="projectIdList"></param>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> GetProjectSelected(ProjectProcessParam projectIdList)
        {
            IEnumerable<MarketingProjectDto> lst = projectDao.GetProjectSelected(projectIdList);
            return lst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Marketing Project By Project Number
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        public IEnumerable<MarketingDataRawDto> GetMarketingProjectByProjectNumber(MarketingProjectDto projectDto)
        {
            IEnumerable<MarketingDataRawDto> lst = dataRawDao.GetMarketingProjectByProjectNumber(projectDto);
            return lst;
            throw new NotImplementedException();
        }


        /// <summary>
        /// Export Data Pick
        /// </summary>
        /// <param name="projectDataPick"></param>
        /// <returns></returns>
        public IWorkbook ExportDataPick(IEnumerable<MarketingDataRawDto> projectDataPick, MarketingProjectDto projectDto)
        {
            // GET PROJECT INFORMATION
            MarketingProjectDto projectObj = projectDao.GetProjectById(projectDto);
            IWorkbook workbook;
            string extension = Constant.EXCEL_XLSX;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "ExportDataPick.xlsx";
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                try
                {
                    if (Constant.EXCEL_XLSX.Equals(extension))
                    {
                        workbook = new XSSFWorkbook(file);
                    }
                    else
                    {
                        throw new Exception("This format is not supported");
                    }

                    ISheet sheet_raw = workbook.GetSheet("DataRawPicked"); // RAW

                    int rowIdx = 1;
                    foreach (MarketingDataRawDto item in projectDataPick)
                    {
                        IRow row = sheet_raw.CreateRow(rowIdx);
                        row.CreateCell(0).SetCellValue(item.CustCode); // Cust code
                        row.CreateCell(1).SetCellValue(item.CustName); // Cust name
                        row.CreateCell(2).SetCellValue(item.Product); // Product
                        row.CreateCell(3).SetCellValue(item.ProductName); // Product name
                        row.CreateCell(4).SetCellValue(item.Addr); // Address
                        row.CreateCell(5).SetCellValue(item.PRINCIPAL); // Principal
                        row.CreateCell(6).SetCellValue(item.Provine); // Provine                    
                        row.CreateCell(7).SetCellValue(item.CustGroup); // Cust group
                        row.CreateCell(8).SetCellValue(item.Region); // Region
                        row.CreateCell(9).SetCellValue(item.District); // District
                        row.CreateCell(10).SetCellValue(item.BillingDate); // BillingDate
                        row.CreateCell(11).SetCellValue(item.Reason); // Reason
                        row.CreateCell(12).SetCellValue(item.Qty.ToString()); // Quantity
                        row.CreateCell(13).SetCellValue(item.SalemanCode); // SalemanCode
                        row.CreateCell(14).SetCellValue(item.TT.ToString()); // TT
                        row.CreateCell(15).SetCellValue(item.TTV.ToString()); // TTV
                        row.CreateCell(16).SetCellValue(item.ORDER_NO); // Order no
                        row.CreateCell(17).SetCellValue(item.SODoc_Date); // SoDocDate
                        rowIdx += 1;
                    }

                    IEnumerable<MarketingDataRawDto> dataRawDetail = dataRawDao.MarketingDataRawDetail(projectObj);
                    IEnumerable<MarketingDataRawDto> custCode = dataRawDao.GetCustCodeLargerThanZero(projectObj);
                    var color_title = new XSSFColor(new byte[] { 243, 56, 6 });
                    var color_green = new XSSFColor(new byte[] { 35, 190, 16 });
                    var font_red = (XSSFFont)workbook.CreateFont();
                    font_red.SetColor(color_title);
                    font_red.IsBold = true;
                    var font_green = (XSSFFont)workbook.CreateFont();
                    font_green.SetColor(color_green);
                    var style_title = (XSSFCellStyle)workbook.CreateCellStyle();
                    style_title.SetFont(font_red);
                    var style_green = (XSSFCellStyle)workbook.CreateCellStyle();
                    style_green.SetFont(font_green);
                    int detailIdx = 1;
                    ISheet sheet_detail = workbook.GetSheet("Detail"); // DETAIL
                    IRow projectName = sheet_detail.GetRow(0);
                    projectName.CreateCell(14).SetCellValue(projectObj.ProjectName);
                    projectName.GetCell(14).CellStyle = style_title;
                    foreach (MarketingDataRawDto item in dataRawDetail)
                    {
                        string channel = string.Empty;
                        IRow row = sheet_detail.CreateRow(detailIdx);
                        row.CreateCell(0).SetCellValue(item.CustCode); // Cust code                                                                               
                        row.CreateCell(1).SetCellValue(item.CustName); // Cust name
                        row.CreateCell(2).SetCellValue(item.Addr); // Address
                        row.CreateCell(3).SetCellValue(item.District); // District
                        row.CreateCell(4).SetCellValue(item.Provine); // Provine
                        row.CreateCell(5).SetCellValue(item.Region); // Region
                        channel = dataRawDao.GetChannelByCGrpCode(item.CGrpCode); // get channel by custgroup
                        row.CreateCell(6).SetCellValue(channel); // Channel
                        row.CreateCell(7).SetCellValue(item.Product); // Product
                        row.CreateCell(8).SetCellValue(item.Qty != null ? item.Qty.ToString() : string.Empty); // Quantity
                        row.CreateCell(9).SetCellValue(item.TT != null ? item.TT.ToString() : string.Empty); // Sale value TT
                        /* Check qualified in outlet */
                        if (projectDto.CampaignType.Equals(Constant.OUTLET))
                        {
                            if (projectObj.MinQuantity > 0 && projectObj.MinTarget == 0)
                            {
                                if (item.Qty >= projectObj.MinQuantity)
                                {
                                    row.CreateCell(10).SetCellValue(Constant.QUALIFIED);
                                    row.GetCell(10).CellStyle = style_green;
                                }
                                else
                                {
                                    row.CreateCell(10).SetCellValue(Constant.UNQUALIFIED);
                                    row.GetCell(10).CellStyle = style_title;
                                }
                            }
                            else if (projectObj.MinTarget > 0 && projectObj.MinQuantity == 0)
                            {
                                if ((decimal)item.TT >= projectObj.MinTarget)
                                {
                                    row.CreateCell(10).SetCellValue(Constant.QUALIFIED);
                                    row.GetCell(10).CellStyle = style_green;
                                }
                                else
                                {
                                    row.CreateCell(10).SetCellValue(Constant.UNQUALIFIED);
                                    row.GetCell(10).CellStyle = style_title;
                                }
                            }
                            else if (projectObj.MinTarget > 0 && projectObj.MinQuantity > 0)
                            {
                                if ((decimal)item.TT >= projectObj.MinTarget && item.Qty >= projectObj.MinQuantity)
                                {
                                    row.CreateCell(10).SetCellValue(Constant.QUALIFIED);
                                    row.GetCell(10).CellStyle = style_green;
                                }
                                else
                                {
                                    row.CreateCell(10).SetCellValue(Constant.UNQUALIFIED);
                                    row.GetCell(10).CellStyle = style_title;
                                }
                            }
                        }
                        // Incharge
                        row.CreateCell(11).SetCellValue("");
                        foreach (MarketingDataRawDto code in custCode)
                        {
                            if (code.CustCode.Equals(item.CustCode))
                            {
                                for (int n = 0; n <= 9; n++)
                                {
                                    row.GetCell(n).CellStyle = style_title;
                                }
                            }
                        }
                        detailIdx += 1;
                    }

                    // TITLE REGION
                    IRow title_row = sheet_detail.GetRow(1);
                    title_row.CreateCell(13).SetCellValue("#");
                    title_row.GetCell(13).CellStyle = style_title;
                    title_row.CreateCell(14).SetCellValue("Qty boxes");
                    title_row.GetCell(14).CellStyle = style_title;
                    title_row.CreateCell(15).SetCellValue("Sale Value");
                    title_row.GetCell(15).CellStyle = style_title;
                    title_row.CreateCell(16).SetCellValue("Qualified OL");
                    title_row.GetCell(16).CellStyle = style_title;
                    IEnumerable<MarketingDataRawDto> sumByRegion = dataRawDao.SumByRegion(projectObj);
                    IEnumerable<MarketingDataRawDto> sumAmountBySaleman = dataRawDao.SumAmountBySaleManCode(projectObj);
                    List<string> regionList = new List<string>();
                    if (projectObj.InExCludedRegion.Equals(Constant.ALL))
                    {
                        IEnumerable<RegionDto> regions = projectDao.GetRegion();
                        foreach (RegionDto region in regions)
                        {
                            regionList.Add(region.Code);
                        }
                    }
                    // WRITE REGION
                    int i = 2;
                    int? totalQty = 0;
                    double totalAmt = 0;
                    double totalOutlet = 0;
                    foreach (MarketingDataRawDto region in sumByRegion)
                    {
                        IRow row = sheet_detail.GetRow(i);
                        row.CreateCell(13).SetCellValue(region.Region);
                        row.CreateCell(14).SetCellValue(region.Qty.ToString());
                        row.CreateCell(15).SetCellValue(region.TT.ToString());
                        row.CreateCell(16).SetCellValue(region.QualifiedOL != null ? region.QualifiedOL.ToString() : "");
                        totalQty += region.Qty != null ? region.Qty : 0;
                        totalAmt += region.TT != null ? (double)region.TT : 0;
                        totalOutlet += region.QualifiedOL != null ? (double)region.QualifiedOL : 0;
                        i += 1;
                    }

                    // WRITE ANGENT
                    IRow project = sheet_detail.GetRow(21);
                    project.CreateCell(15).SetCellValue(projectObj.ProjectName);
                    project.GetCell(15).CellStyle = style_title;
                    IRow title_sale = sheet_detail.GetRow(22);
                    title_sale.CreateCell(13).SetCellValue("Saleman code");
                    title_sale.GetCell(13).CellStyle = style_title;
                    title_sale.CreateCell(14).SetCellValue("Full name");
                    title_sale.GetCell(14).CellStyle = style_title;
                    title_sale.CreateCell(15).SetCellValue("Sales Vaue (-VAT)");
                    title_sale.GetCell(15).CellStyle = style_title;
                    title_sale.CreateCell(16).SetCellValue("Qualified OL");
                    title_sale.GetCell(16).CellStyle = style_title;

                    // Sum amount by sale man code
                    int m = 23;
                    foreach (MarketingDataRawDto item in sumAmountBySaleman)
                    {
                        IRow row = sheet_detail.GetRow(m);
                        row.CreateCell(13).SetCellValue(item.SalemanCode);
                        row.CreateCell(14).SetCellValue(item.FullName);
                        row.CreateCell(15).SetCellValue(item.TT.ToString());
                        row.CreateCell(16).SetCellValue(item.QualifiedOL != null ? item.QualifiedOL.ToString() : "");
                        m += 1;
                    }
                    // Grand total
                    IRow grandRow = sheet_detail.GetRow(sumByRegion.Count() + 2);
                    grandRow.CreateCell(13).SetCellValue("GRAND TOTAL");
                    grandRow.GetCell(13).CellStyle = style_title;
                    grandRow.CreateCell(14).SetCellValue(totalQty.ToString());
                    grandRow.GetCell(14).CellStyle = style_title;
                    grandRow.CreateCell(15).SetCellValue(totalAmt.ToString());
                    grandRow.GetCell(15).CellStyle = style_title;
                    grandRow.CreateCell(16).SetCellValue(totalOutlet.ToString());
                    grandRow.GetCell(16).CellStyle = style_title;

                    /* SUMMARY BY PRODUCT GROUP */
                    ISheet sheet_productgroup = workbook.GetSheet("ProductGroup");
                    //int pgRowIdx = 1;
                    IEnumerable<ProductsGroupDto> productsGroupList = dataRawDao.GetProductGroupByProject(projectObj);
                    if (productsGroupList != null && productsGroupList.Count() > 0) {
                        foreach (ProductsGroupDto item in productsGroupList)
                        {
                            ProductsGroupDto summary = dataRawDao.SummaryDataByProductGroup(item);
                            if (summary != null)
                            {
                                summary.ProductCode = item.ProductCode;
                                summary.ProductGroup = item.ProductGroup;
                                summary.ProjectNumber = projectObj.ProjectNumber;
                                int udt = dataRawDao.UpdateSummaryProduct(summary);
                            }
                        }
                        IEnumerable<ProductsGroupDto> summaryPG = dataRawDao.SummaryProductGroup(projectObj);
                        int pgIndex = 1;
                        foreach (ProductsGroupDto idx in summaryPG)
                        {
                            IRow row = sheet_productgroup.CreateRow(pgIndex);
                            row.CreateCell(0).SetCellValue(idx.ProductGroup);
                            row.CreateCell(1).SetCellValue(string.Format("{0:n0}", idx.SumQuantity));
                            row.CreateCell(2).SetCellValue(string.Format("{0:n0}", idx.SumAmount));
                            pgIndex += 1;
                        }
                    }
                    file.Close();
                    return workbook;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Count Data Raw By Project Number
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<DataRawResult> CountDataRawByProjectNumber(ProjectProcessParam ProjectList)
        {
            IEnumerable<DataRawResult> result = dataRawDao.CountDataRawByProjectNumber(ProjectList);
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Project Name List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> GetProjectNameList()
        {
            IEnumerable<MarketingProjectDto> lst = projectDao.GetProjectNameList();
            return lst;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Project Over View
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> ProjectOverView(MarketingProjectDto param)
        {
            try
            {
                IEnumerable<MarketingProjectDto> list = projectDao.ProjectOverView(param);
                DateTime newDate = DateTime.Now;
                if (list != null && list.Count() > 0)
                {
                    foreach (MarketingProjectDto item in list)
                    {
                        DateTime validFrom = DateTime.ParseExact(item.ValidFrom, ddMMyyyy, CultureInfo.InvariantCulture);
                        DateTime validTo = DateTime.ParseExact(item.ValidTo, ddMMyyyy, CultureInfo.InvariantCulture);
                        int compare = DateTime.Compare(validTo, newDate);
                        double realdate = (newDate - validFrom).Days;
                        double period = (validTo - validFrom).Days;
                        double perDay = realdate / period;
                        if (compare < 0)
                        {
                            item.RunRateMinTarget = 0;
                            item.RunRateMaxTarget = 0;
                        }
                        else if (item.MinTarget == 1)
                        {
                            item.RunRateMinTarget = 100;
                            item.RunRateMaxTarget = 100;
                        }
                        else
                        {
                            item.RunRateMinTarget = (Convert.ToDouble(item.PerformanceArchive) / (Convert.ToDouble(item.MinTarget) * perDay)) * 100;
                            item.RunRateMaxTarget = (Convert.ToDouble(item.PerformanceArchive) / (Convert.ToDouble(item.MaxTarget) * perDay)) * 100;
                        }
                    }
                }                
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
        }

        public ResultMessageDto UpdateDataPick(HttpPostedFileBase aFile, MarketingProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            IWorkbook workbook = null;
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
                int delDataRaw = projectDao.DeleteDataRawByProject(dto);
                ISheet sheet = workbook.GetSheet("Analysic");
                var range = "A1:BD1";
                var cellRange = CellRangeAddress.ValueOf(range);
                ColumnsDto columnsDto = new ColumnsDto();
                int rowCount = 0;
                for (var i = cellRange.FirstColumn; i <= cellRange.LastColumn; i++)
                {
                    var cr = new CellReference(0, i);
                    var firstRow = sheet.GetRow(cr.Row);
                    var cell = firstRow.GetCell(cr.Col);
                    if (cell != null)
                    {
                        int colIdx = cell.ColumnIndex;
                        if (cell.StringCellValue.IndexOf("PRINCIPAL") != -1)
                        {
                            columnsDto.PRINCIPAL = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("PRODUCT") != -1)
                        {
                            columnsDto.PRODUCT = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("KUNNR") != -1)
                        {
                            columnsDto.KUNNR = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("NAME1") != -1)
                        {
                            columnsDto.CustName = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("ADDRESS") != -1)
                        {
                            columnsDto.ADDRESS = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("PROVINCE") != -1)
                        {
                            columnsDto.PROVINCE = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("CUSTGROUP") != -1)
                        {
                            columnsDto.CUSTGROUP = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("REGION") != -1)
                        {
                            columnsDto.REGION = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("DISTRICT") != -1)
                        {
                            columnsDto.DISTRICT = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("VBELN") != -1)
                        {
                            columnsDto.VBELN = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("BILLINGDATE") != -1)
                        {
                            columnsDto.BILLINGDATE = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("REASON") != -1)
                        {
                            columnsDto.REASON = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("Qty") != -1)
                        {
                            columnsDto.Qty = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("PERNR") != -1)
                        {
                            columnsDto.PERNR = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("ORDER_NO") != -1)
                        {
                            columnsDto.ORDER_NO = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("VAT") != -1 && cell.StringCellValue.Equals("VAT"))
                        {
                            columnsDto.VAT = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("TT") != -1 && cell.StringCellValue.Equals("TT"))
                        {
                            columnsDto.TT = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("TTV") != -1 && cell.StringCellValue.Equals("TTV"))
                        {
                            columnsDto.TTV = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("SODoc_Date") != -1)
                        {
                            columnsDto.SODoc_Date = colIdx;
                        }
                        else if (cell.StringCellValue.IndexOf("Final pick") != -1 && cell.StringCellValue.Equals("Final pick"))
                        {
                            columnsDto.FinalPick = colIdx;
                        }

                    }
                }
                MarketingDataRawDto obj = null;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    obj = new MarketingDataRawDto();
                    IRow row = sheet.GetRow(i);
                    obj.Id = Guid.NewGuid();
                    obj.MarketingProjectId = dto.ProjectNumber;
                    obj.PRINCIPAL = Utils.GetFormattedCellValue(row.GetCell(columnsDto.PRINCIPAL));
                    obj.Product = Utils.GetFormattedCellValue(row.GetCell(columnsDto.PRODUCT));
                    obj.CustCode = Utils.GetFormattedCellValue(row.GetCell(columnsDto.KUNNR));
                    obj.CustName = Utils.GetFormattedCellValue(row.GetCell(columnsDto.CustName));
                    obj.Addr = Utils.GetFormattedCellValue(row.GetCell(columnsDto.ADDRESS));
                    obj.Provine = Utils.GetFormattedCellValue(row.GetCell(columnsDto.PROVINCE));
                    obj.CustGroup = Utils.GetFormattedCellValue(row.GetCell(columnsDto.CUSTGROUP));
                    obj.Region = Utils.GetFormattedCellValue(row.GetCell(columnsDto.REGION));
                    obj.District = Utils.GetFormattedCellValue(row.GetCell(columnsDto.DISTRICT));
                    obj.VBELN = Utils.GetFormattedCellValue(row.GetCell(columnsDto.VBELN));
                    obj.BillingDate = Utils.GetFormattedCellValue(row.GetCell(columnsDto.BILLINGDATE));
                    obj.Reason = Utils.GetFormattedCellValue(row.GetCell(columnsDto.REASON));
                    obj.Qty = int.Parse(Utils.GetFormattedCellValue(row.GetCell(columnsDto.Qty)).Trim());
                    obj.SalemanCode = Utils.GetFormattedCellValue(row.GetCell(columnsDto.PERNR));
                    obj.ORDER_NO = Utils.GetFormattedCellValue(row.GetCell(columnsDto.ORDER_NO));
                    if (!string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(columnsDto.VAT))))
                    {
                        obj.VAT = double.Parse(Utils.GetFormattedCellValue(row.GetCell(columnsDto.VAT)).Trim());
                    }
                    else
                    {
                        obj.VAT = 0;
                    }
                    if (!string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(columnsDto.TT))))
                    {
                        obj.TT = double.Parse(Utils.GetFormattedCellValue(row.GetCell(columnsDto.TT)).Trim());
                    }
                    else
                    {
                        obj.TT = 0;
                    }
                    if (!string.IsNullOrEmpty(Utils.GetFormattedCellValue(row.GetCell(columnsDto.TTV))))
                    {
                        obj.TTV = double.Parse(Utils.GetFormattedCellValue(row.GetCell(columnsDto.TTV)).Trim());
                    }
                    else
                    {
                        obj.TTV = 0;
                    }
                    obj.SODoc_Date = Utils.GetFormattedCellValue(row.GetCell(columnsDto.SODoc_Date));
                    obj.Channel = Utils.GetFormattedCellValue(row.GetCell(columnsDto.CUSTGROUP));
                    obj.PickedOriginal = Constant.DATA_ANALYSIC;
                    obj.CreateBy = dto.CreateBy;
                    obj.UpdateBy = dto.UpdateBy;
                    int j = dataRawDao.InsertDataRaw(obj);
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