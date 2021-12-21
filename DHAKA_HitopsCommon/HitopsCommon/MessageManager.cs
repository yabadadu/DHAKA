using HitopsCommon.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitopsCommon
{
    public class MessageManager
    {
        #region FIELD & CONST AREA ***************
        #endregion

        #region METHOD AREA **********************
        public static DialogResult Show(string text)
        {
            DialogResult dResult = MessageBox.Show(text);
            BaseLogger.Info(text);

            return dResult;
        }

        public static DialogResult Show(Exception ex)
        {
            DialogResult dResult = MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            BaseLogger.Error(ex);

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            DialogResult dResult = MessageBox.Show(owner, text);
            BaseLogger.Info(text);

            return dResult;
        }
        
        public static DialogResult Show(string text, string caption)
        {
            DialogResult dResult = MessageBox.Show(text, caption);
            BaseLogger.Info(text);

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons);
            BaseLogger.Info(text);

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption);
            BaseLogger.Info(text);

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption, buttons);
            BaseLogger.Info(text);

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption, buttons, icon);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon, defaultButton);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon, defaultButton, options);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon, defaultButton, options, displayHelpButton);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            DialogResult dResult = MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            DialogResult dResult = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword);

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    BaseLogger.Error(text);
                    break;
                case MessageBoxIcon.Warning:
                    BaseLogger.Warn(text);
                    break;
                default:
                    BaseLogger.Info(text);
                    break;
            }

            return dResult;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            {
                DialogResult dResult = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param);

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        BaseLogger.Error(text);
                        break;
                    case MessageBoxIcon.Warning:
                        BaseLogger.Warn(text);
                        break;
                    default:
                        BaseLogger.Info(text);
                        break;
                }

                return dResult;
            }
        }
        #endregion
    }
}
