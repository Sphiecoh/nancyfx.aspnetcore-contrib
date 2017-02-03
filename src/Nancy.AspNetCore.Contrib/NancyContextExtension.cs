using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Collections.Generic;
using Nancy.Owin;

namespace Nancy.AspNetCore.Contrib
{
    public static class NancyContextExtension
    {
        public static AuthenticationManager GetAuthenticationManager(this NancyContext context, bool throwOnNull = false)
        {
            var environment = context.GetOwinEnvironment();
            if (environment == null && throwOnNull)
            {
                throw new InvalidOperationException("OWIN environment not found. Is this an owin application?");
            }

            try
            {
                var httpcontext = (Microsoft.AspNetCore.Http.HttpContext)environment["Microsoft.AspNetCore.Http.HttpContext"];

                return httpcontext.Authentication;
            }
            catch (KeyNotFoundException)
            {

                try
                {
                    var defaultcontext = (Microsoft.AspNetCore.Http.DefaultHttpContext)environment["Microsoft.AspNetCore.Http.DefaultHttpContext"];

                    return defaultcontext.Authentication;
                }
                catch (KeyNotFoundException)
                {

                    return null;
                }
            }
        }
    }
}
