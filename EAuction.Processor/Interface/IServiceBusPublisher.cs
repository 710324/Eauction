using EAuction.Models.API;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IServiceBusPublisher
    {
        Task Publish(MessagePayload payload);
    }
}
