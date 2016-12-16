namespace Flep.Test
{
    using System;
    using Flep.Controllers;
    using Xunit;
    using Microsoft.Extensions.Logging;

    public class MessagesControllerTests
    {
        private readonly MessagesController _sut; 
         public MessagesControllerTests() 
         {
            var mockDataService = new MockDataService();
            ILoggerFactory loggerFactory = new LoggerFactory();
            _sut = new MessagesController(mockDataService, loggerFactory);
         } 
        
        [Theory]
        [InlineData(-1d, -1d)]
        [InlineData(0d, 0d)]
        [InlineData(1d, 1d)]
        public void IsNotNullForGivenLatLong(double latitude, double longitude)
        {
            var result = _sut.Index(latitude, longitude);

            Assert.NotNull(result);
        } 
    }
}