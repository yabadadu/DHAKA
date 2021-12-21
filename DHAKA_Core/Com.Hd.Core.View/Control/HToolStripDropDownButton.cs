using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Com.Hd.Core.Basis.Helper;
using Com.Hd.Core.View.Control.Interface;

namespace Com.Hd.Core.View.Control
{
    public class HToolStripDropDownButton : ToolStripDropDownButton, IHCulture
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

        #region Constructor

        public HToolStripDropDownButton() : base()
        {

        }

        #endregion

        #region Method

        private void RetrieveText()
        {
            var isNeedDisplayText = this.DisplayStyle == ToolStripItemDisplayStyle.ImageAndText ||
                                    this.DisplayStyle == ToolStripItemDisplayStyle.Text;
            if (isNeedDisplayText == false) return;

            var localText = GlobalizationHelper.RetriveLanguageResource(TextResourceId);
            this.Text = string.IsNullOrEmpty(localText) ? this.Text : localText;
            this.ToolTipText = GetFineToolTipText(Text);
        }

        private string GetFineToolTipText(string toolTipText)
        {
            return string.IsNullOrEmpty(toolTipText) ? toolTipText : toolTipText.Replace("&&", "&");
        }
        #endregion

    }
}
