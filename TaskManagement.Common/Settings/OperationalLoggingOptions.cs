using TaskManagement.Common.Helpers;

namespace TaskManagement.Common.Settings;

public sealed class OperationalLoggingOptions
{
    public const string SectionName = "OperationalLogging";

    public int QueueCapacity { get; set; } = 10000;
    public int SqlBatchSize { get; set; } = 50;
    public string FallbackFilePath { get; set; } = "logs/operational-fallback-.log";
    public int FallbackRetainedFileCount { get; set; } = 7;

    public void Validate()
    {
        if (QueueCapacity < 1)
            throw new InvalidOperationException("OperationalLogging:QueueCapacity must be at least 1.");

        if (SqlBatchSize < 1)
            throw new InvalidOperationException("OperationalLogging:SqlBatchSize must be at least 1.");

        if (FallbackFilePath.IsNullParameter())
            throw new InvalidOperationException("OperationalLogging:FallbackFilePath is required.");

        if (FallbackRetainedFileCount < 1)
            throw new InvalidOperationException("OperationalLogging:FallbackRetainedFileCount must be at least 1.");
    }
}
