namespace Hitops3Login
{
    partial class frmEnvironment
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.radDemo = new System.Windows.Forms.RadioButton();
            this.radReal = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Hitops3Login.Properties.Resources.background;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radDemo);
            this.panel1.Controls.Add(this.radReal);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(329, 105);
            this.panel1.TabIndex = 3;
            // 
            // radDemo
            // 
            this.radDemo.AutoSize = true;
            this.radDemo.ForeColor = System.Drawing.Color.White;
            this.radDemo.Location = new System.Drawing.Point(182, 42);
            this.radDemo.Name = "radDemo";
            this.radDemo.Size = new System.Drawing.Size(98, 18);
            this.radDemo.TabIndex = 19;
            this.radDemo.Text = "Development";
            this.radDemo.UseVisualStyleBackColor = true;
            this.radDemo.CheckedChanged += new System.EventHandler(this.radDemo_CheckedChanged);
            // 
            // radReal
            // 
            this.radReal.AutoSize = true;
            this.radReal.ForeColor = System.Drawing.Color.White;
            this.radReal.Location = new System.Drawing.Point(38, 42);
            this.radReal.Name = "radReal";
            this.radReal.Size = new System.Drawing.Size(84, 18);
            this.radReal.TabIndex = 18;
            this.radReal.Text = "Production";
            this.radReal.UseVisualStyleBackColor = true;
            this.radReal.CheckedChanged += new System.EventHandler(this.radReal_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Hitops3Login.Properties.Resources.btn_cancel;
            this.btnCancel.Location = new System.Drawing.Point(162, 67);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 28);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Image = global::Hitops3Login.Properties.Resources.btn_ok;
            this.btnOK.Location = new System.Drawing.Point(87, 67);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(69, 28);
            this.btnOK.TabIndex = 16;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.AutoSize = true;
            this.panelControl1.Controls.Add(this.panel1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(333, 109);
            this.panelControl1.TabIndex = 4;
            // 
            // frmEnvironment
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(333, 109);
            this.Controls.Add(this.panelControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmEnvironment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Silver;
            this.Load += new System.EventHandler(this.frmEnvironment_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label btnOK;
        private System.Windows.Forms.Label btnCancel;
        private System.Windows.Forms.RadioButton radDemo;
        private System.Windows.Forms.RadioButton radReal;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}