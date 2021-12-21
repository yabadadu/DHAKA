using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_PUM_DTL : DB_TABLE
    {
        #region INITIALIZE
        public SY_PUM_DTL() : base()
        {
        }
        #endregion

        #region PROPERTIES
        public string PUM_NO { get; set; }

        public string DTL_SEQ { get; set; }

        public string HEIGHT { get; set; }

        public Nullable<int> HEIGHTVALUE
        {
            get
            {
                Nullable<int> resultValue = null;

                if (string.IsNullOrEmpty(this.HEIGHT) == false)
                {
                    int intValue;
                    if (int.TryParse(this.HEIGHT, out intValue) == true)
                    {
                        resultValue = intValue;
                    }
                }

                return resultValue;
            }
            set { this.HEIGHT = value.HasValue ? value.ToString() : null; }
        }

        public string WIDTH { get; set; }

        public Nullable<int> WIDTHVALUE
        {
            get
            {
                Nullable<int> resultValue = null;

                if (string.IsNullOrEmpty(this.WIDTH) == false)
                {
                    int intValue;
                    if (int.TryParse(this.WIDTH, out intValue) == true)
                    {
                        resultValue = intValue;
                    }
                }

                return resultValue;
            }
            set { this.WIDTH = value.HasValue ? value.ToString() : null; }
        }

        public string LENGTH { get; set; }

        public Nullable<int> LENGTHVALUE
        {
            get
            {
                Nullable<int> resultValue = null;

                if (string.IsNullOrEmpty(this.LENGTH) == false)
                {
                    int intValue;
                    if (int.TryParse(this.LENGTH, out intValue) == true)
                    {
                        resultValue = intValue;
                    }
                }

                return resultValue;
            }
            set { this.LENGTH = value.HasValue ? value.ToString() : null; }
        }

        public string UNIT_WGT { get; set; }

        public Nullable<double> UNIT_WGT_VALUE
        {
            get
            {
                Nullable<double> resultValue = null;

                if (string.IsNullOrEmpty(this.UNIT_WGT) == false)
                {
                    double doubleValue;
                    if (double.TryParse(this.UNIT_WGT, out doubleValue) == true)
                    {
                        resultValue = doubleValue;
                    }
                }

                return resultValue;
            }
            set { this.UNIT_WGT = value.HasValue ? value.ToString() : null; }
        }
        #endregion
    }
}
