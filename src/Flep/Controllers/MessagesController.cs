namespace Flep.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dto;
    using Flep.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class MessagesController : Controller
    {
        private ILogger messagingLogger;
        private IDataService dataService;

        public MessagesController(
                IDataService dataService,
                ILoggerFactory loggerFactory)
        {
            this.dataService = dataService;
            this.messagingLogger = loggerFactory.CreateLogger(Startup.MessagingCategory);
        }

        [HttpPost]
        public IActionResult Index([FromBody]PostMessageDto msgDto)
        {
            this.messagingLogger.LogInformation("Flep post: " + msgDto.Body);

            var msg = msgDto.ToMessage();
            msg.DateSubmitted = DateTime.Now;
            this.dataService.Add(msg);
            return this.Ok();
        }

        [HttpGet]
        public IEnumerable<GetMessageDto> Index(double latitude, double longitude)
        {
            var point = new Location("Point", new[] { longitude, latitude });
            return this.dataService.Get(point).Select(m => m.ToGetMessage());
        }
    }
}
