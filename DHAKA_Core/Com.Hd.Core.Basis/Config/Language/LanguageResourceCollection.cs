#region

using System.Configuration;
using Com.Hd.Core.Basis.Util;

#endregion

namespace Com.Hd.Core.Basis.Config.Language
{
    [ConfigurationCollection(typeof(LanguageResourceElement))]
    public class  LanguageResourceCollection : ConfigurationElementCollection
    {

        #region Constructor

        public LanguageResourceCollection()
        {
            
        }

        #endregion

        #region Accessors

        public new LanguageResourceElement this[string key]
        {
            get { return (LanguageResourceElement) BaseGet(key); }
        }

        public string[] AllKeys
        {
            get { return StringUtil.ObjectArrayToStringArray(BaseGetAllKeys()); }
        }

        #endregion

        #region Methods
        protected override ConfigurationElement CreateNewElement()
        {
            return new LanguageResourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LanguageResourceElement)element).Key;
        }

        public void Add(LanguageResourceElement element)
        {
            var oldElement = (LanguageResourceElement) BaseGet(element.Key);
            if (oldElement == null)
            {
                BaseAdd(element);
            }
            else
            {
                BaseRemove(element.Key);
                BaseAdd(element);
            }
        }
        #endregion

    }
}
