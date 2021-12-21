#region

using System;
using DevExpress.XtraGrid.Views.Grid;

#endregion

namespace HitopsCommon
{
    public class GridHelpSpecCfg : ICloneable
    {
        #region fields
        private bool _rowIndicatorVisible = false;

        private bool _clipboardAllowCopy = false;
        private bool _clipboardCopyColumnHearders = false;
        private bool _clipboardCopyCollapsedData = false;

        private bool _multiSelection = false;
        private GridMultiSelectMode _multiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;

        private bool _multiSelectionAutoReverse = false;
        private bool _multiSelectionAutoReverseLazy = false;

        private bool _resetSelectionClickOutsideCheckboxSelector = false;
        #endregion

        #region properties
        public bool RowIndicatorVisible
        {
            get { return _rowIndicatorVisible; }
            set { _rowIndicatorVisible = value; }
        }

        public bool ClipboardAllowCopy
        {
            get { return _clipboardAllowCopy; }
            set { _clipboardAllowCopy = value; }
        }

        public bool ClipboardCopyColumnHearders
        {
            get { return _clipboardCopyColumnHearders; }
            set { _clipboardCopyColumnHearders = value; }
        }

        public bool ClipboardCopyCollapsedData
        {
            get { return _clipboardCopyCollapsedData; }
            set { _clipboardCopyCollapsedData = value; }
        }

        public bool MultiSelection
        {
            get { return _multiSelection; }
            set { _multiSelection = value; }
        }

        public GridMultiSelectMode MultiSelectMode
        {
            get { return _multiSelectMode; }
            set { _multiSelectMode = value; }
        }

        public bool MultiSelectionAutoReverse
        {
            get { return _multiSelectionAutoReverse; }
            set { _multiSelectionAutoReverse = value; }
        }

        public bool MultiSelectionAutoReverseLazy
        {
            get { return _multiSelectionAutoReverseLazy; }
            set { _multiSelectionAutoReverseLazy = value; }
        }

        public bool ResetSelectionClickOutsideCheckboxSelector
        {
            get { return _resetSelectionClickOutsideCheckboxSelector; }
            set { _resetSelectionClickOutsideCheckboxSelector = value; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

    }
}
