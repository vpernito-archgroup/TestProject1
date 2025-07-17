using Microsoft.Playwright;

namespace TestProject1.POM
{
    public class Timesheet
    {

        private readonly IPage Page;

        public Timesheet(IPage page)
        {
            Page = page;
        }
        

        public async Task AddTimesheet()
        {

            var addTimesheet = Page.Locator("button[class='btn btn-primary']");
            await Task.Delay(5000);
            await addTimesheet.ClickAsync();
            await Task.Delay(1000);           
        }
       
    }
}
