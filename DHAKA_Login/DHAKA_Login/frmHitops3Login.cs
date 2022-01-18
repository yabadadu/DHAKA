using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Configuration;
using System.Linq;

using Hitops;
using Hitops.Common;
using Hitops.user;
using Hitops.security;
using Hitops.exception;
using System.Net;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using Hitops3Updater_L;
using Hitops3Main;

namespace Hitops3Login
{
    public partial class frmHitops3Login : DevExpress.XtraEditors.XtraForm
    {
        private static frmHitops3Login _instance = null;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        [DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWNORMAL = 1;

        private String _MID = Hitops3Param.HITOPS3_PARAM.SYSTEM_PREFIX + "01";
        
        private ArrayList g_aErrorFile = new ArrayList();
        private Boolean g_isDownload = true;
        private Boolean g_isProcessMain = true;

        public Boolean IS_DOWNLOAD { set { g_isDownload = value; } }

        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public frmHitops3Login()
        {
            InitializeComponent();



            _instance = this;
            KeyPreview = true;
        }

        public static frmHitops3Login GetInstance()
        {
            if (_instance == null)
                _instance = new frmHitops3Login();

            return _instance;
        }

        /******************************************************************************
         *                    Form 관련 Event Handler 함수                            *
         ******************************************************************************/
        private void frmHitops3Login_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;

            //2017.06.01 Modified by Ahn Jinsung. TODO 임시로 중복 생성되게 풀음
            /*//------------S
            //2017.08.22 add by Ahn Jinsung. Process.GetProcessesByName("Hitops3Main"); 함수는 32bit 에서 64bit 조회시 에러가나므로
            //아래 함수로 대체
            int ProcessID = GetProcessInfoByPID("Hitops3Main", "Hyundai-UNI");

            if (ProcessID != -1)
            {
                Process hitopsMain = Process.GetProcessById(ProcessID);

                IntPtr procHandler = hitopsMain.MainWindowHandle;
                //hitopsMain[i].in
                //IntPtr procHandler = FindWindow(null, "SK_IT");

                // 활성화
                ShowWindow(procHandler, SW_SHOWNORMAL);
                SetForegroundWindow(procHandler);
                this.Close();
                return;
            }*/

            Process[] hitopsMain = Process.GetProcessesByName("Hitops3Main");

            for (int i = 0; i < hitopsMain.Length; i++)
            {
                if (hitopsMain[i].MainModule.FileName.Contains("Hyundai-UNI"))
                {
                    IntPtr procHandler = hitopsMain[i].MainWindowHandle;
                    //hitopsMain[i].in
                    //IntPtr procHandler = FindWindow(null, "SK_IT");

                    // 활성화
                    ShowWindow(procHandler, SW_SHOWNORMAL);
                    SetForegroundWindow(procHandler);
                    this.Close();
                    return;
                }
            }
            //------------E

            RegLoadLoginId();
            SetHitops3Param();

            RefershDemoPrdNotice();

            //lblSystemName.Text = Hitops3Param.TERMINAL_NAME;
            //lblSystemName.Text = "Hyundai Intelligence Terminal Operation && Planning System";
            //if (Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME == "HITOPS3_TEST")
            //{
            //    lblSystemName.Text = "Demo Environment";
            //}
            //int iX = Convert.ToInt32((pnlBtm.Width - lblSystemName.Width) / 2);
            //lblSystemName.Location = new Point(iX, lblSystemName.Location.Y);
            //lblSystemName.ForeColor = Color.FromArgb(Convert.ToInt32("61", 16), Convert.ToInt32("8a", 16), Convert.ToInt32("a6", 16));
        }

        private void frmHitops3Login_Shown(object sender, EventArgs e)
        {
            timLoading.Start();

            if (!tbxUserId.Text.Equals(String.Empty))
                tbxPassword.Focus();
        }

        private void frmHitops3Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
        }

        /******************************************************************************
         *                    Button & Key Event Handler 함수                         *
         ******************************************************************************/
        private void btnLogin_Click(object sender, EventArgs e)
        {
            ExecuteLogin();
        }

        private void frmHitops3Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)Keys.Enter))
            {
                ExecuteLogin();
            }
        }

        /******************************************************************************
         *                               Login 실행 함수                              *
         ******************************************************************************/
        private void ExecuteLogin()
        {
            try
            {
                // 1. User ID & Password 입력 여부
                if (!CheckInputLoginData())
                    return;

                // 2. Network Connection 여부
                if (!IsConnectedToInternet())
                {
                    MessageBox.Show("Network Connection is down. Watch your Network.");
                    return;
                }

                // 3. System Information 정보 조회
                GetFTPInfo();

                // 4. Check User Information 
                //if (!CheckUserInfoNew())
                //    return;
                if (!CheckDouzoneERP_LogIn())
                    return;
                
                //4.5 더존ERP의 유저정보를 ADM_USER에 복사
                DouzoneERPUser2AdmUser();

                // 5. Check Executing HI-TOPSIII Program
                if (!CheckExeHitopsPrg())
                    return;

                // 6. Local File Ver Information(ini 파일) 존재 유무 : 없을 경우 생성
                //CheckLocalFileVerInfo();

                // 7. Main EXE File 업데이트
                UpdateEXEFile(Hitops3Param.HITOPS3_PARAM.MAIN_EXE_ID);

                if (!g_isDownload)
                {
                    String sErrorMsg = g_aErrorFile.Count.ToString() + "개의 파일이 업데이트에 실패하였습니다." + Environment.NewLine;
                    sErrorMsg += "상세 정보 : " + Environment.NewLine;

                    foreach (String sError in g_aErrorFile)
                    {
                        sErrorMsg += "   - " + sError + Environment.NewLine;
                    }

                    MessageBox.Show(sErrorMsg);
                    g_isDownload = true;
                }

                ////2017.07.14 add by Ahn Jinsung. 개발서버가 아닌 경우 아래 FailOver 체크 호출
                ////로그인 된 ID에 의해 TERMINAL CODE가 정해진 상황이므로 PNIT 는 PNIT URL로 다시 체크 PHPNT는 PHPNT로 다시 체크
                //if (Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME != "SK_IT_DEV")
                //{
                //    Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME = 
                //        Hitops.common.FailOverSetting.FailOverCheckAndReplace(Hitops3Param.HITOPS3_PARAM.TERMINAL_NAME, 3000);
                //}

                // 8. Main EXE File 실행
                MainEXEProcess();
            }
            catch (HMMException ex)
            {
                switch (ex.Result)
                {
                    case "Hitops_L":
                    case "Hitops":
                        MessageBox.Show("Hitops.ini을 체크하십시오.");
                        break;
                }
            }
        }

        private void UpdateEXEFile(String p_sEXEId)
        {
            // 1. Local File Ver Information(ini 파일) 존재 유무 : 없을 경우 생성
            CheckLocalFileVerInfo();

            frmHitops3Updater update = new frmHitops3Updater(Hitops3Param.HITOPS3_PARAM, p_sEXEId);
            update.ShowDialog();
        }

        // 1. User ID & Password 입력 여부
        private Boolean CheckInputLoginData()
        {
            if (tbxUserId.Text.Equals(""))
            {
                MessageBox.Show("Input User ID.");
                return false;
            }
            else if (tbxPassword.Text.Equals(""))
            {
                MessageBox.Show("Input Password.");
                return false;
            }

            return true;
        }

        // 3. System Information 정보 조회
        private void SetHitops3Param()
        {
            IniFileTool readIniFile = new IniFileTool(Directory.GetCurrentDirectory() + "/DLL/hitops.ini");
            String sSection = "HITOPS3";

            Hitops3Param.HITOPS3_PARAM.TERMINAL_NAME = readIniFile.IniReadValue(sSection, "TERMINAL_NAME");
            Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME = readIniFile.IniReadValue(sSection, "FRAMEWORK_SERVER_NAME");
            Hitops3Param.HITOPS3_PARAM.SYSTEM_PREFIX = readIniFile.IniReadValue(sSection, "SYSTEM_PREFIX");
            Hitops3Param.HITOPS3_PARAM.SYSTEM_CODE = readIniFile.IniReadValue(sSection, "SYSTEM_CODE");

            ////2017.07.14 add by Ahn Jinsung. 개발서버가 아닌 경우 아래 FailOver 체크 호출
            //if (Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME != "HITOPS3_PSA_DEV")
            //{
            //    Hitops.common.FailOverSetting.FailOverCheckAndReplace("",3000);
            //    //정정된 Framework Server Name을 가져옴
            //    Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME = readIniFile.IniReadValue(sSection, "FRAMEWORK_SERVER_NAME");
            //}
            
            try
            {
                RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ENG-WMRCOM-S-GETSYSDATE");
            }
            catch (HMMException ex)
            {
                switch (ex.Result)
                {
                    case "ProtocolError":
                        if(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME == "SK_IT")
                        {
                            //Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME = "HITOPS3_PSA_PNIT_FAIL";
                        }
                        break;
                    case "Hitops_L":
                    case "Hitops":
                        MessageBox.Show("Hitops.ini을 체크하십시오.");
                        break;
                }
            }

            // 1. Get Local IP
            Hitops3Param.HITOPS3_PARAM.LOCAL_IP = GetLocalIP();

            // 2. FTP Information 조회
        }

        // 3-1. Get Local IP
        private String GetLocalIP()
        {
            string sClientIp = String.Empty;
            IPHostEntry senderIP = Dns.GetHostEntry(Dns.GetHostName());
            for (int i = 0; i < senderIP.AddressList.Length; i++)
            {
                if (senderIP.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    sClientIp = senderIP.AddressList[i].ToString();
                    break;
                }
            }

            return sClientIp;
        }

        // 3-2. FTP Information 조회
        private void GetFTPInfo()
        {
            try
            {
                Hashtable hKeyFTPInfo = new Hashtable();
                hKeyFTPInfo.Add("SYSTEM_COD", Hitops3Param.HITOPS3_PARAM.SYSTEM_CODE);

                ArrayList aFTPInfo = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-ADM-S-GETFTPINFO", _MID, hKeyFTPInfo);

                if (aFTPInfo.Count > 0)
                {
                    Hashtable hFTPInfo = (Hashtable)aFTPInfo[0];
                    Hitops3Param.HITOPS3_PARAM.FTP_IP = hFTPInfo["FTP_IP_ADDR"].ToString() + "/" + hFTPInfo["ROOT_DIR"].ToString();
                    Hitops3Param.HITOPS3_PARAM.FTP_USER_ID = hFTPInfo["FTP_USER_ID"].ToString();
                    Hitops3Param.HITOPS3_PARAM.FTP_PWD = hFTPInfo["FTP_USER_PWD"].ToString();
                    Hitops3Param.HITOPS3_PARAM.FTP_EXE_DIR = hFTPInfo["EXE_DIR"].ToString();
                    Hitops3Param.HITOPS3_PARAM.FTP_RELATED_FILE_DIR = hFTPInfo["RELATED_FILE_DIR"].ToString();
                    Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_DIR = hFTPInfo["LOC_VER_INFO_DIR"].ToString();
                    Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_FILE = hFTPInfo["LOC_VER_INFO_FILE"].ToString();
                    Hitops3Param.HITOPS3_PARAM.LOC_VER_EXE_SECTION = hFTPInfo["LOC_VER_EXE_SECTION"].ToString();
                    Hitops3Param.HITOPS3_PARAM.LOC_VER_RELATED_SECTION = hFTPInfo["LOC_VER_RELATED_SECTION"].ToString();
                    Hitops3Param.HITOPS3_PARAM.MAIN_EXE_ID = hFTPInfo["MAIN_EXE_ID"].ToString();
                    Hitops3Param.HITOPS3_PARAM.MAIN_EXE_NAME = hFTPInfo["MAIN_EXE_NAME"].ToString();
                    Hitops3Param.HITOPS3_PARAM.MAIN_EXE_VER = hFTPInfo["MAIN_EXE_VER"].ToString();
                    Hitops3Param.HITOPS3_PARAM.MAIN_EXE_CLIENT_DIR = hFTPInfo["CLIENT_DIR"].ToString();
                    Hitops3Param.HITOPS3_PARAM.LOGIN_EXE_ID = hFTPInfo["LOGIN_EXE_ID"].ToString();

                }
                else
                {
                    MessageBox.Show("Can't receive System Information!!");
                    throw new HMMException();
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }
        }

        private Boolean CheckDouzoneERP_LogIn()
        {
            Boolean isSuccess = false;
            Hitops3Param.HITOPS3_PARAM.USER_ID = tbxUserId.Text;
            Hitops3Param.HITOPS3_PARAM.PWD = tbxPassword.Text;

            try
            {
                Hashtable reqLogin = new Hashtable();
                reqLogin.Add("ID_USER", Hitops3Param.HITOPS3_PARAM.USER_ID);
                reqLogin.Add("PASS_WORD", Hitops3Param.HITOPS3_PARAM.PWD);
                ArrayList aResult = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "SKIT-ADM-USR-J-CHKDOUZONEERPLOGIN", _MID, reqLogin);
                if(aResult.Count > 0)
                {
                    Hashtable result = aResult[0] as Hashtable;
                    if(result["RTN"].ToString().Equals("Y") == true)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        MessageBox.Show(result["DESC"].ToString(), result["ERR"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //for Test
                        //더존쪽 로그인 암호화처리시 오류가 발생하여 임의로 그냥 처리함
                        isSuccess = true;
                    }
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show("DB Connection Exception.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxUserId.Focus();
            }

            return isSuccess;
        }

        // 4. Check User Information 
        private Boolean CheckUserInfoNew()
        {
            Boolean isSuccess = false;
            Hitops3Param.HITOPS3_PARAM.USER_ID = tbxUserId.Text;
            Hitops3Param.HITOPS3_PARAM.PWD = tbxPassword.Text;

            ArrayList aUserInfo = new ArrayList();
            Hashtable hKeyUserInfo = new Hashtable();
            hKeyUserInfo.Add("USER_ID", Hitops3Param.HITOPS3_PARAM.USER_ID);

            try
            {
                aUserInfo = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-USR-S-GETLOGININFO", _MID, hKeyUserInfo);

                if (aUserInfo.Count == 0)
                {
                    MessageBox.Show("Not Registered User ID.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbxUserId.Focus();
                }
                else
                {
                    Hashtable hUserInfo = (Hashtable)aUserInfo[0];

                    int iFailCnt = Convert.ToInt32(hUserInfo["FAIL_CNT"].ToString());

                    // Password Rule을 체크해야 하는 그룹
                    if (hUserInfo["PWD_RULE_CHK"].ToString() == "Y")
                    {
                        // 1. Password 오류 3회 이상
                        if (iFailCnt >= 3)
                        {
                            MessageBox.Show("3번의 Login 실패로 계정이 잠겼습니다. 전산팀으로 문의하여 주십시오.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        // 2. Password 임시 설정
                        else if (iFailCnt == -1)
                        {
                            // 2-1. Password 임시 설정 후 정상 Login
                            if (Hitops3Param.HITOPS3_PARAM.PWD == hUserInfo["PASSWORD"].ToString())
                            {
                                frmChgPwd chgPwd = new frmChgPwd("임시 Password 입니다. Password를 변경하십시오");
                                chgPwd.ShowDialog();
                            }
                            // 2-2. Password 임시 설정 후 Password 오류
                            else
                            {
                                MessageBox.Show("Login에 실패하였습니다.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        // 3. Password 불일치
                        else if (Hitops3Param.HITOPS3_PARAM.PWD != hUserInfo["PASSWORD"].ToString())
                        {
                            int iRemain = 3 - ++iFailCnt;
                            if (iRemain != 0)
                            {
                                MessageBox.Show(String.Format("Password가 틀렸습니다. {0}회의 기회가 남았습니다.", iRemain), "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("3번의 Login 실패로 계정이 잠겼습니다. 전산팀으로 문의하여 주십시오.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            hKeyUserInfo.Add("FAIL_CNT", iFailCnt.ToString());
                            RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-USR-S-UPDFAILCNT", _MID, hKeyUserInfo);
                        }
                        // 4. Password 기한 만료
                        else if (hUserInfo["EXPIRED"].ToString() == "Y")
                        {
                            frmChgPwd chgPwd = new frmChgPwd("Password 의 기한(90일)이 만료되었습니다. Password를 변경하십시오");
                            chgPwd.ShowDialog();
                        }
                        else
                        {
                            isSuccess = true;

                            if (!String.IsNullOrEmpty(hUserInfo["REMAIN_EXPIRED"].ToString()))
                            {
                                int iRemainExpired = Convert.ToInt32(hUserInfo["REMAIN_EXPIRED"].ToString());

                                if(iRemainExpired <= 7)
                                {
                                    String sMsg = String.Format("패스워드 만료 기한이 {0} 일 남았습니다. 만료되기 전에 변경하세요.", iRemainExpired);
                                    MessageBox.Show(sMsg);
                                }
                            }

                            Hashtable hParam = new Hashtable();
                            hParam.Add("USER_ID", Hitops3Param.HITOPS3_PARAM.USER_ID);

                            RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-USR-S-UPDINITFAILCOUNT", _MID, hParam);
                        }
                    }
                    else   // Password Rule을 체크 안하는 그룹은 Password 만 비교
                    {
                        // 5. Password 일치
                        if (Hitops3Param.HITOPS3_PARAM.PWD == hUserInfo["PASSWORD"].ToString())
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            isSuccess = false;
                            MessageBox.Show("Password is wrong.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (isSuccess)
                    {
                        RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-USER-S-UPDUSERLOGINCNT", _MID, hKeyUserInfo);

                        Hitops3Param.HITOPS3_PARAM.TERMINAL_NAME = hUserInfo["TMN_COD"].ToString();
                    }
                }

                if (!isSuccess)
                {
                    tbxPassword.Focus();
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show("DB Connection Exception.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxUserId.Focus();
            }

            return isSuccess;
        }

        private Boolean CheckUserInfo()
        {
            Boolean blnReturn = false;

            Hitops3Param.HITOPS3_PARAM.USER_ID = tbxUserId.Text;
            Hitops3Param.HITOPS3_PARAM.PWD = tbxPassword.Text;

            ArrayList aUserInfo = new ArrayList();
            Hashtable hKeyUserInfo = new Hashtable();
            hKeyUserInfo.Add("USER_ID", Hitops3Param.HITOPS3_PARAM.USER_ID);
            hKeyUserInfo.Add("PASSWORD", Hitops3Param.HITOPS3_PARAM.PWD);

            try
            {
                aUserInfo = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-USR-S-GETPWDMATCH", _MID, hKeyUserInfo);

                Hashtable hUserInfo = (Hashtable)aUserInfo[0];

                String sCase = hUserInfo["EXIST"].ToString() + hUserInfo["MATCH"].ToString();

                switch (sCase)
                {
                    case "YY":
                        blnReturn = true;
                        RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-USER-S-UPDUSERLOGINCNT", _MID, hKeyUserInfo);
                        break;
                    case "YN":
                        MessageBox.Show("Password is wrong.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbxPassword.Text = String.Empty;
                        tbxPassword.Focus();
                        break;
                    case "NN":
                        MessageBox.Show("Not Registered User ID.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbxUserId.Focus();
                        break;
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show("DB Connection Exception.", "SK_IT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxUserId.Focus();
            }

            return blnReturn;
        }

        private void DouzoneERPUser2AdmUser()
        {
            //1. ADM_USER에 정보가 존재하는지 확인
            Hashtable reqUser = new Hashtable();
            reqUser.Add("USER_ID", Hitops3Param.HITOPS3_PARAM.USER_ID);

            ArrayList aUserInfo = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "SKIT-ADM-USR-S-GETADMUSERSIMPLE", _MID, reqUser);

            //2. neoe.ma_user정보를 ADM_USER에 복사
            if(aUserInfo.Count == 0)
            {
                ArrayList rstList = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "SKIT-ADM-USR-S-CRTDOUZONE2ADMUSER", _MID, reqUser);
            }
        }

        // 5. Check Executing HI-TOPSIII Program
        private Boolean CheckExeHitopsPrg()
        {
            //2017.06.01 TODO add by Ahn Jinsung.
            //우선 두개의 프로그램이 실행되어야하므로 아래 중복은 패스
            //-----S
            return true;
            //-----E

            Hashtable hKeyEXEFileNm = new Hashtable();
            hKeyEXEFileNm.Add("LOGIN_EXE_ID", Hitops3Param.HITOPS3_PARAM.LOGIN_EXE_ID);

            try
            {
                ArrayList aEXEFileNm = RequestHandler.Request(Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-ADM-LSTEXEFILENMNOTLOGIN", _MID, hKeyEXEFileNm);

                foreach (Hashtable hEXEFileNm in aEXEFileNm)
                {
                    if (Process.GetProcessesByName(hEXEFileNm["EXE_NM"].ToString().Split('.')[0]).Length > 0)
                    {
                        MessageBox.Show("The programs related to HI-TOPSIII have to be shut down.", "Warning");
                        return false;
                    }
                }
                return true;
            }
            catch (HMMException ex)
            {
                MessageBox.Show(ex.Message1);
                return false;
            }
        }

        // 6. Local File Ver Information(ini 파일) 존재 유무 : 없을 경우 생성
        private void CheckLocalFileVerInfo()
        {
            String sDir = Application.StartupPath + "/" + Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_DIR;
            String sFile = sDir + "/" + Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_FILE;

            if (!Directory.Exists(sDir))
            {
                try
                {
                    Directory.CreateDirectory(sDir);
                }
                catch
                {
                    MessageBox.Show("Can't Create " + Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_DIR + " Directory");
                    return;
                }
            }

            if (!File.Exists(sFile))
            {
                try
                {
                    File.AppendAllText(sFile, "[" + Hitops3Param.HITOPS3_PARAM.LOC_VER_EXE_SECTION + "]" + Environment.NewLine);
                    File.AppendAllText(sFile, "[" + Hitops3Param.HITOPS3_PARAM.LOC_VER_RELATED_SECTION + "]" + Environment.NewLine);
                }
                catch
                {
                    MessageBox.Show("Can't Create " + Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_FILE + " File");
                }
            }
        }

        // 7. Main EXE File 업데이트
        //private void MainEXEUpdate()
        //{
        //    this.WindowState = FormWindowState.Minimized;
        //    // 1. Main EXE File 업데이트
        //    FileVerCheck(Hitops3Param.MAIN_EXE_ID,
        //          Hitops3Param.MAIN_EXE_VER,
        //          Hitops3Param.MAIN_EXE_NAME,
        //          Hitops3Param.FTP_EXE_DIR,
        //          Hitops3Param.MAIN_EXE_CLIENT_DIR,
        //          Hitops3Param.LOC_VER_EXE_SECTION);

        //    // 2. Main EXE File 관련 파일 업데이트
        //    Hashtable hKeyRelatedFile = new Hashtable();
        //    hKeyRelatedFile.Add("EXE_ID", Hitops3Param.MAIN_EXE_ID);

        //    try
        //    {
        //        ArrayList aRelatedFile = RequestHandler.Request(Hitops3Param.FRAMEWORK_SERVER_NAME, "HITOPS3-ADM-ADM-S-LSTEXERELATEDFILE", _MID, hKeyRelatedFile);

        //        foreach (Hashtable hRelatedFile in aRelatedFile)
        //        {
        //            if (g_isDownload.Equals(false))
        //                break;

        //            String sRelatedFileID = hRelatedFile["RELATED_FILE_ID"].ToString();
        //            String sRelatedFileVer = hRelatedFile["FILE_VER"].ToString();
        //            String sRelatedFileNm = hRelatedFile["FILE_NM"].ToString();
        //            String sRelatedFileClientDir = hRelatedFile["CLIENT_DIR"].ToString();

        //            FileVerCheck(sRelatedFileID,
        //                       sRelatedFileVer,
        //                       sRelatedFileNm,
        //                       Hitops3Param.FTP_RELATED_FILE_DIR,
        //                       sRelatedFileClientDir,
        //                       Hitops3Param.LOC_VER_RELATED_SECTION);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        // 7-1. Main EXE File 업데이트 
        //private void FileVerCheck(String p_sFileId, String p_sFileVer, String p_sFileNm, String p_sServerDir, String p_sClientDir, String p_sSection)
        //{
        //    String sIniFilePath = Application.StartupPath + "/" + Hitops3Param.LOC_VER_INFO_DIR + "/" + Hitops3Param.LOC_VER_INFO_FILE;
        //    IniFileTool iniLocalFileVer = new IniFileTool(sIniFilePath);
        //    String sLocalFileVer = iniLocalFileVer.IniReadValue(p_sSection, p_sFileId);

            

        //    if (!p_sFileVer.Equals(sLocalFileVer))
        //    {
        //        try
        //        {
        //            FTPCommFunc.frmDownload download = new FTPCommFunc.frmDownload();
        //            download.changeDownloadVariable = ChangeDownloadVariable;
        //            download.SetDownloadInfo(Hitops3Param.USER_ID,
        //                                     Hitops3Param.FRAMEWORK_SERVER_NAME,
        //                                     Hitops3Param.FTP_IP,
        //                                     Hitops3Param.FTP_USER_ID,
        //                                     Hitops3Param.FTP_PWD,
        //                                     Hitops3Param.FTP_EXE_DIR,
        //                                     Hitops3Param.FTP_RELATED_FILE_DIR,
        //                                     Hitops3Param.LOC_VER_INFO_DIR,
        //                                     Hitops3Param.LOC_VER_INFO_FILE,
        //                                     Hitops3Param.LOC_VER_EXE_SECTION,
        //                                     Hitops3Param.LOC_VER_RELATED_SECTION,
        //                                     p_sFileId,
        //                                     p_sFileVer,
        //                                     p_sFileNm,
        //                                     p_sServerDir,
        //                                     p_sClientDir,
        //                                     p_sSection);
        //            download.ShowDialog();

        //            //if (sLocalFileVer.Equals(String.Empty))
        //            //{
        //            //    iniLocalFileVer.IniWriteValue(p_sSection, p_sFileId, p_sFileVer);
        //            //}
        //        }
        //        catch (TargetInvocationException tie)
        //        {
        //            DialogResult dialogResult = MessageBox.Show(p_sFileNm + " 파일이 이미 사용중입니다. 프로그램 실행을 중단하시겠습니까?","Warning",MessageBoxButtons.YesNo);

        //            if (dialogResult.Equals(DialogResult.No))
        //            {
        //                //g_isDownload = false;
        //                g_aErrorFile.Add(p_sFileNm);
        //            }
        //            else
        //            {
        //                g_isDownload = false;
        //                g_isProcessMain = false;
        //            }
        //        }
        //        catch (UnauthorizedAccessException uae)  
        //        {
        //            DialogResult dialogResult = MessageBox.Show(p_sFileNm + " 파일이 이미 사용중입니다. 프로그램 실행을 중단하시겠습니까?","Warning",MessageBoxButtons.YesNo);

        //            if (dialogResult.Equals(DialogResult.No))
        //            {
        //                //g_isDownload = false;
        //                g_aErrorFile.Add(p_sFileNm);
        //            }
        //            else
        //            {
        //                g_isDownload = false;
        //                g_isProcessMain = false;
        //            }
        //        }
        //        catch
        //        {
        //            DialogResult dialogResult = MessageBox.Show(p_sFileNm + "파일 다운로드 중 에러가 발생하였습니다. 프로그램 실행을 계속 하시겠습니까?", "Warning", MessageBoxButtons.YesNo);

        //            if (dialogResult.Equals(DialogResult.No))
        //            {
        //                //g_isDownload = false;
        //                g_aErrorFile.Add(p_sFileNm);
        //            }
        //            else
        //            {
        //                g_isDownload = false;
        //                g_isProcessMain = false;
        //            }
        //        }
        //    }
        //}

        // 8. Main EXE File 실행
        private void MainEXEProcess()
        {
            if (g_isProcessMain)
            {
                String[] args = { };

                String strArg = "";

                strArg += Hitops3Param.HITOPS3_PARAM.USER_ID + " ";                   // 로그인 ID
                strArg += Hitops3Param.HITOPS3_PARAM.PWD + " ";                       // 로그인 Password
                strArg += Hitops3Param.HITOPS3_PARAM.SYSTEM_PREFIX + " ";             // System prefix
                strArg += Hitops3Param.HITOPS3_PARAM.TERMINAL_NAME + " ";             // 터미널 이름
                strArg += Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME + " ";     // Framework Server 이름
                strArg += Hitops3Param.HITOPS3_PARAM.LOCAL_IP + " ";                  // 호스트 IP
                strArg += Hitops3Param.HITOPS3_PARAM.SYSTEM_CODE + " ";               // System Code
                strArg += Hitops3Param.HITOPS3_PARAM.FTP_IP + " ";                     // FTP IP
                strArg += Hitops3Param.HITOPS3_PARAM.FTP_USER_ID + " ";               // FTP User ID
                strArg += Hitops3Param.HITOPS3_PARAM.FTP_PWD + " ";                   // FTP Password
                strArg += Hitops3Param.HITOPS3_PARAM.FTP_EXE_DIR + " ";               // FTP 내 EXE Directory
                strArg += Hitops3Param.HITOPS3_PARAM.FTP_RELATED_FILE_DIR + " ";      // FTP 내 Related File Directory
                strArg += Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_DIR + " ";          // Local의 File Version 정보 가진 파일의 Directory
                strArg += Hitops3Param.HITOPS3_PARAM.LOC_VER_INFO_FILE + " ";         // Local의 File Version 정보 가진 파일의 Name
                strArg += Hitops3Param.HITOPS3_PARAM.LOC_VER_EXE_SECTION + " ";       // Local의 File Version 정보내에 EXE 파일 Section
                strArg += Hitops3Param.HITOPS3_PARAM.LOC_VER_RELATED_SECTION + " ";   // Local의 File Version 정보내에 Related 파일 Section
                strArg += Hitops3Param.HITOPS3_PARAM.LOGIN_EXE_ID + " ";   // Local의 File Version 정보내에 Related 파일 Section

                RegSaveLoginId();

                String strEXE = Application.StartupPath + "/" + Hitops3Param.HITOPS3_PARAM.MAIN_EXE_NAME;
                String processNm = Hitops3Param.HITOPS3_PARAM.MAIN_EXE_NAME.Split('.')[0];
                Process[] processes = Process.GetProcesses();

                //2017.06.01 TODO add by Ahn Jinsung.
                //구 Hitops 와 동시에 실행해야하기에 우선 아래 로직을 막음
                //-----S
                //foreach (Process proc in processes)
                //{
                //    if (proc.ProcessName.Equals(processNm))
                //    {
                //        MessageBox.Show("This Program is already processing.");
                //        Close();
                //        return;
                //    }
                //}
                //-----E

                // Check File
                FileInfo fileInfo = new FileInfo(strEXE);
                if (fileInfo.Exists == true)
                {
                    Process.Start(strEXE, strArg);
                    Close();
                }
                else
                {
                    MessageBox.Show("메인 프로그램 EXE 파일이 존재하지 않습니다." + Environment.NewLine + "(" + strEXE + ")");
                }
            }
            else
            {
                g_isProcessMain = true;
            }
        }

        // User ID 레지스트리로 저장
        public void RegSaveLoginId()
        {
            RegistryKey userRootKey = Registry.CurrentUser.OpenSubKey(@"Software\HITOPS3\UserId", true);
            userRootKey.SetValue("UserID", tbxUserId.Text);
        }

        // User ID 레지스트리에서 불러오기(크기 지정가능)
        public void RegLoadLoginId()
        {
            RegistryKey userRootKey = null;
            String arrRegKey = "";
            try
            {
                userRootKey = Registry.CurrentUser.CreateSubKey(@"Software\HITOPS3\UserId");
                arrRegKey = userRootKey.GetValue("UserID").ToString();
            }
            catch
            {
                userRootKey = Registry.CurrentUser.OpenSubKey(@"Software\HITOPS3\UserId", true);
                userRootKey.SetValue("UserID", "");
            }
            finally
            {
                if ((arrRegKey != null))
                {
                    tbxUserId.Text = arrRegKey;
                }
            }
        }

        private void ChangeDownloadVariable(Boolean p_isDownload)
        {
            g_isDownload = p_isDownload;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.Image = Properties.Resources.login_btn_close_over;
            this.Cursor = Cursors.Hand;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.Image = Properties.Resources.login_btn_close;
            this.Cursor = Cursors.Default;
        }

        private void btnLogin_MouseHover(object sender, EventArgs e)
        {
            btnLogin.Image = Properties.Resources.login_btn_over;
            this.Cursor = Cursors.Hand;
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.Image = Properties.Resources.login_btn;
            this.Cursor = Cursors.Default;
        }

        private void timLoading_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.5;

            if (this.Opacity > 100)
            {
                timLoading.Stop();
            }
        }

        private void lblEnvironment_Click(object sender, EventArgs e)
        {
            frmEnvironment environment = new frmEnvironment();
            environment.ShowDialog();

            RefershDemoPrdNotice();
        }

        private void RefershDemoPrdNotice()
        {
            if (Hitops3Param.HITOPS3_PARAM.FRAMEWORK_SERVER_NAME == "SK_IT_DEV")
            {
                lblSystemName.Text = "Development Environment";
            }
            else
            {
                lblSystemName.Text = "";
            }
        }

        //2017.08.22 add by Ahn Jinsung.
        //Process.GetProcessesByName 에서는 32Bit로 실행되는 프로그램이 64bit 실행되는 프로세스 검색시 에러가나서 아래 함수 추가
        //public static int GetProcessInfoByPID(String sName, String sCommandLine)//, out string OwnerSID)
        //{
        //    int ProcessID = -1;
        //    string processname = String.Empty;
        //    try
        //    {
        //        ObjectQuery sq = new ObjectQuery("Select * from Win32_Process Where Name like '%" + sName + "%' and ExecutablePath like '%" + sCommandLine +"%'" );
        //        ManagementObjectSearcher searcher = new ManagementObjectSearcher(sq);
        //        if (searcher.Get().Count == 0)
        //            return -1;
        //        foreach (ManagementObject oReturn in searcher.Get())
        //        {
        //            ProcessID = Convert.ToInt32(oReturn["ProcessId"]);
        //            return ProcessID;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return ProcessID;
        //    }
        //    return ProcessID;
        //}
    }
}