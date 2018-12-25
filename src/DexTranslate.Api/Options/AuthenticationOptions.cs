using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DexTranslate.Api.Options
{
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ClaimsIdentity Identity { get; set; }

        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }
    }
}