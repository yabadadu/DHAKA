using CommonClass.Database.Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hmx.DHAKA.TCS.Environment.Item;
using System.ComponentModel;

namespace Hmx.DHAKA.TCS.Common
{
    public static class DataTableController
    {
        #region METHOD AREA
        public static string[] GetList(DataTable dt, string colName)
        {
            return dt.AsEnumerable().Select(r => r.Field<string>(colName)).ToList().ToArray();
        }

        public static string findValue(DataTable dt, string ColName, string ColValue, string targerColName)
        {
            return dt.AsEnumerable().Where((row) => row.Field<string>(ColName).Equals(ColValue)).Select((row) => row.Field<string>(targerColName)).FirstOrDefault();
        }
        #endregion
    }
}
