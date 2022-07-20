using Azure.Messaging.ServiceBus;
using EAuction.Models.API;
using EAuction.Processor.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Tasks;

namespace EAuction.Data.MessageBroker
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {

        private readonly ILogger _logger;

        private readonly ServiceBusClient _client;

        private readonly ServiceBusSender _sender;

        public ServiceBusPublisher(IOptions<ServiceBusSettings> serviceBusSettings, ILogger<ServiceBusPublisher> logger)
        {
            _logger = logger;
            _client = new ServiceBusClient(serviceBusSettings.Value.ConnectionString);
            _sender = _client.CreateSender(serviceBusSettings.Value.QueueName);
        }


        public async Task Publish(MessagePayload payload)
        {
            // create a batch 
            using (ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync())
            {
                string messagePayload = JsonSerializer.Serialize(payload);

                if (!messageBatch.TryAddMessage(new ServiceBusMessage(messagePayload)))
                {
                    _logger.LogError($"The message {messagePayload} is too large to fit in the batch.");
                }

                try
                {
                    await _sender.SendMessagesAsync(messageBatch);
                }
                finally
                {
                    await _sender.DisposeAsync();
                    await _client.DisposeAsync();
                }
            }
        }
    }
}
