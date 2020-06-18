﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    Name = groupNode.Attributes["Name"].Value.ToString(),
                    Wire = groupNode.Attributes["Wire"].Value.ToString(),
                    FixedType = groupNode.Attributes["FixedType"].Value.ToString(),
                    ForceDirection = groupNode.Attributes["ForceDirection"].Value.ToString(),
                    Link = groupNode.Attributes["Link"].Value.ToString()
                };

                if (groupNode.Attributes["TWireNum"] != null)
                    group.TWireNum = groupNode.Attributes["TWireNum"].Value.ToString();

                group.Options = new List<StruCalsDicOption>();

                foreach (XmlNode optNode in groupNode.ChildNodes)
                {
                    StruCalsDicOption opt = new StruCalsDicOption()
                    {
                        Num = Convert.ToInt16(optNode.Attributes["挂点个数"].Value.ToString()),
                    };

                    if (optNode.Attributes["左侧挂点"] != null)
                        opt.LeftPoints = Regex.Split(optNode.Attributes["左侧挂点"].Value.ToString().Trim(), "\\s+");

                    if (optNode.Attributes["右侧挂点"] != null)
                        opt.RightPoints = Regex.Split(optNode.Attributes["右侧挂点"].Value.ToString().Trim(), "\\s+");

                    if (optNode.Attributes["前侧挂点"] != null)
                        opt.FrontPoints = Regex.Split(optNode.Attributes["前侧挂点"].Value.ToString().Trim(), "\\s+");

                    if (optNode.Attributes["后侧挂点"] != null)
                        opt.BackPoints = Regex.Split(optNode.Attributes["后侧挂点"].Value.ToString().Trim(), "\\s+");

                    if (optNode.Attributes["中部挂点"] != null)
                        opt.CentralPoints = Regex.Split(optNode.Attributes["中部挂点"].Value.ToString().Trim(), "\\s+");

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
