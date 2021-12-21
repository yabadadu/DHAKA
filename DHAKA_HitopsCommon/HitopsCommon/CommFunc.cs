#region

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using Hitops;
using Hitops.exception;
using Hitops.user;
using System.Collections.Generic;
using CommonClass.Database.DBTable;
using CommonClass.Database.DBHandler;
using DevExpress.XtraBars.Ribbon;
using System.Reflection;
using System.Linq;
using HitopsCommon.Request;

#endregion

namespace HitopsCommon
{
    static public class CommFunc
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // 
        // HITOPS3 공통모듈
        //
        // 2007. 08. 13  권신희 작성
        //
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("wininet.dll")]                       // Network Connectin Status
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int n, int m);    // n은 주파수, m은 소리내는 시간(단위: 1/1000초)

        public class comboboxDisplayValue
        {
            private string mDisplay, mValue, mExpr;

            public comboboxDisplayValue(string pDisplay, string pValue)
            {
                mDisplay = pDisplay;
                mValue = pValue;
            }

            public comboboxDisplayValue(string pDisplay, string pValue, string pExpr)
            {
                mDisplay = pDisplay;
                mValue = pValue;
                mExpr = pExpr;
            }

            public override string ToString()
            {
                return mDisplay;
                //return mDisplay + " " + mValue;
            }

            public string DISPLAY { get { return mDisplay; } }
            public string VALUE { get { return mValue; } }
            public string EXPR { get { return mExpr; } }
        }

        //MDI MAIN창
        public static Form frmMainForm = null;

        public static String gloUserID = String.Empty;
        public static String gloUserPW = String.Empty;
        public static String gloSystemPrefix = String.Empty;
        public static String gloTMLCod = String.Empty;
        public static String gloTMLName = String.Empty;
        public static String gloUserTMLCod = String.Empty;
        public static String gloFrameworkServerName = String.Empty;
        public static String gloHostIP = String.Empty;
        public static String gloSystemCode = String.Empty;

        public static String gloCompanyCode = String.Empty; // 법인코드
        public static String gloCompanyName = String.Empty; // 법인명

        public static String gloUserType = String.Empty; // 유저타입 : 내부/외부
        public static String gloComCod = String.Empty;

        private static String g_ID = CommFunc.gloSystemPrefix + "18";

        private static String g_ins = "Insert";
        private static String g_inq = "Inquiry";
        private static String g_upd = "Update";
        private static String g_del = "Delete";

        public static String INS { get { return g_ins; } }
        public static String INQ { get { return g_inq; } }
        public static String UPD { get { return g_upd; } }
        public static String DEL { get { return g_del; } }

        private static int g_iAQConnectedCnt = 0;

        public static int[] iColumn = new int[100];  // 함수 내부에서 칼럼 개수 지정시 이벤트 발생할때마다 새로 설정되므로..

        public static String gloUserName = String.Empty;

        public static string gloDefaultCc = string.Empty;   // CC Code
        public static string gloDefaultCcNm = string.Empty; // CC Name

        public static string gloDefaultPu = string.Empty;   // PU Code
        public static string gloDefaultPuNm = string.Empty; // PU Name

        public static string gloDefaultSabun = string.Empty;   // Sabun Code
        public static string gloDefaultSabunNm = string.Empty; // Sabun Name

        /// <summary>
        /// Terminal의 고유 Port값을 리턴
        /// </summary>
        /// <param name="sTmnCod">해당터미널의 TMN_COD</param>
        /// <returns></returns>
        public static String getTmnPort(String sTmnCod)
        {
            TMNPORTCODE code = TMNPORTCODE.GetInstance();
            return code.getTmnPort(sTmnCod);
        }

        //공용 ProgressBar 관련
        public static System.Windows.Forms.ToolStripProgressBar prgProgBar;


        //공용 StatusBar 관련
        public static System.Windows.Forms.StatusStrip stbMain;
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarMsg;
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarStatus;
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarCapsLock;
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarNumLock;
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarDate;
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarTime;

        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarLoginID;        //로그인 ID
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarTML;            //터미널 코드
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarIP;             //접속IP
        public static System.Windows.Forms.ToolStripStatusLabel lblStatusBarAuth;           //권한

        //public delegate void UltraToolClickEventHandler(object sender, ToolClickEventArgs e);
        //public static UltraToolClickEventHandler ultraToolClickEventHandler = null;

        //// 공통 Print 처리
        //public static frmPrintOpts printopts = null;

        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static int AQConnectedCnt
        {
            get { return g_iAQConnectedCnt; }
            set { g_iAQConnectedCnt = value; }
        }

        /*************************************************************************************
         *                                 String 연산 관련 함수                             *
         *************************************************************************************/

        //스트링 형식으로 된 strNum에 iVal만큼의 연산(+/-)을 한다
        public static String StrPlus(String strNum, int iVal)
        {
            return Convert.ToString(ConvertToInt(strNum) + iVal);
        }

        public static void StrPlus(ref String strNum, int iVal)
        {
            strNum = Convert.ToString(ConvertToInt(strNum) + iVal);
        }

        // String 형식의 strNum과 String 형식의 strVal 을 연산
        // strType 이 "Int" 이면 Int32로 연산, strType 이 "Long"이면 Int64로 연산
        public static String StrCalculation(String strNum, String strVal, Char cOperator, String pStyle)
        {
            String strResult = "";

            try
            {
                if (pStyle.Equals(SetTextFormatType.INT))
                {
                    strResult = StrCalculation(strNum.Replace(",", ""), ConvertToInt(strVal.Replace(",", "")), cOperator);
                }
                else if (pStyle.Equals(SetTextFormatType.LONG))
                {
                    strResult = StrCalculation(strNum.Replace(",", ""), ConvertToLong(strVal.Replace(",", "")), cOperator);
                }
                else if (pStyle.Equals(SetTextFormatType.DOUBLE))
                {
                    strResult = StrCalculation(strNum.Replace(",", ""), ConvertToDouble(strVal.Replace(",", "")), cOperator);
                }
            }
            catch (FormatException fe)
            {
                String a = fe.Message;

                return "";
            }

            return strResult;
        }

        public static void StrCalculation(ref String strNum, String strVal, Char cOperator, String pStyle)
        {
            String strResult = "";
            try
            {
                if (pStyle.Equals(SetTextFormatType.INT))
                {
                    strResult = StrCalculation(strNum.Replace(",", ""), ConvertToInt(strVal.Replace(",", "")), cOperator);
                }
                else if (pStyle.Equals(SetTextFormatType.LONG))
                {
                    strResult = StrCalculation(strNum.Replace(",", ""), ConvertToLong(strVal.Replace(",", "")), cOperator);
                }
                else if (pStyle.Equals(SetTextFormatType.DOUBLE))
                {
                    strResult = StrCalculation(strNum.Replace(",", ""), ConvertToDouble(strVal.Replace(",", "")), cOperator);
                }
            }
            catch (FormatException fe)
            {
                string a = fe.Message;
                return;
            }

            strNum = strResult;
        }
        public static String StrCalculation(String strNum, int iVal, Char cOperator)
        {
            String strResult = "";

            switch (cOperator)
            {
                case '+':
                    strResult = Convert.ToString(ConvertToInt(strNum) + iVal);
                    break;
                case '-':
                    strResult = Convert.ToString(ConvertToInt(strNum) - iVal);
                    break;
                case '*':
                    strResult = Convert.ToString(ConvertToInt(strNum) * iVal);
                    break;
                case '/':
                    strResult = Convert.ToString(ConvertToInt(strNum) / iVal);
                    break;
                case '%':
                    strResult = Convert.ToString(ConvertToInt(strNum) % iVal);
                    break;
            }

            return strResult;
        }

        public static void StrCalculation(ref String strNum, int iVal, Char cOperator)
        {
            String strResult = "";

            switch (cOperator)
            {
                case '+':
                    strResult = Convert.ToString(ConvertToInt(strNum) + iVal);
                    break;
                case '-':
                    strResult = Convert.ToString(ConvertToInt(strNum) - iVal);
                    break;
                case '*':
                    strResult = Convert.ToString(ConvertToInt(strNum) * iVal);
                    break;
                case '/':
                    strResult = Convert.ToString(ConvertToInt(strNum) / iVal);
                    break;
                case '%':
                    strResult = Convert.ToString(ConvertToInt(strNum) % iVal);
                    break;
            }

            strNum = strResult;
        }

        public static String StrCalculation(String strNum, long lVal, Char cOperator)
        {
            String strResult = "";

            switch (cOperator)
            {
                case '+':
                    strResult = Convert.ToString(ConvertToLong(strNum) + lVal);
                    break;
                case '-':
                    strResult = Convert.ToString(ConvertToLong(strNum) - lVal);
                    break;
                case '*':
                    strResult = Convert.ToString(ConvertToLong(strNum) * lVal);
                    break;
                case '/':
                    strResult = Convert.ToString(ConvertToLong(strNum) / lVal);
                    break;
                case '%':
                    strResult = Convert.ToString(ConvertToLong(strNum) % lVal);
                    break;
            }

            return strResult;
        }

        public static void StrCalculation(ref String strNum, long lVal, Char cOperator)
        {
            String strResult = "";

            switch (cOperator)
            {
                case '+':
                    strResult = Convert.ToString(ConvertToLong(strNum) + lVal);
                    break;
                case '-':
                    strResult = Convert.ToString(ConvertToLong(strNum) - lVal);
                    break;
                case '*':
                    strResult = Convert.ToString(ConvertToLong(strNum) * lVal);
                    break;
                case '/':
                    strResult = Convert.ToString(ConvertToLong(strNum) / lVal);
                    break;
                case '%':
                    strResult = Convert.ToString(ConvertToLong(strNum) % lVal);
                    break;
            }

            strNum = strResult;
        }

        public static String StrCalculation(String strNum, double dVal, Char cOperator)
        {
            String strResult = "";

            switch (cOperator)
            {
                case '+':
                    strResult = Convert.ToString(ConvertToDouble(strNum) + dVal);
                    break;
                case '-':
                    strResult = Convert.ToString(ConvertToDouble(strNum) - dVal);
                    break;
                case '*':
                    strResult = Convert.ToString(ConvertToDouble(strNum) * dVal);
                    break;
                case '/':
                    strResult = Convert.ToString(ConvertToDouble(strNum) / dVal);
                    break;
                case '%':
                    strResult = Convert.ToString(ConvertToDouble(strNum) % dVal);
                    break;
            }

            return strResult;
        }

        public static void StrCalculation(ref String strNum, double dVal, Char cOperator)
        {
            String strResult = "";

            switch (cOperator)
            {
                case '+':
                    strResult = Convert.ToString(ConvertToDouble(strNum) + dVal);
                    break;
                case '-':
                    strResult = Convert.ToString(ConvertToDouble(strNum) - dVal);
                    break;
                case '*':
                    strResult = Convert.ToString(ConvertToDouble(strNum) * dVal);
                    break;
                case '/':
                    strResult = Convert.ToString(ConvertToDouble(strNum) / dVal);
                    break;
                case '%':
                    strResult = Convert.ToString(ConvertToDouble(strNum) % dVal);
                    break;
            }

            strNum = strResult;
        }

        /*************************************************************************************
         *                                   색상 관련 함수                                  *
         *************************************************************************************/

        //색상저장하고 있는 해쉬테이블에서 원하는 색상 뽑아내기(Color형 리턴)
        public static Color GetValidColor(Hashtable htColor, String strKey, Color mDefColor)
        {
            if ((strKey == null) || (!htColor.Contains(strKey)))
            {
                return mDefColor;
            }
            else
            {
                return ((Color)htColor[strKey]);
            }
        }

        //색상저장하고 있는 해쉬테이블에서 원하는 색상 뽑아내기(Brush형 리턴)
        public static Brush GetValidColor(Hashtable htColor, String strKey, Brush mDefColor)
        {
            if ((strKey == null) || (!htColor.Contains(strKey)))
            {
                return mDefColor;
            }
            else
            {
                return (new SolidBrush((Color)htColor[strKey]));
            }
        }

        /*************************************************************************************
         *                                 Progress Bar 관련 함수                            *
         *************************************************************************************/

        //ProgressBar 설정(컨트롤 설정)
        public static void SetProgressBar(System.Windows.Forms.ToolStripProgressBar _prgProgBar)
        {
            prgProgBar = _prgProgBar;
        }

        //ProgressBar 설정(최대값 설정)
        public static void SetProgressBar(int iMaxVal)
        {
            if (prgProgBar != null) prgProgBar.Maximum = iMaxVal;
        }

        //ProgressBar 보이게
        public static void ShowProgressBar()
        {
            if (prgProgBar != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                prgProgBar.Visible = true;
            }
        }

        //ProgressBar 안보이게
        public static void HideProgressBar()
        {
            if (prgProgBar != null)
            {
                Cursor.Current = Cursors.Default;
                prgProgBar.Visible = false;
            }
        }

        //ProgressBar 값 변경
        public static void AdjustProgressBar(int iVal)
        {
            if (prgProgBar != null)
            {
                if (iVal <= prgProgBar.Maximum) prgProgBar.Value = iVal;
            }
        }

        /*************************************************************************************
         *                                 Status Bar 관련 함수                              *
         *************************************************************************************/

        public static void AddStatusBar()
        {
            if (frmMainForm == null) return;

            stbMain = new System.Windows.Forms.StatusStrip();
            lblStatusBarMsg = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarStatus = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarCapsLock = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarNumLock = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarDate = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarTime = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarLoginID = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarTML = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarAuth = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatusBarIP = new System.Windows.Forms.ToolStripStatusLabel();

            // 
            // stbMain
            // 
            stbMain.AutoSize = false;
            stbMain.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            stbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            lblStatusBarMsg,
            lblStatusBarStatus,
            lblStatusBarLoginID,
            lblStatusBarTML,
            lblStatusBarAuth,
            lblStatusBarIP,
            lblStatusBarCapsLock,
            lblStatusBarNumLock,
            lblStatusBarDate,
            lblStatusBarTime});
            stbMain.Location = new System.Drawing.Point(0, 512);
            stbMain.Name = "stbMain";
            stbMain.Size = new System.Drawing.Size(987, 29);
            stbMain.TabIndex = 23;
            stbMain.Text = "statusStrip1";
            // 
            // lblStatusBarMsg
            // 
            lblStatusBarMsg.AutoSize = false;
            lblStatusBarMsg.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarMsg.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarMsg.Name = "lblStatusBarMsg";
            lblStatusBarMsg.Size = new System.Drawing.Size(467, 24);
            lblStatusBarMsg.Spring = true;
            lblStatusBarMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatusBarStatus
            // 
            lblStatusBarStatus.AutoSize = false;
            lblStatusBarStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarStatus.Name = "lblStatusBarStatus";
            lblStatusBarStatus.Size = new System.Drawing.Size(144, 24);
            lblStatusBarStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;



            // 
            // lblStatusBarLoginID
            // 
            lblStatusBarLoginID.AutoSize = false;
            lblStatusBarLoginID.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarLoginID.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarLoginID.Name = "lblStatusBarLoginID";
            lblStatusBarLoginID.Size = new System.Drawing.Size(90, 24);
            lblStatusBarLoginID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusBarTML
            // 
            lblStatusBarTML.AutoSize = false;
            lblStatusBarTML.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarTML.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarTML.Name = "lblStatusBarTML";
            lblStatusBarTML.Size = new System.Drawing.Size(90, 24);
            lblStatusBarTML.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusBarAuth
            // 
            lblStatusBarAuth.AutoSize = false;
            lblStatusBarAuth.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarAuth.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarAuth.Name = "lblStatusBarAuth";
            lblStatusBarAuth.Size = new System.Drawing.Size(60, 24);
            lblStatusBarAuth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusBarIP
            // 
            lblStatusBarIP.AutoSize = false;
            lblStatusBarIP.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarIP.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarIP.Name = "lblStatusBarIP";
            lblStatusBarIP.Size = new System.Drawing.Size(80, 24);
            lblStatusBarIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;


            // 
            // lblStatusBarCapsLock
            // 
            lblStatusBarCapsLock.AutoSize = false;
            lblStatusBarCapsLock.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarCapsLock.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarCapsLock.Name = "lblStatusBarCapsLock";
            lblStatusBarCapsLock.Size = new System.Drawing.Size(80, 24);
            lblStatusBarCapsLock.Text = "CAPSLOCK";
            // 
            // lblStatusBarNumLock
            // 
            lblStatusBarNumLock.AutoSize = false;
            lblStatusBarNumLock.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarNumLock.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarNumLock.Name = "lblStatusBarNumLock";
            lblStatusBarNumLock.Size = new System.Drawing.Size(80, 24);
            lblStatusBarNumLock.Text = "NUMLOCK";
            // 
            // lblStatusBarDate
            // 
            lblStatusBarDate.AutoSize = false;
            lblStatusBarDate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarDate.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarDate.Name = "lblStatusBarDate";
            lblStatusBarDate.Size = new System.Drawing.Size(90, 24);
            lblStatusBarDate.Text = "2007/09/01";
            // 
            // lblStatusBarTime
            // 
            lblStatusBarTime.AutoSize = false;
            lblStatusBarTime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            lblStatusBarTime.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            lblStatusBarTime.Name = "lblStatusBarTime";
            lblStatusBarTime.Size = new System.Drawing.Size(80, 24);
            lblStatusBarTime.Text = "08:30:12";

            frmMainForm.Controls.Add(stbMain);

            Application.Idle += new EventHandler(RefreshStatusBar);
        }

        //MDI MAIN에 Message출력
        public static void SetStatusBarMessage(String strMsg)
        {
            lblStatusBarMsg.Text = strMsg;
        }

        //MDI MAIN에 Status출력
        public static void SetStatusBarStatus(String strStatus)
        {
            lblStatusBarStatus.Text = strStatus;
        }

        //MDI MAIN의 Window메뉴 클릭시 현재 떠있는 창들 다 리스트업 해주는 함수
        public static void ShowWindowList(ToolStripMenuItem windowToolStripMenuItem)
        {
            int i;
            ToolStripMenuItem mWin;

            windowToolStripMenuItem.DropDownItems.Clear();

            mWin = new ToolStripMenuItem("&Cascade");
            mWin.Click += mWindowListClicked;
            windowToolStripMenuItem.DropDownItems.Add(mWin);

            mWin = new ToolStripMenuItem("&Horizontal");
            mWin.Click += mWindowListClicked;
            windowToolStripMenuItem.DropDownItems.Add(mWin);

            mWin = new ToolStripMenuItem("&Vertical");
            mWin.Click += mWindowListClicked;
            windowToolStripMenuItem.DropDownItems.Add(mWin);

            for (i = 0; i < Application.OpenForms.Count; i++)
            {
                if (Application.OpenForms[i].Text != frmMainForm.Text)
                {
                    //폼이 뜬게 있으면 정렬버튼들과 폼시작 사이에 Seperator추가
                    if (windowToolStripMenuItem.DropDownItems.Count == 3) windowToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

                    mWin = new ToolStripMenuItem(Application.OpenForms[i].Text, (Image)Application.OpenForms[i].Icon.ToBitmap());
                    mWin.Click += mWindowListClicked;
                    windowToolStripMenuItem.DropDownItems.Add(mWin);
                }
            }
        }

        //Context Menu Strip으로 추가 한경우..
        public static void ShowWindowList(ContextMenuStrip windowContextMenuItem)
        {
            int i;
            ToolStripMenuItem mWin;

            windowContextMenuItem.Items.Clear();

            mWin = new ToolStripMenuItem("&Cascade");
            mWin.Click += mWindowListClicked;
            windowContextMenuItem.Items.Add(mWin);

            mWin = new ToolStripMenuItem("&Horizontal");
            mWin.Click += mWindowListClicked;
            windowContextMenuItem.Items.Add(mWin);

            mWin = new ToolStripMenuItem("&Vertical");
            mWin.Click += mWindowListClicked;
            windowContextMenuItem.Items.Add(mWin);

            for (i = 0; i < Application.OpenForms.Count; i++)
            {
                if (Application.OpenForms[i].Text != frmMainForm.Text && Application.OpenForms[i].Text != "")
                {
                    //폼이 뜬게 있으면 정렬버튼들과 폼시작 사이에 Seperator추가
                    if (windowContextMenuItem.Items.Count == 3) windowContextMenuItem.Items.Add(new ToolStripSeparator());

                    //mWin = new ToolStripMenuItem(Application.OpenForms[i].Text, (Image)Application.OpenForms[i].Icon.ToBitmap());

                    mWin = new ToolStripMenuItem(Application.OpenForms[i].Text);
                    mWin.Click += mWindowListClicked;
                    windowContextMenuItem.Items.Add(mWin);
                }
            }
        }


        //Window 메뉴 클릭 이벤트(공개 아님)
        private static void mWindowListClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem mMenu = (ToolStripMenuItem)sender;
            int i;

            switch (mMenu.Text)
            {
                case "&Cascade":
                    frmMainForm.LayoutMdi(MdiLayout.Cascade);
                    break;

                case "&Horizontal":
                    frmMainForm.LayoutMdi(MdiLayout.TileHorizontal);
                    break;

                case "&Vertical":
                    frmMainForm.LayoutMdi(MdiLayout.TileVertical);
                    break;

                default:
                    for (i = 0; i < Application.OpenForms.Count; i++)
                    {
                        if (Application.OpenForms[i].Text == mMenu.Text)
                        {
                            Application.OpenForms[i].BringToFront();
                            return;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Set combobox from SY_CODE_DET
        /// </summary>
        /// <param name="sCodeCls">SY_CODE_DET.CODECLS</param>
        /// <param name="pCtrl">Code ComboBox</param>
        /// <param name="AllEnable">Use * or not</param>
        public static void SetCodeCombo(string sCodeCls, Object pCtrl, Boolean NullEnable, Boolean AllEnable)
        {
            try
            {
                List<CommonClass.Database.DBTable.SY_CODE_DET> codeList = CommonClass.Database.DBHandler.Handler_SY_CODE_DET.GetCodeDetail(gloFrameworkServerName, sCodeCls);
                if (codeList.Count < 1)
                    return;


                DataTable dt = new DataTable();
                DataColumn[] dc = new DataColumn[] { new DataColumn("CODE_NM"), new DataColumn("CODE") };
                dt.Columns.AddRange(dc);

                if (NullEnable)
                {
                    dt.Rows.Add(" ", "");
                }
                if (AllEnable)
                {
                    dt.Rows.Add("ALL", "%%%");
                }

                foreach (CommonClass.Database.DBTable.SY_CODE_DET codeDet in codeList)
                {
                    dt.Rows.Add(new string[] { codeDet.CODE_NM, codeDet.CODE });
                }

                if (pCtrl is ComboBoxEdit)
                {
                    ComboBoxEdit cmbList = (ComboBoxEdit)pCtrl;
                    cmbList.Properties.Items.Clear();
                    DevExpress.XtraEditors.Controls.ComboBoxItemCollection col = cmbList.Properties.Items;
                    col.BeginUpdate();
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            col.Add(new comboboxDisplayValue(row[0].ToString(), row[1].ToString()));
                        }
                    }
                    finally
                    {
                        col.EndUpdate();
                    }
                }
                else if (pCtrl is System.Windows.Forms.ComboBox)
                {
                    System.Windows.Forms.ComboBox cmbList = (System.Windows.Forms.ComboBox)pCtrl;
                    cmbList.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        cmbList.Items.Add(new comboboxDisplayValue(row[0].ToString(), row[1].ToString()));
                    }
                }
                else if (pCtrl is LookUpEdit)
                {
                    LookUpEdit cmbList = (LookUpEdit)pCtrl;
                    cmbList.Properties.DisplayMember = dt.Columns["CODE_NM"].ColumnName;
                    cmbList.Properties.ValueMember = dt.Columns["CODE"].ColumnName;
                    cmbList.Properties.DataSource = dt.DefaultView;
                }
                else if (pCtrl is RepositoryItemLookUpEdit)
                {
                    RepositoryItemLookUpEdit cmbList = (RepositoryItemLookUpEdit)pCtrl;
                    cmbList.DisplayMember = dt.Columns["CODE_NM"].ColumnName;
                    cmbList.ValueMember = dt.Columns["CODE"].ColumnName;
                    cmbList.DataSource = dt.DefaultView;
                }
                else if (pCtrl is RepositoryItemComboBox)
                {
                    RepositoryItemComboBox cmbList = (RepositoryItemComboBox)pCtrl;
                    cmbList.Items.Clear();
                    DevExpress.XtraEditors.Controls.ComboBoxItemCollection col = cmbList.Items;
                    col.BeginUpdate();
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            col.Add(new comboboxDisplayValue(row[0].ToString(), row[1].ToString()));
                        }
                    }
                    finally
                    {
                        col.EndUpdate();
                    }
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Block코드로 콤보박스 채움
        /// </summary>
        /// <param name="sTmnCod"></param>
        /// <param name="cmbCtrl"></param>
        public static void SetBlockCombo(String sTmnCod, object cmbCtrl)
        {
            string sCargoTyp = "%%%";
            SetBlockCombo(sTmnCod, cmbCtrl, sCargoTyp);
        }

        /// <summary>
        /// Block코드와 CargoType으로 콤보박스 채움
        /// </summary>
        /// <param name="sTmnCod"></param>
        /// <param name="cmbCtrl">콤보박스</param>
        /// <param name="sCargoTyp">Cargo Typ</param>
        public static void SetBlockCombo(String sTmnCod, object cmbCtrl, string sCargoTyp)
        {
            int iEqpFlag = 0;
            SetBlockCombo(sTmnCod, cmbCtrl, sCargoTyp, iEqpFlag);
        }

        /// <summary>
        /// Block코드와 CargoType, 장비타입으로 콤보박스 채움
        /// </summary>
        /// <param name="sTmnCod"></param>
        /// <param name="cmbCtrl">콤보박스</param>
        /// <param name="sCargoTyp">Cargo Typ</param>
        /// <param name="iEqpFlag">0:All, 1:RMG/RTG, 2:not RMG/RTG</param>
        public static void SetBlockCombo(String sTmnCod, object cmbCtrl, string sCargoTyp, int iEqpFlag)
        {
            Hashtable hTable = new Hashtable();
            hTable.Add("TMN_COD", sTmnCod);
            hTable.Add("CGO", sCargoTyp);
            hTable.Add("EQP_FLAG", iEqpFlag.ToString());
            try
            {
                ArrayList rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTBLOCKCODEBYTYPE", g_ID, hTable);
                foreach (Hashtable mRecord in rList)
                {
                    string strBlk = mRecord["YBLK"].ToString();
                    if (cmbCtrl is ComboBoxEdit)
                    {
                        (cmbCtrl as ComboBoxEdit).Properties.Items.Add(strBlk);
                    }
                    else if (cmbCtrl is CheckedComboBoxEdit)
                    {
                        (cmbCtrl as CheckedComboBoxEdit).Properties.Items.Add(strBlk);
                    }
                    else if (cmbCtrl is RepositoryItemComboBox)
                    {
                        (cmbCtrl as RepositoryItemComboBox).Items.Add(strBlk);
                    }
                    else if (cmbCtrl is System.Windows.Forms.ComboBox)
                    {
                        (cmbCtrl as System.Windows.Forms.ComboBox).Items.Add(strBlk);
                    }
                    else if (cmbCtrl is ToolStripComboBox)
                    {
                        (cmbCtrl as ToolStripComboBox).Items.Add(strBlk);
                    }
                }
            }
            catch (HMMException ex)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(ex.Result).Append("\n");
                builder.Append(ex.Message1).Append("\n");
                builder.Append(ex.Message2);
                MessageBox.Show(builder.ToString());
            }
        }

        public static void SetYearCombo(object cmbCtrl)
        {
            int iPreNum = -1;
            int iNextNum = -1;
            SetYearCombo(cmbCtrl, iPreNum, iNextNum, false);
        }

        /// <summary>
        /// Year체워주는 함수
        /// </summary>
        /// <param name="cmbCtrl">콤보박스</param>
        /// <param name="bAllTag">'*'입력여부</param>
        public static void SetYearCombo(object cmbCtrl, bool bAllTag)
        {
            int iPreNum = -1;
            int iNextNum = -1;
            SetYearCombo(cmbCtrl, iPreNum, iNextNum, bAllTag);
        }

        public static void SetYearCombo(object cmbCtrl, int iPreNum, int iNextNum)
        {
            SetYearCombo(cmbCtrl, iPreNum, iNextNum, false);
        }

        /// <summary>
        /// 최근년도 넣어주는 함수
        /// </summary>
        /// <param name="cmbCtrl">콤보박스</param>
        /// <param name="iPreNum">From년도횟수</param>
        /// <param name="iNextNum">To년도횟수</param>
        /// <param name="bAllTag">'*'입력여부</param>
        public static void SetYearCombo(object cmbCtrl, int iPreNum, int iNextNum, bool bAllTag)
        {
            try
            {
                if (bAllTag)
                {
                    if (cmbCtrl is ComboBoxEdit)
                    {
                        (cmbCtrl as ComboBoxEdit).Properties.Items.Add("*");
                    }
                    else if (cmbCtrl is CheckedComboBoxEdit)
                    {
                        (cmbCtrl as CheckedComboBoxEdit).Properties.Items.Add("*");
                    }
                    else if (cmbCtrl is RepositoryItemComboBox)
                    {
                        (cmbCtrl as RepositoryItemComboBox).Items.Add("*");
                    }
                    else if (cmbCtrl is System.Windows.Forms.ComboBox)
                    {
                        (cmbCtrl as System.Windows.Forms.ComboBox).Items.Add("*");
                    }
                    else if (cmbCtrl is ToolStripComboBox)
                    {
                        (cmbCtrl as ToolStripComboBox).Items.Add("*");
                    }
                }

                if (iPreNum == -1 && iNextNum == -1)
                {
                    /*************************************************************************************
                     *                    Berth Plan에 있는 년도를 넣어주는 경우                        *
                     *************************************************************************************/
                    ArrayList rList = null;
                    String strYear = "";
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-PLN-BTH-S-LSTBTHVVDYEAR", g_ID);
                    foreach (Hashtable mRecord in rList)
                    {
                        strYear = mRecord["VVD_YEAR"].ToString();
                        if (cmbCtrl is ComboBoxEdit)
                        {
                            (cmbCtrl as ComboBoxEdit).Properties.Items.Add(strYear);
                        }
                        else if (cmbCtrl is CheckedComboBoxEdit)
                        {
                            (cmbCtrl as CheckedComboBoxEdit).Properties.Items.Add(strYear);
                        }
                        else if (cmbCtrl is RepositoryItemComboBox)
                        {
                            (cmbCtrl as RepositoryItemComboBox).Items.Add(strYear);
                        }
                        else if (cmbCtrl is System.Windows.Forms.ComboBox)
                        {
                            (cmbCtrl as System.Windows.Forms.ComboBox).Items.Add(strYear);
                        }
                        else if (cmbCtrl is ToolStripComboBox)
                        {
                            (cmbCtrl as ToolStripComboBox).Items.Add(strYear);
                        }
                    }
                }
                else
                {
                    for (int i = DateTime.Now.Year + iNextNum; i >= DateTime.Now.Year - iPreNum; i--)
                    {
                        if (cmbCtrl is ComboBoxEdit)
                        {
                            (cmbCtrl as ComboBoxEdit).Properties.Items.Add(i);
                        }
                        else if (cmbCtrl is CheckedComboBoxEdit)
                        {
                            (cmbCtrl as CheckedComboBoxEdit).Properties.Items.Add(i);
                        }
                        else if (cmbCtrl is RepositoryItemComboBox)
                        {
                            (cmbCtrl as RepositoryItemComboBox).Items.Add(i);
                        }
                        else if (cmbCtrl is System.Windows.Forms.ComboBox)
                        {
                            (cmbCtrl as System.Windows.Forms.ComboBox).Items.Add(i);
                        }
                        else if (cmbCtrl is ToolStripComboBox)
                        {
                            (cmbCtrl as ToolStripComboBox).Items.Add(i);
                        }
                    }
                }
            }
            catch (HMMException ex)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(ex.Result).Append("\n");
                builder.Append(ex.Message1).Append("\n");
                builder.Append(ex.Message2);
                MessageBox.Show(builder.ToString());
            }
        }
        
        public static void GetGeneralCode(Object pCtrl, String pType, bool pAllFlag, bool pBlankFlag, params String[] strWhere)
        {
            ArrayList rList = null;
            Hashtable table = new Hashtable();
            String strFieldName = "";

            try
            {
                if(strWhere.Length > 0) table["TMN_COD"] = strWhere[0];

                if (pType.Equals(GetGeneralCodeType.MASH_GRP))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-PLN-YRD-S-LSTMASHGRP", g_ID, table);
                    strFieldName = "MASH_GRP_ID";
                }
                else if (pType.Equals(GetGeneralCodeType.BITT))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTBITT_COM", g_ID, table);
                    strFieldName = "BTT_NO";
                }
                else if (pType.Equals(GetGeneralCodeType.AGS_BASIC_CATEGORY))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTAGSBASICCATELIST_COM", g_ID, table);
                    strFieldName = "CAT_COD";
                }
                else if (pType.Equals(GetGeneralCodeType.DAMAGE))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCNTRDAMAGECODE", g_ID);
                    strFieldName = "DMG_COD";
                }
                else if (pType.Equals(GetGeneralCodeType.BLOCK))
                {
                    if (strWhere.Length > 1) table["TYPE_CGO_PRI"] = strWhere[1];
                    else table["TYPE_CGO_PRI"] = "%%%";

                    if (strWhere.Length > 2) table["VIRTUAL_USE_TAG"] = strWhere[2];
                    else table["VIRTUAL_USE_TAG"] = "%%%";

                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTBLOCK_COM", g_ID, table);
                    strFieldName = "YBLK";
                }
                else if (pType.Equals(GetGeneralCodeType.BAY))
                {
                    table["YBLK"] = strWhere[1];

                    if (strWhere.Length > 2) table["TYPE_CGO_PRI"] = strWhere[2];
                    else table["TYPE_CGO_PRI"] = "%%%";

                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTBAYBYBLOCK", g_ID, table);
                    strFieldName = "YBAY";
                }
                else if (pType.Equals(GetGeneralCodeType.SERVICE_AREA))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-GETSERVICEAREANO", g_ID, table);
                    strFieldName = "SA_NO";
                }
                else if (pType.Equals(GetGeneralCodeType.LANE))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-GETLANENO", g_ID, table);
                    strFieldName = "LANE_NO";
                }
                else if (pType.Equals(GetGeneralCodeType.BERTH))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-GETBERTHNO", g_ID, table);
                    strFieldName = "BTH_NO";
                }
                else if (pType.Equals(GetGeneralCodeType.YT))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-OPR-YTP-S-LSTYT_COM", g_ID, table);
                    strFieldName = "YT_NO";
                }
                else if (pType.Equals(GetGeneralCodeType.QC))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTQC_COM", g_ID, table);
                    strFieldName = "EQP_NO";
                }
                else if (pType.Equals(GetGeneralCodeType.TRUCKER))
                {
                    table["TRK_CO"] = "%";
                    table["CO_DESC"] = "%";
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-GTE-COD-J-LSTTRKCOINFO", g_ID, table);
                    strFieldName = "TRK_CO";
                }
                else if (pType.Equals(GetGeneralCodeType.VVD))
                {
                    table["VVD_YEAR"] = strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-PLN-SHP-S-LSTVVD_BYYEAR", g_ID, table);
                    strFieldName = "VVD";
                }
                else if (pType.Equals(GetGeneralCodeType.VVDNEW))
                {
                    table["VVD_YEAR"] = strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-PLN-SHP-S-LSTVVD_BYYEAR2", g_ID, table);
                    strFieldName = "VVD";
                }
                else if (pType.Equals(GetGeneralCodeType.VVDACT))
                {
                    table["VVD_YEAR"] = strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-PLN-SHP-S-LSTVVD_BYYEAR3", g_ID, table);
                    strFieldName = "VVD";
                }
                else if (pType.Equals(GetGeneralCodeType.VVDACTALL))
                {
                    table["VVD_YEAR"] = strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-PLN-SHP-S-LSTVVD_BYYEAR4", g_ID, table);
                    strFieldName = "VVD";
                }
                else if (pType.Equals(GetGeneralCodeType.VVD_LATEST))
                {
                    table["VVD_YEAR"] = strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-PLN-SHP-S-LSTLATESTVVD_BYYEAR", g_ID, table);
                    strFieldName = "VVD";
                }
                else if (pType.Equals(GetGeneralCodeType.VESSEL))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTVESSELCOD_COM", g_ID);
                    strFieldName = "VSL_COD";
                }
                else if (pType.Equals(GetGeneralCodeType.PORT))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTPORTCODE_COM", g_ID);
                    strFieldName = "PORT";
                }
                else if (pType.Equals(GetGeneralCodeType.PORT_ONLY))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTPORTCODE_COM2", g_ID);
                    strFieldName = "PORT_ONLY";
                }
                else if (pType.Equals(GetGeneralCodeType.PORT_BY_ROUTE))
                {
                    table["ROUTE"] = strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTPORTCODEBYROUTE_COD", g_ID, table);
                    strFieldName = "PORT";
                }
                else if (pType.Equals(GetGeneralCodeType.PORT_BY_VVD))
                {
                    table["VVD_YEAR"] = strWhere[1];
                    table["VVD"] = strWhere[2];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTPORTBYVVD_COM", g_ID, table);
                    strFieldName = "PORT";
                }
                else if (pType.Equals(GetGeneralCodeType.ROUTE))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTROUTECODE_COM", g_ID);
                    strFieldName = "ROUTE";
                }
                else if (pType.Equals(GetGeneralCodeType.ROUTEGROUP))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTROUTEGRPCODE_COM", g_ID);
                    strFieldName = "ROUTE_GRP";
                }
                else if (pType.Equals(GetGeneralCodeType.NATION))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTNATIONCODE_COM", g_ID);
                    strFieldName = "NAT";
                }
                else if (pType.Equals(GetGeneralCodeType.OPERATOR))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTOPRCODE_COM", g_ID);
                    strFieldName = "OPR";
                }
                else if (pType.Equals(GetGeneralCodeType.ACT_OPR))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTACTOPRCOD_COM", g_ID);
                    strFieldName = "ACT_OPR";
                }
                else if (pType.Equals(GetGeneralCodeType.CNTR_SIZE))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCNTRSIZECODE", g_ID);
                    strFieldName = "CNTR_SIZ";
                }
                else if (pType.Equals(GetGeneralCodeType.CNTR_TYPE))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCNTRTYPECODE", g_ID);
                    strFieldName = "CNTR_TYP";
                }
                else if (pType.Equals(GetGeneralCodeType.CARGO_TYPE))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCARGOTYPECODE", g_ID);
                    strFieldName = "CARGO_TYP";
                }
                else if (pType.Equals(GetGeneralCodeType.ISO_SZTP))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTISOCODE_COM", g_ID);
                    strFieldName = "ISO_COD";
                }
                else if (pType.Equals(GetGeneralCodeType.LINER_SZTP))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTLINERCODE_COM", g_ID);
                    strFieldName = "LNR_COD";
                }
                else if (pType.Equals(GetGeneralCodeType.CYS_OPR))
                {
                    //-- CYS_OPR Terminal Code Parameter 적용
                    //rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CYS-COD-S-LSTCYSOPR", g_ID);
                    if (table.ContainsKey("TMN_COD"))
                    {
                        rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CYS-COD-S-LSTCYSOPR", g_ID, table);
                    }
                    else
                    {
                        rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CYS-COD-S-LSTCYSOPR", g_ID);
                    }
                    strFieldName = "CNTR_OPR";
                }
                else if (pType.Equals(GetGeneralCodeType.CY_USAGE_COD))
                {
                    table["ON_DOCK_TAG"] = strWhere[1].Equals("*") ? "%%%" : strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCYUSAGECOD_COM", g_ID, table);
                    strFieldName = "CY_USAGE_COD";
                }
                else if (pType.Equals(GetGeneralCodeType.CURRENCY))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCURRENCY_COM", g_ID);
                    strFieldName = "CURRENCY";
                }
                else if (pType.Equals(GetGeneralCodeType.POOL_SCHE_EQP_TYP))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-OPR-YTP-S-LSTSCHDTRGTKND_COM", g_ID, table);
                    strFieldName = "SCHD_TRGT_KND";
                }
                else if (pType.Equals(GetGeneralCodeType.POOL_SCHE_EQP_NO))
                {
                    table["SCHD_TRGT_KND"] = strWhere[1];
                    table["START_DTE"] = strWhere[2];
                    table["END_DTE"] = strWhere[3];

                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-OPR-YTP-S-LSTSCHDTRGT_COM", g_ID, table);
                    strFieldName = "SCHD_TRGT";
                }
                else if (pType.Equals(GetGeneralCodeType.TARIFF_DUE_TYP))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTTARIFFDUETYP_COM", g_ID);
                    strFieldName = "TARIFF_DUE_TYP";
                }
                else if (pType.Equals(GetGeneralCodeType.CNTR_HOLD))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCNTRHOLDCODE_COM", g_ID);
                    strFieldName = "HOLD_COD";
                }
                else if (pType.Equals(GetGeneralCodeType.GATE_NO))
                {
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTGATE_COM", g_ID);
                    strFieldName = "GAT_NO";
                }
                else if (pType.Equals(GetGeneralCodeType.VESSEL_BAY))
                {
                    table["VSL_COD"] = strWhere[1];
                    rList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTVSLBAY_COM", g_ID, table);
                    strFieldName = "VBAY";
                }


                if (pCtrl is ComboBoxEdit)
                {
                    ComboBoxEdit cmbList = (ComboBoxEdit)pCtrl;

                    cmbList.Properties.Items.Clear();
                    if (pAllFlag) cmbList.Properties.Items.Add("*");
                    if (pBlankFlag) cmbList.Properties.Items.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Properties.Items.Add(mRecord[strFieldName].ToString().Trim());
                }
                else if (pCtrl is CheckedComboBoxEdit)
                {
                    CheckedComboBoxEdit cmbList = (CheckedComboBoxEdit)pCtrl;

                    cmbList.Properties.Items.Clear();
                    if (pAllFlag) cmbList.Properties.Items.Add("*");
                    if (pBlankFlag) cmbList.Properties.Items.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Properties.Items.Add(mRecord[strFieldName].ToString().Trim());
                }
                else if (pCtrl is RepositoryItemComboBox)
                {
                    RepositoryItemComboBox cmbList = (RepositoryItemComboBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Items.Add(mRecord[strFieldName].ToString().Trim());
                }
                else if (pCtrl is System.Windows.Forms.ComboBox)
                {
                    System.Windows.Forms.ComboBox cmbList = (System.Windows.Forms.ComboBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Items.Add(mRecord[strFieldName].ToString().Trim());
                }
                else if (pCtrl is ToolStripComboBox)
                {
                    ToolStripComboBox cmbList = (ToolStripComboBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Items.Add(mRecord[strFieldName].ToString().Trim());
                }
                else if (pCtrl is ListBox)
                {
                    ListBox cmbList = (ListBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Items.Add(mRecord[strFieldName].ToString().Trim());
                }
                else if (pCtrl is ListView)
                {
                    ListView cmbList = (ListView)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Items.Add(mRecord[strFieldName].ToString().Trim());
                }
                else if (pCtrl is ArrayList)
                {
                    ArrayList cmbList = (ArrayList)pCtrl;

                    cmbList.Clear();
                    if (pAllFlag) cmbList.Add("*");
                    if (pBlankFlag) cmbList.Add(" ");

                    foreach (Hashtable mRecord in rList) cmbList.Add(mRecord[strFieldName].ToString());
                }
                //else if (pCtrl is FarPoint.Win.Spread.CellType.ComboBoxCellType)
                //{
                //    FarPoint.Win.Spread.CellType.ComboBoxCellType comboType = (FarPoint.Win.Spread.CellType.ComboBoxCellType)pCtrl;
                //    int iCnt = rList.Count;
                //    int k = 0;

                //    if (pAllFlag) iCnt++;
                //    if (pBlankFlag) iCnt++;

                //    String[] mComboStr = new String[iCnt];

                //    if (pBlankFlag) mComboStr[k++] = " ";
                //    if (pAllFlag) mComboStr[k++] = "*";
                //    foreach (Hashtable mRecord in rList) mComboStr[k++] = mRecord[strFieldName].ToString();

                //    comboType.Items = mComboStr;
                //}
            }
            catch (HMMException ex)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(ex.Result).Append("\n");
                builder.Append(ex.Message1).Append("\n");
                builder.Append(ex.Message2);
                MessageBox.Show(builder.ToString());
            }
        }

        //********************************************************************************************
        //* 2010.03.17 SmileMan 추가
        //* DESC : ComboBox Display와 Value를 세팅 (Combo Item Blank 기능 추가)
        //********************************************************************************************
        /// <summary>
        /// ComboBox Item설정
        /// </summary>
        /// <param name="pCtrl">ComboBox Object</param>
        /// <param name="pType">ITEM Name</param>
        /// <param name="strDisplayColumn">ComboBox Display Column Name</param>
        /// <param name="strValueColumn">ComboBox Value Column Name</param>
        /// <param name="bAllFlag">AllFlag Display True/False</param>
        /// <param name="bBlankFlag">Blank Item Display true/False</param>
        public static void SetComboBox(Object pCtrl, String pType, String strDisplayColumn, String strValueColumn, Boolean bAllFlag, Boolean bBlankFlag)
        {
            DataTable dt = new DataTable();
            DataColumn[] dc = new DataColumn[] { new DataColumn(strDisplayColumn), new DataColumn(strValueColumn) };
            dt.Columns.AddRange(dc);

            ArrayList arrList = new ArrayList();

            switch (pType)
            {
                case "CNTR_GRADE":

                    arrList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTCNTRGRADECODE", g_ID);

                    if (bAllFlag) dt.Rows.Add(new string[] { "*", "" });

                    if (bBlankFlag) dt.Rows.Add(new string[] { " ", "" });

                    foreach (Hashtable ht in arrList)
                    {
                        dt.Rows.Add(new string[] { ht[strDisplayColumn].ToString(), ht[strValueColumn].ToString() });
                    }

                    break;

                case "INSPCOD":

                    Hashtable htParam = new Hashtable();
                    htParam["COD_TYP"] = pType.ToUpper();

                    arrList = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTMAINCODE", g_ID, htParam);

                    if (bAllFlag) dt.Rows.Add(new string[] { "*", "" });

                    if (bBlankFlag) dt.Rows.Add(new string[] { " ", "" });

                    foreach (Hashtable ht in arrList)
                    {
                        dt.Rows.Add(new string[] { ht[strDisplayColumn].ToString(), ht[strValueColumn].ToString() });
                    }

                    break;
            }

            if (pCtrl is LookUpEdit)
            {
                LookUpEdit cmbList = (LookUpEdit)pCtrl;
                cmbList.Properties.ValueMember = dt.Columns[strValueColumn].ColumnName;
                cmbList.Properties.DisplayMember = dt.Columns[strDisplayColumn].ColumnName;
                cmbList.Properties.DataSource = dt.DefaultView;
            }
            else if (pCtrl is System.Windows.Forms.ComboBox)
            {
                System.Windows.Forms.ComboBox cmbList = (System.Windows.Forms.ComboBox)pCtrl;
                cmbList.ValueMember = dt.Columns[strValueColumn].ColumnName;
                cmbList.DisplayMember = dt.Columns[strDisplayColumn].ColumnName;
                cmbList.DataSource = dt.DefaultView;
            }
        }

        //********************************************************************************************
        //* 2009.12.17 SmileMan 추가
        //* DESC : ComboBox Display와 Value를 세팅
        //********************************************************************************************

        #region ## 추가된 Source ##
        /// <summary>
        /// ComboBox Item설정
        /// </summary>
        /// <param name="pCtrl">ComboBox Object</param>
        /// <param name="pType">ITEM Name</param>
        /// <param name="strDisplayColumn">ComboBox Display Column Name</param>
        /// <param name="strValueColumn">ComboBox Value Column Name</param>
        /// <param name="bAllFlag">AllFlag Display True/False</param>
        /// <remarks>
        /// <example>
        ///     <code>
        ///         SetComboBox(this.cmbGrade, GetGeneralCodeType.CNTR_GRADE, "DESCRIPTION", "CNTR_GRADE", false);
        ///     </code>
        /// </example>
        /// </remarks>
        public static void SetComboBox(Object pCtrl, String pType, String strDisplayColumn, String strValueColumn, Boolean bAllFlag)
        {
            SetComboBox(pCtrl, pType, strDisplayColumn, strValueColumn, bAllFlag, false);
        }

        #endregion

        //********************************************************************************************

        //코드 정보 읽기(MAIN)
        public static Hashtable GetMainCode(Object pCtrl, String pType, bool pAllFlag, bool pBlankFlag, String pGetMainCodeItemType, ArrayList arlstCOD)
        {
            return GetMainCode(pCtrl, pType, pAllFlag, pBlankFlag, pGetMainCodeItemType, arlstCOD, GetMainCodeOpt.DISPLAY_SEQ);
        }

        public static Hashtable GetMainCode(Object pCtrl, String pType, bool pAllFlag, bool pBlankFlag, String pGetMainCodeItemType, ArrayList arlstCOD, String pGetMainCodeOpt)
        {
            ArrayList rList = null;
            Hashtable htReturn = null;
            Hashtable table = new Hashtable();
            if (pGetMainCodeOpt.Equals(""))
            {
                pGetMainCodeOpt = GetMainCodeOpt.DISPLAY_SEQ;
            }
            //옵션에 따라 컨트롤에 코드를 넣을것인지, 코드설명을 넣을것인지 선택

            try
            {
                table["COD_TYP"] = pType.ToUpper();
                if (pGetMainCodeOpt.Equals(GetMainCodeOpt.DISPLAY_SEQ))
                {
                    rList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTMAINCODE", g_ID, table);
                }
                else if (pGetMainCodeOpt.Equals(GetMainCodeOpt.MAIN_COD))
                {
                    rList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-CDS-COD-S-LSTMAINCODE_SORTNAME", g_ID, table);
                }

                //ArrayList에는 무조건 CODE를 넣어서 준다
                if (arlstCOD != null)   //ArrayList가 필요없는 경우에는 null로 넘어오므로 체크
                {
                    arlstCOD.Clear();

                    if (pAllFlag) arlstCOD.Add("%%%");
                    if (pBlankFlag) arlstCOD.Add(" ");

                    foreach (Hashtable mRecord in rList) arlstCOD.Add(mRecord["MAIN_COD"].ToString());
                }

                //HashTable로 코드를 받고 싶을 경우를 위해 리턴은 Hashtable로!!!
                htReturn = new Hashtable();
                foreach (Hashtable mRecord in rList) htReturn[mRecord["MAIN_COD"].ToString()] = mRecord["COD_NAME"].ToString();



                //각각 컨트롤에 코드 혹은 설명 ADD
                if (pCtrl is ComboBoxEdit)
                {
                    ComboBoxEdit cmbList = (ComboBoxEdit)pCtrl;

                    cmbList.Properties.Items.Clear();
                    if (pAllFlag) cmbList.Properties.Items.Add("*");
                    if (pBlankFlag) cmbList.Properties.Items.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                        cmbList.Properties.Items.Add(sItem);
                    }
                }
                else if (pCtrl is CheckedComboBoxEdit)
                {
                    CheckedComboBoxEdit cmbList = (CheckedComboBoxEdit)pCtrl;

                    cmbList.Properties.Items.Clear();
                    if (pAllFlag) cmbList.Properties.Items.Add("*");
                    if (pBlankFlag) cmbList.Properties.Items.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                        cmbList.Properties.Items.Add(sItem);
                    }
                }
                else if (pCtrl is RepositoryItemComboBox)
                {
                    RepositoryItemComboBox cmbList = (RepositoryItemComboBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                        cmbList.Items.Add(sItem);
                    }
                }
                else if (pCtrl is System.Windows.Forms.ComboBox)
                {
                    System.Windows.Forms.ComboBox cmbList = (System.Windows.Forms.ComboBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);
                        cmbList.Items.Add(sItem);
                    }
                }
                else if (pCtrl is ToolStripComboBox)
                {
                    ToolStripComboBox cmbList = (ToolStripComboBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                        cmbList.Items.Add(sItem);
                    }
                }
                else if (pCtrl is ListBox)
                {
                    ListBox cmbList = (ListBox)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                        cmbList.Items.Add(sItem);
                    }
                }
                else if (pCtrl is ListView)
                {
                    ListView cmbList = (ListView)pCtrl;

                    cmbList.Items.Clear();
                    if (pAllFlag) cmbList.Items.Add("*");
                    if (pBlankFlag) cmbList.Items.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                        cmbList.Items.Add(sItem);
                    }
                }
                //else if (pCtrl is FarPoint.Win.Spread.CellType.ComboBoxCellType)
                //{
                //    FarPoint.Win.Spread.CellType.ComboBoxCellType comboType = (FarPoint.Win.Spread.CellType.ComboBoxCellType)pCtrl;
                //    int iCnt = rList.Count;
                //    int k = 0;

                //    if (pAllFlag) iCnt++;
                //    if (pBlankFlag) iCnt++;

                //    String[] mComboStr = new String[iCnt];

                //    if (pAllFlag) mComboStr[k++] = "*";
                //    if (pBlankFlag) mComboStr[k++] = " ";

                //    foreach (Hashtable mRecord in rList)
                //    {
                //        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                //        mComboStr[k++] = sItem;
                //    }

                //    comboType.Items = mComboStr;
                //}
                else if (pCtrl is ArrayList)
                {
                    ArrayList aCtrl = (ArrayList)pCtrl;

                    aCtrl.Clear();

                    if (pAllFlag) aCtrl.Add("*");
                    if (pBlankFlag) aCtrl.Add(" ");

                    foreach (Hashtable mRecord in rList)
                    {
                        String sItem = GetCodeString(pGetMainCodeItemType, mRecord);

                        aCtrl.Add(sItem);
                    }
                }
            }
            catch (HMMException ex)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(ex.Result).Append("\n");
                builder.Append(ex.Message1).Append("\n");
                builder.Append(ex.Message2);
                MessageBox.Show(builder.ToString());
            }

            return htReturn;
        }

        private static string GetCodeString(string pGetMainCodeItemType, Hashtable mRecord)
        {
            String sItem = String.Empty;

            if (pGetMainCodeItemType == GetMainCodeItemType.MAIN_COD)
                sItem = mRecord["MAIN_COD"].ToString();
            else if (pGetMainCodeItemType == GetMainCodeItemType.COD_NAME)
                sItem = mRecord["COD_NAME"].ToString();
            else if (pGetMainCodeItemType == GetMainCodeItemType.COD_DESC)
                sItem = mRecord["COD_DESC"].ToString();
            else if (pGetMainCodeItemType == GetMainCodeItemType.COD_NAME_DESC)
                sItem = mRecord["COD_NAME"].ToString() + " : " + mRecord["COD_DESC"].ToString();
            else if (pGetMainCodeItemType == GetMainCodeItemType.MAIN_COD_DESC)
                sItem = mRecord["MAIN_COD"].ToString() + " : " + mRecord["COD_DESC"].ToString();
            return sItem;
        }

        /*************************************************************************************
         *                                  Registry 관련 함수                               *
         *************************************************************************************/

        //윈도우 위치,크기 레지스트리로 저장
        public static void SaveWindowLocation(Form pWindow)
        {
            //if (pWindow.WindowState == FormWindowState.Normal)      //최소 모드와 최대 모드는 좌표상 문제가 있으므로 저장하지 않는다!
            //{
            //    RegistryKey userRootKey = null;
            //    userRootKey = Registry.CurrentUser.OpenSubKey(@"Software\HITOPS3\WindowLocation", true);

            //    string[] arrRegKey = new string[] { pWindow.Left.ToString(), pWindow.Top.ToString(), pWindow.Width.ToString(), pWindow.Height.ToString() };
            //    userRootKey.SetValue(pWindow.Name, arrRegKey);
            //}
        }

        //윈도우 위치,크기 레지스트리에서 불러오기
        public static void LoadWindowLocation(Form pWindow)
        {
            //if (pWindow.WindowState == FormWindowState.Normal)      //최소 모드와 최대 모드는 좌표상 문제가 있으므로 저장하지 않는다!
            //{
            //    LoadWindowLocation(pWindow, pWindow.Left, pWindow.Top, pWindow.Width, pWindow.Height);
            //}
        }

        //윈도우 위치,크기 레지스트리에서 불러오기(크기 지정가능)
        public static void LoadWindowLocation(Form pWindow, int iLeft, int iTop, int iWidth, int iHeight)
        {
            //RegistryKey userRootKey = null;
            //userRootKey = Registry.CurrentUser.CreateSubKey(@"Software\HITOPS3\WindowLocation");

            //string[] arrRegKey = (string[])userRootKey.GetValue(pWindow.Name);

            ////올바르게 저장된놈이면 위치,크기 변경
            //if ((arrRegKey != null) && (arrRegKey.Length == 4))
            //{
            //    pWindow.Left = Convert.ToInt32(arrRegKey[0]);
            //    pWindow.Top = Convert.ToInt32(arrRegKey[1]);
            //    pWindow.Width = Convert.ToInt32(arrRegKey[2]);
            //    pWindow.Height = Convert.ToInt32(arrRegKey[3]);
            //}
            //else //아니면 지정된 사이즈로
            //{
            //    pWindow.Left = iLeft;
            //    pWindow.Top = iTop;
            //    pWindow.Width = iWidth;
            //    pWindow.Height = iHeight;
            //}
        }

        /*************************************************************************************
         *                                 Login Data 관련 함수                              *
         *************************************************************************************/

        //프로그램 실행시 넘겨받은 파라미터 검사
        public static bool CheckLoginData(String[] pArgs)
        {

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Network Connection down. Watch your Network.");
                return false;
            }

            if ((pArgs == null) || (pArgs.Length != 7))
            {
                MessageBox.Show("Invalid Parameter", "HI-TOPS III", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            g_ID = pArgs[2] + "28101";

            UserInfoSingleton instance = UserInfoSingleton.GetInstance();
            instance.Userid = pArgs[0];

            gloUserID = pArgs[0];     // 로그인 ID
            gloUserPW = pArgs[1];     // 로그인 Password
            gloSystemPrefix = pArgs[2];     // System prefix
            gloTMLCod = pArgs[3];     // 터미널 코드
            gloUserTMLCod = gloTMLCod;
            gloFrameworkServerName = pArgs[4];     // Framework Server 이름
            gloHostIP = pArgs[5];     // 호스트 IP
            gloSystemCode = pArgs[6];     // System Code
            //_FtpIP                  = pArgs[7];     // FTP IP
            //_FtpUserId              = pArgs[8];     // FTP User ID
            //_FtpPwd                 = pArgs[9];     // FTP Password
            //_FtpEXEDir              = pArgs[10];    // FTP 내 EXE Directory
            //_FtpRelatedFileDir      = pArgs[11];    // FTP 내 Related File Directory
            //_LocVerInfoDir          = pArgs[12];    // Local의 File Version 정보 가진 파일의 Directory
            //_LocVerInfoFile         = pArgs[13];    // Local의 File Version 정보 가진 파일의 Name
            //_LocVerEXESection       = pArgs[14];    // Local의 File Version 정보내에 EXE 파일 Section
            //_LocVerRelatedSection   = pArgs[15];    // Local의 File Version 정보내에 Related 파일 Section

            return true;
        }

        //윈도우 open 함수 (기존에 폼이 있으면 상위로 뛰우고 없으면 새로 연다.)
        public static Form CheckLoadFormOLD<TForm>() where TForm : Form, new()
        {
            return CheckLoadFormOLD<TForm>(true, true, true);
        }
        public static Form CheckLoadFormOLD<TForm>(Boolean blnShow) where TForm : Form, new()
        {
            return CheckLoadFormOLD<TForm>(blnShow, true, true);
        }
        //윈도우 open 함수 (기존에 폼이 있으면 상위로 뛰우고 없으면 새로 연다.)
        public static Form CheckLoadFormOLD<TForm>(Boolean blnShow, Boolean bIsSubForm, Boolean bOnlyOne) where TForm : Form, new()
        {
            if (bOnlyOne)
            {
                foreach (Form theForm in Application.OpenForms)
                {
                    if (theForm is TForm)  //해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
                    {
                        if (blnShow)
                        {
                            theForm.BringToFront();
                            theForm.WindowState = FormWindowState.Normal;
                            theForm.Focus();
                        }
                        return theForm;
                    }
                }
            }

            TForm tf = new TForm();

            if (bIsSubForm)
                tf.MdiParent = frmMainForm;

            if (blnShow) tf.Show();

            return tf;
        }

        public static Form CheckLoadForm(Form form)
        {
            return CheckLoadForm(form, true, true, true);
        }

        public static Form CheckLoadForm(Form form, Boolean blnShow)
        {
            return CheckLoadForm(form, blnShow, true, true);
        }

        public static Form CheckLoadForm(Form form, Boolean blnShow, Boolean bIsSubForm, Boolean bOnlyOne) 
        {
            if (bOnlyOne)
            {
                foreach (Form theForm in Application.OpenForms)
                {
                    if (theForm.Name == form.Name)  //해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
                    {
                        if (blnShow)
                        {
                            theForm.BringToFront();
                            theForm.WindowState = FormWindowState.Normal;
                            theForm.Focus();
                        }
                        return theForm;
                    }
                }
            }
            
            if (bIsSubForm)
                form.MdiParent = frmMainForm;

            if (blnShow) form.Show();

            return form;
        }

        // parameter 내 Input Control을 Disable 시킴
        public static void DisableInputControls(object pObj)
        {
            foreach (Control tCtrl in ((Control)pObj).Controls)
            {
                if (tCtrl is Panel ||
                   tCtrl is GroupBox ||
                   tCtrl is GroupControl ||
                   tCtrl is TabControl ||
                   tCtrl is TabPage)
                {
                    DisableInputControls(tCtrl);
                }
                else if (tCtrl is TextBox ||
                         tCtrl is System.Windows.Forms.ComboBox ||
                         tCtrl is CheckBox ||
                         tCtrl is DateTimePicker ||
                         tCtrl is ComboBoxEdit ||
                         tCtrl is CheckedComboBoxEdit)
                {
                    tCtrl.Enabled = false;
                }
            }
        }

        public static void EnableInputControls(object pObj)
        {
            foreach (Control tCtrl in ((Control)pObj).Controls)
            {
                if (tCtrl is Panel ||
                   tCtrl is GroupBox ||
                   tCtrl is TabControl ||
                   tCtrl is TabPage)
                {
                    EnableInputControls(tCtrl);
                }
                else if (tCtrl is TextBox ||
                         tCtrl is System.Windows.Forms.ComboBox ||
                         tCtrl is CheckBox ||
                         tCtrl is DateTimePicker ||
                         tCtrl is ComboBoxEdit ||
                         tCtrl is CheckedComboBoxEdit)
                {
                    tCtrl.Enabled = true;
                }
            }
        }

        // Information Manager Form을 체크하는 함수
        // 외부 클래스에서 생성시킨 후 넘겨줄 정보를 셋팅한다.
        // 다음으로 이 함수에 해당 객체를 넘겨주면 
        // 객체와 같은 폼이 있으면 close 시킨후 넘겨받은 객체를 show 시켜 준다.
        public static void InfoMngLoadForm<TForm>(Form pForm) where TForm : Form, new()
        {
            foreach (Form theForm in Application.OpenForms)
            {
                if (theForm is TForm)  //해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
                {
                    pForm = theForm;
                    return;
                }
            }

            pForm.MdiParent = frmMainForm;
            pForm.Show();
        }

        ////엑셀 파일 익스포트
        //public static void ExportToXLS(FarPoint.Win.Spread.FpSpread pSpdList)
        //{
        //    SaveFileDialog mDlg = new SaveFileDialog();

        //    mDlg.InitialDirectory = Application.StartupPath;
        //    mDlg.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
        //    mDlg.FilterIndex = 1;

        //    if (mDlg.ShowDialog() == DialogResult.OK)
        //    {
        //        pSpdList.SaveExcel(mDlg.FileName, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
        //        MessageBox.Show("저장이 완료 되었습니다.", frmMainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        public static void ExportToXLS(DevExpress.XtraGrid.GridControl grdCtrl)
        {
            SaveFileDialog mDlg = new SaveFileDialog();

            mDlg.InitialDirectory = Application.StartupPath;
            mDlg.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            mDlg.FilterIndex = 1;

            if (mDlg.ShowDialog() == DialogResult.OK)
            {
                grdCtrl.ExportToXls(mDlg.FileName);
                MessageBox.Show("저장이 완료 되었습니다.", frmMainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(mDlg.FileName);
            }
        }

        ////XML파일 익스포트
        //public static void ExportToXML(FarPoint.Win.Spread.FpSpread pSpdList)
        //{
        //    SaveFileDialog mDlg = new SaveFileDialog();

        //    mDlg.InitialDirectory = Application.StartupPath;
        //    mDlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
        //    mDlg.FilterIndex = 1;

        //    if (mDlg.ShowDialog() == DialogResult.OK)
        //    {
        //        pSpdList.SaveExcel(mDlg.FileName, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
        //        MessageBox.Show("저장이 완료 되었습니다.", frmMainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        ////스프레드 정렬
        //public static void SortSpreadData(FarPoint.Win.Spread.SheetView pSpsList)
        //{
        //    SortSpreadData(pSpsList, true);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pSpsList"></param>
        ///// <param name="doSort">이 메서드에서 정렬을 실행할지 여부를 결정한다.</param>
        //public static void SortSpreadData(FarPoint.Win.Spread.SheetView pSpsList, bool doSort)
        //{
        //    int i = 0;
        //    ArrayList pSortField = new ArrayList();
        //    ArrayList pSortList = new ArrayList();
        //    ArrayList pSortDir = new ArrayList();
        //    String[] strTmp = null;

        //    //스프레드에 박아 놓은 정렬조건들을 끄집어 낸다
        //    for (i = 0; i < pSpsList.Columns.Count; i++)
        //    {
        //        pSortField.Add(pSpsList.ColumnHeader.Cells[0, i].Text);
        //        if (pSpsList.ColumnHeader.Cells[0, i].Tag != null)
        //        {
        //            strTmp = pSpsList.ColumnHeader.Cells[0, i].Tag.ToString().Split(new Char[] { ',' });
        //            pSortList.Add(strTmp[0]);
        //            pSortDir.Add(strTmp[1]);
        //        }
        //    }

        //    frmSort mFrm = new frmSort(pSortField, pSortList, pSortDir);
        //    mFrm.ShowDialog();

        //    //스프레드에 정렬조건들 다시 저장
        //    for (i = 0; i < pSpsList.Columns.Count; i++) pSpsList.ColumnHeader.Cells[0, i].Tag = null;
        //    for (i = 0; i < pSortList.Count; i++)
        //    {
        //        pSpsList.ColumnHeader.Cells[0, i].Tag = pSortList[i].ToString() + "," + pSortDir[i].ToString();
        //    }

        //    if (pSortList.Count > 0 && doSort)
        //    {
        //        FarPoint.Win.Spread.SortInfo[] sort = new FarPoint.Win.Spread.SortInfo[pSortList.Count];
        //        for (i = 0; i < pSortList.Count; i++)
        //        {
        //            sort[i] = new FarPoint.Win.Spread.SortInfo(Convert.ToInt32(pSortList[i]), (pSortDir[i].ToString() == "DESC") ? false : true, System.Collections.Comparer.Default);
        //        }

        //        pSpsList.SortRange(0, 0, pSpsList.RowCount, pSpsList.ColumnCount, true, sort);
        //    }
        //}

        ////ListView 정렬(소트창 이용)
        //public static void mSortListView(ArrayList pSortField, ArrayList pSortList, ArrayList pSortDir, ArrayList pSortType, ListView lvList)
        //{
        //    frmSort mFrm = new frmSort(pSortField, pSortList, pSortDir);
        //    mFrm.ShowDialog();

        //    if (pSortList.Count > 0)
        //    {
        //        ListViewSorter lvSorter = new ListViewSorter(pSortList, pSortDir, pSortType);

        //        lvList.ListViewItemSorter = lvSorter;
        //        lvList.Sort();
        //    }
        //}

        //ListView 정렬(코드로 바로)
        public static void mSortListView(ArrayList pSortList, ArrayList pSortDir, ArrayList pSortType, ListView lvList)
        {
            if (pSortList.Count > 0)
            {
                ListViewSorter lvSorter = new ListViewSorter(pSortList, pSortDir, pSortType);

                lvList.ListViewItemSorter = lvSorter;
                lvList.Sort();
            }
        }

        //VB색상->C#색상으로 변환
        public static Color ConvertToCSColor(String mTmp)
        {
            Color mCSColor = new Color();
            long mVBColor;
            int mRed;
            int mGreen;
            int mBlue;

            mTmp = mTmp.ToLower();


            if ((mTmp.Length > 1) && (mTmp.Substring(0, 2) == "&h"))     //희한하게 16진수로 저장된 놈들도 있고 10진으로 들어가 있는 놈들도 있으므로 구분해서 변환
            {
                mVBColor = Convert.ToInt32(mTmp.Replace("&h", "0x"), 16);
            }
            else
            {
                mVBColor = (mTmp.Length > 0) ? Convert.ToInt32(mTmp) : 0;
            }

            if ((mVBColor > 0x00FFFFFF) || (mVBColor < 0))
            {
                mBlue = 0;
                mGreen = 0;
                mRed = 0;
            }
            else
            {
                mBlue = Convert.ToInt32((mVBColor & 0x00FF0000) >> 16);
                mGreen = Convert.ToInt32((mVBColor & 0x0000FF00) >> 8);
                mRed = Convert.ToInt32((mVBColor & 0x000000FF));
            }

            mCSColor = Color.FromArgb(mRed, mGreen, mBlue);

            return mCSColor;
        }

        public static long ConvertToVBColor(Color mCSColor)
        {
            long mVBColor;

            if (mCSColor.IsEmpty == true)
            {
                mVBColor = 0;
            }
            else
            {
                mVBColor = 0x00000000 | (mCSColor.B << 16) | (mCSColor.G << 8) | mCSColor.R;
            }
            return mVBColor;
        }

        ////스프레드 시트의 특정열에서 문자열 일치 검사
        //public static int FindSpreadData(FarPoint.Win.Spread.SheetView pSpsList, int pCol, string pData)
        //{
        //    for (int i = 0; i < pSpsList.RowCount; i++)
        //    {
        //        if (pSpsList.Cells[i, pCol].Text == pData) return i;
        //    }

        //    return -1;
        //}

        ////스프레드 시트의 특정열에서 문자열 일치 검사
        //public static int FindSpreadDataCol(FarPoint.Win.Spread.SheetView pSpsList, int pRow, string pData)
        //{
        //    for (int i = 0; i < pSpsList.ColumnCount; i++)
        //    {
        //        if (pSpsList.Cells[pRow, i].Text == pData) return i;
        //    }

        //    return -1;
        //}

        ////스프레드 시트의 컬럼에 맞게 width 자동조정
        //public static void ResizeSpreadColumn(FarPoint.Win.Spread.SheetView pSpsList)
        //{
        //    if (pSpsList.RowCount == 0)
        //    {
        //        pSpsList.RowCount = 2;  //RowCount가 0일때는 GetPreferredColumnWidth 함수에서 에러가 남!
        //        for (int i = 0; i < pSpsList.ColumnCount; i++) pSpsList.Columns[i].Width = pSpsList.GetPreferredColumnWidth(i);
        //        pSpsList.RowCount = 0;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < pSpsList.ColumnCount; i++)
        //        {
        //            //pSpsList.Columns[i].Width = pSpsList.GetPreferredColumnWidth(i);
        //            pSpsList.Columns[i].Width = pSpsList.GetPreferredColumnWidth(i, false, false, false);
        //        }
        //    }
        //}

        ////스프레드 시트 내용 프린트
        //public static void PrintSpreadData(FarPoint.Win.Spread.FpSpread spdList)
        //{
        //    FarPoint.Win.Spread.PrintInfo pi = new FarPoint.Win.Spread.PrintInfo();
        //    pi.ShowPrintDialog = true;

        //    spdList.SetPrintInfo(pi, -1);
        //    spdList.PrintSheet(-1);
        //}

        public static string ConvertDateFormat(string pDate, string pUnit)
        {
            Hashtable dateFormat = new Hashtable();
            dateFormat.Add("yd", "yyyyMMdd000000");
            dateFormat.Add("yH", "yyyyMMddHH0000");
            dateFormat.Add("ym", "yyyyMMddHHmm00");
            dateFormat.Add("ys", "yyyyMMddHHmmss");

            return (Convert.ToDateTime(pDate)).ToString(dateFormat[pUnit].ToString());
        }

        public static bool ControlMandatoryItem(ArrayList pMandatoryItm)
        {
            int intMandatoryCnt = 0;
            bool isMandatoryChecked = false;

            for (int i = 0; i < pMandatoryItm.Count; i++)
            {
                object tObj = pMandatoryItm[i];

                //if (tObj is FarPoint.Win.Spread.Cell)
                //{
                //    FarPoint.Win.Spread.Cell tctrl = (FarPoint.Win.Spread.Cell)tObj;

                //    if (tctrl.Text.Equals(""))
                //    {
                //        intMandatoryCnt++;
                //        tctrl.BackColor = Color.FromArgb(255, 192, 192);
                //    }
                //    else
                //    {
                //        if (tctrl.BackColor == Color.FromArgb(255, 192, 192)) tctrl.BackColor = Color.FromArgb(255, 255, 255);
                //    }
                //}
                //else

                if (tObj is CheckEdit)
                {
                    CheckEdit tctrl = (CheckEdit)tObj;

                    if (tctrl.Checked == false)
                    {
                        intMandatoryCnt++;
                        tctrl.BackColor = Color.FromArgb(255, 192, 192);
                        if (!isMandatoryChecked)
                        {
                            isMandatoryChecked = true;
                            tctrl.Focus();
                        }
                    }
                    else
                    {
                        if (tctrl.BackColor == Color.FromArgb(255, 192, 192)) tctrl.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
                else if (tObj is Control)
                {
                    Control tctrl = (Control)tObj;

                    if (tctrl.Text.Equals(""))
                    {
                        intMandatoryCnt++;
                        tctrl.BackColor = Color.FromArgb(255, 192, 192);
                        if (!isMandatoryChecked)
                        {
                            isMandatoryChecked = true;
                            tctrl.Focus();
                        }
                    }
                    else
                    {
                        if (tctrl.BackColor == Color.FromArgb(255, 192, 192)) tctrl.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
                else if (tObj is ToolStripTextBox)
                {
                    ToolStripTextBox tctrl = (ToolStripTextBox)tObj;

                    if (tctrl.Text.Equals(""))
                    {
                        intMandatoryCnt++;
                        tctrl.BackColor = Color.FromArgb(255, 192, 192);
                        if (!isMandatoryChecked)
                        {
                            isMandatoryChecked = true;
                            tctrl.Focus();
                        }
                    }
                    else
                    {
                        if (tctrl.BackColor == Color.FromArgb(255, 192, 192)) tctrl.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
                else if (tObj is ToolStripComboBox)
                {
                    ToolStripComboBox tctrl = (ToolStripComboBox)tObj;

                    if (tctrl.Text.Equals(""))
                    {
                        intMandatoryCnt++;
                        tctrl.BackColor = Color.FromArgb(255, 192, 192);
                        if (!isMandatoryChecked)
                        {
                            isMandatoryChecked = true;
                            tctrl.Focus();
                        }
                    }
                    else
                    {
                        if (tctrl.BackColor == Color.FromArgb(255, 192, 192)) tctrl.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }

            if (intMandatoryCnt > 0)
            {
                MessageBox.Show("필수항목이 누락되었습니다.", CommFunc.frmMainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static void ChangeRowColor(FarPoint.Win.Spread.SheetView pSheet, int pRow, string pClrType, Color pClr)
        //{
        //    if (pClrType.Equals("Back"))
        //    {
        //        pSheet.Rows[pRow].BackColor = pClr;
        //    }
        //    else if (pClrType.Equals("Fore"))
        //    {
        //        pSheet.Rows[pRow].ForeColor = pClr;
        //    }
        //}

        public static void SetTextFormat(object pObj, String pStyle, KeyPressEventArgs pKeyCode, int pLSide)
        {
            SetTextFormat(pObj, pStyle, pKeyCode, pLSide, 0);
        }

        //2014-11-25 modified by Park Yong.
        //안진성 대리 메일(2014-11-25) 바탕으로 키입력 자리수 제한 관련 보강
        public static void SetTextFormat(object pObj, String pStyle, KeyPressEventArgs pKeyCode, int pLSide, int pRSide)
        {
            //2014.11.25 add By Ahn Jinsung. Ctrl C, Ctrl X 은 처리하지 않음
            if (pKeyCode.KeyChar == (char)3 || pKeyCode.KeyChar == (char)24)
            {
                return;
            }
            //If Return(Enter), Just return
            else if (pKeyCode.KeyChar == Convert.ToChar(Keys.Return))
            {
                return;
            }

            String sCtrlText = "";

            if (pObj is TextBox)
            {
                TextBox tCtrl = (TextBox)pObj;
                int iSelectionStart = 0;
                iSelectionStart = tCtrl.SelectionStart;
                if (tCtrl.SelectionLength.Equals(tCtrl.Text.Length))
                    tCtrl.Text = "";
                else if (tCtrl.SelectionLength > 0)
                {
                    tCtrl.Text = tCtrl.Text.Substring(0, tCtrl.SelectionStart) +
                                 tCtrl.Text.Substring(tCtrl.SelectionStart + tCtrl.SelectionLength, tCtrl.Text.Length - (tCtrl.SelectionStart + tCtrl.SelectionLength));
                }

                sCtrlText = tCtrl.Text;
                tCtrl.SelectionStart = iSelectionStart;
            }
            else if (pObj is ComboBoxEdit)
            {
                ComboBoxEdit tCtrl = (ComboBoxEdit)pObj;
                int iSelectionStart = 0;
                iSelectionStart = tCtrl.SelectionStart;
                if (tCtrl.SelectionLength.Equals(tCtrl.Text.Length))
                    tCtrl.Text = "";
                else if (tCtrl.SelectionLength > 0)
                {
                    tCtrl.Text = tCtrl.Text.Substring(0, tCtrl.SelectionStart) +
                                 tCtrl.Text.Substring(tCtrl.SelectionStart + tCtrl.SelectionLength, tCtrl.Text.Length - (tCtrl.SelectionStart + tCtrl.SelectionLength));
                }

                sCtrlText = tCtrl.Text;
                tCtrl.SelectionStart = iSelectionStart;
            }
            else if (pObj is CheckedComboBoxEdit)
            {
                CheckedComboBoxEdit tCtrl = (CheckedComboBoxEdit)pObj;
                int iSelectionStart = 0;
                iSelectionStart = tCtrl.SelectionStart;
                if (tCtrl.SelectionLength.Equals(tCtrl.Text.Length))
                    tCtrl.Text = "";
                else if (tCtrl.SelectionLength > 0)
                {
                    tCtrl.Text = tCtrl.Text.Substring(0, tCtrl.SelectionStart) +
                                 tCtrl.Text.Substring(tCtrl.SelectionStart + tCtrl.SelectionLength, tCtrl.Text.Length - (tCtrl.SelectionStart + tCtrl.SelectionLength));
                }

                sCtrlText = tCtrl.Text;
                tCtrl.SelectionStart = iSelectionStart;
            }
            else if (pObj is System.Windows.Forms.ComboBox)
            {
                System.Windows.Forms.ComboBox tCtrl = (System.Windows.Forms.ComboBox)pObj;
                int iSelectionStart = 0;
                iSelectionStart = tCtrl.SelectionStart;
                if (tCtrl.SelectionLength.Equals(tCtrl.Text.Length))
                    tCtrl.Text = "";
                else if (tCtrl.SelectionLength > 0)
                {
                    tCtrl.Text = tCtrl.Text.Substring(0, tCtrl.SelectionStart) +
                                 tCtrl.Text.Substring(tCtrl.SelectionStart + tCtrl.SelectionLength, tCtrl.Text.Length - (tCtrl.SelectionStart + tCtrl.SelectionLength));
                }

                sCtrlText = tCtrl.Text;
                tCtrl.SelectionStart = iSelectionStart;
            }
            else if (pObj is ToolStripTextBox)
            {
                ToolStripTextBox tCtrl = (ToolStripTextBox)pObj;
                int iSelectionStart = 0;
                iSelectionStart = tCtrl.SelectionStart;
                if (tCtrl.SelectionLength.Equals(tCtrl.Text.Length))
                    tCtrl.Text = "";
                else if (tCtrl.SelectionLength > 0)
                {
                    tCtrl.Text = tCtrl.Text.Substring(0, tCtrl.SelectionStart) +
                                 tCtrl.Text.Substring(tCtrl.SelectionStart + tCtrl.SelectionLength, tCtrl.Text.Length - (tCtrl.SelectionStart + tCtrl.SelectionLength));
                }

                sCtrlText = tCtrl.Text;
                tCtrl.SelectionStart = iSelectionStart;
            }
            else if (pObj is ToolStripComboBox)
            {
                ToolStripComboBox tCtrl = (ToolStripComboBox)pObj;
                int iSelectionStart = 0;
                iSelectionStart = tCtrl.SelectionStart;
                if (tCtrl.SelectionLength.Equals(tCtrl.Text.Length))
                    tCtrl.Text = "";
                else if (tCtrl.SelectionLength > 0)
                {
                    tCtrl.Text = tCtrl.Text.Substring(0, tCtrl.SelectionStart) +
                                 tCtrl.Text.Substring(tCtrl.SelectionStart + tCtrl.SelectionLength, tCtrl.Text.Length - (tCtrl.SelectionStart + tCtrl.SelectionLength));
                }

                sCtrlText = tCtrl.Text;
                tCtrl.SelectionStart = iSelectionStart;
            }
            else if (pObj is TextEdit)
            {
                TextEdit tCtrl = (TextEdit)pObj;
                int iSelectionStart = 0;
                iSelectionStart = tCtrl.SelectionStart;
                if (tCtrl.SelectionLength.Equals(tCtrl.Text.Length))
                    tCtrl.Text = "";
                else if (tCtrl.SelectionLength > 0)
                {
                    tCtrl.Text = tCtrl.Text.Substring(0, tCtrl.SelectionStart) +
                                 tCtrl.Text.Substring(tCtrl.SelectionStart + tCtrl.SelectionLength, tCtrl.Text.Length - (tCtrl.SelectionStart + tCtrl.SelectionLength));
                }

                sCtrlText = tCtrl.Text;
                tCtrl.SelectionStart = iSelectionStart;
            }

            // 2014.11.25 modified by Ahn Jinsung. CTRL+V 자리제한 처리
            //(3 -> Ctrl C ) (22 -> Ctrl V)  (24 -> Ctrl X)
            if (pKeyCode.KeyChar == (char)22)
            {
                String MergeText = GetMergedTextWithClipboard(pObj);

                int byteCnt = Encoding.Default.GetBytes(MergeText).Length;

                if (byteCnt > pLSide)
                {
                    pKeyCode.Handled = true;
                }
            }
            else
            {
                int byteCnt = Encoding.Default.GetBytes(sCtrlText).Length;

                if (pStyle.Equals(SetTextFormatType.INT) || pStyle.Equals(SetTextFormatType.LONG))
                {
                    if ((pKeyCode.KeyChar < '0' ||
                            pKeyCode.KeyChar > '9' ||
                            sCtrlText.Length >= pLSide) &&
                            !pKeyCode.KeyChar.Equals(Convert.ToChar(Keys.Back)) &&
                            !(pKeyCode.KeyChar.Equals('-') && sCtrlText.Trim().Equals("")))
                    {
                        pKeyCode.KeyChar = Convert.ToChar(Keys.None);
                        Beep(500, 100);
                    }
                }
                else if (pStyle.Equals(SetTextFormatType.DOUBLE))
                {
                    if (sCtrlText.IndexOf('.').Equals(-1))
                    {
                        if ((pKeyCode.KeyChar < '0' ||
                                pKeyCode.KeyChar > '9' ||
                                sCtrlText.Length >= pLSide) &&
                                !pKeyCode.KeyChar.Equals(Convert.ToChar(Keys.Back)) &&
                                !pKeyCode.KeyChar.Equals('.') &&
                            !(pKeyCode.KeyChar.Equals('-') && sCtrlText.Trim().Equals("")))
                        {
                            pKeyCode.KeyChar = Convert.ToChar(Keys.None);
                            Beep(500, 100);
                        }
                    }
                    else
                    {
                        if ((pKeyCode.KeyChar < '0' ||
                                pKeyCode.KeyChar > '9' ||
                                sCtrlText.Length >= sCtrlText.IndexOf('.') + 1 + pRSide ||
                                sCtrlText.Length >= pLSide + 1 + pRSide) &&
                                !pKeyCode.KeyChar.Equals(Convert.ToChar(Keys.Back)) &&
                            !(pKeyCode.KeyChar.Equals('-') && sCtrlText.Trim().Equals("")))
                        {
                            pKeyCode.KeyChar = Convert.ToChar(Keys.None);
                            Beep(500, 100);
                        }
                    }
                }
                else if (pStyle.Equals(SetTextFormatType.CAPITAL))
                {
                    if (byteCnt < pLSide || pKeyCode.KeyChar.Equals(Convert.ToChar(Keys.Back)))
                    {
                        if (pKeyCode.KeyChar >= 'a' && pKeyCode.KeyChar <= 'z')
                        {
                            pKeyCode.KeyChar = Convert.ToChar(pKeyCode.KeyChar + 'A' - 'a');
                        }
                    }
                    else
                    {
                        pKeyCode.KeyChar = Convert.ToChar(Keys.None);
                        Beep(500, 100);
                    }
                }
                else if (pStyle.Equals(SetTextFormatType.CHAR) || pStyle.Equals(SetTextFormatType.UPPERCHAR))
                {
                    if ((byteCnt + Encoding.Default.GetBytes(pKeyCode.KeyChar.ToString()).Length) <= pLSide
                        || pKeyCode.KeyChar.Equals(Convert.ToChar(Keys.Back)))
                    {
                        if (pStyle.Equals(SetTextFormatType.UPPERCHAR))
                        {
                            pKeyCode.KeyChar = Char.ToUpper(pKeyCode.KeyChar);
                        }
                    }
                    else
                    {
                        pKeyCode.KeyChar = Convert.ToChar(Keys.None);
                        Beep(500, 100);
                    }
                }
            }
        }

        // 2014.11.25 added by Ahn Jinsung.
        private static String GetMergedTextWithClipboard(object pObj)
        {
            StringBuilder sb = new StringBuilder();

            if (pObj is TextBox)
            {
                TextBox tCtrl = (TextBox)pObj;
                sb.Append(tCtrl.Text).Append(Clipboard.GetText());
            }
            else if (pObj is ComboBoxEdit)
            {
                ComboBoxEdit tCtrl = (ComboBoxEdit)pObj;
                sb.Append(tCtrl.Text).Append(Clipboard.GetText());
            }
            else if (pObj is CheckedComboBoxEdit)
            {
                CheckedComboBoxEdit tCtrl = (CheckedComboBoxEdit)pObj;
                sb.Append(tCtrl.Text).Append(Clipboard.GetText());
            }
            else if (pObj is System.Windows.Forms.ComboBox)
            {
                System.Windows.Forms.ComboBox tCtrl = (System.Windows.Forms.ComboBox)pObj;
                sb.Append(tCtrl.Text).Append(Clipboard.GetText());
            }
            else if (pObj is ToolStripTextBox)
            {
                ToolStripTextBox tCtrl = (ToolStripTextBox)pObj;
                sb.Append(tCtrl.Text).Append(Clipboard.GetText());
            }
            else if (pObj is ToolStripComboBox)
            {
                ToolStripComboBox tCtrl = (ToolStripComboBox)pObj;
                sb.Append(tCtrl.Text).Append(Clipboard.GetText());
            }

            return sb.ToString();
        }

        public static void ClearFormControls(object pObj)
        {
            ClearFormControls(pObj, "");
        }

        // 전체 내용 clear 
        public static void ClearFormControls(object pObj, String pInit)
        {
            foreach (Control tCtrl in ((Control)pObj).Controls)
            {
                if (tCtrl is Panel ||
                   tCtrl is GroupBox ||
                   tCtrl is GroupControl ||
                   tCtrl is TabControl ||
                   tCtrl is TabPage)
                {
                    ClearFormControls(tCtrl, pInit);
                }
                else if (tCtrl is TextBox ||
                    tCtrl is TextEdit)
                {
                    tCtrl.Text = pInit;
                    tCtrl.ForeColor = SystemColors.WindowText;
                    tCtrl.BackColor = SystemColors.Window;
                }
                else if (tCtrl is System.Windows.Forms.ComboBox ||
                         tCtrl is ComboBoxEdit ||
                         tCtrl is CheckedComboBoxEdit)
                {
                    tCtrl.Text = pInit;
                    tCtrl.ForeColor = SystemColors.WindowText;
                    tCtrl.BackColor = SystemColors.Window;
                }
                else if (tCtrl is CheckBox)
                {
                    ((CheckBox)tCtrl).Checked = false;
                }
                else if (tCtrl is DateTimePicker)
                {
                    ((DateTimePicker)tCtrl).Value = DateTime.Today;
                }
                else if (tCtrl is GroupControl)
                {
                    tCtrl.Text = pInit;
                    tCtrl.ForeColor = SystemColors.WindowText;
                    tCtrl.BackColor = SystemColors.Window;
                }
            }
        }

        //StatusBar 상태 리프레쉬
        private static void RefreshStatusBar(object sender, EventArgs e)
        {
            // NumLock키 상태확인
            if (Control.IsKeyLocked(Keys.NumLock))
            {
                lblStatusBarNumLock.Text = "NUMLOCK";
            }
            else
            {
                lblStatusBarNumLock.Text = "";
            }

            // CapsLock키 상태확인(Keys.Capital 사용해도 됨)
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                lblStatusBarCapsLock.Text = "CAPSLOCK";
            }
            else
            {
                lblStatusBarCapsLock.Text = "";
            }

            lblStatusBarDate.Text = DateTime.Now.ToShortDateString();
            lblStatusBarTime.Text = DateTime.Now.ToShortTimeString();

            lblStatusBarIP.Text = gloHostIP;
            lblStatusBarLoginID.Text = gloUserID;
            lblStatusBarTML.Text = gloTMLCod;
            lblStatusBarAuth.Text = gloSystemPrefix;
        }

        public static int ConvertToInt(string sValue)
        {
            if (sValue != null && sValue.Length > 0 && sValue.Substring(0, 1) == ".")
                sValue = "0" + sValue;

            if ((sValue == null) ||
                (sValue.Trim() == "") ||
                ((sValue.Length == 1) && (sValue == "." || sValue == "-")) ||
                ((sValue.Length > 1) && (sValue.Substring(0, 2) == "-."))) return 0;
            else return Convert.ToInt32(sValue.Split('.')[0].Replace(",", ""));
        }

        public static long ConvertToLong(string sValue)
        {
            if (sValue != null && sValue.Length > 0 && sValue[0].ToString() == ".")
                sValue = "0" + sValue;

            if ((sValue == null) ||
                (sValue.Trim() == "") ||
                ((sValue.Length == 1) && (sValue == "." || sValue == "-")) ||
                ((sValue.Length > 1) && (sValue.Substring(0, 2) == "-."))) return 0;
            else return Convert.ToInt64(sValue.Split('.')[0].Replace(",", ""));
        }

        public static double ConvertToDouble(string sValue)
        {
            if ((sValue == null) ||
                (sValue.Trim() == "") ||
                ((sValue.Length == 1) && (sValue == "." || sValue == "-")) ||
                ((sValue.Length > 1) && (sValue.Substring(0, 2) == "-."))) return 0;
            else if(sValue.Trim().Substring(sValue.Trim().Length-1,1 ) ==".") return Convert.ToDouble(sValue.Replace(".","").Replace(",", ""));            
            else return Convert.ToDouble(sValue.Replace(",", ""));
        }

        public static double ConvertToNumber(string sValue)
        {
            if ((sValue == null) ||
                (sValue.Trim() == "") ||
                ((sValue.Length == 1) && (sValue == "." || sValue == "-")) ||
                ((sValue.Length > 1) && (sValue.Substring(0, 2) == "-."))) return 0;
            else return Convert.ToDouble(sValue.Replace(",", ""));
        }

        public static String SetPadRight(Control pCtrl, int pSize)
        {
            int byteCnt = Encoding.Default.GetBytes(pCtrl.Text.Trim()).Length;

            string padRight = pCtrl.Text;

            for (int i = 0; i < pSize - byteCnt; i++)
            {
                padRight += " ";
            }

            return padRight;
        }

        //public static String ConvertThousandSeparator(String pNumber)
        //{
        //    String returnNumber = "";

        //    NumberFormatInfo numberFormat = new CultureInfo("ko-KR", false).NumberFormat;
        //    numberFormat.NumberDecimalDigits = 0;
        //    try
        //    {
        //        double dNumber = Convert.ToDouble(pNumber);
        //        returnNumber = dNumber.ToString("N", numberFormat);
        //    }
        //    catch (FormatException fe)
        //    {
        //        string a = fe.Message;
        //    }

        //    return returnNumber;
        //}

        //2009.12.08 윤나리. 본래 천단위 끊는 계산값이 소수점 존재시 제대로 나타나지 않아 재수정.
        public static String ConvertThousandSeparator(String pNumber)
        {
            String returnNumber = "";

            NumberFormatInfo numberFormat = new CultureInfo("ko-KR", false).NumberFormat;
            numberFormat.NumberDecimalDigits = 0;
            try
            {

                //********************************************************************************
                //* 2010.01.04 SmileMan 수정
                //* DESC : 소수점 및 부호처리를 위해서 출력 패턴변경 (-1,234,567.89)
                //********************************************************************************

                #region ## 수정전 Source ##

                //double dNumber = Convert.ToDouble(pNumber);
                //returnNumber = dNumber.ToString("###,###,###,###.###", numberFormat);

                #endregion

                #region ## 수정후 Source ##

                // trim후 입력값이 없는 경우 그대로 반환한다. 
                if (pNumber.Trim().Equals("")) return pNumber;

                // 소수점이 포함되어 있는지 체크한 후 소수점 사이즈만큼 출력되도록 설정한다.
                if (pNumber.IndexOf(Convert.ToChar(".")) != -1)
                {
                    string[] strBuff = pNumber.Split(new Char[] { '.' });
                    numberFormat.NumberDecimalDigits = strBuff[1].ToString().Length;
                }

                numberFormat.NumberNegativePattern = 1;

                double dNumber = Convert.ToDouble(pNumber);
                returnNumber = dNumber.ToString("N", numberFormat);

                #endregion

                //********************************************************************************

            }
            catch (FormatException fe)
            {
                string a = fe.Message;
            }

            return returnNumber;
        }

        //public static ReportDocument SetCrystalReportDocument(ArrayList pPrintValue,
        //                                                      String pRptFilePath,
        //                                                      String pProgramID,
        //                                                      Object pProgramParam,
        //                                                      String pReportName,
        //                                                      Boolean pShowPreviewForm)
        //{
        //    ReportDocument reportDocument = new ReportDocument();
        //    reportDocument.Load(pRptFilePath);

        //    ArrayList aReportField = new ArrayList();
        //    ArrayList aResult = new ArrayList();

        //    try
        //    {
        //        if (pProgramParam is Hashtable)
        //            aResult = RequestHandler.Request(gloFrameworkServerName, pProgramID, g_ID, (Hashtable)pProgramParam);
        //        if (pProgramParam is ArrayList)
        //            aResult = RequestHandler.Request(gloFrameworkServerName, pProgramID, g_ID, (ArrayList)pProgramParam);
        //    }
        //    catch (HMMException ex)
        //    {
        //        MessageBox.Show(ex.Message1);
        //    }

        //    foreach (Hashtable hResult in aResult)
        //    {
        //        ArrayList aValue = new ArrayList();
        //        foreach (String sValue in pPrintValue)
        //        {
        //            aValue.Add(hResult[sValue].ToString());
        //        }
        //        CrystalReportField crystalReportField = new CrystalReportField(aValue);
        //        aReportField.Add(crystalReportField);
        //    }

        //    reportDocument.SetDataSource(aReportField);

        //    if (pShowPreviewForm)
        //    {
        //        CrystalReportPreview(reportDocument, pReportName);
        //    }

        //    return reportDocument;
        //}

        //public static ReportDocument SetCrystalReportDocument(ArrayList pPrintColumn,
        //                                                      String pRptFilePath,
        //                                                      FarPoint.Win.Spread.SheetView pPrintSheet,
        //                                                      String pReportName,
        //                                                      Boolean pShowPreviewForm)
        //{
        //    ReportDocument reportDocument = new ReportDocument();
        //    reportDocument.Load(pRptFilePath);

        //    ArrayList aReportField = new ArrayList();

        //    for (int row = 0; row < pPrintSheet.RowCount; row++)
        //    {
        //        ArrayList aValue = new ArrayList();
        //        foreach (String sValue in pPrintColumn)
        //        {
        //            aValue.Add(pPrintSheet.GetText(row, CommFunc.ConvertToInt(sValue)));
        //        }
        //        CrystalReportField crystalReportField = new CrystalReportField(aValue);
        //        aReportField.Add(crystalReportField);
        //    }

        //    reportDocument.SetDataSource(aReportField);

        //    if (pShowPreviewForm)
        //    {
        //        CrystalReportPreview(reportDocument, pReportName);
        //    }

        //    return reportDocument;
        //}

        //public static void CrystalReportPreview(ReportDocument pReportDocument, String pReportName)
        //{
        //    frmCrystalReportPreview frmCrystalReportPreview = new frmCrystalReportPreview();
        //    frmCrystalReportPreview.crystalReportPreview.ReportSource = pReportDocument;
        //    frmCrystalReportPreview.Text = pReportName;
        //    frmCrystalReportPreview.ShowDialog();
        //}

        //public static void PickForm(object p_Obj, String p_sCode)
        //{
        //    if (p_Obj is MenuStrip)
        //    {
        //        MenuStrip msTmp = (MenuStrip)p_Obj;

        //        for (int i = 0; i < msTmp.Items.Count; i++)
        //        {
        //            PickForm(msTmp.Items[i], p_sCode);
        //        }
        //    }
        //    else if (p_Obj is ToolStripMenuItem)
        //    {
        //        ToolStripMenuItem tsmi = (ToolStripMenuItem)p_Obj;

        //        for (int i = 0; i < tsmi.DropDownItems.Count; i++)
        //        {
        //            if (tsmi.DropDownItems[i] is ToolStripMenuItem)
        //            {
        //                if (tsmi.DropDownItems[i].Text.Contains(p_sCode))
        //                {
        //                    tsmi.DropDownItems[i].PerformClick();
        //                    break;
        //                }
        //            }

        //            PickForm(tsmi.DropDownItems[i], p_sCode);
        //        }
        //    }
        //}


        public static void PickForm(object p_Obj, String p_sCode)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (p_Obj is MenuStrip)
            {
                MenuStrip msTmp = (MenuStrip)p_Obj;

                for (int i = 0; i < msTmp.Items.Count; i++)
                {
                    PickForm(msTmp.Items[i], p_sCode);
                }
            }
            else if (p_Obj is ToolStripMenuItem)
            {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)p_Obj;

                for (int i = 0; i < tsmi.DropDownItems.Count; i++)
                {
                    if (tsmi.DropDownItems[i] is ToolStripMenuItem)
                    {
                        if (tsmi.DropDownItems[i].Text.Contains(p_sCode))
                        {
                            tsmi.DropDownItems[i].PerformClick();
                            break;
                        }
                    }

                    PickForm(tsmi.DropDownItems[i], p_sCode);
                }
            }

            Cursor.Current = Cursors.Default;
        }

        ////스프레드 시트 Check Box 체크 하기
        //public static void CheckSpreadData(FarPoint.Win.Spread.SheetView pSpsList, FarPoint.Win.Spread.CellClickEventArgs e)
        //{
        //    if (pSpsList.RowCount == 0) return;

        //    if (!e.RowHeader && e.ColumnHeader)
        //    {
        //        if (pSpsList.Columns[e.Column].CellType == null)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            if (pSpsList.Columns[e.Column].CellType.GetType().Equals(new FarPoint.Win.Spread.CellType.CheckBoxCellType().GetType()))
        //            {
        //                if (pSpsList.ColumnHeader.Cells[e.Row, e.Column].Value == null)
        //                {
        //                    if (!pSpsList.ColumnHeader.Cells[e.Row, e.Column].Locked)
        //                    {
        //                        pSpsList.ColumnHeader.Cells[e.Row, e.Column].Value = true;
        //                        pSpsList.Cells[0, e.Column, pSpsList.RowCount - 1, e.Column].Value = true;
        //                        return;
        //                    }
        //                }
        //                else
        //                {

        //                    if (pSpsList.ColumnHeader.Cells[e.Row, e.Column].Text == "True")
        //                    {
        //                        pSpsList.ColumnHeader.Cells[e.Row, e.Column].Value = false;
        //                        pSpsList.Cells[0, e.Column, pSpsList.RowCount - 1, e.Column].Value = false;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        /* 헤더컬럼은 당근 true/false 설정없이 false인데, 헤더값으로 일괄 등록되는 문제가 있음
        //                        if (!pSpsList.ColumnHeader.Cells[e.Row, e.Column].Locked)
        //                        {
        //                            pSpsList.ColumnHeader.Cells[e.Row, e.Column].Value = true;
        //                            pSpsList.Cells[0, e.Column, pSpsList.RowCount - 1, e.Column].Value = true;
        //                            return;
        //                        }*/
        //                        pSpsList.ColumnHeader.Cells[e.Row, e.Column].Value = true;
        //                        for (int i = 0; i < pSpsList.RowCount; i++)
        //                        {
        //                            if (!pSpsList.Cells[i, e.Column].Locked)
        //                            {
        //                                pSpsList.Cells[i, e.Column].Value = true;
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }

        //    }
        //    else if (!e.RowHeader && !e.ColumnHeader)
        //    {
        //        if (pSpsList.Columns[e.Column].CellType == null) return;
        //        if (pSpsList.Columns[e.Column].CellType.GetType().Equals(new FarPoint.Win.Spread.CellType.CheckBoxCellType().GetType()))
        //        {
        //            if (pSpsList.Cells[e.Row, e.Column].Value == null)
        //            {
        //                if (!pSpsList.Cells[e.Row, e.Column].Locked)
        //                {
        //                    pSpsList.Cells[e.Row, e.Column].Value = true;
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                if (pSpsList.Cells[e.Row, e.Column].Text == "True")
        //                {
        //                    pSpsList.Cells[e.Row, e.Column].Value = false;
        //                    return;
        //                }
        //                else
        //                {
        //                    if (!pSpsList.Cells[e.Row, e.Column].Locked)
        //                    {
        //                        pSpsList.Cells[e.Row, e.Column].Value = true;
        //                        return;
        //                    }

        //                }
        //            }
        //        }
        //    }
        //}

        ////스프레드 Mullt
        //public static void CheckExtendedSelectSpreadData(FarPoint.Win.Spread.SheetView pSpsList, int pColumnIndex)
        //{
        //    FarPoint.Win.Spread.Model.CellRange[] cellRange = pSpsList.GetSelections();

        //    if (cellRange.Length > 0)
        //    {
        //        for (int i = cellRange[0].Row; i < cellRange[0].Row + cellRange[0].RowCount; i++)
        //        {
        //            if (pSpsList.Cells[i, pColumnIndex].Value == null)
        //            {
        //                if (!pSpsList.Cells[i, pColumnIndex].Locked)
        //                    pSpsList.Cells[i, pColumnIndex].Value = true;
        //            }
        //            else
        //            {
        //                if (pSpsList.Cells[i, pColumnIndex].Text == "True")
        //                {
        //                    pSpsList.Cells[i, pColumnIndex].Value = false;
        //                }
        //                else
        //                {
        //                    if (!pSpsList.Cells[i, pColumnIndex].Locked)
        //                        pSpsList.Cells[i, pColumnIndex].Value = true;
        //                }
        //            }
        //        }
        //    }
        //}


        ////Spread 신규 디자인 적용부분 2009.07.24 윤나리
        //public static void SpreadControlSet(FarPoint.Win.Spread.FpSpread spdList)
        //{
        //    spdList.Font = new Font("Tahoma", 8f);
        //}

        //public static void SortRows(FarPoint.Win.Spread.FpSpread spdList, int cidx)
        //{
        //    FarPoint.Win.Spread.SheetView spsList = spdList.Sheets[0];

        //    if (spsList.Columns[cidx].CellType != null)
        //    {
        //        FarPoint.Win.Spread.CellType.CheckBoxCellType mChkCellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

        //        if (spsList.Columns[cidx].CellType.ToString() == mChkCellType.ToString())
        //            return;
        //    }


        //    bool blAscend = (iColumn[cidx] % 2 == 1) ? true : false;

        //    iColumn[cidx]++;

        //    FarPoint.Win.Spread.SortInfo[] sort = new FarPoint.Win.Spread.SortInfo[1];
        //    sort[0] = new FarPoint.Win.Spread.SortInfo(cidx, blAscend, System.Collections.Comparer.Default);

        //    for (int i = 0; i < spsList.ColumnCount; i++)
        //    {
        //        spsList.ColumnHeader.Columns[i].SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
        //    }

        //    if (blAscend) spsList.ColumnHeader.Columns[cidx].SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending;
        //    else spsList.ColumnHeader.Columns[cidx].SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Descending;

        //    spsList.SortRows(0, spsList.RowCount, sort);
        //}

        /// <summary>
        /// Set GridView OptionsBehavior & FocusRectStyle
        /// </summary>
        /// <param name="grdCtrl">GridControl Object</param>
        /// <param name="grdView">GridView Object</param>
        /// <param name="isEditable"></param>
        /// <param name="isReadOnly"></param>
        public static void gridViewDesign(DevExpress.XtraGrid.GridControl grdCtrl, object grdView, bool isEditable, bool isReadOnly)
        {
            gridViewDesign(grdCtrl, grdView, isEditable, isReadOnly, DevExpress.XtraGrid.Views.Grid.GridEditingMode.Default, DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus);
        }

        /// <summary>
        /// Set GridView OptionsBehavior & FocusRectStyle
        /// </summary>
        /// <param name="grdView">GridView Object</param>
        /// <param name="isEditable">true is Editable</param>
        /// <param name="isReadOnly">true is ReadOnly</param>
        /// <param name="editMode"></param>
        /// <param name="focusStyle"></param>
        public static void gridViewDesign(DevExpress.XtraGrid.GridControl grdCtrl, object grdView, bool isEditable, bool isReadOnly, DevExpress.XtraGrid.Views.Grid.GridEditingMode editMode, DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle focusStyle)
        {
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in (grdCtrl.MainView as DevExpress.XtraGrid.Views.Grid.GridView).Columns)
            {
                col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                col.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                col.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                col.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            }
            grdCtrl.Font = new Font("Tahoma", 8f);

            if (grdView is DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)
            {
                ((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)grdView).OptionsBehavior.Editable = isEditable;
                ((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)grdView).OptionsBehavior.ReadOnly = isReadOnly;
                ((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)grdView).OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Default;
                ((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)grdView).FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            }
            else
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)grdView).OptionsBehavior.Editable = isEditable;
                ((DevExpress.XtraGrid.Views.Grid.GridView)grdView).OptionsBehavior.ReadOnly = isReadOnly;
                ((DevExpress.XtraGrid.Views.Grid.GridView)grdView).OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Default;
                ((DevExpress.XtraGrid.Views.Grid.GridView)grdView).FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            }
        }

        //public static void spreadDesign(FarPoint.Win.Spread.SheetView spsList, bool selectionColor, bool ColumnHeaderVisible, bool RowHeaderVisible)
        //{
        //    spreadDesign(spsList, selectionColor, ColumnHeaderVisible, RowHeaderVisible, false);
        //}

        //public static void spreadDesign(FarPoint.Win.Spread.SheetView spsList, bool selectionColor, bool ColumnHeaderVisible, bool RowHeaderVisible, bool SeparateOddEvenRows)
        //{

        //    FarPoint.Win.Spread.FpSpread spdList = new FpSpread();
        //    spdList.Sheets.Count = 1;

        //    spdList = (FarPoint.Win.Spread.FpSpread)spsList.ContainingViews[0].Owner;
        //    EnhancedScrollBarRenderer esbr = new EnhancedScrollBarRenderer();

        //    esbr.ArrowColor = Color.FromArgb(171, 171, 171);
        //    esbr.ArrowHoveredColor = Color.FromArgb(100, 100, 100);
        //    esbr.ArrowSelectedColor = Color.FromArgb(56, 56, 56);
        //    esbr.ButtonBackgroundColor = Color.FromArgb(245, 245, 245);
        //    esbr.ButtonBorderColor = Color.FromArgb(151, 151, 151);
        //    esbr.ButtonHoveredBackgroundColor = Color.FromArgb(210, 210, 210);
        //    esbr.ButtonHoveredBorderColor = Color.FromArgb(151, 151, 151);
        //    esbr.ButtonSelectedBackgroundColor = Color.FromArgb(210, 210, 210);
        //    esbr.ButtonSelectedBorderColor = Color.FromArgb(151, 151, 151);
        //    esbr.TrackBarBackgroundColor = Color.FromArgb(151, 151, 151);
        //    esbr.TrackBarSelectedBackgroundColor = Color.FromArgb(151, 151, 151);

        //    spdList.HorizontalScrollBar.Renderer = esbr;
        //    spdList.VerticalScrollBar.Renderer = esbr;


        //    spdList.ColumnSplitBoxPolicy = SplitBoxPolicy.Never;
        //    spdList.RowSplitBoxPolicy = SplitBoxPolicy.Never;

        //    spsList.ColumnHeader.DefaultStyle.CanFocus = false;

        //    //스타일 - 디폴트로 해서, 기본 바탕이 파랑색되는거 막음.
        //    spdList.InterfaceRenderer = null;


        //    //선택 셀 두께 및 라인종류 설정
        //    DefaultFocusIndicatorRenderer dfir = new DefaultFocusIndicatorRenderer();
        //    dfir.Thickness = 1;
        //    spdList.FocusRenderer = dfir;

        //    //spsList.ColumnHeaderVisible = ColumnHeaderVisible;
        //    //spsList.RowHeaderVisible = RowHeaderVisible;

        //    for (int i = 0; i < spsList.ColumnHeader.RowCount; i++)
        //    {

        //        if (spsList.ColumnHeader.RowCount > 0)
        //        {
        //            spsList.ColumnHeader.Rows.Default.Height = 20;

        //            if (i == 0)
        //            {

        //                spsList.ColumnHeader.Rows[i].BackColor = Color.FromArgb(240, 240, 240);
        //                spsList.ColumnHeader.Rows[i].ForeColor = Color.FromArgb(56, 56, 56);
        //            }
        //            else
        //            {
        //                spsList.ColumnHeader.Rows[i].BackColor = Color.FromArgb(240, 240, 240);
        //                spsList.ColumnHeader.Rows[i].ForeColor = Color.FromArgb(56, 56, 56);
        //            }

        //        }
        //    }

        //    //Vertical Center정렬
        //    for(int i=0; i<spsList.ColumnCount; i++)
        //    {
        //        spsList.Columns[i].VerticalAlignment = CellVerticalAlignment.Center;
        //    }


        //    spsList.RowHeader.DefaultStyle.BackColor = Color.FromArgb(240, 240, 240);
        //    spsList.RowHeader.DefaultStyle.ForeColor = Color.FromArgb(56, 56, 56);

        //    spsList.SheetCornerStyle.BackColor = Color.FromArgb(240, 240, 240);

        //    spsList.GrayAreaBackColor = Color.White;

        //    spsList.DefaultStyle.Font = new Font("Tahoma", 8f);
        //    spsList.RowHeader.DefaultStyle.Font = new Font("Tahoma", 8f);
        //    spsList.ColumnHeader.DefaultStyle.Font = new Font("Tahoma", 8f);

        //    if (selectionColor)
        //    {
        //        spsList.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
        //        spsList.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
        //    }

        //    spsList.ActiveSkin = new SheetSkin("nSkin", Color.White, Color.White, Color.FromArgb(56, 56, 56), Color.FromArgb(171, 171, 171), FarPoint.Win.Spread.GridLines.Both, Color.FromArgb(240, 240, 240), Color.FromArgb(56, 56, 56), (selectionColor == true ? Color.FromArgb(205, 230, 247) : Color.Empty), Color.FromArgb(32, 32, 32), Color.White, SeparateOddEvenRows ? Color.FromArgb(240, 240, 240) : Color.White, true, true, false, true, true);


        //    GridLine gLine = new GridLine(GridLineType.Flat, Color.FromArgb(171, 171, 171));
        //    GridLine dLine = new GridLine(GridLineType.Flat, Color.FromArgb(171, 171, 171));

        //    spsList.VerticalGridLine = dLine;
        //    spsList.HorizontalGridLine = dLine;

        //    spsList.ColumnHeader.HorizontalGridLine = gLine;
        //    spsList.ColumnHeader.VerticalGridLine = gLine;
        //    spsList.RowHeader.HorizontalGridLine = gLine;
        //    spsList.RowHeader.VerticalGridLine = gLine;
        //    spsList.SheetCornerHorizontalGridLine = gLine;
        //    spsList.SheetCornerVerticalGridLine = gLine;

        //    spsList.ColumnHeaderVisible = ColumnHeaderVisible;
        //    spsList.RowHeaderVisible = RowHeaderVisible;

        //}

        //신규 디자인 적용부분 2009.07.24 윤나리
        public static void TextViewTrueMode(ToolStrip ts) //Text View Mode True (아이콘 및 텍스트 같이 표시)
        {
            Cursor.Current = Cursors.WaitCursor;

            foreach (ToolStripItem tItm in ts.Items)
            {
                if (tItm is ToolStripButton)
                {
                    ToolStripButton btn = (ToolStripButton)tItm;

                    if (btn.Visible)
                        btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        public static void TextViewFalseMode(ToolStrip ts)  //Text View Mode False (아이콘만 표시)
        {
            Cursor.Current = Cursors.WaitCursor;

            foreach (ToolStripItem tItm in ts.Items)
            {
                if (tItm is ToolStripButton)
                {
                    ToolStripButton btn = (ToolStripButton)tItm;

                    if (btn.Visible)
                        btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        public static void ShowExceptionBox(HMMException e)
        {
            frmExMessageBox frmBox = new frmExMessageBox();
            frmBox.setData(e);
            frmBox.ShowDialog();
        }

        public static void TextViewMode(ToolStrip ts, Boolean p_isTextView, ToolStripButton p_TextBtn, ToolStripButton p_IconBtn) //Text View Mode True (아이콘 및 텍스트 같이 표시)
        {
            Cursor.Current = Cursors.WaitCursor;

            p_TextBtn.Visible = !p_isTextView;
            p_IconBtn.Visible = p_isTextView;

            foreach (ToolStripItem tItm in ts.Items)
            {
                if (tItm is ToolStripButton)
                {
                    ToolStripButton btn = (ToolStripButton)tItm;

                    if (btn == p_TextBtn || btn == p_IconBtn)
                        continue;

                    if (p_isTextView)
                        btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    else
                        btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
                }
            }

            Cursor.Current = Cursors.Default;
        }


        ////Print ID : INI 파일 저장위해, 호출 화면의 ID, 프린트 할 스프레드
        //public static void ShowPrintSetup(String printID, FarPoint.Win.Spread.FpSpread spread)
        //{
        //    if (printopts != null)
        //    {
        //        if (printopts.IsDisposed)
        //        {
        //            printopts = new frmPrintOpts();
        //        }
        //    }
        //    else
        //    {
        //        printopts = new frmPrintOpts();
        //    }

        //    printopts.StartPage(printID, spread);

        //    //printopts.BringToFront();
        //}

        ////Print ID : INI 파일 저장위해, 호출 화면의 ID, 프린트 할 스프레드
        //public static void ShowPrintSetup(Form aForm)
        //{
        //    if (printopts != null)
        //    {
        //        if (printopts.IsDisposed)
        //        {
        //            printopts = new frmPrintOpts();
        //        }
        //    }
        //    else
        //    {
        //        printopts = new frmPrintOpts();
        //    }

        //    printopts.StartPage(aForm);

        //    //printopts.BringToFront();
        //}

        public static String GetActiveMQUri(String p_sAMQNm)
        {
            return GetActiveMQUri(p_sAMQNm, "*");
        }

        public static String GetActiveMQUri(String p_sAMQNm, String p_sNetGrp)
        {
            Hashtable hKeyAMQ = new Hashtable();
            hKeyAMQ.Add("AMQ_TYP", p_sAMQNm.Split('.')[0]);
            hKeyAMQ.Add("AMQ_NM", p_sAMQNm.Split('.')[1]);
            hKeyAMQ.Add("NETWORK_GRP", p_sNetGrp);

            String sUri = String.Empty;

            try
            {
                ArrayList aAMQ = BaseRequestHandler.Request(gloFrameworkServerName, "HITOPS3-ADM-ADM-S-GETAMQURI", g_ID, hKeyAMQ);

                if (aAMQ.Count == 1)
                {
                    Hashtable hAMQ = aAMQ[0] as Hashtable;
                    sUri = hAMQ["AMQ_URI"].ToString();
                }
            }
            catch (HMMException ex)
            {
                //ShowExceptionBox(ex);
            }

            return sUri;
        }

        //사용자 권한 별 메뉴 보이기/숨기기 적용 (코드의 뒷자리 001~999 숫자와 메뉴의 TAG 숫자 3자리와 비교)
        //Visibility 속성을 쓰면 탑 메뉴의 소메뉴가 모두 숨겨졌을때 마우스 클릭시 에러가 발생한다(Devexpress 한계인듯)
        //고로 그냥 메뉴 Object 를 제거하는 방법 사용.
        public static void ApplyUserMenuAuthority(String pTmnCod, String pTopPCode, DevExpress.XtraBars.BarManager pTool)
        {
            ArrayList aList = new ArrayList();
            Hashtable hTable = new Hashtable();

            hTable.Add("TMN_COD", pTmnCod);
            hTable.Add("USER_ID", CommFunc.gloUserID);
            hTable.Add("TOP_P_CODE", pTopPCode);

            aList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-ADM-USR-S-LSTUSERMENUAUTH", hTable);

            if (aList.Count > 0)
            {
                ArrayList arRemoveMenu = new ArrayList();

                foreach (DevExpress.XtraBars.BarItem oBI in pTool.Items)
                {
                    if (oBI.Tag != null && ((String)oBI.Tag).Trim() != "")
                    {
                        foreach (Hashtable sRcd in aList)
                        {
                            if (sRcd["MENU_ID"].ToString().Length > 3)
                            {
                                if (((String)oBI.Tag).Trim() == sRcd["MENU_ID"].ToString().Substring(sRcd["MENU_ID"].ToString().Length - 3, 3))
                                {
                                    if (sRcd["ENABLE"].ToString() != "Y")
                                    {
                                        arRemoveMenu.Add(oBI);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                if(arRemoveMenu.Count>0)
                {
                    foreach(DevExpress.XtraBars.BarItem oBI in arRemoveMenu)
                    {
                        pTool.Items.Remove(oBI);
                    }
                }
            }
        }

        public static void ApplyUserMenuAuthority(String pTmnCod, String pTopPCode, RibbonControl pTool)
        {
            ArrayList aList = new ArrayList();
            Hashtable hTable = new Hashtable();

            hTable.Add("TMN_COD", pTmnCod);
            hTable.Add("USER_ID", CommFunc.gloUserID);
            hTable.Add("TOP_P_CODE", pTopPCode);

            aList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-ADM-USR-S-LSTUSERMENUAUTH", hTable);

            if (aList.Count > 0)
            {
                ArrayList arRemoveMenu = new ArrayList();
                ArrayList arRemovePage = new ArrayList();

                foreach (RibbonPage oBI in pTool.Pages)
                {
                    if (oBI.Tag != null && ((String)oBI.Tag).Trim() != "")
                    {
                        foreach (Hashtable sRcd in aList)
                        {
                            if (sRcd["MENU_ID"].ToString().Length > 0)
                            {
                                if (((String)oBI.Tag).Trim() == sRcd["MENU_ID"].ToString())
                                {
                                    if (sRcd["ENABLE"].ToString() != "Y")
                                    {
                                        arRemovePage.Add(oBI);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                
                foreach (BarItem oBI in pTool.Items)
                {
                    if (oBI.Tag != null && ((String)oBI.Tag).Trim() != "")
                    {
                        foreach (Hashtable sRcd in aList)
                        {
                            if (sRcd["MENU_ID"].ToString().Length > 0)
                            {

                                if (((String)oBI.Tag).Trim() == sRcd["MENU_ID"].ToString())
                                {
                                    if (sRcd["ENABLE"].ToString() != "Y")
                                    {
                                        arRemoveMenu.Add(oBI);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                // Remove Menu
                if (arRemoveMenu.Count > 0)
                {
                    foreach (BarItem oBI in arRemoveMenu)
                    {
                        pTool.Items.Remove(oBI);
                    }
                }

                // Remove Page
                if (arRemovePage.Count > 0)
                {
                    foreach (RibbonPage oBI in arRemovePage)
                    {
                        pTool.Pages.Remove(oBI);
                    }
                }
            }
        }

        //2017.08.01. add by Ahn Jinsung.
        //기존에 Apply Terminal Code를 사용하지 않는 코드를 위함. 대상은 User의 Terminal Code로 Apply Terminal Code를 픽스.
        public static void SetUserForm(Form pForm, String pTmnCod, String pUserId, ToolStrip pToolStrip)
        {
            SetUserForm(pForm, pTmnCod, pUserId, pToolStrip, pTmnCod);
        }

        /// <summary>
        /// 사용자 권한에 따라 폼의 컨트롤 활성화 하기 (SELECT/INSERT/UPDATE/DELETE/SPECIAL 권한 값이 "N"이면 컨트롤의 Enabled = false)
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pTmnCod"></param>
        /// <param name="pUserId"></param>
        /// <param name="pToolStrip"></param>
        /// <param name="pApplyTerminalCode"></param>
        public static void SetUserForm(Form pForm, String pTmnCod, String pUserId, ToolStrip pToolStrip, String pApplyTerminalCode)
        {
            ArrayList sList = new ArrayList();
            Hashtable hTable = new Hashtable();

            try
            {
                hTable.Add("TMN_COD", pTmnCod);
                hTable.Add("USERID", pUserId);
                //hTable.Add("PGM_CODE", pForm.Name);
                hTable.Add("FORM_TAG", pForm.Tag);
                hTable.Add("APPLY_TERMINAL", pApplyTerminalCode);

                sList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-ADM-USR-S-LSTUSERCONTROLAUTH", hTable);

                if (sList.Count == 0)
                {
                    MessageBox.Show("현재 사용자에게 등록되지 않은 화면입니다. 전산실에 문의 바랍니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //모든 컨트롤 Enabled = false
                    ArrayList aList = new ArrayList();

                    getControls(pForm.Controls, ref aList);

                    for (int i = 0; i < aList.Count; i++) ((Control)aList[i]).Enabled = false;

                    if (pToolStrip != null) for (int i = 0; i < pToolStrip.Items.Count; i++) pToolStrip.Items[i].Enabled = false;
                }
                else
                {
                    ArrayList aList = new ArrayList();

                    getControls(pForm.Controls, ref aList);

                    //폼의 컨트롤 권한 설정
                    for (int i = 0; i < aList.Count; i++)
                    {
                        if (((Hashtable)sList[0])["ALLOW_SELECT"].ToString() == "N" && (string)((Control)aList[i]).Tag == "SELECT") ((Control)aList[i]).Enabled = false;
                        if (((Hashtable)sList[0])["ALLOW_INSERT"].ToString() == "N" && (string)((Control)aList[i]).Tag == "INSERT") ((Control)aList[i]).Enabled = false;
                        if (((Hashtable)sList[0])["ALLOW_UPDATE"].ToString() == "N" && (string)((Control)aList[i]).Tag == "UPDATE") ((Control)aList[i]).Enabled = false;
                        if (((Hashtable)sList[0])["ALLOW_DELETE"].ToString() == "N" && (string)((Control)aList[i]).Tag == "DELETE") ((Control)aList[i]).Enabled = false;
                        if (((Hashtable)sList[0])["ALLOW_SPECIAL1"].ToString() == "N" && (string)((Control)aList[i]).Tag == "SPECIAL1") ((Control)aList[i]).Enabled = false;
                        if (((Hashtable)sList[0])["ALLOW_SPECIAL2"].ToString() == "N" && (string)((Control)aList[i]).Tag == "SPECIAL2") ((Control)aList[i]).Enabled = false;
                        if (((Hashtable)sList[0])["ALLOW_SPECIAL3"].ToString() == "N" && (string)((Control)aList[i]).Tag == "SPECIAL3") ((Control)aList[i]).Enabled = false;
                    }

                    //ToolStrip 내의 컨트롤 권한 설정
                    if (pToolStrip != null)
                    {
                        for (int i = 0; i < pToolStrip.Items.Count; i++)
                        {
                            if (((Hashtable)sList[0])["ALLOW_SELECT"].ToString() == "N" && (string)pToolStrip.Items[i].Tag == "SELECT") pToolStrip.Items[i].Enabled = false;
                            if (((Hashtable)sList[0])["ALLOW_INSERT"].ToString() == "N" && (string)pToolStrip.Items[i].Tag == "INSERT") pToolStrip.Items[i].Enabled = false;
                            if (((Hashtable)sList[0])["ALLOW_UPDATE"].ToString() == "N" && (string)pToolStrip.Items[i].Tag == "UPDATE") pToolStrip.Items[i].Enabled = false;
                            if (((Hashtable)sList[0])["ALLOW_DELETE"].ToString() == "N" && (string)pToolStrip.Items[i].Tag == "DELETE") pToolStrip.Items[i].Enabled = false;
                            if (((Hashtable)sList[0])["ALLOW_SPECIAL1"].ToString() == "N" && (string)pToolStrip.Items[i].Tag == "SPECIAL1") pToolStrip.Items[i].Enabled = false;
                            if (((Hashtable)sList[0])["ALLOW_SPECIAL2"].ToString() == "N" && (string)pToolStrip.Items[i].Tag == "SPECIAL2") pToolStrip.Items[i].Enabled = false;
                            if (((Hashtable)sList[0])["ALLOW_SPECIAL3"].ToString() == "N" && (string)pToolStrip.Items[i].Tag == "SPECIAL3") pToolStrip.Items[i].Enabled = false;
                        }
                    }
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show(ex.Message1);
            }
        }

        // 2.1.1. 컨트롤내의 컨트롤을 가져오기 위해 재귀호출 (예. GroupBox 내의 Button 컨트롤)
        public static void getControls(Control.ControlCollection pControl, ref ArrayList pCtrlList)
        {
            for (int i = 0; i < pControl.Count; i++)
            {
                pCtrlList.Add(pControl[i]);
                if (pControl[i].Controls.Count > 0) getControls(pControl[i].Controls, ref pCtrlList);
            }
        }

        /// <summary>
        /// 오브젝트가 들어간 Hashtable로 정의된 Grid필드명에 따라 데이터를 맞춰넣음
        /// </summary>
        /// <param name="p_gridControl">Devexpress GridControl</param>
        /// <param name="sourceTable">Data Hashtable</param>
        public static void GridBindingByHashtable(DevExpress.XtraGrid.GridControl p_gridControl, Hashtable sourceTable)
        {
            System.Data.DataTable table = new System.Data.DataTable("LIST");
            foreach (DevExpress.XtraGrid.Columns.GridColumn col
            in (p_gridControl.MainView as DevExpress.XtraGrid.Views.Grid.GridView).Columns)
            {
                table.Columns.Add(col.FieldName);
            }

            foreach (Object oSource in sourceTable.Values)
            {
                DataRow dataRow = table.NewRow();
                foreach (System.Data.DataColumn dColumn in table.Columns)
                {
                    if (oSource.GetType().GetProperty(dColumn.ToString()) != null)
                    {
                        dataRow[dColumn.ToString()] = oSource.GetType().GetProperty(dColumn.ToString()).GetValue(oSource, null);
                    }
                }
                table.Rows.Add(dataRow);
            }
            p_gridControl.DataSource = table;
        }

        public static IList<Hashtable> Request(string programId)
        {
            IList<Hashtable> resultList = null;

            try
            {
                resultList = Request(programId, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

            return resultList;
        }

        public static IList<Hashtable> Request(string programId, Hashtable parameters)
        {
            IList<Hashtable> resultList = null;

            try
            {
                ArrayList aList = null;
                if (parameters != null)
                    aList = BaseRequestHandler.Request(gloFrameworkServerName, programId, parameters);
                else
                    aList = BaseRequestHandler.Request(gloFrameworkServerName, programId);

                if (aList != null && aList.Count > 0)
                {
                    foreach (Hashtable hTable in aList)
                    {
                        if (resultList == null) resultList = new List<Hashtable>();
                        resultList.Add(hTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

            return resultList;
        }

        /// <summary>
        /// Return Result DataTable without Mapping Control
        /// </summary>
        /// <param name="p_sProgramId"></param>
        /// <param name="p_menuID"></param>
        /// <param name="p_hKey"></param>
        /// <returns></returns>
        public static DataTable RequestHandlerDataTable(String p_sProgramId, String p_menuID, Hashtable p_hKey)
        {
            return RequestHandlerDataTable(p_sProgramId, p_menuID, p_hKey, null);
        }

        public static DataTable RequestHandlerDataTable(String p_sProgramId, String p_menuID, Hashtable p_hKey, Object devExpressObject)
        {
            DataTable table = new DataTable("LIST");

            try
            {
                ArrayList aList = null;

                if (p_hKey != null)
                    aList = BaseRequestHandler.Request(gloFrameworkServerName, p_sProgramId, p_menuID, p_hKey);
                else
                    aList = BaseRequestHandler.Request(gloFrameworkServerName, p_sProgramId, p_menuID);

                if(devExpressObject is DevExpress.XtraGrid.GridControl)
                {
                    foreach (DevExpress.XtraGrid.Columns.GridColumn col
                    in (((DevExpress.XtraGrid.GridControl)devExpressObject).MainView as DevExpress.XtraGrid.Views.Grid.GridView).Columns)
                    {
                        table.Columns.Add(col.FieldName);
                    }
                }
                else if(devExpressObject is DevExpress.XtraTreeList.TreeList)
                {

                    foreach (DevExpress.XtraTreeList.Columns.TreeListColumn col in (devExpressObject as DevExpress.XtraTreeList.TreeList).Columns)
                    {
                        table.Columns.Add(col.FieldName);
                    }
                    table.Columns.Add((devExpressObject as DevExpress.XtraTreeList.TreeList).ParentFieldName);
                }

                if ((table.Columns == null || table.Columns.Count == 0)
                    && aList != null && aList.Count > 0)
                {
                    // If table doesn't have any column, generate columns using result.
                    // Added for RequestHandlerDataTable(String p_sProgramId, String p_menuID, Hashtable p_hKey)
                    Hashtable hTable = aList[0] as Hashtable;
                    foreach(string key in hTable.Keys)
                    {
                        table.Columns.Add(new DataColumn(key));
                    }
                }

                if (aList != null && aList.Count > 0)
                {
                    foreach (Hashtable hTable in aList)
                    {
                        DataRow dataRow = table.NewRow();
                        foreach (DataColumn dColumn in table.Columns)
                        {
                            dataRow[dColumn.ToString()] = (string)hTable[dColumn.ToString()];
                        }
                        table.Rows.Add(dataRow);
                    }
                }

                table.AcceptChanges();
            }
            catch (HMMException ex)
            {
                //CommFunc.ShowExceptionBox(ex);
                throw ex;
            }

            return table;
        }

        /// <summary>
        /// 쿼리 실행후 정의된 Grid필드명에 따라 데이터를 맞춰넣음
        /// </summary>
        /// <param name="p_sProgramId"></param>
        /// <param name="p_hKey"></param>
        /// <param name="p_gridControl"></param>
        public static void RequestHandlerGridView(String p_sProgramId, String p_menuID, Hashtable p_hKey, DevExpress.XtraGrid.GridControl p_gridControl)
        {
            try
            {
                DataTable table = RequestHandlerDataTable(p_sProgramId, p_menuID, p_hKey, p_gridControl);
                p_gridControl.DataSource = table;
            }
            catch (HMMException ex)
            {
                //CommFunc.ShowExceptionBox(ex);
                throw ex;
            }
            finally
            {
            }
        }

        public static void RequestHandlerTreeList(String p_sProgramId, String p_menuID, Hashtable p_hKey, DevExpress.XtraTreeList.TreeList p_treeList)
        {
            try
            {
                DataTable table = RequestHandlerDataTable(p_sProgramId, p_menuID, p_hKey, p_treeList);
                p_treeList.DataSource = table;
            }
            catch (HMMException ex)
            {
                //CommFunc.ShowExceptionBox(ex);
                throw ex;
            }
            finally
            {
            }
        }

        public static string GetComboExpr(Object pCtrl)
        {
            string sValue = String.Empty;

            if (pCtrl is ComboBoxEdit)
            {
                ComboBoxEdit cmbList = (ComboBoxEdit)pCtrl;
                if (cmbList.SelectedItem != null)
                    sValue = ((comboboxDisplayValue)cmbList.SelectedItem).EXPR.ToString();
            }
            else if (pCtrl is BarEditItem)
            {
                BarEditItem barItem = pCtrl as BarEditItem;
                if (barItem.EditValue != null)
                    sValue = ((comboboxDisplayValue)barItem.EditValue).EXPR.ToString();
            }

            return sValue;
        }

        public static string GetComboValue(Object pCtrl)
        {
            string sValue = String.Empty;

            if (pCtrl is ComboBoxEdit)
            {
                ComboBoxEdit cmbList = (ComboBoxEdit)pCtrl;
                if(cmbList.SelectedItem != null)
                    sValue = ((comboboxDisplayValue)cmbList.SelectedItem).VALUE.ToString();
            }
            else if (pCtrl is BarEditItem)
            {
                BarEditItem barItem = pCtrl as BarEditItem;
                if (barItem.EditValue != null)
                    sValue = ((comboboxDisplayValue)barItem.EditValue).VALUE.ToString();
            }
            else if (pCtrl is ToolStripComboBox)
            {
                ToolStripComboBox cmbList = (ToolStripComboBox)pCtrl;
                if (cmbList.SelectedItem != null)
                    sValue = ((comboboxDisplayValue)cmbList.SelectedItem).VALUE.ToString();
            }
            else if (pCtrl is System.Windows.Forms.ComboBox)
            {
                System.Windows.Forms.ComboBox cmbList = (System.Windows.Forms.ComboBox)pCtrl;
                if (cmbList.SelectedItem != null)
                    sValue = ((comboboxDisplayValue)cmbList.SelectedItem).VALUE.ToString();
            }

            return sValue;
        }

        public static void SetTerminalComboBox(Object pCtrl, String pBaseTmnCod)
        {
            SetTerminalComboBox(pCtrl, pBaseTmnCod, false, false, false);
        }

        public static void SetTerminalComboBox(Object pCtrl, String pBaseTmnCod, Boolean pCheckAuthority, Boolean AllEnable, Boolean OnlyBaseTmnCod)
        {
            Hashtable hTable = new Hashtable();
            DataTable dt = new DataTable();
            DataColumn[] dc = new DataColumn[] { new DataColumn("WHSNAME"), new DataColumn("WHS"), new DataColumn("CUSTOMBONDW") };
            dt.Columns.AddRange(dc);
            Boolean bAllowAll = true;

            try
            {
                //1. SY_WHS 조회
                hTable.Add("OWN_TAG", "Y");
                ArrayList arrList = BaseRequestHandler.Request(gloFrameworkServerName, "SKIT-APP-COD-S-LSTTERMINALWHS", g_ID, hTable);


                List<SY_WHS> whsList = Handler_SY_WHS.GetWhsInformation(CommFunc.gloFrameworkServerName, "", "Y");
                foreach (SY_WHS terminal in whsList)
                {
                    if (OnlyBaseTmnCod)
                    {
                        if (pBaseTmnCod == terminal.WHS)
                            dt.Rows.Add(new string[] { terminal.WHSNAME, terminal.WHS, terminal.GetCustBond() });
                    }
                    else if (!pCheckAuthority)
                        dt.Rows.Add(new string[] { terminal.WHSNAME, terminal.WHS, terminal.GetCustBond() });
                    else
                        bAllowAll = false;
                }
                if (AllEnable && bAllowAll && !OnlyBaseTmnCod)
                {
                    dt.Rows.Add("ALL", "%%%");
                }

                if (pCtrl is ComboBoxEdit)
                {
                    ComboBoxEdit cmbList = (ComboBoxEdit)pCtrl;
                    cmbList.Properties.Items.Clear();
                    DevExpress.XtraEditors.Controls.ComboBoxItemCollection col = cmbList.Properties.Items;
                    col.BeginUpdate();
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            comboboxDisplayValue cmbValue = new comboboxDisplayValue(row[0].ToString(), row[1].ToString(), row[2].ToString());
                            col.Add(cmbValue);
                            if (row[1].ToString() == pBaseTmnCod)
                            {
                                cmbList.SelectedItem = cmbValue;
                            }
                        }
                    }
                    finally
                    {
                        col.EndUpdate();
                    }
                }
                else if (pCtrl is BarEditItem)
                {
                    BarEditItem barItem = pCtrl as BarEditItem;
                    RepositoryItemComboBox cmbList = (pCtrl as BarEditItem).Edit as RepositoryItemComboBox;
                    cmbList.Items.Clear();
                    DevExpress.XtraEditors.Controls.ComboBoxItemCollection col = cmbList.Items;
                    col.BeginUpdate();
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            comboboxDisplayValue cmbValue = new comboboxDisplayValue(row[0].ToString(), row[1].ToString(), row[2].ToString());
                            col.Add(cmbValue);
                            if (row[1].ToString() == pBaseTmnCod)
                            {
                                barItem.EditValue = cmbValue;
                            }
                        }
                    }
                    finally
                    {
                        col.EndUpdate();
                    }
                }
                else if (pCtrl is RepositoryItemComboBox)
                {
                    RepositoryItemComboBox cmbList = (RepositoryItemComboBox)pCtrl;
                    cmbList.Items.Clear();
                    DevExpress.XtraEditors.Controls.ComboBoxItemCollection col = cmbList.Items;
                    col.BeginUpdate();
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            col.Add(new comboboxDisplayValue(row[0].ToString(), row[1].ToString(), row[2].ToString()));
                        }
                    }
                    finally
                    {
                        col.EndUpdate();
                    }
                }
                else if (pCtrl is LookUpEdit)
                {
                    LookUpEdit cmbList = (LookUpEdit)pCtrl;
                    cmbList.Properties.DisplayMember = dt.Columns["WHSNAME"].ColumnName;
                    cmbList.Properties.ValueMember = dt.Columns["WHS"].ColumnName;
                    cmbList.Properties.DataSource = dt.DefaultView;
                }
                else if (pCtrl is RepositoryItemLookUpEdit)
                {
                    RepositoryItemLookUpEdit cmbList = (RepositoryItemLookUpEdit)pCtrl;
                    cmbList.DisplayMember = dt.Columns["WHSNAME"].ColumnName;
                    cmbList.ValueMember = dt.Columns["WHS"].ColumnName;
                    cmbList.DataSource = dt.DefaultView;
                }
                else if (pCtrl is Hitops_ComboBox)
                {
                    Hitops_ComboBox cmbList = (Hitops_ComboBox)pCtrl;
                    cmbList.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        cmbList.Items.Add(new comboboxDisplayValue(row[0].ToString(), row[1].ToString()));
                    }

                    //Default 지정
                    if (pBaseTmnCod != String.Empty)
                    {
                        for (int i = 0; i < cmbList.Items.Count; i++)
                        {
                            comboboxDisplayValue item = (comboboxDisplayValue)cmbList.Items[i];
                            if (item.VALUE == pBaseTmnCod)
                            {
                                cmbList.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                else if (pCtrl is System.Windows.Forms.ComboBox)
                {
                    System.Windows.Forms.ComboBox cmbList = (System.Windows.Forms.ComboBox)pCtrl;
                    cmbList.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        cmbList.Items.Add(new comboboxDisplayValue(row[0].ToString(), row[1].ToString()));
                    }

                    //Default 지정
                    if (pBaseTmnCod != String.Empty)
                    {
                        for (int i = 0; i < cmbList.Items.Count; i++)
                        {
                            comboboxDisplayValue item = (comboboxDisplayValue)cmbList.Items[i];
                            if (item.VALUE == pBaseTmnCod)
                            {
                                cmbList.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                else if (pCtrl is ToolStripComboBox)
                {
                    ToolStripComboBox cmbList = (ToolStripComboBox)pCtrl;
                    cmbList.Items.Clear();
                    System.Windows.Forms.ComboBox col = cmbList.ComboBox;
                    foreach (DataRow row in dt.Rows)
                    {
                        col.Items.Add(new comboboxDisplayValue(row[0].ToString(), row[1].ToString()));
                    }

                    //Default 지정
                    if (pBaseTmnCod != String.Empty)
                    {
                        for (int i = 0; i < cmbList.ComboBox.Items.Count; i++)
                        {
                            comboboxDisplayValue item = (comboboxDisplayValue)cmbList.ComboBox.Items[i];
                            if (item.VALUE == pBaseTmnCod)
                            {
                                cmbList.ComboBox.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show(ex.Message1);
            }
        }
        
        public static String getTmnCodSelectedValue(Object pCtrl)
        {
            String result = String.Empty;

            if (pCtrl is ComboBoxEdit)
            {
                ComboBoxEdit cmbList = (ComboBoxEdit)pCtrl;
                result = ((comboboxDisplayValue)cmbList.SelectedItem).VALUE;
            }
            else if (pCtrl is System.Windows.Forms.ComboBox)
            {
                System.Windows.Forms.ComboBox cmbList = (System.Windows.Forms.ComboBox)pCtrl;
                result = ((comboboxDisplayValue)cmbList.SelectedItem).VALUE;
            }
            else if (pCtrl is ToolStripComboBox)
            {
                ToolStripComboBox cmbList = (ToolStripComboBox)pCtrl;
                result = ((comboboxDisplayValue)cmbList.SelectedItem).VALUE;
            }
            else if (pCtrl is BarEditItem)
            {
                BarEditItem cmbList = (BarEditItem)pCtrl;
                result = ((comboboxDisplayValue)cmbList.EditValue).VALUE;
            }

            return result;
        }

        /// <summary>
        /// 메뉴[ToolStrip or BarItems(BarManager.Items)]를 유저권한에 따라 Visible/Unvisible
        /// </summary>
        /// <param name="pBar">ToolStrip or BarItems</param>
        public static void ApplyMenuAuthority(Object pBar)
        {
            Hashtable hTable = new Hashtable();
            hTable.Add("TMN_COD", CommFunc.gloTMLCod);
            hTable.Add("U_TMN_COD", CommFunc.gloUserTMLCod);
            hTable.Add("USER_ID", CommFunc.gloUserID);
            ArrayList aList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "", hTable);
            if (pBar is BarItems)
            {
                BarItems bar = pBar as BarItems;
                foreach (BarItem item in bar)
                {
                    foreach (Hashtable sRcd in aList)
                    {
                        if (item.Name == sRcd["MENU_ID"].ToString())
                        {
                            //권한이 부여된 메뉴는 보이게 해준다.
                            if (sRcd["ENABLE"].ToString() == "Y")
                            {
                                item.Enabled = true;
                                item.Visibility = BarItemVisibility.Always;
                            }
                            else
                            {
                                item.Enabled = false;
                                item.Visibility = BarItemVisibility.Never;
                            }
                            break;
                        }
                    }                    
                }
            }
            else if(pBar is ToolStrip)
            {
                ToolStrip bar = pBar as ToolStrip;
                foreach(ToolStripItem item in bar.Items)
                {
                    foreach (Hashtable sRcd in aList)
                    {
                        if (item.Name == sRcd["MENU_ID"].ToString())
                        {
                            //권한이 부여된 메뉴는 보이게 해준다.
                            if (sRcd["ENABLE"].ToString() == "Y")
                            {
                                item.Enabled = true;
                                item.Visible = true;
                            }
                            else
                            {
                                item.Enabled = false;
                                item.Visible = false;
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMenuID"></param>
        /// <param name="pTmnCod"></param>
        /// <returns></returns>
        public static bool checkMenuAuthority(String pMenuID, String pTmnCod)
        {
            Boolean bAuth = false;

            //TEST 권한비활성
            return true;

            try
            { 
                Hashtable hTable = new Hashtable();
                hTable.Add("USER_ID", CommFunc.gloUserID);
                hTable.Add("MENU_ID", pMenuID);
                hTable.Add("APPLY_TERMINAL", pTmnCod);

                ArrayList aList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-ADM-USR-S-LSTMENUAUTHORITY", hTable);

                if(aList.Count>0)
                {
                    String sGroupEnable = "-"; //Null 을 의미
                    String sUserEnable = "-"; //Null 을 의미

                    foreach (Hashtable hRec in aList)
                    {
                        //User 권한이 Y이면 무조건 권한 있음
                        if(hRec["TYPE"].ToString()=="USER")
                            sUserEnable = hRec["ENABLED"].ToString();
                        else
                            sGroupEnable = hRec["ENABLED"].ToString();
                    }

                    if(sUserEnable=="Y")
                    {
                        bAuth = true;
                    }else
                    {
                        //Group 권한이 있어도 User 권한이 명시적으로 N로 주어지면 권한없음
                        if(sGroupEnable=="Y" && sUserEnable=="-")
                        {
                            bAuth = true;
                        }
                    }
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show(ex.Message1);
            }

            return bAuth;
        }

        /// <summary>
        /// XtraGrid 중앙정렬
        /// </summary>
        /// <param name="grdView"></param>
        public static void setXtraGridHeaderAlignCenter(DevExpress.XtraGrid.Views.Grid.GridView grdView)
        {
            for (int i = 0; i < grdView.Columns.Count; i++)
            {
                grdView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBoxEdit"></param>
        /// <param name="value"></param>
        /// <param name="forceMatch">Value가 맞는게 없으면 강제로 만들어서 붙임</param>
        /// <returns>선택되면 True, 없으면 false</returns>
        public static bool SelectComboBoxItemByValue(DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit, string value, bool forceMatch = false)
        {
            bool bMatched = false;
            foreach (comboboxDisplayValue item in comboBoxEdit.Properties.Items)
            {
                if (item.VALUE == value)
                {
                    comboBoxEdit.SelectedItem = item;
                    bMatched = true;
                    break;
                }
            }
            if (!bMatched && forceMatch)
            {
                int idx = comboBoxEdit.Properties.Items.Add(new CommFunc.comboboxDisplayValue("", value));
                comboBoxEdit.SelectedItem = comboBoxEdit.Properties.Items[idx];
                bMatched = true;
            }

            return bMatched;
        }

        public static void SelectComboBoxItemByDisplay(DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit, string display)
        {
            foreach (comboboxDisplayValue item in comboBoxEdit.Properties.Items)
            {
                if (item.DISPLAY == display)
                {
                    comboBoxEdit.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Substring
        /// </summary>
        /// <param name="str">원본문자열</param>
        /// <param name="startIdx">시작인덱스</param>
        /// <param name="length">문자열 길이</param>
        /// <returns></returns>
        public static string SubString(string str, int startIdx = 0, int length = 0)
        {
            if (str == null || startIdx < 0 || length < 0)
            {
                return null;
            }

            // if start is greater than end, return ""
            if (startIdx >= str.Length)
            {
                return String.Empty;
            }

            if (length == 0 || startIdx + length > str.Length)
            {
                length = str.Length - startIdx;
            }

            return str.Substring(startIdx, length);
        }

        public static void CopyAllPropertyByName(object srcObj, object destObj)
        {
            if (srcObj == null || destObj == null) return;

            try
            {
                PropertyInfo[] srcProperties = srcObj.GetType().GetProperties();
                PropertyInfo[] destProperties = destObj.GetType().GetProperties();

                foreach (PropertyInfo destProp in destProperties)
                {
                    if (destProp.GetSetMethod() == null) continue;

                    PropertyInfo srcProp = srcProperties.Where(x => x.Name == destProp.Name && x.PropertyType == destProp.PropertyType).FirstOrDefault();
                    if (srcProp != null)
                    {
                        object srcValue = srcProp.GetValue(srcObj);
                        destProp.SetValue(destObj, srcValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public static readonly string DEFAULT_REPORT_FORMAT_PATH = AppDomain.CurrentDomain.BaseDirectory + "\\ReportFormat\\";

    }
}
