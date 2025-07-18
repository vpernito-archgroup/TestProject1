using Microsoft.Playwright;

namespace TestProject1.POM
{
    public class LogActivity
    {

        private readonly IPage Page;

        //Page Objects
        private ILocator menuLogActivity => Page.Locator("a[href='/activitylog']");
        private ILocator workshiftList => Page.Locator("div[class='mud-input mud-input-text mud-input-text-with-label mud-input-adorned-end mud-input-underline mud-shrink mud-typography-subtitle1 mud-select-input']");
        private ILocator buttonClockIn => Page.Locator("p:has-text(\"Clock In\")");
        private ILocator buttonAddProd => Page.Locator("path[d='M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z']");
        private ILocator buttonAddNonProd => Page.Locator("path[d='M22,5.18L10.59,16.6l-4.24-4.24l1.41-1.41l2.83,2.83l10-10L22,5.18z M12,20c-4.41,0-8-3.59-8-8s3.59-8,8-8 c1.57,0,3.04,0.46,4.28,1.25l1.45-1.45C16.1,2.67,14.13,2,12,2C6.48,2,2,6.48,2,12s4.48,10,10,10c1.73,0,3.36-0.44,4.78-1.22 l-1.5-1.5C14.28,19.74,13.17,20,12,20z M19,15h-3v2h3v3h2v-3h3v-2h-3v-3h-2V15z']");
        //private ILocator listWorkstream => Page.Locator("div[class='mud-grid-item mud-grid-item-xs-12 mud-grid-item-sm-12 mud-grid-item-md-12']");
        private ILocator listTransaction => Page.Locator("div[class='mud-input-control mud-input-required mud-input-text-with-label mud-select input-activity']");
        private ILocator listStage => Page.Locator("div[class='mud-input-control mud-input-required mud-input-text-with-label mud-select input-stage']");
        private ILocator inputQuantity => Page.GetByRole(AriaRole.Textbox, new() { Name = "Quantity*" });
        private ILocator buttonSubmit => Page.Locator("p:has-text(\"Submit\")");

        //Assert Objects
        private ILocator headerRegisterActivity => Page.Locator("h6:has-text(\"Register Activity\")");
        private ILocator headerProdActivity => Page.Locator("h6:has-text(\"Register Production Activity\")");


        public LogActivity(IPage page) => Page = page;

        public async Task navigateLogActivity()
        {
            await menuLogActivity.ClickAsync();
        }

        public async Task assertRegisterActivityAsync()
        {
            bool isVisible = await headerRegisterActivity.IsVisibleAsync();
            if (!isVisible)
                throw new Exception("Register Activity page not displayed!");
        }

        public async Task assertWorkshiftClockInAsync()
        {
            bool isVisible = await buttonClockIn.IsVisibleAsync();
            if (isVisible)
                await buttonClockIn.ClickAsync();
        }

        public async Task clickAddProdAsync()
        {
            await buttonAddProd.ClickAsync();
        }

        public async Task assertProdActivityWindowAsync()
        {
            bool isVisible = await headerProdActivity.IsVisibleAsync();
            if (!isVisible)
                throw new Exception("Register Production Activity window is not displayed!");
        }

        /*public async Task selectWorkstreamAsync()
        {
            await listWorkstream.ClickAsync();
        }*/

       public async Task selectTransactionAsync()
       {
           await listTransaction.ClickAsync();
           var option = Page.GetByText("Claim", new() { Exact = true });
           await option.ScrollIntoViewIfNeededAsync();
           await option.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
           await option.ClickAsync();
       }
       public async Task selectStageAsync()
       {
           await listStage.ClickAsync();
           var option = Page.GetByText("Queried", new() { Exact = true });
           await option.ScrollIntoViewIfNeededAsync();
           await option.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
           await option.ClickAsync();
        }
       public async Task inputQuantityAsync()
       {
           await inputQuantity.FillAsync("1");
       }

       public async Task clickSubmitAsync()
       {
            await buttonSubmit.ClickAsync();
       }
    }
}
