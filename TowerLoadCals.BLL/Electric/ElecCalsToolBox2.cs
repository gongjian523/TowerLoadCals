using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsToolBox2
    {
        /// <summary>
        /// 比载计算BiZai(1e-3 kg/m/mm2)
        /// </summary>
        /// <param name="wei">单位重量（kg/km</param>
        /// <param name="dia">导线外径(mm)</param>
        /// <param name="sec">截面积(mm2)</param>
        /// <param name="iceThickness">覆冰厚度(mm)</param>
        /// <param name="windSpeed">风速(m/s)</param>
        /// <param name="windSpeedBase">基本风速</param>
        /// <returns></returns>
        public static double BiZai(double wei, double dia, double sec, double iceThickness, double windSpeed, double windSpeedBase)
        {
            double Usc = UscValue(iceThickness,dia);
            double a = aValue(windSpeedBase);
            double BB = BBValue(iceThickness);

            //BiZai = ((Q / S + 2.82743334 * b * (d + b) / S) ^ 2 + (a * Usc * (d + 2 * b) * v ^ 2 / 16 / S * BB) ^ 2) ^ (1 / 2)
            double BiZai = Math.Pow(Math.Pow(wei / sec + 2.82743334 * iceThickness * (dia + iceThickness) / sec, 2) + Math.Pow(a * Usc * (dia + 2 * iceThickness) * Math.Pow(windSpeed, 2) / 1.6 / sec * BB / ConstVar.GraAcc, 2), 0.5);
            return BiZai;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizai1">比载(kg/m/mm2)</param>
        /// <param name="bizai2">比载(kg/m/mm2)</param>
        /// <param name="bizai3">比载(kg/m/mm2)</param>
        /// <param name="bizai4">比载(kg/m/mm2)</param>
        /// <param name="gkName1">工况名字</param>
        /// <param name="gkName2">工况名字</param>
        /// <param name="gkName3">工况名字</param>
        /// <param name="gkName4">工况名字</param>
        /// <param name="span">档距(m)</param>
        /// <param name="elsa">弹性模量(N/mm2)</param>
        /// <param name="StressM">控制应力(kg/mm2</param>
        /// <param name="StressA">平均应立(kg/mm2)</param>
        /// <param name="coef">温度系数(℃)</param>
        /// <param name="temp1">温度(℃)/param>
        /// <param name="temp2">温度(℃)</param>
        /// <param name="temp3">温度(℃)</param>
        /// <param name="temp4">温度(℃)</param>
        /// <returns></returns>
        public static string LiMax(double bizai1, double bizai2, double bizai3, double bizai4, string gkName1, string gkName2, string gkName3, string gkName4, double span, double elsa, double StressM, double StressA, double coef, double temp1, double temp2, double temp3, double temp4)
        {
            double[] VMax = new double[4];
            int i, LinShiJi;
            double LinShiV;

            VMax[0] = elsa / ConstVar.GraAcc * Math.Pow(span, 2) * Math.Pow(bizai1, 2) / 24 / Math.Pow(StressM, 2) - (StressM + coef * elsa / ConstVar.GraAcc * temp1);
            VMax[1] = elsa / ConstVar.GraAcc * Math.Pow(span, 2) * Math.Pow(bizai2, 2) / 24 / Math.Pow(StressM, 2) - (StressM + coef * elsa / ConstVar.GraAcc * temp2);
            VMax[2] = elsa / ConstVar.GraAcc * Math.Pow(span, 2) * Math.Pow(bizai3, 2) / 24 / Math.Pow(StressM, 2) - (StressM + coef * elsa / ConstVar.GraAcc * temp3);
            VMax[3] = elsa / ConstVar.GraAcc * Math.Pow(span, 2) * Math.Pow(bizai4, 2) / 24 / Math.Pow(StressM, 2) - (StressM + coef * elsa / ConstVar.GraAcc * temp4);

            LinShiV = VMax[0];
            LinShiJi = 0;

            for (i = 1; i <= 3; i++) {
                if (LinShiV < VMax[i]) {
                    LinShiV = VMax[i];
                    LinShiJi = i;
                }
            }

            if (LinShiJi == 0) {
                return gkName1;
            }
            else if (LinShiJi == 1) {
                return gkName2;
            }
            else if (LinShiJi == 2) {
                return gkName3;
            }
            else {
                return gkName4;
            }
        }


        /// <summary>
        /// 获取控制工况名字
        /// </summary>
        /// <param name="bzDic">比例字典</param>
        /// <param name="wkCdtList">备选工况</param>
        /// <param name="span">档距(m)</param>
        /// <param name="elas">弹性模量(N/mm2)</param>
        /// <param name="StressM">控制应力(kg/mm2)</param>
        /// <param name="StressA">平均应立(kg/mm2)</param>
        /// <param name="coef">温度系数(℃)/线性膨胀系数</param>
        /// <returns></returns>
        public static string GetCtrlWorkConditionName(Dictionary<string, BZResult> bzDic, List<ElecCalsWorkCondition> wkCdtList, double span, double elas, double StressM, double StressA, double coef) 
        {
            string wkCdtName = "";
            double maxV = double.MinValue;

            foreach(var wkCdt in wkCdtList)
            {
                var stress = wkCdt.Name == "平均气温" ? StressA : StressM;

                var tempV = elas / ConstVar.GraAcc * Math.Pow(span, 2) * Math.Pow(bzDic[wkCdt.Name].BiZai, 2) / 24 / Math.Pow(stress, 2) - 
                    (stress + coef * elas / ConstVar.GraAcc * wkCdt.Temperature);

                if(tempV > maxV)
                {
                    maxV = tempV;
                    wkCdtName = wkCdt.Name;
                }
            }

            return wkCdtName;
        }


        /// <summary>
        /// 计算应力
        /// </summary>
        /// <param name="StressM">控制工况应力(kg/mm2)</param>
        /// <param name="bizaiM">控制工况比载(kg/mm2/m)</param>
        /// <param name="tempM">控制工况温度(℃)</param>
        /// <param name="e">弹性模量(kg/mm2)</param>
        /// <param name="aa">膨胀系数(1/℃</param>
        /// <param name="span">档距</param>
        /// <param name="bizaiN">比载(kg/mm2/m)</param>
        /// <param name="tempN">温度(℃</param>
        /// <returns></returns>
        public static double StressNew(double StressM, double bizaiM, double tempM, double elas, double coef, double span, double bizaiN, double tempN)
        {
            double a, b, aD, bD;
            double dt;

            //'x^3+a*x^2+b=0
            a = Math.Pow(span, 2) * Math.Pow(bizaiM, 2) * elas / 24 / Math.Pow(StressM, 2) - StressM - coef * elas * (tempM - tempN);
            b = Math.Pow(span, 2) * Math.Pow(bizaiN, 2) * elas / 24;
            aD = Math.Pow(b, 2) / 4 - b * Math.Pow(a, 3) / 27;
            bD = b / 2 - Math.Pow(a, 3) / 27;

            if (aD >= 0) {
                return spr3(bD + Math.Pow(aD, 0.5)) + spr3(bD - Math.Pow(aD, 0.5)) - a / 3;
            }
            else {
                if (bD > 0) {
                    dt = Math.Atan(Math.Pow((-aD), 0.5) / bD);
                }
                else if (bD == 0) {
                    dt = Math.PI / 2;
                }
                else { 
                //if (bD < 0) {
                    dt = (Math.PI + Math.Atan(Math.Pow((-aD), 0.5) / bD));
                }
            }
            return (a / 3 * (2 * Math.Cos(dt / 3) - 1));
        }

        /// <summary>
        /// 垂直荷载(1e-3 kg/m/mm2)
        /// </summary>
        /// <param name="wei">单位重量(kg/km)</param>
        /// <param name="dia">导线外径(mm)</param>
        /// <param name="iceThickness">覆冰厚度(mm)</param>
        /// <returns></returns>
        public static double Weight(double wei, double dia, double iceThickness)
        {
            return wei + 2.82743334 * iceThickness * (dia + iceThickness);
        }

        /// <summary>
        /// 风荷载(1e-3 kg/m)
        /// </summary>
        /// <param name="VoL">电压</param>
        /// <param name="dia">单位重量(kg/km)</param>
        /// <param name="iceThickness">覆冰厚度(mm)</param>
        /// <param name="windSpeed">风速(m/s)</param>
        /// <param name="windSpeedBase">基本风速</param>
        /// <returns></returns>
        public static double WindPa(string VoL, double dia, double iceThickness, double windSpeed, double windSpeedBase)
        { 
            double Usc = UscValue(iceThickness,dia);
            double a = aValue(windSpeedBase);
            double BB = BBValue2(iceThickness);
            double Bc = BcValue(VoL, windSpeedBase);

            //WindPa = Bc * a * Usc * (d + 2 * b) * v ^ 2 / 16 * BB
            return Bc * a * Usc * (dia + 2 * iceThickness) * Math.Pow(windSpeed, 2) / 1.6 / ConstVar.GraAcc * BB;
        }

        /// <summary>
        /// 横向比载(1e-3 kg/m)
        /// </summary>
        /// <param name="dia">导线外径(mm)</param>
        /// <param name="iceThickness">覆冰厚度(mm)</param>
        /// <param name="windSpeed">风速(m/s)</param>
        /// <param name="windSpeedBase">基本风速</param>
        /// <param name="sec">截面积(mm2)</param>
        /// <returns></returns>
        public static double BiZaiH(double dia, double iceThickness, double windSpeed, double windSpeedBase, double sec)
        {
            double Usc = UscValue(iceThickness, dia);
            double a = aValue(windSpeedBase);
            double BB = BBValue(iceThickness);
           
            //BiZaiH = a * Usc * (d + 2 * b) * v ^ 2 / 16 * BB / S
            return a * Usc * (dia + 2 * iceThickness) * Math.Pow(windSpeed, 2) / 1.6 / ConstVar.GraAcc * BB / sec;
        }
    
        //风压系数
        public static double KWindValue(double HBase, double HWind, double KWind)
        {
            return Math.Pow(HWind/HBase, KWind * 2);
        }

        /// <summary>
        /// 串风压kg/m
        /// </summary>
        /// <param name="pieceNum">单联片数</param>
        /// <param name="lNum">联数</param>
        /// <param name="glodPieceNum">折金具片数</param>
        /// <param name="iceThickness">覆冰厚度</param>
        /// <param name="windSpeed">风速</param>
        /// <param name="windSpeedBase">基本风速</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double StringWind(double pieceNum, double lNum, double glodPieceNum, double iceThickness, double windSpeed, double windSpeedBase, out string str)
        {
            double BB = BBValue(iceThickness);

            double rlst = BB * 0.04 * (pieceNum * lNum + glodPieceNum) * Math.Pow(windSpeed, 2) / 1.6 / ConstVar.GraAcc;
            str = BB.ToString() + "*" + (0.04 * (pieceNum * lNum + glodPieceNum)).ToString() + "*" + windSpeed.ToString() + "^2/1.6/" + ConstVar.GraAcc.ToString() + "=" + rlst.ToString("0.00");
            return rlst;
        }


        /// <summary>
        /// 跳线风荷载(1e-3 kg/m)
        /// </summary>
        /// <param name="VoL">电压</param>
        /// <param name="dia">导线外径(mm)</param>
        /// <param name="iceThickness">覆冰厚度(mm)</param>
        /// <param name="windSpeed">风速(m/s)</param>
        /// <param name="windSpeedBase">基本风速</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double WindPaT(string VoL, double dia, double iceThickness, double windSpeed, double windSpeedBase, out string str)
        {
            double Usc = UscValue(iceThickness,dia);
            double a = GetA(windSpeedBase, VoL);
            double BB = BBValue(iceThickness);
            double Bc = BcValue(VoL, windSpeedBase);

            //WindPaT = Bc * a * Usc * (d + 2 * b) * v ^ 2 / 16 * BB
            double rslt =  Bc * a * Usc * (dia + 2 * iceThickness) * Math.Pow(windSpeed, 2) / 1.6 / ConstVar.GraAcc * BB;
            str = Bc.ToString() + "*" + a.ToString() + "*" + Usc.ToString() + "*" + BB.ToString() + "*(" + dia.ToString() + "+2*" + iceThickness.ToString() + ")*" + windSpeed.ToString() + "^2/1.6/" + ConstVar.GraAcc.ToString() + "=" + rslt.ToString("0.00");

            return rslt;
        }

        //开三次方
        protected static double spr3(double yuanValue)
        {
            if (yuanValue < 0)
            {
                return -Math.Pow(-yuanValue, (double)1 / 3);
            }
            else
            {
                return Math.Pow(yuanValue, (double)1 / 3);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iceThickness">冰厚</param>
        /// <param name="dia">导线外径</param>
        /// <returns></returns>
        protected static double UscValue (double iceThickness, double dia)
        {
            if (iceThickness > 0){
                return 1.2;
            }
            else{
                if (dia < 17){
                    return 1.2;
                }
                else{
                    return 1.1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windSpeedBase">基本风速</param>
        /// <returns></returns>
        protected static double aValue(double windSpeedBase)
        {
            if (windSpeedBase < 20){
                return 1;
            }
            else if (windSpeedBase >= 20 && windSpeedBase < 27){
                return 0.85;
            }
            else if (windSpeedBase >= 27 && windSpeedBase < 31.5){
                return 0.75;
            }
            else{
                //else if (VBase >= 31.5) {
                return 0.7;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windSpeedBase">基本风速</param>
        /// <param name="VoL">电压</param>
        /// <returns></returns>
        protected static double GetA(double windSpeedBase, string VoL)
        {
            if (windSpeedBase > 0)
            {
                if (VoL == "1000kV" || VoL == "±800" || VoL == "±1100kV"){
                    return 1.2;
                }
                else{
                    return 1;
                }
            }
            else{
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iceThickness">冰厚</param>
        /// <returns></returns>
        protected static double BBValue(double iceThickness)
        {
            if (iceThickness < 10 && iceThickness >= 5) {
                return 1.1;
            }
            else if (iceThickness < 15 && iceThickness >= 10){
                return 1.2;
            }
            else if (iceThickness < 20 && iceThickness >= 15){
                return 1.3;
            }
            else if (iceThickness < 30 && iceThickness >= 20){
                return 1.5;
            }
            else if (iceThickness < 50 && iceThickness >= 30){
                return 1.8;
            }
            else if (iceThickness >= 50){
                return 2;
            }
            else {
                return 1;
            }
        }

        protected static double BBValuePy(double iceThickness)
        {
            if (iceThickness <= 5 && iceThickness > 0)
            {
                return 1.1;
            }
            else if (iceThickness <= 10 && iceThickness > 5)
            {
                return 1.2;
            }
            else if (iceThickness <= 15 && iceThickness > 10)
            {
                return 1.3;
            }
            else if (iceThickness <= 20 && iceThickness > 15)
            {
                return 1.5;
            }
            else if (iceThickness <= 30 && iceThickness > 20)
            {
                return 1.8;
            }
            else if (iceThickness >= 30)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iceThickness">冰厚</param>
        /// <returns></returns>
        protected static double BBValue2(double iceThickness)
        {
            if (iceThickness < 9.99 && iceThickness >= 5){
                return 1.1;
            }
            else if (iceThickness < 14.99 && iceThickness >= 9.99){
                return 1.2;
            }
            else if (iceThickness < 19.99 && iceThickness >= 14.99){
                return 1.3;
            }
            else if (iceThickness < 29.99 && iceThickness >= 19.99){
                return 1.5;
            }
            else if (iceThickness < 49.99 && iceThickness >= 29.99){
                return 1.8;
            }
            else if (iceThickness >= 49.99){
                return 2;
            }
            else{
                return 1;
            }
        }

        //Bc的确定
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VoL">电压</param>
        /// <param name="windSpeedBase">基本风速</param>
        /// <returns></returns>
        public static double BcValue(string VoL, double windSpeedBase)
        {
            if (VoL == "500kV" || VoL == "750kV" || VoL == "1000kV" || VoL == "±500" || VoL == "±800" || VoL == "±1100kV")
            {
                if (windSpeedBase < 20){
                    return 1;
                }
                else if (windSpeedBase >= 20 && windSpeedBase < 27){
                    return 1.1;
                }
                else if (windSpeedBase >= 27 && windSpeedBase < 31.5){
                    return 1.2;
                }
                else{
                    //if( VBase >= 31.5 ){
                    return 1.3;
                }
            }
            else{
                return 1;
            }
        }

        public static double UZFunction(double H, char  CZD,  double JSH)
        {
            switch (CZD)
            {
                case 'A':
                    return Math.Round(1.379 * Math.Pow(H / JSH, 0.24), 3);
                case 'B':
                    return Math.Round(1 * Math.Pow(H / JSH, 0.32), 3);
                case 'C':
                    return Math.Round(0.544 * Math.Pow(H / JSH, 0.44), 3);
                default:
                    return Math.Round(0.262 * Math.Pow(H / JSH, 0.6), 3);
            }
        }


        public static double TerrainValue(char CZD)
        {
            switch (CZD)
            {
                case 'A':
                    return 0.12;
                case 'B':
                    return 0.16;
                case 'C':
                    return 0.22;
                default:
                    return 0.3;
            }
        }
    }
}
