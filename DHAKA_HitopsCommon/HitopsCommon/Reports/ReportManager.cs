using Hitops.exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitopsCommon.Reports
{
    public class ReportManager
    {
        #region FIELD AREA **********************
        private Form _owner;
        private BaseReportView _reportView;
        #endregion

        #region INITIALIZE AREA *****************
        public ReportManager(Form owner)
        {
            this._owner = owner;
        }
        #endregion

        #region METHOD AREA *********************
        public void Show(BaseReportParam reportParam, bool isChart=false)
        {
            try
            {
                BaseReportView reportView = this.GetReportView();
                reportView.SetReportInfo(reportParam);
                if (isChart)
                {
                    reportView.SetChartInfo();
                }
                reportView.ShowDialog(_owner.MdiParent);
            }
            catch (Exception ex)
            {
                if (ex is HMMException) CommFunc.ShowExceptionBox(ex as HMMException);
                else MessageManager.Show(ex);
            }
        }
        private BaseReportView GetReportView()
        {
            if (_reportView == null || _reportView.IsDisposed)
            {
                _reportView = new BaseReportView();
                _reportView.ShowInTaskbar = false;
                if (_owner != null)
                {
                    _reportView.SetTitle(_owner.Text);
                }

                _reportView.Disposed += new EventHandler(_reportView_Disposed);

                if (_owner != null)
                {
                    _owner.Disposed -= new EventHandler(_owner_Disposed);
                    _owner.Disposed += new EventHandler(_owner_Disposed);
                }
            }

            return _reportView;
        }


        #endregion

        #region DELIGATE AREA *********************
        private void _reportView_Disposed(object sender, EventArgs e)
        {
            _reportView = null;
            if (_owner != null)
            {
                _owner.Disposed -= new EventHandler(_owner_Disposed);
            }
        }

        private void _owner_Disposed(object sender, EventArgs e)
        {
            if (_reportView != null)
            {
                _reportView.Close();
            }

            if (_owner != null)
            {
                _owner.Disposed -= new EventHandler(_owner_Disposed);
            }
        }
        #endregion
    }
}
