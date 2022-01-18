using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Hitops.exception;
using Hitops;
using Hitops3Updater_L;

namespace Hitops3Main
{
    public partial class frmChgPwd : Form
    {
        private String _MID = Hitops3Param.HITOPS3_PARAM.SYSTEM_PREFIX + "01";
      
        public frmChgPwd()
        {
            InitializeComponent();
        }

        public frmChgPwd(String p_sMsg)
        {
            InitializeComponent();

            lblMsg.Text = p_sMsg;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            Hashtable hKeyPassword = new Hashtable();
            hKeyPassword.Add("USER_ID", Hitops3Param.HITOPS3_PARAM.USER_ID);
            hKeyPassword.Add("OLD_PASSWORD", Hitops3Param.HITOPS3_PARAM.PWD);
            hKeyPassword.Add("NEW_PASSWORD", tbxNewPwd.Text);
            hKeyPassword.Add("CFM_PASSWORD", tbxCfm.Text);

            if(tbxNewPwd.Text != tbxCfm.Text)
            {
                MessageBox.Show("Password가 다릅니다. 다시 확인하여 주십시오.", "Warning");
            }
            else if (tbxNewPwd.Text == Hitops3Param.HITOPS3_PARAM.PWD)
            {
                MessageBox.Show("기존 Password와 동일합니다. 다른 Password를 입력하십시오.", "Warning");
            }
            else if (tbxNewPwd.Text.Length < 8)
            {
                MessageBox.Show("8자리 이상 입력하여 주십시오.", "Warning");
            }
            else
            {
                Boolean isAlpha = false;
                Boolean isNum = false;
                Boolean isSymbol = false;
                Boolean isIllegal = false;

                for (int i = 0; i < tbxNewPwd.Text.Length; i++)
                {
                    Char sChar = Convert.ToChar(tbxNewPwd.Text.Substring(i, 1));

                    if (sChar >= 'a' && sChar <= 'z') { isAlpha = true; }
                    else if (sChar >= 'A' && sChar <= 'Z') { isAlpha = true; }
                    else if (sChar >= '0' && sChar <= '9') { isNum = true; }
                    else if ("!@#$%&*()_".Contains(tbxNewPwd.Text.Substring(i, 1))) { isSymbol = true; }
                    else { isIllegal = true; break; }
                }
                if (isIllegal)
                {
                    MessageBox.Show("알파벳(a-z, A-Z)/숫자(0-9)/기호 !@#$%&*()_ 내에서 입력하십시오.", "Warning");
                }
                else if (isAlpha && isNum && isSymbol)
                {
                    try
                    {
                        ArrayList aResult = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-USR-P-UPDCHGPWD", _MID, hKeyPassword);

                        if (aResult.Count == 0)
                        {
                            MessageBox.Show("Password change error.");
                        }
                        else
                        {
                            Hashtable hResult = aResult[0] as Hashtable;

                            if (hResult["RESULT"].ToString() == "N")
                            {
                                MessageBox.Show(hResult["MESSAGE"].ToString());
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                    }
                    catch (HMMException ex)
                    {
                    }
                }
                else
                {
                    MessageBox.Show("알파벳(a-z, A-Z)/숫자(0-9)/기호 !@#$%&*()_ 가 반드시 포함되어야 합니다.", "Warning");
                }
            }

        }
    }
}