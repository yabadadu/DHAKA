using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;
using HitopsCommon.FormTemplate;
using System.Text.RegularExpressions;

namespace HitopsCommon.GridCommon
{
    public class DragCellsValuesHelper
    {

        private SelectedCellsBorderHelper _Helper;

        public GridView View
        {
            get { return this._Helper.GridView; }
        }

        private GridCell _SourceGridCell;
        public GridCell SourceGridCell
        {
            get { return _SourceGridCell; }
            set { _SourceGridCell = value; }
        }
        public DragCellsValuesHelper(SelectedCellsBorderHelper selectedCellsHelper)
        {
            this._Helper = selectedCellsHelper;
            InitViewEvents();
        }

        private void InitViewEvents()
        {
            View.MouseDown += new MouseEventHandler(View_MouseDown);
            View.MouseUp += new MouseEventHandler(View_MouseUp);
            View.ShowingEditor += new CancelEventHandler(View_ShowingEditor);
            View.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(View_SelectionChanged);
        }

        void View_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (this._Helper.IsCopyMode)
            {
                UpdateHint();
            }
        }

        private void UpdateHint()
        {
            ToolTipController.DefaultController.HideHint();
            
            string text = string.Empty;

            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                text = GetIncrementDisplayText();
            }
            else
            {
                text = View.GetFocusedDisplayText();
            }
            
            if (string.IsNullOrEmpty(text)) return;

            ToolTipController.DefaultController.ShowHint(text);
        }

        string GetIncrementDisplayText()
        {
            string orgValue = View.GetFocusedDisplayText();
            if (string.IsNullOrEmpty(orgValue) == true)
            {
                return orgValue;
            }

            // find Number Only(Right Side)
            string getNumberOnly = Regex.Match(orgValue, @"\d+$").Value;
            if (string.IsNullOrEmpty(getNumberOnly) == true)
            {
                return orgValue;
            }

            // Check Number
            int outInt = 0;
            bool isNumber = int.TryParse(getNumberOnly, out outInt);
            if (isNumber == false)
            {
                return orgValue;
            }

            // find String Only
            int stringLen = orgValue.Length - getNumberOnly.Length;
            string getStringOnly = orgValue.Substring(0, stringLen);

            // Increment Values
            int i = (View.GetSelectedCells().Length - 1);
            string incrementValue = getStringOnly + (int.Parse(getNumberOnly) + i).ToString();
            
            return incrementValue;
        }

        void View_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = this._Helper.IsCopyMode;
        }

        void View_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._Helper.IsCopyMode)
            {
                OnCopyFinished();
            }
                
            this._Helper.IsCopyMode = false;
        }

        private void OnCopyFinished()
        {
            ToolTipController.DefaultController.HideHint();

            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                // Increment
                CopyAndIncrementCellsValues();
            }
            else
            {
                // Copy
                CopyCellsValues();
            }
        }


        private void CopyCellsValues()
        {
            object value = View.GetRowCellValue(SourceGridCell.RowHandle, SourceGridCell.Column);
            GridCell[] selectedCells = View.GetSelectedCells();
            foreach (GridCell cell in selectedCells)
            {
                // Check Cell Lock
                bool isCellLock = BaseGridCommon.IsRowCellLock(View, cell.RowHandle, cell.Column.Name);

                if (isCellLock == false)
                {
                    // Set Value
                    View.SetRowCellValue(cell.RowHandle, cell.Column, value);
                }
            }
        }

        private void CopyAndIncrementCellsValues()
        {
            string orgValue = View.GetRowCellValue(SourceGridCell.RowHandle, SourceGridCell.Column).ToString();

            if (string.IsNullOrEmpty(orgValue) == true)
            {
                // GoTo Copy
                CopyCellsValues();
                return;
            }

            // find Number Only(Right Side)
            string getNumberOnly = Regex.Match(orgValue, @"\d+$").Value;

            if (string.IsNullOrEmpty(getNumberOnly) == true)
            {
                // GoTo Copy
                CopyCellsValues();
                return;
            }

            // Check Number
            int outInt = 0;
            bool isNumber = int.TryParse(getNumberOnly, out outInt);

            if (isNumber == false)
            {
                // GoTo Copy
                CopyCellsValues();
                return;
            }

            // find String Only
            int stringLen = orgValue.Length - getNumberOnly.Length;
            string getStringOnly = orgValue.Substring(0, stringLen);
            
            // Increment Values
            GridCell[] selectedCells = View.GetSelectedCells();

            int i = 0;
            foreach (GridCell cell in selectedCells)
            {
                // Check Cell Lock
                bool isCellLock = BaseGridCommon.IsRowCellLock(View, cell.RowHandle, cell.Column.Name);

                if (isCellLock == false)
                {
                    // Set Value
                    string incrementValue = getStringOnly + (int.Parse(getNumberOnly) + i).ToString();
                    View.SetRowCellValue(cell.RowHandle, cell.Column, incrementValue);
                    i++;
                }
            }
        }

        void View_MouseDown(object sender, MouseEventArgs e)
        {
            this._Helper.IsCopyMode = this._Helper.GetDragRect().Contains(e.Location);
            if (this._Helper.IsCopyMode)
            {
                OnStartCopy();
            }
        }

        private void OnStartCopy()
        {
            SourceGridCell = new GridCell(View.FocusedRowHandle, View.FocusedColumn);
        }
    }
}
