#region

using Com.Hd.Core.View.Control;
using Com.Hd.Core.View.Control.Interface;

#endregion

namespace Com.Hd.Core.View.Template
{
    public class BaseForm : HForm, IHForm
    {
        #region Property
        public virtual string ViewId { get; set; }
        #endregion

        #region Constructor
        public BaseForm() : base()
        {
        }
        #endregion
    }
}
