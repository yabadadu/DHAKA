using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using HitopsCommon.GridCommon;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace HitopsCommon.FormTemplate
{
    public partial class BaseMultiGridView : BaseView
    {
        #region FIELD AREA *************************************
        private GridView[] _grids = null;
        private GridView _activeGrid = null;
        private Point _point;
        private TextEditCodeListPairItem[] _textEditCodeListClearItems;
        private GridCell _copyCell = null;
        #endregion

        #region PROPERTY AREA **********************************
        public GridView ActiveGrid
        {
            get
            {
                return this._activeGrid;
            }
            set
            {
                this._activeGrid = value;
            }
        }

        public GridView[] Grids
        {
            get
            {
                return this._grids;
            }
            set
            {
                if (this._activeGrid == null && value.Length > 0)
                {
                    this._activeGrid = value[0];
                }

                this._grids = value;
            }
        }

        public TextEditCodeListPairItem[] TextEditCodeListClearList
        {
            get
            {
                return this._textEditCodeListClearItems;
            }
            set
            {
                this._textEditCodeListClearItems = value;
            }
        }
        #endregion

        public BaseMultiGridView()
            : base()
        {
            this.AddEventHandler();
        }

        private void AddEventHandler()
        {
            this.Load += new EventHandler(BaseMultiGridView_Load);
            return;
        }

        void BaseMultiGridView_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Grids != null && this.Grids.Length > 0)
                {
                    foreach (GridView grid in this.Grids)
                    {
                        // Set DragCell Helper
                        SelectedCellsBorderHelper selectedCellsHelper = new SelectedCellsBorderHelper(grid);
                        new DragCellsValuesHelper(selectedCellsHelper);

                        // Column Init
                        if (grid.Columns == null || grid.Columns.Count == 0) grid.Columns.Clear();

                        // Default Config
                        grid.OptionsView.ShowGroupPanel = false;
                        
                        grid.OptionsSelection.MultiSelect = true;
                        grid.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;

                        grid.FocusRectStyle = DrawFocusRectStyle.RowFocus;
                        grid.IndicatorWidth = BaseGridCommon.getIndicatorDefaultWidth;
                        grid.OptionsClipboard.CopyColumnHeaders = DevExpress.Utils.DefaultBoolean.False;

                        // Default Event
                        grid.RowCellStyle += new RowCellStyleEventHandler(this.gridView_RowCellStyle);

                        grid.ShownEditor += new EventHandler(this.gridView_ShownEditor);
                        grid.ShowingEditor += new CancelEventHandler(this.gridView_ShowingEditor);
                        grid.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.Grid_CustomRowCellEdit);

                        grid.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridView_CustomDrawCell);
                        grid.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);

                        grid.KeyPress += new KeyPressEventHandler(gridView_KeyPress);
                        grid.KeyDown += new KeyEventHandler(gridView_KeyDown);

                        grid.InitNewRow += new InitNewRowEventHandler(gridView_InitNewRow);
                        grid.CellValueChanged += new CellValueChangedEventHandler(this.Grid_CellValueChanged);

                        grid.GridControl.ProcessGridKey += new KeyEventHandler(this.GridControl_ProcessGridKey);
                        grid.GridControl.DataSourceChanged += new EventHandler(this.GridControl_DataSourceChanged);

                        grid.GotFocus += new EventHandler(this.Grid_GotFocus);
                        grid.PopupMenuShowing += new PopupMenuShowingEventHandler(this.Grid_PopupMenuShowing);
                        grid.Click += new EventHandler(this.Grid_Click);
                        grid.MouseDown += new MouseEventHandler(this.Grid_MouseDown);

                        grid.GridControl.KeyDown += new KeyEventHandler(this.GridControl_KeyDown);

                        grid.ShownEditor += new EventHandler(this.grid_ShownEditor);
                        grid.RowCellClick += new RowCellClickEventHandler(this.grid_RowCellClick);
                    }
                }

                if (this.TextEditCodeListClearList != null)
                {
                    foreach (TextEditCodeListPairItem item in this.TextEditCodeListClearList)
                    {
                        if (item != null && item.CheckTextEdit != null && item.ClearTextEdit != null)
                        {
                            (item.CheckTextEdit).TextChanged += new EventHandler(this.TextEdit_TextChanged);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }

            return;
        }
        
        void gridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;

            if (e.RowHandle != view.FocusedRowHandle)
            {
                e.Appearance.BackColor = this.GetCellBackColor(view, e.Column, e.RowHandle);
            }

            if (view.IsCellSelected(e.RowHandle, e.Column) == true)
            {
                e.Appearance.Assign(view.PaintAppearance.SelectedRow);
                e.Appearance.TextOptions.HAlignment = e.Column.AppearanceCell.TextOptions.HAlignment;
            }

            if (view.FocusedRowHandle == e.RowHandle
                && view.IsCellSelected(e.RowHandle, e.Column) == false)
            {
                e.Appearance.BackColor = this.GetCellBackColor(view, e.Column, e.RowHandle);
            }
        }

        Color GetCellBackColor(GridView view, GridColumn col, int rowIndex)
        {
            if (view.OptionsBehavior.Editable == true
                && col.OptionsColumn.AllowEdit == true)
            {
                // Check RowLock or CellLock
                bool isCellLock = BaseGridCommon.IsRowCellLock(view, rowIndex, col.Name);
                if (isCellLock == true)
                {
                    return BaseGridCommon.getBackColorLock;
                }
                else if (isCellLock == false)
                {
                    if (col.ColumnEditName.Contains("RepositoryItemLookUpEdit") == true)
                    {
                        return BaseGridCommon.getBackColorLookup;
                    }
                    else
                    {
                        return BaseGridCommon.getBackColorEdit;
                    }
                }
            }

            return BaseGridCommon.getBackColorLock;
        }
        
        void gridView_ShownEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            GridCell cell = new GridCell(view.FocusedRowHandle, view.FocusedColumn);

            // Check RowLock or CellLock
            if (BaseGridCommon.IsRowCellLock(view, view.FocusedRowHandle, view.FocusedColumn.Name) == true)
            {
                view.CloseEditor();
                return;
            }

            if (view.ActiveEditor != null)
            {
                if (view.FocusedColumn.ColumnEditName.Contains("RepositoryItemLookUpEdit") == true)
                {
                    view.ActiveEditor.BackColor = BaseGridCommon.getBackColorLookup;
                }
                else
                {
                    view.ActiveEditor.BackColor = BaseGridCommon.getBackColorEdit;
                }
            }
        }

        void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            // Check RowLock or CellLock
            if (BaseGridCommon.IsRowCellLock(view, view.FocusedRowHandle, view.FocusedColumn.Name) == true)
            {
                e.Cancel = true;
            }
        }

        private void Grid_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            GridView view = sender as GridView;

            if (view.IsCellSelected(e.RowHandle, e.Column) == true)
            {
                // Check RowLock or CellLock
                if (BaseGridCommon.IsRowCellLock(view, view.FocusedRowHandle, view.FocusedColumn.Name) == true)
                {
                    view.CloseEditor();
                    return;
                }

                if (e.Column.ColumnEditName.Contains("RepositoryItemLookUpEdit") == true)
                {
                    view.Appearance.FocusedCell.BackColor = BaseGridCommon.getBackColorLookup;
                }
                else
                {
                    view.Appearance.FocusedCell.BackColor = BaseGridCommon.getBackColorEdit;
                }
            }
        }

        void gridView_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column == view.FocusedColumn && e.RowHandle == view.FocusedRowHandle)
            {
                CellDrawHelper.DoDefaultDrawCell(view, e);
                CellDrawHelper.DrawCellBorder(e);
                e.Handled = true;
            }
        }

        void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator)
            {
                GridView view = sender as GridView;
                if (view.RowCount > 0)
                {
                    // Set Indicator Width
                    int rowCountlen = view.RowCount.ToString().Length;
                    view.IndicatorWidth = BaseGridCommon.getIndicatorDefaultWidth + (BaseGridCommon.getIndicatorAddWidth * rowCountlen);

                    // Show Row Number
                    if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Row)
                    {
                        string rowNumber = string.Empty;
                        if (e.RowHandle == GridControl.NewItemRowHandle)
                        {
                            rowNumber = (view.RowCount).ToString();
                        }
                        else
                        {
                            rowNumber = (e.RowHandle + 1).ToString();
                        }
                        e.Info.DisplayText = rowNumber;
                    }

                    // Show Custom Image
                    object rowObj = view.GetRow(e.RowHandle);

                    bool isShowCustomImage = false;
                    Image customImage = null;

                    if (rowObj is BaseItem)
                    {
                        if ((rowObj as BaseItem).OpCode == BaseItem.OpCodes.Create)
                        {
                            customImage = Properties.Resources.Actions_Add;
                            isShowCustomImage = true;
                        }
                        else if ((rowObj as BaseItem).OpCode == BaseItem.OpCodes.Update)
                        {
                            customImage = Properties.Resources.Actions_Edit;
                            isShowCustomImage = true;
                        }
                        else if ((rowObj as BaseItem).OpCode == BaseItem.OpCodes.Read
                                 || (rowObj as BaseItem).OpCode == BaseItem.OpCodes.None)
                        {
                            customImage = null;
                            isShowCustomImage = true;
                        }
                    }
                    else
                    {
                        DataRow row = view.GetDataRow(e.RowHandle);
                        if (row != null)
                        {
                            if (row.RowState == DataRowState.Added)
                            {
                                customImage = Properties.Resources.Actions_Add;
                                isShowCustomImage = true;
                            }
                            else if (row.RowState == DataRowState.Modified)
                            {
                                customImage = Properties.Resources.Actions_Edit;
                                isShowCustomImage = true;
                            }
                            else if (row.RowState == DataRowState.Unchanged)
                            {
                                customImage = null;
                                isShowCustomImage = true;
                            }
                        }
                    }

                    if (isShowCustomImage == true)
                    {
                        e.Info.ImageIndex = -1;
                        e.Painter.DrawObject(e.Info);
                        Rectangle r = e.Bounds;
                        r.Inflate(-1, -1);

                        if (customImage != null)
                        {
                            int imgWidth = customImage.Size.Width;
                            int imgHeight = customImage.Size.Height;

                            customImage = resizeImage(customImage, new Size(imgWidth / 2, imgHeight / 2));

                            int x = r.X + (r.Width - customImage.Size.Width / 1);
                            int y = r.Y + (r.Height - customImage.Size.Height) / 2;
                            e.Graphics.DrawImageUnscaled(customImage, x, y);
                        }

                        e.Handled = true;
                    }

                }
            }
        }

        void gridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (e.KeyChar == (char)22)   // Ctrl+V
            {
                BaseGridCommon.CopyPasteRangeClipboard(gridView);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)3)  //Ctrl + C
            {
                // Lookup 의 경우 복사시 value 값을 Clipboard copy
                if (gridView.FocusedColumn.ColumnEdit.GetType() == typeof(RepositoryItemLookUpEdit))
                {
                    string value = gridView.GetRowCellValue(gridView.FocusedRowHandle, gridView.FocusedColumn).ToString();
                    Clipboard.SetText(value);
                }
            }
        }

        void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            // N/A
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        
        private void gridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView grid = sender as GridView;
            object addedObj = grid.GetRow(e.RowHandle);

            if (addedObj is BaseItem)
            {
                (addedObj as BaseItem).OpCode = BaseItem.OpCodes.Create;
            }
        }

        private void Grid_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            GridView gridView = sender as GridView;
            bool isDate = false;

            // Check Date
            if (e.Column.Tag != null && e.Value != null
                && string.IsNullOrEmpty(e.Value.ToString()) == false
                && e.Column.Tag.ToString().Contains("DateTime_") == true)
            {
                string format = e.Column.Tag.ToString().Replace("DateTime_", "");

                if (format == "YYYY-MM-DD")
                {
                    isDate = CheckDate(e.Value.ToString());
                }
                else if (format == "YYYY-MM-DD HH:MM:SS")
                {
                    isDate = CheckDate(e.Value.ToString());
                }
                else if (format == "YYYYMMDD")
                {
                    isDate = CheckDate(e.Value.ToString());
                }
                else if (format == "YYYYMMDD HHMMSS")
                {
                    isDate = CheckDate(e.Value.ToString());
                }
                else if (format == "MM-DD")
                {
                    isDate = CheckDate(DateTime.Now.Year.ToString() + "-" + e.Value.ToString());
                }
                else if (format == "MMDD")
                {
                    isDate = CheckDate(DateTime.Now.Year.ToString() + e.Value.ToString());
                }

                if (isDate == false)
                {
                    gridView.SetRowCellValue(e.RowHandle, e.Column.FieldName, string.Empty);
                }
            }

            // for DataTable : Grid Cell Editor만 열었다 닫아도 Modified 되는 것을 방지
            object rowObj = gridView.GetRow(e.RowHandle);
            if (rowObj is System.Data.DataRowView)
            {
                DataRow row = gridView.GetDataRow(e.RowHandle);
                if (row != null)
                {
                    if (row.RowState == DataRowState.Unchanged)
                    {
                        string orgValue = row[e.Column.FieldName, DataRowVersion.Original].ToString();
                        string currValue = e.Value.ToString();
                        if (orgValue == currValue)
                        {
                            row.AcceptChanges();
                        }
                    }
                }
            }

            gridView.UpdateCurrentRow();
        }

        private void GridControl_ProcessGridKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GridView grid = (sender as GridControl).MainView as GridView;

            if (e.KeyCode == Keys.Enter)
            {
                if (grid.ActiveEditor != null 
                    && grid.ActiveEditor.GetType() != typeof(LookUpEdit))
                {
                    grid.CloseEditor();
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                int rowIdx = grid.FocusedRowHandle;
                GridColumn column = grid.FocusedColumn;

                if (BaseGridCommon.IsRowCellLock(grid, rowIdx, column.FieldName) == false
                    && BaseGridCommon.IsRowLock(grid, rowIdx) == false)
                {
                    grid.SetRowCellValue(rowIdx, column, string.Empty);
                }
            }
        }

        private void GridControl_DataSourceChanged(object sender, EventArgs e)
        {
            GridView grid = (sender as GridControl).MainView as GridView;
            this.DisplayRowCount(grid);
        }

        private void Grid_GotFocus(object sender, EventArgs e)
        {
            GridView grid = sender as GridView;
            this.DisplayRowCount(grid);
        }

        private void Grid_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Column)
            {
                e.Menu.Items[GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnAutoFilterRowShow)].Visible = false;
            }
        }

        private void Grid_Click(object sender, EventArgs e)
        {
            GridView grid = sender as GridView;
            int focusedRowIdx = grid.FocusedRowHandle;
            GridColumn focusedColumn = grid.FocusedColumn;

            if (focusedRowIdx < 0) return;
            if (focusedColumn == null) return;

            GridHitInfo info = grid.CalcHitInfo(grid.GridControl.PointToClient(MousePosition));
            if (info.InRow == false && info.InRowCell == false) return;

            // for Check Box Click
            if (focusedColumn.Name == "CHECK"
                || (focusedColumn.ColumnEdit != null && focusedColumn.ColumnEdit.GetType() == typeof(RepositoryItemCheckEdit)))
            {
                // Check RowLock or CellLock
                if (BaseGridCommon.IsRowCellLock(grid, grid.FocusedRowHandle, grid.FocusedColumn.Name) == true)
                {
                    grid.CloseEditor();
                    return;
                }

                string checkValue = string.Empty;
                if (grid.GetRowCellValue(focusedRowIdx, focusedColumn) != null)
                {
                    checkValue = grid.GetRowCellValue(focusedRowIdx, focusedColumn).ToString();
                }

                if (checkValue == "Y")
                {
                    // Uncheck
                    grid.SetRowCellValue(focusedRowIdx, focusedColumn, "N");
                }
                else
                {
                    // Check
                    grid.SetRowCellValue(focusedRowIdx, focusedColumn, "Y");
                }
            }

            // for Button Edit
            if (focusedColumn.ColumnEdit != null && focusedColumn.ColumnEdit.GetType() == typeof(RepositoryItemButtonEdit))
            {
                grid.ShowEditor();

                // Force button click  
                ButtonEdit edit = (grid.ActiveEditor as ButtonEdit);
                if (edit != null)
                {
                    Point p = grid.GridControl.PointToScreen(this._point);
                    p = edit.PointToClient(p);
                    EditHitInfo ehi = (edit.GetViewInfo() as ButtonEditViewInfo).CalcHitInfo(p);
                    if (ehi.HitTest == EditHitTest.Button)
                    {
                        edit.PerformClick(ehi.HitObject as EditorButton);
                        ((DevExpress.Utils.DXMouseEventArgs)e).Handled = true;
                    }
                }
            }
        }

        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            this._point = e.Location;
        }

        private void GridControl_KeyDown(object sender, KeyEventArgs e)
        {
            GridControl gridControl = sender as GridControl;
            GridView gridView = gridControl.MainView as GridView;
            
            if (e.KeyCode == Keys.Right)
            {
                if (gridView.FocusedColumn == gridView.VisibleColumns[gridView.VisibleColumns.Count - 1])
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                if (gridView.FocusedColumn == gridView.VisibleColumns[0])
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void grid_ShownEditor(object sender, EventArgs e)
        {
            GridView gridView = sender as GridView;
            gridView.ActiveEditor.IsModified = true;

            if (gridView.ActiveEditor is LookUpEdit)
            {
                ((LookUpEdit)gridView.ActiveEditor).ShowPopup();
            }
        }

        private void grid_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (e.Button == MouseButtons.Right)
            {
                if (e.Column.ColumnEditName.Contains("RepositoryItemLookUpEdit") == true)
                {
                    RepositoryItemLookUpEdit lookup = e.Column.ColumnEdit as RepositoryItemLookUpEdit;
                    if (lookup != null)
                    {
                        gridView.GridControl.BeginInvoke(new MethodInvoker(() => { gridView.ShowEditor(); }));
                    }
                }
            }
        }

        private void TextEdit_TextChanged(object sender, EventArgs e)
        {
            TextEdit checkTextEdit = sender as TextEdit;

            foreach (TextEditCodeListPairItem item in this.TextEditCodeListClearList)
            {
                if (item.CheckTextEdit.Equals(checkTextEdit) == true
                    && string.IsNullOrEmpty(checkTextEdit.Text) == true)
                {
                    item.ClearTextEdit.Text = string.Empty;
                }
            }
        }

        private void DisplayRowCount(GridView grid)
        {
            string rowCount = grid.RowCount.ToString();
            CommFunc.SetStatusBarMessage("조회 건수 : " + rowCount);
        }

        private bool CheckDate(string dateStr)
        {
            DateTime dateTime;

            if (DateTime.TryParse(dateStr, out dateTime) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
