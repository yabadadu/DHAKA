#region

using System;
using System.Drawing;
using System.Windows.Forms;
using Hitops.exception;
using HitopsCommon.Logger;

#endregion

namespace HitopsCommon
{
    public partial class frmExMessageBox : DevExpress.XtraEditors.XtraForm
    {
        public frmExMessageBox()
        {
            InitializeComponent();
        }

        public void setData(HMMException e)
        {
            String strTitle;
            String strMessage1;
            String strMessage2;

            if (e.Result == null)
                strTitle = "EXCEPTION (CODE-1000)";
            else
                strTitle = "EXCEPTION (CODE-" + e.Result + ")"; ;

            Text = strTitle;

            if (e.Message2 == null)
                strMessage1 = "SYSTEM ERROR.";
            else
                strMessage1 = e.Message2;

            if (strMessage1.Length > lblMsg.MaxLength)
                strMessage1 = strMessage1.Substring(0, 30) + "\n" + strMessage1.Substring(30, lblMsg.MaxLength - 30);

            lblMsg.Text = strMessage1;

            if (e.Message1 == null)
                strMessage2 = "Detail information not exists.";
            else
                strMessage2 = e.Message1;

            if (strMessage2.Length > tbxDetail.MaxLength)
                strMessage2 = strMessage1.Substring(0, tbxDetail.MaxLength);

            tbxDetail.Text = strMessage2;
            //String [] token = strMessage2.Split('\n');

            //foreach (String str in token)
            //{
            //    tbxDetail.AppendText(str + Environment.NewLine);   
            //}


            int lGap = 0;

            lGap = (this.Width - lblMsg.Width) / 2;

            lblMsg.Left = lGap > 0 ? lGap : 0;

            BaseLogger.Error(e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (tbxDetail.Visible)
            {
                label2.Visible = false;
                label1.Image = HitopsCommon.Properties.Resources.Standard_06;
                tbxDetail.Visible = false;
                this.Height -= 120;
            }
            else
            {
                label2.Visible = true;
                label1.Image = HitopsCommon.Properties.Resources.Standard_05;
                label2.Image = HitopsCommon.Properties.Resources.Standard_16;
                tbxDetail.Visible = true;
                this.Height += 120;
            }
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Blue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            label2.Image = HitopsCommon.Properties.Resources.Standard_08;
            Clipboard.SetText(tbxDetail.Text);
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Blue;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }
    }
}