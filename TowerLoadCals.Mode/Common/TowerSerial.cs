namespace TowerLoadCals.Mode
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
        /// 导线安全系数
        /// </summary>
        public string ConductorSafeCoef { get; set; }

        /// <summary>
        /// 地线型号
        /// </summary>
        public string GroundWireModel { get; set; }

        /// <summary>
        /// 地线安全系数
        /// </summary>
        public string GroundWireSafeCoef { get; set; }

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
        public double FrontPreSpan { get; set; }

        /// <summary>
        /// 前侧耐张段长度
        /// </summary>
        public double FrontAccPreSpan { get; set; }

        /// <summary>
        ///后侧代表档距
        /// </summary>
        public double BackPreSpan { get; set; }

        /// <summary>
        /// 后侧耐张段长度
        /// </summary>
        public double BackAccPreSpan { get; set; }


        /// <summary>
        ///前侧垂距
        /// </summary>
        public double FrontVerticalSpan { get; set; }


        /// <summary>
        ///后侧垂距
        /// </summary>
        public double BackVerticalSpan { get; set; }

        /// <summary>
        /// 挂点高
        /// </summary>
        public double guadg { get; set; }

        /// <summary>
        /// 挂线点高差
        /// </summary>
        public double h { get; set; }


        /// <summary>
        /// 存在该塔型
        /// </summary>
        public bool ExistsTowerPattern { get; set; }

        /// <summary>
        /// 存在导线型号
        /// </summary>
        public bool ExistsConductorModel { get; set; }

        /// <summary>
        /// 存在导线型号
        /// </summary>
        public bool ExistsWireInsulationString { get; set; }

        /// <summary>
        /// 存在地线型号
        /// </summary>
        public bool ExistsGroundWireModel { get; set; }



        #region 电气计算-验算条件


        /// <summary>
        /// 导线防震锤数量
        /// </summary>
        public int WireCounterWeightNum { get; set; }

        /// <summary>
        /// 地线防震锤数量
        /// </summary>
        public int GroundCounterWeightNum { get; set; }

        /// <summary>
        /// 导线间隔棒数量
        /// </summary>
        public int WireSpacerNum { get; set; }

        /// <summary>
        /// 工程参数
        /// </summary>
        public int EngineerPar { get; set; }

        /// <summary>
        /// 前侧档内参数
        /// </summary>
        public string FrontSidePar { get; set; } = "";

        /// <summary>
        /// 后侧档内参数
        /// </summary>
        public string BackSidePar { get; set; } = "";

        /// <summary>
        /// 公共参数
        /// </summary>
        public string CommPar { get; set; } = "";

        /// <summary>
        /// 铁塔配置参数
        /// </summary>
        public string TowerPar { get; set; } = "";
        #endregion



    }
}
