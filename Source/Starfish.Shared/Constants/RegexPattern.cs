namespace Nerosoft.Starfish.Shared;

public static class RegexPattern
{
	public const string Secret = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,32}$";
	public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\x00-\xff]{8,32}$";
}