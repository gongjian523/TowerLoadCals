using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public enum TowerType
    {
        LineTower,
        LineCornerTower,
        CornerTower,
        TerminalTower,
        BranchTower
    }
    
    public class TowerTypeStringConvert
    {
        static public TowerType TowerStringToType(string tower)
        {
            if (tower == "直线塔")
                return TowerType.LineTower;
            else if (tower == "直转塔")
                return TowerType.LineCornerTower;
            else if (tower == "转角塔")
                return TowerType.CornerTower;
            else if (tower == "分支塔")
                return TowerType.BranchTower;
            else
                return TowerType.TerminalTower;
        }

        static public string TowerTypeToString(TowerType type)
        {
            if (type == TowerType.LineTower)
                return "直线塔";
            else if (type == TowerType.LineCornerTower)
                return "直转塔";
            else if (type == TowerType.CornerTower)
                return "转角塔";
            else if (type == TowerType.BranchTower)
                return "分支塔";
            else
                return "终端塔";
        }
    }

    public class TowerTypeEngStringConvert
    {
        static public TowerType TowerStringToType(string tower)
        {
            if (tower == ConstVar.LineTowerStr)
                return TowerType.LineTower;
            else if (tower == ConstVar.LineCornerTowerStr)
                return TowerType.LineCornerTower;
            else if (tower == ConstVar.CornerTowerStr)
                return TowerType.CornerTower;
            else if (tower == ConstVar.BranchTowerStr)
                return TowerType.BranchTower;
            else
                return TowerType.TerminalTower;
        }

        static public string TowerTypeToString(TowerType type)
        {
            if (type == TowerType.LineTower)
                return ConstVar.LineTowerStr;
            else if (type == TowerType.LineCornerTower)
                return ConstVar.LineCornerTowerStr;
            else if (type == TowerType.CornerTower)
                return ConstVar.CornerTowerStr;
            else if (type == TowerType.BranchTower)
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
