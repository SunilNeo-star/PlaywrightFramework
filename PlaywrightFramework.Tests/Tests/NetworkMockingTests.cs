using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

[TestFixture]
public class NetworkMockingTests :PageTest
{
    [Test]
    public async Task CanInterceptAndMockNetworkRequest()
    {
        var requests = new List<string>();

        // Listen to wevery request the page makes
        Page.Request += (_, request) =>
        {
            requests.Add(request.Url);
        };

        await Page.GotoAsync("https://quotes.toscrape.com/");
        Console.WriteLine("Requests made by the page:");
        foreach (var url in requests)
        {
            Console.WriteLine($"  {url}");
        }
        Assert.That(requests.Count, Is.GreaterThan(0));
    }
    [Test]
    public async Task CanBlockGoogleFonts()
    {
        // Intercept and abort any request to Google Fonts
        await Page.RouteAsync("**/fonts.googleapis.com/**", route => route.AbortAsync());
        await Page.GotoAsync("https://quotes.toscrape.com/");

            // Page still loads — just without the custom font
            var count  = await Page.Locator(".quote").CountAsync();
            Assert.That(count, Is.EqualTo(10));

            Console.WriteLine("Page loaded successfully even with Google Fonts blocked!");


    }
    [Test]
   public async Task CanMockPageResponse()
{
    // Intercept the main page request and return fake content
    await Page.RouteAsync("https://quotes.toscrape.com/", async route =>
    {
        await route.FulfillAsync(new RouteFulfillOptions
        {
            ContentType = "text/html",
            Body = @"<html><body>
                        <div class='quote'>
                            <span class='text'>Fake quote for testing</span>
                            <span class='author'>Test Author</span>
                        </div>
                     </body></html>"
        });
    });
    await Page.GotoAsync("https://quotes.toscrape.com/");
    var author = await Page.Locator(".author").First.InnerTextAsync();
    Assert.That(author, Is.EqualTo("Test Author"));
     Console.WriteLine("Page showed our fake content instead of the real site!");
}
}