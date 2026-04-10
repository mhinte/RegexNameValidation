using Core.Enums;
using Core.Models;
using Core.Providers;

namespace Core
{
    public static class NameValidator
    {
        public static Result<string, string> Validate(string? itemName, ItemType itemType, string? projectId)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return Result<string, string>.Err("[System] Der Name darf nicht null oder leer sein.");

            // Wir holen die eine zutreffende Regel
            var rule = RuleProvider.GetRule(itemType, projectId ?? string.Empty);

            // Wenn es eine Regel gibt, prüfe sie
            if (rule != null && !rule.Pattern.IsMatch(itemName))
            {
                return Result<string, string>.Err($"[{rule.Name}] {rule.ErrorMessage}");
            }

            // Wenn die Regel passt (oder es gar keine Regel für diesen Typ gibt), ist es Ok
            return Result<string, string>.Ok(itemName);
        }

        public static bool IsValid(string? itemName, ItemType itemType, string? projectId)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return false;

            var rule = RuleProvider.GetRule(itemType, projectId ?? string.Empty);

            if (rule != null && !rule.Pattern.IsMatch(itemName))
            {
                return false;
            }

            return true;
        }
    }
}