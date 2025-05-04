using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnys.api.ViewModels;

public class OrderViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; } = new();
}
