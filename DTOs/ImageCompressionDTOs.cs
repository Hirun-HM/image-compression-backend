using System.ComponentModel.DataAnnotations;

namespace ImageCompressionAPI.DTOs;

/// <summary>
/// Request DTO for image compression
/// </summary>
public class CompressImageRequest
{
    /// <summary>
    /// Target quality (1-100)
    /// </summary>
    [Range(1, 100, ErrorMessage = "Quality must be between 1 and 100")]
    public int Quality { get; set; } = 80;
    
    /// <summary>
    /// Maximum width for resizing
    /// </summary>
    [Range(1, 8000, ErrorMessage = "Max width must be between 1 and 8000 pixels")]
    public int? MaxWidth { get; set; }
    
    /// <summary>
    /// Maximum height for resizing
    /// </summary>
    [Range(1, 8000, ErrorMessage = "Max height must be between 1 and 8000 pixels")]
    public int? MaxHeight { get; set; }
    
    /// <summary>
    /// Output format
    /// </summary>
    [RegularExpression("^(jpeg|jpg|png|webp)$", ErrorMessage = "Output format must be jpeg, jpg, png, or webp")]
    public string OutputFormat { get; set; } = "jpeg";
    
    /// <summary>
    /// Whether to use ML-based optimization
    /// </summary>
    public bool UseMLOptimization { get; set; } = true;
    
    /// <summary>
    /// Whether to preserve original metadata
    /// </summary>
    public bool PreserveMetadata { get; set; } = false;
}

/// <summary>
/// Response DTO for image compression
/// </summary>
public class CompressImageResponse
{
    /// <summary>
    /// Operation success status
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Error message if operation failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Compression result details
    /// </summary>
    public CompressionResultDto? Result { get; set; }
}

/// <summary>
/// DTO for compression result
/// </summary>
public class CompressionResultDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Original file information
    /// </summary>
    public FileInfoDto OriginalFile { get; set; } = new();
    
    /// <summary>
    /// Compressed file information
    /// </summary>
    public FileInfoDto CompressedFile { get; set; } = new();
    
    /// <summary>
    /// Compression statistics
    /// </summary>
    public CompressionStatsDto Stats { get; set; } = new();
    
    /// <summary>
    /// Base64 encoded compressed image
    /// </summary>
    public string CompressedImageData { get; set; } = string.Empty;
    
    /// <summary>
    /// Download URL for the compressed image
    /// </summary>
    public string? DownloadUrl { get; set; }
}

/// <summary>
/// DTO for file information
/// </summary>
public class FileInfoDto
{
    /// <summary>
    /// File name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long Size { get; set; }
    
    /// <summary>
    /// File size formatted as human-readable string
    /// </summary>
    public string FormattedSize { get; set; } = string.Empty;
    
    /// <summary>
    /// Content type
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
    
    /// <summary>
    /// Image dimensions
    /// </summary>
    public ImageDimensionsDto? Dimensions { get; set; }
}

/// <summary>
/// DTO for image dimensions
/// </summary>
public class ImageDimensionsDto
{
    /// <summary>
    /// Image width in pixels
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    /// Image height in pixels
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// Aspect ratio
    /// </summary>
    public double AspectRatio { get; set; }
}

/// <summary>
/// DTO for compression statistics
/// </summary>
public class CompressionStatsDto
{
    /// <summary>
    /// Compression ratio as percentage
    /// </summary>
    public double CompressionRatio { get; set; }
    
    /// <summary>
    /// Size reduction in bytes
    /// </summary>
    public long SizeReduction { get; set; }
    
    /// <summary>
    /// Size reduction formatted as human-readable string
    /// </summary>
    public string FormattedSizeReduction { get; set; } = string.Empty;
    
    /// <summary>
    /// Processing time in milliseconds
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    
    /// <summary>
    /// Quality setting used
    /// </summary>
    public int Quality { get; set; }
    
    /// <summary>
    /// Whether ML optimization was used
    /// </summary>
    public bool UsedMLOptimization { get; set; }
    
    /// <summary>
    /// ML model information if used
    /// </summary>
    public MLResultDto? MLResult { get; set; }
}

/// <summary>
/// DTO for ML analysis result
/// </summary>
public class MLResultDto
{
    /// <summary>
    /// Model name used
    /// </summary>
    public string ModelName { get; set; } = string.Empty;
    
    /// <summary>
    /// Confidence score of ML prediction
    /// </summary>
    public float Confidence { get; set; }
    
    /// <summary>
    /// Predicted optimal quality
    /// </summary>
    public float PredictedQuality { get; set; }
    
    /// <summary>
    /// Image complexity analysis
    /// </summary>
    public float ImageComplexity { get; set; }
    
    /// <summary>
    /// Recommended compression strategy
    /// </summary>
    public string CompressionStrategy { get; set; } = string.Empty;
    
    /// <summary>
    /// Analysis processing time
    /// </summary>
    public long AnalysisTimeMs { get; set; }
}

/// <summary>
/// Response for batch compression operations
/// </summary>
public class BatchCompressionResponse
{
    /// <summary>
    /// Overall operation success
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Number of successfully processed images
    /// </summary>
    public int SuccessCount { get; set; }
    
    /// <summary>
    /// Number of failed images
    /// </summary>
    public int FailureCount { get; set; }
    
    /// <summary>
    /// Total processing time
    /// </summary>
    public long TotalProcessingTimeMs { get; set; }
    
    /// <summary>
    /// Individual results
    /// </summary>
    public List<CompressImageResponse> Results { get; set; } = new();
    
    /// <summary>
    /// Overall compression statistics
    /// </summary>
    public BatchStatsDto? OverallStats { get; set; }
}

/// <summary>
/// DTO for batch compression statistics
/// </summary>
public class BatchStatsDto
{
    /// <summary>
    /// Total original size in bytes
    /// </summary>
    public long TotalOriginalSize { get; set; }
    
    /// <summary>
    /// Total compressed size in bytes
    /// </summary>
    public long TotalCompressedSize { get; set; }
    
    /// <summary>
    /// Overall compression ratio
    /// </summary>
    public double OverallCompressionRatio { get; set; }
    
    /// <summary>
    /// Total size savings in bytes
    /// </summary>
    public long TotalSavings { get; set; }
    
    /// <summary>
    /// Average quality used
    /// </summary>
    public double AverageQuality { get; set; }
}

/// <summary>
/// Generic error response DTO
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Error code
    /// </summary>
    public string? Code { get; set; }
    
    /// <summary>
    /// Additional error details
    /// </summary>
    public Dictionary<string, object>? Details { get; set; }
    
    /// <summary>
    /// Timestamp of the error
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Response DTO for image analysis
/// </summary>
public class AnalyzeImageResponse
{
    /// <summary>
    /// Success status
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Image entropy value
    /// </summary>
    public double Entropy { get; set; }
    
    /// <summary>
    /// Mean intensity value
    /// </summary>
    public double MeanIntensity { get; set; }
    
    /// <summary>
    /// Standard deviation of intensity
    /// </summary>
    public double StandardDeviation { get; set; }
    
    /// <summary>
    /// Dominant colors in the image
    /// </summary>
    public List<string> DominantColors { get; set; } = new();
    
    /// <summary>
    /// Image complexity level
    /// </summary>
    public string Complexity { get; set; } = "medium";
    
    /// <summary>
    /// AI-generated recommendation for compression
    /// </summary>
    public string Recommendation { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for compression info endpoint
/// </summary>
public class CompressionInfoResponse
{
    /// <summary>
    /// Available compression methods
    /// </summary>
    public List<string> SupportedMethods { get; set; } = new();
    
    /// <summary>
    /// Supported image formats
    /// </summary>
    public List<string> SupportedFormats { get; set; } = new();
    
    /// <summary>
    /// Maximum file size in bytes
    /// </summary>
    public long MaxFileSizeBytes { get; set; }
    
    /// <summary>
    /// Available ML models
    /// </summary>
    public List<string> AvailableModels { get; set; } = new();
    
    /// <summary>
    /// API version
    /// </summary>
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for health check endpoint
/// </summary>
public class HealthResponse
{
    /// <summary>
    /// Overall health status
    /// </summary>
    public string Status { get; set; } = "Healthy";
    
    /// <summary>
    /// API version
    /// </summary>
    public string Version { get; set; } = string.Empty;
    
    /// <summary>
    /// Timestamp of health check
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Component health statuses
    /// </summary>
    public Dictionary<string, string> Components { get; set; } = new();
    
    /// <summary>
    /// System information
    /// </summary>
    public Dictionary<string, object> SystemInfo { get; set; } = new();
}
