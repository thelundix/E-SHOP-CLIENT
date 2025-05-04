using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnys.api.Entities;

public class RawMaterial
{
    public int RawMaterialId { get; set; }

    public string Name { get; set; }

    public decimal PricePerKg { get; set; }

    // Navigational property...
    public IList<SupplierRawMaterial> SupplierRawMaterials { get; set; }
}
