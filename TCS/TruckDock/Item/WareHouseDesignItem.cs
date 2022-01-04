using HitopsCommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmx.DHAKA.TCS.TruckDock.Item
{
    public class WareHouseDesignItem : BaseItem
    {
        #region FIELD AREA
        #endregion
        #region PROPERTY AREA 
        public string WH_Name { get; set; }
        public string WH_Desc { get; set; }
        public string WH_ForeColor { get; set; }
        public string WH_BackColor { get; set; }
        public string WH_POS_X { get; set; }
        public string WH_POS_Y { get; set; }
        public string WH_DIRECTION { get; set; }
        public string TD_Name { get; set; }        
        public string TD_ForeColor { get; set; }
        public string TD_BackColor { get; set; }

        public Color WH_ForeColor2
        {
            get
            {
                Color cl = System.Drawing.ColorTranslator.FromHtml(this.WH_ForeColor);
                return cl;
            }
        }
        public Color WH_BackColor2
        {
            get
            {
                Color cl = System.Drawing.ColorTranslator.FromHtml(this.WH_BackColor);
                return cl;
            }
        }
        public float WH_POS_X2
        {
            get
            {
                if (string.IsNullOrEmpty(this.WH_POS_X)) return 0;
                return Convert.ToSingle(this.WH_POS_X);
            }
        }
        public float WH_POS_Y2
        {
            get
            {
                if (string.IsNullOrEmpty(this.WH_POS_Y)) return 0;
                return Convert.ToSingle(this.WH_POS_Y);
            }
        }
        public Color TD_ForeColor2
        {
            get
            {
                Color cl = System.Drawing.ColorTranslator.FromHtml(this.TD_ForeColor);
                return cl;
            }
        }
        public Color TD_BackColor2
        {
            get
            {
                Color cl = System.Drawing.ColorTranslator.FromHtml(this.TD_BackColor);
                return cl;
            }
        }
        #endregion
    }

    public class WareHouseListItem : WareHouseDesignItem
    {
        #region FIELD AREA 
        private IList<WareHouseDesignItem> _wh_Design;
        private IList<WareHouseListItem> _wh_List;
        #endregion
        #region PROPERTY AREA 
        public IList<WareHouseDesignItem> WH_Design
        {
            get
            {
                if (this._wh_Design == null) this._wh_Design = new List<WareHouseDesignItem>();
                return this._wh_Design;
            }
            set
            {
                this._wh_Design = value;
                this.makeWH_List();
            }
        }
        private void makeWH_List()
        {
            if (this._wh_List == null) this._wh_List = new List<WareHouseListItem>();
            this._wh_List.Clear();

            string tmpWH_Name = "";
            WareHouseListItem WH_Item = new WareHouseListItem();
            foreach (WareHouseDesignItem item in this.WH_Design)
            {
                if (!item.WH_Name.Equals(tmpWH_Name))
                {
                    WH_Item = new WareHouseListItem();
                    WH_Item.WH_Name = item.WH_Name;
                    WH_Item.WH_Desc = item.WH_Desc;
                    WH_Item.WH_ForeColor = item.WH_ForeColor;
                    WH_Item.WH_BackColor = item.WH_BackColor;
                    WH_Item.WH_POS_X = item.WH_POS_X;
                    WH_Item.WH_POS_Y = item.WH_POS_Y;
                    WH_Item.WH_DIRECTION = item.WH_DIRECTION;
                    this._wh_List.Add(WH_Item);

                    tmpWH_Name = WH_Item.WH_Name;
                }
            }
        }
        public IList<WareHouseListItem> WH_List
        {
            get
            {
                return this._wh_List;
            }
        }
        #endregion
    }

    public class TruckDockListItem : WareHouseListItem
    {
        #region FIELD AREA         
        private IList<TruckDockListItem> _td_List;
        private string _compWH_Name = "";
        #endregion
        #region PROPERTY AREA 
        public string CompWH_Name
        {
            get
            {
                return this._compWH_Name;
            }
            set
            {
                this._compWH_Name = value;
                this.makeTD_List();
            }
        }
        private void makeTD_List()
        {
            if (this._td_List == null) this._td_List = new List<TruckDockListItem>();
            this._td_List.Clear();

            TruckDockListItem TD_Item = new TruckDockListItem();
            foreach (WareHouseDesignItem item in this.WH_Design)
            {
                if (item.WH_Name.Equals(this.CompWH_Name))
                {
                    TD_Item = new TruckDockListItem();
                    TD_Item.TD_Name = item.TD_Name;
                    TD_Item.TD_ForeColor = item.TD_ForeColor;
                    TD_Item.TD_BackColor = item.TD_BackColor;
                    this._td_List.Add(TD_Item);
                }
            }
        }
        public IList<TruckDockListItem> TD_List
        {
            get
            {
                return this._td_List;
            }
        }
        #endregion        
    }
}