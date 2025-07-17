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
            await Login.Login();
            await Expect(Page).ToHaveURLAsync("http://acs-pwcmanutl01:8082/");
            Assert.Pass();
        }

        [Test]
        public async Task TestAddTimesheet()
        {
            var Login = new LoginPage(Page);
            var Timesheet = new Timesheet(Page);
            await Login.Login();
            await Expect(Page).ToHaveURLAsync("http://acs-pwcmanutl01:8082/");
            await Timesheet.AddTimesheet();
            Assert.Pass();
        }
    }
}