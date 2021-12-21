using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace HitopsCommon.FormTemplate
{
    class BaseGridCommon
    {
        #region fields
        private static readonly string ROW_LOCK = "ROW_LOCK";
        private static readonly string CELL_LOCK = "CELL_LOCK";

        private static readonly Color BACKCOLOR_EDIT_CELL = Color.LightCyan;
        private static readonly Color BACKCOLOR_LOOKUP_CELL = Color.FromArgb(255, 215, 249, 199);
        private static readonly Color BACKCOLOR_LOCK_CELL = Color.White;

        private static readonly int _indicatorDefaultWidth = 33;
        private static readonly int _indicatorAddWidth = 6;
        #endregion

        public static string RowLockColumnName
        {
            get { return ROW_LOCK; }
        }

        public static string CellLockColumnName
        {
            get { return CELL_LOCK; }
        }
        
        public static Color getBackColorEdit
        {
            get { return BACKCOLOR_EDIT_CELL; }
        }

        public static Color getBackColorLookup
        {
            get { return BACKCOLOR_LOOKUP_CELL; }
        }

        public static Color getBackColorLock
        {
            get { return BACKCOLOR_LOCK_CELL; }
        }

        public static int getIndicatorDefaultWidth
        {
            get { return _indicatorDefaultWidth; }
        }

        public static int getIndicatorAddWidth
        {
            get { return _indicatorAddWidth; }
        }

        public static string[] GetColumns(GridView gridView)
        {
            string[] columnNames = null;
            columnNames = new string[gridView.Columns.Count];
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                columnNames[i] = gridView.Columns[i].FieldName;
            }
            return columnNames;
        }

        public static int GetColumnIndex(string columnName, string[] columnArray)
        {
            int returnIdx = -1;
            for (int i = 0; i < columnArray.Length; i++)
            {
                if (columnArray[i] == columnName)
                {
                    returnIdx = i;
                    break;
                }
            }
            return returnIdx;
        }

        public static string GetColumnName(int columnIndex, string[] columnArray)
        {
            if (columnIndex > columnArray.Length - 1) return null;
            return columnArray[columnIndex];
        }

        public static void CopyPasteRangeClipboard(GridView gridView)
        {
            try
            {
                IDataObject obj = Clipboard.GetDataObject();

                string[] columnNames = GetColumns(gridView);

                int startRow = gridView.FocusedRowHandle;
                int startCol = GetColumnIndex(gridView.FocusedColumn.FieldName, columnNames);

                if (obj.GetDataPresent(DataFormats.Text) == true)
                {
                    string clipboard = (string)obj.GetData(DataFormats.Text);
                    string[] line = clipboard.Replace(Environment.NewLine, "\n").Split('\n');

                    string[] oneLineSample = line[0].Split('\t');

                    int endRow = Math.Min(startRow + line.Length, gridView.RowCount);
                    int endCol = Math.Min(startCol + oneLineSample.Length, gridView.Columns.Count);

                    int cntRow = endRow - startRow;
                    int cntCol = endCol - startCol;

                    for (int y = 0; y < cntRow; y++)
                    {
                        string[] oneLine = line[y].Split('\t');
                        if (oneLine.Length < cntCol) break;

                        for (int x = 0; x < cntCol; x++)
                        {
                            try
                            {
                                GridColumn column = gridView.Columns[GetColumnName(x + startCol, columnNames)];
                                string value = string.Empty;

                                if (column.ColumnEdit.GetType() == typeof(RepositoryItemLookUpEdit))
                                {
                                    // GetKeyValueByDisplayText 데이터 많을때 찾는데 시간이 너무 오래 걸림
                                    // Ctrl+C 에서 임의로 Lookup 에 대해서 Value 값을 Clipboard Set 하도록 수정함
                                    // RepositoryItemLookUpEdit 의 key value 찾는 부분 주석처리. value 값 그대로 붙여넣기되도록함

                                    //RepositoryItemLookUpEdit repLookUp = column.ColumnEdit as RepositoryItemLookUpEdit;
                                    //object lookUpValue = repLookUp.GetKeyValueByDisplayText(oneLine[x]);
                                    //if (lookUpValue != null) value = lookUpValue.ToString();

                                    value = oneLine[x].Trim();
                                }
                                else
                                {
                                    value = oneLine[x].Trim();
                                }

                                // Check CellLock
                                bool isLock = BaseGridCommon.IsRowCellLock(gridView, y + startRow, GetColumnName(x + startCol, columnNames));
                                if (isLock == false)
                                {
                                    bool isNumeric = false;
                                    if (column.ColumnEditName.Contains("RepositoryItemTextEdit") == true)
                                    {
                                        RepositoryItemTextEdit rep = column.ColumnEdit as RepositoryItemTextEdit;
                                        if (rep.Mask.MaskType == DevExpress.XtraEditors.Mask.MaskType.Numeric)
                                        {
                                            isNumeric = true;
                                        }
                                    }

                                    if (isNumeric == true)
                                    {
                                        decimal? numericValue = null;

                                        decimal outNumeric = 0;
                                        bool isNumber = decimal.TryParse(value.Replace(",", ""), out outNumeric);
                                        if (isNumber == true)
                                        {
                                            numericValue = outNumeric;
                                        }

                                        gridView.SetRowCellValue(y + startRow, GetColumnName(x + startCol, columnNames), numericValue);
                                    }
                                    else
                                    {
                                        gridView.SetRowCellValue(y + startRow, GetColumnName(x + startCol, columnNames), value);
                                    }
                                }
                            }
                            catch
                            { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static bool IsRowLock(GridView view, int rowIndex)
        {
            object rowObj = view.GetRow(rowIndex);
            if (rowObj is BaseItem
                && (rowObj as BaseItem).RowLock == true)
            {
                return true;
            }

            var columns = view.Columns.Where(col => col.FieldName == BaseGridCommon.RowLockColumnName);
            if (columns.Count() > 0)
            {
                string cellValue = view.GetRowCellValue(rowIndex, BaseGridCommon.RowLockColumnName).ToString();
                if (cellValue == "Y")
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsRowCellLock(GridView view, int rowIndex, string fieldName)
        {
            // Check Grid 
            if (view.OptionsBehavior.Editable == false
                || view.OptionsBehavior.ReadOnly == true)
            {
                return true;
            }

            // Check Grid Column
            GridColumn column = view.Columns[fieldName];
            if (column != null && column.OptionsColumn.AllowEdit == false)
            {
                return true;
            }

            // Check RowLock
            if (IsRowLock(view, rowIndex) == true)
            {
                return true;
            }

            string cellValue = string.Empty;
            object rowObj = view.GetRow(rowIndex);

            if (rowObj is BaseItem)
            {
                cellValue = (rowObj as BaseItem).CellLockFieldList;
            }
            else
            {
                var columns = view.Columns.Where(col => col.FieldName == BaseGridCommon.CellLockColumnName);
                if (columns.Count() > 0)
                {
                    cellValue = view.GetRowCellValue(rowIndex, BaseGridCommon.CellLockColumnName).ToString();
                }
            }

            List<string> fieldList = new List<string>();

            if (string.IsNullOrEmpty(cellValue) == false)
            {
                fieldList = cellValue.Split(',').ToList();
                if (fieldList.IndexOf(fieldName) > -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
