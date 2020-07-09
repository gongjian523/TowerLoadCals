using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    public class HangingPointLoadComposeBase
    {
        protected HangingPointParas pointParas = new HangingPointParas();

        protected StruCalsDicGroup dicGroup = new StruCalsDicGroup();

        protected HangingPointSettingParas ratioParas = new HangingPointSettingParas();

        protected List<string> dicComposeInfo = new List<string>();
        protected StruCalsDicOption option = new StruCalsDicOption();

        protected TowerTemplate towerTemplate = new TowerTemplate();

        //工况号
        protected int iIndex = 0;

        protected int jIndex = 0;

        protected string orientation;

        protected string wireType;

        protected WorkConditionCombo wd;

        protected float[,] lineLoad;

        protected float[,] xLineLoad;
        protected float[,] yLineLoad;
        protected float[,] zLineLoad;


        public HangingPointLoadComposeBase(int i, int j, string orient, float[,] xLoad, float[,] yLoad, float[,] zLoad, string group, string link,  
            List<HangingPointParas> pointsParas, HangingPointSettingParas raios, TowerTemplate template, List<StruCalsDicGroup> dicGrps)
        {
            iIndex = i;
            jIndex = j;
            orientation = orient;

            xLineLoad = xLoad;
            yLineLoad = yLoad;
            zLineLoad = zLoad;

            if (orient == "X")
                lineLoad = xLoad;
            else if (orient == "Y")
                lineLoad = yLoad;
            else
                lineLoad = zLoad;

            ratioParas = raios;
            towerTemplate = template;

            wireType = towerTemplate.Wires[i];
            wd = towerTemplate.WorkConditionCombos[j];

            StruCalsDicGroup dicGroup = new StruCalsDicGroup();

            if(link.Contains("常规"))
            {
                pointParas = pointsParas.Where(item => item.WireType == wireType).First();

                string vStr = pointParas.StringType.Contains("V") ? "V串" : pointParas.StringType;
                dicGroup = dicGrps.Where(item => item.Group == group && item.FixedType == vStr && item.Link == link).First();
            }
            else if(link.Contains("转向"))
            {
                pointParas = pointsParas.Where(item => item.WireType == wireType).First();
                dicGroup = dicGrps.Where(item => item.Group == group && item.Link == link).First();
            }
            else
            {
                string wCC = template.WorkConditionCombos[jIndex].WorkConditionCode;
                string array = wCC.Substring(wCC.Length - 1);

                pointParas = pointsParas.Where(item => item.WireType == wireType && item.Array != null && item.Array.Contains(array)).First();
                dicGroup = dicGrps.Where(item => item.Group == group && item.Link == link).First();
            }

            option = dicGroup.Options.Where(item => item.Num == pointParas.PointNum).First();
            dicComposeInfo = option.ComposrInfos.Where(item => item.Orientation == orientation).First().PointCompose;
        }


        public void ComposeHangingPointsLoad(out string resStr, out List<StruCalsPointLoad> resList, bool isTurningPoint = false)
        {
            resList = new List<StruCalsPointLoad>();

            string preStr = "  " + wireType;

            if (isTurningPoint)
                preStr += "转向荷载";

            preStr += ("工况" + (jIndex+1).ToString()).PadLeft(8);
            resStr = preStr + orientation.PadLeft(6) + lineLoad[jIndex, iIndex].ToString("0.00").PadLeft(10);

            //导线V串
            if(pointParas.StringType != null &&  pointParas.StringType.Contains("V"))
            {
                //跳跃脱冰工况
                if(wd.WorkConditionCode == "T" && wd.WireIndexCodes[iIndex] > 10)
                {
                    if(!wireType.Contains("右"))
                    {
                        //左边和中间的导线，获取左侧挂点
                        for (int kl = 0; kl < option.LeftPoints.Count(); kl++)
                        {
                            int pointIndex = Convert.ToInt16(option.LeftPoints[kl].Substring(1)) - 1;

                            GetPointProportionAndLoad(dicComposeInfo[pointIndex], lineLoad[jIndex, iIndex], out float proportion, out float laod);

                            resStr += pointParas.Points[pointIndex].PadLeft(10) + proportion.ToString("0.00").PadLeft(10) + laod.ToString("0.00").PadLeft(10);

                            resList.Add(new StruCalsPointLoad()
                            {
                                Name = Convert.ToInt16(pointParas.Points[pointIndex]),
                                Wire = wireType,
                                WorkConditionId = jIndex,
                                Orientation = orientation,
                                Proportion = proportion,
                                Load = laod,
                                HPSettingName = ratioParas.HangingPointSettingName,
                            });
                        }
                    }
                    else
                    {
                        for (int kr = 0; kr < option.RightPoints.Count(); kr++)
                        {
                            int pointIndex = Convert.ToInt16(option.RightPoints[kr].Substring(1)) - 1;

                            GetPointProportionAndLoad(dicComposeInfo[pointIndex], lineLoad[jIndex, iIndex], out float proportion, out float laod);

                            resStr += pointParas.Points[pointIndex].PadLeft(10) + proportion.ToString("0.00").PadLeft(10) + laod.ToString("0.00").PadLeft(10);

                            resList.Add(new StruCalsPointLoad()
                            {
                                Name = Convert.ToInt16(pointParas.Points[pointIndex]),
                                Wire = wireType,
                                WorkConditionId = jIndex,
                                Orientation = orientation,
                                Proportion = proportion,
                                Load = laod,
                                HPSettingName = ratioParas.HangingPointSettingName,
                            });
                        }
                    }
                }
                else
                {
                    VStringParas vParas = ratioParas.VStrings.Where(item => item.Index == pointParas.StringType).First();

                    VStringCompose vStringCompose = new VStringCompose(vParas.L1, vParas.L2, vParas.H1, vParas.H2, vParas.StressLimit, xLineLoad[jIndex, iIndex], yLineLoad[jIndex, iIndex], zLineLoad[jIndex, iIndex]);

                    for (int kl = 0; kl < option.LeftPoints.Count(); kl++)
                    {
                        int pointIndex = Convert.ToInt16(option.LeftPoints[kl].Substring(1)) - 1;

                        GetPointProportionAndLoad(dicComposeInfo[pointIndex], lineLoad[jIndex, iIndex], out float proportion, out float laod);

                        if (orientation == "X")
                        {
                            laod = vStringCompose.VCX1 * proportion;
                        }
                        else if (orientation == "Y")
                        {
                            laod = vStringCompose.VCY1 * proportion;
                        }
                        else
                        {
                            laod = vStringCompose.VCZ1 * proportion;
                        }

                        resStr += pointParas.Points[pointIndex].PadLeft(10) + proportion.ToString("0.00").PadLeft(10) + laod.ToString("0.00").PadLeft(10);

                        resList.Add(new StruCalsPointLoad()
                        {
                            Name = Convert.ToInt16(pointParas.Points[pointIndex]),
                            Wire = wireType,
                            WorkConditionId = jIndex,
                            Orientation = orientation,
                            Proportion = proportion,
                            Load = laod,
                            HPSettingName = ratioParas.HangingPointSettingName,
                        });
                    }

                    for (int kr = 0; kr < option.RightPoints.Count(); kr++)
                    {
                        int pointIndex = Convert.ToInt16(option.RightPoints[kr].Substring(1)) - 1;

                        GetPointProportionAndLoad(dicComposeInfo[pointIndex], lineLoad[jIndex, iIndex], out float proportion, out float laod);

                        if (orientation == "X")
                        {
                            laod = vStringCompose.VCX2 * proportion;
                        }
                        else if (orientation == "Y")
                        {
                            laod = vStringCompose.VCY2 * proportion;
                        }
                        else
                        {
                            laod = vStringCompose.VCZ2 * proportion;
                        }

                        resStr += pointParas.Points[pointIndex].PadLeft(10) + proportion.ToString("0.00").PadLeft(10) + laod.ToString("0.00").PadLeft(10);

                        resList.Add(new StruCalsPointLoad()
                        {
                            Name = Convert.ToInt16(pointParas.Points[pointIndex]),
                            Wire = wireType,
                            WorkConditionId = jIndex,
                            Orientation = orientation,
                            Proportion = proportion,
                            Load = laod,
                            HPSettingName = ratioParas.HangingPointSettingName,
                        });
                    }

                    resStr = resStr + "     V串    左侧" + vStringCompose.VCX1.ToString("0.00").PadLeft(10) + vStringCompose.VCY1.ToString("0.00").PadLeft(10) + vStringCompose.VCZ1.ToString("0.00").PadLeft(10);
                    resStr = resStr + "    右侧" + vStringCompose.VCX2.ToString("0.00").PadLeft(10) + vStringCompose.VCY2.ToString("0.00").PadLeft(10) + vStringCompose.VCZ2.ToString("0.00").PadLeft(10);
                }

            }
            else
            // 针对地线（常规、悬臂）; 导线-I串: 吊装
            {
                for (int k = 0; k < pointParas.PointNum; k++)
                {
                    GetPointProportionAndLoad(dicComposeInfo[k], lineLoad[jIndex, iIndex], out float proportion, out float laod);

                    resStr += pointParas.Points[k].PadLeft(10) + proportion.ToString("0.00").PadLeft(10) + laod.ToString("0.00").PadLeft(10);

                    resList.Add(new StruCalsPointLoad()
                    {
                        Name = Convert.ToInt16(pointParas.Points[k]),
                        Wire = wireType,
                        WorkConditionId = jIndex,
                        Orientation = orientation,
                        Proportion = proportion,
                        Load = laod,
                        HPSettingName = ratioParas.HangingPointSettingName,
                    });
                }
            }
        }

        private void GetPointProportionAndLoad(string expressStr, float lineLoad, out float proportion, out float load)
        {

            if(expressStr.Contains("*"))
            {
                string[] expressList = expressStr.Split('*');
                proportion = 1;

                foreach (string expressItem in expressList)
                {
                    proportion *= GetProportion(expressItem);
                }
                
                load = lineLoad * proportion;
            }
            else if (expressStr.Contains("/"))
            {
                string[] expressList = expressStr.Split('/');
                proportion = GetProportion(expressList[0]) / GetProportion(expressList[1]);

                load = lineLoad * proportion;
            }
            else 
            {
                proportion = GetProportion(expressStr);
                load = lineLoad * proportion;
            }

        }

        private float GetProportion(string expressStr)
        {
            float result = 0;

            switch (expressStr.Trim())
            {
                case "[地线常规前侧_txtbox]":
                    result = ratioParas.GCQ;
                    break;
                case "[地线常规后侧_txtbox]":
                    result = ratioParas.GCH;
                    break;
                case "[地线悬臂内侧_txtbox]":
                    result = ratioParas.GXN;
                    break;
                case "[地线悬臂外侧_txtbox]":
                    result = ratioParas.GXW;
                    break;
                case "[前侧风荷其他_txtbox]":
                    result = ratioParas.DQWQ;
                    break;
                case "[后侧风荷其他_txtbox]":
                    result = ratioParas.DQWH;
                    break;
                case "[前侧垂荷其他_txtbox]":
                    result = ratioParas.DQCQ;
                    break;
                case "[后侧垂荷其他_txtbox]":
                    result = ratioParas.DQCH;
                    break;
                case "[前侧风荷吊装_txtbox]":
                    result = ratioParas.DDWQ;
                    break;
                case "[后侧风荷吊装_txtbox]":
                    result = ratioParas.DDWH;
                    break;
                case "[前侧垂荷吊装_txtbox]":
                    result = ratioParas.DDCQ;
                    break;
                case "[后侧垂荷吊装_txtbox]":
                    result = ratioParas.DDCH;
                    break;
                case "[前侧风荷锚线_txtbox]":
                    result = ratioParas.DMWQ;
                    break;
                case "[后侧风荷锚线_txtbox]":
                    result = ratioParas.DMWH;
                    break;
                case "[前侧垂荷锚线_txtbox]":
                    result = ratioParas.DMCQ;
                    break;
                case "[后侧垂荷锚线_txtbox]":
                    result = ratioParas.DMCH;
                    break;
                default:
                    result = (float)Convert.ToDecimal(expressStr);
                    break;
            }

            return result;
        }
    }
}
