using System.Xml.Serialization;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsTowerRes
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string UpIndStrDataName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string MidIndStrDataName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string DnIndStrDataName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public int IndStrDataNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string UpJumpStrDataName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string MidJumpStrDataName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string DnJumpStrDataName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public int JumpStrDataNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string JumpName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public int JumpDevideNum { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string GrdStrDataName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public int GrdStrDataNum { get; set; }

        /// <summary>
        ///导线上拔力
        /// </summary>
        [XmlAttribute]
        public double IndUpliftForce { get; set; }

        /// <summary>
        ///一根导线上拔力
        /// </summary>
        [XmlAttribute]
        public double Grd1UpliftForce { get; set; }

        /// <summary>
        ///另一根导线上拔力
        /// </summary>
        [XmlAttribute]
        public double Grd2pliftForce { get; set; }


        /// <summary>
        ///导线串等效串长
        /// </summary>
        [XmlAttribute]
        public double RepStrIndLen { get; set; }

        /// <summary>
        ///地线串等效串长
        /// </summary>
        [XmlAttribute]
        public double RepStrGrdLen { get; set; }

        /// <summary>
        ///支撑管直径,单位mm,
        /// </summary>
        [XmlAttribute]
        public double SuTubleDia { get; set; }

        /// <summary>
        /// 单位长度重量,单位kg
        /// </summary>
        [XmlAttribute]
        public double SuTubleWei { get; set; }

        /// <summary>
        /// 跳线绝缘子串长
        /// </summary>
        [XmlAttribute]
        public double JumpStrLen { get; set; }


        public ElecCalsTowerRes()
        {

        }
    }
}
