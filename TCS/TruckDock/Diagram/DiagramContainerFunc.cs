using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraDiagram;
using Hmx.DHAKA.TCS.TruckDock.Item;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hmx.DHAKA.TCS.TruckDock.Diagram
{
    public class DiagramContainerFunc : DiagramFunc
    {
        #region FIELD AREA ***********************
        private static float WH_SIZE = 20;
        private static float TD_SIDE = 150;
        private static float TD_ENTRANCE = 30;
        private static int OUTLINE_MARGIN = 3;

        private PopupMenu _menuContainer;
        private PopupMenu _menuItem;
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

        private DiagramItem _selectItem;
        #endregion
        #region INITIALIZE AREA *********************

        public DiagramContainerFunc() : base()
        {

        }
        #endregion
        #region PROPERTY AREA ***********************
        public PopupMenu MenuContainer
        {
            get
            {
                if (this._menuContainer == null) this._menuContainer = new PopupMenu();
                return this._menuContainer;
            }
        }
        public PopupMenu MenuItem
        {
            get
            {
                if (this._menuItem == null) this._menuItem = new PopupMenu();
                return this._menuItem;
            }
        }
        #endregion
        #region METHOD AREA       
        public void InitDiagramPopUp(PopupMenu item, PopupMenu container)
        {
            _menuItem = item;
            _menuContainer = container;
            this.AddEvent();
        }
        private void AddEvent()
        {
            this.DiagControl.MouseClick += new MouseEventHandler(this.DiagControl_MouseClick);
        }
        public void POPDirection(BarSubItem Item)
        {
            for (int i = 0; i < Item.ItemLinks.Count; i++)
            {
                if (Item.ItemLinks[i].Caption.Substring(0, 1).Equals(this._selectItem.ParentItem.Tag))
                    ((BarCheckItem)Item.ItemLinks[i].Item).Checked = true;
                else
                    ((BarCheckItem)Item.ItemLinks[i].Item).Checked = false;
            }
        }
        public string SetDirection(string dir)
        {
            if (!dir.Equals(this._selectItem.ParentItem.Tag))
            {
                this.DiagControl.Items.Remove(this._selectItem.ParentItem);                  
            }
            return ((DiagramContainer)this._selectItem.ParentItem).Header;
        }
        private void DiagControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this._selectItem = this.DiagControl.CalcHitItem(((MouseEventArgs)e).Location) as DiagramItem;

                if (this._selectItem != null)
                {
                    if (this._selectItem.GetType() == typeof(DiagramShape))
                        if (this._selectItem.Tag.Equals("WH"))
                            this.MenuContainer.ShowPopup(Control.MousePosition); 
                        else
                            this.MenuItem.ShowPopup(Control.MousePosition);
                }
            }
        }
        public void SetItemBackColor(Color color)
        {
            this._selectItem.Appearance.BackColor = color;
        }
        public void SetItemForeColor(Color color)
        {
            this._selectItem.Appearance.ForeColor = color;
        }
        public void AddingContainer(WareHouseListItem wh_item, TruckDockListItem td_item)
        {
            DiagramContainer container = new DiagramContainer() { Shape = StandardContainers.Classic };
            container.X = wh_item.WH_POS_X2;
            container.Y = wh_item.WH_POS_Y2;

            container.CanAddItems = true;
            container.CanEdit = true;
            container.CanResize = true;
            container.ShowHeader = false;            

            container.ItemsCanMove = false;
            container.ItemsCanResize = false;
            container.ItemsCanRotate = false;
            container.ItemsCanSelect = true;
            container.Header = wh_item.WH_Name;
            container.Tag = wh_item.WH_DIRECTION;

            this.setContainerSize((string)container.Tag, td_item.TD_List.Count);

            container.Width = this._ct_Width + OUTLINE_MARGIN * 2;
            container.Height = this._ct_Height + OUTLINE_MARGIN * 2;

            this.DiagControl.Items.Add(container);

            CreateNewShapeInContainer(container, container.Header, this._wh_Pos_x, this._wh_Pos_y, this._wh_Width, this._wh_Height, wh_item.WH_BackColor2, wh_item.WH_ForeColor2, BasicShapes.Rectangle, "WH");
            int i = 0;
            
            foreach(TruckDockListItem item in td_item.TD_List)
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
        public void DiagControl_ItemContentChanged(object sender, DiagramItemContentChangedEventArgs e)
        {
            //string newValue = RemoveCR(e.NewValue);

            //if (e.Item.GetType() == typeof(DiagramShape))
            //{
            //    if (IsDup(newValue, (DiagramShape)e.Item))
            //    {
            //        MessageBox.Show(newValue + "은(는) 이미 존재합니다.");
            //        DiagramShape shape = (DiagramShape)e.Item;
            //        shape.Content = e.OldValue;
            //    }
            //    else
            //    {
            //        this.ModifiedItem(newValue);
            //    }
            //}
            //else if (e.Item.GetType() == typeof(DiagramContainer))
            //{
            //    MessageBox.Show("wwwwwww");
            //}
            if (e.Item.GetType() == typeof(DiagramContainer))
            {
                MessageBox.Show("wwwwwww");
            }
        }
        public void DiagControl_ItemsChanged(object sender, DiagramItemsChangedEventArgs e)
        { 
        }
        #endregion
    }
}
