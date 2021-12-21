#region

using System;
using System.Windows.Forms;

#endregion

namespace Com.Hd.Core.View.Control
{
    public class HToolStrip : ToolStrip
    {
        #region Constructor

        public HToolStrip() : base()
        {

        }

        #endregion

        #region Event

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            this.Refresh();
        }

        #endregion
    }
}
