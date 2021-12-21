#region

using System;
using System.Configuration;

#endregion

namespace Com.Hd.Core.Basis.Config.Language
{
    public class LanguageResourceSection : ConfigurationSection
    {
        #region Field

        internal static readonly string DEFAULT_COLLECTION = "";
        #endregion

        #region Constructor
        #endregion

        #region Accessor

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public LanguageResourceCollection Resources
        {
            get { return (LanguageResourceCollection) base[DEFAULT_COLLECTION]; }
            set { base[DEFAULT_COLLECTION] = value; }
        }

        #endregion

        #region Method

        public string GetValue(string key)
        {
            try
            {
                var languageRsrc = Resources[key];
                return languageRsrc == null ? string.Empty : Resources[key].Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return string.Empty;
        }
        
        #endregion
    }
}
