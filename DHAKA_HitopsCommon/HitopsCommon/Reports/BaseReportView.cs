using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using Hitops.exception;
using HitopsCommon.FormTemplate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitopsCommon.Reports
{
    public partial class BaseReportView : BaseView 
    {
        #region FIELD AREA *******************************
        #endregion

        #region PROPERTY AREA ****************************
        public BaseReportParam ReportParam { get; set; }
        #endregion

        #region INITIALIZE AREA **************************
        public BaseReportView()
        {
            this.InitializeComponent();
            this.Load += new EventHandler(this.BaseReportView_Load);
        }

        public BaseReportView(BaseReportParam param)
        {
            this.InitializeComponent();
            this.ReportParam = param;

            this.Load += new EventHandler(this.BaseReportView_Load);
        }
        #endregion

        #region EVENT AREA *******************************
        public void BaseReportView_Load(object sender, EventArgs e)
        {
            if (this.ReportParam != null)
            {
                this.DoMakeReport();
            }
        }
        #endregion
         
        #region METHOD AREA ******************************
        private void AddEventHandler()
        {

        }
        public void SetChartInfo()
        {
            #region Chart Viewer
            CrystalReportViewer viewer = this.crystalReportViewer; 

            viewer.ShowCloseButton = false;
            viewer.ShowGotoPageButton = false;
            viewer.ShowGroupTreeButton = false;
            viewer.ShowLogo = false;
            viewer.ShowPageNavigateButtons = false;
            viewer.ShowTextSearchButton = false;
            viewer.ShowParameterPanelButton = false;
            viewer.ShowCopyButton = false;

            viewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            #endregion
            #region Form
            this.Width = 1246;
            this.Height = 1033;
            #endregion
        }
        public void SetReportInfo(BaseReportParam reportParam)
        {
            this.ReportParam = reportParam;
            if (string.IsNullOrEmpty(this.ReportParam.ReportName) == false)
            {
                this.Text = this.ReportParam.ReportName + "-" + this.Text;
            }
        }

        private void DoMakeReport()
        {
            try
            {
                CrystalReportViewer viewer = this.crystalReportViewer;
                viewer.ReportSource = null;

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(BaseReportParam.GetNameWithPath(ReportParam.ReportFileName));
                reportDocument.SetDataSource(ReportParam.ReportDataSet);
                
                if (this.ReportParam.ReportParameters != null && this.ReportParam.ReportParameters.Keys.Count > 0)
                {
                    viewer.ReuseParameterValuesOnRefresh = true;

                    foreach (string key in this.ReportParam.ReportParameters.Keys)
                    {
                        reportDocument.SetParameterValue(key, this.ReportParam.ReportParameters[key]);
                    }
                }

                if (ReportParam.SubreportNames != null && ReportParam.SubreportNames.Count > 0)
                {
                    foreach (string subReportName in this.ReportParam.SubreportNames)
                    {
                        ReportDocument subReport = reportDocument.OpenSubreport(subReportName);
                        //subReport.Load(subReportName);
                        subReport.SetDataSource(this.ReportParam.SubreportDataSets[subReportName]);
                    }
                }

                viewer.ReportSource = reportDocument;
                viewer.RefreshReport();
            }
            catch (Exception ex)
            {
                if (ex is HMMException) CommFunc.ShowExceptionBox(ex as HMMException);
                else MessageManager.Show(ex);

            }
        }

        public void SetTitle(string title)
        {
            this.Text = title;
        }
        #endregion
    }
}
