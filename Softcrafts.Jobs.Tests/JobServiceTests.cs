using FluentAssertions;
using Softcrafts.Jobs.Business;
using NUnit.Framework;

namespace Softcrafts.Jobs.Tests
{
    [TestFixture]
    public class JobServiceTests
    {
        [Test]
        public void CreateApiUserTest()
        {
            IHandleJobs repo = new JobRepository("test");
            var sut = repo.CreateApiUser("test", "test");
            sut.Result.Success.Should().BeTrue();
        }

        [Test]
        public void IsApiUserAllowedTest()
        {
            IHandleJobs repo = new JobRepository("test");
            var sut = repo.IsApiUserAllowed("test", "test");
            sut.Success.Should().BeTrue();
        }
    }
}
