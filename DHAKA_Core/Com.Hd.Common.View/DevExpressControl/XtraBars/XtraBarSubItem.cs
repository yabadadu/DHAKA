using System.ComponentModel;
using Com.Hd.Core.Basis.Helper;

namespace Com.Hd.Common.View.DevExpressControl.XtraBars
{
    public class XtraBarSubItem : DevExpress.XtraBars.BarSubItem
    {
        #region Field
        private string _textResourceId = string.Empty;
        #endregion

        #region Property

        [
            Browsable(true), DefaultValue(""), Category("HD Define"),
            Description("This resource id is used as key for searching localization value. this key should be in database or localization resource file")
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

        #region Initialization

        public XtraBarSubItem() : base()
        {
        }
        #endregion

        #region Method

        private void RetrieveText()
        {
            var localText = GlobalizationHelper.RetriveLanguageResource(TextResourceId);
            Caption = string.IsNullOrEmpty(localText) ? Caption : localText;

        }
        #endregion
    }
}
