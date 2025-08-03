namespace ImageCompressionAPI.Models;

/// <summary>
/// Represents the result of an image compression operation
/// </summary>
/// 

public class CompressionResult
{
    /// <summary>
    /// Unique identifier for the compression operation
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// Original file name
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;
    
    /// <summary>
    /// Compressed file name
    /// </summary>
    public string CompressedFileName { get; set; } = string.Empty;
    
    /// <summary>
    /// Original file size in bytes
    /// </summary>
    public long OriginalSize { get; set; }
    
    /// <summary>
    /// Compressed file size in bytes
    /// </summary>
    public long CompressedSize { get; set; }
    
    /// <summary>
    /// Compression ratio as a percentage
    /// </summary>
    public double CompressionRatio => OriginalSize > 0 ? 
        Math.Round((1.0 - (double)CompressedSize / OriginalSize) * 100, 2) : 0;
    
    /// <summary>
    /// Quality setting used for compression (0-100)
    /// </summary>
    public int Quality { get; set; }
    
    /// <summary>
    /// Processing time in seconds
    /// </summary>
    public double ProcessingTime { get; set; }
    
    /// <summary>
    /// Compression method used
    /// </summary>
    public string Method { get; set; } = "traditional";
    
    /// <summary>
    /// Processing time in milliseconds
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    
    /// <summary>
    /// Compression time
    /// </summary>
    public double CompressionTime => ProcessingTime;
    
    /// <summary>
    /// Timestamp when compression was completed
    /// </summary>
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Base64 encoded compressed image data
    /// </summary>
    public string? CompressedImageData { get; set; }
    
    /// <summary>
    /// Download URL for the compressed image
    /// </summary>
    public string DownloadUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Content type of the compressed image
    /// </summary>
    public string ContentType { get; set; } = "image/jpeg";
    
    /// <summary>
    /// Machine learning model used for compression
    /// </summary>
    public string? ModelUsed { get; set; }
    
    /// <summary>
    /// Quality analysis results
    /// </summary>
    public QualityAnalysis? Analysis { get; set; }
    
    /// <summary>
    /// Additional metadata from ML processing
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Configuration options for image compression
/// </summary>
public class CompressionOptions
{
    /// <summary>
    /// Compression method to use
    /// </summary>
    public CompressionMethod Method { get; set; } = CompressionMethod.Traditional;
    
    /// <summary>
    /// Target quality (0-100, higher is better quality)
    /// </summary>
    public int Quality { get; set; } = 80;
    
    /// <summary>
    /// Target size in KB (optional)
    /// </summary>
    public int? TargetSizeKB { get; set; }
    
    /// <summary>
    /// Whether to enable quality analysis
    /// </summary>
    public bool EnableAnalysis { get; set; } = true;
    
    /// <summary>
    /// Maximum width for resizing (optional)
    /// </summary>
    public int? MaxWidth { get; set; }
    
    /// <summary>
    /// Maximum height for resizing (optional)
    /// </summary>
    public int? MaxHeight { get; set; }
    
    /// <summary>
    /// Output format (jpeg, png, webp)
    /// </summary>
    public string OutputFormat { get; set; } = "jpeg";
    
    /// <summary>
    /// Whether to use ML-based compression
    /// </summary>
    public bool UseMLCompression { get; set; } = true;
    
    /// <summary>
    /// ML model to use for compression
    /// </summary>
    public string? MLModel { get; set; }
    
    /// <summary>
    /// Whether to preserve metadata
    /// </summary>
    public bool PreserveMetadata { get; set; } = false;
}

/// <summary>
/// Represents an uploaded image file
/// </summary>
public class ImageUpload
{
    /// <summary>
    /// File name
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// Content type
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
    
    /// <summary>
    /// Image data as byte array
    /// </summary>
    public byte[] Data { get; set; } = Array.Empty<byte>();
    
    /// <summary>
    /// Upload timestamp
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents the analysis result for an image
/// </summary>
public class ImageAnalysis
{
    /// <summary>
    /// Image width
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    /// Image height
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// Image format
    /// </summary>
    public string Format { get; set; } = string.Empty;
    
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// Color depth
    /// </summary>
    public int ColorDepth { get; set; }
    
    /// <summary>
    /// Whether image has transparency
    /// </summary>
    public bool HasTransparency { get; set; }
    
    /// <summary>
    /// Estimated complexity
    /// </summary>
    public string EstimatedComplexity { get; set; } = "medium";
    
    /// <summary>
    /// Recommended compression method
    /// </summary>
    public string RecommendedMethod { get; set; } = "traditional";
    
    /// <summary>
    /// Recommended quality
    /// </summary>
    public int RecommendedQuality { get; set; } = 80;
    
    /// <summary>
    /// Potential savings percentage
    /// </summary>
    public double PotentialSavings { get; set; }
    
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
/// Enumeration of supported compression methods
/// </summary>
public enum CompressionMethod
{
    Traditional,
    ML,
    Hybrid
}

/// <summary>
/// Represents quality analysis metrics
/// </summary>
public class QualityAnalysis
{
    /// <summary>
    /// Peak Signal-to-Noise Ratio
    /// </summary>
    public double? PSNR { get; set; }
    
    /// <summary>
    /// Structural Similarity Index
    /// </summary>
    public double? SSIM { get; set; }
    
    /// <summary>
    /// Mean Squared Error
    /// </summary>
    public double? MSE { get; set; }
    
    /// <summary>
    /// Image entropy
    /// </summary>
    public double? Entropy { get; set; }
    
    /// <summary>
    /// Color histogram similarity
    /// </summary>
    public double? ColorHistogramSimilarity { get; set; }
    
    /// <summary>
    /// Edge preservation score
    /// </summary>
    public double? EdgePreservation { get; set; }
}
