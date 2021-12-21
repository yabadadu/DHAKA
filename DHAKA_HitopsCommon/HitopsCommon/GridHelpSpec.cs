
#region

using System.Windows.Forms;
using DevExpress.XtraGrid;

#endregion

namespace HitopsCommon
{
    public class GridHelpSpec
    {
        #region fields
        private GridControl _gridControl = null;
        private Timer _gridScrolltimer = new Timer();

        private GridHelpSpecCfg _gridHelpSpecCfg = new GridHelpSpecCfg();
        private GridHelpSpecInfo _gridHelpSpecInfo = new GridHelpSpecInfo();

        #endregion

        #region properties
        public GridHelpSpecCfg GridHelpSpecCfg
        {
            get { return _gridHelpSpecCfg; }
            set { _gridHelpSpecCfg = value; }
        }

        public GridHelpSpecInfo GridHelpSpecInfo
        {
            get { return _gridHelpSpecInfo; }
            set { _gridHelpSpecInfo = value; }
        }
        public GridControl GridControl
        {
            get { return _gridControl; }
            set { _gridControl = value; }
        }

        public Timer GridScrolltimer
        {
            get { return _gridScrolltimer; }
            set { _gridScrolltimer = value; }
        }

        #endregion

        #region initialize
        public GridHelpSpec(GridControl gridControl)
        {
            _gridControl = gridControl;
        }
        #endregion
    }
}
