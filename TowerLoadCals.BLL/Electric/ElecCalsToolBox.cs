﻿using System;
using System.Linq;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsToolBox
    {

        /// <summary>
        /// 换算高度
        /// 注意：风荷载调整系数再核实End public static double
        /// </summary>
        /// <param name="voltage"></param>
        /// <param name="ACorDC"></param>
        /// <returns></returns>
        public static int GetLevelOfLine(int voltage, int ACorDC)
        {
            //默认都为3类线路
            int LeValue = 3;   
            if(ACorDC == 1){
                //直流线路都认为是1类线路
                LeValue = 1;
            } 
            else if(voltage >= 330){
                //交流330kV以上认为是1类线路
                LeValue = 1;
            }
            else if(voltage >= 220){
                //交流220kV以上认为是2类线路
                LeValue = 2;
            }

            return LeValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voltage"></param>
        /// <returns></returns>
        public static double AveHeightDefault(int voltage)
        {
            double height = 0;

            if (voltage <= 220){
                height = 15;
            }
            else if( voltage > 220 && voltage <= 500){
                height = 20;
            }
            else if( voltage > 500 && voltage <= 1000){
                height = 30;
            }
            else if (voltage >= 1100){
                height = 40;
            }
            return height;
        }

        /// <summary>
        /// 电线体形系数μsc
        /// </summary>
        /// <param name="ice">工况覆冰厚度</param>
        /// <param name="d">导线直径</param>
        /// <returns></returns>
        public static double WindEpson(double ice, double d)
        {
            double Epson;

            //风载体形系数
            if (d >= 17 && ice <= 0){
                Epson = 1.1;
            }
            else{
                Epson = 1.2;
            }
            return Epson;
        }


        /// 风压不均匀系数Alpha
        /// V为工况的风速,CalType=1为应力计算，其他为间隙校核
        /// </summary>
        public static double WindAlpha(double wind, double ice = 0, int CalType= 1)
        {
            //风速不均匀系数
            double Alpha = 0;

            if(CalType == 1)
            {
                if (wind < 20){
                    Alpha = 1;
                }
                else if (wind >= 20 && wind < 27){
                    Alpha = 0.85;
                }
                else if (wind >= 27 && wind < 31.5){
                    Alpha = 0.75;
                }
                else{
                    Alpha = 0.7;
                }
            }
            else
            {
                if (wind < 20){
                    Alpha = 1;
                }
                else if( wind >= 20 && wind< 27){
                    Alpha = 0.75;
                }
                else if(wind >= 27 && wind< 31.5){
                    Alpha = 0.61;
                }
                else{
                    Alpha = 0.61;
                }
            }

            //覆冰情况下，直接取1
            if (ice > 0){
                Alpha = 1;
            }
            return Alpha;
        }

        /// <summary>
        ///根据地形类别换算风速
        ///地形类别，B类系数1.6，其他类待定
        /// </summary>
        /// <param name="wind"></param>
        /// <param name="height"></param>
        /// <param name="TerType"></param>
        /// <returns></returns>
        public static double WindExChange(double wind, double height, char TerType)
        {
            //换算后风速
            if (TerType == 'A')
            {
                wind = wind * Math.Sqrt(1.379) * (Math.Pow((height / 10),0.12));
            }
            else if (TerType == 'B')
            {
                wind = wind * Math.Sqrt(1) * (Math.Pow((height / 10), 0.16));
            }
            else if (TerType == 'C')
            {
                wind = wind * Math.Sqrt(0.616) * (Math.Pow((height / 10), 0.22));
            }
            else if (TerType== 'D')
            {
                wind = wind * Math.Sqrt(0.318) * (Math.Pow((height / 10),0.30));
            }

            return Math.Round(wind, 2);
        }

        public static double WindExChange(double wind, double height, double terrainPara)
        {
            return Math.Round(Math.Pow((height / 10), terrainPara) * wind, 2);
        }

        /// <summary>
        /// 顺线路外角侧45风,逆线路内角侧45风
        /// </summary>
        /// <param name="maxWind"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double Wind45ExChange1(double maxWind, double angle)
        {
            var wind = maxWind * Math.Sin((45 + angle / 2) / 180 * Math.PI);

            return Math.Round(wind, 2);
        }

        /// <summary>
        /// 逆线路外角侧45风, 顺线路内角侧45风
        /// </summary>
        /// <param name="maxWind"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double Wind45ExChange2(double maxWind, double angle)
        {
            var wind = maxWind * Math.Sin((45 - angle / 2) / 180 * Math.PI);
            return Math.Round(wind, 2);
        }

        /// <summary>
        /// 断线和补不均匀冰工况下的冰厚换算
        /// </summary>
        /// <param name="dia">导地线的直径</param>
        /// <param name="iceCover">断线覆冰率</param>
        /// <param name="iceThick">冰厚</param>
        /// <returns></returns>
        public static double IceThicknessExChange(double dia, double iceCover,double iceThick)
        {
            var thickExc = (-dia + Math.Sqrt(Math.Pow(dia,2) + 4 * iceCover * (dia + iceThick) * iceThick)) / 2;
            return Math.Round(thickExc, 2);
        }

        /// <summary>
        /// 风荷载增大系数B
        /// </summary>
        /// <param name="ice"> ice覆冰厚度，不支持覆冰为</param>
        /// <returns></returns>
        public static double WindLoadEnlargeCoe(double ice)
        {
            //风荷载增大系数B
            double enlargeCoe;
            if (ice <= 5){
                enlargeCoe = 1;
            }
            else if( ice >= 5 && ice<10){
                enlargeCoe = 1.1f;
            }
            else if( ice >= 10 && ice<15){
                enlargeCoe = 1.2f;
            }
            //覆冰增大系数的取值与传统计算不符，注意后期调整
            else if( ice >= 15 && ice<20){
                enlargeCoe = 1.3f;
            }
            else if( ice >= 20 && ice<30){
                enlargeCoe = 1.5f;
            }
            else if( ice >= 30 && ice<= 50){
                enlargeCoe = 1.8f;
            }
            else{
                //大于20mm取中间值，规范要求为2.0-2.5
                enlargeCoe = 2;  
            }
            return enlargeCoe;
        }

        /// <summary>
        /// 求解方程，形式为x**2（x+a）=b,采用邵天晓方法计算
        /// </summary>
        /// <param name="Tj_A"></param>
        /// <param name="Tj_B"></param>
        /// <returns></returns>
        public static double caculateCurDelta(double Tj_A, double Tj_B)
        {
            var CalA = Math.Pow(Tj_B, 2) / 4 - Tj_B * Math.Pow(Tj_A, 3) / 27;
            var CalB = Tj_B / 2 - Math.Pow(Tj_A, 3) / 27;

            double Delta;

            if (CalA >= 0)
            {
                Delta = Math.Pow((CalB + Math.Sqrt(CalA)), (1 / 3)) + Math.Pow((CalB - Math.Sqrt(CalA)), (1 / 3)) - Tj_A / 3;
            }
            else
            {
                double Tho;
                if( CalB > 0)
                {
                    Tho = Math.Atan(Math.Sqrt(-CalA) / CalB);
                }
                else if( CalB== 0)
                {
                    Tho = Math.PI / 2;
                }
                else
                {
                    Tho = Math.PI + Math.Atan(Math.Sqrt(-CalA) / CalB);
                }
                Delta = Tj_A / 3 * (2 * Math.Cos(Tho / 3) - 1);
            }

            return Delta;
        }

        /// <summary>
        /// 断线张力
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="iceType"></param>
        /// <param name="iceThickness"></param>
        /// <param name="terrain"></param>
        /// <param name="wireType"></param>
        /// <param name="devideNum"></param>
        /// <returns></returns>
        public static double UBlanceK(string towerType, string iceType, double iceThickness, string terrain , string wireType, int devideNum = 0)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();

            if (elecCalsSpec == null)
                return 0;

            string wire;

            if (wireType == "地线"){
                wire = "地线";
            }
            else{ 
                if (towerType == "悬垂塔")
                {
                    //导线和塔型、冰区类型以及分裂数有关
                    if (iceType == "轻冰区" || iceType == "中冰区")
                    {
                        if (devideNum == 1){
                            wire = "单导线";
                        }
                        else if(devideNum == 2){
                            wire = "双分裂导线";
                        }
                        else{
                            wire = "双分裂以上导线";
                        }
                    }
                    //重冰区
                    else{
                        wire = "导线";
                    }
                }
                //耐张塔
                else{
                    wire = devideNum == 1 ? "单导线" : "双分裂及以上导线";
                }
            }

            ElecCalsBreakWireSpec entity;

            if (iceType == "轻冰区")
            {
                //轻冰区需要查询地形
                entity = elecCalsSpec.BreakWireStress.Where(item => item.IceArea == iceType && item.IceThickness == iceThickness && item.WireType == wire
                    && item.TowerType == towerType && item.Terrain == terrain).FirstOrDefault();
            }
            else
            {
                entity = elecCalsSpec.BreakWireStress.Where(item => item.IceArea == iceType && item.IceThickness == iceThickness && item.WireType == wire
                    && item.TowerType == towerType).FirstOrDefault();
            }

            return entity == null ? 0 : (double)entity.Stress / 100;
        }

        /// <summary>
        /// 断线覆冰率 耐张塔
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="iceThickness1"></param>
        /// <param name="iceThickness2"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static double UBlanceR(string towerType, double iceThickness1, double iceThickness2, string  category)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return 1;

            double iceThickness = iceThickness1 < iceThickness2 ? iceThickness2 : iceThickness1;

            var entity = elecCalsSpec.BreakIceRate.Where(item => item.TowerType == towerType && item.IceThickness == iceThickness && item.Category == category).FirstOrDefault();
            return entity ==null ? 1 :(double)entity.Percent / 100;
        }

        /// <summary>
        /// 断线覆冰率
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="iceThickness"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static double UBlanceR(string towerType, double iceThickness, string category)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return 1;

            var entity = elecCalsSpec.BreakIceRate.Where(item => item.TowerType == towerType && item.IceThickness == iceThickness && item.Category == category).FirstOrDefault();
            return entity == null ? 1 : (double)entity.Percent / 100;
        }

        /// <summary>
        /// 不均匀冰不平衡张力
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="iceType"></param>
        /// <param name="iceThickness"></param>
        /// <param name="wireType"></param>
        /// <returns></returns>
        public static double IBlanceK(string towerType, string iceType, double iceThickness, string  wireType)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return 0;

            var entity = elecCalsSpec.UnevenIceStress.Where(item => item.IceArea == iceType && item.IceThickness == iceThickness && item.WireType == wireType
                && item.TowerType == towerType).FirstOrDefault();
            return entity == null ? 0 : (double)entity.Stress / 100;

        }

        /// <summary>
        /// 不均匀冰重冰覆冰率-一侧
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static double IBlanceR1(string towerType, string category)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return 0;

            var entity = elecCalsSpec.UnevenIceRate.Where(item => item.TowerType == towerType && item.Category == category && item.Side == "一侧").FirstOrDefault();
            return entity == null ? 0 : (double)entity.Percent / 100;
        }

        /// <summary>
        /// 不均匀冰重冰覆冰率-另一侧
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static double IBlanceR2(string towerType, string category)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return 0;

            var entity = elecCalsSpec.UnevenIceRate.Where(item => item.TowerType == towerType && item.Category == category && item.Side == "另一侧").FirstOrDefault();
            return entity == null ? 0 :(double)entity.Percent / 100;
        }

        
        public static string GetCatogory ( string volt)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return "一类";

            var entity =  elecCalsSpec.Category.Where(item => item.Voltage.Contains(volt)).FirstOrDefault();
            return entity == null ? "一类" : entity.Category;
        }
    }
}
