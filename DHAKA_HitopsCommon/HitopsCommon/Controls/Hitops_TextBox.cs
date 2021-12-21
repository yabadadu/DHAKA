#region

using System;

#endregion

namespace HitopsCommon
{
    /// <summary>
    /// 2017.04.04 add by Ahn Jinsung
    /// System.Windows.Forms.TextBox 클래스를 쉽게 Replace로 치환하기 위환 Wrapper Class
    /// </summary>
    public class Hitops_TextBox : DevExpress.XtraEditors.TextEdit, System.ComponentModel.ISupportInitialize
    {
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                if (this.Properties.BorderStyle == DevExpress.XtraEditors.Controls.BorderStyles.NoBorder)
                {
                    return System.Windows.Forms.BorderStyle.None;
                }
                else if (this.Properties.BorderStyle == DevExpress.XtraEditors.Controls.BorderStyles.Simple)
                {
                    return System.Windows.Forms.BorderStyle.FixedSingle;
                }
                else
                {
                    return System.Windows.Forms.BorderStyle.Fixed3D;
                }
            }
            set
            {
                if (value == System.Windows.Forms.BorderStyle.None)
                {
                    this.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                }
                else if(value == System.Windows.Forms.BorderStyle.FixedSingle)
                {
                    this.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                }
                else
                {
                    this.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                }
            }
        }

        public System.Windows.Forms.HorizontalAlignment TextAlign
        {
            get
            {
                if (this.Properties.Appearance.TextOptions.HAlignment == DevExpress.Utils.HorzAlignment.Far)
                {
                    return System.Windows.Forms.HorizontalAlignment.Right;
                }
                else if (this.Properties.Appearance.TextOptions.HAlignment == DevExpress.Utils.HorzAlignment.Near)
                {
                    return System.Windows.Forms.HorizontalAlignment.Left;
                }
                else
                {
                    return System.Windows.Forms.HorizontalAlignment.Center;
                }
            }
            set
            {
                if (value == System.Windows.Forms.HorizontalAlignment.Right)
                {
                    this.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                }
                else if (value == System.Windows.Forms.HorizontalAlignment.Left)
                {
                    this.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else
                {
                    this.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }
        }
        
        public int MaxLength
        {
            get
            {
                return this.Properties.MaxLength;
            }
            set
            {
                this.Properties.MaxLength = value;
            }
        }

        public Boolean UseSystemPasswordChar
        {
            get
            {
                return this.Properties.UseSystemPasswordChar;
            }
            set
            {
                this.Properties.UseSystemPasswordChar = value;
            }
        }

        public char PasswordChar
        {
            get
            {
                return this.Properties.PasswordChar;
            }
            set
            {
                this.Properties.PasswordChar = value;
            }
        }

        public void BeginInit()
        {
            //No Operation
            base.BeginUpdate();

        }

        public void EndInit()
        {
            //No Operation
            base.EndUpdate();
        }
    }
}
