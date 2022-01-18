using CommonClass.Database.Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hmx.DHAKA.TCS.TruckDock.Item;

namespace Hmx.DHAKA.TCS.TruckDock.Service
{
    public class WareHouseDesignService
    {
        #region INITIALIZE AREA ********************
        public WareHouseDesignService()
        {
        }
        #endregion
        #region METHOD AREA ********************
        public IList<WareHouseDesignItem> Inquiry()
        {
            DataTable dataTable;
            IList<WareHouseDesignItem> resultItems = new List<WareHouseDesignItem>();

            try
            {
                Hashtable parameters = BindDB2Class.BindDBClass2Hashtable("", false);
                //dataTable = CommFunc.RequestHandlerDataTable("", null, parameters);
                dataTable = (new TempData()).GetData();

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    resultItems = BindDB2Class.BindDataTableToListNoFormat<WareHouseDesignItem>(dataTable);
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
            dt.Columns.Add(new DataColumn("WH_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("WH_Desc", typeof(string)));
            dt.Columns.Add(new DataColumn("WH_ForeColor", typeof(string)));
            dt.Columns.Add(new DataColumn("WH_BackColor", typeof(string)));
            dt.Columns.Add(new DataColumn("WH_POS_X", typeof(string)));
            dt.Columns.Add(new DataColumn("WH_POS_Y", typeof(string)));
            dt.Columns.Add(new DataColumn("WH_DIRECTION", typeof(string)));
            dt.Columns.Add(new DataColumn("TD_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("TD_ForeColor", typeof(string)));
            dt.Columns.Add(new DataColumn("TD_BackColor", typeof(string)));

            string direction = "B";
            DataRow row = dt.NewRow();
            row["WH_Name"] = "WareHouse1";
            row["WH_Desc"] = "First WareHouse";
            row["WH_ForeColor"] = "#FF0000FF";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "50";
            row["WH_POS_Y"] = "50";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock11";
            row["TD_ForeColor"] = "#FF0000FF";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse1";
            row["WH_Desc"] = "First WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "50";
            row["WH_POS_Y"] = "50";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock12";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse1";
            row["WH_Desc"] = "First WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "50";
            row["WH_POS_Y"] = "50";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock13";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            //
            direction = "T";
            row = dt.NewRow();
            row["WH_Name"] = "WareHouse2";
            row["WH_Desc"] = "Second WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "250";
            row["WH_POS_Y"] = "250";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock21";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse2";
            row["WH_Desc"] = "Second WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "250";
            row["WH_POS_Y"] = "250";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock22";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse2";
            row["WH_Desc"] = "Second WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "250";
            row["WH_POS_Y"] = "250";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock23";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            //
            direction = "L";
            row = dt.NewRow();
            row["WH_Name"] = "WareHouse3";
            row["WH_Desc"] = "Third WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "450";
            row["WH_POS_Y"] = "450";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock31";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse3";
            row["WH_Desc"] = "Third WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "450";
            row["WH_POS_Y"] = "450";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock32";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse3";
            row["WH_Desc"] = "Third WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "450";
            row["WH_POS_Y"] = "450";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock33";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            //
            direction = "R";
            row = dt.NewRow();
            row["WH_Name"] = "WareHouse4";
            row["WH_Desc"] = "Fourth WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "650";
            row["WH_POS_Y"] = "650";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock41";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse4";
            row["WH_Desc"] = "Fourth WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "650";
            row["WH_POS_Y"] = "650";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock42";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["WH_Name"] = "WareHouse4";
            row["WH_Desc"] = "Fourth WareHouse";
            row["WH_ForeColor"] = "#FF000000";
            row["WH_BackColor"] = "#FFFFFFFF";
            row["WH_POS_X"] = "650";
            row["WH_POS_Y"] = "650";
            row["WH_DIRECTION"] = direction;
            row["TD_Name"] = "TruckDock43";
            row["TD_ForeColor"] = "#FF000000";
            row["TD_BackColor"] = "#FFFFFFFF";
            dt.Rows.Add(row);

            return dt;
        }
    }
}
