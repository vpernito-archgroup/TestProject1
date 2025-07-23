
using System.Collections;
using System.Formats.Asn1;
using System.Globalization;
using ARTAutomation.Base;
using ARTAutomation.Config;
using ARTAutomation.Main.Models;
using CsvHelper;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;


namespace ARTAutomation.Tests
{
	[TestFixture]

    public class LoginTest : BaseTest
	{
		public static IEnumerable TestDataFromCsv()
		{
			var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "FinalTestData.csv");
			using var reader = new StreamReader(csvFilePath);
			using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			var records = csv.GetRecords<LoginCredentials>().ToList();

			foreach (var record in records)
			{
				if (!string.IsNullOrWhiteSpace(record.Username))
				{
					yield return new TestCaseData(record)
					.SetName($"Login_{record.Username}");
				}
			}
		}

		[Test, TestCaseSource(nameof(TestDataFromCsv))]
		public async Task ValidLoginTest(LoginCredentials data)
		{
			var testName = TestContext.CurrentContext.Test.Name;
			try
			{
                var loginUrl = EnvironmentConfig.GetLoginUrl("STG");
                await _loginPage.LoginAsync(loginUrl, data.Username!, data.Password!);
				await Task.Delay(1000);

                var usernameLabel = _page.Locator(".mud-typography.mud-typography-h6.ml-3");
                var text = await usernameLabel.InnerTextAsync();
                StringAssert.Contains("Activity Resource Tracker", text, "Expected Header text after login.");


            }
			catch (Exception ex)
			{
				// Capture screenshot on failure 

				await CaptureScreenshotAsync(testName, isSuccess: false);
				TestContext.Error.WriteLine($"Test failed for User: {data.Username}\n{ex}");
				throw;

			}

		}
	}
}


