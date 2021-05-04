using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SpacedRepApp.Infrastructure;
using SpacedRepApp.Infrastructure.Domain;
using SpacedRepApp.Share;

namespace SpacedRepApp.UnitTests
{
    [TestFixture]
    public class TagRepositoryTest
    {
        private ITagRepository tagRepositoryToTest;
        private Mock<ICacheService> _cacheServiceMock;

        private DbContextOptions<SpacedRepAppDbContext> _options;

        [SetUp]
        public void OneTimeSetup()
        {
            _options = new DbContextOptionsBuilder<SpacedRepAppDbContext>()
           .UseInMemoryDatabase(databaseName: "SpacedRepAppDb")
           .Options;

            _cacheServiceMock = new Mock<ICacheService>();
        }

        [Test]
        public void AddingTagWithSameName_NoDuplicate()
        {
            using (var context = new SpacedRepAppDbContext(_options))
            {
                tagRepositoryToTest = new TagRepository(context, _cacheServiceMock.Object);

                tagRepositoryToTest.Create(new Tag() { Name = "testTag" });
                tagRepositoryToTest.Create(new Tag() { Name = "testTag" });

                tagRepositoryToTest.Create(new Tag() { Name = "One More Tag" });
                tagRepositoryToTest.Create(new Tag() { Name = "Another Tag" });

                var tagCount = tagRepositoryToTest.GetAll().Result.Count;

                Assert.AreEqual(3, tagCount);
            }
        }
    }
}

