using DevExpress.Mvvm;

namespace TowerLoadCals.Mode
{
    public class FormulaParas : BindableBase
    //public class FormulaParas
    {
        protected string _selectedStandard = "GB50545-2010";
        public string SelectedStandard
        {
            get
            {
                return _selectedStandard;
            }
            set
            {
                _selectedStandard = value;
                RaisePropertyChanged("SelectedStandard");
            }
        }

        /// <summary>
        /// 荷载比例
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 BL1
        /// </summary>
        public float LoadRatio { get; set; }


        /// <summary>
        /// 结构重要性系数_安装     
        /// DL/T5551-2018 
        /// 旧名字 r1
        /// </summary>
        //protected float _riInstall;
        public float R1Install { get; set; }


        /// <summary>
        /// 结构重要性系数_其他     
        /// DL/T5551-2018 
        /// 旧名字 r0
        /// </summary>
        public float R0Normal { get; set; }


        /// <summary>
        /// 永久荷载分项系数-不利
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Dload_b
        /// </summary>
        public float RGBad { get; set; }

        /// <summary>
        /// 永久荷载分项系数-有利
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Dload_g
        /// </summary>
        public float RGGood { get; set; }

        /// <summary>
        /// 荷载分项系数-可变
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vload
        /// </summary>
        public float RQ { get; set; }


        /// <summary>
        /// 永久荷载分项系数-倾覆
        /// 旧名字 Dload_qh
        /// DL/T5551-2018
        /// </summary>
        public float RGOverturn { get; set; }


        /// <summary>
        /// 偶然荷载分项系数
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vload_A
        /// </summary>
        public float RA
        {
            get
            {
                return SelectedStandard == "GB50545-2010" ? RQ : 1;
            }
        }

        /// <summary>
        /// 可变荷载组合系数_运行 GB50545-2010
        /// 可变荷载组合系数_大风 DL/T5551-2018
        /// 旧名字 Vcb_N
        /// </summary>
        public float VcFNormal { get; set; }

        /// <summary>
        /// 可变荷载组合系数_安装
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_L
        /// </summary>
        public float VcFInstall { get; set; }

        /// <summary>
        /// 可变荷载组合系数_断线
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_B
        /// </summary>
        public float VcFBroken { get; set; }

        /// <summary>
        /// 可变荷载组合系数_不均匀冰
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_U
        /// </summary>
        public float VcFUnevenIce { get; set; }

        /// <summary>
        /// 可变荷载组合系数_验算     
        /// GB50545-2010 
        /// 旧名字 Vcb_Y
        /// </summary>
        protected float _vcFCheck;
        public float VcFCheck
        {
            get
            {
                return SelectedStandard == "GB50545-2010" ? _vcFCheck : 1;
            }
            set
            {
                _vcFCheck = value;
            }
        }

        /// <summary>
        /// 可变荷载组合系数_覆冰
        /// DL/T5551-2018
        /// 旧名字 Vcb_I
        /// </summary>
        public float VcFIce { get; set; }

        /// <summary>
        /// 可变荷载组合系数_低温
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_D
        /// </summary>
        protected float _vcFCold;
        public float VcFCold
        {
            get
            {
                return SelectedStandard == "GB50545-2010" ? VcFNormal : _vcFCold;
            }
            set
            {
                _vcFCold = value;
            }
        }

        /// <summary>
        /// 有利永久荷载分项系数_验算     
        /// GB50545-2010 DL/T5551-2018 
        /// 旧名字 Dload_Y1
        /// </summary>
        public float RGCheck01
        {
            get
            {
                return SelectedStandard == "GB50545-2010" ? RGBad : 1;
            }
        }


        /// <summary>
        /// 有利永久荷载分项系数_验算     
        /// GB50545-2010 DL/T5551-2018 
        /// 旧名字 Dload_Y2
        /// </summary>
        public float RGCheck02
        {
            get
            {
                return SelectedStandard == "GB50545-2010" ? RGGood : 1;
            }
        }


        /// <summary>
        ///  大风线条风压调整系数         
        /// 旧名字 Beta1
        /// </summary>
        public float WindAdjustFactor { get; set; }

        /// <summary>
        /// 其他情况线条风压调整系数         
        /// 旧名字 Beta2
        /// </summary>
        public float OtherWindAdjustFactor { get; set; }

        /// <summary>
        /// 动力系数         
        /// 旧名字 DX
        /// </summary>
        public float DynamicCoef { get; set; }

        /// <summary>
        /// 锚线风荷系数         
        /// 旧名字 cofw
        /// </summary>
        public float AnchorWindCoef { get; set; }

        /// <summary>
        /// 锚线垂荷系数         
        /// 旧名字 cofg
        /// </summary>
        public float AnchorGravityCoef { get; set; }

        /// <summary>
        /// 锚角         
        /// 旧名字 maojiao
        /// </summary>
        public float AnchorAngle { get; set; }


        /// <summary>
        /// 跳线吊装系数            
        /// 旧名字 tdx
        /// </summary>
        public float LiftCoefJumper { get; set; }   //cd修改


        /// <summary>
        /// 临时拉线对地夹角            
        /// 旧名字 dangle
        /// </summary>
        public float TempStayWireAngle { get; set; }

        /// <summary>
        /// 牵引角度           
        /// 旧名字 qangle
        /// </summary>
        public float TractionAgnle { get; set; }

        public TowerType Type { get; set; }


        public bool IsLineTower
        {
            get
            {
                return Type == TowerType.LineTower;
            }
        }

        public bool IsCornerTower
        {
            get
            {
                return Type == TowerType.CornerTower;
            }
        }

        public bool IsLineCornerTower
        {
            get
            {
                return Type == TowerType.LineCornerTower;
            }
        }

        public bool IsBranchTower
        {
            get
            {
                return Type == TowerType.BranchTower;
            }
        }

        public bool IsTerminalTower
        {
            get
            {
                return Type == TowerType.TerminalTower;
            }
        }

        public bool IsOtherParasAngleVisible
        {
            get
            {
                return Type == TowerType.LineTower;
            }
        }

        public bool IsTensionTower
        {
            get
            {
                return (Type == TowerType.CornerTower || Type == TowerType.TerminalTower || Type == TowerType.BranchTower);
            }
        }

        public bool IsMethod1Selected { get; set; }
    }
}
