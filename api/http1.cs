using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace swatest5601fa;

public class http1
{
    private readonly ILogger<http1> _logger;
    private int counter = 0;

    public http1(ILogger<http1> logger)
    {
        _logger = logger;
    }

    [Function("message")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        if (counter != 1)
        {
            counter = 1;

            Task.Factory.StartNew(async () =>
            {
                while(true)
                {
                    _logger.LogInformation($"Counter: {counter}");

                    var httpReq = new HttpRequestMessage(HttpMethod.Get, "https://ambitious-field-0ee1c3f0f.6.azurestaticapps.net");
                    
                    using(httpClient = new HttpClient())
                    {
                        var response = await httpClient.SendAsync(httpReq);
                        _logger.LogInformation($"Response Code: {response.StatusCode}");
                    }

                    Thread.Sleep(6000);
                }
            });
        }        

        return new OkObjectResult("Welcome to Azure Functions!");
    }
}