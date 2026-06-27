
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;


namespace PlaywrightFramework.Tests.Tests;

[TestFixture]
public class LoggedInTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        // Load the saved login state — no login needed!
        return new BrowserNewContextOptions()
        {
            StorageStatePath = "loginState.json"
        };
    }

    [Test]
    public async Task LoggedInUser_ShouldSeeLogoutLink()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        
        var logoutLink = Page.GetByRole(AriaRole.Link, new() { Name = "Logout" });
        await Expect(logoutLink).ToBeVisibleAsync();
        
        Console.WriteLine("Already logged in — no login step needed!");
    }

    [Test]
    public async Task LoggedInUser_ShouldBeAbleToNavigate()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        
        // Verify we're logged in
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Logout" })).ToBeVisibleAsync();
        
        // Navigate to page 2
        await Page.GetByRole(AriaRole.Link, new() { Name = "Next" }).ClickAsync();
        Assert.That(Page.Url, Does.Contain("/page/2/"));
    }
}