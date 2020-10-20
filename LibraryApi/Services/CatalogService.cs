using LibraryApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
	public class CatalogService : ICacheTheCatalog
	{
		private readonly IDistributedCache _cache;

		public CatalogService(IDistributedCache cache)
		{
			_cache = cache;
		}

		public async Task<CatalogModel> GetCatalogAsync()
		{
			// Ask the cache for a thing
			var catalog = await _cache.GetAsync("catalog");
			string result = null;

			// If it is in the catalog, Use the cached catalog.
			if (catalog != null)
			{
				result = Encoding.UTF8.GetString(catalog);
			}
			// If it isn't:
			else
			{
				// 1.) build the thing
				result = $"this catalog was created at {DateTime.Now.ToLongTimeString()}";
				var encodedCatalog = Encoding.UTF8.GetBytes(result);
				var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
				await _cache.SetAsync("catalog", encodedCatalog, options);
			}


			// 3.) return the thing
			return new CatalogModel { Data = result };
		}
	}
}
