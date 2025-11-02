using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command object base class with lazy service provider support.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class CommandObjectBase<T> : CommandObject<T>, IHasLazyServiceProvider
    where T : CommandObjectBase<T>
{
    /// <summary>
    /// Gets or sets the lazy service provider.
    /// </summary>
    /// <remarks>
    /// This property allows for lazy loading of services, enabling dependency injection and service resolution at runtime.
    /// Please note that this property should be set by the framework or infrastructure code that manages the lifecycle of command objects.
    /// </remarks>
    public ILazyServiceProvider LazyServiceProvider { get; set; }

    /// <summary>
    /// Gets the current user identity from the lazy service provider.
    /// </summary>
    protected virtual UserPrincipal Identity => LazyServiceProvider.GetRequiredService<UserPrincipal>();
}