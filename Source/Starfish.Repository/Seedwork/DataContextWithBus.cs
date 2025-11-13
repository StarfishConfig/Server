using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="DataContextWithBus{TContext}"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
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

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryModule).Assembly, type => type.GetCustomAttribute<DbContextAttribute>()?.ContextType == typeof(TContext));
        base.OnModelCreating(modelBuilder);
    }

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

    protected override void SetEntryValues(IEnumerable<EntityEntry> entries)
    {
        if (!AutoSetEntryValues)
        {
            return;
        }

        var user = GetCurrentUser();

        foreach (EntityEntry entry in entries)
        {
            DateTime dateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is IHasCreateTime)
                    {
                        entry.CurrentValues[nameof(IHasCreateTime.CreateTime)] = dateTime;
                    }

                    if (entry.Entity is IHasUpdateTime)
                    {
                        entry.CurrentValues[nameof(IHasUpdateTime.UpdateTime)] = dateTime;
                    }

                    if (entry.Entity is ITombstone)
                    {
                        entry.CurrentValues[nameof(ITombstone.IsDeleted)] = false;
                    }

                    if (entry.Entity is IAuditing auditing)
                    {
                        auditing.CreatedBy = user;
                        auditing.UpdatedBy = user;
                    }

                    break;
                case EntityState.Deleted:
                    if (entry.Entity is ITombstone tombstone)
                    {
                        entry.State = EntityState.Modified;
                        tombstone.IsDeleted = true;
                        tombstone.DeleteTime = dateTime;
                    }

                    break;
                case EntityState.Modified:
                    SetModifiedEntry(entry, dateTime, user);
                    break;
            }
        }
    }

    private void SetModifiedEntry(EntityEntry entry, DateTime time, string user)
    {
        if (entry.State != EntityState.Modified)
        {
            return;
        }

        switch (entry.Entity)
        {
            case IAuditing entity:
                entity.UpdatedBy = user;
                entity.UpdateTime = time;
                break;
            case IHasUpdateTime entity:
                entity.UpdateTime = time;
                break;
        }
    }

    private string GetCurrentUser()
    {
        return _request?.Context?.User?.Identity?.Name ?? "Anonymous";
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