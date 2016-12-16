namespace Flep.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetMessageDto
    {
        public string Body { get; set; }

        public DateTime DateSubmitted { get; set; }
    }
}
