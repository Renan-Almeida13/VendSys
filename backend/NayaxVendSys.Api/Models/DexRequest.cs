using System.ComponentModel.DataAnnotations;

namespace NayaxVendSys.Api.Models;

public class DexRequest
{
    [Required]
    [RegularExpression("^[AB]$", ErrorMessage = "Machine must be either 'A' or 'B'")]
    public string Machine { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "DEX content cannot be empty")]
    public string DexContent { get; set; }
} 