using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQPublisher.Models;
using RabbitMQPublisher.Services.PublishMessageService;

namespace RabbitMQPublisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> logger;
        private readonly IPublisher publisher;
        public MessageController(ILogger<MessageController> logger, IPublisher publisher)
        {
            this.logger = logger;
            this.publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomMessage message)
        {
            Guid correlationId = Guid.NewGuid();
            logger.LogInformation("Request object : {message}  ", JsonConvert.SerializeObject(message));

            await publisher.Publish(message, correlationId);
            return Ok();
        }
    }
}
