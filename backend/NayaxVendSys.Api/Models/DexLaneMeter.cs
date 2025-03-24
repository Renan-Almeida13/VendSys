
namespace NayaxVendSys.Api.Models
{
    public class DexLaneMeter
    {
        public int Id { get; set; }
        public int DexMeterId { get; set; }
        public string ProductIdentifier { get; set; }
        public decimal Price { get; set; }
        public int NumberOfVends { get; set; }
        public decimal ValueOfPaidSales { get; set; }
        
        public DexMeter DexMeter { get; set; }
    }
} 