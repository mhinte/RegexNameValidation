using Core.Enums;
using Core.Models;
using Core.Providers;

namespace Core
{
    public static class NameValidator
    {
        /// <summary>
        /// Prüft den Namen und liefert im Fehlerfall genaue Details zur verletzten Regel zurück.
        /// </summary>
        public static Result<string, string> Validate(string? itemName, ItemType itemType, string? projectId)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return Result<string, string>.Err("[System] Der Name darf nicht null oder leer sein.");

            var rules = RuleProvider.GetRules(itemType, projectId ?? string.Empty);

            foreach (var rule in rules)
            {
                if (!rule.Pattern.IsMatch(itemName))
                {
                    return Result<string, string>.Err($"[{rule.Name}] {rule.ErrorMessage}");
                }
            }

            return Result<string, string>.Ok(itemName);
        }

        public static bool IsValid(string? itemName, ItemType itemType, string? projectId)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return false;

            var rules = RuleProvider.GetRules(itemType, projectId ?? string.Empty);

            foreach (var rule in rules)
            {
                if (!rule.Pattern.IsMatch(itemName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}