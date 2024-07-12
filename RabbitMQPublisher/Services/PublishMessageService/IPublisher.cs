namespace RabbitMQPublisher.Services.PublishMessageService
{
    public interface IPublisher
    {
        Task Publish<TMessage>(TMessage message, Guid correlationId, CancellationToken cancellationToken = default);
    }
}
