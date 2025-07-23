using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace ARTAutomation.Main.Pages
{
    public class TimesheetPage
    {
        private readonly IPage _page;

        private ILocator TimeSheetLink => _page.Locator("div.mud-nav-link-text:has-text(\"Timesheet\")");
        private ILocator ButtonAddTimeSheet => _page.Locator("button.btn.btn-primary:has-text(\"Add Timesheet\")");
        //private ILocator DDWorkshift => _page.Locator("label.mud-input-label.mud-input-label-animated.mud-input-label-text.mud-input-label-inputcontrol:has-text(\"Workshift\")");
        private ILocator DDWorkshift => _page.Locator("div[class='mud-input mud-input-text mud-input-text-with-label mud-input-adorned-end mud-input-underline mud-shrink mud-typography-subtitle1 mud-select-input'] div[class='mud-input-slot mud-input-root mud-input-root-text mud-input-root-adorned-end mud-select-input']");
        private ILocator PickerStartDate => _page.Locator("label.mud-input-label.mud-input-label-animated.mud-input-label-text.mud-input-label-inputcontrol:has-text(\"Start Date\")");
        private ILocator PickerEndDate => _page.Locator("label.mud-input-label.mud-input-label-animated.mud-input-label-text.mud-input-label-inputcontrol:has-text(\"End Date\")");
        private ILocator TBExcludeWeekends => _page.Locator("p.mud-typography.mud-typography-body1:has-text(\"Exclude Weekends\")");
        private ILocator TBExcludeHolidays => _page.Locator("p.mud-typography.mud-typography-body1:has-text(\"Exclude Holidays\")");
        private ILocator BtnGenerate => _page.Locator("p.mud-typography.mud-typography-body1:has-text(\"GENERATE\")");
        private ILocator BtnCancel => _page.Locator("span.mud-button-label:has-text(\"Cancel\")");
        private ILocator SuccessSnackbar => _page.Locator("div.mud-snackbar-content-message:has-text(\"Success: Shift saved!\")");


        public TimesheetPage(IPage page) => _page = page;

        public async Task ClickAddTimesheetAsync()
        {
            await ButtonAddTimeSheet.ClickAsync();
        }

        public async Task SelectWorkshiftAsync(string workshift)
        {
            await DDWorkshift.ClickAsync();
            var option = _page.Locator($"p.mud-typography-body1:has-text(\"{workshift}\")");
            await option.ScrollIntoViewIfNeededAsync();
            await option.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await option.ClickAsync();
        }

        public async Task SetDateRangeAsync(string startDate, string endDate)
        {
            await PickerStartDate.FillAsync(startDate);
            await PickerEndDate.FillAsync(endDate);
        }


        public async Task ToggleExcludeOptionsAsync(bool excludeWeekends, bool excludeHolidays)
        {
            if (excludeWeekends)
                await TBExcludeWeekends.CheckAsync();
            else
                await TBExcludeWeekends.UncheckAsync();

            if (excludeHolidays)
                await TBExcludeHolidays.CheckAsync();
            else
                await TBExcludeHolidays.UncheckAsync();
        }


        public async Task ClickGenerateAsync()
        {
            await BtnGenerate.ClickAsync();
        }

        public async Task ClickCancelAsync()
        {
            await BtnCancel.ClickAsync();
        }

        public async Task NavigateToTimesheetAsync()
        {
            await TimeSheetLink.ClickAsync();
        }

        public async Task<bool> IsSuccessMessageDisplayedAsync()
        {
            await SuccessSnackbar.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            var text = await SuccessSnackbar.InnerTextAsync();
            return text.Contains("Success: Shift saved!");
        }



    }
}
