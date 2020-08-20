﻿namespace TowerLoadCals.Mode
{
    public class TowerSerial
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 是否需要验算
        /// </summary>
        public bool IsChecking { get; set; }

        /// <summary>
        /// 气象条件
        /// </summary>
        public string WeatherCondition { get; set; }

        /// <summary>
        /// 塔名
        /// </summary>
        public string TowerName { get; set; }

        /// <summary>
        /// 塔类型:直线塔1转角塔2门架3
        /// </summary>
        public int TowerType { get; set; }


        /// <summary>
        /// 塔类型:直线塔1转角塔2门架3
        /// </summary>
        public string TowerTypeName { get; set; }

        /// <summary>
        /// 累距
        /// </summary>
        public double AccumulativeDistance { get; set; }

        /// <summary>
        /// 挂线点与地面距离
        /// </summary>
        public double PointAndGround_Distance { get; set; }

        /// <summary>
        /// 导线K值 
        /// </summary>
        public double KValueOfConductor { get; set; }

        /// <summary>
        /// 偏差距离  
        /// </summary>
        public double DeviationDistance { get; set; }

        /// <summary>
        /// 塔脚高程  
        /// </summary>
        public double TowerFootElevation { get; set; }

        /// <summary>
        /// 定位桩名  
        /// </summary>
        public string NameOfPositioningPile { get; set; }

        /// <summary>
        /// 杆塔型式    
        /// </summary>
        public string TowerPattern { get; set; }

        /// <summary>
        /// 呼称高    
        /// </summary>
        public double CallItHigh { get; set; }

        /// <summary>
        /// 绝缘串型号    
        /// </summary>
        public string InsulationStringModel { get; set; }

        /// <summary>
        /// 绝缘串片数  
        /// </summary>
        public string InsulationStringSlices { get; set; }

        /// <summary>
        /// 绝缘串数量    
        /// </summary>
        public double InsulationStringNumber { get; set; }

        /// <summary>
        /// 绝缘串长    
        /// </summary>
        public double InsulationStringLength { get; set; }

        /// <summary>
        /// 基面下降    
        /// </summary>
        public double BaseLevelDescent { get; set; }

        /// <summary>
        /// 地线K值    
        /// </summary>
        public double GroundWireKValue { get; set; }

        /// <summary>
        /// 后视档导线防振锤安装距离    
        /// </summary>
        public double BackWireDistance { get; set; }

        /// <summary>
        /// 前视档导线防振锤安装距离    
        /// </summary>
        public double FrontWireDistance { get; set; }

        /// <summary>
        /// 地线金具型号    
        /// </summary>
        public string ModelOfGroundWireFittings { get; set; }

        /// <summary>
        /// 地线金具数量    
        /// </summary>
        public double QuantityOfGroundWireFittings { get; set; }

        /// <summary>
        /// 后视档地线防振锤安装距离    
        /// </summary>
        public double BackGroundDistance { get; set; }

        /// <summary>
        /// 前视档导线防振锤安装距离    
        /// </summary>
        public double FrontGroundDistance { get; set; }

        /// <summary>
        /// 转角(度 分)   源文件值
        /// </summary>
        public double TurningAngle { get; set; }

        /// <summary>
        /// 转角(度 分)   转换值  
        /// </summary>
        public string TurningAngleStr { get; set; }

        /// <summary>
        /// 垂直档距
        /// </summary>
        public double VerticalSpan { get; set; }

        /// <summary>
        /// 切地线安全距离
        /// </summary>
        public double SafetyDistanceOfCutWire { get; set; }

        /// <summary>
        /// 导线安全系数K值
        /// </summary>
        public double SafetyFactorKConductor { get; set; }

        /// <summary>
        /// 地线安全系数K值
        /// </summary>
        public double SafetyFactorKValueOfGroundWire { get; set; }

        /// <summary>
        ///  单串/双串
        /// </summary>
        public double SingleORDoubleString { get; set; }

        /// <summary>
        ///  已挂重锤总重量
        /// </summary>
        public double TotalWeightOfHungWeightHammer { get; set; }

        /// <summary>
        ///  后视档内导线应力
        /// </summary>
        public double WireStressInRearSight { get; set; }

        /// <summary>
        ///  耐张绝缘子串是否倒挂
        /// </summary>
        public bool tensionInsulatorStringIsHangingUpsideDown { get; set; }

        /// <summary>
        ///  平面图画法(邢台网办)
        /// </summary>
        public string PlanDrawing { get; set; }

        /// <summary>
        ///  显示: 上导线1 下导线2 上下导线3
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 地线根数
        /// </summary>
        public int NumberOfGroundWires { get; set; }

        /// <summary>
        /// 导线型号
        /// </summary>
        public string ConductorModel { get; set; }

        /// <summary>
        /// 地线型号
        /// </summary>
        public string GroundWireModel { get; set; }


        /// <summary>
        /// 地线串型号
        /// </summary>
        public string GroundWireInsulationStringModel { get; set; }


        /// <summary>
        /// 地线串数量
        /// </summary>
        public string GroundWireInsulationStringNumber { get; set; }

        /// <summary>
        /// 钢重量KG
        /// </summary>
        public string SteelWeight { get; set; }

        /// <summary>
        /// 导地水平距
        /// </summary>
        public double GroundHorizontalDistance { get; set; }

        /// <summary>
        /// 上下导线距
        /// </summary>
        public double DistanceBetweenUpperAndLowerConductors { get; set; }

        /// <summary>
        /// 中下导线距
        /// </summary>
        public double MiddleAndLowerConductorDistance { get; set; }

        /// <summary>
        /// 地线支架高
        /// </summary>
        public double HeightOfGroundWireSupport { get; set; }



        /// <summary>
        /// 档距
        /// </summary>
        public double Span { get; set; }

        /// <summary>
        /// 水平档距:
        /// 后侧档距 = 自己的累距-前一个塔的累距
        /// 水平档距 = (前侧档距 + 后侧档距)/2
        /// </summary>
        public double HorizontalSpan { get; set; }

        /// <summary>
        ///前侧代表档距
        /// </summary>
        public double FrontSpan { get; set; }

        /// <summary>
        ///后侧代表档距
        /// </summary>
        public double BackSpan { get; set; }

    }
}
