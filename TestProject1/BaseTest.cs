using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class BaseTest : PageTest

    {

        protected IPlaywright Playwright;

        protected IBrowser Browser;

        protected IPage Page;

        [OneTimeSetUp]

        public async Task GlobalSetup()

        {

            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions

            {

                Headless = false

            });

            var context = await Browser.NewContextAsync();

            Page = await context.NewPageAsync();

        }

        [OneTimeTearDown]

        public async Task GlobalTeardown()

        {

            await Browser.CloseAsync();

            Playwright.Dispose();

        }

    }
}
