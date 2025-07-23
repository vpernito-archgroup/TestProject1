using Microsoft.VisualBasic;
using TestProject1.POM;
namespace TestProject1
{   
    public class Tests:BaseTest
    {

        [Test]
        public async Task TestLogin()
        {
            var Login = new LoginPage(Page);
            await Login.LoginAsync();
            await Task.Delay(1000);
            await Login.AssertLoginAsync();
            Assert.Pass();
        }

        [Test]
        public async Task TestAddTimesheet()
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);
            
            await Login.LoginAsync();
            await Task.Delay(1000);
            await Login.AssertLoginAsync();

            await Task.Delay(1000);
            //await Timesheet.checkExistingTimesheetAsync();
            await Timesheet.clickAddTimesheetAsync();

            await Task.Delay(5000);
            await Timesheet.assertAddShiftAsync();
            await Timesheet.selectWorkshiftAsync();
            //await Timesheet.selectWorkshiftDateAsync();
            await Timesheet.clickGenerateWorkshiftAsync();

            await Task.Delay(1000);
            await Timesheet.assertClockInButtonAsync();
            await Timesheet.clickClockInAsync();

            await Task.Delay(1000);
            await LogActivity.assertRegisterActivityAsync();

            Assert.Pass();
        }

        [Test]
        public async Task TestLogActivityClockIn()
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

            await Login.LoginAsync();
            await Task.Delay(1000);
            await Login.AssertLoginAsync();

            await Task.Delay(500);
            await LogActivity.navigateLogActivity();
            await Task.Delay(1000);
            await LogActivity.assertRegisterActivityAsync();
            await Task.Delay(5000);
            await LogActivity.assertWorkshiftClockInAsync();

            Assert.Pass();
        }

        [Test]
        public async Task TestAddProductionActivityCAS()
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

            await Login.LoginAsync();
            await Task.Delay(5000);
            await Login.AssertLoginAsync();

            await Task.Delay(500);
            await LogActivity.navigateLogActivity();
            await Task.Delay(5000);
            await LogActivity.assertRegisterActivityAsync();
            await Task.Delay(1000);
            await LogActivity.clickAddProdAsync();
            await Task.Delay(1000);
            await LogActivity.assertProdActivityWindowAsync();
            await Task.Delay(500);
            await LogActivity.selectTransactionAsync();
            await LogActivity.selectStageAsync();
            await LogActivity.inputQuantityAsync();
            await LogActivity.clickSubmitAsync();
            await Task.Delay(5000);

            Assert.Pass();
        }

        [Test]
        public async Task TestLogActivityE2E()
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

            int rec = 0;
            string currentDate = DateTime.Now.ToString("MM-dd-yyyy");

            await Login.LoginAsync();
            await Task.Delay(1000);
            await Login.AssertLoginAsync();

            await Task.Delay(1000);
            int recCount = await Timesheet.checkExistingTimesheetAsync(rec);

            await Task.Delay(500);
            if (recCount > 0)
            {
                var tsDateIn = await Page.Locator("table tr:nth-child(1) td:nth-child(3)").InnerTextAsync();
                var tsDateOut = await Page.Locator("table tr:nth-child(1) td:nth-child(4)").InnerTextAsync();

                if (tsDateIn.Contains("CLOCK IN"))
                {
                    await Task.Delay(1000);
                    await Timesheet.assertClockInButtonAsync();
                    await Timesheet.clickClockInAsync();
                    await Task.Delay(5000);
                    await LogActivity.assertRegisterActivityAsync();
                    await LogActivity.assertWorkshiftClockInAsync();
                    await Task.Delay(5000);
                    await LogActivity.clickAddProdAsync();
                }

                else if (tsDateIn.Contains(currentDate) && string.IsNullOrEmpty(tsDateOut))
                {
                    await Page.Locator("table tr:nth-child(1) td:nth-child(3)").ClickAsync();
                    await Task.Delay(1000);
                    await LogActivity.assertRegisterActivityAsync();
                    await Task.Delay(5000);
                    await LogActivity.clickAddProdAsync();
                }
            }
            else
            {
                await Task.Delay(1000);
                await Timesheet.clickAddTimesheetAsync();

                await Task.Delay(5000);
                await Timesheet.assertAddShiftAsync();
                await Timesheet.selectWorkshiftAsync();
                //await Timesheet.selectWorkshiftDateAsync();
                await Timesheet.clickGenerateWorkshiftAsync();

                await Task.Delay(1000);
                await Timesheet.assertClockInButtonAsync();
                await Timesheet.clickClockInAsync();
                await Task.Delay(5000);
                await LogActivity.assertRegisterActivityAsync();
                await LogActivity.assertWorkshiftClockInAsync();
                await Task.Delay(5000);
                await LogActivity.clickAddProdAsync();
            }

            await Task.Delay(5000);
            Assert.Pass();
        }
        
    }
}