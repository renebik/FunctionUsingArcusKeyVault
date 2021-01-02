using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Arcus.Security.Core;
using Arcus.Security.Providers.AzureKeyVault;

namespace FunctionUsingArcusKeyVault
{
    public class FunctionUsingArcusKeyVault
    {

        private readonly ISecretProvider secretProvider;

        public FunctionUsingArcusKeyVault(ISecretProvider secretProvider)
        {
             this.secretProvider = secretProvider;
        }

        [FunctionName(nameof(FunctionUsingArcusKeyVault))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string secretName = req.Query["secretName"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            secretName = secretName ?? data?.secretName;

            var secret = await secretProvider.GetSecretAsync(secretName);

            string responseMessage = string.IsNullOrEmpty(secretName)
                ? "This HTTP triggered function executed successfully. Pass a secretName in the query string or in the request body for a response."
                : $"Hello this HTTP triggered function retrieved the {secretName} successfully: {secret.Value}";

            return new OkObjectResult(responseMessage);
        }
    }
}
