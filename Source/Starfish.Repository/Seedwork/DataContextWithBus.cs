using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;
using Nerosoft.Euonia.Repository.EfCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Base data context with bus and request context support.
/// </summary>
internal abstract class DataContextWithBus<TContext> : DataContextBase<TContext>
    where TContext : DbContext, IRepositoryContext
{
    private readonly IBus _bus;
    private readonly IRequestContextAccessor _request;

    protected DataContextWithBus(DbContextOptions<TContext> options, ILazyServiceProvider provider)
        : base(options)
    {
        _bus = provider.GetService<IBus>();
        _request = provider.GetService<IRequestContextAccessor>();
    }

    /// <inheritdoc/>
    protected override bool AutoSetEntryValues => true;

    /// <summary>
    /// Gets the DateTimeKind used for date and time values.
    /// </summary>
    protected override DateTimeKind DateTimeKind => DateTimeKind.Utc;

    /// <inheritdoc />
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        if (_bus != null)
        {
            var events = GetTrackedEvents();

            if (result > 0 && events.Count > 0)
            {
                var options = new PublishOptions
                {
                    RequestTraceId = _request?.Context?.TraceIdentifier
                };
                foreach (var @event in events)
                {
                    await _bus.PublishAsync(@event, options, null, cancellationToken);
                }
            }
        }

        {
        }

        return result;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTime>()
                            .HaveConversion<UniversalTimeConverter>();
        configurationBuilder.Properties<DateTime?>()
                            .HaveConversion<UniversalTimeConverter>();
    }

    private List<DomainEvent> GetTrackedEvents()
    {
        var entries = ChangeTracker.Entries<IHasDomainEvents>();

        var events = new List<DomainEvent>();

        foreach (var entry in entries)
        {
            var aggregate = entry.Entity;

            aggregate.AttachToEvents();
            events.AddRange(aggregate.GetEvents());
            aggregate.ClearEvents();
        }

        return events;
    }
}
