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
        public const string Identifier = "id";
        public const string Username = "username";
        public const string Email = "email";
        public const string Phone = "phone";
        public const string Password = "password";
        public const string Github = "github";
        public const string Google = "google";
        public const string Facebook = "facebook";
        public const string Microsoft = "microsoft";
        public const string RefreshToken = "refresh_token";
        public const string Wechat = "wechat";
        public const string Weibo = "weibo";
        public const string Apple = "apple";
        public const string Twitter = "twitter";
        public const string LinkedIn = "linkedin";
        public const string QQ = "qq";
    }

    public class TokenType
    {
        public const string Bearer = nameof(Bearer);
    }

    public class Role
    {
        /// <summary>
        /// Super user role constant.
        /// </summary>
        public const string SuperUser = "SU";

        /// <summary>
        /// System administrator role constant.
        /// </summary>
        public const string SystemAdmin = "AD";

        /// <summary>
        /// Helpdesk role constant.
        /// </summary>
        public const string Helpdesk = "HD";

        /// <summary>
        /// Normal user role constant.
        /// </summary>
        public const string NormalUser = "US";

        /// <summary>
        /// Combined roles for system support.
        /// </summary>
        public const string Support = "SU,AD,HD";

        /// <summary>
        /// Combined roles for administrators.
        /// </summary>
        public const string Admin = "SU,AD";
    }
}