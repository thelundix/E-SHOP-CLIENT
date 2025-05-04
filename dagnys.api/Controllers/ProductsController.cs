using dagnys.api.Data;
using dagnys.api.Entities;
using dagnys.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly DataContext _context;

    public ProductsController(DataContext context)
    {
        _context = context;
    }

    // http://localhost:5000/api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductViewModel productViewModel)
    {
        try
        {
            if (
                productViewModel == null
                || string.IsNullOrWhiteSpace(productViewModel.Name)
                || productViewModel.Price <= 0
                || productViewModel.Weight <= 0
                || productViewModel.PackageQuantity <= 0
                || productViewModel.ExpiryDate == default
                || productViewModel.ManufactureDate == default
            )
            {
                return BadRequest("Alla fält är obligatoriska och måste ha giltiga värden.");
            }

            var product = new Product
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Weight = productViewModel.Weight,
                PackageQuantity = productViewModel.PackageQuantity,
                ExpiryDate = productViewModel.ExpiryDate,
                ManufactureDate = productViewModel.ManufactureDate,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productViewModel.Id = product.Id;
            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, productViewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett internt serverfel uppstod: {ex.Message}");
        }
    }

    // http://localhost:5000/api/products

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
    {
        try
        {
            var products = await _context.Products.ToListAsync();

            var productViewModels = products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Weight = p.Weight,
                    PackageQuantity = p.PackageQuantity,
                    ExpiryDate = p.ExpiryDate,
                    ManufactureDate = p.ManufactureDate,
                })
                .ToList();

            return Ok(productViewModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett internt serverfel uppstod: {ex.Message}");
        }
    }

    // http://localhost:5000/api/products/3
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductViewModel>> GetProductById(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound($"Produkten med ID {id} hittades inte.");

            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Weight = product.Weight,
                PackageQuantity = product.PackageQuantity,
                ExpiryDate = product.ExpiryDate,
                ManufactureDate = product.ManufactureDate,
            };

            return Ok(productViewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett internt serverfel uppstod: {ex.Message}");
        }
    }

    // http://localhost:5000/api/products/3/price
    [HttpPut("{id}/price")]
    public async Task<IActionResult> UpdatePrice(int id, [FromBody] decimal newPrice)
    {
        try
        {
            if (newPrice <= 0)
                return BadRequest("Priset måste vara större än 0.");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound($"Produkten med ID {id} hittades inte.");

            product.Price = newPrice;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett internt serverfel uppstod: {ex.Message}");
        }
    }
}
