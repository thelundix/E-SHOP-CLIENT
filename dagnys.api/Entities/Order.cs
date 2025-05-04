using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnys.api.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
