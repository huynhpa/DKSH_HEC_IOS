using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class DataProcessDao
    {
        private GenericRepository<DataProcessDto> dataProcessRepo;
        private GenericRepository<CustomerClassSFADto> customerClassRepo;
        private GenericRepository<SupplierDto> clientRepo;
        private GenericRepository<CSSalesDto> inhouseRepo;
        private CCCToolsEntity cccToolsEntity = null;
        private InHouseEntity inHouseEntity = null;

        public DataProcessDao()
        {
            dataProcessRepo = new GenericRepository<DataProcessDto>(new CCCToolsEntity());
            customerClassRepo = new GenericRepository<CustomerClassSFADto>(new CCCToolsEntity());
            clientRepo = new GenericRepository<SupplierDto>(new CCCToolsEntity());
            inhouseRepo = new GenericRepository<CSSalesDto>(new InHouseEntity());
            cccToolsEntity = new CCCToolsEntity();
            inHouseEntity = new InHouseEntity();
        }

        /// <summary>
        /// SaleByCustCode
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SupplierDto> SaleByCustCode()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_SaleByCustCode]";
            return clientRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int UpdateSuppiers(SupplierDto item)
        {
            object[] parameter = { item.CustCode, item.Suppliers };
            string spQuery = @"EXEC [sp_UpdateSuppiers] {0}, {1}";
            return customerClassRepo.ExecuteCommand(spQuery, parameter);
        }

        public int SaveDataProcess(DataProcessDto dto)
        {
            object[] parameter = { dto.CustCode,dto.CustName,dto.Address,dto.District,dto.Province,
                                    dto.Region,dto.Channel,dto.Client,dto.ClientName,dto.SKUCode,dto.SKUName,dto.Sales,dto.IND };
            string spQuery = @"EXEC [sp_SaveDataProcess] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}";
            return dataProcessRepo.ExecuteCommand(spQuery, parameter);
        }


        public int SaveCustomerClassSFA(CustomerClassSFADto dto)
        {
            object[] parameter = { dto.CustCode, dto.Class };
            string spQuery = @"EXEC [sp_SaveCustomerClassSFA] {0}, {1}";
            return customerClassRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Save Data Sales Inhouse
        /// </summary>
        /// <param name="csSales"></param>
        /// <returns></returns>
        public int SaveDataSalesInhouse(CSSalesDto csSales)
        {
            object[] parameter = { csSales.Plant, csSales.PRINCIPALCODE, csSales.PRINCIPAL, csSales.PRODUCT,
                                    csSales.PRODUCTNAME, csSales.KUNNR, csSales.NAME1, csSales.NAME2, csSales.STRAS1, csSales.STRAS2, csSales.BZIRK, csSales.KDGRP,
                                    csSales.KDGRPDESC, csSales.REGIODESC, csSales.BZIRKDESC, csSales.VBELN, csSales.BUDAT, csSales.REASON, csSales.Qty, csSales.DGLE,
                                    csSales.PERNR, csSales.VAT, csSales.SUOM, csSales.CUSTPAYTO, csSales.TT, csSales.NGUYENGIA, csSales.TTV, csSales.DISCOUNT,
                                    csSales.Code, csSales.Device, csSales.orginv, csSales.BATCH, csSales.CHARGE, csSales.BNAME, csSales.TEXT1 };
            string spQuery = @"EXEC [sp_SaveDataSaleInhouse] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12},
                                                             {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22},
                                                             {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}";
            return inhouseRepo.ExecuteCommandInHouse(spQuery, parameter);
        }
    }
}