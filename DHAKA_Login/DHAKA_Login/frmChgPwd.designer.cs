namespace Hitops3Main
{
    partial class frmChgPwd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChgPwd));
            this.label1 = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.tbxCfm = new System.Windows.Forms.TextBox();
            this.tbxNewPwd = new System.Windows.Forms.TextBox();
            this.tsMng = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbIcon = new System.Windows.Forms.ToolStripButton();
            this.tbbText = new System.Windows.Forms.ToolStripButton();
            this.lblMsg = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tsMng.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Teal;
            this.label1.Location = new System.Drawing.Point(58, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 14);
            this.label1.TabIndex = 11;
            this.label1.Text = "Confirm";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPwd.ForeColor = System.Drawing.Color.Teal;
            this.lblPwd.Location = new System.Drawing.Point(19, 90);
            this.lblPwd.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(87, 14);
            this.lblPwd.TabIndex = 12;
            this.lblPwd.Text = "New Password";
            // 
            // tbxCfm
            // 
            this.tbxCfm.BackColor = System.Drawing.Color.LightBlue;
            this.tbxCfm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxCfm.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxCfm.Location = new System.Drawing.Point(111, 113);
            this.tbxCfm.MaxLength = 20;
            this.tbxCfm.Name = "tbxCfm";
            this.tbxCfm.Size = new System.Drawing.Size(100, 22);
            this.tbxCfm.TabIndex = 10;
            this.tbxCfm.UseSystemPasswordChar = true;
            // 
            // tbxNewPwd
            // 
            this.tbxNewPwd.BackColor = System.Drawing.Color.LightBlue;
            this.tbxNewPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxNewPwd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxNewPwd.Location = new System.Drawing.Point(111, 86);
            this.tbxNewPwd.MaxLength = 20;
            this.tbxNewPwd.Name = "tbxNewPwd";
            this.tbxNewPwd.Size = new System.Drawing.Size(100, 22);
            this.tbxNewPwd.TabIndex = 9;
            this.tbxNewPwd.UseSystemPasswordChar = true;
            // 
            // tsMng
            // 
            this.tsMng.BackgroundImage = global::Hitops3Login.Properties.Resources.Top_BG;
            this.tsMng.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsMng.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMng.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.tbbIcon,
            this.tbbText});
            this.tsMng.Location = new System.Drawing.Point(0, 0);
            this.tsMng.Name = "tsMng";
            this.tsMng.Size = new System.Drawing.Size(230, 25);
            this.tsMng.TabIndex = 13;
            this.tsMng.Text = "toolStrip1";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::Hitops3Login.Properties.Resources.Standard_04;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tbbIcon
            // 
            this.tbbIcon.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbbIcon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbIcon.Image = global::Hitops3Login.Properties.Resources.Standard_29;
            this.tbbIcon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbIcon.Name = "tbbIcon";
            this.tbbIcon.Size = new System.Drawing.Size(23, 22);
            this.tbbIcon.Text = "Icon Mode";
            this.tbbIcon.Visible = false;
            // 
            // tbbText
            // 
            this.tbbText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbbText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbText.Image = global::Hitops3Login.Properties.Resources.Standard_28;
            this.tbbText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbText.Name = "tbbText";
            this.tbbText.Size = new System.Drawing.Size(23, 22);
            this.tbbText.Text = "Text View Mode";
            // 
            // lblMsg
            // 
            this.lblMsg.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(15, 140);
            this.lblMsg.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(200, 47);
            this.lblMsg.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(7, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "① 8자리 이상 ";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(7, 51);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(216, 32);
            this.label3.TabIndex = 12;
            this.label3.Text = "② 알파벳(a-z, A-Z)/숫자(0-9)/기호 !@#$%^&*()_ 가 반드시 포함";
            // 
            // frmChgPwd
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(230, 193);
            this.Controls.Add(this.tsMng);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.tbxCfm);
            this.Controls.Add(this.tbxNewPwd);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChgPwd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Password";
            this.tsMng.ResumeLayout(false);
            this.tsMng.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.TextBox tbxCfm;
        private System.Windows.Forms.TextBox tbxNewPwd;
        private System.Windows.Forms.ToolStrip tsMng;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tbbIcon;
        private System.Windows.Forms.ToolStripButton tbbText;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}