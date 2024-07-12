using MassTransit;
using Newtonsoft.Json;
using RabbitMQConsumer.Models;

namespace RabbitMQConsumer.Consumers
{
    public class MessageConsumer : IConsumer<CustomMessage>
    {
        private readonly ILogger<MessageConsumer> _logger;
        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CustomMessage> context)
        {
            try
            {
                _logger.LogDebug("Received message: " + JsonConvert.SerializeObject(context.Message), null);

                // Process the message
                //
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error while processing " + JsonConvert.SerializeObject(context.Message), ex);
                throw;
            }
        }
    }
}
