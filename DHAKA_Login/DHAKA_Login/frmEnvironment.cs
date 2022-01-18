using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Hitops3Updater_L;

namespace Hitops3Login
{
    public partial class frmEnvironment : Form
    {
        private String _MID = Hitops3Param.HITOPS3_PARAM.SYSTEM_PREFIX + "01";
        private String ServerName = "";
        
        public frmEnvironment()
        {
            InitializeComponent();
        }

        private void frmEnvironment_Load(object sender, EventArgs e)
        {
            if (Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME == "SK_IT_DEV")
            {
                radDemo.Checked = true;
            }
            else
            {
                radReal.Checked = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radReal_CheckedChanged(object sender, EventArgs e)
        {
            if (radReal.Checked)
            {
                ServerName = "SK_IT";
                radDemo.Checked = false;
            }
        }

        private void radDemo_CheckedChanged(object sender, EventArgs e)
        {
            if (radDemo.Checked)
            {
                ServerName = "SK_IT_DEV";
                radReal.Checked = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SetHitops3Param();

            //2017.07.14 add by Ahn Jinsung. 개발서버가 아닌 경우 아래 FailOver 체크 호출
            if (Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME != "SK_IT_DEV")
            {
                Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME = Hitops.common.FailOverSetting.FailOverCheckAndReplace("",3000);
            }

            this.Close();
        }

        // 3. System Information 정보 조회
        private void SetHitops3Param()
        {
            IniFileTool readIniFile = new IniFileTool(Directory.GetCurrentDirectory() + "/DLL/hitops.ini");
            String sSection = "HITOPS3";

            readIniFile.IniWriteValue(sSection, "FRAMEWORK_SERVER_NAME", ServerName);
            Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME = ServerName;
        }
    }
}