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
            await Timesheet.clickAddTimesheetAsync();

            await Task.Delay(5000);
            await Timesheet.assertAddShiftAsync();
            await Timesheet.selectWorkshiftAsync();
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

        /*[Test]
        public async Task TestSearchTimesheet()
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);

            await Login.LoginAsync();
            await Task.Delay(1000);
            await Login.AssertLoginAsync();
            await Task.Delay(1000);
            await Timesheet.searchTimesheet();
            Assert.Pass();
        }
        */
    }
}