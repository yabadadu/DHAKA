using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DevExpress.Skins;

namespace Hitops3Login
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.HitopsSkin).Assembly);
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.Skins.SkinManager.EnableMdiFormSkins();
            DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "Hitops_Skin";

            Application.Run(new frmHitops3Login());
        }
    }
}