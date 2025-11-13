using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

[SuppressMessage("Style", "IDE0051")]
public sealed class DictionaryItemBusiness(IDictionaryRepository repository)
    : EditableObjectBase<DictionaryItemBusiness, DictionaryRoot>
{
    private DictionaryRoot _aggregate;
    protected override DictionaryRoot Aggregate => _aggregate;

    #region Properties
    public static readonly PropertyInfo<string> KeyProperty = RegisterProperty<string>(p => p.Key);

    public string Key
    {
        get => GetProperty(KeyProperty);
        set => SetProperty(KeyProperty, value);
    }

    public static readonly PropertyInfo<string> ValueProperty = RegisterProperty<string>(p => p.Value);

    public string Value
    {
        get => GetProperty(ValueProperty);
        set => SetProperty(ValueProperty, value);
    }

    public static readonly PropertyInfo<string> RemarkProperty = RegisterProperty<string>(p => p.Remark);

    public string Remark
    {
        get => GetProperty(RemarkProperty);
        set => SetProperty(RemarkProperty, value);
    }

    #endregion

    #region Methods
    [FactoryFetch]
    private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
    {
        _aggregate = await repository.GetAsync(id, true, [nameof(DictionaryRoot.Items)], cancellationToken);
        if (_aggregate == null)
        {
            throw new NotFoundException();
        }
    }

    [FactoryInsert]
    protected override Task InsertAsync(CancellationToken cancellationToken = default)
    {
        Aggregate.AddItem(Key, Value, Remark);
        return base.InsertAsync(cancellationToken);
    }

    [FactoryUpdate]
    protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        Aggregate.SetItem(Key, Value, Remark);
        await repository.UpdateAsync(Aggregate, true, cancellationToken);
    }

    [FactoryDelete]
    protected override async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        Aggregate.RemoveItem(Key);
        await repository.UpdateAsync(Aggregate, true, cancellationToken);
    }

    [FactoryDelete]
    private async Task DeleteAsync(long id, IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        _aggregate = await repository.GetAsync(id, true, [nameof(DictionaryRoot.Items)], cancellationToken);
        foreach (var key in keys)
        {
            Aggregate.RemoveItem(key);
        }
        await repository.UpdateAsync(Aggregate, true, cancellationToken);
    }
    #endregion
}
