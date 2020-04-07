using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.DataMaterials;

namespace TowerLoadCals.Readers
{
    public class XmlReader
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
    }
}
