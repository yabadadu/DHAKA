using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonClass.Database.Mapper;
using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraDiagram;
using HitopsCommon.FormTemplate;
using Hmx.DHAKA.TCS.TruckDock.Diagram;
using Hmx.DHAKA.TCS.TruckDock.Item;
using Hmx.DHAKA.TCS.TruckDock.Service;

namespace Hmx.DHAKA.TCS.TruckDock
{
    public partial class frmDesign : BaseSingleGridView
    {
        #region FIELD AREA
        private DiagramContainerFunc _diagFunc;
        private WareHouseDesignService _service;
        private IList<WareHouseDesignItem> _design_items;
        private WareHouseListItem _wh_item;
        #endregion
        #region PROPERTY AREA
        public DiagramContainerFunc DiagFunc
        {
            get
            {
                if (this._diagFunc == null) this._diagFunc = new DiagramContainerFunc();
                return this._diagFunc;
            }
        }
        public WareHouseDesignService Service
        {
            get
            {
                if (this._service == null) this._service = new WareHouseDesignService();
                return this._service;
            }
        }
        public IList<WareHouseDesignItem> Design_items
        {
            get
            {
                if (this._design_items == null) this._design_items = new List<WareHouseDesignItem>();
                return this._design_items;
            }
            set
            {
                this._design_items = value;
            }
        }
        public WareHouseListItem WH_Item
        {
            get
            {
                if (this._wh_item == null) this._wh_item = new WareHouseListItem();
                return this._wh_item;
            }
            set 
            {
                this._wh_item = value;
            }
        }
        #endregion
        #region INITIALIZE AREA
        public frmDesign()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.frmDesign_Load);
            this.AddEventHandler();
        }
        #endregion
        #region EVENT AREA
        #endregion
        #region METHOD AREA
        private void frmDesign_Load(object sender, EventArgs e)
        {
            this.DiagFunc.InitDiagramControl(this.diagramControl1);
            this.DiagFunc.InitDiagramPopUp(this.popupMenuItem, this.popupMenuContainer);
            
            //this.DiagFunc.AddingContainer("FSDFDS",12);

            this.DoInquiry();

           // var container = new DiagramContainer() { Shape = StandardContainers.Classic };
           // container.X = 0;
           // container.Y = 0;
           // container.Width = 100;
           // container.Height = 100;

           // container.CanAddItems = true;
           // container.CanEdit = true;
           // container.ShowHeader = true;

           // container.Header = "DFSDFDS";
           // container.Items.Add(
           //    new DiagramShape
           //    {
           //        Shape = BasicShapes.Rectangle,
           //        Width = 100,
           //        Height = 100,
           //        Size = new SizeF(100f, 100f),
           //        Position = new PointFloat(0 + 150f, 0),
           //        Content = "Shape"
           //    }
           //);

            
           // //container.Items.Add()
           // this.diagramControl1.Items.Add(container);

            //this.DiagFunc.AddingItem("FSDFDS", BasicShapes.container.Frame);
        }
        private void AddEventHandler()
        {
            #region Diagram
            this.btnBackColor.EditValueChanged += new EventHandler(this.btnBackColor_EditValueChanged);
            this.btnForeColor.EditValueChanged += new EventHandler(this.btnForeColor_EditValueChanged);
            //this.diagramControl1.MouseClick += new MouseEventHandler(this.diagramControl1_MouseClick);  
            this.btnDirection.Popup += new EventHandler(this.btnDirection_Pop);
            this.chkBottom.ItemClick += new ItemClickEventHandler(this.chkDirection_ItemClick);
            this.chkTop.ItemClick += new ItemClickEventHandler(this.chkDirection_ItemClick);
            this.chkLeft.ItemClick += new ItemClickEventHandler(this.chkDirection_ItemClick);
            this.chkRight.ItemClick += new ItemClickEventHandler(this.chkDirection_ItemClick);
            #endregion

        }
        private void btnBackColor_EditValueChanged(object sender, EventArgs e)
        {
            this.DiagFunc.SetItemBackColor((Color)((BarEditItem)sender).EditValue);
        }
        private void btnForeColor_EditValueChanged(object sender, EventArgs e)
        {
            this.DiagFunc.SetItemForeColor((Color)((BarEditItem)sender).EditValue);
        }
        private void btnDirection_Pop(object sender, EventArgs e)
        {
            this.DiagFunc.POPDirection(btnDirection);
        }
        private void chkDirection_ItemClick(object sender, ItemClickEventArgs e)
        {            
            string wh_name = this.DiagFunc.SetDirection(e.Item.Caption.Substring(0,1));
            foreach (WareHouseListItem item in this.WH_Item.WH_List)
            {
                if (item.WH_Name.Equals(wh_name))
                {
                    item.WH_DIRECTION = e.Item.Caption.Substring(0, 1);
                    TruckDockListItem td_items = new TruckDockListItem();
                    td_items.WH_Design = this.Design_items;
                    td_items.CompWH_Name = item.WH_Name;
                    this.DiagFunc.AddingContainer(item, td_items);
                    break;
                }
            }
        }
        private void DoInquiry()
        {
            this.Design_items = this.Service.Inquiry();
            
            this.WH_Item.WH_Design = this.Design_items;
            TruckDockListItem td_items = new TruckDockListItem();
            td_items.WH_Design = this.Design_items;

            foreach (WareHouseListItem item in this.WH_Item.WH_List)
            {
                td_items.CompWH_Name = item.WH_Name;
                
                this.DiagFunc.AddingContainer(item, td_items);
            }

            //DataTable dtThema = BindDB2Class.BindDatatable(items);
        }
        #endregion
    }
}
