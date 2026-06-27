using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightFramework.Tests.Pages;


[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class HomePageTests : PageTest

{
    private  QuotesHomePage _homePage;

    [SetUp]
    public void SetUp()
    {
        _homePage = new QuotesHomePage(Page);
    }

[Test]
public async Task HomePage_ShouldShowTenQuotes()
{
    await _homePage.GoToAsync();
    var count = await _homePage.GetQuoteCountAsync();
    Assert.That(count, Is.EqualTo(10));
}
[Test]
public async Task HomePage_AuthorShouldIncludeEinstein()
    {
        await _homePage.GoToAsync();
        var authors = await _homePage.GetAllAuthorsAsync();
        Assert.That(authors, Does.Contain("Albert Einstein"));
    }
    [Test]
    public async Task ClickingNext_ShouldGoToPage2()
    {
        await _homePage.GoToAsync();
        await _homePage.GoToNextPageAsync();
        Assert.That(Page.Url, Does.Contain("/page/2/"));
    }




}
