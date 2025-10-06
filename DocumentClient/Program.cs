using Microsoft.Identity.Client;
using System.Net.Http.Headers;

// Details for the CLIENT application
var clientId = "YOUR_CONSOLE_APP_CLIENT_ID_HERE";
var tenantId = "YOUR_TENANT_ID_HERE";


var apiScope = "api://YOUR_API_CLIENT_ID_HERE/Documents.ReadWrite";
var apiUrl = "https://localhost:7212/documents";
//MSAL Setup
var app = PublicClientApplicationBuilder.Create(clientId).WithAuthority($"https://login.microsoftonline.com/{tenantId}").WithRedirectUri("http://localhost").Build();

string[] scopes = { apiScope };
AuthenticationResult result;

try
{
    Console.WriteLine("Acquiring token...");
    result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
    Console.WriteLine("Token acquired successfully!");
}
catch (MsalClientException ex)
{
    Console.WriteLine($"Error acquiring token: {ex.Message}");
    return;
}

//  Call API with Token
Console.WriteLine("\nCalling the API...");
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

try
{
    var response = await httpClient.GetAsync(apiUrl);
    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("API Response: " + content);
    }
    else
    {
        Console.WriteLine("API Error: " + response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Error Details: " + errorContent);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error calling API: {ex.Message}");
}

Console.ReadLine(); 