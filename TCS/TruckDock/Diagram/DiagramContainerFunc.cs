using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraDiagram;
using Hmx.DHAKA.TCS.TruckDock.Item;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hmx.DHAKA.TCS.TruckDock.Diagram
{
    public class DiagramContainerFunc 
    {
        #region FIELD AREA ***********************
        #region setting Diagram 
        private DiagramControl _diagControl;
        private bool _allowDup = false;
        public bool _allowModifiedEvent;
        private bool _IsEditable;
        public delegate void DataHandler(string name, bool isDeleted, bool allClear);
        public event DataHandler ModifiedEvent;
        #endregion

        private static float WH_SIZE = 20;
        private static float TD_SIDE = 150;
        private static float TD_ENTRANCE = 30;
        private static int OUTLINE_MARGIN = 3;

        private float _wh_Pos_x = 0;
        private float _wh_Pos_y = 0;
        private float _ct_Width = 0;
        private float _ct_Height = 0;
        private float _td_Pos_x = 0;
        private float _td_Pos_y = 0;
        private float _wh_Width = 0;
        private float _wh_Height = 0;
        private float _td_Width = 0;
        private float _td_Height = 0;

        private string _default_Direction = "B";
        private Color _default_BackColor = Color.White;
        private Color _default_ForeColor = Color.Black;

        private DiagramItem _selectedItem;
        #endregion
        #region INITIALIZE AREA *********************

        public DiagramContainerFunc() : base()
        {

        }
        #endregion
        #region PROPERTY AREA ***********************
        public bool AllowModifiedEvent
        {
            get
            {
                return _allowModifiedEvent;
            }
            set
            { 
                _allowModifiedEvent = value; 
            }
        }
        public DiagramControl DiagControl
        {
            get
            {
                if (this._diagControl == null) this._diagControl = new DiagramControl();
                return this._diagControl;
            }
        }
        public bool IsEditable
        {
            get 
            {
                return _IsEditable;
            }
            set
            {
                _IsEditable = value;
            }
        }
        public bool AllowDup
        {
            get
            {
                return _allowDup;
            }
            set
            {
                _allowDup = value;
            }
        }
        public string Default_Direction
        {
            get
            {
                return _default_Direction;
            }
        }
        public Color Default_BackColor
        {
            get
            {
                return _default_BackColor;
            }
        }
        public Color Default_ForeColor
        {
            get
            {
                return _default_ForeColor;
            }
        }
        public DiagramItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set 
            {
                _selectedItem = value;
            }
        }
        #endregion
        #region METHOD AREA
        public void InitDiagramControl(DiagramControl diagControl, float editWidth, float editHeight)
        {
            _diagControl = diagControl;
            this.SetDiagramControl(editWidth, editHeight);
            this.AddEvent();
        }
        private void SetDiagramControl(float editWidth, float editHeight)
        {
            this.DiagControl.OptionsView.CanvasSizeMode = CanvasSizeMode.None;            
            this.DiagControl.OptionsView.Landscape = true;
            this.DiagControl.OptionsView.ScrollMargin = new Padding(int.MaxValue);
            this.DiagControl.OptionsView.ShowGrid = this.IsEditable;
            this.DiagControl.OptionsView.ShowRulers = false;

            //this.DiagControl.OptionsProtection.AllowZoom = false;
            //this.DiagControl.OptionsView.PaperKind = PaperKind.A4;

            this.DiagControl.OptionsView.PageSize = new SizeF(editWidth, editHeight);
            this.DiagControl.OptionsView.PageMargin = new Padding(0);
            this.DiagControl.OptionsView.PaperKind = PaperKind.Custom;
            
            this.DiagControl.OptionsBehavior.EnableProportionalResizing = false;
            this.DiagControl.AutoSizeInLayoutControl = false;
            this.DiagControl.OptionsView.FitToDrawingMargin = new Padding(0); 
        }
        private void AddEvent()
        {
            //this.DiagControl.ItemContentChanged += new EventHandler<DiagramItemContentChangedEventArgs>(this.DiagControl_ItemContentChanged);
            this.DiagControl.ItemsChanged += new EventHandler<DiagramItemsChangedEventArgs>(this.DiagControl_ItemsChanged);
            //this.DiagControl.QueryItemsAction += new EventHandler<DiagramQueryItemsActionEventArgs>(this.DiagControl_QueryItemsAction);
        }
        public void POPDirection(BarSubItem Item)
        {
            for (int i = 0; i < Item.ItemLinks.Count; i++)
            {
                if (Item.ItemLinks[i].Caption.Substring(0, 1).Equals(this.SelectedItem.ParentItem.Tag))
                    ((BarCheckItem)Item.ItemLinks[i].Item).Checked = true;
                else
                    ((BarCheckItem)Item.ItemLinks[i].Item).Checked = false;
            }
        }
        public string SetDirection(string dir)
        {
            if (!dir.Equals(this.SelectedItem.ParentItem.Tag))
            {
                this.DiagControl.Items.Remove(this.SelectedItem.ParentItem);                  
            }
            return ((DiagramContainer)this.SelectedItem.ParentItem).Header;
        }
        public void ChangeItemColor(Color color, bool isBackColor)
        {
            if (isBackColor)
                this.SelectedItem.Appearance.BackColor = color;
            else
                this.SelectedItem.Appearance.ForeColor = color;
        }
        public void RemoveContainer(DiagramItem diagItem)
        {
            this.DiagControl.Items.Remove(diagItem);
        }
        public void AddContainer(WareHouseListItem wh_item)
        {
            DiagramContainer container = new DiagramContainer() { Shape = StandardContainers.Classic };
            container.X = wh_item.WH_POS_X2;
            container.Y = wh_item.WH_POS_Y2;

            container.CanMove = this.IsEditable;
            container.CanAddItems = this.IsEditable;
            container.CanEdit = this.IsEditable;
            container.CanResize = false;
            container.CanChangeParent = false;            
            container.ShowHeader = false;            

            container.ItemsCanMove = false;
            container.ItemsCanResize = false;
            container.ItemsCanRotate = false;
            container.ItemsCanSelect = this.IsEditable;
            container.ItemsCanDeleteWithoutParent = false;
            container.Header = wh_item.WH_Name;
            container.Tag = wh_item.WH_DIRECTION;            

            this.setContainerSize((string)container.Tag, wh_item.TD_List.Count);

            container.Width = this._ct_Width + OUTLINE_MARGIN * 2;
            container.Height = this._ct_Height + OUTLINE_MARGIN * 2;

            this.DiagControl.Items.Add(container);

            CreateNewShapeInContainer(container, container.Header, this._wh_Pos_x, this._wh_Pos_y, this._wh_Width, this._wh_Height, wh_item.WH_BackColor2, wh_item.WH_ForeColor2, BasicShapes.Rectangle, "WH");
            int i = 0;
            
            foreach(WareHouseDesignItem item in wh_item.TD_List)
            {
                switch (container.Tag)
                {
                    case "T":
                        this._td_Pos_x = i++ * this._td_Width;
                        this._td_Pos_y = 0;
                        break;
                    case "B":
                        this._td_Pos_x = i++ * this._td_Width;
                        this._td_Pos_y = this._wh_Height;
                        break;
                    case "L":
                        this._td_Pos_x = 0;
                        this._td_Pos_y = i++ * this._td_Height;
                        break;
                    case "R":
                        this._td_Pos_x = WH_SIZE;
                        this._td_Pos_y = i++ * this._td_Height;
                        break;
                }
                CreateNewShapeInContainer(container, item.TD_Name, this._td_Pos_x, this._td_Pos_y, this._td_Width, this._td_Height, item.TD_BackColor2, item.TD_ForeColor2, BasicShapes.Rectangle, "TD");
            }
        }
        private void setContainerSize(string DspType, int td_count)
        { 
            switch (DspType)
            {
                case "T":
                    this._td_Width = TD_ENTRANCE;
                    this._td_Height = TD_SIDE;
                    this._wh_Pos_x = 0;
                    this._wh_Pos_y = this._td_Height;
                    this._wh_Width = this._td_Width * td_count;
                    this._wh_Height = WH_SIZE;
                    this._ct_Width = this._wh_Width;
                    this._ct_Height = WH_SIZE + this._td_Height;
                    break;
                case "B":
                    this._td_Width = TD_ENTRANCE;
                    this._td_Height = TD_SIDE;
                    this._wh_Pos_x = 0;
                    this._wh_Pos_y = 0;
                    this._wh_Width = this._td_Width * td_count;
                    this._wh_Height = WH_SIZE;
                    this._ct_Width = this._wh_Width;
                    this._ct_Height = WH_SIZE + this._td_Height;
                    break;
                case "L":
                    this._td_Width = TD_SIDE;
                    this._td_Height = TD_ENTRANCE;
                    this._wh_Pos_x = this._td_Width;
                    this._wh_Pos_y = 0;
                    this._wh_Width = WH_SIZE;
                    this._wh_Height = this._td_Height * td_count;
                    this._ct_Width = WH_SIZE + this._td_Width;
                    this._ct_Height = this._wh_Height ;
                    break;
                case "R":
                    this._td_Width = TD_SIDE;
                    this._td_Height = TD_ENTRANCE;
                    this._wh_Pos_x = 0;
                    this._wh_Pos_y = 0;
                    this._wh_Width = WH_SIZE;
                    this._wh_Height = this._td_Height * td_count;
                    this._ct_Width = WH_SIZE + this._td_Width;
                    this._ct_Height = this._wh_Height;
                    break;
            }
        }
        private void CreateNewShapeInContainer(DiagramContainer container, string name, float x_pos, float y_pos, float width, float heigth, Color background, Color forecolor, ShapeDescription kind, string tag)
        {
            DiagramShape NewShape = new DiagramShape()
            {
                Shape = kind,
                Size = new SizeF(width, heigth),
                Position = new PointFloat(x_pos + OUTLINE_MARGIN, y_pos + OUTLINE_MARGIN),
                Content = name,
                Tag = tag
            };
            NewShape.Appearance.BackColor = background;
            NewShape.Appearance.ForeColor = forecolor;
            container.Items.Add(NewShape);
        }
        //public void DiagControl_ItemContentChanged(object sender, DiagramItemContentChangedEventArgs e)
        //{
        //    //string newValue = RemoveCR(e.NewValue);

        //    //if (e.Item.GetType() == typeof(DiagramShape))
        //    //{
        //    //    if (IsDup(newValue, (DiagramShape)e.Item))
        //    //    {
        //    //        MessageBox.Show(newValue + "은(는) 이미 존재합니다.");
        //    //        DiagramShape shape = (DiagramShape)e.Item;
        //    //        shape.Content = e.OldValue;
        //    //    }
        //    //    else
        //    //    {
        //    //        this.ModifiedItem(newValue);
        //    //    }
        //    //}
        //    //else if (e.Item.GetType() == typeof(DiagramContainer))
        //    //{
        //    //    MessageBox.Show("wwwwwww");
        //    //}
        //    if (e.Item.GetType() == typeof(DiagramContainer))
        //    {
        //        MessageBox.Show("wwwwwww");
        //    }
        //}
        public void DiagControl_ItemsChanged(object sender, DiagramItemsChangedEventArgs e)
        { 
        }
        #endregion
    }
}
