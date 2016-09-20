namespace Flep.Models
{
    using System;
    using MongoDB.Bson;

    public class Location
    {
        public string type { get; set; }

        public double[] coordinates { get; set; }
    }

    public class LocationDto
    {
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public Location ToLocation()
        {
            return new Location()
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

    public class MessageDto
    {
        public LocationDto Location { get; set; }

        public string Body { get; set; }

        public Message ToMessage()
        {
            return new Message()
            {
                Location = this.Location.ToLocation(),
                Body = this.Body
            };
        }
    }

    public class GetMessageDto
    {
        public string Body { get; set; }

        public DateTime DateSubmitted { get; set; }
    }

    public class Message
    {
        public ObjectId Id { get; set; }

        public Location Location { get; set; }

        public string Body { get; set; }

        public DateTime DateSubmitted { get; set; }

        public GetMessageDto ToGetMessage()
        {
            return new GetMessageDto()
            {
                Body = this.Body,
                DateSubmitted = this.DateSubmitted
            };
        }
    }
}
