using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public enum TowerTypeEnum
    {
        LineTower,
        LineCornerTower,
        CornerTower,
        TerminalTower,
        BranchTower
    }
    
    public class TowerTypeStringConvert
    {
        static public TowerTypeEnum TowerStringToType(string tower)
        {
            if (tower == "直线塔")
                return TowerTypeEnum.LineTower;
            else if (tower == "直转塔")
                return TowerTypeEnum.LineCornerTower;
            else if (tower == "转角塔")
                return TowerTypeEnum.CornerTower;
            else if (tower == "分支塔")
                return TowerTypeEnum.BranchTower;
            else
                return TowerTypeEnum.TerminalTower;
        }

        static public string TowerTypeToString(TowerTypeEnum type)
        {
            if (type == TowerTypeEnum.LineTower)
                return "直线塔";
            else if (type == TowerTypeEnum.LineCornerTower)
                return "直转塔";
            else if (type == TowerTypeEnum.CornerTower)
                return "转角塔";
            else if (type == TowerTypeEnum.BranchTower)
                return "分支塔";
            else
                return "终端塔";
        }
    }

    public class TowerTypeEngStringConvert
    {
        static public TowerTypeEnum TowerStringToType(string tower)
        {
            if (tower == ConstVar.LineTowerStr)
                return TowerTypeEnum.LineTower;
            else if (tower == ConstVar.LineCornerTowerStr)
                return TowerTypeEnum.LineCornerTower;
            else if (tower == ConstVar.CornerTowerStr)
                return TowerTypeEnum.CornerTower;
            else if (tower == ConstVar.BranchTowerStr)
                return TowerTypeEnum.BranchTower;
            else
                return TowerTypeEnum.TerminalTower;
        }

        static public string TowerTypeToString(TowerTypeEnum type)
        {
            if (type == TowerTypeEnum.LineTower)
                return ConstVar.LineTowerStr;
            else if (type == TowerTypeEnum.LineCornerTower)
                return ConstVar.LineCornerTowerStr;
            else if (type == TowerTypeEnum.CornerTower)
                return ConstVar.CornerTowerStr;
            else if (type == TowerTypeEnum.BranchTower)
                return ConstVar.BranchTowerStr;
            else
                return ConstVar.TerminalTowerStr;
        }
    }


    public class TowerCHENStringConvert
    {
        static public string EN2CH(string tower)
        {
            if (tower == ConstVar.LineTowerStr)
                return "直线塔";
            else if (tower == ConstVar.LineCornerTowerStr)
                return "直转塔";
            else if (tower == ConstVar.CornerTowerStr)
                return "转角塔";
            else if (tower == ConstVar.BranchTowerStr)
                return "分支塔";
            else
                return "终端塔";
        }

        static public string CH2EN(string tower)
        {
            if (tower == "直线塔")
                return ConstVar.LineTowerStr;
            else if (tower == "直转塔")
                return ConstVar.LineCornerTowerStr;
            else if (tower == "转角塔")
                return ConstVar.CornerTowerStr;
            else if (tower == "分支塔")
                return ConstVar.BranchTowerStr;
            else
                return ConstVar.TerminalTowerStr;
        }
    }

}
