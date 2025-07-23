using ARTAutomation.Main.Models;
using ARTAutomation.Main.Pages;
using NUnit.Framework;
using ARTAutomation.Config;
using ARTAutomation.Base;

namespace ARTAutomation.Tests
{
    [TestFixture]
    public class ClockInTest : BaseTest
    {
        [Test, TestCaseSource(typeof(TestDataUtility), nameof(TestDataUtility.GetLoginTestData))]
        public async Task ClockInUser(LoginCredentials data)
        {
            var testName = TestContext.CurrentContext.Test.Name;
            try
            {
                var loginUrl = EnvironmentConfig.GetLoginUrl("STG");
                await _loginPage.LoginAsync(loginUrl, data.Username!, data.Password!);

                var logActivityPage = new LogActivityPage(_page);

                await logActivityPage.ClickLogActivityPageAsync();

                //await logActivityPage.SelectWorkshiftCustomAsync(data.WorkShift!);

                await logActivityPage.ClickClockInAsync();

                Assert.IsTrue(await logActivityPage.IsSuccessMessageDisplayedAsync(), "Clock-in success message not displayed.");

            }
            catch (Exception ex)
            {
                await CaptureScreenshotAsync(testName, isSuccess: false);
                TestContext.Error.WriteLine($"Clock-in failed for User: {data.Username}\n{ex}");
                throw;
            }
        }
    }
}
