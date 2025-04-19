using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using TPS.Infrastructure.Data;
using TSP.Domain.Primitives;

namespace TPS.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(
    ILogger<ProcessOutboxMessagesJob> _logger,
    ApplicationDbContext _dbContext,
    IPublisher _publisher
    ) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);


        foreach (var msg in messages)
        {
            await ProcessEventAsync(context, msg);
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task ProcessEventAsync(IJobExecutionContext context, Data.Outbox.OutboxMessage msg)
    {
        try
        {
            var domainEvent = JsonConvert
                        .DeserializeObject<DomainEvent>(msg.Content, new JsonSerializerSettings
                        {
                           TypeNameHandling = TypeNameHandling.All
                        });

            await _publisher.Publish(domainEvent, context.CancellationToken);

            msg.ProcessedOnUtc = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            msg.Error = ex.Message;
            msg.ProcessedOnUtc = DateTime.UtcNow;
            _logger.LogError(ex, "Error processing outbox message: {MessageId}", msg.Id);
        }
    }

}
