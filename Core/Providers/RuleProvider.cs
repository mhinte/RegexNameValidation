using System.Text.RegularExpressions;
using Core.Enums;
using Core.Models;

namespace Core.Providers
{
    internal static class RuleProvider
    {
        // Globale Regeln
        private static readonly Dictionary<ItemType, ValidationRule> GlobalRules = new()
        {
            {
                ItemType.Rohr,
                new ValidationRule(
                    Name: "Rohr-Konvention",
                    ErrorMessage: "Rohre müssen mit 'RO-' beginnen, gefolgt von 4 Ziffern.",
                    Pattern: new Regex(@"^RO-\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
            },
            {
                ItemType.Blech,
                new ValidationRule(
                    Name: "Blech-Format",
                    ErrorMessage: "Bleche müssen das Format 'BL-Länge-Breite' haben (z.B. BL-100-50).",
                    Pattern: new Regex(@"^BL-\d+-\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
            }
        };

        // Projektspezifische Ausnahmen (Ignoriert Groß-/Kleinschreibung bei der ProjectId)
        private static readonly Dictionary<string, Dictionary<ItemType, ValidationRule>> ProjectRules =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["13579"] = new Dictionary<ItemType, ValidationRule>
                {
                    {
                        ItemType.Rohr, 
                        new ValidationRule(
                            Name: "Sonderprojekt-Rohr",
                            ErrorMessage: "Im Sonderprojekt müssen Rohre mit 'SP-RO-' beginnen.",
                            Pattern: new Regex(@"^SP-RO-\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
                    }
                }
            };

        /// <summary>
        /// Holt die passende Regel. Projektregel hat Vorrang vor der globalen Regel.
        /// Gibt null zurück, wenn gar keine Regel existiert.
        /// </summary>
        public static ValidationRule? GetRule(ItemType itemType, string projectId)
        {
            if (!string.IsNullOrWhiteSpace(projectId) && 
                ProjectRules.TryGetValue(projectId, out var projectSpecificRules) &&
                projectSpecificRules.TryGetValue(itemType, out var rule))
            {
                return rule;
            }

            // 2. Fallback auf globale Regel
            if (GlobalRules.TryGetValue(itemType, out var globalRule))
            {
                return globalRule;
            }

            // 3. Keine Regel definiert -> null
            return null;
        }
    }
}