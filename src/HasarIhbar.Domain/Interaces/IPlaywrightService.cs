namespace HasarIhbar.Domain.Interfaces
{
	public interface IPlaywrightService : IAsyncDisposable
	{
		Task InitializeAsync(bool headless = false);
		Task<bool> NavigateAsync(string url);
		Task<bool> SelectTabAsync(int tabIndex);
		Task TakeScreenshotAsync(string path);
		Task CloseAsync();
	}
}