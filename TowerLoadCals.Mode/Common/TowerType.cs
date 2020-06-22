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


}
