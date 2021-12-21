#region

using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#endregion

namespace HitopsCommon
{
    public class IniFileTool
    {
        private String g_sIniFilePath = "";
        public String Path
        {
            get { return g_sIniFilePath; }
        }

        private String g_sIniDirPath = String.Empty;
        private String g_sIniFileNm = String.Empty;

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
                                                               uint nSize,
                                                               string lpFileName);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName,
                                                          string lpKeyName,
                                                          string lpDefault,
                                                          [In, Out] char[] lpReturnedString,
                                                          int nSize,
                                                          string lpFileName);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileString(string lpAppName,
                                                         string lpKeyName,
                                                         string lpDefault,
                                                         IntPtr lpReturnedString,
                                                         uint nSize,
                                                         string lpFileName);
        public const int MaxSectionSize = 32767; // 32 KB
        /// <summary>

        /// INIFile Constructor.

        /// </summary>

        /// <PARAM name="INIPath"></PARAM>

        public IniFileTool(String pFileAddr)
        {
            g_sIniFilePath = pFileAddr;
        }

        public IniFileTool(String p_sAppStartPath, String p_sIniDir, String p_sIniFileNm)
        {
            g_sIniDirPath = p_sAppStartPath + "/" + p_sIniDir;
            g_sIniFileNm = p_sIniFileNm;
            g_sIniFilePath = g_sIniDirPath + "/" + p_sIniFileNm;
        }


        /// <summary>

        /// Write Data to the INI File

        /// </summary>

        /// <PARAM name="Section"></PARAM>

        /// Section name

        /// <PARAM name="Key"></PARAM>

        /// Key Name

        /// <PARAM name="Value"></PARAM>

        /// Value Name

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.g_sIniFilePath);
        }

        /// <summary>

        /// Read Data Value From the Ini File

        /// </summary>

        /// <PARAM name="Section"></PARAM>

        /// <PARAM name="Key"></PARAM>

        /// <PARAM name="Path"></PARAM>

        /// <returns></returns>

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.g_sIniFilePath);

            return temp.ToString();
        }

        public void IniDeleteSection(string Section)
        {
            if (Section == null)
                throw new ArgumentNullException("Section");

            WriteValueInternal(Section, null, null);
        }

        public void IniDeleteKey(string Section, string Key)
        {
            if (Section == null)
                throw new ArgumentNullException("Section");

            if (Key == null)
                throw new ArgumentNullException("Key");

            WriteValueInternal(Section, Key, null);
        }

        private void WriteValueInternal(string sectionName, string keyName, string value)
        {
            if (!WritePrivateProfileString(sectionName, keyName, value, g_sIniFilePath))
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public ArrayList GetSectionNames()
        {
            ArrayList aRetVal = new ArrayList();

            string[] retval;
            int len;

            //Allocate a buffer for the returned section names.
            IntPtr ptr = Marshal.AllocCoTaskMem(IniFileTool.MaxSectionSize);

            try
            {
                //Get the section names into the buffer.
                len = GetPrivateProfileSectionNames(ptr,
                    IniFileTool.MaxSectionSize, g_sIniFilePath);

                retval = ConvertNullSeperatedStringToStringArray(ptr, len);

                aRetVal.AddRange(retval);
            }
            finally
            {
                //Free the buffer
                Marshal.FreeCoTaskMem(ptr);
            }

            return aRetVal;
        }

        public ArrayList GetKeyNames(string sectionName)
        {
            ArrayList aRetVal = new ArrayList();
            
            int len;
            string[] retval;

            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            //Allocate a buffer for the returned section names.
            IntPtr ptr = Marshal.AllocCoTaskMem(IniFileTool.MaxSectionSize);

            try
            {
                //Get the section names into the buffer.
                len = GetPrivateProfileString(sectionName,
                                                            null,
                                                            null,
                                                            ptr,
                                                            IniFileTool.MaxSectionSize,
                                                            g_sIniFilePath);

                retval = ConvertNullSeperatedStringToStringArray(ptr, len);
                aRetVal.AddRange(retval);
            }
            finally
            {
                //Free the buffer
                Marshal.FreeCoTaskMem(ptr);
            }

            return aRetVal;
        }

        private static string[] ConvertNullSeperatedStringToStringArray(IntPtr ptr, int valLength)
        {
            string[] retval;

            if (valLength == 0)
            {
                //Return an empty array.
                retval = new string[0];
            }
            else
            {
                //Convert the buffer into a string.  Decrease the length 
                //by 1 so that we remove the second null off the end.
                string buff = Marshal.PtrToStringAuto(ptr, valLength - 1);

                //Parse the buffer into an array of strings by searching for nulls.
                retval = buff.Split('\0');
            }

            return retval;
        }

        public Boolean CheckIniDir()
        {
            Boolean Flag = false;

            if (!Directory.Exists(g_sIniDirPath))
            {
                try
                {
                    Directory.CreateDirectory(g_sIniDirPath);
                    return true;
                }
                catch
                {
                    MessageBox.Show("Can't Create " + g_sIniDirPath + " Directory");
                }
            }
            else
            {
                return true;
            }

            return Flag;
        }

        public Boolean CheckIniFile()
        {
            Boolean Flag = false;

            if (!File.Exists(g_sIniFilePath))
            {
                DialogResult dr = MessageBox.Show("Do you want to create " + g_sIniFileNm + " file?", "Confirm", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        File.AppendAllText(g_sIniFilePath, String.Empty);
                        Flag = true;
                    }
                    catch
                    {
                        MessageBox.Show("Can't Create " + g_sIniFilePath + " File");
                    }
                }
            }
            else
            {
                Flag = true;
            }

            return Flag;
        }

        public Boolean InsertSection(String p_sSection)
        {
            try
            {
                File.AppendAllText(g_sIniFilePath, Environment.NewLine + "[" + p_sSection + "]");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
