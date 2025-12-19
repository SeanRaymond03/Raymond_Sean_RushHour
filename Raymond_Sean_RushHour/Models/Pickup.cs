namespace Raymond_Sean_RushHour.Models
{
    // Collectible items (coins, fuel, boosts)
    public class Pickup
    {
        public int Lane { get; set; }
        public double YPosition { get; set; }
        public string Type { get; set; } // "coin", "fuel", "boost"

        public Pickup(int lane, string type = "coin")
        {
            Lane = lane;
            YPosition = -100;
            Type = type;
        }
    }
}
