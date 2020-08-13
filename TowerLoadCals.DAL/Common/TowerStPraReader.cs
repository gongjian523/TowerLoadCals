using System.Collections.Generic;
using System.Data;
using TowerLoadCals.Mode;
using TowerLoadCals.Common;
using System.Linq;
using System.ComponentModel;
using System.Text;

namespace TowerLoadCals.DAL
{
    public class TowerStPraReader
    {
        public static List<TowerStPra> Read(string dir, string tableName)
        {
            List<TowerStPra> towerStPraList = new List<TowerStPra>();

            DataTable dataTable = DbfUtils.ReadDbf(dir, tableName);
 
            foreach (DataRow row in dataTable.Rows)
            {
                towerStPraList.Add(new TowerStPra
                {
                    VoltageLevel = row["电压等级"].ToString(),
                    TowerModel = row["杆塔型号"].ToString(),
                    PicNum = row["图号"].ToString(),
                    Height = row["呼高"].ToString(),
                    Line1Tension2 = row["直线1耐张2"].ToString(),
                    AngleMax = row["最大转角"].ToString(),
                    AllowedLH = row["允许LH"].ToString(),
                    AllowedLV = row["允许LV"].ToString(),
                    DistanceMax = row["最大档距"].ToString(),
                    AllowSwingAngle = row["允许摇摆角"].ToString(),
                    InsideSwingAngle = row["内过摇摆角"].ToString(),
                    OutsideSwingAngle = row["外过摇摆角"].ToString(),
                    HorizontalWireDsitance = row["水平线间距"].ToString(),
                    EarthWireHorizontalDsitance = row["导地水平距"].ToString(),
                    EarthWireCarrierHeight = row["地线支架高"].ToString(),
                    UpDownWireDistance = row["上下导线距"].ToString(),
                    MiddleDownWireDistance = row["中下导线距"].ToString(),
                    LineH1V2V3 = row["导平1垂2V3"].ToString(),
                    LineHProjectionDP = row["导平投影DP"].ToString(),
                    LineVProjectionDZ = row["导垂投影DZ"].ToString(),
                    FrontDistanceM = row["正面根开米"].ToString(),
                    SideDistanceM = row["侧面根开米"].ToString(),
                    SteelWeightKG = row["钢重量KG"].ToString(),
                    A3FWeightKG = row["A3F重量KG"].ToString(),
                    MNWeightKG = row["MN重量KG"].ToString(),
                    CementWeightKG = row["水泥重量KG"].ToString(),
                    BodyPrice = row["本体造价"].ToString(),
                    AngelEffectLH = row["角影响LH"].ToString(),
                    AngelEffectDistance = row["角影响档距"].ToString(),
                    LVMax = row["单侧最大LV"].ToString(),
                    LHMin = row["最小LH"].ToString(),
                    TowerKV = row["塔KV值"].ToString(),
                    Intercept = row["截距"].ToString(),
                    Slope = row["斜率"].ToString(),
                    InterceptGrowthRate = row["截距增长值"].ToString(),
                    SlopeGrowthRate = row["斜率增长值"].ToString(),
                    TowerHangStringNum = row["塔挂串个数"].ToString(),
                    TotalStringNum = row["串总个数"].ToString(),
                    VStringYN = row["V串YN"].ToString(),
                    TowerType = row["杆塔种类"].ToString(),
                    BaseFrontDistance = row["基正面根开"].ToString(),
                    BaseSideDistance = row["基侧面根开"].ToString(),
                    BaseDiagonal = row["基对角线长"].ToString(),
                    ExpectedDeviatingDistance = row["预偏距离S1"].ToString(),
                    SidelineWidth = row["横担宽度"].ToString(),
                    IsTightTower = row["是否紧凑塔"].ToString(),
                    VStringAngle = row["V串夹角"].ToString(),
                    BuriedDeepth = row["埋深"].ToString(),
                    Margin = row["裕度"].ToString(),
                    GroundBoltModel = row["地栓型号"].ToString(),
                    HungPointABC = row["挂点ABC"].ToString()
                });
            }

            return towerStPraList;
        }

        public static List<TowerStrData> Read1(string dir)
        {
            List<TowerStrData> towerStPraList = new List<TowerStrData>();

            DataTable dataTable = DbfUtils.ReadDbf(dir, "");
            TowerStrData data = null;
            foreach (DataRow row in dataTable.Rows)
            {
                data = new TowerStrData();
                data.Name = row["杆塔型号"].ToString().Split('-')[0];
                data.Type = int.Parse(row["直线1耐张2"].ToString())==1?"直线塔":"耐张塔";
                data.VoltageLevel = row["电压等级"].ToString()==""?0:double.Parse(row["电压等级"].ToString());
                data.MaxAngel = row["最大转角"].ToString() == "" ? 0 : double.Parse(row["最大转角"].ToString(),0);
                data.MinHeight = row["呼高"].ToString() == "" ? 0 : double.Parse(row["呼高"].ToString());//最小呼高
                data.MaxHeight = row["呼高"].ToString() == "" ? 0 : double.Parse(row["呼高"].ToString());//最大呼高
                data.AllowedHorSpan = row["允许LH"].ToString() == "" ? 0 : double.Parse(row["允许LH"].ToString());//设计水平档距
                data.OneSideMinHorSpan = row["最小LH"].ToString() == "" ? 0 : double.Parse(row["最小LH"].ToString());//单侧最小水平档距
                data.AllowedVerSpan = row["允许LV"].ToString() == "" ? 0 : double.Parse(row["允许LV"].ToString());//最大垂直档距
                data.OneSideMaxVerSpan = row["单侧最大LV"].ToString() == "" ? 0 : double.Parse(row["单侧最大LV"].ToString());//单侧最大垂直档距
                data.StrHeightSer = row["呼高"].ToString();//"直线塔呼高序列字符串"

                towerStPraList.Add(data);
            }

            var groups = towerStPraList.GroupBy(item => item.Name).ToList();//按塔型型号分组

             towerStPraList = new List<TowerStrData>();
            StringBuilder StrHeightSer = new StringBuilder();
            foreach (var group in groups)
            {
                data = new TowerStrData();
                data.Name = group.Key;
                data.MaxHeight = group.Select(k => k.MaxHeight).Max();
                data.MinHeight = group.Select(k => k.MinHeight).Min();

                int i = 0;
                foreach (TowerStrData item in group)
                {
                    if(i==0)
                    {
                        data.Type = item.Type;
                        data.VoltageLevel = item.VoltageLevel;
                        data.AllowedHorSpan = item.AllowedHorSpan;
                        data.OneSideMinHorSpan = item.OneSideMinHorSpan;
                        data.AllowedVerSpan = item.AllowedVerSpan;
                        data.OneSideMaxVerSpan = item.OneSideMaxVerSpan;
                    }
                    StrHeightSer.Append(item.StrHeightSer+",");
                    i++;
                }
                data.StrHeightSer = StrHeightSer.ToString().TrimEnd(',');
                towerStPraList.Add(data);
            }
            return towerStPraList;
        }

    }
}
