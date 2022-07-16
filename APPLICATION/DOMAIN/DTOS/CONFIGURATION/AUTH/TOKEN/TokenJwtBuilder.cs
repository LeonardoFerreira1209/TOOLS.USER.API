using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;

/// <summary>
/// Classe responsável por fazer as operações com o token.
/// </summary>
public class TokenJwtBuilder
{
    private SecurityKey securityKey = null;

    private string subject, issuer, audience, username = String.Empty;

    private List<Claim> claims = new(); private List<string> roles = new();

    private int expiryInMinutes = 10;

    /// <summary>
    /// Método que adiciona o username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddUsername(string username)
    {
        this.username = username;

        return this;
    }

    /// <summary>
    /// Método que adiciona o securotyKey.
    /// </summary>
    /// <param name="securityKey"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddSecurityKey(SecurityKey securityKey)
    {
        this.securityKey = securityKey;

        return this;
    }

    /// <summary>
    /// Método que adiciona o subject.
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddSubject(string subject)
    {
        this.subject = subject;

        return this;
    }

    /// <summary>
    /// Método que adiciona o issuer.
    /// </summary>
    /// <param name="issuer"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddIssuer(string issuer)
    {
        this.issuer = issuer;

        return this;
    }

    /// <summary>
    /// Método que adiciona o audience.
    /// </summary>
    /// <param name="audience"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddAudience(string audience)
    {
        this.audience = audience;

        return this;
    }

    /// <summary>
    /// Método que adicona uma role.
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddRole(string role)
    {
        this.roles.Add(role);

        return this;
    }

    /// <summary>
    /// Método que adiciona várias roles.
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddRoles(List<string> roles)
    {
        this.roles.AddRange(roles);

        return this;
    }

    /// <summary>
    /// Método que adiciona uma claim.
    /// </summary>
    /// <param name="claim"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddClaim(Claim claim)
    {
        this.claims.Add(claim);

        return this;
    }

    /// <summary>
    /// Método que adiciona várias claims.
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddClaims(List<Claim> claims)
    {
        this.claims.AddRange(claims);

        return this;
    }

    /// <summary>
    /// Método que adiciona o tempo de expiração.
    /// </summary>
    /// <param name="expiryInMinutes"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddExpiry(int expiryInMinutes)
    {
        this.expiryInMinutes = expiryInMinutes;

        return this;
    }

    /// <summary>
    /// Método que verifica se os dados estão validos.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private void EnsureArguments()
    {
        if (this.securityKey == null) throw new ArgumentNullException("Security Key");

        if (String.IsNullOrEmpty(this.subject)) throw new ArgumentNullException("Subject");

        if (String.IsNullOrEmpty(this.issuer)) throw new ArgumentNullException("Issuer");

        if (String.IsNullOrEmpty(this.audience)) throw new ArgumentNullException("Audience");
    }

    /// <summary>
    /// Método que cria e retorna o token.
    /// </summary>
    /// <returns></returns>
    public TokenJWT Builder()
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(TokenJwtBuilder)} - METHOD {nameof(Builder)}\n");

        try
        {
            // Verifica os dados.
            EnsureArguments();

            // Adiciona as claims a uma lista.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, this.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, "Bearer")

            }.Union(this.claims);

            // Gera o token com os dados passados.
            var token = new JwtSecurityToken(
                issuer: this.issuer,
                audience: this.audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(this.expiryInMinutes),
                signingCredentials: new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256)
            );

            // retornando o token.
            return new TokenJWT(token);
        }
        catch (Exception exception)
        {
            Log.Error("[LOG ERROR]", exception, exception.Message);

            throw new Exception(exception.Message);
        }
    }
}

