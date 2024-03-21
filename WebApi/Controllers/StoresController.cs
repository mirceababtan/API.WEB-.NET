using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        public static readonly List<Store> _stores = new List<Store>{
            new Store
            {
                Id = Guid.NewGuid(),
                Name = "name 1",
                Country = "romania",
                City = "Oradea",
                MonthlyIncome = 123,
                OwnerName = "Mircea",
                ActiveSince = DateTime.UtcNow,
            },
            new Store
            {
                Id = Guid.NewGuid(),
                Name = "name 2",
                Country = "romania",
                City = "Cluj",
                MonthlyIncome = 14213,
                OwnerName = "Marcel",
                ActiveSince = DateTime.UtcNow,
            },
            new Store
            {
                Id = Guid.NewGuid(),
                Name = "name 3",
                Country = "England",
                City = "Bucuresti",
                MonthlyIncome = 4213,
                OwnerName = "Danut",
                ActiveSince = DateTime.UtcNow,
            }
        };

        [HttpGet]
        public Store[] GetAllStores()
        {
            return _stores.ToArray();
        }

        [HttpPost]
        public IActionResult CreateStore([FromBody] Store store)
        {
            if (store == null)
                return BadRequest("Store is null.");

            foreach (var existingStore in _stores)
            {
                if (store.Id == existingStore.Id)
                {
                    return BadRequest("Store with the same Id already exists.");
                }
            }
            _stores.Add(store);
            return Ok(store);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStore(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Store is null.");

            foreach (var existingStore in _stores)
            {
                if (id == existingStore.Id)
                {
                    _stores.Remove(existingStore);
                    return Ok("Store deleted.");
                }
            }
            return NotFound("Store not found!");
        }

        [HttpPut("transfer-ownership/{storeId}")]
        public IActionResult TransferOwnership(Guid storeId, [FromBody] string name)
        {
            foreach (var existingStore in _stores)
            {
                if (storeId == existingStore.Id)
                {
                    existingStore.OwnerName = name;
                    return Ok("Ownership changed.");
                }
            }
            return BadRequest();
        }

        [HttpGet("store-by-key/{keyword}")]
        public IActionResult StoreByKey(string keyword)
        {
            List<Store> filteredStores = new();
            if (keyword == null)
                return BadRequest("Keyword not provided.");
            foreach (var store in _stores)
            {
                if (store.Name == keyword || store.OwnerName == keyword)
                {
                    filteredStores.Add(store);
                }
            }
            if (filteredStores.Count == 0)
                return NotFound("No Stores Found.");
            else
                return Ok(filteredStores);
        }

        [HttpGet("store-by-country/{country}")]
        public IActionResult StoreByCountry(string country)
        {
            List<Store> storesByCountry = new();
            if (country == null) return BadRequest();

            foreach (var existingStore in _stores)
            {
                if (existingStore.Country.ToLower() == country.ToLower())
                    storesByCountry.Add(existingStore);
            }
            if (storesByCountry.Count == 0)
                return NotFound("No stores found");
            else
                return Ok(storesByCountry);
        }

        [HttpGet("store-by-city/{city}")]
        public IActionResult StoreByCity(string city)
        {
            List<Store> storesByCity = new();
            if (city == null)
                return BadRequest();

            foreach (var existingStore in _stores)
            {
                if (existingStore.City.ToLower() == city.ToLower())
                    storesByCity.Add(existingStore);
            }
            if (storesByCity.Count == 0)
                return NotFound("No stores found");
            else
                return Ok(storesByCity);
        }

        [HttpGet("sorted-by-income")]
        public IActionResult SortedByIncome()
        {
            List<Store> sortedStores = _stores;


            /*sortedStores.Sort((p, q) => p.MonthlyIncome.CompareTo(q.MonthlyIncome));*/
            for (int i = 0; i < sortedStores.Count - 1; i++)
                for (int j = i + 1; j < sortedStores.Count; j++)
                {
                    if (sortedStores[i].MonthlyIncome > sortedStores[j].MonthlyIncome)
                    {
                        Store auxStore;
                        auxStore = sortedStores[i];
                        sortedStores[i] = sortedStores[j];
                        sortedStores[j] = auxStore;
                    }
                }

            return Ok(sortedStores);
        }

        [HttpGet("oldest-store")]
        public Store OldestStore()
        {
            var store = _stores[0];

            for (int i = 1; i < _stores.Count; i++)
            {
                if (DateTime.Compare(store.ActiveSince, _stores[i].ActiveSince) > 0)
                {
                    store = _stores[i];
                }
            }

            return store;
        }
    }
}
