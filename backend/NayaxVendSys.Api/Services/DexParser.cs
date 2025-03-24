using NayaxVendSys.Api.Models;

namespace NayaxVendSys.Api.Services
{
    public class DexParser
    {
        public DexMeter ParseDexMeter(string dexContent, string machine)
        {
            var lines = dexContent.Split('\n');
            var dexMeter = new DexMeter
            {
                Machine = machine,
                DexDateTime = DateTime.Now
            };

            foreach (var line in lines)
            {
                var fields = line.Split('*');
                if (fields.Length < 2) continue;

                switch (fields[0])
                {
                    case "ID1":
                        dexMeter.MachineSerialNumber = fields[1];
                        break;
                    case "VA1":
                        if (decimal.TryParse(fields[1], out decimal value))
                        {
                            dexMeter.ValueOfPaidVends = value / 100;
                        }
                        break;
                }
            }

            return dexMeter;
        }

        public List<DexLaneMeter> ParseDexLaneMeters(string dexContent, int dexMeterId)
        {
            var lines = dexContent.Split('\n');
            var laneMeters = new List<DexLaneMeter>();
            var currentLaneMeter = new DexLaneMeter { DexMeterId = dexMeterId };

            foreach (var line in lines)
            {
                var fields = line.Split('*');
                if (fields.Length < 2) continue;

                switch (fields[0])
                {
                    case "PA1":
                        if (currentLaneMeter.ProductIdentifier != null)
                        {
                            laneMeters.Add(currentLaneMeter);
                        }
                        currentLaneMeter = new DexLaneMeter { DexMeterId = dexMeterId };
                        if (fields.Length > 1)
                        {
                            currentLaneMeter.ProductIdentifier = fields[1];
                            if (decimal.TryParse(fields[2], out decimal price))
                            {
                                currentLaneMeter.Price = price / 100;
                            }
                        }
                        break;
                    case "PA2":
                        if (fields.Length > 1)
                        {
                            if (int.TryParse(fields[1], out int vends))
                            {
                                currentLaneMeter.NumberOfVends = vends;
                            }
                            if (decimal.TryParse(fields[2], out decimal sales))
                            {
                                currentLaneMeter.ValueOfPaidSales = sales / 100;
                            }
                        }
                        break;
                }
            }

            if (currentLaneMeter.ProductIdentifier != null)
            {
                laneMeters.Add(currentLaneMeter);
            }

            return laneMeters;
        }
    }
} 