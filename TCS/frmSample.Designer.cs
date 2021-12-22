
namespace Hmx.DHAKA.TCS
{
    partial class frmSample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSample));
            this.bmgToolBar = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btnInquiry = new DevExpress.XtraBars.BarButtonItem();
            this.btnNew = new DevExpress.XtraBars.BarButtonItem();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.btnRemove = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportPdf = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportExcel = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.btnImport = new DevExpress.XtraBars.BarButtonItem();
            this.btnCheck = new DevExpress.XtraBars.BarButtonItem();
            this.btnUncheck = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bdsSearchParam = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bmgToolBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsSearchParam)).BeginInit();
            this.SuspendLayout();
            // 
            // bmgToolBar
            // 
            this.bmgToolBar.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.bmgToolBar.DockControls.Add(this.barDockControlTop);
            this.bmgToolBar.DockControls.Add(this.barDockControlBottom);
            this.bmgToolBar.DockControls.Add(this.barDockControlLeft);
            this.bmgToolBar.DockControls.Add(this.barDockControlRight);
            this.bmgToolBar.Form = this;
            this.bmgToolBar.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnInquiry,
            this.btnNew,
            this.btnSave,
            this.btnAdd,
            this.btnRemove,
            this.btnExportExcel,
            this.btnExportPdf,
            this.btnPrint,
            this.btnImport,
            this.btnCheck,
            this.btnUncheck});
            this.bmgToolBar.MainMenu = this.bar2;
            this.bmgToolBar.MaxItemId = 11;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnInquiry),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnNew),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAdd),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRemove),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportPdf),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportExcel),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPrint),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnImport),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnCheck, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnUncheck)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btnInquiry
            // 
            this.btnInquiry.Caption = "Inquiry";
            this.btnInquiry.Id = 0;
            this.btnInquiry.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInquiry.ImageOptions.Image")));
            this.btnInquiry.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnInquiry.ImageOptions.LargeImage")));
            this.btnInquiry.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.btnInquiry.Name = "btnInquiry";
            // 
            // btnNew
            // 
            this.btnNew.Caption = "New";
            this.btnNew.Id = 1;
            this.btnNew.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.ImageOptions.Image")));
            this.btnNew.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnNew.ImageOptions.LargeImage")));
            this.btnNew.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.btnNew.Name = "btnNew";
            // 
            // btnSave
            // 
            this.btnSave.Caption = "Save";
            this.btnSave.Id = 2;
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.LargeImage")));
            this.btnSave.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S));
            this.btnSave.Name = "btnSave";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "Add";
            this.btnAdd.Id = 3;
            this.btnAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.ImageOptions.Image")));
            this.btnAdd.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.ImageOptions.LargeImage")));
            this.btnAdd.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4);
            this.btnAdd.Name = "btnAdd";
            // 
            // btnRemove
            // 
            this.btnRemove.Caption = "Remove";
            this.btnRemove.Id = 4;
            this.btnRemove.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.ImageOptions.Image")));
            this.btnRemove.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnRemove.ImageOptions.LargeImage")));
            this.btnRemove.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete));
            this.btnRemove.Name = "btnRemove";
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Caption = "Export PDF";
            this.btnExportPdf.Id = 6;
            this.btnExportPdf.ImageOptions.Image = global::Hmx.DHAKA.TCS.Properties.Resources.exporttopdf_16x16;
            this.btnExportPdf.ImageOptions.LargeImage = global::Hmx.DHAKA.TCS.Properties.Resources.exporttopdf_32x32;
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Caption = "Export Excel";
            this.btnExportExcel.Id = 5;
            this.btnExportExcel.ImageOptions.Image = global::Hmx.DHAKA.TCS.Properties.Resources.exporttoxls_16x16;
            this.btnExportExcel.ImageOptions.LargeImage = global::Hmx.DHAKA.TCS.Properties.Resources.exporttoxls_32x32;
            this.btnExportExcel.Name = "btnExportExcel";
            // 
            // btnPrint
            // 
            this.btnPrint.Caption = "Print";
            this.btnPrint.Id = 7;
            this.btnPrint.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.ImageOptions.Image")));
            this.btnPrint.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnPrint.ImageOptions.LargeImage")));
            this.btnPrint.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // btnImport
            // 
            this.btnImport.Caption = "Import";
            this.btnImport.Id = 8;
            this.btnImport.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.ImageOptions.Image")));
            this.btnImport.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnImport.ImageOptions.LargeImage")));
            this.btnImport.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.btnImport.Name = "btnImport";
            this.btnImport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // btnCheck
            // 
            this.btnCheck.Caption = "Check";
            this.btnCheck.Id = 9;
            this.btnCheck.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCheck.ImageOptions.Image")));
            this.btnCheck.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCheck.ImageOptions.LargeImage")));
            this.btnCheck.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.C));
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // btnUncheck
            // 
            this.btnUncheck.Caption = "Uncheck";
            this.btnUncheck.Id = 10;
            this.btnUncheck.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUncheck.ImageOptions.Image")));
            this.btnUncheck.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUncheck.ImageOptions.LargeImage")));
            this.btnUncheck.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.D));
            this.btnUncheck.Name = "btnUncheck";
            this.btnUncheck.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.bmgToolBar;
            this.barDockControlTop.Size = new System.Drawing.Size(1425, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 740);
            this.barDockControlBottom.Manager = this.bmgToolBar;
            this.barDockControlBottom.Size = new System.Drawing.Size(1425, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.bmgToolBar;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 716);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1425, 24);
            this.barDockControlRight.Manager = this.bmgToolBar;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 716);
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(537, 195);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(860, 263);
            this.gridControl.TabIndex = 28;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            // 
            // frmSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 740);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmSample";
            this.Text = "frmSample";
            ((System.ComponentModel.ISupportInitialize)(this.bmgToolBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsSearchParam)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager bmgToolBar;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btnInquiry;
        private DevExpress.XtraBars.BarButtonItem btnNew;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraBars.BarButtonItem btnRemove;
        private DevExpress.XtraBars.BarButtonItem btnExportPdf;
        private DevExpress.XtraBars.BarButtonItem btnExportExcel;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
        private DevExpress.XtraBars.BarButtonItem btnImport;
        private DevExpress.XtraBars.BarButtonItem btnCheck;
        private DevExpress.XtraBars.BarButtonItem btnUncheck;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl gridControl;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private System.Windows.Forms.BindingSource bdsSearchParam;
    }
}