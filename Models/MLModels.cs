using Microsoft.ML.Data;

namespace ImageCompressionAPI.Models;

/// <summary>
/// ML.NET input data structure for image compression
/// </summary>
public class ImageData
{
    [LoadColumn(0)]
    public byte[] ImageBytes { get; set; } = Array.Empty<byte>();
    
    [LoadColumn(1)]
    public string ImagePath { get; set; } = string.Empty;
    
    // Image properties for ML analysis
    public float Width { get; set; }
    public float Height { get; set; }
    public float AspectRatio { get; set; }
    public float FileSize { get; set; }
    public string Format { get; set; } = string.Empty;
}

/// <summary>
/// ML.NET prediction result for optimal compression settings
/// </summary>
public class CompressionPrediction
{
    [ColumnName("Score")]
    public float[] Score { get; set; } = Array.Empty<float>();
    
    /// <summary>
    /// Predicted optimal quality setting
    /// </summary>
    public float OptimalQuality { get; set; }
    
    /// <summary>
    /// Predicted compression ratio
    /// </summary>
    public float PredictedCompressionRatio { get; set; }
    
    /// <summary>
    /// Confidence score of the prediction
    /// </summary>
    public float Confidence { get; set; }
    
    /// <summary>
    /// Recommended output format
    /// </summary>
    public string RecommendedFormat { get; set; } = "jpeg";
}

/// <summary>
/// Training data for ML compression model
/// </summary>
public class CompressionTrainingData
{
    public float Width { get; set; }
    public float Height { get; set; }
    public float AspectRatio { get; set; }
    public float OriginalFileSize { get; set; }
    public string OriginalFormat { get; set; } = string.Empty;
    
    // Features extracted from image content
    public float Complexity { get; set; }  // Edge density, texture complexity
    public float ColorVariance { get; set; }  // Color distribution
    public float NoiseLevel { get; set; }  // Image noise estimation
    
    // Target values
    [ColumnName("Label")]
    public float OptimalQuality { get; set; }
    
    public float AchievedCompressionRatio { get; set; }
    public string BestFormat { get; set; } = string.Empty;
}

/// <summary>
/// Image analysis result from ML model
/// </summary>
public class ImageAnalysisResult
{
    /// <summary>
    /// Estimated image complexity (0-1)
    /// </summary>
    public float Complexity { get; set; }
    
    /// <summary>
    /// Detected dominant colors
    /// </summary>
    public List<string> DominantColors { get; set; } = new();
    
    /// <summary>
    /// Estimated noise level (0-1)
    /// </summary>
    public float NoiseLevel { get; set; }
    
    /// <summary>
    /// Edge density (0-1)
    /// </summary>
    public float EdgeDensity { get; set; }
    
    /// <summary>
    /// Recommended compression strategy
    /// </summary>
    public string CompressionStrategy { get; set; } = "balanced";
    
    /// <summary>
    /// Processing time for analysis
    /// </summary>
    public TimeSpan AnalysisTime { get; set; }
}
