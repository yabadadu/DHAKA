#region
using HitopsCommon;
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
        static void Main()
        {

            CommFunc.frmMainForm = new frmMain();         //공용 함수에 MDI폼 지정
            CommFunc.AddStatusBar();

            Application.Run(CommFunc.frmMainForm);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmMain());

        }
    }
}
