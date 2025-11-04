using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Modularity;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Editable object base class with lazy service provider support.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EditableObjectBase<T> : EditableObject<T>, IHasLazyServiceProvider
    where T : EditableObjectBase<T>
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

    /// <summary>
    /// Gets the message bus from the lazy service provider.
    /// </summary>
    protected virtual IBus Bus => LazyServiceProvider.GetRequiredService<IBus>();

    /// <summary>
    /// Gets the request context accessor from the lazy service provider.
    /// </summary>
    protected virtual IRequestContextAccessor RequestContextAccessor => LazyServiceProvider.GetRequiredService<IRequestContextAccessor>();

    protected override void OnSaved(T newObject, Exception error, object userState)
    {
        base.OnSaved(newObject, error, userState);

        if (error == null)
        {
        }
    }
}