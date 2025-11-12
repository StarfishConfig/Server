using System.Reflection;
using System.Threading;
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

        foreach (EntityEntry entry in entries)
        {
            DateTime dateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is IHasCreateTime)
                    {
                        entry.CurrentValues["CreateTime"] = dateTime;
                    }

                    if (entry.Entity is IHasUpdateTime)
                    {
                        entry.CurrentValues["UpdateTime"] = dateTime;
                    }

                    if (entry.Entity is ITombstone)
                    {
                        entry.CurrentValues["IsDeleted"] = false;
                    }

                    if (entry.Entity is IAuditing auditing)
                    {
                        var userId = _request.Context.User?.Identity?.Name ?? "Anonymous";
                        auditing.CreatedBy = userId;
                        auditing.UpdatedBy = userId;
                    }

                    break;
                case EntityState.Deleted:
                    if (entry.Entity is ITombstone)
                    {
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        entry.CurrentValues["DeleteTime"] = dateTime;
                    }

                    break;
                case EntityState.Modified:
                    SetModifiedEntry(entry, dateTime);
                    break;
            }
        }
    }

    private void SetModifiedEntry(EntityEntry entry, DateTime time)
    {
        if (entry.State == EntityState.Modified)
        {
            switch (entry.Entity)
            {
                case ITombstone entity:
                    entity.IsDeleted = true;
                    entity.DeleteTime = time;
                    break;
                case IAuditing entity:
                    entity.UpdatedBy = _request.Context.User?.Identity?.Name ?? "Anonymous";
                    entity.UpdateTime = time;
                    break;
                case IHasUpdateTime entity:
                    entity.UpdateTime = time;
                    break;
            }
        }
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