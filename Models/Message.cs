namespace Flep.Models
{
    using System;
    using Dto;
    using MongoDB.Bson;

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
