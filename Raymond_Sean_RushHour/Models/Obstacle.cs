namespace Raymond_Sean_RushHour.Models
{
    // Obstacle (enemy cars) model
    public class Obstacle
    {
        public int Lane { get; set; }
        public double YPosition { get; set; }
        public string Color { get; set; }
        public string Type { get; set; } // "car", "truck", etc.

        public Obstacle(int lane, string color = "#4ECDC4", string type = "car")
        {
            Lane = lane;
            YPosition = -100;
            Color = color;
            Type = type;
        }
    }
}
