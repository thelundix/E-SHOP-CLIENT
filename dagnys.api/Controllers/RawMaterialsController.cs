using dagnys.api.Data;
using dagnys.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RawMaterialsController : ControllerBase
{
    private readonly DataContext _context;

    public RawMaterialsController(DataContext context)
    {
        _context = context;
    }

    // http://localhost:5018/api/rawmaterials

    [HttpGet]
    public async Task<ActionResult> GetAllRawMaterials()
    {
        try
        {
            var rawMaterials = await _context
                .RawMaterials.Include(r => r.SupplierRawMaterials)
                .ThenInclude(srm => srm.Supplier)
                .Select(r => new RawMaterialViewModel
                {
                    RawMaterialId = r.RawMaterialId,
                    Name = r.Name,
                    PricePerKg = r.PricePerKg,
                    Suppliers = r
                        .SupplierRawMaterials.Select(srm => new SupplierViewModel
                        {
                            SupplierId = srm.SupplierId,
                            Name = srm.Supplier.Name,
                            Price = srm.Price,
                        })
                        .ToList(),
                })
                .ToListAsync();

            return Ok(new { sucess = true, data = rawMaterials });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }

    // http://localhost:5018/api/rawmaterials/1

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRawMaterialById(int id)
    {
        try
        {
            var rawMaterial = await _context
                .RawMaterials.Include(r => r.SupplierRawMaterials)
                .ThenInclude(srm => srm.Supplier)
                .Where(r => r.RawMaterialId == id)
                .Select(r => new RawMaterialViewModel
                {
                    RawMaterialId = r.RawMaterialId,
                    Name = r.Name,
                    PricePerKg = r.PricePerKg,
                    Suppliers = r
                        .SupplierRawMaterials.Select(srm => new SupplierViewModel
                        {
                            SupplierId = srm.SupplierId,
                            Name = srm.Supplier.Name,
                            Price = srm.Price,
                        })
                        .ToList(),
                })
                .FirstOrDefaultAsync();

            if (rawMaterial == null)
            {
                return NotFound(
                    new
                    {
                        error = "Raw material not found",
                        details = $"No raw material with ID {id} exists in the database.",
                    }
                );
            }

            return Ok(new { sucess = true, data = rawMaterial });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }
}
