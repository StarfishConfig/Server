using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

internal class DictionaryCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
    : CommandHandlerBase(unitOfWork, factory),
    IHandler<DictionaryRootCreateCommand>,
    IHandler<DictionaryRootUpdateCommand>,
    IHandler<DictionaryRootDeleteCommand>
{
    public Task HandleAsync(DictionaryRootCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.CreateAsync<DictionaryGeneralBusiness>(cancellationToken);
            business.Code = message.Code;
            business.Name = message.Name;
            business.Remark = message.Remark;
            business.MarkAsInsert();
            await business.SaveAsync(false, cancellationToken);
        }, cancellationToken);
    }

    public Task HandleAsync(DictionaryRootUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<DictionaryGeneralBusiness>(message.RootId, cancellationToken);
            business.Code = message.Code;
            business.Name = message.Name;
            business.Remark = message.Remark;
            business.MarkAsUpdate();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    public Task HandleAsync(DictionaryRootDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<DictionaryGeneralBusiness>(message.RootId, cancellationToken);
            business.MarkAsDelete();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }
}
