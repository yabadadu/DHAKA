#region

using System.ComponentModel;
using Com.Hd.Core.Basis.Helper;
using Com.Hd.Core.View.Control.Interface;

#endregion

namespace Com.Hd.Core.View.Control
{
    public class HButton : System.Windows.Forms.Button, IHCulture
    {
        #region Field

        private string _textResourceId = string.Empty;

        #endregion

        #region Property

        [
            Browsable(true), DefaultValue(""), Category("HD Define"),
            Description("This resource id is used as key for searching localization value. this key should be in database or localization resource file" )
        ]
        public string TextResourceId
        {
            get { return _textResourceId; }
            set
            {
                _textResourceId = value;
                if (SystemDiagnosticsHelper.IsRunMode)
                {
                    RetrieveText();
                }
            }
        }
        #endregion

        #region Constructor

        public HButton() : base()
        {

        }

        #endregion

        #region Method

        private void RetrieveText()
        {
            var localText = GlobalizationHelper.RetriveLanguageResource(TextResourceId);
            this.Text = string.IsNullOrEmpty(localText) ? this.Text : localText;
        }

        #endregion
    }
}
