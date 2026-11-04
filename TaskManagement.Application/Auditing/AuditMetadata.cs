using System.Text.Json;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.LogEntities;

namespace TaskManagement.Application.Auditing;

public static class AuditMetadata
{
    public const int MaxEntries = 20;
    public const int MaxKeyLength = 50;
    public const int MaxValueLength = 100;

    private static readonly string[] ForbiddenFragments =
    [
        "password",
        "token",
        "secret",
        "authorization",
        "cookie",
        "apikey",
        "connectionstring"
    ];

    public static IReadOnlyDictionary<string, string>? Pairs(params (string Key, string Value)[] pairs)
    {
        if (pairs.Length == 0)
            return null;

        var metadata = new Dictionary<string, string>(pairs.Length, StringComparer.Ordinal);
        foreach (var (key, value) in pairs)
        {
            if (!metadata.TryAdd(key, value))
                throw new Exception($"Duplicate audit metadata key '{key}'. In {nameof(AuditMetadata)}.{nameof(pairs)} Method!");
        }

        return metadata;
    }
    public static string? Serialize(IReadOnlyDictionary<string, string>? metadata)
    {
        if (metadata.IsNullParameter() || metadata!.Count == 0)
            return null;

        if (metadata.Count > MaxEntries)
            throw new Exception($"Audit metadata accepts at most {MaxEntries} entries. In {nameof(AuditMetadata)}.{nameof(Serialize)} Method!");

        var sanitized = new Dictionary<string, string>(metadata.Count, StringComparer.Ordinal);
        foreach (var (key, value) in metadata)
        {
            if (key.IsNullParameter() || key.Length > MaxKeyLength)
                throw new Exception($"Audit metadata key '{key}' is empty or longer than {MaxKeyLength} characters. In {nameof(AuditMetadata)}.{nameof(Serialize)} Method!");

            if (value.IsNullParameter() || value.Length > MaxValueLength)
                throw new Exception($"Audit metadata value for '{key}' is missing or longer than {MaxValueLength} characters. In {nameof(AuditMetadata)}.{nameof(Serialize)} Method!");

            var normalized = new string(key.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
            if (ForbiddenFragments.Any(fragment => normalized.Contains(fragment, StringComparison.Ordinal)))
                throw new Exception($"Audit metadata key '{key}' is not allowed. In {nameof(AuditMetadata)}.{nameof(Serialize)} Method!");

            sanitized.Add(key, value);
        }

        var json = JsonSerializer.Serialize(sanitized);
        if (json.Length > AuditLog.MetadataMaxLength)
            throw new Exception($"Audit metadata must be at most {AuditLog.MetadataMaxLength} characters. In {nameof(AuditMetadata)}.{nameof(Serialize)} Method!");

        return json;
    }
}
