using dagnys.api.Data;
using dagnys.api.Entities;
using dagnys.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly DataContext _context;

    public CustomersController(DataContext context)
    {
        _context = context;
    }

    // http://localhost:5000/api/customers
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerViewModel customerViewModel)
    {
        if (customerViewModel == null)
            return BadRequest("Ogiltig kunddata.");

        if (
            string.IsNullOrWhiteSpace(customerViewModel.Name)
            || string.IsNullOrWhiteSpace(customerViewModel.Phone)
            || string.IsNullOrWhiteSpace(customerViewModel.Email)
            || string.IsNullOrWhiteSpace(customerViewModel.ContactPerson)
            || string.IsNullOrWhiteSpace(customerViewModel.DeliveryAddress)
            || string.IsNullOrWhiteSpace(customerViewModel.InvoiceAddress)
        )
        {
            return BadRequest(
                "Alla fält är obligatoriska: Namn, Telefon, E-post, Kontaktperson, Leveransadress och Fakturaadress."
            );
        }

        try
        {
            var customer = new Customer
            {
                Name = customerViewModel.Name,
                Phone = customerViewModel.Phone,
                Email = customerViewModel.Email,
                ContactPerson = customerViewModel.ContactPerson,
                DeliveryAddress = customerViewModel.DeliveryAddress,
                InvoiceAddress = customerViewModel.InvoiceAddress,
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            customerViewModel.Id = customer.Id;
            return CreatedAtAction(
                nameof(GetCustomer),
                new { id = customer.Id },
                customerViewModel
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett fel uppstod när kunden skapades: {ex.Message}");
        }
    }

    // http://localhost:5000/api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
    {
        try
        {
            var customers = await _context.Customers.ToListAsync();

            if (!customers.Any())
                return NotFound("Inga kunder hittades.");

            var customerViewModels = customers
                .Select(c => new CustomerViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    Email = c.Email,
                    ContactPerson = c.ContactPerson,
                    DeliveryAddress = c.DeliveryAddress,
                    InvoiceAddress = c.InvoiceAddress,
                })
                .ToList();

            return Ok(customerViewModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett fel uppstod när kunder skulle hämtas: {ex.Message}");
        }
    }

    // http://localhost:5000/api/customers/1
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerWithOrdersViewModel>> GetCustomer(int id)
    {
        if (id <= 0)
            return BadRequest("Ogiltigt kund ID.");

        try
        {
            var customer = await _context
                .Customers.Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound($"Kund med {id} hittades inte.");

            var customerViewModel = new CustomerWithOrdersViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                ContactPerson = customer.ContactPerson,
                DeliveryAddress = customer.DeliveryAddress,
                InvoiceAddress = customer.InvoiceAddress,
                Orders = customer
                    .Orders.Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        OrderNumber = o.OrderNumber,
                        CustomerId = o.CustomerId,
                        OrderItems = o
                            .OrderItems.Select(oi => new OrderItemViewModel
                            {
                                ProductId = oi.Product.Id,
                                ProductName = oi.Product.Name,
                                Quantity = oi.Quantity,
                                TotalPrice = oi.Quantity * oi.Product.Price,
                            })
                            .ToList(),
                    })
                    .ToList(),
            };

            return Ok(customerViewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett fel uppstod när kunden skulle hämtas: {ex.Message}");
        }
    }
}
