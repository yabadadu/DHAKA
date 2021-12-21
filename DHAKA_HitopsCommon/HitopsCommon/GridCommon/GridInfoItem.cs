namespace HitopsCommon.GridCommon
{
    public class GridInfoItem
    {
        #region INITIALIZE
        public GridInfoItem()
        {
            
        }
        #endregion

        #region FIELDS
        private int _fromRowIndex;
        private int _toRowIndex;
        private string _fieldName;
        private string _columnCaption;
        private string _cellValue;
        #endregion

        #region PROPERTIES
        public int FromRowIndex
        {
            get { return this._fromRowIndex; }
            set { this._fromRowIndex = value; }
        }

        public int ToRowIndex
        {
            get { return this._toRowIndex; }
            set { this._toRowIndex = value; }
        }

        public string FieldName
        {
            get { return this._fieldName; }
            set { this._fieldName = value; }
        }

        public string ColumnCaption
        {
            get { return this._columnCaption; }
            set { this._columnCaption = value; }
        }

        public string CellValue
        {
            get { return this._cellValue; }
            set { this._cellValue = value; }
        }
        #endregion
    }
}
