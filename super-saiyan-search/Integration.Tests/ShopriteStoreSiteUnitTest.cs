using System.Linq;
using SuperSaiyanSearch.Domain.Interfaces;
using Xunit;

namespace SuperSaiyanSearch.Integration.Tests
{
    public class ShopriteStoreSiteUnitTest
    {
        private readonly IStoreSite _shopriteStoreSite;

        public ShopriteStoreSiteUnitTest()
        {
           _shopriteStoreSite = new ShopriteStoreSite(new WebScrapper());
        }
        [Fact]
        public void GivenUrlWhenSearchingScrapThenReturnSiteContent()
        {
            var results = _shopriteStoreSite.Search("bread");
            Assert.Equal(true, results.All(r => r.Price > 0));
            Assert.NotNull(results);
        }
    }
}