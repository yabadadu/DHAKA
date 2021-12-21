#region

using System;

#endregion

namespace HitopsCommon
{
    public static class SetTextFormatType
    {
        static String sInt = "INT";
        static String sLong = "LONG";
        static String sDouble = "DOUBLE";
        static String sCatpital = "CAPITAL";
        static String sChar = "CHAR";
        static String sUpperChar = "UPPERCHAR";

        public static String INT { get { return sInt; } }
        public static String LONG { get { return sLong; } }
        public static String DOUBLE { get { return sDouble; } }
        public static String CAPITAL { get { return sCatpital; } }
        public static String CHAR { get { return sChar; } }
        public static String UPPERCHAR { get { return sUpperChar; } }
    }
}
