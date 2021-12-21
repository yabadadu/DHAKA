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
    public class MsgProtocols
    {
        int i = 0;
        String _MID = "";
        private static MsgProtocols INSTANCE;
        List<MsgContract> mMsgtList = new List<MsgContract>();

        /// <summary>
        /// 필드가 비워진 MessageContract를 리턴
        /// </summary>
        /// <param name="msgCommand"></param>
        /// <param name="sGroup"></param>
        /// <param name="sCommand"></param>
        /// <returns></returns>
        public MsgContract getMsgContract(String msgCommand, String sGroup, String sCommand)
        {
            MsgContract contract = null;
            foreach (MsgContract msg in mMsgtList)
            {
                if (msg.USAGE == msgCommand && msg.GROUP == sGroup && msg.COMMAND == sCommand)
                {
                    contract = msg.Clone() as MsgContract;
                    contract.clearField();
                    break;
                }

            }

            return contract;
        }

        public MsgContract setMsgContractbyMsg(String sMessage)
        {
            MsgContract contract = null;
            String sMsgHead = null;

            try
            {

                foreach (MsgContract msgContract in mMsgtList)
                {
                    sMsgHead = msgContract.GROUP + msgContract.SEPARATOR + msgContract.COMMAND + msgContract.SEPARATOR;
                    if (sMsgHead == sMessage.Substring(0, sMsgHead.Length))
                    {
                        contract = msgContract;
                        contract.setValueByMsg(sMessage);
                    }
                }
            }
            catch (HMMException e)
            {
                //
            }

            return contract;
        }

        //2018.01.24 add by Ahn Jinsung. 새로 생성된 MsgContract 객체를 리턴하는 함수 추가
        public MsgContract getNewMsgContractbyMsg(String sMessage)
        {
            MsgContract contract = null;
            String sMsgHead = null;

            try
            {
                foreach (MsgContract msgContract in mMsgtList)
                {
                    sMsgHead = msgContract.GROUP + msgContract.SEPARATOR + msgContract.COMMAND + msgContract.SEPARATOR;
                    if (sMsgHead == sMessage.Substring(0, sMsgHead.Length))
                    {
                        contract = (MsgContract)msgContract.Clone();
                        contract.setValueByMsg(sMessage);
                    }
                }
            }
            catch (HMMException e)
            {
                //
            }

            return contract;
        }

        protected MsgProtocols()
        {
            Hashtable oldContract = null;
            ArrayList aList = null;
            ArrayList msgFieldList = new ArrayList();

            try
            {
                aList = BaseRequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-MOP-YTD-S-LSTALLYTPMSGCONTRACT", _MID);

                for (int i = 0; i < aList.Count; i++)
                {
                    MsgContract msg = null;
                    Hashtable contract = (Hashtable)aList[i];

                    //이전과  MSG_USAGE+MSG_GRP+MSG_COMMAND값이 바뀌었거나
                    //마지막이면 저장하던 MsgContract를 생성시킨다.
                    if (oldContract != null && (oldContract["MSG_USAGE"].ToString() != contract["MSG_USAGE"].ToString()
                        || oldContract["MSG_GRP"].ToString() != contract["MSG_GRP"].ToString()
                        || oldContract["MSG_COMMAND"].ToString() != contract["MSG_COMMAND"].ToString()))
                    {
                        msg = new MsgContract(oldContract["MSG_USAGE"].ToString(),
                            oldContract["MSG_GRP"].ToString(), oldContract["MSG_COMMAND"].ToString(), msgFieldList);
                        mMsgtList.Add(msg);
                        msgFieldList.Clear();
                    }

                    Hashtable hField = new Hashtable();
                    hField.Add("MSG_FIELD", contract["MSG_FIELD"].ToString());
                    hField.Add("FIELD_SEPARATOR", contract["FIELD_SEPARATOR"].ToString());
                    hField.Add("FIELD_INDEX", contract["FIELD_INDEX"].ToString());
                    msgFieldList.Add(hField);
                    oldContract = (Hashtable)contract.Clone();

                    //마지막
                    if (i == aList.Count - 1)
                    {
                        msg = new MsgContract(oldContract["MSG_USAGE"].ToString(),
                            oldContract["MSG_GRP"].ToString(), oldContract["MSG_COMMAND"].ToString(), msgFieldList);
                        mMsgtList.Add(msg);
                    }
                }
            }
            catch (HMMException e)
            {
                //
            }
        }

        public static MsgProtocols getInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new MsgProtocols();
            }
            return INSTANCE;
        }
    }
}
