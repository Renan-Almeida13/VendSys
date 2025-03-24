
namespace NayaxVendSys.Api.Models
{
    public class DexMeter
    {
        public int Id { get; set; }
        public string Machine { get; set; }
        public DateTime DexDateTime { get; set; }
        public string MachineSerialNumber { get; set; }
        public decimal ValueOfPaidVends { get; set; }
    }
} 