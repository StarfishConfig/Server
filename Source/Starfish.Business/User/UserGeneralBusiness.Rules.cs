using Nerosoft.Euonia.Business;

namespace Nerosoft.Starfish.Business;

internal partial class UserGeneralBusiness
{
    /// <summary>
    /// The username availability check rule.
    /// </summary>
    /// <param name="repository"></param>
    public class UsernameAvailabilityCheckRule : RuleBase
    {
        public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
        {
            // Only check for new user insertions
            if (context.Target is not UserGeneralBusiness target || !target.IsInsert)
            {
                return;
            }

            var exists = await target.Repository.CheckUsernameExistsAsync(target.Username, cancellationToken);
            if (exists)
            {
                context.AddErrorResult(string.Format(Resources.IDS_ERROR_USERNAME_NOT_AVAILABLE, target.Username));
            }
        }
    }

    /// <summary>
    /// The email availability check rule.
    /// </summary>
    /// <param name="repository"></param>
    public class EmailAvailabilityCheckRule : RuleBase
    {
        public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
        {
            // Only check for new user insertions
            if (context.Target is not UserGeneralBusiness target)
            {
                return;
            }

            var exists = await target.Repository.CheckEmailExistsAsync(target.Email, target.Id, cancellationToken);
            if (exists)
            {
                context.AddErrorResult(string.Format(Resources.IDS_ERROR_EMAIL_ALREADY_TAKEN, target.Email));
            }
        }
    }

    internal class PhoneAvailabilityCheckRule : RuleBase
    {
        public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
        {
            // Only check for new user insertions
            if (context.Target is not UserGeneralBusiness target)
            {
                return;
            }
            var exists = await target.Repository.CheckPhoneExistsAsync(target.Phone, target.Id, cancellationToken);
            if (exists)
            {
                context.AddErrorResult(string.Format(Resources.IDS_ERROR_PHONE_ALREADY_TAKEN, target.Phone));
            }
        }
    }
}
