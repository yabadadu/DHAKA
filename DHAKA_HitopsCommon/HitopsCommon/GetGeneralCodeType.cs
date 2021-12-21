#region

using System;

#endregion

namespace HitopsCommon
{
    static public  class GetGeneralCodeType
    {
        static String port = "PORT";
        static String port_only = "PORT_ONLY";
        static String portByRoute = "PORT_BY_ROUTE";
        static String portByVVD = " PORT_BY_VVD";

        static String route = "ROUTE";
        static String routeGroup = "ROUTEGROUP";

        static String block = "BLOCK";
        static String bay = "BAY";

        static String actOpr = "ACT_OPR";
        static String opr = "OPERATOR";
        static String nation = "NATION";

        static String cntrSize = "CNTR_SIZE";
        static String cntrTyp = "CNTR_TYPE";
        static String cargoTyp = "CARGO_TYPE";
        static String isoSztp = "ISO_SZTP";

        static String linerSztp = "LINER_SZTP";
        static String vessel = "VESSEL";
        static String vvd = "VVD";
        static String vvdNew = "VVDNEW";
        static String vvdAct = "VVDACT";
        static String vvdActAll = "VVDACTAll";
        static String vvdLatest = "VVD_LATEST";
        static String trucker = "TRUCKER";
        static String qc = "QC";
        static String yt = "YT";

        static String bitt = "BITT";
        static String berth = "BERTH";
        static String serviceArea = "SERVICE_AREA";
        static String lane = "LANE";
        static String damage = "DAMAGE";
        static String agsBasicCategory = "AGS_BASIC_CATEGORY";
        static String mashGrp = "MASH_GRP";

        static String cysOpr = "CYS_OPR";
        static String cyUsageCode = "CY_USAGE_COD";

        static String currency = "CURRENCY";
        static String poolScheEqpTyp = "POOL_SCHE_EQP_TYP";
        static String poolScheEqpNo = "POOL_SCHE_EQP_NO";
        static String tariffDueType = "TARIFF_DUE_TYP";
        static String vesselBay = "VESSEL_BAY";

        public static String PORT { get { return port; } }
        public static String PORT_ONLY { get { return port_only; } }
        public static String PORT_BY_ROUTE { get { return portByRoute; } }
        public static String PORT_BY_VVD { get { return portByVVD; } }

        public static String ROUTE { get { return route; } }
        public static String ROUTEGROUP { get { return routeGroup; } }

        public static String BLOCK { get { return block; } }
        public static String BAY { get { return bay; } }

        public static String ACT_OPR { get { return actOpr; } }
        public static String OPERATOR { get { return opr; } }
        public static String NATION { get { return nation; } }

        public static String CNTR_SIZE { get { return cntrSize; } }
        public static String CNTR_TYPE { get { return cntrTyp; } }
        public static String CARGO_TYPE { get { return cargoTyp; } }
        public static String ISO_SZTP { get { return isoSztp; } }

        public static String LINER_SZTP { get { return linerSztp; } }
        public static String VESSEL { get { return vessel; } }
        public static String VVD { get { return vvd; } }
        public static String VVDNEW { get { return vvdNew; } }
        public static String VVDACT { get { return vvdAct; } }
        public static String VVDACTALL { get { return vvdActAll; } }
        public static String VVD_LATEST { get { return vvdLatest; } }
        public static String TRUCKER { get { return trucker; } }
        public static String QC { get { return qc; } }
        public static String YT { get { return yt; } }

        public static String BITT { get { return bitt; } }
        public static String BERTH { get { return berth; } }
        public static String SERVICE_AREA { get { return serviceArea; } }
        public static String LANE { get { return lane; } }
        public static String DAMAGE { get { return damage; } }
        public static String AGS_BASIC_CATEGORY { get { return agsBasicCategory; } }
        public static String MASH_GRP { get { return mashGrp; } }

        public static String CYS_OPR { get { return cysOpr; } }
        public static String CY_USAGE_COD { get { return cyUsageCode; } }

        public static String CURRENCY { get { return currency; } }
        public static String POOL_SCHE_EQP_TYP { get { return poolScheEqpTyp; } }
        public static String POOL_SCHE_EQP_NO { get { return poolScheEqpNo; } }
        public static String TARIFF_DUE_TYP { get { return tariffDueType; } }
        public static String VESSEL_BAY { get { return vesselBay; } }

        //***********************************************************************
        //* 2009.12.17 SmileMan 추가
        //* DESC : Container Grade 항목 추가
        //***********************************************************************

        static String cntrGrade = "CNTR_GRADE";

        public static String CNTR_GRADE { get { return cntrGrade; } }

        //***********************************************************************

        //***********************************************************************
        //* 2009.12.22 jerry.Yeo 추가
        //* DESC : Container Hold Code, Gate No Code 항목 추가
        //***********************************************************************

        static String cntrHold = "CNTR_HOLD";
        static String gateno = "GATE_NO";

        public static String CNTR_HOLD { get { return cntrHold; } }
        public static String GATE_NO { get { return gateno; } }

        //***********************************************************************
    }
}
