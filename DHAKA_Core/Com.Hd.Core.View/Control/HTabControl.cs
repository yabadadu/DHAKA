#region

using System.ComponentModel;
using System.Windows.Forms;
using Com.Hd.Core.Basis.Helper;

#endregion

namespace Com.Hd.Core.View.Control
{
    public class HTabControl: TabControl
    {
        #region Field

        private string[] _textResourceIds= null;

        #endregion 

        #region Property
        [
            Browsable(true), DefaultValue(""), Category("HD Define"),
            Description("This resource id is used as key for searching localization value. this key should be in database or localization resource file" )
        ]
        public string[] TextResourceIds
        {
            get { return _textResourceIds; }
            set
            {
                this._textResourceIds = value;

                if (SystemDiagnosticsHelper.IsRunMode)
                {
                    RetrieveText();
                }
            }
        }
        #endregion

        #region Constructor

        public HTabControl() : base()
        {
            
        }

        #endregion

        #region Method

        private void RetrieveText()
        {

            if (this._textResourceIds.Length == this.TabPages.Count)
            {
                for (int i = 0; i < TabPages.Count; i++)
                {
                    var index = i;
                    var localText = GlobalizationHelper.RetriveLanguageResource(TextResourceIds[index]);
                    this.Text = string.IsNullOrEmpty(localText) ? this.Text : localText;

                    TabPages[index].Refresh();
                }
            }
            else
            {
                return;
            }

        }
        
        #endregion

    }
}
