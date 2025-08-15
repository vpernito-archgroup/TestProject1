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
            var filepath = "../../../../TestData/LogProdCLM.csv";
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<LogProd>().ToList();

            foreach (var record in records)
            {
                yield return new TestCaseData(record);
            }

        }
        public async Task LoginART(string uname, string pass)
        {
            var Login = new LoginPage(Page);
            await Login.LoginAsync(uname, pass);
            await Task.Delay(5000);
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

            await Task.Delay(5000);
            await Timesheet.assertClockInButtonAsync();
            await Timesheet.clickClockInAsync();
            await Task.Delay(5000);
            await Timesheet.assertClockInWindowAsync();
            await Timesheet.clickProceedClockInAsync();

            //Assert.Pass();
        }

        public async Task AddProductionActivity(string wstream, string trans, string stg, string desc, string type, string stat, string qty, bool sla, bool ot)
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

            await LogActivity.clickAddProdAsync();
            await Task.Delay(5000);
            await LogActivity.assertProdActivityWindowAsync();
            await Task.Delay(1000);
            await LogActivity.selectWorkstreamAsync(wstream);

            if(wstream == "CAS_Claims Administration")
            {
                await Task.Delay(2000);
                if(trans != "")
                    await LogActivity.selectTransactionAsync(trans);
                await Task.Delay(2000);
                if (stg != "")
                    await LogActivity.selectStageAsync(stg);
                await Task.Delay(2000);
                if (qty != "")
                await LogActivity.inputQuantityAsync(qty);
                await Task.Delay(2000);
                await LogActivity.optionWithinSLAAsync(sla);
                await Task.Delay(2000);
                await LogActivity.optionIsOvertimeAsync(ot);
            }
            else if (wstream == "COS_Claims Operations" || wstream == "CRC_Claims Reporting and Controls" || wstream == "CBS_Claims Bordereaux Services")
            {
                await Task.Delay(2000);
                if (desc != "")
                    await LogActivity.selectDescriptionAsync(desc);
                await Task.Delay(2000);
                if (type != "")
                    await LogActivity.selectTypeAsync(type);
                await Task.Delay(2000);
                if (stat != "")
                    await LogActivity.selectStatusAsync(stat);
                await Task.Delay(2000);
                await LogActivity.inputQuantityAsync(qty);
            }
            else
            {
                await Task.Delay(2000);
                if (trans != "")
                    await LogActivity.selectActivityAsync(trans);
                await Task.Delay(2000);
                if (stg != "")
                    await LogActivity.selectStageAsync(stg);
                await Task.Delay(2000);
                if (qty != "")
                    await LogActivity.inputQuantityAsync(qty);
            }
            await Task.Delay(2000);
            await LogActivity.inputNotesAsync();
            await LogActivity.clickSubmitAsync();
            await Task.Delay(3000);
            await LogActivity.assertSnackbarAsync();
            //Assert.Pass();
        }

        [Test, TestCaseSource(nameof(ReadCSV))]
        public async Task TestLogActivityE2E(LogProd data)
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            var LogActivity = new LogActivity(Page);

            int rec = 0;
            string currentDate = DateTime.Now.ToString("MM-dd-yyyy");

            await LoginART(data.Username, data.Password);

            await Task.Delay(1000);
            int recCount = await Timesheet.checkExistingTimesheetAsync(rec);

            await Task.Delay(1000);
            if (recCount > 0)
            {
                bool buttonClock = await Page.Locator("button[class='mud-button-root mud-button mud-button-filled mud-button-filled-success mud-button-filled-size-small mud-ripple']").IsVisibleAsync();
                var tsDateIn = await Page.Locator("table tr:nth-child(1) td:nth-child(3)").InnerTextAsync();
                var tsDateOut = await Page.Locator("table tr:nth-child(1) td:nth-child(4)").InnerTextAsync();

                if (buttonClock)
                {
                    await Task.Delay(1000);
                    await Timesheet.assertClockInButtonAsync();
                    await Timesheet.clickClockInAsync();
                    await Task.Delay(5000);
                    await Timesheet.assertClockInWindowAsync();
                    await Timesheet.clickProceedClockInAsync();
                    await Task.Delay(3000);
                    await Page.Locator("table tr:nth-child(1) td:nth-child(3)").ClickAsync();
                    await Task.Delay(5000);
                    await LogActivity.assertRegisterActivityAsync();
                    //await LogActivity.assertWorkshiftClockInAsync();
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
                    await Task.Delay(3000);
                    await Page.Locator("table tr:nth-child(1) td:nth-child(3)").ClickAsync();
                    await Task.Delay(5000);
                    await LogActivity.assertRegisterActivityAsync();
                    //await LogActivity.assertWorkshiftClockInAsync();
                    await Task.Delay(5000);
                    await AddProductionActivity(data.Workstream, data.Transaction, data.Stage, data.Description, data.Type, data.Status, data.Quantity, data.SLA, data.OT);
                }
            }
            else
            {
                await Task.Delay(1000);
                await AddTimesheet();
                await Task.Delay(3000);
                await Page.Locator("table tr:nth-child(1) td:nth-child(3)").ClickAsync();
                await Task.Delay(5000);
                await LogActivity.assertRegisterActivityAsync();
                //await LogActivity.assertWorkshiftClockInAsync();
                await Task.Delay(5000);
                await AddProductionActivity(data.Workstream, data.Transaction, data.Stage, data.Description, data.Type, data.Status, data.Quantity, data.SLA, data.OT);
            }
            await Task.Delay(3000);
            Assert.Pass();
        }
        
    }
}