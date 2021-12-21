#region

using System;
using System.Collections;
using System.Collections.Generic;
using Hitops;
using Hitops.exception;
using HitopsCommon.Request;

#endregion

namespace HitopsCommon
{
    /// <summary>
    /// 메세지 Command단위 클래스
    /// 전체 Command 리스트는 MsgProtocols으로 관리됨
    /// </summary>
    public class MsgContract : ICloneable
    {
        int i = 0;
        String _MID = "";
        private String msgUsage = "";   //메세지용도 YTP(YT-Pooling), RT(Real-Time) 등
        private String msgGroup = "";   //메세지그룹 ST(ServerToTerminal), TS(TerminalToServer), JB(Job), EQ, TC 등
        private String msgCommand = ""; //메세지의 비지니스모델 헤더
        private const String separator = "↔";	//구분자
	    private LinkedList<MsgField> msgFieldList = new LinkedList<MsgField>(); //메세지필드 리스트

        public object Clone()
        {
            //2018.01.24 modified by Ahn Jinsung. LinkedList에 대해 얕은 복사에서 깊은 복사로 수정
            MsgContract oClone = (MsgContract)this.MemberwiseClone();
            oClone.msgFieldList = new LinkedList<MsgField>();

            foreach(MsgField oMF in msgFieldList)
            {
                MsgField oNewMF = new MsgField();
                oNewMF.fieldIndex = oMF.fieldIndex;
                oNewMF.msgField = oMF.msgField;
                oNewMF.fieldSeparator = oMF.fieldSeparator;

                oClone.msgFieldList.AddLast(oNewMF);
            }

            return oClone;
        }

        //메세지필드
        class MsgField
        {
            public int fieldIndex = 0;          //메세지의 순번
            public String msgField = "";        //메세지필드헤더
            public String msgValue = "";        //메세지필드헤더
            public String fieldSeparator = "↔"; //메세지필드의 끝에 붙이는 구분자
        }

        public String Header
        {
            get { return msgGroup + separator + msgCommand; }
        }

        public String GroupCmd
        {
            get { return msgGroup + msgCommand; }
        }

        public String USAGE
        {
            get { return msgUsage; }
            //set { msgUsage = value; }
        }

        public String GROUP
        {
            get { return msgGroup; }
            //set { msgGroup = value; }
        }

        public String COMMAND
        {
            get { return msgCommand; }
            //set { msgCommand = value; }
        }

        public String SEPARATOR
        {
            get { return separator; }
        }

        public MsgContract(String sMsgUsage, String sMsgGroup, String sMsgCommand)
        {
            msgUsage = sMsgUsage;
            msgGroup = sMsgGroup;
            msgCommand = sMsgCommand;

            Hashtable hField;
            Hashtable hReq = new Hashtable();
            try
            {
                hReq.Add("MSG_USAGE", msgUsage);
                hReq.Add("MSG_GRP", msgGroup);
                hReq.Add("MSG_COMMAND", msgCommand);
                ArrayList aList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-ENG-WMRCOM-S-LSTMSGFIELDLIST", _MID, hReq);
                for (int i = 0; i < aList.Count; i++)
                {
                    hField = (Hashtable)aList[i];

                    MsgField field = new MsgField();
                    field.msgField = hField["MSG_FIELD"].ToString();
                    field.fieldSeparator = hField["FIELD_SEPARATOR"].ToString();
                    field.fieldIndex = CommFunc.ConvertToInt(hField["FIELD_INDEX"].ToString());
                    msgFieldList.AddLast(field);
                }
            }
            catch (HMMException e)
            {
                //
            }
        }

        public MsgContract(String sMsgUsage, String sMsgGroup, String sMsgCommand, ArrayList msgContractList)
        {
            msgUsage = sMsgUsage;
            msgGroup = sMsgGroup;
            msgCommand = sMsgCommand;

            Hashtable hField;
            Hashtable hReq = new Hashtable();
            try
            {
                hReq.Add("MSG_USAGE", msgUsage);
                hReq.Add("MSG_GRP", msgGroup);
                hReq.Add("MSG_COMMAND", msgCommand);
                for (int i = 0; i < msgContractList.Count; i++)
                {
                    hField = (Hashtable)msgContractList[i];

                    MsgField field = new MsgField();
                    field.msgField = hField["MSG_FIELD"].ToString();
                    field.fieldSeparator = hField["FIELD_SEPARATOR"].ToString();
                    field.fieldIndex = CommFunc.ConvertToInt(hField["FIELD_INDEX"].ToString());
                    msgFieldList.AddLast(field);
                }
            }
            catch (HMMException e)
            {
                //
            }
        }

        public void clearField()
        {
            foreach (MsgField field in msgFieldList)
            {
                field.msgValue = "";
            }
        }

        public ArrayList getFieldList()
        {
            ArrayList aList = new ArrayList();

            foreach(MsgField field in msgFieldList)
            {
                aList.Add(field.msgField);
            }

            return aList;
        }

        public String getValue(String sMsgField)
        {
            String sRtn = "";
            try
            {
                foreach(MsgField field in msgFieldList)
                {
                    if (field.msgField == sMsgField)
                    {
                        sRtn = field.msgValue;
                        break;
                    }
                }
            }
            catch (Exception e)
            {

            }

            return sRtn;
        }

        public void setValue(String sMsgField, String sMsgValue)
        {
            try
            {
                foreach(MsgField field in msgFieldList)
                {
                    if (field.msgField == sMsgField)
                    {
                        field.msgValue = sMsgValue;
                        break;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public Boolean setValueByMsg(String sMsg)
        {
            Boolean bRst = true;
            String sHandleMsg = null;

            if (sMsg.Length < 7)
                return false;

            try
            {
                //1. Save Field Message without Header
                sHandleMsg = sMsg.Substring((msgGroup + SEPARATOR + msgCommand + SEPARATOR).Length);

                //2. Save Field Values to msgFieldList
                foreach(MsgField field in msgFieldList)
                {
                    if (sHandleMsg.IndexOf(field.fieldSeparator) < 0)
                        break;
                    field.msgValue = sHandleMsg.Substring(0, sHandleMsg.IndexOf(field.fieldSeparator));
                    sHandleMsg = sHandleMsg.Substring(sHandleMsg.IndexOf(field.fieldSeparator) + field.fieldSeparator.Length);
                }
            }
            catch (Exception e)
            {
                bRst = false;
            }

            return bRst;
        }

        public String getMsg()
        {
            String sMsg = "";

            try
            {
                sMsg = msgGroup + SEPARATOR + msgCommand + SEPARATOR;
                foreach(MsgField field in msgFieldList)
                {
                    sMsg += field.msgValue + field.fieldSeparator;
                }
            }
            catch (Exception e)
            {
                sMsg = "";
            }

            return sMsg;
        }
    }
}
