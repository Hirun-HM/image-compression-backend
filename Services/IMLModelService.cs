using ImageCompressionAPI.Models;

namespace ImageCompressionAPI.Services;

/// <summary>
/// Interface for ML model services
/// </summary>
public interface IMLModelService
{
    /// <summary>
    /// Analyzes an image and predicts optimal compression settings
    /// </summary>
    /// <param name="imageData">Image data to analyze</param>
    /// <returns>Compression prediction result</returns>
    Task<CompressionPrediction> PredictOptimalCompressionAsync(ImageData imageData);
    
    /// <summary>
    /// Analyzes image characteristics for compression optimization
    /// </summary>
    /// <param name="imageBytes">Raw image bytes</param>
    /// <returns>Image analysis result</returns>
    Task<ImageAnalysisResult> AnalyzeImageAsync(byte[] imageBytes);
    
    /// <summary>
    /// Loads or reloads the ML model
    /// </summary>
    /// <param name="modelPath">Path to the ML model file</param>
    /// <returns>True if model loaded successfully</returns>
    Task<bool> LoadModelAsync(string modelPath);
    
    /// <summary>
    /// Gets information about the currently loaded model
    /// </summary>
    /// <returns>Model information</returns>
    Task<ModelInfo> GetModelInfoAsync();
}

/// <summary>
/// Model information structure
/// </summary>
public class ModelInfo
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public DateTime LoadedAt { get; set; }
    public bool IsLoaded { get; set; }
    public string Description { get; set; } = string.Empty;
}
