using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public static class StruLoadComposeDicReader
    { 
        public static  List<StruCalsDicGroup> Read(String path)
        {
            List<StruCalsDicGroup> groups = new List<StruCalsDicGroup>();

            if (!File.Exists(path))
                return groups;

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return groups;

            foreach (XmlNode groupNode in rootNode.ChildNodes)
            {
                StruCalsDicGroup group = new StruCalsDicGroup()
                {
                    Group = groupNode.Attributes["Group"].Value.ToString(),
                    WireType = groupNode.Attributes["WireType"].Value.ToString(),
                    Link = groupNode.Attributes["Link"].Value.ToString()
                };

                if (groupNode.Attributes["Type"] != null)
                    group.Type = groupNode.Attributes["Type"].Value.ToString();

                group.Options = new List<StruCalsDicOption>();

                foreach (XmlNode optNode in groupNode.ChildNodes)
                {
                    StruCalsDicOption opt = new StruCalsDicOption()
                    {
                        Num = Convert.ToInt16(optNode.Attributes["挂点个数"].Value.ToString())
                    };

                    opt.ComposrInfos = new List<StruCalsDicComposeInfo>();

                    foreach (XmlNode composeNode in optNode.ChildNodes)
                    {
                        StruCalsDicComposeInfo composeInfo = new StruCalsDicComposeInfo()
                        {
                            Orientation = composeNode.Attributes["方向"].Value.ToString()
                        };

                        composeInfo.PointCompose = new List<string>();

                        for(int i = 1; i <= opt.Num; i++)
                        {
                            composeInfo.PointCompose.Add(composeNode.Attributes["点" + i.ToString()].Value.ToString());
                        }

                        opt.ComposrInfos.Add(composeInfo);
                    }

                    group.Options.Add(opt);
                }

                groups.Add(group);
            }
            return groups;
        }
    }
}
