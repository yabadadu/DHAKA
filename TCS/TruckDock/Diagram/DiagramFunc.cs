using DevExpress.Diagram.Core;
using DevExpress.XtraDiagram;
using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.Utils;
using System.Drawing.Printing;
using System.IO;

namespace Hmx.DHAKA.TCS.TruckDock.Diagram
{
    public class DiagramFunc
    {
        #region FIELD AREA ***********************
        private DiagramControl _diagControl;
        private bool _allowDup = false;
        private int x_Pos;
        private int y_Pos;
        #endregion
        #region INITIALIZE AREA *********************

        public DiagramFunc() : base()
        {
        }
        #endregion

        #region PROPERTY AREA ***********************
        public bool AllowModifiedEvent;
        public delegate void DataHandler(string name, bool isDeleted, bool allClear);
        public event DataHandler ModifiedEvent;
        public DiagramControl DiagControl
        {
            get
            {
                if (this._diagControl == null) this._diagControl = new DiagramControl();
                return this._diagControl;
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
        public string ReturnXML
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                this.DiagControl.SaveDocument(stream);

                return Convert.ToBase64String(stream.ToArray());
            }
        }
        #endregion
        #region METHOD AREA
        public void InitDiagramControl(DiagramControl diagControl, float editWidth, float editHeight)
        {
            _diagControl = diagControl;
            this.SetDiagramControl(editWidth - 100 , editHeight - 100);
            this.AddEvent();
        }
        private void SetDiagramControl(float editWidth, float editHeight)
        {
            this.DiagControl.OptionsView.CanvasSizeMode = CanvasSizeMode.None;
            //this.DiagControl.OptionsView.PaperKind = PaperKind.A4;
            this.DiagControl.OptionsView.Landscape = true;
            this.DiagControl.OptionsView.ScrollMargin = new Padding(int.MaxValue);
            this.DiagControl.OptionsView.ShowGrid = true;
            this.DiagControl.OptionsView.ShowRulers = false;

            //this.DiagControl.OptionsProtection.AllowZoom = false;

            this.DiagControl.OptionsView.PageSize = new SizeF(editWidth, editHeight);
            this.DiagControl.OptionsView.PaperKind = PaperKind.Custom;
            this.DiagControl.FitToDrawing();
            this.DiagControl.OptionsBehavior.EnableProportionalResizing = false;
            this.DiagControl.AutoSizeInLayoutControl = false;
        }
        private void AddEvent()
        {
            this.DiagControl.ItemContentChanged += new EventHandler<DiagramItemContentChangedEventArgs>(this.DiagControl_ItemContentChanged);
            this.DiagControl.ItemsChanged += new EventHandler<DiagramItemsChangedEventArgs>(this.DiagControl_ItemsChanged);
            this.DiagControl.QueryItemsAction += new EventHandler<DiagramQueryItemsActionEventArgs>(this.DiagControl_QueryItemsAction);
        }
        private void DiagControl_CustomDrawBackground(object sender, CustomDrawBackgroundEventArgs e)
        {
            try
            {
                if (this.DiagControl.BackgroundImage == null) { e.Graphics.Clear(Color.White); return; }
                e.Graphics.DrawImage(this.DiagControl.BackgroundImage, e.TotalBounds);
                this.DiagControl.OptionsView.ShowGrid = false;
            }
            catch (Exception ex) { }
        }
        public void ClearBackGround()
        {
            this.DiagControl.BackgroundImage = null;
            this.DiagControl.OptionsView.ShowGrid = true;
        }
        public void SetBackGround(Stream image)
        {
            this.DiagControl.BackgroundImage = new Bitmap(image);
            this.DiagControl.CustomDrawBackground += new EventHandler<CustomDrawBackgroundEventArgs>(this.DiagControl_CustomDrawBackground);
        }
        public void setBackGround(string imageStr)
        {
            MemoryStream img = new MemoryStream(Convert.FromBase64String(imageStr));
            this.SetBackGround((Stream)img);
        }
        private void DiagControl_ItemContentChanged(object sender, DiagramItemContentChangedEventArgs e)
        {
            string newValue = RemoveCR(e.NewValue);

            if (e.Item.GetType() == typeof(DiagramShape))
            {
                if (IsDup(newValue, (DiagramShape)e.Item))
                {
                    MessageBox.Show(newValue + "은(는) 이미 존재합니다.");
                    DiagramShape shape = (DiagramShape)e.Item;
                    shape.Content = e.OldValue;
                }
                else
                {
                    this.ModifiedItem(newValue);
                }
            }
        }
        private void DiagControl_ItemsChanged(object sender, DiagramItemsChangedEventArgs e)
        {
            if (e.Item.GetType() == typeof(DiagramShape))
            {
                if (e.Action == ItemsChangedAction.Removed)
                {
                    DiagramShape shape = (DiagramShape)e.Item;
                    this.ModifiedItem(shape.Content, true);
                }
            }
        }
        private void DiagControl_QueryItemsAction(object sender, DiagramQueryItemsActionEventArgs e)
        {
            if (e.Action == ItemsActionKind.Copy)
            {
                e.Allow = false;
            }
        }
        public void AddingItem(string name, ShapeDescription kind)
        {
            name = RemoveCR(name);
            if (IsDup(name) == false)
            {
                this.CreateNewShape(name, kind);
                this.ModifiedItem(name);
            }
        }
        
        public void DeletingItem(string name)
        {
            DiagramShape delShape = FindItem(name);
            this.DiagControl.Items.Remove(delShape);

            this.ModifiedItem(name, true);
        }
        private void ModifiedItem(string item, bool isDeleted = false, bool allClear = false)
        {
            try
            {
                if (AllowModifiedEvent) this.ModifiedEvent(item, isDeleted, allClear);
            }
            catch { }
        }
        private void CreateNewShape(string name, ShapeDescription kind)
        {
            x_Pos += 10;
            y_Pos += 10;
            this.DiagControl.Items.Add(
                new DiagramShape
                {
                    Shape = kind,
                    Width = 100,
                    Height = 100,
                    Size = new SizeF(100f, 100f),
                    Position = new PointFloat(x_Pos + 150f, y_Pos),
                    Content = name,
                    Tag = kind.ToString()
                }
            );
            if (x_Pos == 100)
            {
                x_Pos = 0;
                y_Pos = 0;
            }
        }
        private bool IsDup(string name, DiagramShape exceptionShape = null)
        {
            bool rtn = false;

            if (this.AllowDup) return false;

            foreach (DiagramItem item in this.DiagControl.Items)
            {
                if (item.GetType() == typeof(DiagramShape))
                {
                    DiagramShape shape = (DiagramShape)item;
                    if (exceptionShape != null && shape != exceptionShape)
                    {
                        if (shape.Content == name)
                        {
                            rtn = true;
                            break;
                        }
                    }
                }
            }
            return rtn;
        }
        private DiagramShape FindItem(string name)
        {
            DiagramShape rtnShape = null;
            foreach (DiagramItem item in this.DiagControl.Items)
            {
                if (item.GetType() == typeof(DiagramShape))
                {
                    DiagramShape shape = (DiagramShape)item;
                    if (shape.Content == name)
                    {
                        rtnShape = shape;
                        break;
                    }
                }
            }
            return rtnShape;
        }
        public void makeDisabled()
        {
            foreach (DiagramItem item in this.DiagControl.Items)
            {
                item.CanEdit = false;
                item.CanCopy = false;
                item.CanDelete = false;
                item.CanMove = false;
                item.CanRotate = false;
                item.CanResize = false;
            }
        }
        public void ClearDocument()
        {
            this.DiagControl.Items.Clear();
            this.ModifiedItem("", true, true);
            this.DiagControl.FitToDrawing();
        }
        public void LoadDocument(string XML)
        {
            //this.SetDiagramControl();
            this.DiagControl.LoadDocument(this.ChangeStringToStream(XML));
            this.AfterLooading();
        }
        private Stream ChangeStringToStream(string XML)
        {
            MemoryStream img = new MemoryStream(Convert.FromBase64String(XML));
            return img;
        }
        private void AfterLooading()
        {
            foreach (DiagramItem item in this.DiagControl.Items)
            {
                if (item.GetType() == typeof(DiagramShape))
                {
                    DiagramShape shape = (DiagramShape)item;
                    this.ModifiedItem(shape.Content);
                }
            }
        }
        private string RemoveCR(string inStr)
        {
            return inStr.TrimEnd('\r', '\n');
        }
        #endregion
    }
}
