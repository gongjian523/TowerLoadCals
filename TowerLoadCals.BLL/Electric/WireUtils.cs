using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class WireUtils
    {

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="WeaData"></param>
        /// <param name="CoePara">通用系数</param>
        /// <param name="WirePara">导线参数</param>
        public void UpdataPara(string WeaData, string CoePara, string WirePara)
        {

        }

        /// <summary>
        /// 计算指定档距下应力比载并存入对象中,一般采用这个接口计算
        /// </summary>
        /// <param name="span"></param>
        public void SaveYLTabel(string span)
        {
            var YLTable = CalFTable(span);
        }

        /// <summary>
        /// #计算导线比载,默认采用老规范计算，计算参数取1，新规范取2
        /// </summary>
        /// <param name="CalType"></param>
        public void CalBZ(int CalType = 1)
        {

        }


        /// <summary>
        /// 计算给定档距下各个工况的应力
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        protected string CalFTable(string span)
        {
            return "";
        }

    }
}
