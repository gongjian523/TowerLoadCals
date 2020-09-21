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
        public string UpJmmpStrDataName { get; set; }

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

        public ElecCalsTowerRes()
        {

        }
    }
}
