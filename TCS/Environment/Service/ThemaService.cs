using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClass.Database.Mapper;
using Hmx.DHAKA.TCS.Environment.Item;

namespace Hmx.DHAKA.TCS.Environment.Service
{
    public class ThemaService
    {
        #region INITIALIZE AREA ********************
        public ThemaService()
        {
        }
        #endregion
        #region METHOD AREA ********************
        public IList<ThemaItem> Inquiry()
        {
            DataTable dataTable;
            IList<ThemaItem> resultItems = new List<ThemaItem>();

            try
            {
                Hashtable parameters = BindDB2Class.BindDBClass2Hashtable("", false);
                //dataTable = CommFunc.RequestHandlerDataTable("", null, parameters);
                dataTable = (new TempData()).GetData();

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    resultItems = BindDB2Class.BindDataTableToListNoFormat<ThemaItem>(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultItems;
        }
        #endregion
    }


    class TempData
    {
        public DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("KEY", typeof(string)));
            dt.Columns.Add(new DataColumn("VALUE", typeof(string)));

            DataRow row = dt.NewRow();
            row["KEY"] = "DevExpress Style";
            row["VALUE"] = "Basic";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = "Office 2013 White";
            row["VALUE"] = "White";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = "Office 2013 Dark Gray";
            row["VALUE"] = "Gray";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = "Office 2010 Blue";
            row["VALUE"] = "Blue";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = "DevExpress Dark Style";
            row["VALUE"] = "Dark";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = "Office 2016 Black";
            row["VALUE"] = "Black";
            dt.Rows.Add(row);
            
            return dt;
        }
    }
}
