using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace ARTAutomation.Main.Pages
{
    public class LogActivityPage
    {

        private readonly IPage _page;

        public LogActivityPage(IPage page)
        {
            _page = page;
        }

        private ILocator LogActivityLink => _page.Locator("div.mud-nav-link-text:has-text(\"Log Activity\")");

        private ILocator DDWorkshift => _page.Locator("label.mud-input-label.mud-input-label-animated.mud-input-label-text.mud-input-label-inputcontrol:has-text(\"Workshift\")");

        private ILocator BtnClockIn => _page.Locator("p.mud-typography.mud-typography-body1:has-text(\"Clock In\")");

        private ILocator SuccessSnackbar => _page.Locator("div.mud-snackbar-content-message:has-text(\"Logged in. You can now register your activities.\")");


        public async Task ClickLogActivityPageAsync()
        {
            await LogActivityLink.ClickAsync();
        }

        public async Task SelectWorkshiftCustomAsync(string WorkShift)
        {

            await DDWorkshift.ClickAsync();

            var optionLocator = _page.Locator("p.mud-typography-body1", new PageLocatorOptions { HasTextString = WorkShift });

            await optionLocator.WaitForAsync();

            await optionLocator.ClickAsync();
        }

        public async Task ClickClockInAsync()
        {
            await BtnClockIn.ClickAsync();
        }

        public async Task<bool> IsSuccessMessageDisplayedAsync()
        {
            await SuccessSnackbar.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            var text = await SuccessSnackbar.InnerTextAsync();
            return text.Contains("Logged in. You can now register your activities.");
        }

    }
}
