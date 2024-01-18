namespace RESTAURANT.ASYNC.Broker
{
    public interface IMessageBroker
    {
        void publishMessage<T>(T message);
    }
}
