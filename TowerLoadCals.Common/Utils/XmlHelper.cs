
using System;
using System.Collections.Generic;
using System.Xml;

namespace TowerLoadCals.Common
{
    public class XMLHelper
    {
        #region object --> XML string.
        public static string ToRequestXML<T>(T obj) where T : class
        {
            string xmlStr = string.Empty;
            try
            {
                var type = typeof(T);
                var properties = type.GetProperties();
                XmlDocument xd = new XmlDocument();
                XmlElement xe = xd.CreateElement("msgBody");

                for (int i = 0; i < properties.Length; i++)
                {
                    var elementName = properties[i].Name;

                    #region current type is List
                    if (properties[i].PropertyType.FullName.Contains("System.Collections.Generic.List"))
                    {
                        var propertyValue = properties[i].GetValue(obj, null);
                        if (propertyValue == null)
                        {
                            XmlElement xa = xd.CreateElement(elementName);
                            xe.AppendChild(xa);
                        }
                        else
                        {
                            xe = getArray(propertyValue, xe, xd, true, true, elementName);
                        }
                        continue;
                    }
                    #endregion

                    #region current type is Array
                    if (properties[i].PropertyType.BaseType.Name == "Array")
                    {
                        var propertyName = properties[i].Name;
                        var propertyValue = properties[i].GetValue(obj, null);
                        if (propertyValue == null)
                        {
                            XmlElement xa = xd.CreateElement(elementName);
                            xe.AppendChild(xa);
                        }
                        else
                        {
                            xe = getArray(propertyValue, xe, xd, true, false, elementName);
                        }
                        continue;
                    }
                    #endregion

                    #region current type is Model
                    if (properties[i].PropertyType.BaseType.Name != "Array" && (properties[i].PropertyType != typeof(long) && properties[i].PropertyType != typeof(int) && properties[i].PropertyType != typeof(string)) && !properties[i].PropertyType.FullName.Contains("System.Collections.Generic.List"))
                    {
                        var childPropertyName = properties[i].Name;
                        var childPropertyValue = properties[i].GetValue(obj, null);

                        if (childPropertyValue == null)
                        {
                            var modelType = properties[i].PropertyType.UnderlyingSystemType;
                            var model = modelType.Assembly.CreateInstance(modelType.FullName);
                            xe = getArray(model, xe, xd, false, false, elementName);
                        }
                        else
                        {
                            xe = getArray(childPropertyValue, xe, xd, false, false, elementName);
                        }
                        continue;
                    }
                    #endregion

                    #region current type is int or string
                    if (properties[i].PropertyType == typeof(long) || properties[i].PropertyType == typeof(string) || properties[i].PropertyType == typeof(int))
                    {
                        var childPropertyName = properties[i].Name;
                        var childPropertyValue = properties[i].GetValue(obj, null);
                        XmlElement xa = xd.CreateElement(childPropertyName);
                        if (childPropertyValue != null)
                        {
                            xa.InnerText = childPropertyValue.ToString();
                        }
                        else
                        {
                            xa.InnerText = "";
                        }
                        xe.AppendChild(xa);
                        continue;
                    }
                    #endregion
                }
                xd.AppendChild(xe);
                xmlStr = xd.InnerXml;
                return xmlStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get array value
        private static XmlElement getArray(object propertyValue, XmlElement xe, XmlDocument xd, bool isArray, bool isList, string elementName)
        {
            try
            {
                if (propertyValue != null)
                {
                    if (isList)
                    {
                        #region current type is Array
                        var array = propertyValue as System.Collections.IEnumerable;
                        if (array != null)
                        {
                            foreach (var item in array)
                            {
                                var obj = item;
                                var type = item.GetType();
                                var properties = type.GetProperties();
                                XmlElement childXe = xd.CreateElement(elementName);
                                for (int i = 0; i < properties.Length; i++)
                                {
                                    var propertyType = properties[i].PropertyType;

                                    #region current type is List
                                    if (properties[i].PropertyType.FullName.Contains("System.Collections.Generic.List"))
                                    {
                                        var elementName2 = properties[i].Name;
                                        var propertyValue2 = properties[i].GetValue(obj, null);
                                        if (propertyValue2 == null)
                                        {
                                            XmlElement xa = xd.CreateElement(elementName);
                                            childXe.AppendChild(xa);
                                        }
                                        else
                                        {
                                            childXe = getArray(propertyValue2, childXe, xd, true, true, elementName2);
                                        }
                                        continue;
                                    }
                                    #endregion

                                    #region current type is Array
                                    if (properties[i].PropertyType.BaseType.Name == "Array")
                                    {
                                        var elementName2 = properties[i].Name;
                                        var childPropertyValue = properties[i].GetValue(obj, null);
                                        if (childPropertyValue == null)
                                        {
                                            XmlElement xa = xd.CreateElement(elementName);
                                            childXe.AppendChild(xa);
                                        }
                                        else
                                        {
                                            childXe = getArray(childPropertyValue, childXe, xd, true, false, elementName2);
                                        }

                                    }
                                    #endregion

                                    #region current type is Model
                                    if (properties[i].PropertyType.BaseType.Name != "Array" && (properties[i].PropertyType != typeof(long) && properties[i].PropertyType != typeof(int) && properties[i].PropertyType != typeof(string)) && !properties[i].PropertyType.FullName.Contains("System.Collections.Generic.List"))
                                    {
                                        elementName = properties[i].Name;
                                        var childPropertyValue = properties[i].GetValue(obj, null);
                                        if (childPropertyValue == null)
                                        {
                                            var modelType = type.UnderlyingSystemType;
                                            var model = modelType.Assembly.CreateInstance(modelType.FullName);
                                            xe = getArray(model, xe, xd, false, false, elementName);
                                        }
                                        else
                                        {
                                            childXe = getArray(childPropertyValue, childXe, xd, false, false, elementName);
                                        }
                                    }
                                    #endregion

                                    #region current type is Number
                                    if (properties[i].PropertyType == typeof(long) || properties[i].PropertyType == typeof(string) || properties[i].PropertyType == typeof(int))
                                    {
                                        var childPropertyName = properties[i].Name;
                                        var childPropertyValue = properties[i].GetValue(obj, null);
                                        XmlElement xa = xd.CreateElement(childPropertyName);
                                        if (childPropertyValue != null)
                                        {
                                            xa.InnerText = childPropertyValue.ToString();
                                        }
                                        else
                                        {
                                            xa.InnerText = "";
                                        }
                                        childXe.AppendChild(xa);
                                    }
                                    #endregion
                                }
                                xe.AppendChild(childXe);
                            }
                        }
                        else
                        {
                            var list = (List<object>)propertyValue;
                        }
                        #endregion
                        return xe;
                    }

                    if (isArray)
                    {
                        #region current type is Array
                        var array = (Array)propertyValue;
                        if (array != null)
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                var obj = array.GetValue(i);
                                var type = array.GetValue(i).GetType();
                                var properties = type.GetProperties();
                                XmlElement childXe = xd.CreateElement(elementName);
                                for (int j = 0; j < properties.Length; j++)
                                {
                                    var propertyType = properties[j].PropertyType;

                                    #region current type is List
                                    if (properties[j].PropertyType.FullName.Contains("System.Collections.Generic.List"))
                                    {
                                        var elementName2 = properties[j].Name;
                                        var propertyValue2 = properties[j].GetValue(obj, null);
                                        if (propertyValue2 == null)
                                        {
                                            XmlElement xa = xd.CreateElement(elementName);
                                            childXe.AppendChild(xa);
                                        }
                                        else
                                        {
                                            childXe = getArray(propertyValue2, childXe, xd, true, true, elementName2);
                                        }
                                        continue;
                                    }
                                    #endregion

                                    #region current type is Array
                                    if (properties[j].PropertyType.BaseType.Name == "Array")
                                    {
                                        var elementName2 = properties[j].Name;
                                        var childPropertyValue = properties[j].GetValue(obj, null);
                                        if (childPropertyValue == null)
                                        {
                                            XmlElement xa = xd.CreateElement(elementName);
                                            childXe.AppendChild(xa);
                                        }
                                        else
                                        {
                                            childXe = getArray(childPropertyValue, childXe, xd, true, false, elementName2);
                                        }

                                    }
                                    #endregion

                                    #region current type is Model
                                    if (properties[j].PropertyType.BaseType.Name != "Array" && (properties[j].PropertyType != typeof(long) && properties[j].PropertyType != typeof(int) && properties[j].PropertyType != typeof(string)))
                                    {
                                        elementName = properties[j].Name;
                                        var childPropertyValue = properties[j].GetValue(obj, null);
                                        if (childPropertyValue == null)
                                        {
                                            var modelType = properties[i].PropertyType.UnderlyingSystemType;
                                            var model = modelType.Assembly.CreateInstance(modelType.FullName);
                                            xe = getArray(model, xe, xd, false, false, elementName);
                                        }
                                        else
                                        {
                                            childXe = getArray(childPropertyValue, childXe, xd, false, false, elementName);
                                        }
                                    }
                                    #endregion

                                    #region current type is Number
                                    if (properties[j].PropertyType == typeof(long) || properties[j].PropertyType == typeof(string) || properties[j].PropertyType == typeof(int))
                                    {
                                        var childPropertyName = properties[j].Name;
                                        var childPropertyValue = properties[j].GetValue(obj, null);
                                        XmlElement xa = xd.CreateElement(childPropertyName);
                                        if (childPropertyValue != null)
                                        {
                                            xa.InnerText = childPropertyValue.ToString();
                                        }
                                        else
                                        {
                                            xa.InnerText = "";
                                        }
                                        childXe.AppendChild(xa);
                                    }
                                    #endregion
                                }
                                xe.AppendChild(childXe);
                            }
                        }
                        else
                        {
                            var list = (List<object>)propertyValue;
                        }
                        #endregion
                    }
                    else
                    {
                        #region current type isn't Array
                        var objType = propertyValue.GetType();
                        var properties = objType.GetProperties();
                        XmlElement childXe = xd.CreateElement(elementName);
                        for (int j = 0; j < properties.Length; j++)
                        {
                            var propertyType = properties[j].PropertyType;

                            #region current type is Array
                            if (properties[j].PropertyType.BaseType.Name == "Array")
                            {
                                var elementName2 = properties[j].Name;
                                var childPropertyValue = properties[j].GetValue(propertyValue, null);
                                if (childPropertyValue == null)
                                {
                                    XmlElement xa = xd.CreateElement(elementName);
                                    childXe.AppendChild(xa);
                                }
                                else
                                {
                                    childXe = getArray(childPropertyValue, childXe, xd, true, false, elementName2);
                                }
                            }
                            #endregion

                            #region current type is Model
                            if (properties[j].PropertyType.BaseType.Name != "Array" && (properties[j].PropertyType != typeof(long) && properties[j].PropertyType != typeof(int) && properties[j].PropertyType != typeof(string)))
                            {
                                elementName = properties[j].Name;
                                var childPropertyValue = properties[j].GetValue(propertyValue, null);
                                if (childPropertyValue == null)
                                {
                                    var modelType = properties[j].PropertyType.UnderlyingSystemType;
                                    var model = modelType.Assembly.CreateInstance(modelType.FullName);
                                    xe = getArray(model, xe, xd, false, false, elementName);
                                }
                                else
                                {
                                    childXe = getArray(childPropertyValue, childXe, xd, false, false, elementName);
                                }
                            }
                            #endregion

                            #region current is Number
                            if (properties[j].PropertyType == typeof(long) || properties[j].PropertyType == typeof(string) || properties[j].PropertyType == typeof(int))
                            {
                                var childPropertyName = properties[j].Name;
                                var childPropertyValue = properties[j].GetValue(propertyValue, null);
                                XmlElement xa = xd.CreateElement(childPropertyName);
                                if (childPropertyValue != null)
                                {
                                    xa.InnerText = childPropertyValue.ToString();
                                }
                                else
                                {
                                    xa.InnerText = "";
                                }
                                childXe.AppendChild(xa);
                            }
                            #endregion
                        }
                        xe.AppendChild(childXe);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xe;
        }
        #endregion

        #region XML string --> object
        /// <summary>
        /// Convert xml message to object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static T ToResponseObject<T>(string xmlStr) where T : new()
        {
            try
            {
                //T obj = default(T);
                T obj = new T();
                var repType = typeof(T);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlStr);    //加载Xml文件  
                XmlElement node = document.DocumentElement; //xml的根标签
                var nodeList = document.ChildNodes;
                var properties = repType.GetProperties();

                foreach (var itemProp in properties)
                {
                    #region current type is List
                    if (itemProp.PropertyType.FullName.Contains("System.Collections.Generic.List"))
                    {

                        object array = new object();
                        var arryLength = 0;
                        var notNullLength = 0;
                        var arryType = itemProp.PropertyType.UnderlyingSystemType;

                        var objList = itemProp.GetValue(obj, null) as System.Collections.IEnumerable;
                        var enumt = objList.GetEnumerator();
                        //enumt.
                        var currentType = itemProp.PropertyType.GetGenericArguments()[0];

                        foreach (XmlNode xmlitem in node.ChildNodes)
                        {
                            if (xmlitem.Name == itemProp.Name)
                            {
                                arryLength++;
                            }
                        }
                        if (arryLength > 0)
                        {
                            var arrayModel = arryType.InvokeMember("Set", System.Reflection.BindingFlags.CreateInstance, null, array, new object[] { arryLength }) as System.Collections.IList;
                            foreach (XmlNode item in node.ChildNodes)
                            {
                                //current type is array
                                if (item.Name == itemProp.Name)
                                {
                                    var model = currentType.Assembly.CreateInstance(currentType.FullName); // arryType.GetElementType().Assembly.CreateInstance(currentType.FullName);
                                    SetArray(item.ChildNodes, model, true);
                                    arrayModel.Add(model);
                                    //arrayModel[notNullLength] = model;
                                    notNullLength++;
                                }
                            }
                            itemProp.SetValue(obj, arrayModel, null);
                        }

                        continue;
                    }
                    #endregion

                    var baseType = itemProp.PropertyType.BaseType.Name;
                    if (baseType == "Array")
                    {
                        #region Current type is Array
                        object array = new object();
                        var arryLength = 0;
                        var notNullLength = 0;
                        var arryType = itemProp.PropertyType.UnderlyingSystemType;
                        foreach (XmlNode xmlitem in node.ChildNodes)
                        {
                            if (xmlitem.Name == itemProp.Name)
                            {
                                arryLength++;
                            }
                        }
                        if (arryLength > 0)
                        {
                            var arrayModel = arryType.InvokeMember("Set", System.Reflection.BindingFlags.CreateInstance, null, array, new object[] { arryLength }) as System.Collections.IList;
                            foreach (XmlNode item in node.ChildNodes)
                            {
                                //current type is array
                                if (item.Name == itemProp.Name)
                                {
                                    var model = arryType.GetElementType().Assembly.CreateInstance(arryType.GetElementType().FullName);
                                    SetArray(item.ChildNodes, model);
                                    arrayModel[notNullLength] = model;
                                    notNullLength++;
                                }
                            }
                            itemProp.SetValue(obj, arrayModel, null);
                        }
                        #endregion

                        continue;
                    }
                    else
                    {
                        #region Current type isn't Array
                        foreach (XmlNode item in node.ChildNodes)
                        {
                            #region Current type is Number
                            if (itemProp.Name == item.Name && (itemProp.PropertyType == typeof(long) || itemProp.PropertyType == typeof(int) || itemProp.PropertyType == typeof(string)))
                            {
                                if (itemProp.PropertyType == typeof(int) || itemProp.PropertyType == typeof(long))
                                {
                                    if (!string.IsNullOrEmpty(item.InnerText))
                                    {
                                        if (itemProp.PropertyType == typeof(int))
                                        {
                                            itemProp.SetValue(obj, Convert.ToInt32(item.InnerText), null);
                                        }
                                        else
                                        {
                                            itemProp.SetValue(obj, Convert.ToInt64(item.InnerText), null);
                                        }

                                    }
                                    else
                                    {
                                        itemProp.SetValue(obj, 0, null);
                                    }
                                }
                                else
                                {
                                    itemProp.SetValue(obj, item.InnerText, null);
                                }
                            }
                            #endregion

                            #region Current type is Model
                            if (itemProp.PropertyType != typeof(long) && itemProp.PropertyType != typeof(string) && itemProp.PropertyType != typeof(int) && itemProp.PropertyType.Name == item.Name && item.HasChildNodes && item.FirstChild.NodeType == System.Xml.XmlNodeType.Element)
                            {
                                var modelType = itemProp.PropertyType.UnderlyingSystemType;
                                var model = modelType.Assembly.CreateInstance(modelType.FullName);
                                SetArray(item.ChildNodes, model);
                                itemProp.SetValue(obj, model, null);
                            }
                            #endregion
                        }
                        #endregion

                        continue;
                    }
                }
                repType = obj.GetType();
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Set array value
        private static Object SetArray(XmlNodeList xmlNodeList, object obj, bool isList = false)
        {
            try
            {
                var type = obj.GetType();
                var properties = type.GetProperties();
                foreach (var itemProp in properties)
                {
                    //if (isList)
                    if (itemProp.PropertyType.FullName.Contains("System.Collections.Generic.List"))
                    {
                        #region Current type is List
                        object array = new object();
                        var arryLength = 0;
                        var notNullLength = 0;
                        var arryType = itemProp.PropertyType.UnderlyingSystemType;
                        var currentType = itemProp.PropertyType.GetGenericArguments()[0];
                        foreach (XmlNode xmlitem in xmlNodeList)
                        {
                            if (xmlitem.Name == itemProp.Name)
                            {
                                arryLength++;
                            }
                        }

                        if (arryLength > 0)
                        {
                            var arrayModel = arryType.InvokeMember("Set", System.Reflection.BindingFlags.CreateInstance, null, array, new object[] { arryLength }) as System.Collections.IList;
                            foreach (XmlNode item in xmlNodeList)
                            {
                                //current type is array
                                if (item.Name == itemProp.Name)
                                {
                                    var model = currentType.Assembly.CreateInstance(currentType.FullName); // var model = arryType.GetElementType().Assembly.CreateInstance(arryType.GetElementType().FullName);
                                    SetArray(item.ChildNodes, model, true);
                                    arrayModel.Add(model);
                                    notNullLength++;

                                }
                            }
                            itemProp.SetValue(obj, arrayModel, null);
                        }
                        #endregion
                        return obj;
                    }


                    var baseType = itemProp.PropertyType.BaseType.Name;
                    if (baseType == "Array")
                    {
                        #region Current type is Array
                        object array = new object();
                        var arryLength = 0;
                        var notNullLength = 0;
                        var arryType = itemProp.PropertyType.UnderlyingSystemType;
                        foreach (XmlNode xmlitem in xmlNodeList)
                        {
                            if (xmlitem.Name == itemProp.Name)
                            {
                                arryLength++;
                            }
                        }

                        if (arryLength > 0)
                        {
                            var arrayModel = arryType.InvokeMember("Set", System.Reflection.BindingFlags.CreateInstance, null, array, new object[] { arryLength }) as System.Collections.IList;
                            foreach (XmlNode item in xmlNodeList)
                            {
                                //current type is array
                                if (item.Name == itemProp.Name)
                                {
                                    var model = arryType.GetElementType().Assembly.CreateInstance(arryType.GetElementType().FullName);
                                    SetArray(item.ChildNodes, model);
                                    arrayModel[notNullLength] = model;
                                    notNullLength++;

                                }
                            }
                            itemProp.SetValue(obj, arrayModel, null);
                        }
                        #endregion
                    }
                    else
                    {
                        foreach (XmlNode item in xmlNodeList)
                        {
                            #region Current type is Number
                            if (itemProp.Name == item.Name && (itemProp.PropertyType == typeof(long) || itemProp.PropertyType == typeof(int) || itemProp.PropertyType == typeof(string)))
                            {
                                if (itemProp.PropertyType == typeof(int) || itemProp.PropertyType == typeof(long))
                                {
                                    if (!string.IsNullOrEmpty(item.InnerText))
                                    {
                                        if (itemProp.PropertyType == typeof(int))
                                        {
                                            itemProp.SetValue(obj, Convert.ToInt32(item.InnerText), null);
                                        }
                                        else
                                        {
                                            itemProp.SetValue(obj, Convert.ToInt64(item.InnerText), null);
                                        }
                                    }
                                    else
                                    {
                                        itemProp.SetValue(obj, 0, null);
                                    }
                                }
                                else
                                {
                                    itemProp.SetValue(obj, item.InnerText, null);
                                }
                            }
                            #endregion

                            #region Current type is Model
                            if (itemProp.PropertyType != typeof(long) && itemProp.PropertyType != typeof(int) && itemProp.PropertyType != typeof(string) && itemProp.PropertyType.Name == item.Name && item.HasChildNodes && item.FirstChild.NodeType == System.Xml.XmlNodeType.Element)
                            {
                                var modelType = itemProp.PropertyType.UnderlyingSystemType;
                                var model = modelType.Assembly.CreateInstance(modelType.FullName);
                                SetArray(item.ChildNodes, model);
                                itemProp.SetValue(obj, model, null);
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return obj;
        }
        #endregion
    }
}