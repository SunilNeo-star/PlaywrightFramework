using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightFramework.Tests.TestData;


namespace PlaywrightFramework.Tests.Tests;

[TestFixture]
public class DataDrivenTests : PageTest
{
    // TestCase — inline data, simple and quick
   // [Test]
    //[TestCase("Albert Einstein")]
    //[TestCase("J.K. Rowling")]
    //[TestCase("Jane Austen")]

    [Test]
    [TestCaseSource(typeof(AuthorTestData), nameof(AuthorTestData.HomepageAuthors))]
    public async Task Homepage_ShouldContainAuthor(string authorName)
    {
        await Page.GotoAsync("https://quotes.toscrape.com/");
        
        var authors = await Page.Locator(".author").AllInnerTextsAsync();
        
        Assert.That(authors, Does.Contain(authorName),
            $"Expected to find author '{authorName}' on the homepage");
    }
}