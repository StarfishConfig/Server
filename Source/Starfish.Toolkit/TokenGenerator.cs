using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace Nerosoft.Starfish.Toolkit;

/// <summary>
/// Utility for generating tokens programmatically
/// </summary>
public static class TokenGenerator
{
    /// <summary>
    /// Create a new token with a specified subject claim
    /// </summary>
    public static TokenGeneratorBuilder Create<TSubject>(TSubject subject)
    {
        return Create(subject, null, null);
    }

    /// <summary>
    /// Create a new token with a specified subject and name claims
    /// </summary>
    public static TokenGeneratorBuilder Create<TSubject>(TSubject subject, string name)
    {
        return Create(subject, name, null);
    }

    /// <summary>
    /// Create a new token with a specified subject, name and audience claims
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="name"></param>
    /// <param name="audience"></param>
    /// <returns></returns>
    public static TokenGeneratorBuilder Create<TSubject>(TSubject subject, string name, string audience)
    {
        var builder = new TokenGeneratorBuilder();

        switch (subject)
        {
            case null:
                break;
            case string strSubject:
                builder.AddClaimSubject(strSubject);
                break;
            case Guid guidSubject:
                builder.AddClaimSubject(guidSubject.ToString());
                break;
            default:
                builder.AddClaimSubject(subject.ToString());
                break;
        }

        if (name != null)
        {
            builder.AddClaimName(name);
        }

        if (audience != null)
        {
            builder.WithAudience(audience);
        }

        return builder;
    }
}

public class TokenGeneratorBuilder
{
    private readonly List<Claim> _claims = [];

    private DateTime IssueTime { get; set; } = DateTime.UtcNow;

    private TimeSpan ExpireTime { get; set; } = TimeSpan.FromDays(1);

    private string Audience { get; set; }

    private string Issuer { get; set; }

    private string SigningKey { get; set; }

    private string Algorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

    internal TokenGeneratorBuilder()
    {
        //generate token id
        _claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
    }

    public TokenGeneratorBuilder AddClaim(string type, string value)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!string.IsNullOrWhiteSpace(value))
        {
            _claims.Add(new Claim(type, value));
        }

        return this;
    }

    public TokenGeneratorBuilder AddClaim(string type, string value, string valueType)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!string.IsNullOrWhiteSpace(value))
        {
            _claims.Add(new Claim(type, value, valueType));
        }

        return this;
    }

    public TokenGeneratorBuilder AddScope(params string[] scopes)
    {
        foreach (var scope in scopes)
        {
            AddClaim(JwtClaimTypes.Scope, scope);
        }

        return this;
    }

    public TokenGeneratorBuilder AddAudience(string audience)
    {
        return AddClaim(JwtRegisteredClaimNames.Aud, audience);
    }

    public TokenGeneratorBuilder AddRole(params string[] roles)
    {
        if (roles?.Length > 0)
        {
            foreach (var role in roles)
            {
                AddClaim(ClaimTypes.Role, role);
            }
        }

        return this;
    }

    public TokenGeneratorBuilder AddClaimName(string name, string type = JwtRegisteredClaimNames.Name)
    {
        return AddClaim(type, name);
    }

    public TokenGeneratorBuilder AddClaimEmail(string value, string type = JwtRegisteredClaimNames.Email)
    {
        return AddClaim(type, value);
    }

    public TokenGeneratorBuilder AddClaimSubject(string value, string type = JwtRegisteredClaimNames.Sub)
    {
        return AddClaim(type, value);
    }

    public TokenGeneratorBuilder ExpiresIn(TimeSpan time)
    {
        ExpireTime = time;
        return this;
    }

    public TokenGeneratorBuilder IssuedAt(DateTime time)
    {
        // if (time != null)
        // {
        // 	AddClaim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(time.Value).ToString(), ClaimValueTypes.Integer64);
        // }

        IssueTime = time;

        return this;
    }

    public TokenGeneratorBuilder WithSigningKey(string key)
    {
        SigningKey = key;
        return this;
    }

    public TokenGeneratorBuilder WithAlgorithm(string algorithm)
    {
        Algorithm = algorithm;
        return this;
    }

    public TokenGeneratorBuilder WithAudience(string audience)
    {
        Audience = audience;
        return this;
    }

    public TokenGeneratorBuilder WithIssuer(string issuer)
    {
        Issuer = issuer;
        return this;
    }

    public string Build()
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SigningKey);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(_claims),
            Expires = IssueTime.Add(ExpireTime),
            Issuer = Issuer,
            IncludeKeyIdInHeader = true,
            Audience = Audience,
            IssuedAt = IssueTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), Algorithm)
        };

        var token = handler.CreateToken(descriptor);
        var accessToken = handler.WriteToken(token);

        return accessToken;
    }
}
