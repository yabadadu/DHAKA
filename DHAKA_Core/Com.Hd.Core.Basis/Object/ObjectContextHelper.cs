#region

using System;

#endregion

namespace Com.Hd.Core.Basis.Object
{
    public static class ObjectContextHelper
    {
        public static T CreateInstance<T>(string typeName)
        {
            typeName = typeName.Trim();
            var type = Type.GetType(typeName);
            var obj = (T) Activator.CreateInstance(type);
            return obj;
        }

        public static T CreateInstance<T>(string typeName, string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName) == false)
            {
                typeName = typeName + "," + assemblyName;
            }
            return CreateInstance<T>(typeName);
        }
    }
}
