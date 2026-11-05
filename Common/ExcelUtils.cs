using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;

namespace DKSHApplication.Common
{
    public static class ExcelUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="workbook"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static DataTable ImportDataFromExcel(IWorkbook workbook, string extension)
        {
            DataTable tb = new DataTable();

            return tb;
        }
    }
}