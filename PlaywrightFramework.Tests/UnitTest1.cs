using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task PageTitle_ShouldContainQuotesToScrape()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(3000);
        var title = await Page.TitleAsync();
        Assert.That(title, Does.Contain("Quotes to Scrape"));
    }
    [Test]
    public async Task HomePage_ShouldShowTenQuotes()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var quotes = Page.Locator(".quote");
        var count = await quotes.CountAsync();
        Assert.That(count, Is.EqualTo(10));
    }
    [Test]
    public async Task Quote_ShouldContainAuthor()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var quotes = await Page.Locator(".quote").AllAsync();
        string? foundAuthor = null;
        foreach (var quote in quotes)
        {
            var author = await quote.Locator(".author").InnerTextAsync();
            Console.WriteLine(author);
            if (author.Contains("Albert Einstein"))
            {
                foundAuthor = author;
                break;
            }
        }
        Assert.That(foundAuthor, Does.Contain("Albert Einstein"));
    }
    [Test]
    public async Task FirstQuote_ShouldBeAlbertEinstein()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        var firstAuthor = Page.Locator(".author").First;
        var name = await firstAuthor.InnerTextAsync();
        Assert.That(name, Is.EqualTo("Albert Einstein"));
    }
    [Test]
    public async Task HomePageShouldListAllAuthors()
        {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        var allAuthors = await Page.Locator(".author").AllTextContentsAsync();
        Console.WriteLine("Authors found on the homepage:");
        Console.WriteLine(string.Join(", ", allAuthors));
        Assert.That(allAuthors, Does.Contain("Albert Einstein"));
        Assert.That(allAuthors, Does.Contain("J.K. Rowling"));
        }
    [Test]
    public async Task ClickNextPage_ShouldGoToPage2()
    { 
        // TO SEE THE BROWSER IN DEBUG MODE, SET Headless = false
        //var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        //await browser.NewPageAsync();
        await Page.GotoAsync("https://quotes.toscrape.com/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Link, new() { Name = "Next" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var currentUrl = Page.Url;
        Assert.That(currentUrl, Does.Contain("/page/2/"));
        
    }
    [Test]
    public async Task Login_ShouldSucceedWithValidCredentials()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
   
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
           await Page.WaitForTimeoutAsync(2000);
        await Page.GetByLabel("Username").FillAsync("admin");
        await Page.GetByLabel("Password").FillAsync("admin");
        await Page.WaitForTimeoutAsync(2000);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000);

        var logoutButton = await Page.GetByRole(AriaRole.Link, new() { Name = "Logout" }).InnerTextAsync();

        Assert.That(logoutButton, Does.Contain("Logout"));

    }

    [Test]
    public async Task AfterLogin_ShouldShowLogoutLink()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/login");
        await Page.GetByLabel("Username").FillAsync("admin");
        await Page.GetByLabel("Password").FillAsync("admin");
        await Page.WaitForTimeoutAsync(2000);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000);
        var logoutLink = Page.GetByRole(AriaRole.Link, new() { Name = "Logout" });
        await Expect(logoutLink).ToBeVisibleAsync();
        await Expect(logoutLink).ToHaveTextAsync("Logout");
    }
    [Test]
    public async Task Navigation_ShhouldWaitForPageToLoad()
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        //Wait for a specific element to be visible before proceeding
        await Page.WaitForSelectorAsync(".quote");

        var count = await Page.Locator(".quote").CountAsync();
        Assert.That(count, Is.EqualTo(10));
    }
}

