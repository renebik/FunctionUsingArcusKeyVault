﻿using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: FunctionsStartup(typeof(FunctionUsingArcusKeyVault.Startup))]
namespace FunctionUsingArcusKeyVault
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;

            builder.ConfigureSecretStore(stores =>
            {
                var keyVaultName = config["KeyVault_Name"];
                stores.AddAzureKeyVaultWithManagedServiceIdentity($"https://{keyVaultName}.vault.azure.net/");
            });
        }
    }
}
