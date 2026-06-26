using Microsoft.Playwright;

namespace PlaywrightFramework.Tests.Pages;

public class QuotesHomePage
{
    private readonly IPage _page;

    public QuotesHomePage(IPage page)
    {
        _page = page;
    }

    // All selectors in one place
    private ILocator QuoteElements => _page.Locator(".quote");
    private ILocator AuthorElements => _page.Locator(".author");
    private ILocator NextButton => _page.GetByRole(AriaRole.Link, new() { Name = "Next" });
    // Actions
    public async Task GoToAsync()
    {
        await _page.GotoAsync("https://quotes.toscrape.com/");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<int> GetQuoteCountAsync()
    {
        return await QuoteElements.CountAsync();
    }

    public async Task<IReadOnlyList<string>> GetAllAuthorsAsync()
    {
        return await AuthorElements.AllTextContentsAsync();
    }

    public async Task<string?> FindAuthorByNameAsync(string authorName)
    {
        var quotes = await QuoteElements.AllAsync();
        foreach (var quote in quotes)
        {
            var author = await quote.Locator(".author").InnerTextAsync();
            if (author.Contains(authorName))
            {
                return author;
            }
        }
        return null;
    }
    public async Task<string> GetFirstAuthorAsync()
    {
        return await AuthorElements.First.InnerTextAsync();
    }
      public async Task GoToNextPageAsync()
    {
        await NextButton.ClickAsync();
    }
}