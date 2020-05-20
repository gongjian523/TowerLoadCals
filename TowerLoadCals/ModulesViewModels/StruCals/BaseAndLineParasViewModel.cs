using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;
using static TowerLoadCals.DAL.TowerTemplateReader;

namespace TowerLoadCals.Modules
{
    public class BaseAndLineParasViewModel:IBaseViewModel
    {

        public String SelectedStandard { get; set; }

        protected List<String> _standards = new List<string>();
        public List<String>  Standards
        {
            get {
                return _standards;
            }
            set
            {
                _standards = value;
            }
        }

        protected FormulaParas _baseParas = new FormulaParas();
        public FormulaParas BaseParas
        {
            get
            {
                return _baseParas;
            }
            set
            {
                _baseParas = value;
            }
        }
        public StruLineParas LineParas { get; set; }

        public TowerType  Type { get; set; }

        public int Test { get; set; }

        public bool IsLineTower { get
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

        public bool IsStandard2010
        {
            get
            {
                return SelectedStandard == "GB50545-2010";
            }
        }

        public bool IsOtherParasAngleVisible
        {
            get
            {
                return Type == TowerType.LineTower;
            }
        }

        public bool IsOtherParasJumpVisible
        {
            get
            {
                return (Type == TowerType.CornerTower || Type == TowerType.TerminalTower || Type == TowerType.BranchTower);
            }
        }

        public BaseAndLineParasViewModel()
        {
            _standards.Add("GB50545-2010");
            _standards.Add("DL/T5551-2018");

            SelectedStandard = Standards[1];

            Type = TowerType.LineTower;

            BaseParas.R1Install = 1;
            Test = 4;

        }

        void IBaseViewModel.Save()
        {
            var itme = BaseParas;
            var ti = SelectedStandard;
            var aa = Test;
        }

        void IBaseViewModel.UpDateView(string para1, string para2)
        {
            throw new NotImplementedException();
        }

        void IBaseViewModel.DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
