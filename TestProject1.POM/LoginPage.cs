using Microsoft.Playwright;

namespace TestProject1.POM
{
    public class LoginPage
    {

        private readonly IPage Page;
        public LoginPage(IPage page)
        {
            Page = page;
        }

        public async Task Login()
        {
            var username = Page.Locator("input[autocomplete='username']");
            var password = Page.Locator("input[autocomplete='current-password']");
            var loginButton = Page.Locator("button:has-text(\"Login\")");
            Page.GotoAsync("http://acs-pwcmanutl01:8082/Account/Login?ReturnUrl=%2F");
            await Task.Delay(5000);
            await username.FillAsync("Jabad@archgroup.com");
            await Task.Delay(1000);
            await password.FillAsync("JAbad_60000");
            await Task.Delay(1000);
            await loginButton.ClickAsync();
        }
       
    }
}
