using TestProject1.POM;
namespace TestProject1
{   
    public class Tests:BaseTest
    {
       

        [Test]
        public async Task Test1()
        {
            var Login = new LoginPage(Page);
            await Login.Login();

            Assert.Pass();
        }
    }
}