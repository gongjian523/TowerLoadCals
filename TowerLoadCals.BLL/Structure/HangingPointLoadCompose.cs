using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    /// <summary>
    /// 结构计算-挂点荷载分配
    /// </summary>
    public class HangingPointLoadCompose
    {
        protected HangingPointParas pointParas = new HangingPointParas();

        protected StruCalsDicGroup dicGroup = new StruCalsDicGroup();
        protected List<StruCalsDicGroup> dicGroupOptions = new List<StruCalsDicGroup>();

        protected HangingPointSettingParas hpPointsParas = new HangingPointSettingParas();

        protected List<string> dicComposeInfo = new List<string>();
        protected StruCalsDicOption option = new StruCalsDicOption();

        protected TowerTemplate towerTemplate = new TowerTemplate();

        //工况号
        protected int iIndex = 0;
        //线的序号
        protected int jIndex = 0;

        protected string wireType;

        protected WorkConditionCombo wd;

        protected float[,] xLineLoad;
        protected float[,] yLineLoad;
        protected float[,] zLineLoad;

        protected string xyLink;
        protected string zLink;

        protected string group;

        protected string position;
        protected bool isTensionTower;

        public HangingPointLoadCompose(int i, int j, float[,] xLoad, float[,] yLoad, float[,] zLoad, string grp, string linkXY, string linkZ,
            HangingPointSettingParas hpSetting, TowerTemplate template, List<StruCalsDicGroup> dicGrps, string pos = null)
        {
            iIndex = i;
            jIndex = j;

            xLineLoad = xLoad;
            yLineLoad = yLoad;
            zLineLoad = zLoad;

            xyLink = linkXY;
            zLink = linkZ;

            if (pos == null)
            {
                isTensionTower = false;
            }
            else
            {
                isTensionTower = true;
                position = pos;
            }

            group = grp;

            hpPointsParas = hpSetting;
            towerTemplate = template;

            wireType = towerTemplate.Wires[i];
            wd = towerTemplate.WorkConditionCombos[j];

            dicGroupOptions = dicGrps;
        }

        protected void SetParas(string link, string orientation)
        {
            StruCalsDicGroup dicGroup;


            //常规挂点
            if (link.Contains("常规"))
            {
                //悬垂塔常规挂点XY和Z向分别来自NormalXYPoints和NormalZPoints
                if (!isTensionTower)
                {
                    if (orientation == "X" || orientation == "Y")
                        pointParas = hpPointsParas.NormalXYPoints.Where(item => item.WireType == wireType).First();
                    else
                        pointParas = hpPointsParas.NormalZPoints.Where(item => item.WireType == wireType).First();
                }
                //耐张塔常规导线挂点XYZ向全部来做NormalXYPoints， 常规跳线挂点XYZ向全部来做NormalZPoints
                else
                {
                    if (!link.Contains("跳线"))
                        pointParas = hpPointsParas.NormalXYPoints.Where(item => item.WireType == wireType).First();
                    else
                        pointParas = hpPointsParas.NormalZPoints.Where(item => item.WireType == wireType).First();
                }

                string vStr = pointParas.StringType.Contains("V") ? "V串" : pointParas.StringType;
                dicGroup = dicGroupOptions.Where(item => item.Group == group && item.FixedType == vStr && item.Link == link).First();
            }
            //转向和过滑车挂点
            else if (link.Contains("转向") || link.Contains("过滑车"))
            {
                //转向和过滑车挂点XYZ向都来自TurningPoints
                pointParas = hpPointsParas.TurningPoints.Where(item => item.WireType == wireType).First();
                dicGroup = dicGroupOptions.Where(item => item.Group == group && item.Link == link).First();
            }
            //吊装挂点
            else
            {
                List<HangingPointParas> pointsParas;

                //悬垂塔吊装挂点XY和Z向分别来自InstallXYPoints和InstallZPoints
                //耐张塔跳线吊装挂点XY和Z向分别来自InstallXYPoints和InstallZPoints
                if (orientation == "X" || orientation == "Y")
                    pointsParas = hpPointsParas.InstallXYPoints;
                else
                    pointsParas = hpPointsParas.InstallZPoints;


                //吊装挂点会分a/b/c组,需要找到对应组别的挂点信息
                string wCC = towerTemplate.WorkConditionCombos[jIndex].WorkConditionCode;
                string array = wCC.Substring(wCC.Length - 1);
                pointParas = pointsParas.Where(item => item.WireType == wireType && item.Array != null && item.Array.Contains(array)).First();

                if (!isTensionTower)
                {
                    //悬垂塔的吊装挂点全部按照I串
                    dicGroup = dicGroupOptions.Where(item => item.Group == group && item.Link == link).First();
                }
                else
                {
                    //耐张塔需要区分I串V串两种情况
                    string vStr = pointParas.StringType.Contains("V") ? "V串" : pointParas.StringType;
                    dicGroup = dicGroupOptions.Where(item => item.Group == group && item.FixedType == vStr && item.Link == link).First();
                }
            }

            option = dicGroup.Options.Where(item => item.Num == pointParas.PointNum).First();
            dicComposeInfo = option.ComposrInfos.Where(item => item.Orientation == orientation).First().PointCompose;
        }

        public void ComposeHangingXPointsLoad(out string resStr, out List<StruCalsPointLoad> resList, bool isTurningPoint = false)
        {
            SetParas(xyLink, "X");
            ComposeHangingPointsLoad(xLineLoad[jIndex, iIndex], "X", out resStr, out resList, isTurningPoint);
        }

        public void ComposeHangingYPointsLoad(out string resStr, out List<StruCalsPointLoad> resList, bool isTurningPoint = false)
        {
            SetParas(xyLink, "Y");
            ComposeHangingPointsLoad(yLineLoad[jIndex, iIndex], "Y", out resStr, out resList, isTurningPoint);
        }

        public void ComposeHangingZPointsLoad(out string resStr, out List<StruCalsPointLoad> resList, bool isTurningPoint = false)
        {
            SetParas(zLink, "Z");
            ComposeHangingPointsLoad(zLineLoad[jIndex, iIndex], "Z", out resStr, out resList, isTurningPoint);
        }

        public void ComposeHangingPointsLoad(float load, string orientation, out string resStr, out List<StruCalsPointLoad> resList, bool isTurningPoint = false)
        {
            resList = new List<StruCalsPointLoad>();

            resStr = "  " + wireType;
            if (orientation == "X" || orientation == "Y")
            {
                if (xyLink.Contains("跳线"))
                    resStr += "跳线";
            }
            else
            {
                if (zLink.Contains("跳线"))
                    resStr += "跳线";
            }

            if (position != null)
                resStr += position;

            if (isTurningPoint)
                resStr += "转向荷载";

            resStr += ("工况" + (jIndex + 1).ToString()).PadLeft(8);
            resStr = resStr + orientation.PadLeft(6) + load.ToString("0.00").PadLeft(10);

            //导线V串
            if (pointParas.StringType != null && pointParas.StringType.Contains("V"))
            {
                //V串的跳跃冰工况，并且工况代号大于10000），不按照常规的V串规则分配荷载;
                //分配的规则是：
                //左边和中间的导线，荷载按照I串规则全部分配在V串左侧挂点;
                //右边的导线，荷载按照I串规则全部分配在V串右侧挂点;
                if (wd.WorkConditionCode == "T" && wd.WireIndexCodes[iIndex] > 1000)
                {
                    //悬垂塔根据导线的方向遍历这个线左边或者右边的所有点
                    if (!isTensionTower)
                    {
                        //左边和中间的导线，获取左侧挂点
                        if (!wireType.Contains("右"))
                        {
                            for (int kl = 0; kl < option.LeftPoints.Count(); kl++)
                            {
                                int pointIndex = Convert.ToInt32(option.LeftPoints[kl].Substring(1)) - 1;

                                GeneratePointsLoadNormal(pointIndex, load, orientation, out string loadStr, out StruCalsPointLoad pLoad);
                                resStr += loadStr;
                                resList.Add(pLoad);
                            }
                        }
                        //右边的导线，获取右侧挂点;
                        else
                        {
                            for (int kr = 0; kr < option.RightPoints.Count(); kr++)
                            {
                                int pointIndex = Convert.ToInt32(option.RightPoints[kr].Substring(1)) - 1;

                                GeneratePointsLoadNormal(pointIndex, load, orientation, out string loadStr, out StruCalsPointLoad pLoad);
                                resStr += loadStr;
                                resList.Add(pLoad);
                            }
                        }
                    }
                    //悬垂塔根据导线的方向遍历指定位置上左边或者右边的所有点
                    else
                    {
                        string[] points = GetPositionPointsInTensionTower();

                        //左边和中间的导线，获取左侧挂点
                        if (!wireType.Contains("右"))
                        {
                            for (int kl = 0; kl < points.Count(); kl++)
                            {
                                //不在指定位置上的点不用分配荷载
                                if (option.LeftPoints.Where(p => p == points[kl]).Count() == 0)
                                    continue;
                                int pointIndex = Convert.ToInt32(option.LeftPoints[kl].Substring(1)) - 1;

                                GeneratePointsLoadNormal(pointIndex, load, orientation, out string loadStr, out StruCalsPointLoad pLoad);
                                resStr += loadStr;
                                resList.Add(pLoad);
                            }
                        }
                        else
                        {
                            for (int kr = 0; kr < points.Count(); kr++)
                            {
                                //不在指定位置上的点不用分配荷载
                                if (option.RightPoints.Where(p => p == points[kr]).Count() == 0)
                                    continue;
                                int pointIndex = Convert.ToInt32(option.RightPoints[kr].Substring(1)) - 1;

                                GeneratePointsLoadNormal(pointIndex, load, orientation, out string loadStr, out StruCalsPointLoad pLoad);
                                resStr += loadStr;
                                resList.Add(pLoad);
                            }
                        }
                    }
                }
                else
                {
                    VStringParas vParas = hpPointsParas.VStrings.Where(item => item.Index == pointParas.StringType).First();

                    VStringCompose vStringCompose = new VStringCompose(vParas.L1, vParas.L2, vParas.H1, vParas.H2, vParas.StressLimit, xLineLoad[jIndex, iIndex], yLineLoad[jIndex, iIndex], zLineLoad[jIndex, iIndex]);

                    //悬垂塔先后遍历这个线左边和右边的所有点
                    if (!isTensionTower)
                    {
                        //左侧挂点
                        for (int kl = 0; kl < option.LeftPoints.Count(); kl++)
                        {
                            int pointIndex = Convert.ToInt32(option.LeftPoints[kl].Substring(1)) - 1;

                            //非跳跃冰工况的V串需要按照V串的的规则计算比例
                            GeneratePointsLoadVString(pointIndex, orientation, true, vStringCompose, out string loadStr, out StruCalsPointLoad pLoad);
                            resStr += loadStr;
                            resList.Add(pLoad);
                        }

                        //右侧挂点
                        for (int kr = 0; kr < option.RightPoints.Count(); kr++)
                        {
                            int pointIndex = Convert.ToInt32(option.RightPoints[kr].Substring(1)) - 1;

                            //非跳跃冰工况的V串需要按照V串的的规则计算比例
                            GeneratePointsLoadVString(pointIndex, orientation, false, vStringCompose, out string loadStr, out StruCalsPointLoad pLoad);
                            resStr += loadStr;
                            resList.Add(pLoad);
                        }
                    }
                    //耐张塔先后遍历这个线左边和右边的指定方向上的点
                    else
                    {
                        string[] points = GetPositionPointsInTensionTower();

                        //左侧挂点
                        for (int kl = 0; kl < points.Count(); kl++)
                        {
                            //不在指定位置上的点不用分配荷载
                            if (option.LeftPoints.Where(p => p == points[kl]).Count() == 0)
                                continue;
                            int pointIndex = Convert.ToInt32(points[kl].Substring(1)) - 1;

                            //非跳跃冰工况的V串需要按照V串的的规则计算比例
                            GeneratePointsLoadVString(pointIndex, orientation, false, vStringCompose, out string loadStr, out StruCalsPointLoad pLoad);
                            resStr += loadStr;
                            resList.Add(pLoad);
                        }

                        //右侧挂点
                        for (int kr = 0; kr < points.Count(); kr++)
                        {
                            //不在指定位置上的点不用分配荷载
                            if (option.RightPoints.Where(p => p == points[kr]).Count() == 0)
                                continue;
                            int pointIndex = Convert.ToInt32(points[kr].Substring(1)) - 1;

                            //非跳跃冰工况的V串需要按照V串的的规则计算比例
                            GeneratePointsLoadVString(pointIndex, orientation, false, vStringCompose, out string loadStr, out StruCalsPointLoad pLoad);
                            resStr += loadStr;
                            resList.Add(pLoad);
                        }
                    }

                    resStr = resStr + "     V串    左侧" + vStringCompose.VCX1.ToString("0.00").PadLeft(10) + vStringCompose.VCY1.ToString("0.00").PadLeft(10) + vStringCompose.VCZ1.ToString("0.00").PadLeft(10);
                    resStr = resStr + "    右侧" + vStringCompose.VCX2.ToString("0.00").PadLeft(10) + vStringCompose.VCY2.ToString("0.00").PadLeft(10) + vStringCompose.VCZ2.ToString("0.00").PadLeft(10);
                }

            }
            else
            // 针对地线（常规、悬臂）; 导线-I串: 吊装
            {
                //悬垂塔遍历这个线条上面的所有点
                if(!isTensionTower)
                {
                    for (int k = 0; k < pointParas.PointNum; k++)
                    {
                        GeneratePointsLoadNormal(k, load, orientation, out string loadStr, out StruCalsPointLoad pLoad);
                        resStr += loadStr;
                        resList.Add(pLoad);
                    }
                }
                //耐张塔遍历指定位置上的所有点
                else
                {
                    string[] points = GetPositionPointsInTensionTower();

                    for (int k = 0; k < points.Count(); k++)
                    {
                        int pointIndex = Convert.ToInt32(points[k].Substring(1)) - 1;

                        GeneratePointsLoadNormal(pointIndex, load, orientation, out string loadStr, out StruCalsPointLoad pLoad);
                        resStr += loadStr;
                        resList.Add(pLoad);
                    }
                }
            }
        }

        /// <summary>
        /// 获取不同位置上的点
        /// </summary>
        /// <returns></returns>
        protected string[] GetPositionPointsInTensionTower()
        {
            if (position == "前侧")
                return option.FrontPoints;
            else if (position == "中部")
                return option.CentralPoints;
            else
                return option.BackPoints;
        }

        private void GetPointProportion(string expressStr, out float proportion)
        {
            if (expressStr.Contains("*"))
            {
                string[] expressList = expressStr.Split('*');
                proportion = 1;

                foreach (string expressItem in expressList)
                {
                    proportion *= DecodeProportion(expressItem);
                }
            }
            else if (expressStr.Contains("/"))
            {
                string[] expressList = expressStr.Split('/');
                proportion = DecodeProportion(expressList[0]) / DecodeProportion(expressList[1]);
            }
            else
            {
                proportion = DecodeProportion(expressStr);
            }
        }

        private void GetPointProportionAndLoad(string expressStr, float lineLoad, out float proportion, out float load)
        {
            GetPointProportion(expressStr, out proportion);

            load = lineLoad * proportion;
        }

        private float DecodeProportion(string expressStr)
        {
            float result;

            switch (expressStr.Trim())
            {
                case "[地线常规前侧_txtbox]":
                    result = hpPointsParas.GCQ;
                    break;
                case "[地线常规后侧_txtbox]":
                    result = hpPointsParas.GCH;
                    break;
                case "[地线悬臂内侧_txtbox]":
                    result = hpPointsParas.GXN;
                    break;
                case "[地线悬臂外侧_txtbox]":
                    result = hpPointsParas.GXW;
                    break;
                case "[前侧风荷其他_txtbox]":
                    result = hpPointsParas.DQWQ;
                    break;
                case "[后侧风荷其他_txtbox]":
                    result = hpPointsParas.DQWH;
                    break;
                case "[前侧垂荷其他_txtbox]":
                    result = hpPointsParas.DQCQ;
                    break;
                case "[后侧垂荷其他_txtbox]":
                    result = hpPointsParas.DQCH;
                    break;
                case "[前侧风荷吊装_txtbox]":
                    result = hpPointsParas.DDWQ;
                    break;
                case "[后侧风荷吊装_txtbox]":
                    result = hpPointsParas.DDWH;
                    break;
                case "[前侧垂荷吊装_txtbox]":
                    result = hpPointsParas.DDCQ;
                    break;
                case "[后侧垂荷吊装_txtbox]":
                    result = hpPointsParas.DDCH;
                    break;
                case "[前侧风荷锚线_txtbox]":
                    result = hpPointsParas.DMWQ;
                    break;
                case "[后侧风荷锚线_txtbox]":
                    result = hpPointsParas.DMWH;
                    break;
                case "[前侧垂荷锚线_txtbox]":
                    result = hpPointsParas.DMCQ;
                    break;
                case "[后侧垂荷锚线_txtbox]":
                    result = hpPointsParas.DMCH;
                    break;
                default:
                    result = (float)Convert.ToDecimal(expressStr);
                    break;
            }

            return result;
        }


        /// <summary>
        /// 平常挂点的分配
        /// </summary>
        protected void GeneratePointsLoadNormal(int index, float lineload, string orientation,  out string loadStr, out StruCalsPointLoad pointLoad)
        {
            GetPointProportionAndLoad(dicComposeInfo[index], lineload, out float proportion, out float laod);

            loadStr = pointParas.Points[index].PadLeft(10) + proportion.ToString("0.00").PadLeft(10) + laod.ToString("0.00").PadLeft(10);

            pointLoad = new StruCalsPointLoad()
            {
                Name = Convert.ToInt32(pointParas.Points[index]),
                Wire = wireType,
                WorkConditionId = jIndex,
                Orientation = orientation,
                Proportion = proportion,
                Load = laod,
                HPSettingName = hpPointsParas.HangingPointSettingName,
            };
        }

        /// <summary>
        /// V串挂点的分配
        /// </summary>
        protected void GeneratePointsLoadVString(int index, string orientation, bool isLeft, VStringCompose vString, out string loadStr, out StruCalsPointLoad pointLoad)
        {
            GetPointProportion(dicComposeInfo[index], out float proportion);

            float load;
            if (orientation == "X")
            {
                load = (isLeft ? vString.VCX1: vString.VCX2) * proportion;
            }
            else if (orientation == "Y")
            {
                load = (isLeft ? vString.VCY1 : vString.VCY2) * proportion;
            }
            else
            {
                load = (isLeft ? vString.VCZ1 : vString.VCZ2) * proportion;
            }

            loadStr = pointParas.Points[index].PadLeft(10) + proportion.ToString("0.00").PadLeft(10) + load.ToString("0.00").PadLeft(10);

            pointLoad = new StruCalsPointLoad()
            {
                Name = Convert.ToInt32(pointParas.Points[index]),
                Wire = wireType,
                WorkConditionId = jIndex,
                Orientation = orientation,
                Proportion = proportion,
                Load = load,
                HPSettingName = hpPointsParas.HangingPointSettingName,
            };
        }
    }





}
