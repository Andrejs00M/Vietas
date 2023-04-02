namespace webapi.Models
{
    public class Location
    {
        public int Code { get; set; }
        public int Type_Cd { get; set; }
        public string Title { get; set; }
        public int Vkur_Cd { get; set; }
        public int Vkur_Type { get; set; }
        public string Std { get; set; }
        public double Coord_X { get; set; }
        public double Coord_Y { get; set; }
        public double Dd_N { get; set; }
        public double Dd_E { get; set; }
    }
}
