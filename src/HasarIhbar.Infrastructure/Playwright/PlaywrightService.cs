using HasarIhbar.Domain.Interfaces;
using Microsoft.Playwright;

namespace HasarIhbar.Infrastructure.Playwright
{
	public class PlaywrightService : IPlaywrightService
	{
		private IPlaywright? _playwright;
		private IBrowser? _browser;
		private IBrowserContext? _context;
		private IPage? _page;

		public IPage Page => _page ?? throw new InvalidOperationException("Playwright not initialized.");

		public async Task InitializeAsync(bool headless = false)
		{
			_playwright = await Microsoft.Playwright.Playwright.CreateAsync();
			_browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
			{
				Headless = headless,
				SlowMo = 100
			});
			_context = await _browser.NewContextAsync();
			_page = await _context.NewPageAsync();
		}

		public async Task<bool> NavigateAsync(string url)
		{
			if (_page is null) return false;
			var response = await _page.GotoAsync(url, new PageGotoOptions
			{
				WaitUntil = WaitUntilState.NetworkIdle,
				Timeout = 30000
			});
			return response?.Ok ?? false;
		}

		public async Task<bool> SelectTabAsync(int tabIndex)
		{
			if (_page is null) return false;
			try
			{
				var tabs = await _page.QuerySelectorAllAsync(".tab-item, .nav-link, li[role='tab']");
				if (tabIndex < tabs.Count)
				{
					await tabs[tabIndex].ClickAsync();
					await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		public async Task TakeScreenshotAsync(string path)
		{
			if (_page is null) return;
			await _page.ScreenshotAsync(new PageScreenshotOptions { Path = path, FullPage = true });
		}

		public async Task CloseAsync()
		{
			if (_page is not null) await _page.CloseAsync();
			if (_context is not null) await _context.CloseAsync();
			if (_browser is not null) await _browser.CloseAsync();
		}

		public async ValueTask DisposeAsync()
		{
			await CloseAsync();
			_playwright?.Dispose();
		}
	}
}