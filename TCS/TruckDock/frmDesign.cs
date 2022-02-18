using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private string _newName = "New";
        private DiagramContainerFunc _diagFunc;
        private WareHouseDesignService _service;
        private IList<WareHouseDesignItem> _design_items;
        private WareHouseListItem _wh_items;        

        //**When selecting Item in Diagram
        private DiagramContainer _selectedContainer;
        private DiagramItem _selectedDiagramItem;
        private WareHouseListItem _selectedWHListItem;
        private WareHouseDesignItem _selectedTDListItem;
        private int _POS_X;
        private int _POS_Y;
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
        public WareHouseListItem WH_Items
        {
            get
            {
                if (this._wh_items == null) this._wh_items = new WareHouseListItem();
                return this._wh_items;
            }
            set 
            {
                this._wh_items = value;
            }
        }
        public DiagramContainer SelectedContainer
        {
            get
            {
                if (this._selectedContainer == null) this._selectedContainer = new DiagramContainer();
                return this._selectedContainer;
            }
            set
            {
                this._selectedContainer = value;
            }
        }
        public DiagramItem SelectedDiagramItem
        {
            get
            {
                return this._selectedDiagramItem;
            }
            set
            {
                this._selectedDiagramItem = value;
            }
        }
        public WareHouseListItem SelectedWHListItem
        {
            get
            {
                if (this._selectedWHListItem == null) this._selectedWHListItem = new WareHouseListItem();
                return this._selectedWHListItem;
            }
            set
            {
                this._selectedWHListItem = value;
            }
        }
        public WareHouseDesignItem SelectedTDListItem
        {
            get
            {
                if (this._selectedTDListItem == null) this._selectedTDListItem = new WareHouseDesignItem();
                return this._selectedTDListItem;
            }
            set
            {
                this._selectedTDListItem = value;
            }
        }
        #endregion
        #region INITIALIZE AREA
        public frmDesign()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.frmDesign_Load);
            this.FormClosing += new FormClosingEventHandler(this.frmDesign_FormClosing);
        }
        #endregion
        #region EVENT AREA
        #endregion
        #region METHOD AREA
        private void frmDesign_Load(object sender, EventArgs e)
        {
            if (this.Tag == null) this.DiagFunc.IsEditable = true;
            if (this.DiagFunc.IsEditable) this.AddEventHandler();

            this.Left = 0;
            this.Top = 0;
            this.Width = this.Parent.ClientSize.Width;
            this.Height = this.Parent.ClientSize.Height;

            this.DiagFunc.InitDiagramControl(this.diagramControl1, this.ClientSize.Width , this.ClientSize.Height);            

            this.DoInquiry();
        }
        private void frmDesign_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
        private void AddEventHandler()
        {
            #region Diagram
            this.diagramControl1.MouseClick += new MouseEventHandler(this.diagramControl1_MouseClick);            
            this.diagramControl1.CustomItemDrag += new EventHandler<DiagramCustomItemDragEventArgs>(this.diagramControl1_CustomItemDrag);
            this.diagramControl1.CustomItemDragResult += new EventHandler<DiagramCustomItemDragResultEventArgs>(this.diagramControl1_CustomItemDragResult);
            this.diagramControl1.ItemContentChanged += new EventHandler<DiagramItemContentChangedEventArgs>(this.diagramControl1_ItemContentChanged);
         
            #endregion
            #region Button
            this.btnDirection.Popup += new EventHandler(this.btnDirection_Pop);
            this.btnBottom.ItemClick += new ItemClickEventHandler(this.btnDirection_ItemClick);
            this.btnTop.ItemClick += new ItemClickEventHandler(this.btnDirection_ItemClick);
            this.btnLeft.ItemClick += new ItemClickEventHandler(this.btnDirection_ItemClick);
            this.btnRight.ItemClick += new ItemClickEventHandler(this.btnDirection_ItemClick);
            this.btnAddWareHouse.ItemClick += new ItemClickEventHandler(this.btnAddWareHouse_ItemClick);
            this.btnAddTruckDock.ItemClick += new ItemClickEventHandler(this.btnAddTruckDock_ItemClick);
            this.btnDeleteTruckDock.ItemClick += new ItemClickEventHandler(this.btnDeleteTruckDock_ItemClick); 
            #endregion
        }
        private void diagramControl1_ItemContentChanged(object sender, DiagramItemContentChangedEventArgs e)
        {
            string newValue = Regex.Replace(e.NewValue, @"\r\n?|\n|\s", "", RegexOptions.Singleline);
            if (string.IsNullOrEmpty(newValue))
            {
                ((DiagramShape)e.Item).Content = e.OldValue;
                return;
            }

            if (this.AllowChangeToNewName(newValue, e.OldValue, (string)e.Item.Tag))
                this.ItemModifying(newValue, (string)e.Item.Tag); 
            else
                ((DiagramShape)e.Item).Content = e.OldValue;
        }
        private bool AllowChangeToNewName(string newValue, string oldValue, string tag)
        {
            bool rtnDup;
            if (tag.Equals("WH"))
                rtnDup = IsDup(newValue);
            else
                rtnDup = IsDup(this.SelectedWHListItem, newValue);

            if (rtnDup)
                { 
                    MessageBox.Show(newValue + " is duplicated");
                    return false;
                }

            bool rtnUse;
            if (tag.Equals("WH"))
                rtnUse = IsUse(oldValue);
            else
                rtnUse = IsUse(this.SelectedWHListItem, oldValue);

            if (rtnUse)
            {
                MessageBox.Show(newValue + " is in use");
                return false;
            }

            return true;
        }
        private bool IsUse(string whName)
        {
            return false;
        }
        private bool IsUse(WareHouseListItem whItem, string whName)
        {
            return false;
        }
        private bool IsDup(string whName)
        {
            bool rtnDup = false;
            foreach (WareHouseListItem wh_item in this.WH_Items.WH_List)
            {
                if (wh_item.WH_Name.Equals(whName))
                {
                    rtnDup = true;
                    break;
                }
            }
            
            return rtnDup;
        }
        private bool IsDup(WareHouseListItem whItem, string tdName)
        {
            {
                bool rtnDup = false;
                foreach (WareHouseDesignItem td_item in whItem.TD_List)
                {
                    if (td_item.TD_Name.Equals(tdName))
                    {
                        rtnDup = true;
                        break;
                    }
                }
                return rtnDup;
            }
        }
        private void btnBackColor_EditValueChanged(object sender, EventArgs e)
        {
            this.DiagFunc.ChangeItemColor((Color)((BarEditItem)sender).EditValue, true);

            if (this.SelectedDiagramItem.Tag.Equals("WH"))
                this.ItemModifying(((DiagramShape)this.SelectedDiagramItem).Content, 
                                   this.SelectedDiagramItem.Appearance.BackColor, this.SelectedDiagramItem.Appearance.ForeColor);
            else if (this.SelectedDiagramItem.Tag.Equals("TD"))
                this.ItemModifying(((DiagramContainer)this.SelectedDiagramItem.ParentItem).Header, ((DiagramShape)this.SelectedDiagramItem).Content, 
                                   this.SelectedDiagramItem.Appearance.BackColor, this.SelectedDiagramItem.Appearance.ForeColor);
        }
        private void btnForeColor_EditValueChanged(object sender, EventArgs e)
        {
            this.DiagFunc.ChangeItemColor((Color)((BarEditItem)sender).EditValue, false);

            if (this.SelectedDiagramItem.Tag.Equals("WH"))
                this.ItemModifying(((DiagramShape)this.SelectedDiagramItem).Content, 
                                   this.SelectedDiagramItem.Appearance.BackColor, this.SelectedDiagramItem.Appearance.ForeColor);
            else if (this.SelectedDiagramItem.Tag.Equals("TD"))
                this.ItemModifying(((DiagramContainer)this.SelectedDiagramItem.ParentItem).Header, ((DiagramShape)this.SelectedDiagramItem).Content, 
                                   this.SelectedDiagramItem.Appearance.BackColor, this.SelectedDiagramItem.Appearance.ForeColor);
        }
        private void btnDirection_Pop(object sender, EventArgs e)
        {
            this.DiagFunc.POPDirection(btnDirection);
        }
        private void btnDirection_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!this.SelectedContainer.Tag.Equals(e.Item.Caption.Substring(0, 1)))
            {
                this.ItemModifying(e.Item.Caption.Substring(0, 1));
                this.ReDraw();
            }            
        }
        private void ReDraw()
        {
            this.diagramControl1.Items.Remove(this.SelectedContainer);
            this.DiagFunc.AddContainer(this.SelectedWHListItem);
        }
        private void btnAddWareHouse_ItemClick(object sender, ItemClickEventArgs e)
        {
            WareHouseListItem new_WHItem = new WareHouseListItem();
            new_WHItem.WH_Name = getNewName();
            new_WHItem.WH_DIRECTION = this.DiagFunc.Default_Direction;
            new_WHItem.WH_BackColor = ColorTranslator.ToHtml(this.DiagFunc.Default_BackColor);
            new_WHItem.WH_ForeColor = ColorTranslator.ToHtml(this.DiagFunc.Default_ForeColor);
            new_WHItem.WH_POS_X = this._POS_X.ToString();
            new_WHItem.WH_POS_Y = this._POS_Y.ToString();

            this.WH_Items.WH_List.Add(new_WHItem);
            this.WH_Items.WH_List[WH_Items.WH_List.Count - 1].TD_List = new List<WareHouseDesignItem>();
            WareHouseDesignItem new_TDItem = new WareHouseDesignItem();
            new_TDItem.TD_BackColor = ColorTranslator.ToHtml(this.DiagFunc.Default_BackColor);
            new_TDItem.TD_ForeColor = ColorTranslator.ToHtml(this.DiagFunc.Default_ForeColor);
            new_TDItem.TD_Name = getNewName(new_WHItem);
            this.WH_Items.WH_List[WH_Items.WH_List.Count - 1].TD_List.Add(new_TDItem);

            this.SelectedContainer = null;
            this.SelectedWHListItem = new_WHItem;
            this.ReDraw();
        }
        private string getNewName()
        {
            string allNames = "";
            foreach (WareHouseListItem whListItem in this.WH_Items.WH_List)
            {
                allNames += "<" + whListItem.WH_Name + ">";
            }

            return getNewIndex(allNames);
        }
        private string getNewName(WareHouseListItem whListItem)
        {
            string allNames = "";
            foreach (WareHouseDesignItem tdListItem in whListItem.TD_List)
            {
                allNames += "<" + tdListItem.TD_Name + ">";
            }

            return getNewIndex(allNames);
        }
        private string getNewIndex(string allName)
        {
            string rtnStr = "";
            for (int i = 1; i <= 100; i++)
            {
                if (allName.IndexOf(this._newName + i) == -1)
                {
                    rtnStr = this._newName + i;
                    break;
                }
            }
            return rtnStr;
        }
        private void btnAddTruckDock_ItemClick(object sender, ItemClickEventArgs e)
        {
            WareHouseDesignItem new_TDItem = new WareHouseDesignItem();
            new_TDItem.TD_Name = getNewName(this.SelectedWHListItem);
            new_TDItem.TD_BackColor = ColorTranslator.ToHtml(this.DiagFunc.Default_BackColor);
            new_TDItem.TD_ForeColor = ColorTranslator.ToHtml(this.DiagFunc.Default_ForeColor);
            this.SelectedWHListItem.TD_List.Add(new_TDItem);
            this.ReDraw();
        }
        private void btnDeleteTruckDock_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.SelectedWHListItem.TD_List.Remove(this.SelectedTDListItem);
            this.ReDraw();
        }
        private void SetSelectedItem(DiagramItem diagItem)
        {
            switch (diagItem.Tag)
            {
                case "WH":
                    this.SelectedContainer = (DiagramContainer)diagItem.ParentItem;
                    this.SelectedWHListItem = this.FindWareHouseItem(((DiagramShape)diagItem).Content);
                    this.SelectedTDListItem = null;
                    break;
                case "TD":
                    this.SelectedContainer = (DiagramContainer)diagItem.ParentItem;
                    this.SelectedWHListItem = this.FindWareHouseItem(((DiagramContainer)diagItem.ParentItem).Header);
                    this.SelectedTDListItem = this.FindTruckDockItem(this.SelectedWHListItem, ((DiagramShape)diagItem).Content);
                    break;
                default:
                    this.SelectedContainer = (DiagramContainer)diagItem;
                    this.SelectedWHListItem = this.FindWareHouseItem(((DiagramContainer)diagItem).Header); 
                    this.SelectedTDListItem = null;
                    break;
            }
            this.DiagFunc.SelectedItem = diagItem;
        } 
        private void diagramControl1_MouseClick(object sender, MouseEventArgs e)
        {
           
            this._POS_X = e.X + (int)this.diagramControl1.ScrollPos.X;
            this._POS_Y = e.Y + (int)this.diagramControl1.ScrollPos.Y;

            this.SelectedDiagramItem = this.diagramControl1.CalcHitItem(((MouseEventArgs)e).Location) as DiagramItem;
            if (this.SelectedDiagramItem == null)
            {
                if (e.Button == MouseButtons.Right)
                    this.popupMenuAddContainer.ShowPopup(Control.MousePosition);

                return;
            }

            this.SetSelectedItem(this.SelectedDiagramItem);

            if (e.Button == MouseButtons.Right)
            {
                this.btnBackColor.EditValueChanged -= this.btnBackColor_EditValueChanged;
                this.btnForeColor.EditValueChanged -= this.btnForeColor_EditValueChanged;
                btnBackColor.EditValue = this.SelectedDiagramItem.Appearance.BackColor;
                btnForeColor.EditValue = this.SelectedDiagramItem.Appearance.ForeColor;
                this.btnBackColor.EditValueChanged += this.btnBackColor_EditValueChanged;
                this.btnForeColor.EditValueChanged += this.btnForeColor_EditValueChanged;

                if (this.SelectedDiagramItem.Tag.Equals("WH"))
                    this.popupMenuContainer.ShowPopup(Control.MousePosition);
                else if (this.SelectedDiagramItem.Tag.Equals("TD"))
                    this.popupMenuItem.ShowPopup(Control.MousePosition);
             }
        }
        private WareHouseListItem FindWareHouseItem(string whName)
        {
            WareHouseListItem rtnWHItem = new WareHouseListItem();
            foreach (WareHouseListItem item in this.WH_Items.WH_List)
            {
                if (item.WH_Name.Equals(whName))
                {
                    rtnWHItem = item;
                    break;
                }
            }
            return rtnWHItem;
        }
        private WareHouseDesignItem FindTruckDockItem(WareHouseListItem whItem, string tdName)
        {
            WareHouseDesignItem rtnTDItem = new WareHouseDesignItem();
            foreach (WareHouseDesignItem item in whItem.TD_List)
            {
                if (item.TD_Name.Equals(tdName))
                {
                    rtnTDItem = item;
                    break;
                }
            }
            return rtnTDItem;
        }
        private void diagramControl1_CustomItemDrag(object sender, DiagramCustomItemDragEventArgs e)
        {
            if (e.SourceItem.GetType() == typeof(DiagramContainer)) 
                this.SetSelectedItem(e.SourceItem);

        }
        private void diagramControl1_CustomItemDragResult(object sender, DiagramCustomItemDragResultEventArgs e)
        {
            foreach (DiagramItem diagItem in e.Items)
            {
                if (diagItem.GetType() == typeof(DiagramContainer))
                {
                    this.SelectedContainer = (DiagramContainer)diagItem;
                    this.SelectedWHListItem = this.FindWareHouseItem(((DiagramContainer)diagItem).Header);
                    this.SelectedTDListItem = null;
                    this.ItemModifying(this.SelectedContainer.Header, this.SelectedContainer.Position.X, this.SelectedContainer.Position.Y);
                }
            }
        }
        private void DoInquiry()
        {
            this.Design_items = this.Service.Inquiry();
            
            this.WH_Items.WH_Design = this.Design_items;
            foreach (WareHouseListItem wh_item in this.WH_Items.WH_List)
            {
                this.DiagFunc.AddContainer(wh_item);
            }
        }
        private void ItemModifying(string newName, string tag)
        {
            if (tag.Equals("WH"))
                this.SelectedWHListItem.WH_Name = newName;
            else
                this.SelectedTDListItem.TD_Name = newName;
        }
        private void ItemModifying(string direction)
        {
            this.SelectedWHListItem.WH_DIRECTION = direction;
        }
        private void ItemModifying(string whName, float pos_x, float pos_y)
        {
            this.SelectedWHListItem.WH_POS_X = pos_x.ToString();
            this.SelectedWHListItem.WH_POS_Y = pos_y.ToString();
        }
        private void ItemModifying(string whName, Color backColor, Color foreColor)
        {
            this.SelectedWHListItem.WH_BackColor = ColorTranslator.ToHtml(backColor);
            this.SelectedWHListItem.WH_ForeColor = ColorTranslator.ToHtml(foreColor);
        }
        private void ItemModifying(string whName, string tdName, Color backColor, Color foreColor)
        {
            this.SelectedTDListItem.TD_BackColor = ColorTranslator.ToHtml(backColor);
            this.SelectedTDListItem.TD_ForeColor = ColorTranslator.ToHtml(foreColor);
        }
        #endregion
    }
}
