using B2CIdTokenBuilder.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace B2CIdTokenBuilder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new TokenGenerationRequest
            {
                B2CTenant = ConfigurationManager.AppSettings["B2CTenant"],
                B2CPolicy = ConfigurationManager.AppSettings["B2CPolicy"],
                ClientId = ConfigurationManager.AppSettings["B2CClientId"],
                ReplyUrl = ConfigurationManager.AppSettings["B2CRedirectUri"]
            };

            return View(model);
        }

        public ActionResult GenerateToken(TokenGenerationRequest request)
        {
            Dictionary<string, string> claims = new Dictionary<string, string>();
            if (request.Claim1Value != null) { claims[request.Claim1Name] = request.Claim1Value; }
            if (request.Claim2Value != null) { claims[request.Claim2Name] = request.Claim2Value; }
            if (request.Claim3Value != null) { claims[request.Claim3Name] = request.Claim3Value; }
            if (request.Claim4Value != null) { claims[request.Claim4Name] = request.Claim4Value; }
            if (request.Claim5Value != null) { claims[request.Claim5Name] = request.Claim5Value; }
            if (request.Claim6Value != null) { claims[request.Claim6Name] = request.Claim6Value; }
            if (request.Claim7Value != null) { claims[request.Claim7Name] = request.Claim7Value; }
            if (request.Claim8Value != null) { claims[request.Claim8Name] = request.Claim8Value; }

            string token = BuildIdToken(claims, request.ClientId);
            string Url = BuildInviteUrl(request.B2CTenant, request.B2CPolicy, request.ClientId, request.ReplyUrl, token);

            return PartialView(new TokenGenerationResult
            {
                InviteLink = Url,
                TokenData = token
            });
        }

        private string BuildIdToken(IDictionary<string, string> claimsDict, string audience)
        {
            // All parameters sent to Azure AD B2C need to be sent as claims
            IEnumerable<Claim> claims = claimsDict.Select(kvp => new Claim(kvp.Key, kvp.Value, ClaimValueTypes.String, JwtConfig.JwtIssuer));

            // Create the token
            JwtSecurityToken token = new JwtSecurityToken(
                    JwtConfig.JwtIssuer,
                    audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddDays(JwtConfig.JwtExpirationDays),
                    JwtConfig.JwtSigningCredentials);

            // Get the representation of the signed token
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }

        private string BuildInviteUrl(string tenant, string policy, string clientId, string redirectUri, string token)
        {
            string nonce = Guid.NewGuid().ToString("n");

            // A normal Azure AD B2C OIDC request plus the id_token_hint parameter
            UriBuilder builder = new UriBuilder($"https://{tenant}.b2clogin.com")
            {
                Path = $"{tenant}.onmicrosoft.com/oauth2/v2.0/authorize",
                Query = $"p={policy}&client_id={clientId}&redirect_uri={redirectUri}&nonce={nonce}&scope=openid&response_type=id_token&id_token_hint={token}"
            };

            return builder.Uri.ToString();
        }
    }
}