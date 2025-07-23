using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace ARTAutomation.Main.Pages
{
	public class LoginPage
	{
		private readonly IPage _page;

		private ILocator UsernameInput => _page.Locator("input[placeholder='name@example.com']");
		private ILocator PasswordInput => _page.Locator("input[placeholder='password']");

        private ILocator SignInButton => _page.Locator(".mud-typography.mud-typography-body1.ms-2");

		public LoginPage(IPage page) => _page = page;

		public async Task LoginAsync(string baseUrl, string username, string password)
		{
			try
			{
				await _page.GotoAsync(baseUrl);
				await UsernameInput.FillAsync(username);
				await PasswordInput.FillAsync(password);
				await Task.Delay(500);
				await SignInButton.ClickAsync();
				await Task.Delay(1000);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Login failed: {ex.Message}");
				throw;
			}
		}
	}
}
