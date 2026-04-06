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
        private IFrame? _frame;

        public IPage Page => _page ?? throw new InvalidOperationException("Playwright not initialized.");
        public IFrame Frame => _frame ?? throw new InvalidOperationException("Frame not initialized.");

        public async Task InitializeAsync(bool headless = false)
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = headless,
                SlowMo = 300,
                Args = new[] { "--start-maximized" }
            });

            _context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = null,
                Locale = "tr-TR",
                TimezoneId = "Europe/Istanbul"
            });
            _context.SetDefaultTimeout(30000);

            _page = await _context.NewPageAsync();
        }

        public async Task<bool> NavigateAsync(string url)
        {
            if (_page is null) return false;

            await _page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 60000
            });

            await _page.WaitForTimeoutAsync(3000);
            await KapatPopupAsync();
            await InitFrameAsync();
            return true;
        }
        private async Task InitFrameAsync()
        {
            if (_page is null) return;
            Console.WriteLine("► iframe bekleniyor...");

            var iframeEl = await _page.WaitForSelectorAsync(
                "iframe.ts-iframe, iframe[src*='onlinepolice'], iframe[src*='hasar']",
                new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 60000 });

            _frame = await iframeEl!.ContentFrameAsync()
                ?? throw new Exception("iframe içeriği alınamadı!");

            await _frame.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await _page.WaitForTimeoutAsync(2000);
            Console.WriteLine("  iframe yüklendi ✓");
        }

        private async Task KapatPopupAsync()
        {
            if (_page is null) return;
            string[] sels =
            {
                "button:has-text('Kabul Et')",
                "button:has-text('Tümünü Kabul Et')",
                "button:has-text('Kapat')",
                "[class*='cookie'] button",
                "[id*='cookie'] button",
                "[aria-label='Close']"
            };

            foreach (var sel in sels)
            {
                try
                {
                    var btn = await _page.WaitForSelectorAsync(sel,
                        new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 2000 });
                    if (btn != null)
                    {
                        await btn.ClickAsync();
                        await _page.WaitForTimeoutAsync(400);
                        Console.WriteLine($"  Popup kapatıldı: {sel}");
                    }
                }
                catch { }
            }
        }

        public async Task<bool> SelectTabAsync(int tabIndex) => true;

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