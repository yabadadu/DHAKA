#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using System.Data;
using HitopsCommon.FormTemplate;
using System.ComponentModel;
using HitopsCommon.GridCommon;

#endregion

namespace HitopsCommon
{
    public static class GridHelper
    {
        #region Enums
        public enum ColType
        {
            None = 0,
            Edit = 1,
            Lock = 2,
            Hide = 4
        }

        public enum CellLockType
        {
            Add = 0,
            Remove = 1
        }
        #endregion

        #region fields
        private static Dictionary<GridControl, GridHelpSpec> _gridControlRegistry = new Dictionary<GridControl, GridHelpSpec>();
        private static Dictionary<string, GridHelpSpecCfg> _gridHelpSpecTemplate = new Dictionary<string, GridHelpSpecCfg>();
        #endregion

        #region properties
        public static Dictionary<string, GridHelpSpecCfg> GridHelpSpecTemplate
        {
            get { return _gridHelpSpecTemplate; }
            set { _gridHelpSpecTemplate = value; }
        }
        #endregion

        #region initialize
      
        public static void InitGrid(GridControl gridControl, string template)
        {
            var templateCfg = GetGridHelpSpecCfgTemplate(template);
            if (templateCfg == null)
            {
                InitGridDefault(gridControl);
            }
            else
            {
                InitGrid(gridControl, templateCfg);
            }
        }

        public static void InitGrid(GridControl gridControl, GridHelpSpecCfg helpSpecCfg)
        {
            if (_gridControlRegistry.ContainsKey(gridControl)) return;

            var gridHelpSpec = new GridHelpSpec(gridControl)
                               {
                                   GridHelpSpecCfg = helpSpecCfg
                               };
            _gridControlRegistry.Add(gridControl, gridHelpSpec);

            UnBindingEventHandler(gridHelpSpec);
            SetActivity(gridHelpSpec);
            BindingEventHandler(gridHelpSpec);
        }

        public static void InitGridDefault(GridControl gridControl)
        {
            if (_gridControlRegistry.ContainsKey(gridControl) != false) return;

            var gridHelpSpec = new GridHelpSpec(gridControl)
                               {
                                   GridHelpSpecCfg =
                                   {
                                       MultiSelectionAutoReverse = true,
                                       MultiSelectionAutoReverseLazy = true,
                                       MultiSelection = true,
                                       RowIndicatorVisible = true,
                                       MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect,
                                       ResetSelectionClickOutsideCheckboxSelector = false,

                                       ClipboardAllowCopy = true,
                                       ClipboardCopyColumnHearders = true,
                                       ClipboardCopyCollapsedData = true
                                   }
                               };

            _gridControlRegistry.Add(gridControl, gridHelpSpec);

            UnBindingEventHandler(gridHelpSpec);
            SetActivity(gridHelpSpec);
            BindingEventHandler(gridHelpSpec);
        }

        private static void SetActivity(GridHelpSpec gridHelpSpec)
        {
            SetGridMultiSelection(gridHelpSpec);
            SetGridClipboardOption(gridHelpSpec);

            gridHelpSpec.GridControl.Invalidate();
        }
        
        private static void BindingEventHandler(GridHelpSpec gridHelpSpec)
        {
            var helpSpecCfg = gridHelpSpec.GridHelpSpecCfg;
            if (helpSpecCfg.MultiSelection && helpSpecCfg.MultiSelectionAutoReverse)
            {
                BindingMultiSelectionAutoReverseEvent(gridHelpSpec);
            }

            if (helpSpecCfg.RowIndicatorVisible)
            {
                BindingCustomDrawRowIndicatorEvent(gridHelpSpec);
            }
        }

        private static void SetGridMultiSelection(GridHelpSpec gridHelpSpec)
        {
            var helpSpecCfg = gridHelpSpec.GridHelpSpecCfg;
            var view = gridHelpSpec.GridControl.FocusedView as GridView;

            if (view == null) return;

            bool enable = helpSpecCfg.MultiSelection;
            view.OptionsSelection.MultiSelect = enable;
            view.OptionsSelection.MultiSelectMode = enable ? helpSpecCfg.MultiSelectMode : GridMultiSelectMode.RowSelect;
            view.OptionsSelection.CheckBoxSelectorColumnWidth = enable ? 40 : 0;
            view.OptionsSelection.ResetSelectionClickOutsideCheckboxSelector = helpSpecCfg.ResetSelectionClickOutsideCheckboxSelector;
        }

        private static void SetGridClipboardOption(GridHelpSpec gridHelpSpec)
        {
            var helpSpecCfg = gridHelpSpec.GridHelpSpecCfg;
            var view = gridHelpSpec.GridControl.FocusedView as GridView;

            if (view == null) return;

            var allowCopyEnum = helpSpecCfg.ClipboardAllowCopy ? DefaultBoolean.True : DefaultBoolean.False;
            view.OptionsClipboard.AllowCopy = allowCopyEnum;

            var allowCopyCollapsedData = helpSpecCfg.ClipboardCopyCollapsedData ? DefaultBoolean.True : DefaultBoolean.False;
            view.OptionsClipboard.CopyCollapsedData = allowCopyCollapsedData;
            
            var allowCopyColumnHeaders = helpSpecCfg.ClipboardCopyColumnHearders ? DefaultBoolean.True : DefaultBoolean.False;
            view.OptionsClipboard.CopyColumnHeaders = allowCopyColumnHeaders;
        }

        private static void BindingMultiSelectionAutoReverseEvent(GridHelpSpec gridHelpSpec)
        {
            try
            {
                var gridControl = gridHelpSpec.GridControl;
                gridControl.MouseDown += OnGridControlMouseDown;
                gridControl.MouseUp += OnGridControlMouseUp;
                gridControl.MouseMove += OnGridControlMouseMove;
                
                //background control
                var gridView = gridControl.FocusedView as GridView;
                gridView.RowStyle += OnGridViewRowStyle;
                gridView.RowCellStyle += OnGridViewRowCellStyle;

                var timer = gridHelpSpec.GridScrolltimer;
                timer.Interval = 100;
                timer.Tag = gridHelpSpec;
                timer.Tick += OnGridScrollTimerTick;
                timer.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        

        private static void UnBindingEventHandler(GridHelpSpec gridHelpSpec)
        {
            try
            {
                var gridControl = gridHelpSpec.GridControl;
                gridControl.MouseDown -= OnGridControlMouseDown;
                gridControl.MouseUp -= OnGridControlMouseUp;
                gridControl.MouseMove -= OnGridControlMouseMove;
                var view = gridControl.FocusedView as GridView;
                if (view != null)
                {
                    view.CustomDrawRowIndicator -= CustomDrawRowIndicator;
                    view.RowStyle -= OnGridViewRowStyle;
                    view.RowCellStyle -= OnGridViewRowCellStyle;
                }

                var timer = gridHelpSpec.GridScrolltimer;
                timer.Tick -= OnGridScrollTimerTick;
                timer.Tag = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddCheckBox(GridView gridView)
        {
            try
            {
                gridView.OptionsSelection.MultiSelect = true;
                gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                gridView.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        

        private static GridColumn GetStandardGridColumn(string fieldName, string caption, bool visible, bool editable, int? width)
        {
            GridColumn resultCol = null;

            if (string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true)
                return resultCol;

            try
            {
                resultCol = new GridColumn();
                resultCol.Name = "GridColumn_" + fieldName;
                resultCol.FieldName = fieldName;
                resultCol.Caption = caption;
                resultCol.Visible = visible;
                resultCol.OptionsColumn.AllowEdit = editable;

                if (width.HasValue == true) resultCol.Width = width.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return resultCol;
        }

        private static GridColumn GetStandardGridColumn(string fieldName, string caption, bool visible, bool editable)
        {
            return GetStandardGridColumn(fieldName, caption, visible, editable, null);
        }

        public static void AddTextGridColumn(GridView grid, string fieldName, string caption, ColType visEditType)
        {
            AddTextGridColumn(grid, fieldName, caption, visEditType, null, null);
        }

        public static void AddTextGridColumn(GridView grid, string fieldname, string caption, ColType visEditType, int? width, int? maxLength, HorzAlignment align = HorzAlignment.Default)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddTextGridColumn(grid, fieldname, caption, visible, editable, width, maxLength, align);
        }

        public static void AddTextGridColumn(GridView grid, string fieldName, string caption, bool visible, bool editable, int? width, int? maxLength, HorzAlignment align = HorzAlignment.Default)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemTextEdit repText = new RepositoryItemTextEdit();
                repText.Name = "RepositoryItemTextEdit_" + fieldName;
                if (align != HorzAlignment.Default) repText.Appearance.TextOptions.HAlignment = align;
                if (maxLength.HasValue == true) repText.MaxLength = maxLength.Value;
                
                newCol.ColumnEdit = repText;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.AppearanceCell.TextOptions.HAlignment = align;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddMemoGridColumn(GridView grid, string fieldname, string caption, ColType visEditType, int? width, int? maxLength, HorzAlignment align = HorzAlignment.Default)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddMemoGridColumn(grid, fieldname, caption, visible, editable, width, maxLength, align);
        }

        public static void AddMemoGridColumn(GridView grid, string fieldName, string caption, bool visible, bool editable, int? width, int? maxLength, HorzAlignment align = HorzAlignment.Default)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemMemoEdit repMemo = new RepositoryItemMemoEdit();
                repMemo.Name = "RepositoryItemMemoEdit_" + fieldName;
                if (align != HorzAlignment.Default) repMemo.Appearance.TextOptions.HAlignment = align;
                if (maxLength.HasValue == true) repMemo.MaxLength = maxLength.Value;

                newCol.ColumnEdit = repMemo;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.AppearanceCell.TextOptions.HAlignment = align;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddDateTimeGridColumn(GridView grid, string fieldName, string caption, ColType visEditType, string format, int? width)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddDateTimeGridColumn(grid, fieldName, caption, visible, editable, format, width);
        }

        public static void AddDateTimeGridColumn(GridView grid, string fieldName, string caption, bool visible, bool editable, string format, int? width)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemTextEdit repText = new RepositoryItemTextEdit();
                repText.Name = "RepositoryItemTextEdit_" + fieldName;
                repText.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                repText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                repText.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Default;
                repText.Mask.ShowPlaceHolders = false;
                repText.Mask.UseMaskAsDisplayFormat = false;

                string dateFormat = "0000-00-00"; // Default format

                if (format.ToUpper() == "YYYY-MM-DD")
                {
                    dateFormat = "0000-00-00";
                }
                else if (format.ToUpper() == "YYYY-MM-DD HH:MM:SS")
                {
                    dateFormat = "0000-00-00 00:00:00";
                }
                else if (format.ToUpper() == "YYYYMMDD")
                {
                    dateFormat = "0000-00-00";
                }
                else if (format.ToUpper() == "YYYYMMDD HHMMSS")
                {
                    dateFormat = "0000-00-00 00:00:00";
                }
                else if (format.ToUpper() == "MM-DD")
                {
                    dateFormat = "00-00";
                }
                else if (format.ToUpper() == "MMDD")
                {
                    dateFormat = "00-00";
                }

                repText.Mask.EditMask = dateFormat;
                
                newCol.ColumnEdit = repText;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.Name = fieldName;

                newCol.Tag = "DateTime_" + format.ToUpper();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddStartEndTimeGridColumn(GridView grid, string fieldName, string caption, ColType visEditType, int? width)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddStartEndTimeGridColumn(grid, fieldName, caption, visible, editable, width);
        }

        public static void AddStartEndTimeGridColumn(GridView grid, string fieldName, string caption, bool visible, bool editable, int? width)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemTextEdit repText = new RepositoryItemTextEdit();
                repText.Name = "RepositoryItemTextEdit_" + fieldName;
                repText.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                repText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                repText.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Default;
                repText.Mask.ShowPlaceHolders = false;
                repText.Mask.UseMaskAsDisplayFormat = false;
                repText.Mask.EditMask = "00:00~00:00";

                newCol.ColumnEdit = repText;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddMrnMaskGridColumn(GridView grid, string fieldName, string caption, ColType visEditType, int? width)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddMrnMaskGridColumn(grid, fieldName, caption, visible, editable, width);
        }

        public static void AddMrnMaskGridColumn(GridView grid, string fieldName, string caption, bool visible, bool editable, int? width)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemTextEdit repText = new RepositoryItemTextEdit();
                repText.Name = "RepositoryItemTextEdit_" + fieldName;
                repText.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                repText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                repText.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Default;
                repText.Mask.ShowPlaceHolders = true;
                repText.Mask.UseMaskAsDisplayFormat = true;
                
                string MRN_Format = "[0-9A-Z]{11}-[0-9A-Z]{7}"; // Default format
                
                repText.Mask.EditMask = MRN_Format;
                
                newCol.ColumnEdit = repText;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddComboBoxGridColumn (GridView grid, string fieldName, string caption, bool visible, bool editable, int? width, RepositoryItemComboBox repCmb)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                repCmb.Name = "RepositoryItemComboBox_" + fieldName;
                newCol.ColumnEdit = repCmb;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddLookupGridColumn(GridView grid, string fieldName, string caption, ColType visEditType, int? width, RepositoryItemLookUpEdit repLookUp, HorzAlignment hAlign = HorzAlignment.Center)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddLookupGridColumn(grid, fieldName, caption, visible, editable, width, repLookUp, hAlign);
        }

        public static void AddLookupGridColumn (GridView grid, string fieldName, string caption, bool visible, bool editable, int? width, RepositoryItemLookUpEdit repCmb, HorzAlignment hAlign = HorzAlignment.Center)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                repCmb.Name = "RepositoryItemLookUpEdit_" + fieldName;
                newCol.ColumnEdit = repCmb;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceCell.TextOptions.HAlignment = hAlign;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddNumericGridColumn (GridView grid, string fieldName, string caption, ColType visEditType, int? width, string format)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddNumericGridColumn(grid, fieldName, caption, visible, editable, width, format);
        }

        public static void AddNumericGridColumn (GridView grid, string fieldName, string caption, bool visible, bool editable, int? width, string format)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemTextEdit repText = new RepositoryItemTextEdit();
                repText.Name = "RepositoryItemTextEdit_" + fieldName;
                repText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                repText.Mask.EditMask = format;
                repText.AllowNullInput = DefaultBoolean.True;
                repText.Mask.UseMaskAsDisplayFormat = true;
                repText.Appearance.Options.UseTextOptions = true;
                repText.Appearance.TextOptions.HAlignment = HorzAlignment.Far;

                newCol.ColumnEdit = repText;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddButtonGridColumn(GridView grid, string fieldName, string caption, ColType visEditType, int? width)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddButtonGridColumn(grid, fieldName, caption, visible, editable, width);
        }

        public static void AddButtonGridColumn(GridView grid, string fieldName, string caption, bool visible, bool editable, int? width)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemButtonEdit repButton = new RepositoryItemButtonEdit();
                repButton.Name = "RepositoryItemButtonEdit_" + fieldName;
                repButton.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
                repButton.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                repButton.Buttons[0].Caption = caption;
                repButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;

                newCol.ColumnEdit = repButton;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddCheckGridColumn(GridView grid, string fieldName, string caption, ColType visEditType, int? width)
        {
            bool visible = (visEditType == ColType.Edit || visEditType == ColType.Lock) ? true : false;
            bool editable = (visEditType == ColType.Edit) ? true : false;

            AddCheckGridColumn(grid, fieldName, caption, visible, editable, width);
        }

        public static void AddCheckGridColumn(GridView grid, string fieldName, string caption, bool visible, bool editable, int? width)
        {
            if (grid == null || string.IsNullOrEmpty(fieldName) == true || string.IsNullOrEmpty(caption) == true) return;

            try
            {
                if (string.IsNullOrEmpty(caption) == true) { caption = " "; }

                GridColumn newCol = GetStandardGridColumn(fieldName, caption, visible, editable, width);
                grid.Columns.Add(newCol);

                RepositoryItemCheckEdit repCheck = new RepositoryItemCheckEdit();
                repCheck.Name = "RepositoryItemCheckEdit_" + fieldName;
                repCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
                repCheck.ValueChecked = "Y";
                repCheck.ValueUnchecked = "N";
                
                newCol.ColumnEdit = repCheck;
                newCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                newCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                newCol.AppearanceHeader.Options.UseTextOptions = true;
                newCol.Name = fieldName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetRowLock(GridView grid, int rowIndex)
        {
            try
            {
                object dataSource = grid.GridControl.DataSource;
                if (dataSource is DataTable)
                {
                    // Check RowLock Column
                    var columns = grid.Columns.Where(col => col.FieldName == BaseGridCommon.RowLockColumnName);
                    if (columns.Count() == 0)
                    {
                        // Add RowLock Column
                        (dataSource as DataTable).Columns.Add(BaseGridCommon.RowLockColumnName);
                        AddTextGridColumn(grid, BaseGridCommon.RowLockColumnName, BaseGridCommon.RowLockColumnName, GridHelper.ColType.Hide);
                    }

                    grid.SetRowCellValue(rowIndex, BaseGridCommon.RowLockColumnName, "Y");
                }
                else if (dataSource is BaseItem)
                {
                    BaseItem baseItem = grid.GetRow(rowIndex) as BaseItem;
                    baseItem.RowLock = true;
                }

                grid.RefreshRow(rowIndex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetRowUnlock(GridView grid, int rowIndex)
        {
            try
            {
                object dataSource = grid.GridControl.DataSource;
                if (dataSource is DataTable)
                {
                    // Check RowLock Column
                    var columns = grid.Columns.Where(col => col.Name == BaseGridCommon.RowLockColumnName);
                    if (grid.Columns.Count() == 0)
                    {
                        // Add RowLock Column
                        (dataSource as DataTable).Columns.Add(BaseGridCommon.RowLockColumnName);
                        AddTextGridColumn(grid, BaseGridCommon.RowLockColumnName, BaseGridCommon.RowLockColumnName, GridHelper.ColType.Hide);
                    }

                    grid.SetRowCellValue(rowIndex, BaseGridCommon.RowLockColumnName, "N");
                }
                else if (dataSource is BaseItem)
                {
                    BaseItem baseItem = grid.GetRow(rowIndex) as BaseItem;
                    baseItem.RowLock = false;
                }

                grid.RefreshRow(rowIndex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetCellLock(GridView gridView, BaseItem item, string fieldName, bool doRefresh = true)
        {
            if (item == null || string.IsNullOrEmpty(fieldName) == true) return;

            try
            {
                if (item != null)
                {
                    string cellLockFields = ConvertCellLockFields(item.CellLockFieldList, fieldName, CellLockType.Add);
                    item.CellLockFieldList = cellLockFields;
                }

                if (doRefresh == true) gridView.RefreshData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetCellLock(GridView gridView, BaseItem item, string[] fields)
        {
            if (item == null || fields == null || fields.Count() == 0) return;

            try
            {
                foreach(string fieldName in fields)
                {
                    SetCellLock(gridView, item, fieldName, false);
                }

                gridView.RefreshData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetCellLock(GridView grid, int rowIndex, string fieldName)
        {
            try
            {
                bool isUnchanged = false;

                object dataSource = grid.GridControl.DataSource;
                if (dataSource is DataTable)
                {
                    isUnchanged = false;
                    DataRow row = grid.GetDataRow(rowIndex);
                    if (row.RowState == DataRowState.Unchanged)
                    {
                        isUnchanged = true;
                    }

                    // Check CellLock Column
                    var columns = grid.Columns.Where(col => col.FieldName == BaseGridCommon.CellLockColumnName);
                    if (columns.Count() == 0)
                    {
                        // Add CellLock Column
                        (dataSource as DataTable).Columns.Add(BaseGridCommon.CellLockColumnName);
                        AddTextGridColumn(grid, BaseGridCommon.CellLockColumnName, BaseGridCommon.CellLockColumnName, GridHelper.ColType.Hide);
                    }

                    string cellValue = grid.GetRowCellValue(rowIndex, BaseGridCommon.CellLockColumnName).ToString();
                    string cellLockFields = ConvertCellLockFields(cellValue, fieldName, CellLockType.Add);

                    grid.SetRowCellValue(rowIndex, BaseGridCommon.CellLockColumnName, cellLockFields);

                    if (isUnchanged == true)
                    {
                        row.AcceptChanges();
                    }
                }
                else
                {
                    BaseItem baseItem = grid.GetRow(rowIndex) as BaseItem;
                    
                    if (baseItem != null)
                    {
                        string cellLockFields = ConvertCellLockFields(baseItem.CellLockFieldList, fieldName, CellLockType.Add);
                        baseItem.CellLockFieldList = cellLockFields;
                    }
                }

                grid.RefreshRow(rowIndex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetCellUnlock(GridView gridView, BaseItem item, string fieldName, bool doRefresh = true)
        {
            if (item == null || string.IsNullOrEmpty(fieldName) == true) return;

            try
            {
                if (item != null)
                {
                    string cellLockFields = ConvertCellLockFields(item.CellLockFieldList, fieldName, CellLockType.Remove);
                    item.CellLockFieldList = cellLockFields;
                }

                if (doRefresh == true) gridView.RefreshData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetCellUnlock(GridView gridView, BaseItem item, string[] fields)
        {
            if (item == null || fields == null || fields.Count() == 0) return;

            try
            {
                foreach (string fieldName in fields)
                {
                    SetCellUnlock(gridView, item, fieldName, false);
                }

                gridView.RefreshData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetCellUnlock(GridView grid, int rowIndex, string fieldName)
        {
            try
            {
                object dataSource = grid.GridControl.DataSource;
                if (dataSource is DataTable)
                {
                    // Check CellLock Column
                    var columns = grid.Columns.Where(col => col.FieldName == BaseGridCommon.CellLockColumnName);
                    if (columns.Count() == 0)
                    {
                        // Add CellLock Column
                        (dataSource as DataTable).Columns.Add(BaseGridCommon.CellLockColumnName);
                        AddTextGridColumn(grid, BaseGridCommon.CellLockColumnName, BaseGridCommon.CellLockColumnName, GridHelper.ColType.Hide);
                    }

                    string cellValue = grid.GetRowCellValue(rowIndex, BaseGridCommon.CellLockColumnName).ToString();
                    string cellLockFields = ConvertCellLockFields(cellValue, fieldName, CellLockType.Remove);

                    grid.SetRowCellValue(rowIndex, BaseGridCommon.CellLockColumnName, cellLockFields);
                }
                else
                {
                    BaseItem baseItem = grid.GetRow(rowIndex) as BaseItem;
                    if (baseItem != null)
                    {
                        string cellLockFields = ConvertCellLockFields(baseItem.CellLockFieldList, fieldName, CellLockType.Remove);
                        baseItem.CellLockFieldList = cellLockFields;
                    }
                }

                grid.RefreshRow(rowIndex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetCellLockAllRowsByFields(GridView grid, string[] fields)
        {
            try
            {
                object dataSource = grid.GridControl.DataSource;

                foreach (string field in fields)
                {
                    for (int i = 0; i < grid.RowCount; i++)
                    {
                        SetCellLock(grid, i, field);
                    }
                }
                
                grid.RefreshData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string ConvertCellLockFields(string cellValue, string fieldName, CellLockType addOrRemove)
        {
            string converStr = string.Empty;
            List<string> fieldList = new List<string>();
            
            if (string.IsNullOrEmpty(cellValue) == false)
            {
                fieldList = cellValue.Split(',').ToList();
            }
            
            if (addOrRemove == CellLockType.Add)
            {
                if (fieldList.Count == 0)
                {
                    return fieldName;
                }
                else
                {
                    if (fieldList.IndexOf(fieldName) < 0)
                    {
                        fieldList.Add(fieldName);
                    }
                    return string.Join(",", fieldList);
                }
            }
            else if (addOrRemove == CellLockType.Remove)
            {
                if (fieldList.Count > 0)
                {
                    if (fieldList.IndexOf(fieldName) > -1)
                    {
                        fieldList.Remove(fieldName);
                    }
                    return string.Join(",", fieldList);
                }
            }
            
            return converStr;
        }


        #endregion

        #region eventHandler
        private static void OnGridControlMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var gridControl = sender as GridControl;
                if (gridControl == null) return;

                if (_gridControlRegistry.ContainsKey(gridControl) == false) return;

                if (IsRowIndicatorClicked(sender, e))
                {
                    var gridHelpSpec = _gridControlRegistry[gridControl];
                    var helpInfo = gridHelpSpec.GridHelpSpecInfo;
                    // 클릭 상태 on
                    helpInfo.IsMouseDragOn = true;

                    // event handle을 deselect. row indicator로 인한 multi select를 막는다.
                    (e as DXMouseEventArgs).Handled = true;

                    var view = gridControl.FocusedView as GridView;
                    if (view == null) return;
                    // 현재 row handle
                    var currRowhandle = GetRowHandleAt(gridHelpSpec, e.Location);
                    helpInfo.PrevRowIdx = currRowhandle;

                    var isMultiSelectionLazy = gridHelpSpec.GridHelpSpecCfg.MultiSelectionAutoReverseLazy;
                    if (isMultiSelectionLazy)
                    {
                        helpInfo.LazySelStartRowHandle = currRowhandle;
                        helpInfo.LazySelEndRowHanle = currRowhandle;
                        view.RefreshRow(currRowhandle);
                    }
                    else
                    {
                        // selection list와 비교.
                        if (view.IsRowSelected(currRowhandle))
                        {
                            view.UnselectRow(currRowhandle);
                        }
                        else
                        {
                            view.SelectRow(currRowhandle);
                        }

                        helpInfo.TreatedRows.Add(currRowhandle);
                    }
                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static bool IsRowIndicatorClicked(object sender, MouseEventArgs e)
        {
            var point = e.Location;
            var gridControl = sender as GridControl;
            if (gridControl == null) return false;

            var view = gridControl.FocusedView as GridView;
            if (view == null) return false;

            var gridHitInfo = view.CalcHitInfo(point);
            return gridHitInfo.HitTest == GridHitTest.RowIndicator;
        }

        private static void OnGridControlMouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                var gridHelpSpec = GetGridHelpSpecFromDic(sender);
                if (gridHelpSpec != null)
                {
                    var isMultiSelectionLazy = gridHelpSpec.GridHelpSpecCfg.MultiSelectionAutoReverseLazy;
                    if (isMultiSelectionLazy)
                    {
                        var view = gridHelpSpec.GridControl.FocusedView as GridView;
                        var startRow = gridHelpSpec.GridHelpSpecInfo.LazySelStartRowHandle;
                        var endRow = gridHelpSpec.GridHelpSpecInfo.LazySelEndRowHanle;
                        SelectRows(view, startRow, endRow);
                    }

                    InitGridHelpSpecInfo(gridHelpSpec);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void InitGridHelpSpecInfo(GridHelpSpec gridHelpSpec)
        {
            var helpInfo = gridHelpSpec.GridHelpSpecInfo;
            //클릭 상태 off
            helpInfo.IsMouseDragOn = false;
            //current row idx를 -1
            helpInfo.PrevRowIdx = -1;
            //current row array 초기화 
            helpInfo.TreatedRows.Clear();

            helpInfo.ScrollingOn = false;
            gridHelpSpec.GridScrolltimer.Stop();

            helpInfo.LazySelStartRowHandle = -1;
            helpInfo.LazySelEndRowHanle = -1;
        }

        private static void OnGridControlMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var gridControl = sender as GridControl;
                if (gridControl == null) return;

                if (_gridControlRegistry.ContainsKey(gridControl) == false) return;

                var gridHelpSpec = _gridControlRegistry[gridControl];
                var helpInfo = gridHelpSpec.GridHelpSpecInfo;

                var view = gridControl.FocusedView as GridView;
                if (view == null) return;
                
                try
                {
                    if (helpInfo.PrevRowIdx == -1) return;

                    //timer will be start with out of grid. 
                    gridHelpSpec.GridScrolltimer.Stop();
                    gridHelpSpec.GridHelpSpecInfo.ScrollingOn = true;

                    // 현재 row handle
                    var currRowhandle = GetRowHandleAt(gridHelpSpec, e.Location);
                    if (helpInfo.PrevRowIdx == -1 || helpInfo.PrevRowIdx == currRowhandle) return;

                    if (helpInfo.IsMouseDragOn == false) return;
                    if (currRowhandle == GridControl.InvalidRowHandle) return;
                    helpInfo.PrevRowIdx = currRowhandle;

                    var isMultiSelectionLazy = gridHelpSpec.GridHelpSpecCfg.MultiSelectionAutoReverseLazy;
                    if (isMultiSelectionLazy)
                    {
                        if (helpInfo.LazySelEndRowHanle != currRowhandle)
                        {
                            helpInfo.LazySelEndRowHanle = currRowhandle;

                            //int startIdx = -1;
                            //int endIdx = -1;
                            //GetStartEnd(helpInfo.LazySelStartRowHandle, helpInfo.LazySelEndRowHanle, ref startIdx, ref endIdx);
                            //for (int i = startIdx; i <= endIdx; i++)
                            //{
                            //    view.RefreshRow(i);
                            //}
                            for (int i = helpInfo.MinRowRange; i <= helpInfo.MaxRowRange; i++)
                            {
                                view.RefreshRow(i);
                            }
                        }
                    }
                    else
                    {
                        // 이번 드래그에 처리 유무
                        if (helpInfo.TreatedRows.Contains(currRowhandle)) return;

                        if (view.IsRowSelected(currRowhandle))
                        {
                            view.UnselectRow(currRowhandle);
                        }
                        else
                        {
                            view.SelectRow(currRowhandle);
                        }

                        helpInfo.TreatedRows.Add(currRowhandle);
                    }
                    
                }
                finally
                {
                    gridHelpSpec.GridHelpSpecInfo.ScrollingOn = false;
                }
                

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        private static void GetStartEnd(int startRow, int endRow, ref int startIdx, ref int endIdx)
        {
            startIdx = startRow >= endRow ? endRow : startRow;
            endIdx = startRow >= endRow ? startRow : endRow;
        }

        private static void OnGridScrollTimerTick(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            var gridHelpSpec = timer.Tag as GridHelpSpec;
            var gridControl = gridHelpSpec.GridControl;

            var mousePosition = gridControl.PointToClient(Control.MousePosition);
            OnGridControlMouseMove(gridControl,
                new MouseEventArgs(Control.MouseButtons, 1, mousePosition.X, mousePosition.Y, 0));

        }

        private static void BindingCustomDrawRowIndicatorEvent(GridHelpSpec gridHelpSpec)
        {
            var view = gridHelpSpec.GridControl.FocusedView as GridView;
            if (view == null) return;
            view.CustomDrawRowIndicator += CustomDrawRowIndicator;
        }

        private static void CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    var displayTxt = (e.RowHandle + 1).ToString();
                    var displayLen = (displayTxt.Length + 2) * 10;

                    var view = sender as GridView;
                    if (view == null) return;
                  
                    if (view.IndicatorWidth < displayLen)
                    {
                        view.IndicatorWidth = displayLen;
                        view.GridControl.RefreshDataSource();
                    }
                    
                    e.Info.DisplayText = displayTxt;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void OnGridViewRowStyle(object sender, RowStyleEventArgs e)
        {
            var view = sender as GridView;
            if (e.RowHandle < 0) return;

            var gridHelpSpec = GetGridHelpSpecFromDic(view);
            if (gridHelpSpec == null) return;

            var cfg = gridHelpSpec.GridHelpSpecCfg;
            var isAutoReverse = cfg.MultiSelectionAutoReverse;
            if (isAutoReverse == false) return;

            var isAutoReverseLazy = gridHelpSpec.GridHelpSpecCfg.MultiSelectionAutoReverseLazy;
            if (isAutoReverseLazy)
            {
                //will selected row range color
                var startRow = gridHelpSpec.GridHelpSpecInfo.LazySelStartRowHandle;
                var endRow = gridHelpSpec.GridHelpSpecInfo.LazySelEndRowHanle;
                
                var isLazySelectedRow = IsLazySelectedRow(e.RowHandle, startRow, endRow);
                if (isLazySelectedRow)
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                    e.HighPriority = true;
                }
                else
                {
                    //selected row color is normal color
                    if (view.IsRowSelected(e.RowHandle))
                    {
                        var isEmptyBackColor = view.Appearance.Row.BackColor == Color.Empty;
                        e.Appearance.BackColor = isEmptyBackColor ? Color.White : view.Appearance.Row.BackColor;
                        e.HighPriority = true;
                    }
                }
            }
        }

        private static bool IsLazySelectedRow(int drawRowHandle, int aRow, int bRow)
        {
            if (aRow < 0) return false;

            var startRow = aRow >= bRow ? bRow : aRow;
            var endRow = aRow >= bRow ? aRow : bRow;

            if (drawRowHandle >= startRow && drawRowHandle <= endRow)
            {
                return true;
            }
            return false;
        }

        private static void OnGridViewRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            var view = sender as GridView;
            if (e.RowHandle < 0) return;

            var gridHelpSpec = GetGridHelpSpecFromDic(view);
            if (gridHelpSpec == null) return;

            var cfg = gridHelpSpec.GridHelpSpecCfg;
            var isAutoReverse = cfg.MultiSelectionAutoReverse;
            if (isAutoReverse == false) return;

            var isAutoReverseLazy = gridHelpSpec.GridHelpSpecCfg.MultiSelectionAutoReverseLazy;
            if (isAutoReverseLazy)
            {
                //will selected row range color
                var startRow = gridHelpSpec.GridHelpSpecInfo.LazySelStartRowHandle;
                var endRow = gridHelpSpec.GridHelpSpecInfo.LazySelEndRowHanle;

                var isLazySelectedRow = IsLazySelectedRow(e.RowHandle, startRow, endRow);
                if (isLazySelectedRow)
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                }
                else
                {
                    //selected row color is normal color
                    if (view.IsRowSelected(e.RowHandle))
                    {
                        var isEmptyBackColor = e.Column.AppearanceCell.BackColor == Color.Empty;
                        e.Appearance.BackColor = isEmptyBackColor ? Color.White : e.Column.AppearanceCell.BackColor;
                    }
                }
            }
        }

        #endregion

        #region general function


        private static int GetRowHandleAt(GridHelpSpec helpSpec, Point point)
        {
            point.X = point.X <= 0 ? 1 : point.X;

            var view = helpSpec.GridControl.FocusedView as GridView;
            var info = helpSpec.GridHelpSpecInfo;

            int result = view.CalcHitInfo(point).RowHandle;
            if (result > 0) return result;

            var isMouseOutOfGrid = result == GridControl.InvalidRowHandle;
            if (isMouseOutOfGrid && info.ScrollingOn)
            {
                var isUpScrolling = ((GridViewInfo) view.GetViewInfo()).ViewRects.Rows.Top > point.Y;
                if (isUpScrolling)
                {
                    result = view.GetVisibleRowHandle(view.TopRowIndex);
                    result += result >= 0 ? -1 : 1;
                    view.TopRowIndex -= 1;
                }
                else
                {
                    int bottomRowIndex = view.TopRowIndex;
                    while (view.IsRowVisible(view.GetVisibleRowHandle(bottomRowIndex)) != RowVisibleState.Hidden)
                        bottomRowIndex += bottomRowIndex >= 0 ? 1 : -1;
                    result = view.GetVisibleRowHandle(bottomRowIndex);
                    view.TopRowIndex += 1;
                }
                helpSpec.GridScrolltimer.Start();

            }
            return result;
        }

        private static GridHelpSpecCfg GetGridHelpSpecCfgTemplate(string template)
        {
            if (GridHelpSpecTemplate.ContainsKey(template))
            {
                return GridHelpSpecTemplate[template].Clone() as GridHelpSpecCfg;
            }
            return null;
        }
        
        public static GridHelpSpecCfg GetGridHelpSpecCfg(GridControl gridControl)
        {
            var gridHelpSpec = GetGridHelpSpecFromDic(gridControl);
            return gridHelpSpec != null ? gridHelpSpec.GridHelpSpecCfg : null;
        }

        public static GridHelpSpec GetGridHelpSpecFromDic(object o)
        {
            var gridControl = o as GridControl;
            if (gridControl == null) return null;

            return _gridControlRegistry.ContainsKey(gridControl) ? _gridControlRegistry[gridControl] : null;
        }

        public static GridHelpSpec GetGridHelpSpecFromDic(GridView view)
        {
            try
            {
                return _gridControlRegistry.Single(g => g.Key.FocusedView.Equals(view)).Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
        public static void InvalidateHelpSpec(GridControl gridControl)
        {
            if (_gridControlRegistry.ContainsKey(gridControl) == false) return;

            var helpSpec = _gridControlRegistry[gridControl];

            UnBindingEventHandler(helpSpec);
            SetActivity(helpSpec);
            BindingEventHandler(helpSpec);
        }

        private static void SelectRows(GridView view, int aRow, int bRow)
        {
            var selectedRows = view.GetSelectedRows().ToDictionary(row => row, row => row);
            var startRow = aRow >= bRow ? bRow : aRow;
            var endRow = aRow >= bRow ? aRow : bRow;

            for (int row = startRow; row <= endRow; row++)
            {
                if (selectedRows.ContainsKey(row))
                {
                    selectedRows.Remove(row);
                }
                else
                {
                    selectedRows.Add(row, row);
                }
            }

            view.BeginSelection();
            view.ClearSelection();

            foreach (var selectedRow in selectedRows)
            {
                view.SelectRow(selectedRow.Value);    
            }

            view.EndSelection();
        }

        public static RepositoryItemButtonEdit GetRepositoryItemButton(GridView gridView, string colName)
        {
            RepositoryItemButtonEdit repButtonEdit = null;

            try
            {
                foreach (GridColumn col in gridView.Columns)
                {
                    if (col == gridView.Columns[colName])
                    {
                        if (col.ColumnEdit.Name.Contains("RepositoryItemButtonEdit") == true)
                        {
                            repButtonEdit = col.ColumnEdit as RepositoryItemButtonEdit;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return repButtonEdit;
        }

        public static void NotifyChangedFocusedRow(GridView gridView)
        {
            if (gridView == null) return;
            if (gridView.GridControl.DataSource == null) return;

            // Force Close Editor
            gridView.CloseEditor();

            // for DataRow
            object rowObj = gridView.GetRow(gridView.FocusedRowHandle);
            if (rowObj is System.Data.DataRowView)
            {
                DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);
                if (row != null) row.EndEdit();
            }
        }

        public static List<GridInfoItem> GetDuplicateRowsInfo(GridView grid, string[] fieldList)
        {
            List<GridInfoItem> duplicateInfo = new List<GridInfoItem>();

            // Check Length
            if (fieldList == null || fieldList.Length == 0) return null;

            // Check Exist
            bool isNotFound = false;
            foreach (string field in fieldList)
            {
                var columns = grid.Columns.Where(col => col.FieldName == field);
                if (columns.Count() == 0)
                {
                    isNotFound = true;
                    break;
                }   
            }
            if (isNotFound == true) return null;

            // Check Duplicated
            bool isDuplicated = false;
            foreach (string field in fieldList)
            {
                isDuplicated = false;

                for (int rowCnt = 0; rowCnt < grid.DataRowCount; rowCnt++)
                {
                    string value = grid.GetRowCellValue(rowCnt, field).ToString();

                    for (int i = 0; i < grid.DataRowCount; i++)
                    {
                        if (i != rowCnt)
                        {
                            if (grid.GetRowCellValue(i, field) != null
                                && grid.GetRowCellValue(i, field).ToString() == value)
                            {
                                GridInfoItem infoItem = new GridInfoItem();
                                infoItem.FromRowIndex = rowCnt + 1;
                                infoItem.ToRowIndex = i + 1;
                                infoItem.FieldName = field;
                                infoItem.ColumnCaption = grid.Columns[field].Caption;
                                infoItem.CellValue = value;

                                duplicateInfo.Add(infoItem);

                                isDuplicated = true;
                                break;
                            }
                        }
                    }

                    if (isDuplicated == true)
                    {
                        break;
                    }
                }
            }
            
            return duplicateInfo;
        }

        public static List<GridInfoItem> GetDuplicateMultiColumnRowsInfo(GridView grid, string[] fieldList)
        {
            List<GridInfoItem> duplicateInfo = new List<GridInfoItem>();

            // Check Length
            if (fieldList == null || fieldList.Length == 0) return null;

            // Check Exist
            bool isNotFound = false;
            foreach (string field in fieldList)
            {
                var columns = grid.Columns.Where(col => col.FieldName == field);
                if (columns.Count() == 0)
                {
                    isNotFound = true;
                    break;
                }
            }
            if (isNotFound == true) return null;

            // Check Duplicated
            bool isDuplicated = false;

            string keySource = string.Empty;
            string keyTarget = string.Empty;

            for (int rowCnt = 0; rowCnt < grid.DataRowCount; rowCnt++)
            {
                keySource = string.Empty;
                foreach (string field in fieldList)
                {
                    var columns = grid.Columns.Where(col => col.FieldName == field);
                    if (columns.Count() == 1)
                    {
                        if (string.IsNullOrEmpty(keySource) == false)
                        {
                            keySource = keySource + "/";
                        }

                        keySource = keySource + grid.GetRowCellValue(rowCnt, field).ToString();
                    }
                }

                for (int i = 0; i < grid.DataRowCount; i++)
                {
                    if (i != rowCnt)
                    {
                        keyTarget = string.Empty;
                        foreach (string field in fieldList)
                        {
                            var columns = grid.Columns.Where(col => col.FieldName == field);
                            if (columns.Count() == 1)
                            {
                                if (string.IsNullOrEmpty(keyTarget) == false)
                                {
                                    keyTarget = keyTarget + "/";
                                }

                                keyTarget = keyTarget + grid.GetRowCellValue(i, field).ToString();
                            }
                        }

                        if (keySource == keyTarget)
                        {
                            GridInfoItem infoItem = new GridInfoItem();
                            infoItem.FromRowIndex = rowCnt + 1;
                            infoItem.ToRowIndex = i + 1;
                            infoItem.FieldName = string.Join("/", fieldList);

                            string captionList = string.Empty;
                            foreach (string field in fieldList)
                            {
                                var columns = grid.Columns.Where(col => col.FieldName == field);
                                if (columns.Count() == 1)
                                {
                                    if (string.IsNullOrEmpty(captionList) == false)
                                    {
                                        captionList = captionList + "/";
                                    }

                                    captionList = captionList + grid.Columns[field].Caption;
                                }
                            }

                            infoItem.ColumnCaption = captionList;
                            infoItem.CellValue = keySource;

                            duplicateInfo.Add(infoItem);

                            isDuplicated = true;
                            break;
                        }
                    }
                }

                if (isDuplicated == true)
                {
                    break;
                }
            }

            return duplicateInfo;
        }

        public static int GetRowHandleByBaseItem<T>(GridView gridView, T item)
        {
            // -1 means can't find row index.
            int rowIdx = -1;
            if (gridView == null || item == null) return rowIdx;

            try
            {
                BindingList<T> dataSource = gridView.GridControl.DataSource as BindingList<T>;
                if (dataSource == null || dataSource.Count == 0) return rowIdx;

                // Check BindingList's list index
                int sourceIdx = dataSource.IndexOf(item);

                // Check Is matched object?
                T findItem = (T)gridView.GetRow(sourceIdx);
                if ((item as BaseItem).GUID != (findItem as BaseItem).GUID)
                {
                    // If GUID is not matched, find item by GUID.
                    for (int idx = 0; idx < dataSource.Count; idx++)
                    {
                        T rowItem = (T)gridView.GetRow(idx);
                        if ((item as BaseItem).GUID == (rowItem as BaseItem).GUID)
                        {
                            rowIdx = idx;
                            break;
                        }
                    }
                }
                else
                {
                    // If GUID is matched, return list index
                    rowIdx = sourceIdx;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return rowIdx;
        }

        /// <summary>
        /// BaseItem을 상속받은 클래스로 이루어진 Grid의 변경된 Row리스트를 반환
        /// </summary>
        /// <typeparam name="T">BaseItem의 자식 Class</typeparam>
        /// <param name="gridView">그리드뷰</param>
        /// <param name="iMode">옵셔날, 0(default)이면 변경건만, 1이면 변경+추가, 2이면 추가건만</param>
        /// <param name="exceptRowRock">RowLock걸린 Row를 제외</param>
        /// <returns></returns>
        public static IList<T> GetChangedList<T>(GridView gridView, int iMode = 0, bool exceptRowRock = true) where T : BaseItem
        {
            IList<T> saveItems = null;
            if (gridView == null || gridView.DataSource == null || gridView.RowCount == 0) return null;

            try
            {
                gridView.CloseEditor();
                if (gridView.DataSource as IList<T> == null || (gridView.DataSource as IList<T>).Count == 0) return null;

                IList<T> filterdList = new List<T>();
                for (int rowIdx = 0; rowIdx < gridView.RowCount; rowIdx++)
                {
                    T rowItem = gridView.GetRow(rowIdx) as T;

                    if (exceptRowRock && rowItem.RowLock)
                    {
                        continue;
                    }

                    //if (gridView.IsRowVisible(rowIdx) != RowVisibleState.Hidden)  //이건 스크롤밖의 Row는 생략시키는 코드이므로 주석처리
                    {
                        filterdList.Add(rowItem);
                    }
                }

                if (iMode == 1)
                    saveItems = filterdList.Where(x => x.OpCode == BaseItem.OpCodes.Update || x.OpCode == BaseItem.OpCodes.Create).ToList();
                else if (iMode == 2)
                    saveItems = filterdList.Where(x => x.OpCode == BaseItem.OpCodes.Create).ToList();
                else
                    saveItems = filterdList.Where(x => x.OpCode == BaseItem.OpCodes.Update).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return saveItems;
        }
        #endregion

        #region disposed
        public static void DisposedGrid(GridControl gridControl)
        {
            try
            {
                var gridHelpSpec = GetGridHelpSpecFromDic(gridControl);
                UnBindingEventHandler(gridHelpSpec);
                _gridControlRegistry.Remove(gridControl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion
    }
}
