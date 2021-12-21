using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops;
using Hitops.exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public static class Handler_CM_VESSEL
    {
        /// <summary>
        /// Make a CM_VESSEL class from database
        /// </summary>
        /// <param name="frameworkServer">FrameworkServerName</param>
        /// <param name="sVessel">Vessel</param>
        /// <returns></returns>
        public static CM_VESSEL GetVesselInformation(string frameworkServer, string sVessel)
        {
            CM_VESSEL oVessel = new CM_VESSEL();

            Hashtable hReq = new Hashtable();
            hReq.Add("VESSEL", sVessel);
            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VSL-S-GETVESSELINFO", hReq);
                if (aList.Count > 0)
                {
                    BindDB2Class.BindDBHashtable2Class((Hashtable)aList[0], oVessel);
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return oVessel;
        }

        public static List<CM_VESSEL> GetVesselInformationList(string frameworkServer, string sVessel)
        {
            List<CM_VESSEL> aVessel = null;
            Hashtable hReq = new Hashtable();

            try
            {
                hReq.Add("VESSEL", sVessel);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VSL-S-GETVESSELINFO", hReq);
                if (aList != null)
                {
                    aVessel = BindDB2Class.BindDBArrayList2Class(aList, new CM_VESSEL());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aVessel;
        }
    }
}
