using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightFramework.Tests.Pages;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class LoginTests : PageTest
{
    private LoginPage _loginPage;

    [SetUp]
    public void SetUp()
    {
        _loginPage = new LoginPage(Page);
    }

    [Test]
    public async Task Login_WithValidCredentials_ShouldSucceed()
    {
        await _loginPage.GoToAsync();
        await _loginPage.LoginAsync("admin", "admin");

        Assert.That(await _loginPage.IsLoggedInAsync(), Is.True);
    }

    [Test]
    public async Task Login_WithEmptyCredentials_ShouldFail()
    {
        await _loginPage.GoToAsync();
        await _loginPage.LoginAsync("", "");

        Assert.That(await _loginPage.IsLoggedInAsync(), Is.False);
    }
}