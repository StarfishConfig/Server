using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Modularity;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Editable object base class with lazy service provider support.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EditableObjectBase<TTarget> : EditableObject<TTarget>, IHasLazyServiceProvider
    where TTarget : EditableObjectBase<TTarget>
{
    /// <summary>
    /// Gets or sets the lazy service provider.
    /// </summary>
    /// <remarks>
    /// This property allows for lazy loading of services, enabling dependency injection and service resolution at runtime.
    /// Please note that this property should be set by the framework or infrastructure code that manages the lifecycle of business objects.
    /// </remarks>
    public ILazyServiceProvider LazyServiceProvider { get; set; }

    /// <summary>
    /// Gets the current user identity from the lazy service provider.
    /// </summary>
    protected virtual UserPrincipal Identity => LazyServiceProvider.GetRequiredService<UserPrincipal>();
}

/// <summary>
/// Editable object base class with lazy service provider support.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EditableObjectBase<TTarget, TAggregate> : EditableObjectBase<TTarget>
    where TTarget : EditableObjectBase<TTarget, TAggregate>
    where TAggregate : class, IAggregateRoot
{
    /// <summary>
    /// Gets the message bus from the lazy service provider.
    /// </summary>
    protected virtual IBus Bus => LazyServiceProvider.GetRequiredService<IBus>();

    /// <summary>
    /// Gets the request context accessor from the lazy service provider.
    /// </summary>
    protected virtual IRequestContextAccessor RequestContextAccessor => LazyServiceProvider.GetRequiredService<IRequestContextAccessor>();

    /// <summary>
    /// Gets the aggregate root associated with this business object.
    /// </summary>
    protected virtual TAggregate Aggregate { get; }

    protected override async void OnSaved(TTarget newObject, Exception error, object userState)
    {
        base.OnSaved(newObject, error, userState);

        if (error == null && Aggregate is IHasDomainEvents aggregate)
        {
            var events = aggregate.GetEvents();

            if (events != null && events.Any())
            {
                await Parallel.ForEachAsync(events, async (@event, _) =>
                {
                    await Bus.PublishAsync(@event, _);
                });
            }
        }
    }
}