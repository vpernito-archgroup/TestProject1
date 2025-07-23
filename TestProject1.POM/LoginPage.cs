using Microsoft.Playwright;

namespace TestProject1.POM
{
    public class LoginPage
    {

        private readonly IPage Page;

        //Page Objects
        private ILocator username => Page.Locator("input[autocomplete='username']");
        private ILocator password => Page.Locator("input[autocomplete='current-password']");
        private ILocator loginButton => Page.Locator("button:has-text(\"Login\")");

        //Assert Objects
        private ILocator headerART => Page.Locator("h6:has-text(\"Activity Resource Tracker\")");


        public LoginPage(IPage page) => Page = page;
      
        public async Task LoginAsync()
        {
            await Page.GotoAsync("http://acs-pwcmanutl01:8082/Account/Login?ReturnUrl=%2F");
            await username.FillAsync("smarino@Archgroup.com");
            await password.FillAsync("SMarino_16272");
            await Task.Delay(500);
            await loginButton.ClickAsync();
            await Task.Delay(5000);
        }

        public async Task AssertLoginAsync()
        {
            bool isVisible = await headerART.IsVisibleAsync();
            if(!isVisible)
                throw new Exception("Login Failed!");
        }
       
    }
}
