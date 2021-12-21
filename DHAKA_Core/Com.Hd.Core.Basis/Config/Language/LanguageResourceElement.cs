#region

using System.Configuration;
using Com.Hd.Core.Basis.Data.Item;

#endregion

namespace Com.Hd.Core.Basis.Config.Language
{
    public class LanguageResourceElement : ConfigurationElement, IBaseItem
    {
        /// <summary>
        /// Key is language key.
        /// </summary>
        #region Property
        [ConfigurationProperty("key", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Key
        {
            get
            {
                var value = base["key"] as string;
                return value ?? string.Empty;
            }
            set
            {
                base["key"] = value;
            }
        }

        /// <summary>
        /// Value is belong to language key. 
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true, DefaultValue = "")]
        public string Value
        {
            get
            {
                var value = base["value"] as string;
                return value ?? string.Empty;
            }
            set
            {
                base["value"] = value;
            }
        }

        /// <summary>
        /// Resource Type is type of resource. such as MSG, WRD, IMG, and etc...
        /// </summary>
        [ConfigurationProperty("rscType", IsRequired = true, DefaultValue = "")]
        public string ResourceType
        {
            get
            {
                var value = base["rscType"] as string;
                return value ?? string.Empty;
            }
            set
            {
                base["rscType"] = value;
            }
        }

        /// <summary>
        /// cultureName is culture name. such as ko-KR, en-US, and etc...
        /// </summary>
        [ConfigurationProperty("cultureName", IsRequired = true, DefaultValue = "")]
        public string CultureName
        {
            get
            {
                var value = base["cultureName"] as string;
                return value ?? string.Empty;
            }
            set
            {
                base["cultureName"] = value;
            }
        }

        /// <summary>
        /// encoding is language charater set
        /// </summary>
        [ConfigurationProperty("encoding", IsRequired = true, DefaultValue = "")]
        public string Encoding
        {
            get
            {
                var value = base["encoding"] as string;
                return value ?? string.Empty;
            }
            set
            {
                base["encoding"] = value;
            }
        }

        #endregion

    }
}
