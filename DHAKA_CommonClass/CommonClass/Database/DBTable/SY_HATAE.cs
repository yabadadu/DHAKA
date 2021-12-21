using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_HATAE : DB_TABLE
    {
        #region PROPERTIES
        public string HATAE { get; set; }
        public string HATAE_NM { get; set; }
        public string PUMDIV { get; set; }
        public string HATAE_L { get; set; }
        #endregion

        #region INITIALIZE
        public SY_HATAE() : base()
        {

        }
        #endregion
    }
}
