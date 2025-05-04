using dagnys.api.Data;
using dagnys.api.Entities;
using dagnys.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly DataContext _context;

    public SuppliersController(DataContext context)
    {
        _context = context;
    }

    // http://localhost:5000/api/suppliers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierViewModel>>> GetSupplier()
    {
        try
        {
            var suppliers = await _context.Suppliers.ToListAsync();

            if (!suppliers.Any())
                return NotFound("Inga kunder hittades.");

            var supplierViewModels = suppliers
                .Select(s => new SupplierViewModel
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    Address = s.Address,
                    ContactPerson = s.ContactPerson,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email,
                })
                .ToList();

            return Ok(supplierViewModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett fel uppstod när kunder skulle hämtas: {ex.Message}");
        }
    }
}
