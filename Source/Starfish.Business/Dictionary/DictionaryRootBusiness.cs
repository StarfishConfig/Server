using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// The general business of dictionary.
/// </summary>
/// <param name="repository"></param>
internal class DictionaryRootBusiness(IDictionaryRootRepository repository)
    : EditableObjectBase<DictionaryRootBusiness, DictionaryRoot>
{
    private DictionaryRoot _aggregate;
    protected override DictionaryRoot Aggregate => _aggregate;

    #region Properties

    public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);

    public long Id
    {
        get => GetProperty(IdProperty);
        set => LoadProperty(IdProperty, value);
    }

    public static readonly PropertyInfo<string> CodeProperty = RegisterProperty<string>(p => p.Code);

    public string Code
    {
        get => GetProperty(CodeProperty);
        set => SetProperty(CodeProperty, value);
    }

    public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);

    public string Name
    {
        get => GetProperty(NameProperty);
        set => SetProperty(NameProperty, value);
    }

    public static readonly PropertyInfo<string> RemarkProperty = RegisterProperty<string>(p => p.Remark);

    public string Remark
    {
        get => GetProperty(RemarkProperty);
        set => SetProperty(RemarkProperty, value);
    }

    #endregion

    #region Methods

    [FactoryCreate]
    protected override Task CreateAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    [FactoryFetch]
    private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
    {
        _aggregate = await repository.GetAsync(id, true, cancellationToken);
        if (_aggregate == null)
        {
            throw new NotFoundException($"Dictionary with id {id} not found.");
        }
        using (BypassRuleChecks)
        {
            LoadProperty(IdProperty, _aggregate.Id);
            LoadProperty(CodeProperty, _aggregate.Code);
            LoadProperty(NameProperty, _aggregate.Name);
            LoadProperty(RemarkProperty, _aggregate.Remark);
        }
    }

    [FactoryInsert]
    protected override async Task InsertAsync(CancellationToken cancellationToken = default)
    {
        _aggregate = DictionaryRoot.Create(Code, Name);
        _aggregate.SetRemark(Remark);

        await repository.InsertAsync(_aggregate, true, cancellationToken);
        LoadProperty(IdProperty, _aggregate.Id);
    }

    [FactoryUpdate]
    protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        if (!HasChangedProperties)
        {
            return;
        }

        if (ChangedProperties.Contains(CodeProperty))
        {
            _aggregate.SetCode(Code);
        }

        if (ChangedProperties.Contains(NameProperty))
        {
            _aggregate.SetName(Name);
        }

        if (ChangedProperties.Contains(RemarkProperty))
        {
            _aggregate.SetRemark(Remark);
        }

        await repository.UpdateAsync(_aggregate, true, cancellationToken);
    }

    [FactoryDelete]
    protected override async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(_aggregate, () => new DictionaryDeletedEvent(Id, Code, Name), true, cancellationToken);
    }

    #endregion
}
