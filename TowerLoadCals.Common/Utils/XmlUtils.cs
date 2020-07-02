using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace TowerLoadCals.Common
{
    public class XmlUtils
    {

        //使用XML原生的序列化函数保存XML文件
        public static void Serializer<T>(string filePath, T sourceObj)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    xmlSerializer.Serialize(writer, sourceObj, ns);
                }
            }
        }

        //直接使用XML原生的反序列函数读取XML文件
        public static T Deserializer<T>(string filePath)
        {
            T t = default(T);

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    t = (T)xmlSerializer.Deserialize(reader);
                }
            }

            return t;
        }


        //public static NodeXml ReadXml(string path)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(path);

        //    XmlNode rootNode = doc.FirstChild;
        //    if (rootNode == null)
        //        return null;

        //    GetAttributesAndSubNodes(rootNode, out Dictionary<string, string> atts, out List<NodeXml> subNodes);

        //    return new NodeXml {
        //        atts = atts,
        //        subNodes = subNodes
        //    };
        //}

        //private static void GetAttributesAndSubNodes(XmlNode node, out Dictionary<string,string > atts, out List<NodeXml> subNodes)
        //{
        //    subNodes = new List<NodeXml>();
        //    atts = new Dictionary<string, string>();

        //    foreach (XmlAttribute att in  node.Attributes)
        //    {
        //        atts.Add(att.Name, att.Value);
        //    }
            
        //    if(node.HasChildNodes)
        //    {
        //        foreach(XmlNode subNodeItem in node.ChildNodes )
        //        {
        //            GetAttributesAndSubNodes(subNodeItem, out Dictionary<string, string> subNodeAtts, out List<NodeXml> subSubNode);

        //            NodeXml subNode = new NodeXml
        //            {
        //                atts = subNodeAtts,
        //                subNodes = subSubNode
        //            };

        //            subNodes.Add(subNode);
        //        }
        //    }

        //    return;
        //}


        protected static XmlDocument doc;

        public static void Save<T>(string filePath, T sourceObj)
        {
            if (string.IsNullOrWhiteSpace(filePath) || sourceObj == null)
                return;

            doc = new XmlDocument();

            XmlNode decNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(decNode);

            XmlNode rootNode = doc.CreateElement("Root");
            doc.AppendChild(rootNode);

            Serializer(rootNode, sourceObj);

            doc.Save(filePath);
        }

        protected static void Serializer<T>(XmlNode node, T sourceObj)
        {
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                foreach (var item in (sourceObj as IEnumerable))
                {
                    Serializer(node, item);
                }
            }
            else
            {
                Type t = sourceObj.GetType();

                XmlNode subNode = doc.CreateElement(sourceObj.GetType().Name.ToString());
                node.AppendChild(subNode);

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    if (pi.GetValue(sourceObj, null) == null)
                        continue;

                    //不能直接用pi的Type，List会判断不对，而要用这个属性的值的Type
                    Type t2 = pi.GetValue(sourceObj, null).GetType();
                    if (t2.IsGenericType && t2.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        //一定要先转换成list，注释中的写法不会被判定成list在递归调用中
                        //var subObj = pi.GetValue(sourceObj, null);
                        var subObj = ((IEnumerable)pi.GetValue(sourceObj, null)).Cast<object>().ToList();
                        Serializer(subNode, subObj);
                    }
                    else
                    {
                        XmlAttribute attr = doc.CreateAttribute(pi.Name);
                        attr.Value = pi.GetValue(sourceObj,null).ToString();
                        subNode.Attributes.Append(attr);
                    }
                }
            }
        }

        public static T Read<T>(string filePath)
        {
            doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return default(T);

            T desObj = Activator.CreateInstance<T>();

            Deserializer(rootNode, desObj);

            return desObj;
        }

        protected static void  Deserializer<T>(XmlNode node, T desObj)
        {
            Type t1 = desObj.GetType();

            if (t1.FullName.StartsWith("System.Collections.Generic.List`1"))
            //if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                PropertyInfo pi = desObj.GetType().GetProperty("Item");

                Type tSubItem = pi.PropertyType;
                Assembly ass = Assembly.GetAssembly(tSubItem);

                var listType = typeof(List<>).MakeGenericType(new Type[] { tSubItem });
                var list = Activator.CreateInstance(listType);

                foreach (XmlNode subNode in node.ChildNodes)
                {
                    object subItem = ass.CreateInstance(tSubItem.FullName);
                    Deserializer(subNode, subItem);
                    ((IEnumerable)list).Cast<object>().ToList().Add(subItem);
                }

                pi.SetValue(desObj, list, null);

            }
            else
            {
                Type t = desObj.GetType();

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    if (pi.PropertyType.FullName.StartsWith("System.Collections.Generic.List`1"))
                    {
                        Type tList = pi.PropertyType;
                        Assembly ass = Assembly.GetAssembly(tList);
                        Type type = pi.PropertyType.GetGenericArguments()[0];
                        Assembly ass2 = Assembly.GetAssembly(type);


                        var listType = typeof(List<>).MakeGenericType(new Type[] { type });
                        var subList = Activator.CreateInstance(listType);

                        pi.SetValue(desObj, ass.CreateInstance(tList.FullName), null);




                        //var subList = ((IEnumerable)pi.GetValue(desObj, null)).Cast<object>().ToList();

                        //List<object> subList = ass.CreateInstance(tList.FullName);

                        //foreach (XmlNode subNode in node.ChildNodes)
                        //{
                        //    object subItme = ass2.CreateInstance(type.FullName);
                        //    Deserializer(subNode, subItme);
                        //    subList.Add(subItme);
                        //}
                        //object subList = ((IEnumerable)ass.CreateInstance(tList.FullName)).Cast<object>().ToList();

                        Deserializer(node, subList);
                        pi.SetValue(desObj, subList, null);

                    }
                    else if (pi.PropertyType.Equals(typeof(string)))//判断属性的类型是不是String
                    {
                        pi.SetValue(desObj, node.Attributes[pi.Name].Value.ToString(), null);//给泛型的属性赋值
                    }
                    else if (pi.PropertyType.Equals(typeof(int)))
                    {
                        pi.SetValue(desObj, Convert.ToInt16(node.Attributes[pi.Name].Value), null);//给泛型的属性赋值
                    }
                    else if(pi.PropertyType.Equals(typeof(bool)))
                    {
                        pi.SetValue(desObj, Convert.ToBoolean(node.Attributes[pi.Name].Value), null);//给泛型的属性赋值
                    }
                  
                }
            }
        }

        protected T Copy<T>(T obj)where T:new()
        {
　　        T obj2 = new T();
　　        return obj2;
        }

        //private static IEnumerable<object> CreateAListContainingOneObject(Type type)
        //{
        //    var del = typeof(Program).GetMethod("CreateAGenericListContainingOneObject",
        //    BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type);

        //    return del.Invoke(null, new object[] { }) as IEnumerable<object>;
        //}

        //private static List<T> CreateAGenericListContainingOneObject<T>()
        // where T : new()
        //{
        //    return new List<T> { new T() };
        //}
    }
}
