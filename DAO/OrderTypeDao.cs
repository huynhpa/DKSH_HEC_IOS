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
    public class OrderTypeDao
    {
        private GenericRepository<OrderTypeDto> orderTypeRepo;

        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// 
        /// </summary>
        public OrderTypeDao()
        {
            orderTypeRepo = new GenericRepository<OrderTypeDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }



        /// <summary>
        /// InsertOrderTypeCount
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public int InsertOrderTypeCount(OrderTypeDto orderType)
        {
            object[] parameter = { orderType.Id, orderType.RetSo, orderType.Device, orderType.Potext,
                                   orderType.VBeln, orderType.OrderNo };
            string spQuery = @"EXEC [InsertOrderTypeCount] {0}, {1}, {2}, {3}, {4}, {5}";
            return orderTypeRepo.ExecuteCommand(spQuery, parameter);
        }

    }
}