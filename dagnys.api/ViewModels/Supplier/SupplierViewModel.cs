using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnys.api.ViewModels;

public class SupplierViewModel
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactPerson { get; set; }
    public decimal Price { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
