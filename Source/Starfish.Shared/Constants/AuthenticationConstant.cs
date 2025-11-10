namespace Nerosoft.Starfish.Shared;

/// <summary>
/// Defines the constants for authentication.
/// </summary>
public class AuthenticationConstant
{
    /// <summary>
    /// 
    /// </summary>
    public class Provider
    {
        public const string Username = "username";
        public const string Email = "email";
        public const string Phone = "phone";
        public const string Password = "password";
        public const string Github = "github";
        public const string Google = "google";
        public const string Facebook = "facebook";
        public const string Microsoft = "microsoft";
        public const string RefreshToken = "refresh_token";
    }

    public class TokenType
    {
        public const string Bearer = nameof(Bearer);
    }
}
