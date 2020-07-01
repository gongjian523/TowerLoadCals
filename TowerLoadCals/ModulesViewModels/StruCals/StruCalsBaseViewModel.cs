using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class StruCalsBaseViewModel: ViewModelBase, IStruCalsBaseViewModel, INotifyPropertyChanged
    {
        public string TowerName { get; set; }

        protected StruCalsParas struCalsParas;

        public virtual void Save()
        {
            ProjectUtils.GetInstance().SaveStruCalsTower(new List<string> { TowerName });
        }

        public virtual void UpDateView(string para1, string para2)
        {
            throw new NotImplementedException();
        }

        public virtual void  DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }

        public string GetTowerName()
        {
            return TowerName;
        }

        /// <summary>
        /// 结构计算中塔位的参数来源两个：
        /// 1 新增塔位，在增加新塔时，已经将数据加入到globalInfo，
        /// 2 原来已有塔位，塔位数据是逐步加载的，只有在需要查看这个塔位时，才会去配置文件中读取到
        /// 针对这两种情况，首先在globalInfo中找到对应的数据；
        /// 如果找不到，要从配置文件中将相应塔位数据读取到globalInfo中
        /// </summary>
        /// <param name="towerName"></param>
        protected virtual void InitializeData(string towerName)
        {
            TowerName = towerName;

            var globalInfo = GlobalInfo.GetInstance();

            if(globalInfo.StruCalsParas.Where(item => item.TowerName == towerName).Count() <= 0)
            {
                ProjectUtils.GetInstance().ReadStruCalsTowerParas(towerName);
            }

            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == towerName);
            if (index < 0) 
            {
                return;
            }

            struCalsParas = globalInfo.StruCalsParas[index];

        }
    }
}
