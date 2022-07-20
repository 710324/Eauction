
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IServiceBusConsumer
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }
}
