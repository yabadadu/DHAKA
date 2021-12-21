#region

using System.Configuration;

#endregion

namespace Com.Hd.Core.Basis.Config.Language
{
    public class LanguageInfoSection : ConfigurationSection
    {
        #region Field

        internal static readonly string DEFAULT_COLLECTION = "";

        #endregion

        #region Constructor

        public LanguageInfoSection() : base()
        {
            
        }

        #endregion

        #region Accessor

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public KeyValueConfigurationCollection Settings
        {
            get { return (KeyValueConfigurationCollection) base[DEFAULT_COLLECTION]; }
            set { base[DEFAULT_COLLECTION] = value; }
        }

        #endregion
    }
}
