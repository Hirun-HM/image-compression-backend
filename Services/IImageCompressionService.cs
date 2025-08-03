using ImageCompressionAPI.DTOs;
using ImageCompressionAPI.Models;

namespace ImageCompressionAPI.Services;

/// <summary>
/// Service interface for image compression operations
/// </summary>
public interface IImageCompressionService
{
    /// <summary>
    /// Compresses an image using the specified compression method
    /// </summary>
    /// <param name="imageData">Raw image data</param>
    /// <param name="options">Compression options</param>
    /// <returns>Compression result with compressed image and metadata</returns>
    Task<CompressionResult> CompressImageAsync(byte[] imageData, CompressionOptions options);

    /// <summary>
    /// Analyzes an image to provide optimal compression recommendations
    /// </summary>
    /// <param name="imageData">Raw image data</param>
    /// <returns>Image analysis with recommendations</returns>
    Task<ImageAnalysis> AnalyzeImageAsync(byte[] imageData);

    /// <summary>
    /// Gets supported image formats for compression
    /// </summary>
    /// <returns>List of supported formats</returns>
    IEnumerable<string> GetSupportedFormats();

    /// <summary>
    /// Validates image data and format
    /// </summary>
    /// <param name="imageData">Image data to validate</param>
    /// <param name="fileName">Original file name</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateImage(byte[] imageData, string fileName);
}
