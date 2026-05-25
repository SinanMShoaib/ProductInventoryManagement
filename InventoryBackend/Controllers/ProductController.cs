using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using InventoryBackend.Models;

namespace InventoryBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductsController()
        {
            // Pointing directly to your local MongoDB Compass instance
            var mongoClient = new MongoClient("mongodb://localhost:27017"); 
            var database = mongoClient.GetDatabase("InventoryDB");
            _productsCollection = database.GetCollection<Product>("Products");
        }

        // 1. GET: Fetch all products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get() =>
            await _productsCollection.Find(_ => true).ToListAsync();

        // 2. GET: Fetch single product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id) =>
            await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // 3. POST: Add a new product
        [HttpPost]
        public async Task<IActionResult> Post(Product newProduct)
        {
            await _productsCollection.InsertOneAsync(newProduct);
            return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
        }

        // 4. PUT: Update an existing product
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Product updatedProduct)
        {
            await _productsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);
            return NoContent();
        }

        // 5. DELETE: Remove a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productsCollection.DeleteOneAsync(x => x.Id == id);
            return NoContent();
        }
    }
}