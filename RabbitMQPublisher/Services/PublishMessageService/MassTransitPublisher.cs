using MassTransit;

namespace RabbitMQPublisher.Services.PublishMessageService
{
    public class MassTransitPublisher : IPublisher
    {
        private readonly IBus bus;

        public MassTransitPublisher(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            await bus.Publish(message, cancellationToken);
        }

        public async Task Publish<TMessage>(TMessage message, Guid correlationId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            await bus.Publish(message, context => { context.CorrelationId = correlationId; }, cancellationToken);
        }
    }
}
