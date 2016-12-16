namespace Flep.Dto
{
    using Models;

    public class PostMessageDto
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
}
