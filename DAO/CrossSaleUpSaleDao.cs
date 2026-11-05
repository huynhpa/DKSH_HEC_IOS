using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class CrossSaleUpSaleDao
    {
        private GenericRepository<CrossSaleUpSaleReportDto> CSUSRepo;

        private GenericRepository<SalemanPOTextDto> salemanPORepo;

        private GenericRepository<DataSaleCSUSDto> dataSaleRepo;

        private GenericRepository<DataCallsDto> dataCallRepo;

        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// Instance
        /// </summary>
        public CrossSaleUpSaleDao()
        {
            CSUSRepo = new GenericRepository<CrossSaleUpSaleReportDto>(new CCCToolsEntity());
            salemanPORepo = new GenericRepository<SalemanPOTextDto>(new CCCToolsEntity());
            dataSaleRepo = new GenericRepository<DataSaleCSUSDto>(new CCCToolsEntity());
            dataCallRepo = new GenericRepository<DataCallsDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Get GroupSaleman POText
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalemanPOTextDto> GetGroupSalemanPOText(CrossSaleUpSaleReportDto dto)
        {
            object[] param = { Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = "EXEC [sp_GetGroupCodeList] {0}";
            return salemanPORepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get All Saleman POText
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalemanPOTextDto> GetAllSalemanPOText()
        {
            object[] param = { };
            string spQuery = "EXEC [sp_GetAllSalemanPOText]";
            return salemanPORepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Data Call By Month
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<DataCallsDto> GetDataCallByMonth(DataCallSalemanParam pr)
        {
            object[] param = { Utils.ConvertToYYYYMM(pr.Monthly) };
            string spQuery = "EXEC [sp_GetDataCallByMonth] {0}";
            return dataCallRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Delete Saleman POText
        /// </summary>
        /// <returns></returns>
        public int DeleteSalemanPOText()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_DeleteSalemanPOText]";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }


        /// <summary>
        /// Update total invoice
        /// </summary>
        /// <returns></returns>
        public int SummaryItemNoteTotalInvoice(SalemanPOTextDto dto)
        {
            object[] parameter = { Utils.ConvertToYYYYMM(dto.Monthly), dto.POText };
            string spQuery = @"EXEC [sp_SummaryItemNoteTotalInvoice] {0}, {1}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Saleman POText
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertSalemanPOText(SalemanPOTextDto dto)
        {
            object[] parameter = { dto.PERNR, dto.Agent, dto.Team, dto.GroupCode, dto.POText
                                    , dto.SAPExtension, dto.AgentName, Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = @"EXEC [sp_InsertSalemanPOText] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Data Calls
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertDataCalls(DataCallsDto dto)
        {
            object[] parameter = { dto.DataCallsID,dto.AgentId,dto.AgentName,dto.Extension,dto.IntervalStartTime,dto.IntervalEndTime,dto.TotalLoggedInTime
                                    ,dto.CallPresented,dto.CallHandled,dto.CallbackPresent,dto.CallbackHandled,dto.TotalCallsPresented,dto.TotalCallsHandled,dto.CallRatio
                                    ,dto.AverageReservedTime,dto.ReadyTimeTalkTime,dto.PercentReadyTimeTalkTime
                                    ,Utils.ConvertToYYYYMM(dto.Monthly),dto.CreateBy,dto.UpdateBy };
            string spQuery = @"EXEC [sp_InsertDataCalls] {0}, {1}, {2}, {3}, {4}, {5}, {6},
                                                         {7}, {8}, {9}, {10}, {11}, {12}, 
                                                         {13}, {14}, {15}, {16}, {17}, {18}, {19}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Data Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertDataSale(DataSaleCSUSDto dto)
        {
            object[] parameter = { dto.Id, dto.KUNNR, dto.REASON, dto.VBELN, dto.BILLINGDATE, dto.REASON,
                                   dto.QTY, dto.PERNR, dto.TT, dto.NGUYENGIA, dto.TTV, dto.ORDERNO, dto.POTEXT,
                                   dto.SODocDate, dto.ItemNotes, dto.CreateBy, dto.UpdateBy };
            string spQuery = @"EXEC [sp_InsertDataSale] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, 
                                                        {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Search Up Sale Cross Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CrossSaleUpSaleReportDto> SearchUpSaleCrossSale(CrossSaleUpSaleReportDto dto)
        {
            object[] parameter = { dto.Name, dto.AgentUser, dto.GroupCode, Utils.ConvertToYYYYMM(dto.Monthly),
                                   dto.RangeMonth, Utils.ConvertToYYYYMM(dto.FromMonth), Utils.ConvertToYYYYMM(dto.ToMonth) };
            string spQuery = @"EXEC [sp_SearchUpSaleCrossSale] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return CSUSRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Process Data Up Sale Cross Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int ProcessDataUpSaleCrossSale(CrossSaleUpSaleReportDto dto)
        {
            object[] parameter = { dto.UpdateBy, dto.Monthly, dto.SalemanCode, dto.ApproachedCSUP,
                                   dto.SuccessfulCalls, dto.SuccessfulOrderCSUP, dto.SalesValueCSUP, dto.SuccessfulInvoiceApproachedCSUS,
                                   dto.SuccessfulCustomersApproachedCSUS, dto.SalesValueOrderItemnote, dto.TotalInvoices };
            string spQuery = @"EXEC [sp_ProcessDataUpSaleCrossSale] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Insert Data Sale Exist Saleman PO
        /// </summary>
        /// <returns></returns>
        public int InsertDataSaleExistSalemanPO(CrossSaleUpSaleReportDto dto)
        {
            object[] parameter = { dto.UpdateBy, Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = @"EXEC [sp_InsertDataSaleExistSalemanPO] {0}, {1}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// summary data sale 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CrossSaleUpSaleReportDto> SummaryDataSaleUSCS(CrossSaleUpSaleReportDto dto)
        {
            object[] parameter = { Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = @"EXEC [sp_SummaryDataSaleUSCS] {0}";
            return CSUSRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Delete Data Calls
        /// </summary>
        /// <returns></returns>
        public int DeleteDataCalls(DataCallsDto dto)
        {
            object[] parameter = { Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = @"EXEC [sp_DeleteDataCalls] {0}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Delete Data Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int DeleteDataSale(DataSaleParam dto)
        {
            object[] parameter = { Utils.ConvertToYYYYMM(dto.Month) };
            string spQuery = @"EXEC [sp_DeleteDataSaleInhouse] {0}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Calculate Sale Value Item Notes
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CrossSaleUpSaleReportDto> CalculateSaleValueItemNotes(CrossSaleUpSaleReportDto dto)
        {
            object[] parameter = { Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = @"EXEC [sp_CalculateSaleValueItemNotes] {0}";
            return CSUSRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Update Sale Value Item Notes
        /// </summary>
        /// <param name="svi"></param>
        /// <returns></returns>
        public int UpdateSaleValueItemNotes(CrossSaleUpSaleReportDto svi)
        {
            object[] parameter = { svi.SalemanCode, svi.SalesValueOrderItemnote, svi.Monthly };
            string spQuery = @"EXEC [sp_UpdateSaleValueItemNotes] {0}, {1}, {2}";
            return CSUSRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Check DataCall
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal CrossSaleUpSaleReportDto CheckDataCall(CrossSaleUpSaleReportDto dto)
        {
            object[] parameter = { Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = @"EXEC [sp_CheckDataCall] {0}";
            return CSUSRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Check SalemanCode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal SalemanPOTextDto CheckSalemanCode()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_CheckSalemanCode]";
            return salemanPORepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Get List Data Sales By Month
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<DataSaleCSUSDto> GetListDataSalesByMonth(CrossSaleUpSaleReportDto dto)
        {
            object[] parameter = { Utils.ConvertToYYYYMM(dto.Monthly) };
            string spQuery = @"EXEC [sp_GetListDataSales] {0}";
            return dataSaleRepo.ExecuteQuery(spQuery, parameter);
        }


    }
}