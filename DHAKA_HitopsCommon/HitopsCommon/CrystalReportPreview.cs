using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace HitopsCommon
{
    public partial class frmCrystalReportPreview : Form
    {
        public CrystalReportViewer crystalReportPreview = new CrystalReportViewer();

        public frmCrystalReportPreview()
        {
            InitializeComponent();
        }
        
        private void frmCrystalReportPreview_Load(object sender, EventArgs e)
        {
            crystalReportPreview.ActiveViewIndex = -1;
            crystalReportPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            crystalReportPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            crystalReportPreview.Location = new System.Drawing.Point(0, 0);
            crystalReportPreview.SelectionFormula = "";
            crystalReportPreview.TabIndex = 0;
            crystalReportPreview.ViewTimeSelectionFormula = "";
            Controls.Add(crystalReportPreview);
        }
    }
}