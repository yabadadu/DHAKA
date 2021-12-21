#region

using System;
using DevExpress.XtraEditors.Controls;

#endregion

namespace HitopsCommon
{
    /// <summary>
    /// 2017.04.04 add by Ahn Jinsung
    /// System.Windows.Forms.ComboBox 클래스를 쉽게 Replace로 치환하기 위환 Wrapper Class
    /// </summary>
    public class Hitops_ComboBox : DevExpress.XtraEditors.ComboBoxEdit
    {
        public Hitops_ComboBox() : base()
        {
            InitializeComponent();
        }

        public ComboBoxItemCollection Items
        {
            get { return this.Properties.Items; }
        }

        public System.Windows.Forms.ComboBoxStyle DropDownStyle
        {
            get
            {
                if (this.Properties.TextEditStyle == TextEditStyles.Standard)
                {
                    return System.Windows.Forms.ComboBoxStyle.DropDown;
                }
                else
                {
                    return System.Windows.Forms.ComboBoxStyle.DropDownList;
                }
            }
            set
            {
                if (value == System.Windows.Forms.ComboBoxStyle.DropDown)
                {
                    this.Properties.TextEditStyle = TextEditStyles.Standard;
                }
                else
                {
                    this.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
                }
            }
        }

        public Boolean FormattingEnabled
        {
            get
            {
                return true;
            }
            set
            {
                //No Operation
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

        public object SelectedValue
        {
            get
            {
                return this.EditValue;
            }
            set
            {
                this.EditValue = value;
            }
        }

        public Boolean Sorted
        {
            get
            {
                return this.Properties.Sorted;
            }
            set
            {
                this.Properties.Sorted = value;
            }
        }


        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).BeginInit();
            this.SuspendLayout();

            // 
            // Hitops_ComboBox
            // 
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Hitops_ComboBox_Layout);
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private void Hitops_ComboBox_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            //치환을 하면 아래 Combo Type EditorButton이 안들어간다(우측 끝 삼각형 아이콘)
            //생성자에서 억지로 넣으면 vs2015 Designer 에서 화면 수정중 자동으로 EditorButton 이 들어가버려 두개가 생기게된다.
            //여러 실험끝에 Layout 그려질때 개수를 파악하고 한개만 표시되도록 함.
            if (this.Properties.Buttons.Count > 1 || this.Properties.Buttons.Count == 0)
            {
                this.Properties.Buttons.Clear();
                this.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            }
        }
    }
}
