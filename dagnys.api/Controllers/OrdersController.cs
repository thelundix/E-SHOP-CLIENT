using dagnys.api.Data;
using dagnys.api.Entities;
using dagnys.api.Services;
using dagnys.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IOrderService _orderService;

    public OrdersController(DataContext context, IOrderService orderService)
    {
        _context = context;
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    // http://localhost:5000/api/orders
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel orderViewModel)
    {
        if (
            orderViewModel == null
            || orderViewModel.CustomerId <= 0
            || orderViewModel.OrderItems == null
            || !orderViewModel.OrderItems.Any()
        )
        {
            return BadRequest("Alla fält är obligatoriska: CustomerId och minst en OrderItem.");
        }

        try
        {
            // Sätt OrderDate till dagens datum om det inte har skickats med
            var orderDate =
                orderViewModel.OrderDate == default ? DateTime.Now : orderViewModel.OrderDate;

            var order = new Order
            {
                OrderDate = orderDate,

                OrderNumber = _orderService.GenerateOrderNumber(),
                CustomerId = orderViewModel.CustomerId,
                OrderItems = orderViewModel
                    .OrderItems.Select(oi => new OrderItem
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                    })
                    .ToList(),
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            orderViewModel.Id = order.Id;
            orderViewModel.OrderNumber = order.OrderNumber;

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderViewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett fel uppstod när beställningen skapades: {ex.Message}");
        }
    }

    // http://localhost:5000/api/orders/2
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderViewModel>> GetOrder(int id)
    {
        if (id <= 0)
            return BadRequest("Fel order ID.");

        try
        {
            var order = await _context
                .Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound($"Order med ID {id} hittades inte.");

            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                OrderItems = order
                    .OrderItems.Select(oi => new OrderItemViewModel
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        TotalPrice = oi.Quantity * oi.Product.Price,
                    })
                    .ToList(),
            };

            return Ok(orderViewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                $"Ett fel uppstod när beställningen skulle hämtas: {ex.Message}"
            );
        }
    }

    // Hämta och filtrera beställningar baserat på ordernummer och/eller datum
    // http://localhost:5000/api/orders
    // http://localhost:5000/api/orders?orderNumber=ORD-20250218123644-3c5da24e0bbfa89f9e8ecaef24
    //  http://localhost:5000/api/orders?orderDate=2025-02-13
    // http://localhost:5000/api/orders?orderNumber=ORD-20250218123644-3c5da24e0bbfa89f9e8ecaef24&orderDate=2025-02-13

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOrders(
        [FromQuery] string orderNumber,
        [FromQuery] DateTime? orderDate
    )
    {
        try
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(orderNumber))
            {
                query = query.Where(o => o.OrderNumber.Contains(orderNumber));
            }

            if (orderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date == orderDate.Value.Date);
            }

            var orders = await query
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            if (!orders.Any())
                return NotFound("Inga beställningar hittades som matchade kriterierna.");

            var orderViewModels = orders
                .Select(order => new OrderViewModel
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    OrderNumber = order.OrderNumber,
                    CustomerId = order.CustomerId,
                    OrderItems = order
                        .OrderItems.Select(oi => new OrderItemViewModel
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.Product.Name,
                            Quantity = oi.Quantity,
                            TotalPrice = oi.Quantity * oi.Product.Price,
                        })
                        .ToList(),
                })
                .ToList();

            return Ok(orderViewModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett fel uppstod vid hämtning av beställningar: {ex.Message}");
        }
    }
}
