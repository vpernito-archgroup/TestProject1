using ART.Data;
using ART.Pages;
using CsvHelper;
using CsvHelper.Configuration;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Globalization;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ART.Test
{   
    public class Tests:BaseTest
    {
        public static IEnumerable ReadCSV()
        {
            var filepath = "../../../../TestData/TestData.csv";
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<TestData>().ToList();

            foreach (var record in records)
            {
                yield return new TestCaseData(record);
            }

        }
        public async Task LoginART(string uname, string pass)
        {
            var Login = new LoginPage(Page);
            await Login.LoginAsync(uname, pass);
            await Task.Delay(1000);
            await Login.AssertLoginAsync();
            //Assert.Pass();
        }

        public async Task AddTimesheet()
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

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

            await Task.Delay(1000);
            await LogActivity.assertRegisterActivityAsync();

            //Assert.Pass();
        }

        public async Task AddProductionActivity(string wstream, string trans, string stg, string desc, string type, string stat, string qty, bool sla, bool ot)
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

            await LogActivity.clickAddProdAsync();
            await Task.Delay(1000);
            await LogActivity.assertProdActivityWindowAsync();
            await Task.Delay(500);
            await LogActivity.selectWorkstreamAsync(wstream);

            if(wstream == "CAS_Claims Administration")
            {
                await Task.Delay(5000);
                await LogActivity.selectTransactionAsync(trans);
                await LogActivity.selectStageAsync(stg);
                await LogActivity.inputQuantityAsync(qty);
                await LogActivity.optionWithinSLAAsync(sla);
                await LogActivity.optionIsOvertimeAsync(ot);
            }
            else if (wstream == "COS_Claims Operations" || wstream == "CRC_Claims Reporting and Controls" || wstream == "CBS_Claims Bordereaux Services")
            {
                await Task.Delay(5000);
                await LogActivity.selectDescriptionAsync(desc);
                await LogActivity.selectTypeAsync(type);
                await LogActivity.selectStatusAsync(stat);
                await LogActivity.inputQuantityAsync(qty);
            }
            await LogActivity.clickSubmitAsync();
            await Task.Delay(5000);

            //Assert.Pass();
        }

        [Test, TestCaseSource(nameof(ReadCSV))]
        public async Task TestLogActivityE2E(TestData data)
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

            int rec = 0;
            string currentDate = DateTime.Now.ToString("MM-dd-yyyy");

            await LoginART(data.Username, data.Password);

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
                    await AddProductionActivity(data.Workstream,data.Transaction,data.Stage,data.Description,data.Type,data.Status,data.Quantity,data.SLA,data.OT);
                }

                else if (tsDateIn.Contains(currentDate) && string.IsNullOrEmpty(tsDateOut))
                {
                    await Page.Locator("table tr:nth-child(1) td:nth-child(3)").ClickAsync();
                    await Task.Delay(5000);
                    await LogActivity.assertRegisterActivityAsync();
                    await Task.Delay(5000);
                    await AddProductionActivity(data.Workstream, data.Transaction, data.Stage, data.Description, data.Type, data.Status, data.Quantity, data.SLA, data.OT);
                }
                else
                {
                    await Task.Delay(1000);
                    await AddTimesheet();
                    await LogActivity.assertWorkshiftClockInAsync();
                    await Task.Delay(5000);
                    await AddProductionActivity(data.Workstream, data.Transaction, data.Stage, data.Description, data.Type, data.Status, data.Quantity, data.SLA, data.OT);
                }
            }
            else
            {
                await Task.Delay(1000);
                await AddTimesheet();
                await LogActivity.assertWorkshiftClockInAsync();
                await Task.Delay(5000);
                await AddProductionActivity(data.Workstream, data.Transaction, data.Stage, data.Description, data.Type, data.Status, data.Quantity, data.SLA, data.OT);
            }

            await Task.Delay(5000);
            Assert.Pass();
        }
        
    }
}