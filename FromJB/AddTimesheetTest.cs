using System.Collections;
using System.Globalization;
using ARTAutomation.Base;
using ARTAutomation.Config;
using ARTAutomation.Main.Models;
using ARTAutomation.Main.Pages;
using CsvHelper;
using Microsoft.Playwright;
using NUnit.Framework;

namespace ARTAutomation.Tests
{
    [TestFixture]
    public class TimesheetTest : BaseTest
    {
        public static IEnumerable TestDataFromCsv()
        {
            var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "ClockInTestData.csv");
            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<LoginCredentials>().ToList();

            foreach (var record in records)
            {
                if (!string.IsNullOrWhiteSpace(record.Username))
                {
                    yield return new TestCaseData(record)
                        .SetName($"Timesheet_{record.Username}");
                }
            }
        }

        [Test, TestCaseSource(nameof(TestDataFromCsv))]
        public async Task GenerateTimesheet(LoginCredentials data)
        {
            var testName = TestContext.CurrentContext.Test.Name;
            try
            {
                var loginUrl = EnvironmentConfig.GetLoginUrl("STG");
                await _loginPage.LoginAsync(loginUrl, data.Username!, data.Password!);

                var timesheetPage = new TimesheetPage(_page);

                await timesheetPage.ClickAddTimesheetAsync();
                await Task.Delay(2000);
                //await timesheetPage.SelectWorkshiftAsync(data.WorkShift!);
                //await timesheetPage.SetDateRangeAsync(data.StartDate!, data.EndDate!);
                //await timesheetPage.ToggleExcludeOptionsAsync(data.ExcludeWeekends, data.ExcludeHolidays);
                await timesheetPage.ClickGenerateAsync();


                Assert.IsTrue(await timesheetPage.IsSuccessMessageDisplayedAsync(), "Expected success message was not displayed.");

            }
            catch (Exception ex)
            {
                await CaptureScreenshotAsync(testName, isSuccess: false);
                TestContext.Error.WriteLine($"Test failed for User: {data.Username}\n{ex}");
                throw;
            }
        }
    }
}
