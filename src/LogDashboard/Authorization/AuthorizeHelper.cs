using System.Collections.Generic;
using System.Threading.Tasks;
using LogDashboard.Ioc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LogDashboard.Authorization
{
    public class AuthorizeHelper
    {

        public static async Task<bool> AuthorizeAsync(HttpContext context, IList<IAuthorizeData> policies)
        {
            if (policies.Count == 0)
            {
                return true;
            }
            var policyProvider = context.RequestServices.GetRequiredService<IAuthorizationPolicyProvider>();

            var authorizePolicy = await AuthorizationPolicy.CombineAsync(policyProvider, policies);

            var policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();

            var authenticateResult = await policyEvaluator.AuthenticateAsync(authorizePolicy, context);

            var authorizeResult = await policyEvaluator.AuthorizeAsync(authorizePolicy, authenticateResult, context, null);
            if (authorizeResult.Succeeded)
            {
                return true;
            }

            if (authorizeResult.Challenged)
            {
                if (authorizePolicy.AuthenticationSchemes.Count > 0)
                {
                    foreach (var scheme in authorizePolicy.AuthenticationSchemes)
                    {
                        await context.ChallengeAsync(scheme);
                    }
                }
                else
                {
                    await context.ChallengeAsync();
                }
                return false;
            }

            if (!authorizeResult.Forbidden) return false;
            {
                if (authorizePolicy.AuthenticationSchemes.Count > 0)
                {
                    foreach (var scheme in authorizePolicy.AuthenticationSchemes)
                    {
                        await context.ForbidAsync(scheme);
                    }
                }
                else
                {
                    await context.ForbidAsync();
                }
                return false;
            }
        }

    }
}
