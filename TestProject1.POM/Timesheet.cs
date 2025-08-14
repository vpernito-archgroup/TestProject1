using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace ART.Pages
{
    public class Timesheet
    {

        private readonly IPage Page;

        //Page Objects
        private ILocator addTimesheetButton => Page.Locator("button[class='btn btn-primary']");
        private ILocator workshiftList => Page.Locator("div[class='mud-input mud-input-text mud-input-text-with-label mud-input-adorned-end mud-input-underline mud-shrink mud-typography-subtitle1 mud-select-input']");
        private ILocator workshiftStart => Page.Locator("label.mud-input-label.mud-input-label-animated.mud-input-label-text.mud-input-label-inputcontrol:has-text(\"Start Date\")");
        private ILocator workshiftEnd => Page.Locator("label.mud-input-label.mud-input-label-animated.mud-input-label-text.mud-input-label-inputcontrol:has-text(\"End Date\")");

        //private ILocator dateFrom => Page.Locator("input[class='mud-input-slot mud-input-root mud-input-root-text mud-input-root-adorned-end']");
        //private ILocator dateTo => Page.Locator("input[id='mud-input-slot mud-input-root mud-input-root-text mud-input-root-adorned-end']");
        //private ILocator searchButton => Page.Locator("button[class='mud-button-root mud-button mud-button-filled mud-button-filled-info mud-button-filled-size-small mud-ripple pa-2']");
        private ILocator timesheetBody => Page.Locator("tbody[class='mud-table-body']");

        private ILocator buttonGenerate => Page.Locator("p:has-text(\"GENERATE\")");
        private ILocator buttonCancel => Page.Locator("span:has-text(\"Cancel\")");
        private ILocator buttonClockIn => Page.Locator("button[class='mud-button-root mud-button mud-button-filled mud-button-filled-success mud-button-filled-size-small mud-ripple']");
        private ILocator buttonProceedClockIn => Page.Locator("span:has-text(\"Proceed\")");

        //Assert Objects
        private ILocator headerAddShift => Page.Locator("h6:has-text(\"Add Shift\")");
        private ILocator textClockIn => Page.Locator("h4:has-text(\"Are you sure you want to Clock In?\")");


        public Timesheet(IPage page) => Page = page;
   

        public async Task clickAddTimesheetAsync()
        {
            await addTimesheetButton.ClickAsync();        
        }

        public async Task assertAddShiftAsync()
        {
            bool isVisible = await headerAddShift.IsVisibleAsync();
            if (!isVisible)
                throw new Exception("Add Shift window not displayed!");
        }

        public async Task selectWorkshiftAsync()
        {
            await workshiftList.ClickAsync();
            var option = Page.Locator("p:has-text(\"3:00 PM - 12:00 AM\")");
            await option.ScrollIntoViewIfNeededAsync();
            await option.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await option.ClickAsync();
        }

        public async Task selectWorkshiftDateAsync()
        {
            string startDate = DateTime.Now.ToString("yyyy-MM-dd");
            string endDate = DateTime.Now.ToString("yyyy-MM-dd");
            await workshiftStart.ClickAsync(); 
            await workshiftStart.FillAsync(startDate);
            await workshiftEnd.FillAsync(endDate);
         }

        public async Task clickGenerateWorkshiftAsync()
        {
          await buttonGenerate.ClickAsync();
        }

        public async Task assertClockInButtonAsync()
        {
          bool isVisible = await buttonClockIn.IsVisibleAsync();
          if (!isVisible)
              throw new Exception("Shift not created successfully");
        }
        public async Task clickClockInAsync()
        {
          await buttonClockIn.ClickAsync();
        }

        public async Task assertClockInWindowAsync()
        {
          bool isVisible = await textClockIn.IsVisibleAsync();
            if (!isVisible)
                throw new Exception("Clock In unsuccessful!");
        }

        public async Task clickProceedClockInAsync()
        {
            await buttonProceedClockIn.ClickAsync();
        }

        public async Task<int> checkExistingTimesheetAsync(int rowCount)
        {
          rowCount = await timesheetBody.Locator("tr").CountAsync();
            return rowCount;
        }
      
    }
}
