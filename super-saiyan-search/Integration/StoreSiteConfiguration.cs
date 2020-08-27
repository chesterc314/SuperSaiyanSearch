
using System.Collections.Generic;
using SuperSaiyanSearch.Domain.Interfaces;
using SuperSaiyanSearch.Integration.Interfaces;

namespace SuperSaiyanSearch.Integration
{
    public class StoreSiteConfiguration : IStoreSiteConfiguration
    {
        private readonly IWebScrapper _webScrapper;

        public StoreSiteConfiguration(IWebScrapper webScrapper)
        {
            _webScrapper = webScrapper;
        }
        public IEnumerable<IStoreSite> StoreSites
        {
            get
            {
                return new List<IStoreSite> { new TakealotStoreSite(), new GameStoreSite(_webScrapper), new IncredibleConnectionStoreSite(_webScrapper) };
            }
        }
    }
}