namespace Flep.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Flep.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class MessagesController : Controller
    {
        private ILogger statisticsLogger;
        private IDataService dataService;

        public MessagesController(
                IDataService dataService,
                ILoggerFactory loggerFactory)
        {
            this.dataService = dataService;
            this.statisticsLogger = loggerFactory.CreateLogger(Startup.StatisticsCategory);
        }

        [HttpPost]
        public IActionResult Index([FromBody]MessageDto msgDto)
        {
            Console.WriteLine("Post to " + msgDto.Body);
            this.statisticsLogger.LogInformation("Flep post: " + msgDto.Body);
            Console.WriteLine("Wrote to log");
            var msg = msgDto.ToMessage();
            msg.DateSubmitted = DateTime.Now;
            this.dataService.Add(msg);
            return this.Ok();
        }

        [HttpGet]
        public IEnumerable<GetMessageDto> Index(double latitude, double longitude)
        {
            Console.WriteLine($"Query for {latitude} {longitude}");
            Location point = new Location()
            {
                type = "Point",
                coordinates = new double[] { longitude, latitude }
            };

            return this.dataService.Get(point)
                .Select(m => m.ToGetMessage());
        }
    }
}
