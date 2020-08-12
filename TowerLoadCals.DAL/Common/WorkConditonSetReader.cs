using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Common;

namespace TowerLoadCals.DAL.Common
{
    /// <summary>
    /// 工况组合-代码解析
    /// </summary>
    public class WorkConditonSetReader
    {

        public static WorkConditonSet Read()
        {
            string path = Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.TemplateCorrespondTableFileName; 
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new WorkConditonSet();

            WorkConditonSet set = new WorkConditonSet();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    foreach (XmlNode child in childNode.ChildNodes)
                    {
                        if (node.Name == "LineTower")//直线塔、直转塔
                        {
                            set.LineTower.Add(child.InnerXml.Trim(), childNode.Attributes.GetNamedItem("Type").InnerText + child.Attributes.GetNamedItem("Type").InnerText);//添加具体key,value映射关系
                        }
                        else if (node.Name == "CornerTower")//终端塔、分支塔、转角塔
                        {
                            set.CornerTower.Add(child.InnerXml.Trim(), childNode.Attributes.GetNamedItem("Type").InnerText + child.Attributes.GetNamedItem("Type").InnerText);//添加具体key,value映射关系
                        }
                    }

                }


            }
            return set;
        }


        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="tower"></param>
        /// <param name="key"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public static string ConvertWorkConditionCode(string tower, string key, WorkConditonSet set)
        {
            string value = "";
            if (tower == "直线塔" || tower == "直转塔" || tower == "直线转角塔")
            { 
                set.LineTower.TryGetValue(key.Trim(), out value);
                if(string.IsNullOrEmpty(value))//特殊处理的key
                {
                    foreach (string itemKey in set.LineTower.Keys)
                    { 
                        if(key.Contains(itemKey))
                        { 
                            set.LineTower.TryGetValue(itemKey, out value);
                            break;
                        }
                    }
                }
            }
            else if (tower == "终端塔" || tower == "分支塔" || tower == "转角塔")
            { 
                set.CornerTower.TryGetValue(key.Trim(), out value);
                if (string.IsNullOrEmpty(value))//特殊处理的key
                {
                    foreach (string itemKey in set.CornerTower.Keys)
                    {
                        if (key.Contains(itemKey))
                        {
                            set.CornerTower.TryGetValue(itemKey, out value);
                            break;
                        }
                    }
                }
            }

            return value;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="tower">塔型</param>
        /// <param name="index">循环I</param>
        /// <param name="code">工况代号</param>
        /// <param name="wireIndexCodes">工况类型</param>
        /// <param name="workConditongs">代号解析</param>
        /// <returns></returns>
        public static string ConvertWireCodes(string tower, int index, string code, List<int> wireIndexCodes, Dictionary<int, string> workConditongs)
        {
            string value = "";
            int key = wireIndexCodes[index - 1];
            if (key == 0)
                value = "无荷载";
            else
            {
                workConditongs.TryGetValue(Math.Abs(key), out value);
                if (!string.IsNullOrEmpty(value))
                    value = key > 0 ? value : "反向" + value;
                else
                {

                    if (code.Substring(0, 1) == "B" && new Regex(@"100[0-9]").Match(Math.Abs(key).ToString()).Success)//断线
                        value = key > 0 ? "已断线" : "反向 已断线";
                    else if (code.Substring(0, 1) == "T" && new Regex(@"100[0-9]").Match(Math.Abs(key).ToString()).Success)//脱冰跳跃
                        value = key > 0 ? "已脱冰跳跃" : "反向 已脱冰跳跃";
                    else if (code.Substring(0, 1) == "L")//吊装
                    {
                        if (tower == "直线塔" || tower == "直转塔")
                        {
                            if (new Regex(@"[1-9]00[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "正安装" : "反向 正安装";
                            else if (new Regex(@"[1-9]0[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "检修提线" : "反向 检修提线";
                           
                        }
                        else
                        {
                            if (new Regex(@"200[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "导线有荷载，跳线正吊" : "反向 导线有荷载，跳线正吊";
                            else if (new Regex(@"[1-9]0[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "导线有荷载，跳线已吊" : "反向 导线有荷载，跳线已吊";
                            else if (new Regex(@"[1-2]AB0[0-9]").Match(Math.Abs(key).ToString()).Success || new Regex(@"3ABC0[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "导线有荷载，跳线各侧已、正吊或未吊" : "反向 线有荷载，跳线各侧已、正吊或未吊";

                        }
                    }
                    else if (code.Substring(0, 1) == "C")//施工提线
                    {
                        if (new Regex(@"[1-9]00[0-9]").Match(Math.Abs(key).ToString()).Success || new Regex(@"1[1-9]00[0-9]").Match(Math.Abs(key).ToString()).Success)
                            value = key > 0 ? "施工提线" : "反向 施工提线"; 
                        else if (new Regex(@"[1-9]0[0-9]").Match(Math.Abs(key).ToString()).Success)
                            value = key > 0 ? "已锚" : "反向 已锚";
                    }
                    else if (code.Substring(0, 1) == "G")//过滑车
                    {
                        if (new Regex(@"[1-9]00[0-9]").Match(Math.Abs(key).ToString()).Success || new Regex(@"1[1-9]00[0-9]").Match(Math.Abs(key).ToString()).Success)
                            value = key > 0 ? "正过滑车" : "反向 正过滑车";
                        else if (new Regex(@"[1-9]0[0-9]").Match(Math.Abs(key).ToString()).Success)
                            value = key > 0 ? "已锚相" : "反向 已锚相";
                    }
                    else if (code.Substring(0, 1) == "M")//锚线
                    {
                        if (code.Length>1&&code.Substring(0, 2) == "MQ")//锚兼牵
                        {
                            if (new Regex(@"[1-9]0[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "前侧已锚，后侧已挂" : "反向 前侧已锚，后侧已挂";

                        }
                        else
                        {

                            if (new Regex(@"[1-9]00[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "正锚" : "反向 正锚";
                            else if (new Regex(@"[1-9]0[0-9]").Match(Math.Abs(key).ToString()).Success)
                                value = key > 0 ? "已锚" : "反向 已锚"; 
                        }
                    }
                }
                if (char.Parse(code.Substring(code.Length - 1)) >= 97)
                    value += " 第" + code.Substring(code.Length - 1) + " 组挂点";

            }
            return value;
        }


        /// <summary>
        /// 张力角解析
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ConvertTensionAngleCode(string code)
        {
            char[] str = code.ToCharArray();

            string value = "";
            for(int i=0;i<str.Length;i++)
            {
                value += i == 0 ? "前侧" : "后侧";
                switch(str[i])
                {
                    case 'D':
                        value+= "大张力，";
                        break;
                    case 'X':
                        value += "小张力，";
                        break;
                    case '0':
                        value += "张力为0，";
                        break;
                }
            }

            return value.TrimEnd('，');

        }

        /// <summary>
        /// 垂直垂荷解析
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ConvertVertialLoadCode(string code)
        {
            char[] str = code.ToCharArray();

            string value = "";
            for (int i = 0; i < str.Length; i++)
            {
                value += i == 0 ? "前侧" : "后侧";
                switch (str[i])
                {
                    case 'Y':
                        value += "下压，";
                        break;
                    case 'T':
                    case 'B':
                        value += "上拔，";
                        break;
                    case '0':
                        value += "压力为0，";
                        break;
                }
            }

            return value.TrimEnd('，');
        }
    }
}
