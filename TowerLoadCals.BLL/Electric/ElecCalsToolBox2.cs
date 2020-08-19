using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsToolBox2
    {

        //比载计算
        //BiZai(1e-3 kg/m/mm2),q单位重量（kg/km） b覆冰厚度(mm) d导线外径(mm) v风速(m/s) s截面积(mm2),VBase基本风速
        public static double BiZai(double Q, double d, double S, double b, double v, double VBase)
        {
            double Usc, a, BB;

            if (b > 0){
                Usc = 1.2;
            }
            else
            {
                if (d < 17){
                    Usc = 1.2;
                }
                else{
                    Usc = 1.1;
                }
            }

            if (VBase < 20) {
                a = 1;
            }
            else if (VBase >= 20 && VBase < 27) {
                a = 0.85;
            }
            else if (VBase >= 27 && VBase < 31.5) {
                a = 0.75;
            }
            else {
                //else if (VBase >= 31.5) {
                a = 0.7;
            }


            BB = 1;
            if (b < 10 && b >= 5) {
                BB = 1.1;
            }
            else if (b < 15 && b >= 10) {
                BB = 1.2;
            }
            else if (b < 20 && b >= 15) {
                BB = 1.3;
            }
            else if (b < 30 && b >= 20) {
                BB = 1.5;
            }
            else if (b < 50 && b >= 30) {
                BB = 1.8;
            }
            else if (b >= 50) {
                BB = 2;
            }

            double BiZai = Math.Pow(Math.Pow(Q / S + 2.82743334 * b * (d + b) / S, 2) + Math.Pow(a * Usc * (d + 2 * b) * Math.Pow(v, 2) / 1.6 / S * BB / 9.80665, 2), 0.5);
            //BiZai = ((Q / S + 2.82743334 * b * (d + b) / S) ^ 2 + (a * Usc * (d + 2 * b) * v ^ 2 / 16 / S * BB) ^ 2) ^ (1 / 2)
            return BiZai;
        }

        //g比载(kg/m/mm2)，l档距(m)，E弹性模量(N/mm2)，Stess应力(kg/mm2)，a温度系数(℃)，t温度(℃)
        public static string LiMax(double g1, double g2, double g3, double g4, string WName1, string WName2, string WName3, string WName4, double l, double e, double StressM, double StressA, double a, double T1, double T2, double T3, double T4)
        {
            double[] VMax = new double[4];
            int i, LinShiJi;
            double LinShiV;

            VMax[0] = e / 9.80665 * Math.Pow(l, 2) * Math.Pow(g1, 2) / 24 / Math.Pow(StressM, 2) - (StressM + a * e / 9.80665 * T1);
            VMax[1] = e / 9.80665 * Math.Pow(l, 2) * Math.Pow(g2, 2) / 24 / Math.Pow(StressM, 2) - (StressM + a * e / 9.80665 * T2);
            VMax[2] = e / 9.80665 * Math.Pow(l, 2) * Math.Pow(g3, 2) / 24 / Math.Pow(StressM, 2) - (StressM + a * e / 9.80665 * T3);
            VMax[3] = e / 9.80665 * Math.Pow(l, 2) * Math.Pow(g4, 2) / 24 / Math.Pow(StressM, 2) - (StressM + a * e / 9.80665 * T4);

            LinShiV = VMax[0];
            LinShiJi = 0;

            for (i = 1; i <= 3; i++) {
                if (LinShiV < VMax[i]) {
                    LinShiV = VMax[i];
                    LinShiJi = i;
                }
            }

            if (LinShiJi == 0) {
                return WName1;
            }
            else if (LinShiJi == 1) {
                return WName2;
            }
            else if (LinShiJi == 2) {
                return WName3;
            }
            else {
                return WName4;
            }
        }

        //开三次方
        public static double spr3(double yuanValue)
        {
            if (yuanValue < 0) {
                return -Math.Pow(-yuanValue, (double)1/3);
            }
            else {
                return Math.Pow(yuanValue, (double)1/3);
            }
        }


        //M是已知，N是未知,stress(kg/mm2),g(kg/mm2/m),t(℃),E(kg/mm2),aa膨胀系数(1/℃)
        public static double StressNew(double StressM, double GM, double tM, double e, double aa, double l, double gN, double tN)
        {
            double a, b, aD, bD;
            double dt;

            //'x^3+a*x^2+b=0
            a = Math.Pow(l, 2) * Math.Pow(GM, 2) * e / 24 / Math.Pow(StressM, 2) - StressM - aa * e * (tM - tN);
            b = Math.Pow(l, 2) * Math.Pow(gN, 2) * e / 24;
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
    
        //垂直荷载铁塔计算
        //Weight(1e-3 kg/m/mm2),q单位重量（kg/km） b覆冰厚度(mm) d导线外径(mm)  s截面积(mm2)
        public static double Weight(double Q, double d, double b)
        {
            return Q + 2.82743334 * b * (d + b);
        }

        //风荷载铁塔计算
        //WindP(1e-3 kg/m),q单位重量（kg/km） b覆冰厚度(mm) d导线外径(mm) v风速(m/s) s截面积(mm2),VBase基本风速
        public static double WindPa(string VoL, double d, double b, double v, double VBase)
        { 
            double a, BB, Bc;
            double Usc;

            if (b > 0) {
                Usc = 1.2;
            }
            else {
                if (d < 17) {
                    Usc = 1.2; }
                else {
                    Usc = 1.1;
                }
            }

            if( VBase< 20 ){
                a = 1;
            }
            else if( VBase >= 20 && VBase< 27 ){
                a = 0.85;
            }
            else if( VBase >= 27 && VBase< 31.5 ){
                a = 0.75;
            }
            else { 
            //else if( VBase >= 31.5 ){
                a = 0.7;
            }
            
            BB = 1;
            if( b< 9.99 && b >= 5 ){
                BB = 1.1;
            }
            else if( b< 14.99 && b >= 9.99 ){
                BB = 1.2;
            }
            else if( b< 19.99 && b >= 14.99 ){
                BB = 1.3;
            }
            else if( b< 29.99 && b >= 19.99 ){
                BB = 1.5;
            }
            else if( b< 49.99 && b >= 29.99 ){
                BB = 1.8;
            }
            else if( b >= 49.99 ){
                BB = 2;
            }

            if (VoL == "500kV" || VoL == "750kV" || VoL == "1000kV" || VoL == "±500" || VoL == "±800" || VoL == "±1100kV") {
                if (VBase < 20) {
                    Bc = 1;
                }
                else if (VBase >= 20 && VBase < 27) {
                    Bc = 1.1;
                }
                else if (VBase >= 27 && VBase < 31.5) {
                    Bc = 1.2;
                }
                else {
                    //if( VBase >= 31.5 ){
                    Bc = 1.3;
                }
            }
            else {
                Bc = 1;
            }

            return Bc * a * Usc * (d + 2 * b) * Math.Pow(v, 2) / 1.6 / 9.80665 * BB;
            //WindPa = Bc * a * Usc * (d + 2 * b) * v ^ 2 / 16 * BB
        }

        //横向比载计算
        //WindP(1e-3 kg/m),q单位重量（kg/km） b覆冰厚度(mm) d导线外径(mm) v风速(m/s) s截面积(mm2),VBase基本风速
        public static double BiZaiH(double d, double b, double v, double VBase, double S)
        { 
            double Usc, a, BB;

            if (b > 0) {
                Usc = 1.2;
            }
            else {
                if (d < 17) {
                    Usc = 1.2;
                }
                else {
                    Usc = 1.1;
                }
            }

            if (VBase < 20) {
                a = 1;
            }
            else if (VBase >= 20 && VBase < 27) {
                a = 0.85;
            }
            else if (VBase >= 27 && VBase < 31.5) {
                a = 0.75;
            }
            else {
                a = 0.7;
            }

            BB = 1;
            if (b < 10 && b >= 5) {
                BB = 1.1;
            }
            else if (b < 15 && b >= 10) {
                BB = 1.2;
            }
            else if (b < 20 && b >= 15) {
                BB = 1.3;
            }
            else if (b < 30 && b >= 20) {
                BB = 1.5;
            }
            else if (b < 50 && b >= 30) {
                BB = 1.8;
            }
            else if (b >= 50) {
                BB = 2;
            }

            return a * Usc * (d + 2 * b) * Math.Pow(v, 2) / 1.6 / 9.80665 * BB / S;
           //BiZaiH = a * Usc * (d + 2 * b) * v ^ 2 / 16 * BB / S
        }

        //Bc的确定
        public static double BcValue(string VoL, double VBase)
        { 
            if( VoL == "500kV" || VoL == "750kV" || VoL == "1000kV" || VoL == "±500" || VoL == "±800" || VoL == "±1100kV" )
            {
                if( VBase< 20 ){
                    return 1;
                }
                else if( VBase >= 20 && VBase< 27 ){
                    return 1.1;
                }
                else if( VBase >= 27 && VBase< 31.5 ){
                    return 1.2;
                }
                else {
                    //if( VBase >= 31.5 ){
                    return 1.3;
                }
            }
            else {
                return 0;
            }
        }
    
        //风压系数
        public static double KWindValue(double HBase, double HWind, double KWind)
        {
            return Math.Pow(HWind/HBase, KWind * 2);
        }

        //串风压kg/m
        //单联片数，联数，折金具片数，覆冰，风速，基本风速
        public static double StringWind(double DLPN, double LS, double MN, double b, double v, double VBase)
        {
            double BB = 1;
            if( b< 10 && b >= 5 ){
                BB = 1.1;
            }
            else if( b < 15 && b >= 10 ){
                BB = 1.2;
            }
            else if( b < 20 && b >= 15 ){
                BB = 1.3;
            }
            else if( b < 30 && b >= 20 ){
                BB = 1.5;
            }
            else if( b < 50 && b >= 30 ){
                BB = 1.8;
            }
            else if(b >= 50){
                BB = 2;
            }

            return BB * 0.04 * (DLPN * LS + MN) * Math.Pow(v, 2) / 1.6 / 9.80665;
        }

        //串风压字符串
        //单联片数，联数，折金具片数，覆冰，风速，基本风速
        public static string StringWindString(double DLPN, double LS, double MN, double b, double v, double VBase)
        { 
            double BB = 1;
            if(b< 10 && b >= 5 ){
                BB = 1.1;
            }
            else if(b< 15 && b >= 10 ){
                BB = 1.2;
            }
            else if(b< 20 && b >= 15 ){
                BB = 1.3;
            }
            else if(b< 30 && b >= 20 ){
                BB = 1.5;
            }
            else if(b< 50 && b >= 30 ){
                BB = 1.8;
            }
            else if (b >= 50){
                BB = 2;
            }
            return BB.ToString() + "*" + (0.04 * (DLPN * LS + MN)).ToString() + "*" + v.ToString() + "^2/1.6/9.80665";
        }

        //风荷载跳线计算
        //WindP(1e-3 kg/m),q单位重量（kg/km） b覆冰厚度(mm) d导线外径(mm) v风速(m/s) s截面积(mm2),VBase基本风速
        public static double WindPaT(string VoL, double d, double b, double v, double VBase)
        {
            double Usc, a, Bc;

            if (b > 0)
            {
                Usc = 1.2;
            }
            else
            {
                if (d < 17)
                {
                    Usc = 1.2;
                }
                else
                {
                    Usc = 1.1;
                }
            }

            if (VBase > 0) {
                if (VoL == "1000kV" || VoL == "±800" || VoL == "±1100kV") {
                    a = 1.2; }
                else {
                    a = 1;
                }
            }
            else {
                a = 1;
            }

            double BB = 1;
            if (b < 10 && b >= 5)
            {
                BB = 1.1;
            }
            else if (b < 15 && b >= 10)
            {
                BB = 1.2;
            }
            else if (b < 20 && b >= 15)
            {
                BB = 1.3;
            }
            else if (b < 30 && b >= 20)
            {
                BB = 1.5;
            }
            else if (b < 50 && b >= 30)
            {
                BB = 1.8;
            }
            else if (b >= 50){
                BB = 2;
            }


            if (VoL == "500kV" || VoL == "750kV" || VoL == "1000kV" || VoL == "±500" || VoL == "±800" || VoL == "±1100kV") {
                if (VBase < 20) {
                    Bc = 1;
                }
                else if (VBase >= 20 && VBase < 27) {
                    Bc = 1.1;
                }
                else if (VBase >= 27 && VBase < 31.5) {
                    Bc = 1.2;
                }
                else {
                    Bc = 1.3;
                }
            }
            else {
                Bc = 1;
            }

            return Bc * a * Usc * (d + 2 * b) * Math.Pow(v, 2) / 1.6 / 9.80665 * BB;
            //WindPaT = Bc * a * Usc * (d + 2 * b) * v ^ 2 / 16 * BB
        }


        //风荷载跳线字符串
        //WindP(1e-3 kg/m),q单位重量（kg/km） b覆冰厚度(mm) d导线外径(mm) v风速(m/s) s截面积(mm2),VBase基本风速
        public static string WindPaTString(string VoL, double d, double b, double v , double VBase)
        {
            double Usc, a, Bc;

            if (b > 0)
            {
                Usc = 1.2;
            }
            else
            {
                if (d < 17)
                {
                    Usc = 1.2;
                }
                else
                {
                    Usc = 1.1;
                }
            }

            if (VBase > 0)
            {
                if (VoL == "1000kV" || VoL == "±800" || VoL == "±1100kV")
                {
                    a = 1.2;
                }
                else
                {
                    a = 1;
                }
            }
            else
            {
                a = 1;
            }

            double BB = 1;
            if (b < 10 && b >= 5)
            {
                BB = 1.1;
            }
            else if (b < 15 && b >= 10)
            {
                BB = 1.2;
            }
            else if (b < 20 && b >= 15)
            {
                BB = 1.3;
            }
            else if (b < 30 && b >= 20)
            {
                BB = 1.5;
            }
            else if (b < 50 && b >= 30)
            {
                BB = 1.8;
            }
            else if (b >= 50)
            {
                BB = 2;
            }

            if (VoL == "500kV" || VoL == "750kV" || VoL == "1000kV" || VoL == "±500" || VoL == "±800" || VoL == "±1100kV")
            {
                if (VBase < 20){
                    Bc = 1;
                }
                else if (VBase >= 20 && VBase < 27){
                    Bc = 1.1;
                }
                else if (VBase >= 27 && VBase < 31.5){
                    Bc = 1.2;
                }
                else{
                    Bc = 1.3;
                }
            }
            else
            {
                Bc = 1;
            }
            return Bc.ToString() + "*" + a.ToString() + "*" + Usc.ToString() + "*" + BB.ToString() + "*(" + d.ToString() + "+2*" + b.ToString() + ")*" + v.ToString() + "^2/1.6/9.80665";
            //WindPaT = Bc * a * Usc * (d + 2 * b) * v ^ 2 / 16 * BB
        }
    }
}
