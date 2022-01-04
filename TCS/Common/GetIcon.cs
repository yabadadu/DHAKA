using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Resources;
using Hmx.DHAKA.TCS.Properties;
using System.Windows;
using System.Reflection;
using System.IO;

namespace Hmx.DHAKA.TCS.Common
{
    public static class GetIcon
    {
        public static Icon GetFormIcon(string formName)
        {
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Hmx.DHAKA.TCS.Resources.FormIcon.frmRegistrationAgent.ico");

            Icon ico = new Icon(s);

            return ico;


        }
    }
}
