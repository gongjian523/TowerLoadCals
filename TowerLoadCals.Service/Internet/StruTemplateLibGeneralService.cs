using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Helpers;

namespace TowerLoadCals.Service.Internet
{
   public  class StruTemplateLibGeneralService: DbContext
    {
        
        /// <summary>
     /// 查询所有信息
     /// </summary>
     /// <returns></returns>
        public IList<StruTemplateLibGeneral> GetList()
        {
            return StruTemplateLibGeneralDb.GetList();
        }   
        
        /// <summary>
             /// 查询所有信息
             /// </summary>
             /// <returns></returns>
        public bool Add (StruTemplateLibGeneral item)
        {
            return StruTemplateLibGeneralDb.Insert(item);
        }


    }
}
