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

            await test.Test();

            Assert.Pass();
        }
    }
}