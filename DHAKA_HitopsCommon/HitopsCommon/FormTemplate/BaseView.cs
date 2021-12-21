using DevExpress.XtraEditors;
using HitopsCommon.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitopsCommon.FormTemplate
{
    public partial class BaseView : XtraForm
    {
        #region FIELD & CONST AREA *********************
        #endregion

        #region PROPERTY AREA **************************
        #endregion

        #region INITIALIZE AREA ************************
        public BaseView() : base()
        {
            InitializeComponent();
        }
        #endregion

        #region METHOD AREA ****************************
        protected void ShowPreviewReport(BaseReportParam reportParam, bool isChart=false)
        {
            if (reportParam == null) return;

            ReportManager reportMng = new ReportManager(this);
            reportMng.Show(reportParam, isChart);
        }
        #endregion

    }
}
