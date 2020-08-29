using System.Linq;
using SuperSaiyanSearch.Domain.Interfaces;
using Xunit;

namespace SuperSaiyanSearch.Integration.Tests
{
    public class MakroStoreSiteUnitTest
    {
        private readonly IStoreSite _makroStoreSite;

        public MakroStoreSiteUnitTest()
        {
           _makroStoreSite = new MakroStoreSite(new WebScrapper());
        }
        [Fact]
        public void GivenUrlWhenSearchingScrapThenReturnSiteContent()
        {
            var results = _makroStoreSite.Search("laptop");
            Assert.Equal(true, results.All(r => r.Price > 0));
            Assert.NotNull(results);
        }
    }
}