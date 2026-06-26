using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Text.Json;

[TestFixture]
public class APITests : PageTest
{
    private IAPIRequestContext _apiContext;

    [SetUp]
    public async Task SetUpApiContext()
    {
         // Create an API context — like a Page but for HTTP requests
         _apiContext =  await Playwright.APIRequest.NewContextAsync(new()
            {
            BaseURL = "https://jsonplaceholder.typicode.com", 
            ExtraHTTPHeaders = new Dictionary<string, string>
         {
            {"Accept", "application/json"}
         }
         });
         }
    
[TearDown]
  public async Task TearDownApiContext()
    {
        await _apiContext.DisposeAsync();
    }

    [Test]
    public async Task GetRequest_ShouldReturn200()
{
    // Make a GET request to fetch a post
    var response = await _apiContext.GetAsync("/posts/1");
    // Check the status code
        Assert.That(response.Status, Is.EqualTo(200));
        Console.WriteLine($"Status: {response.Status}");
        Console.WriteLine($"Body: {await response.TextAsync()}");
    }


  [Test]
public async Task GitHubApi_ShouldReturnUserProfile()
{
    // Read token from environment variable — NEVER hardcode tokens in test files!
    var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
    
    var githubContext = await Playwright.APIRequest.NewContextAsync(new()
    {
        BaseURL = "https://api.github.com",
        ExtraHTTPHeaders = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {token}" },
            { "Accept", "application/vnd.github+json" },
            { "X-GitHub-Api-Version", "2022-11-28" }
        }
    });

    var response = await githubContext.GetAsync("/user");
    
    Assert.That(response.Status, Is.EqualTo(200));

    var body = await response.TextAsync();
    var json = JsonDocument.Parse(body);
    
    var username = json.RootElement.GetProperty("login").GetString();
    var publicRepos = json.RootElement.GetProperty("public_repos").GetInt32();

    Console.WriteLine($"Username: {username}");
    Console.WriteLine($"Public repos: {publicRepos}");

    Assert.That(username, Is.Not.Null);
    
    await githubContext.DisposeAsync();
}

}