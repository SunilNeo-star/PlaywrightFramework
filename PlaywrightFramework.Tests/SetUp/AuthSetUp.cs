using Microsoft.Playwright;
using NUnit.Framework;

// NO namespace here — this makes it run for ALL tests in the project

[SetUpFixture]
public class AuthSetup
{
    [OneTimeSetUp]
    public async Task SaveLoginState()
    {
        var username = Environment.GetEnvironmentVariable("TEST_USERNAME") 
                       ?? throw new Exception("TEST_USERNAME environment variable not set");
        var password = Environment.GetEnvironmentVariable("TEST_PASSWORD") 
                       ?? throw new Exception("TEST_PASSWORD environment variable not set");

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = true
        });

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

     await page.GotoAsync("https://quotes.toscrape.com/login");
await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
await page.GetByLabel("Username").FillAsync(username);
await page.GetByLabel("Password").FillAsync(password);
await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await context.StorageStateAsync(new()
        {
            Path = "loginState.json"
        });

        Console.WriteLine($"Login state saved for user: {username}");
    }
}