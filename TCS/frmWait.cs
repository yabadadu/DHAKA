#region
using System;
using DevExpress.XtraWaitForm;
#endregion

namespace Hmx.Skit.SunkwangApp
{
    public partial class frmWait : WaitForm
    {
        public frmWait()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }
        #endregion
    }
}