namespace TowerLoadCals.Mode
{
    public class HangingPointParas
    {
        public HangingPointParas()
        {
            Points = new string[8]; 
        }

        public int Index { get; set; }

        public string WireType { get; set; }
        
        public string StringType { get; set; }

        public string Array { get; set; }

        public int Angle { get; set; }

        public string[] Points { get; set; } 
    }
}
