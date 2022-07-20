using EAuction.Models.API;
using EAuction.Processor.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EAuction.Data.MessageBroker
{
    public class ProcessData : IProcessData
    {
        private ILogger _logger;

        public ProcessData(ILogger<ProcessData> logger)
        {
            _logger = logger;
        }
        public async Task Process(MessagePayload payload)
        {
            //using (var payloadMessageContext =
            //    new PayloadMessageContext(
            //        _configuration.GetConnectionString("DefaultConnection")))
            //{
            //    await payloadMessageContext.AddAsync(new MessagePayload
            //    {
            //        Id = messagePayload.Id,
            //        Message = messagePayload.Message,
            //        Created = DateTime.UtcNow
            //    });

            //    await payloadMessageContext.SaveChangesAsync();
            //}

            string messagePayload = JsonSerializer.Serialize(payload);

            _logger.LogError(messagePayload);
            await Task.CompletedTask;
        }
    }
}
