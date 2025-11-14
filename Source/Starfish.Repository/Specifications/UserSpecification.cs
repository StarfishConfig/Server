using Nerosoft.Euonia.Linq;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying User entities.
/// </summary>
internal static class UserSpecification
{
	public static Specification<User> All => new DirectSpecification<User>(t => t.Id > 0);

	/// <summary>
	/// Specification to check if User is valid (Id > 0).
	/// </summary>
	/// <returns></returns>
	public static Specification<User> Valid()
	{
		return new DirectSpecification<User>(t => t.Id > 0);
	}

	/// <summary>
	/// Specification to check if User Id equals the given id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<User> IdEquals(long id)
	{
		return new DirectSpecification<User>(t => t.Id == id);
	}

	/// <summary>
	/// Specification to check if User Id does not equal the given id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<User> IdNotEquals(long id)
	{
		return new DirectSpecification<User>(t => t.Id != id);
	}

	/// <summary>
	/// Specification to check if Username equals the given username.
	/// </summary>
	/// <param name="username"></param>
	/// <returns></returns>
	public static Specification<User> UsernameEquals(string username)
	{
		username = username.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Username == username);
	}

	/// <summary>
	/// Specification to check if Username contains the given username.
	/// </summary>
	/// <param name="username"></param>
	/// <returns></returns>
	public static Specification<User> UsernameContains(string username)
	{
		username = username.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Username.Contains(username));
	}

	/// <summary>
	/// Specification to check if Nickname contains the given nickname.
	/// </summary>
	/// <param name="nickname"></param>
	/// <returns></returns>
	public static Specification<User> NickNameContains(string nickname)
	{
		nickname = nickname.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Nickname.ToLower().Contains(nickname));
	}

	/// <summary>
	/// Specification to check if Email equals the given email.
	/// </summary>
	/// <param name="email"></param>
	/// <returns></returns>
	public static Specification<User> EmailEquals(string email)
	{
		email = email.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Email == email);
	}

	/// <summary>
	/// Specification to check if Email contains the given email.
	/// </summary>
	/// <param name="email"></param>
	/// <returns></returns>
	public static Specification<User> EmailContains(string email)
	{
		email = email.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Email.Contains(email));
	}

	/// <summary>
	/// Specification to check if Phone equals the given phone.
	/// </summary>
	/// <param name="phone"></param>
	/// <returns></returns>
	public static Specification<User> PhoneEquals(string phone)
	{
		phone = phone.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Phone == phone);
	}

	/// <summary>
	/// Specification to check if Phone contains the given phone.
	/// </summary>
	/// <param name="phone"></param>
	/// <returns></returns>
	public static Specification<User> PhoneContains(string phone)
	{
		phone = phone.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Phone.Contains(phone));
	}

	/// <summary>
	/// Specification to check if any Authority matches the given provider and value.
	/// </summary>
	/// <param name="provider"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	public static Specification<User> OpenIdEquals(string provider, string value)
	{
		provider = provider?.Trim().ToLowerInvariant() ?? throw new ArgumentNullException(nameof(provider));
		value = value.Normalize(TextCaseType.Lower);

		return new DirectSpecification<User>(x => x.Authorities.Any(t => t.Provider == provider && t.OpenId == value));
	}

	/// <summary>
	/// Specification to check if any of Username, Nickname, or Email contains the given keyword.
	/// </summary>
	/// <param name="keyword"></param>
	/// <returns></returns>
	public static Specification<User> Matches(string keyword)
	{
		ISpecification<User>[] specifications =
		[
			UsernameContains(keyword),
			NickNameContains(keyword),
			EmailContains(keyword),
			PhoneContains(keyword)
		];

		return new CompositeSpecification<User>(PredicateOperator.OrElse, specifications);
	}

	/// <summary>
	/// Specification to check if Source equals the given source.
	/// </summary>
	/// <param name="source"></param>
	/// <returns></returns>
	public static Specification<User> SourceEquals(int source)
	{
		return new DirectSpecification<User>(t => t.Source == source);
	}

	/// <summary>
	/// Specification to check if User is locked out.
	/// </summary>
	/// <returns></returns>
	public static Specification<User> IsLockedOut()
	{
		return new DirectSpecification<User>(t => t.LockoutEnd.HasValue && t.LockoutEnd.Value > DateTimeOffset.UtcNow);
	}

	/// <summary>
	/// Specification to check if User is not locked out.
	/// </summary>
	/// <returns></returns>
	public static Specification<User> IsNotLockedOut()
	{
		return new DirectSpecification<User>(t => !t.LockoutEnd.HasValue || t.LockoutEnd.Value <= DateTimeOffset.UtcNow);
	}

	/// <summary>
	/// Applies the given <see cref="UserCriteriaDto"/> to build a composite specification.
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	public static Specification<User> ApplyCriteria(UserCriteriaDto criteria)
	{
		var speficication = All;

		if (criteria != null)
		{
			if (!string.IsNullOrWhiteSpace(criteria.Keyword))
			{
				speficication &= Matches(criteria.Keyword);
			}

			if (criteria.Source.HasValue)
			{
				speficication &= SourceEquals(criteria.Source.Value);
			}

			switch (criteria.Locked)
			{
				case true:
					speficication &= IsLockedOut();
					break;
				case false:
					speficication &= IsNotLockedOut();
					break;
			}
		}

		{ }

		return speficication;
	}
}