using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;

namespace CommonClass.Database.Mapper
{
    public class BindDB2Class
    {
        private static PropertyInfo[] GetClassField(Object obj)
        {
            PropertyInfo[] propertyinfos;
            propertyinfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return propertyinfos;
        }

        private static HashSet<Type> NumericTypes = new HashSet<Type>()
        {
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        /// <summary>
        /// Data binding as a child class of DB_TABLE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hTable"></param>
        /// <param name="clazz"></param>
        public static void BindDBHashtable2Class<T>(Hashtable hTable, T clazz)
        {
            if (hTable == null) return;
            if (clazz == null) return;

            PropertyInfo[] propertyinfos = GetClassField(clazz);

            foreach (PropertyInfo info in propertyinfos)
            {
                if (hTable.ContainsKey(info.Name) == true)
                {
                    info.SetValue(clazz, hTable[info.Name].ToString());
                }
            }
        }

        /// <summary>
        /// Data binding as a child class of DB_TABLE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aList"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static List<T> BindDBArrayList2Class<T>(ArrayList aList, T clazz)
        {
            List<T> rstList = new List<T>();

            if (aList == null) return rstList;

            foreach (Hashtable hTable in aList)
            {
                T cls = (T)Activator.CreateInstance(clazz.GetType());
                BindDBHashtable2Class(hTable, cls);
                rstList.Add(cls);
            }

            return rstList;
        }

        public static IList<T> BindDBArrayList2Class<T>(ArrayList aList)
        {
            IList<T> resultList = new List<T>();
            if (aList == null) return resultList;

            foreach (Hashtable hTable in aList)
            {
                T obj = Activator.CreateInstance<T>();
                BindDBHashtable2Class<T>(hTable, obj);
                resultList.Add(obj);
            }

            return resultList;
        }

        /// <summary>
        /// Hashtable from a child class of DB_TABLE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static Hashtable BindDBClass2Hashtable<T>(T clazz, bool pascalToSnake = false)
        {
            if (clazz == null) return null;
            if (typeof(T) == typeof(DataRow))
            {
                throw new Exception("Please use the function BindDataRow2Hashtable instead of this");
            }

            Hashtable columnTable = new Hashtable();
            PropertyInfo[] propertyinfos = GetClassField(clazz);

            foreach (PropertyInfo info in propertyinfos)
            {
                if (info.PropertyType == typeof(string) || info.PropertyType == typeof(String))
                {
                    string key = info.Name;
                    if (pascalToSnake == true)
                    {
                        string newKey = string.Empty;

                        for (int charIdx = 0; charIdx < key.Length; charIdx++)
                        {
                            char charValue = key[charIdx];

                            if (char.IsUpper(charValue) == true && charIdx != 0)
                            {
                                newKey += '_';
                            }

                            newKey += char.ToUpper(charValue);
                        }

                        key = newKey;
                    }

                    columnTable.Add(key, info.GetValue(clazz));
                }
            }

            return columnTable;
        }

        /// <summary>
        /// Get Hashtable List using ItemList
        /// </summary>
        /// <typeparam name="T">Item Class</typeparam>
        /// <param name="itemList">Item List</param>
        /// <param name="pascalToSnake">Set Hashtable Key name that converted casing pascal to snake</param>
        /// <returns>Hashtable List</returns>
        public static IList<Hashtable> BindList2HashtableList<T>(IList<T> itemList, bool pascalToSnake = false)
        {
            if (itemList == null) return null;
            IList<Hashtable> hashtableList = new List<Hashtable>();
            
            foreach (T item in itemList)
            {
                Hashtable hTable = BindDBClass2Hashtable<T>(item, pascalToSnake);
                if (hTable != null) hashtableList.Add(hTable);
            }

            return hashtableList;
        }

        /// <summary>
        /// Get Hashtable using DataRow
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Hashtable</returns>
        public static Hashtable BindDataRow2Hashtable(DataRow dataRow)
        {
            if (dataRow == null) return null;
            Hashtable columnTable = new Hashtable();

            DataTable dataTable = dataRow.Table;
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.DataType == typeof(string) || column.DataType == typeof(String))
                {
                    columnTable.Add(column.ColumnName, dataRow[column.ColumnName] as string);
                }
            }

            return columnTable;
        }

        /// <summary>
        /// Get Hashtable List using DataRow List
        /// </summary>
        /// <param name="dataRowList">List of DataRow</param>
        /// <returns>Hashtable List</returns>
        public static IList<Hashtable> BindDataRowList2HashtableList(IList<DataRow> dataRowList)
        {
            if (dataRowList == null) return null;
            IList<Hashtable> hashtableList = new List<Hashtable>();

            foreach (DataRow dataRow in dataRowList)
            {
                Hashtable hTable = BindDataRow2Hashtable(dataRow);
                if (hTable != null) hashtableList.Add(hTable);
            }

            return hashtableList;
        }

        public static DataTable BindDatatable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// Map DB Columns to Class Properties (COLUMN_NAME1 => ColumnName1)
        /// </summary>
        /// <param name="makeRead">If T is BaseItem make OpCode to Read</param>
        public static IList<T> BindDataTableToList<T>(DataTable dt, bool makeRead = true)
        {
            IList<T> resultList = new List<T>();
            if (dt == null || dt.Rows.Count == 0) return resultList;

            Dictionary<string, string> columnNameList = new Dictionary<string, string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columnNameList.Add(dc.ColumnName.Replace("_", string.Empty).ToLower(), dc.ColumnName);
            }

            foreach (DataRow eachRow in dt.Rows)
            {
                var obj = Activator.CreateInstance<T>();
                var properties = typeof(T).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (columnNameList.ContainsKey(propertyInfo.Name.ToLower()) == true)
                    {
                        if (NumericTypes.Contains(propertyInfo.PropertyType) == true)
                        {
                            decimal numericValue;
                            if (decimal.TryParse(eachRow[columnNameList[propertyInfo.Name.ToLower()]] as string, out numericValue) == true)
                            {
                                if (propertyInfo.PropertyType == typeof(int)) propertyInfo.SetValue(obj, (int)numericValue);
                                else if (propertyInfo.PropertyType == typeof(long)) propertyInfo.SetValue(obj, (long)numericValue);
                                else if (propertyInfo.PropertyType == typeof(float)) propertyInfo.SetValue(obj, (float)numericValue);
                                else if (propertyInfo.PropertyType == typeof(double)) propertyInfo.SetValue(obj, (double)numericValue);
                                else propertyInfo.SetValue(obj, numericValue);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(obj, eachRow[columnNameList[propertyInfo.Name.ToLower()]]);
                        }
                    }
                }

                MethodInfo makeBackupItemMethod = typeof(T).GetMethod("MakeBackupItem", new Type[] { typeof(bool) });
                if (makeBackupItemMethod != null)
                {
                    makeBackupItemMethod.Invoke(obj, new object[] { makeRead } );
                }
                resultList.Add(obj);
            }

            return resultList;
        }

        /// <summary>
        /// Map DB Columns to Class Properties (COLUMN_NAME1 => COLUMN_NAME1)
        /// </summary>
        /// <param name="makeRead">If T is BaseItem make OpCode to Read</param>
        public static IList<T> BindDataTableToListNoFormat<T>(DataTable dt, bool makeRead = true)
        {
            IList<T> resultList = new List<T>();
            if (dt == null || dt.Rows.Count == 0) return resultList;

            List<string> columnNameList = new List<string>();
                                          
            foreach (DataColumn dc in dt.Columns)
            {
                columnNameList.Add(dc.ColumnName);
            }

            foreach (DataRow eachRow in dt.Rows)
            {
                var obj = Activator.CreateInstance<T>();
                var properties = typeof(T).GetProperties();
                
                foreach (var propertyInfo in properties)
                {
                    if (columnNameList.Contains(propertyInfo.Name) == true)
                    {
                        if (NumericTypes.Contains(propertyInfo.PropertyType) == true)
                        {
                            decimal numericValue;
                            if (decimal.TryParse(eachRow[propertyInfo.Name] as string, out numericValue) == true)
                            {
                                if (propertyInfo.PropertyType == typeof(int)) propertyInfo.SetValue(obj, (int)numericValue);
                                else if (propertyInfo.PropertyType == typeof(long)) propertyInfo.SetValue(obj, (long)numericValue);
                                else if (propertyInfo.PropertyType == typeof(float)) propertyInfo.SetValue(obj, (float)numericValue);
                                else if (propertyInfo.PropertyType == typeof(double)) propertyInfo.SetValue(obj, (double)numericValue);
                                else propertyInfo.SetValue(obj, numericValue);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(obj, eachRow[propertyInfo.Name]);
                        }
                    }
                }

                MethodInfo makeBackupItemMethod = typeof(T).GetMethod("MakeBackupItem", new Type[] { typeof(bool) });
                if (makeBackupItemMethod != null)
                {
                    makeBackupItemMethod.Invoke(obj, new object[] { makeRead });
                }
                resultList.Add(obj);
            }

            return resultList;
        }

        
    }
}
