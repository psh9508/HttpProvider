namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var test = new TestProvider();

            var result = await test.GetSummonerInfo("hideonbush");

            //if (result != String.Empty)
            //    Assert.Pass();
            //else
            //    Assert.Fail();
        }
    }
}