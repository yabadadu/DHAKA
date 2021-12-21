using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_PUM : DB_TABLE
    {
        #region PROPERTIES
        public string PUM_NO { get; set; }
        public string PUM_NM { get; set; }
        public string PUM_SNM { get; set; }
        public string HATAE { get; set; }
        public string HATAE_P { get; set; }

        public string PUM_DISPLAY
        {
            get
            {
                string resultValue = this.PUM_NM;

                if (string.IsNullOrEmpty(this.PUM_NO) == false)
                {
                    resultValue += " [" + PUM_NO + "]";
                }

                return resultValue;
            }
        }
        #endregion

        #region INITIALIZE
        public SY_PUM() : base()
        {
            
        }
        #endregion
    }
}
