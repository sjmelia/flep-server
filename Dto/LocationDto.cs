namespace Flep.Dto
{
    using Models;

    public class LocationDto
    {
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public Location ToLocation()
        {
            return new Location("Point")
            {
                type = "Point",
                coordinates = new double[]
                {
                    double.Parse(this.Longitude),
                    double.Parse(this.Latitude)
                }
            };
        }
    }
}
