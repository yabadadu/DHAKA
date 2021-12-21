#region

using System.Collections;

#endregion

namespace HitopsCommon
{
    public class GridHelpSpecInfo
    {

        #region fields
        private bool _isMouseDragOn = false;
        private int _prevRowIdx = -1;
        private ArrayList _treatedRows = new ArrayList();

        private int _lazySelStartRowHandle = -1;
        private int _lazySelEndRowHanle = -1;

        private int _minRowRange = -1;
        private int _maxRowRange = -1;

        private bool _scrollingOn = false;
        #endregion

        #region properties

        public bool IsMouseDragOn
        {
            get { return _isMouseDragOn; }
            set { _isMouseDragOn = value; }
        }

        public int PrevRowIdx
        {
            get { return _prevRowIdx; }
            set { _prevRowIdx = value; }
        }

        public ArrayList TreatedRows
        {
            get { return _treatedRows; }
            set { _treatedRows = value; }
        }

        public bool ScrollingOn
        {
            get { return _scrollingOn; }
            set { _scrollingOn = value; }
        }

        public int LazySelStartRowHandle
        {
            get { return _lazySelStartRowHandle; }
            set
            {
                _lazySelStartRowHandle = value;

                //update min and row
                MinRowRange = value;
                MaxRowRange = value;
            }
        }

        public int LazySelEndRowHanle
        {
            get { return _lazySelEndRowHanle; }
            set
            {
                _lazySelEndRowHanle = value;

                MinRowRange = MinRowRange > value ? value : MinRowRange;
                MaxRowRange = MaxRowRange < value ? value : MaxRowRange;
            }
        }

        public int MinRowRange
        {
            get { return _minRowRange; }
            set { _minRowRange = value; }
        }

        public int MaxRowRange
        {
            get { return _maxRowRange; }
            set { _maxRowRange = value; }
        }

        #endregion

    }
}
