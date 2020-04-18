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
        //            XmlSerializer xmlSerializer =  new XmlSerializer(typeof(T));
        //            xmlSerializer.Serialize(writer, sourceObj);
        //        }
        //    }
        //}

        public static T Read<T>(string filePath)
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

            if (typeof(T) == typeof(List<>))
            {
                foreach (var item in (sourceObj as IEnumerable))
                {
                    XmlNode subNode = doc.CreateElement(item.GetType().ToString());
                    Serializer(subNode, item);
                    node.AppendChild(subNode);
                }
            }
            else
            {
                Type t = sourceObj.GetType();

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    Type t2 = pi.GetType();
                    //if (t2 == typeof(List<>))
                    //{
                    //    Serializer(nodeSub, pi.GetValue);
                    //}
                    //else
                    //{
                    //    XmlAttribute attr = doc.CreateAttribute(pi.Name);
                    //    attr.Value = pi.GetValue();
                    //    node.Attributes.Append(attr);
                    //}
                }
            }
        }
    }
}
