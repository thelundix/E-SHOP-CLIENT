namespace dagnys.api.ViewModels;

public class RawMaterialViewModel
{
    public int RawMaterialId { get; set; }
    public string Name { get; set; }
    public decimal PricePerKg { get; set; }
    public List<SupplierViewModel> Suppliers { get; set; }
}
