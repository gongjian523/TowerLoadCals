using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Utils
{
    public class XmlUtils
    {
        public static NodeXml ReadXml(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.FirstChild;
            if (rootNode == null)
                return null;

            GetAttributesAndSubNodes(rootNode, out Dictionary<string, string> atts, out List<NodeXml> subNodes);

            return new NodeXml {
                atts = atts,
                subNodes = subNodes
            };
        }

        private static void GetAttributesAndSubNodes(XmlNode node, out Dictionary<string,string > atts, out List<NodeXml> subNodes)
        {
            subNodes = new List<NodeXml>();
            atts = new Dictionary<string, string>();

            foreach (XmlAttribute att in  node.Attributes)
            {
                atts.Add(att.Name, att.Value);
            }
            
            if(node.HasChildNodes)
            {
                foreach(XmlNode subNodeItem in node.ChildNodes )
                {
                    GetAttributesAndSubNodes(subNodeItem, out Dictionary<string, string> subNodeAtts, out List<NodeXml> subSubNode);

                    NodeXml subNode = new NodeXml
                    {
                        atts = subNodeAtts,
                        subNodes = subSubNode
                    };

                    subNodes.Add(subNode);
                }
            }

            return;
        }

        //public static void Save<T>(string filePath, T sourceObj)
        //{
        //    if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
        //    {
        //        using (StreamWriter writer = new StreamWriter(filePath))
        //        {
        //            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //            ns.Add("", "");
        //            xmlSerializer.Serialize(writer, sourceObj, ns);
        //        }
        //    }
        //}

        //public static T Read<T>(string filePath)
        //{
        //    T t = default(T);

        //    if (File.Exists(filePath))
        //    {
        //        using (StreamReader reader = new StreamReader(filePath))
        //        {
        //            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //            t = (T)xmlSerializer.Deserialize(reader);
        //        }
        //    }

        //    return t;
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


            Deserializer(rootNode, out T desObj);

            return desObj;
        }

        protected static void Deserializer<T>(XmlNode node, out T desObj)
        {
            desObj = Activator.CreateInstance<T>();

            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                List<object> list = ((IEnumerable)desObj).Cast<object>().ToList();

                foreach (XmlNode subNode in node.ChildNodes)
                {
        
                    Deserializer(subNode, out T subItem);
                    list.Add(subItem);
                }
            }
            else
            {
                Type t = desObj.GetType();

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    Type t2 = pi.GetValue(desObj, null).GetType();
                    if (t2.IsGenericType && t2.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        //List<object> subList = ((IEnumerable)desObj).Cast<object>().ToList();
                        //一定要先转换成list，注释中的写法不会被判定成list在递归调用中
                        //var subObj = pi.GetValue(sourceObj, null);
                        //var subObj = ((IEnumerable)pi.GetValue(desObj, null)).Cast<object>().ToList();
                        Deserializer(node, out List<object> subList);
                        pi.SetValue(subList, node.Attributes[pi.Name].ToString(), null);
                    }
                    else
                    {
                        if (pi.PropertyType.Equals(typeof(string)))//判断属性的类型是不是String
                        {
                            pi.SetValue((T)desObj, node.Attributes[pi.Name].ToString(), null);//给泛型的属性赋值
                        }
                        else if (pi.PropertyType.Equals(typeof(int)))
                        {
                            pi.SetValue((T)desObj, Convert.ToInt16(node.Attributes[pi.Name]), null);//给泛型的属性赋值
                        }
                    }
                }
            }
        }
    }
}
