﻿using ActiveLogin.Authentication.GrandId.AspNetCore.Models;
using ActiveLogin.Authentication.GrandId.AspNetCore.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace ActiveLogin.Authentication.GrandId.AspNetCore
{
    public abstract class
        GrandIdAuthenticationPostConfigureOptions<TOptions, THandler> : IPostConfigureOptions<TOptions>
        where TOptions : GrandIdAuthenticationOptions
    {
        private readonly IDataProtectionProvider _dp;

        protected GrandIdAuthenticationPostConfigureOptions(IDataProtectionProvider dataProtection)
        {
            _dp = dataProtection;
        }

        public void PostConfigure(string name, TOptions options)
        {
            options.DataProtectionProvider = options.DataProtectionProvider ?? _dp;

            if (options.StateDataFormat == null)
            {
                IDataProtector dataProtector = options.DataProtectionProvider.CreateProtector(
                    typeof(THandler).FullName,
                    name,
                    "v1"
                );

                options.StateDataFormat = new SecureDataFormat<GrandIdState>(
                    new GrandIdStateSerializer(),
                    dataProtector
                );
            }
        }
    }
}
