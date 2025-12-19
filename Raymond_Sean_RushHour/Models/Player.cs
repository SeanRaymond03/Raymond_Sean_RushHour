namespace Raymond_Sean_RushHour.Models
{
    // Player vehicle model
    public class Player
    {
        public int Lane { get; set; } // 0 = left, 1 = middle, 2 = right
        public string VehicleColor { get; set; }
        public string VehicleModel { get; set; }

        public Player()
        {
            Lane = 1; // Start in middle lane
            VehicleColor = "#FF6B6B"; // Default red
            VehicleModel = "car"; // Default model
        }
    }
}
