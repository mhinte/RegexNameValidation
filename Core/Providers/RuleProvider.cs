using System.Text.RegularExpressions;
using Core.Enums;
using Core.Models;

namespace Core.Providers
{
    internal static class RuleProvider
    {
        // Globale Regeln
        private static readonly Dictionary<ItemType, List<ValidationRule>> GlobalRules = new()
        {
            [ItemType.Rohr] = new List<ValidationRule>
            {
                new ValidationRule("Prefix", "Muss mit 'RO-' beginnen.", new Regex(@"^RO-", RegexOptions.Compiled)),
                new ValidationRule("Länge", "Muss mindestens 7 Zeichen lang sein.", new Regex(@"^.{7,}$", RegexOptions.Compiled))
            }
        };

        // Projektspezifische Ausnahmen (Ignoriert Groß-/Kleinschreibung bei der ProjectId)
        private static readonly Dictionary<string, Dictionary<ItemType, List<ValidationRule>>> ProjectRules = new(StringComparer.OrdinalIgnoreCase)
        {
            ["13579"] = new Dictionary<ItemType, List<ValidationRule>>
            {
                [ItemType.Rohr] = new List<ValidationRule>
                {
                    // Im Projekt 13579 müssen Rohre z.B. anders heißen
                    new ValidationRule("Prefix", "Muss mit 'RO-Spezial-' beginnen.", new Regex(@"^RO-Spezial-", RegexOptions.Compiled))
                }
            }
        };

        public static IReadOnlyList<ValidationRule> GetRules(ItemType itemType, string projectId)
        {
            // 1. Prüfen, ob es für dieses Projekt eine spezielle Regel für den ItemType gibt
            if (!string.IsNullOrWhiteSpace(projectId) && 
                ProjectRules.TryGetValue(projectId, out var projectSpecificRules) &&
                projectSpecificRules.TryGetValue(itemType, out var rules))
            {
                return rules;
            }

            // 2. Fallback auf globale Regeln
            if (GlobalRules.TryGetValue(itemType, out var globalRules))
            {
                return globalRules;
            }

            // 3. Wenn keine Regeln definiert sind, geben wir eine leere Liste zurück
            return Array.Empty<ValidationRule>();
        }
    }
}
