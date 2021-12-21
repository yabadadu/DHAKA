#region

using System;
using System.Collections;
using System.Collections.Generic;
using Hitops;
using HitopsCommon.Request;

#endregion

namespace HitopsCommon
{
    class TMNPORTCODE
    {
        static TMNPORTCODE _instance;
        List<TMN_PORT> mPortList = new List<TMN_PORT>();
        struct TMN_PORT
        {
            public String tmn_cod;
            public String tmn_port;
        }

        public String getTmnPort(String sTmnCode)
        {
            String sPort = "";
            for (int i = 0; i < mPortList.Count; i++)
            {
                if (mPortList[i].tmn_cod == sTmnCode)
                {
                    sPort = mPortList[i].tmn_port;
                    break;
                }
            }

            return sPort;
        }

        private TMNPORTCODE()
        {
            try
            {
                ArrayList aList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-CDS-DSN-S-LSTTMNPORT");
                foreach(Hashtable aTable in aList)
                {
                    TMN_PORT port = new TMN_PORT();
                    port.tmn_cod = aTable["TMN_COD"].ToString();
                    port.tmn_port = aTable["TMN_PORT"].ToString();
                    mPortList.Add(port);
                }
            }
            catch{}
            _instance = this;
        }

        public static TMNPORTCODE GetInstance()
        {
            if (_instance == null)
                _instance = new TMNPORTCODE();

            return _instance;
        }
    }
}
