#region
using HitopsCommon;
using HitopsCommon.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace Hmx.DHAKA.TCS
{
    static class Program
    {
        public static String g_MenuPrefix = "A10APP";
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(String[] pArgs)
        {
            BaseLogger.Info("App Initialized");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!CommFunc.CheckLoginData(pArgs)) return;  //파라미터 체크

            CommFunc.frmMainForm = new frmMain();         //공용 함수에 MDI폼 지정
            CommFunc.AddStatusBar();

            Application.Run(CommFunc.frmMainForm);

            BaseLogger.Info("App Closed");

        }
    }
}
