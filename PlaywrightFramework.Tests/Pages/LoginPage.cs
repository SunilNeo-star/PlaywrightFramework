using Microsoft.Playwright;

namespace PlaywrightFramework.Tests.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
         // Constructor — receives the page from the test
        _page = page;

    }
    //// All selectors in one place
    private ILocator UsernameInput => _page.GetByLabel("Username");
    private ILocator PasswordInput => _page.GetByLabel("Password");
    private ILocator LoginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Login" });
    private ILocator LogoutLink => _page.GetByRole(AriaRole.Link, new() { Name = "Logout" });

    //Actions
    public async Task GoToAsync()
    {
        await _page.GotoAsync("https://quotes.toscrape.com/login");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    
    public async Task LoginAsync(string username, string password)
    {
        
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    public async Task<bool> IsLoggedInAsync()
    {
        return await LogoutLink.IsVisibleAsync();
    }
    




}

    
