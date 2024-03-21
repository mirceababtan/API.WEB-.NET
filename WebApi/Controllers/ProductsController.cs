using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /* 
        - Id - Guid(must be unique)
        - Name - string
        - Description - string
        - Ratings - int[] (array of ratings between 1 and 5)
        - CreatedOn - Datetime(when it was added to the list) -> automatically generated
        */
        public static readonly List<Product> _products = [
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Bike",
                Description = "Description",
                Ratings = [4,3,2,1,5,4,1,3],
                CreatedOn = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "iPhone",
                Description = "Description2",
                Ratings = [5,4,3,3,4,5,3,3,4,4,5],
                CreatedOn = DateTime.UtcNow
            }
        ];

        [HttpGet("all")]
        public IActionResult GetAllProducts()
        {
            return Ok(_products);
        }

        [HttpPost("add-product")]
        public IActionResult AddProduct([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest("No product provided.");

            foreach (var item in _products)
            {
                if (item.Id == productDTO.Id)
                {
                    return BadRequest("Product already exists.");
                }
            }

            Product product = new()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Ratings = productDTO.Ratings,
                CreatedOn = DateTime.UtcNow
            };
            _products.Add(product);

            return Ok("Product added successfully.");
        }

        [HttpPut("modify-product")]
        public IActionResult EditProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("No product provided.");
            for (int i = 0; i < _products.Count; i++)
            {
                if (_products[i].Id == product.Id)
                {
                    _products[i] = product;
                    return Ok("Product edited successfully.");
                }
            }
            return NotFound("Product not found.");
        }

        [HttpDelete("delete-product")]

        public IActionResult DeleteProduct([FromBody] Guid productId)
        {
            if (productId == Guid.Empty)
                return NotFound("No product found.");
            foreach (var item in _products)
            {
                if (productId == item.Id)
                    _products.Remove(item);
                return Ok("Product deleted.");
            }
            return NotFound("Product not found.");
        }

        [HttpGet("filter-by-field/{keyword}")]
        public IActionResult FilteredByField(string keyword)
        {
            List<Product> foundProducts = new();
            foreach (var item in _products)
            {
                string avgRating = ArrayAverage(item.Ratings).ToString();
                if (item.Id.ToString() == keyword || item.Name == keyword || item.Description == keyword || item.CreatedOn.ToShortDateString() == keyword || avgRating == keyword)
                {
                    foundProducts.Add(item);
                }
            }
            if (foundProducts.Count > 0)
            {
                return Ok(foundProducts);
            }
            else
            {
                return NotFound("No product found that matches keyword.");
            }
        }

        [HttpGet("sort-by-rating-asc")]
        public IActionResult SortedByIncomeASC()
        {
            List<Product> sortedProducts = _products;


            /*sortedStores.Sort((p, q) => p.MonthlyIncome.CompareTo(q.MonthlyIncome));*/
            for (int i = 0; i < sortedProducts.Count - 1; i++)
                for (int j = i + 1; j < sortedProducts.Count; j++)
                {
                    if (ArrayAverage(sortedProducts[i].Ratings) > ArrayAverage(sortedProducts[j].Ratings))
                    {
                        Product auxProduct;
                        auxProduct = sortedProducts[i];
                        sortedProducts[i] = sortedProducts[j];
                        sortedProducts[j] = auxProduct;
                    }
                }

            return Ok(sortedProducts);
        }
        [HttpGet("sort-by-rating-desc")]
        public IActionResult SortedByIncomeDESC()
        {
            List<Product> sortedProducts = _products;

            for (int i = 0; i < sortedProducts.Count - 1; i++)
                for (int j = i + 1; j < sortedProducts.Count; j++)
                {
                    if (ArrayAverage(sortedProducts[i].Ratings) < ArrayAverage(sortedProducts[j].Ratings))
                    {
                        Product auxProduct;
                        auxProduct = sortedProducts[i];
                        sortedProducts[i] = sortedProducts[j];
                        sortedProducts[j] = auxProduct;
                    }
                }

            return Ok(sortedProducts);
        }

        [HttpGet("get-oldest-product")]
        public IActionResult GetOldestProduct()
        {
            Product oldestProduct = _products[0];

            for (int i = 1; i < _products.Count; i++)
            {
                if (DateTime.Compare(oldestProduct.CreatedOn, _products[i].CreatedOn) > 0)
                {
                    oldestProduct = _products[i];
                }
            }
            return Ok(oldestProduct);
        }

        [HttpGet("get-newest-product")]
        public IActionResult GetOldestNewest()
        {
            Product newestProduct = _products[0];

            for (int i = 1; i < _products.Count; i++)
            {
                if (DateTime.Compare(newestProduct.CreatedOn, _products[i].CreatedOn) < 0)
                {
                    newestProduct = _products[i];
                }
            }
            return Ok(newestProduct);
        }

        static double ArrayAverage(int[] array)
        {
            double sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum / array.Length;
        }
    }
}
