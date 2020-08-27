using SuperSaiyanSearch.Domain.Interfaces;
using Xunit;

namespace SuperSaiyanSearch.Integration.Tests
{
    public class TakealotStoreSiteUnitTest
    {
        private readonly IStoreSite _takealotStoreSite;

        public TakealotStoreSiteUnitTest()
        {
           _takealotStoreSite = new TakealotStoreSite();
        }
        [Fact]
        public void GivenUrlWhenSearchingScrapThenReturnSiteContent()
        {
            var results = _takealotStoreSite.Search("laptop");
            Assert.NotNull(results);
        }
    }
}