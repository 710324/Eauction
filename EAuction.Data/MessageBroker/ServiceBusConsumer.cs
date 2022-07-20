using Azure.Messaging.ServiceBus;
using EAuction.Models.API;
using EAuction.Processor.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace EAuction.Data.MessageBroker
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly ILogger _logger;

        private readonly IProcessData _processData;

        private readonly ServiceBusClient _client;

        private readonly ServiceBusSender _sender;

        private ServiceBusProcessor _processor;

        private readonly int _numOfMessages;

        public ServiceBusConsumer(IProcessData processData, IOptions<ServiceBusSettings> serviceBusSettings, ILogger<ServiceBusConsumer> logger)
        {
            _logger = logger;
            _processData = processData;
            _client = new ServiceBusClient(serviceBusSettings.Value.ConnectionString);
            _sender = _client.CreateSender(serviceBusSettings.Value.QueueName);

            ConfigProcessor(serviceBusSettings.Value.QueueName);

            _numOfMessages = serviceBusSettings.Value.NumOfMessages;
        }

        private void ConfigProcessor(string queueName)
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(queueName, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            try
            {
                await _processor.StartProcessingAsync().ConfigureAwait(false);
            }
            finally
            {
                await _processor.DisposeAsync();
                await _client.DisposeAsync();
            }
        }

        // handle received messages
        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<MessagePayload>();
            await _processData.Process(myPayload).ConfigureAwait(false);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        // handle any errors when receiving messages
        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            _logger.LogDebug(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseQueueAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
