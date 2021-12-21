using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class MA_PARTNER : DB_TABLE
    {
        private const string DISPLAY_FORMAT = "{0} [{1}]";

        public string CD_COMPANY { get; set; } // Company Code

        public string CD_PARTNER { get; set; } // Partner Code

        public string LN_PARTNER { get; set; } // Partner Name

        public string LN_PARTNER2 { get; set; } // Partner Name, LookUp Display용

        public string FG_PARTNER { get; set; } // Partner Type

        public string CEO_NAME { get; set; }

        public string ZIP_CODE { get; set; }

        public string ADDR { get; set; }

        public string ADDR_DETAIL { get; set; }

        public string TEL_NO { get; set; }

        public string REG_NO { get; set; }

        public string BIZ_CATEGORY { get; set; }

        public string BIZ_TYPE { get; set; }

        public string BIZ_DATE { get; set; }

        public string LN_PARTNER_CD_PARTNER
        {
            get
            {
                string resultValue = this.LN_PARTNER;
                
                if (string.IsNullOrEmpty(this.CD_PARTNER) == false)
                {
                    resultValue = string.Format(DISPLAY_FORMAT, this.LN_PARTNER, this.CD_PARTNER);
                }

                return resultValue;
            }
        }

        public string CD_PARTNER_LN_PARTNER
        {
            get
            {
                string resultValue = this.CD_PARTNER;

                if (string.IsNullOrEmpty(this.LN_PARTNER) == false)
                {
                    resultValue = string.Format(DISPLAY_FORMAT, this.CD_PARTNER, this.LN_PARTNER);
                }

                return resultValue;
            }
        }


        public string SN_PARTNER { get; set; }

        public string SN_PARTNER2 { get; set; }

        public string SN_PARTNER_CD_PARTNER
        {
            get
            {
                string resultValue = this.SN_PARTNER;

                if (string.IsNullOrEmpty(this.CD_PARTNER) == false)
                {
                    resultValue = string.Format(DISPLAY_FORMAT, this.SN_PARTNER, this.CD_PARTNER);
                }

                return resultValue;
            }
        }

        public string CD_PARTNER_SN_PARTNER
        {
            get
            {
                string resultValue = this.CD_PARTNER;

                if (string.IsNullOrEmpty(this.SN_PARTNER) == false)
                {
                    resultValue = string.Format(DISPLAY_FORMAT, this.CD_PARTNER, this.SN_PARTNER);
                }

                return resultValue;
            }
        }
    }
}
