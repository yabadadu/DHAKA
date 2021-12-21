#region

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Hd.Core.Basis.Helper;
using Com.Hd.Core.View.Control.Interface;

#endregion

namespace Com.Hd.Core.View.Control
{
    public class HLabel : System.Windows.Forms.Label, IHCulture
    {
        #region Field

        private string _textResourceId = string.Empty;
        private string _designTimeText = string.Empty;
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
                if (string.IsNullOrEmpty(_textResourceId))
                {
                    var parentForm = this.FindForm();
                    var isIHForm = parentForm is IHForm;
                    if (isIHForm)
                    {
                        var viewId = ((IHForm) parentForm).ViewId;
                        if (string.IsNullOrEmpty(viewId) == false)
                        {
                            _textResourceId = "wrd-" + viewId + "." + this.Name;
                        }
                    }
                }
                if (SystemDiagnosticsHelper.IsRunMode)
                {
                    RetrieveText();
                }
            }
        }
        
        #endregion

        #region Initialization

        public HLabel() : base()
        {
            //RegisterEventHandler();
        }

        public void RegisterEventHandler()
        {
            if (SystemDiagnosticsHelper.IsRunMode)
            {
                this.HandleCreated += HLabel_HandleCreated;
                this.HandleDestroyed += HLabel_HandleDestroyed;
            }
        }

        private void HLabel_HandleDestroyed(object sender, EventArgs e)
        {
        }

        private void HLabel_HandleCreated(object sender, EventArgs e)
        {
        }

        #endregion

        #region EventHandler

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            bool isSizeable = this.AutoSize;
            bool isWidthNeedAdjust = SystemDiagnosticsHelper.IsRunMode && string.IsNullOrEmpty(_designTimeText) == false;

            if (isSizeable && isWidthNeedAdjust)
            {
                var oldWidth = TextRenderer.MeasureText(_designTimeText, this.Font).Width;
                var newWidth = TextRenderer.MeasureText(this.Text, this.Font).Width + 3; // margin 3
                if (newWidth != oldWidth)
                {
                    this.Width = newWidth;
                    this.Left = this.Left - (newWidth - oldWidth);
                }
            }
        }

        #endregion

        #region Method

        private void RetrieveText()
        {
            _designTimeText = this.Text;

            var localText = GlobalizationHelper.RetriveLanguageResource(TextResourceId);
            this.Text = string.IsNullOrEmpty(localText) ? this.Text : localText;

        }
        #endregion
    }
}
