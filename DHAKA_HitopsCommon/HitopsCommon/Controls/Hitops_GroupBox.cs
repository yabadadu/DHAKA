#region

using System;

#endregion

namespace HitopsCommon
{
    /// <summary>
    /// 2017.04.04 add by Ahn Jinsung
    /// Infragistics.Win.Misc.UltraGroupBox,System.Windows.Forms.GroupBox 클래스를 쉽게 Replace로 치환하기 위환 Wrapper Class
    /// </summary>
    public class Hitops_GroupBox : DevExpress.XtraEditors.GroupControl
    {
        public Hitops_GroupBox() : base()
        {
            this.ShowCaption = false;
        }
        public new String Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                if(base.Text.Length>0) this.ShowCaption = true;
                else this.ShowCaption = false;
            }
        }
    }
}
