using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database
{
    public class DB_TABLE : ICloneable
    {
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
