using EAuction.Models.API;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IProcessData
    {
        Task Process(MessagePayload payload);
    }
}
