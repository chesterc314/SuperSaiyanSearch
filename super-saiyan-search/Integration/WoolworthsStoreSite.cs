using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSaiyanSearch.Domain;
using SuperSaiyanSearch.Domain.Interfaces;
using SuperSaiyanSearch.Integration.Interfaces;

namespace SuperSaiyanSearch.Integration
{
    public class WoolworthsStoreSite : IStoreSite
    {
        private readonly IHttpClient _httpClient;

        public WoolworthsStoreSite(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IEnumerable<IProduct> Search(string keyword)
        {
            var headers = new List<KeyValuePair<string, string>>();
            var host = "www.woolworths.co.za";
            headers.Add(KeyValuePair.Create("Referer", $"https://{host}/cat?Ntt={keyword}&Dy=1"));
            headers.Add(KeyValuePair.Create("Host", host));
            var response = _httpClient.Get($"https://{host}/server/searchCategory?pageURL=%2Fcat&Ntt={keyword}&Dy=1", headers);
            
            var records = new JArray();
            try
            {
                var parent = JObject.Parse(response.Content);
                var contents = parent.Value<JArray>("contents");
                var mainContent = contents[0].Value<JArray>("mainContent");
                var contents2 = mainContent[0].Value<JArray>("contents");
                records = contents2[0].Value<JArray>("records");
            }
            catch (InvalidCastException ex)
            {
                if (ex.Message.Equals("Cannot cast Newtonsoft.Json.Linq.JObject to Newtonsoft.Json.Linq.JToken."))
                {
                    records = new JArray();
                }
                else
                {
                    throw ex;
                }
            }
            catch (JsonReaderException)
            {
                return new List<Product>();
            }

            var resultProducts = new List<Product>();
            if (records.Count > 0)
            {
                var index = 0;
                foreach (var result in records)
                {
                    var imageUrl = result.SelectToken("attributes.p_imageReference").Value<string>();
                    var sourceUrl = result.SelectToken("attributes.detailPageURL").Value<string>();
                    var displayName = result.SelectToken("attributes.p_displayName");
                    var title = result.SelectToken("attributes.p_title");
                    var name = (displayName ?? title).Value<string>();
                    var startingPrice = result.SelectToken("startingPrice.p_pl00");
                    var price = startingPrice != null ? startingPrice.Value<decimal>() : 0;

                    resultProducts.Add(new Product
                    {
                        Name = name,
                        Description = name,
                        Price = price,
                        Brand = null,
                        Source = StoreSiteName.Woolworths.ToString(),
                        SourceUrl = $"https://{host}{sourceUrl}",
                        ImageUrl = $"https://{host}{imageUrl}",
                        Units = 1,
                        Order = index
                    });
                    ++index;
                }
            }

            return resultProducts;
        }
    }
}