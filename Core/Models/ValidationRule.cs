using System.Text.RegularExpressions;

namespace Core.Models
{
    public record ValidationRule(string Name, string ErrorMessage, Regex Pattern);
}