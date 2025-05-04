namespace dagnys.api.Entities;

public class SupplierRawMaterial
{
    public int SupplierRawMaterialId { get; set; }

    public int SupplierId { get; set; }

    public int RawMaterialId { get; set; }

    public decimal Price { get; set; }

    public Supplier Supplier { get; set; }

    public RawMaterial RawMaterial { get; set; }
}
