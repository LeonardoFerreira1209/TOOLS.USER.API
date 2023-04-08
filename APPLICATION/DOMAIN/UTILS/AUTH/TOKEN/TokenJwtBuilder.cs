using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENTITY.USER;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APPLICATION.DOMAIN.UTILS.AUTH.TOKEN;

/// <summary>
/// Classe responsável por fazer as operações com o token.
/// </summary>
[ExcludeFromCodeCoverage]
public class TokenJwtBuilder
{
    private SecurityKey securityKey = null;

    private string subject, issuer, audience, username;

    private readonly List<Claim> claims = new(); private readonly List<Claim> roles = new();

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
    public TokenJwtBuilder AddRole(Claim role)
    {
        roles.Add(role);

        return this;
    }

    /// <summary>
    /// Método que adiciona várias roles.
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    public TokenJwtBuilder AddRoles(List<Claim> roles)
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
        claims.Add(claim);

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
    private (bool success, List<DadosNotificacao> messages) EnsureArguments()
    {
        var notifications = new List<DadosNotificacao>();

        if (securityKey == null) notifications.Add(new DadosNotificacao("securotyKey não existe!"));

        if (string.IsNullOrEmpty(subject)) notifications.Add(new DadosNotificacao("subject não existe!"));

        if (string.IsNullOrEmpty(issuer)) notifications.Add(new DadosNotificacao("issuer não existe!"));

        if (string.IsNullOrEmpty(audience)) notifications.Add(new DadosNotificacao("audience não existe!"));

        return notifications.Count > 0 ? (false, null) : (true, notifications);
    }

    /// <summary>
    /// Método que cria e retorna o token.
    /// </summary>
    /// <returns></returns>
    public (TokenJWT, List<DadosNotificacao>) Builder(UserEntity userEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(TokenJwtBuilder)} - METHOD {nameof(Builder)}\n");

        try
        {
            // Verifica os dados.
            var (success, messages) = EnsureArguments(); if (success is false)
            {
                Log.Error($"[LOG ERROR] - {messages}\n");

                return (null, messages);
            }

            // Adiciona as claims a uma lista.
            var baseClaims = new[]
            {
                new Claim("id", userEntity.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, "Bearer"),
                new Claim(JwtRegisteredClaimNames.Email, userEntity.Email),
                new Claim("phoneNumber", userEntity.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Website, "https://toolsuserapi.azurewebsites.net/")

            }.Union(roles.AsParallel()).Union(claims.AsParallel());

            Log.Information($"[LOG INFORMATION] - Token gerado com sucesso.\n");

            // Gera o token com os dados passados.
            return (new TokenJWT(
                            new JwtSecurityToken(
                                issuer: issuer,
                                audience: audience,
                                claims: baseClaims,
                                expires: DateTime.Now.AddMinutes(expiryInMinutes),
                                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256))

                        ), messages);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            return (null, new List<DadosNotificacao>() { new DadosNotificacao(exception.Message) });
        }
    }
}

