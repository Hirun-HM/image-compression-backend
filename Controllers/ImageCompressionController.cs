using Microsoft.AspNetCore.Mvc;
using ImageCompressionAPI.Services;
using ImageCompressionAPI.DTOs;
using ImageCompressionAPI.Models;

namespace ImageCompressionAPI.Controllers;

/// <summary>
/// Controller for image compression operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ImageCompressionController : ControllerBase
{
    private readonly IImageCompressionService _compressionService;
    private readonly ILogger<ImageCompressionController> _logger;

    /// <summary>
    /// Initialize controller with dependencies
    /// </summary>
    public ImageCompressionController(
        IImageCompressionService compressionService,
        ILogger<ImageCompressionController> logger)
    {
        _compressionService = compressionService;
        _logger = logger;
    }

    /// <summary>
    /// Compress an uploaded image
    /// </summary>
    /// <param name="image">Image file to compress</param>
    /// <param name="method">Compression method (traditional, ml, hybrid)</param>
    /// <param name="quality">Quality level (10-100)</param>
    /// <param name="targetSizeKB">Optional target size in KB</param>
    /// <param name="enableAnalysis">Whether to enable quality analysis</param>
    /// <returns>Compression result with compressed image data</returns>
    [HttpPost("compress")]
    [ProducesResponseType(typeof(CompressionResult), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> CompressImage(
        IFormFile image,
        [FromForm] string method = "traditional",
        [FromForm] int quality = 85,
        [FromForm] int? targetSizeKB = null,
        [FromForm] bool enableAnalysis = true)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new ErrorResponse { Message = "No image file provided" });
            }

            if (image.Length > 50 * 1024 * 1024) // 50MB limit
            {
                return BadRequest(new ErrorResponse { Message = "File size exceeds 50MB limit" });
            }

            // Validate compression method
            if (!Enum.TryParse<CompressionMethod>(method, true, out var compressionMethod))
            {
                compressionMethod = CompressionMethod.Traditional;
            }

            // Read image data
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();

            // Create compression options
            var options = new CompressionOptions
            {
                Method = compressionMethod,
                Quality = Math.Max(10, Math.Min(100, quality)),
                TargetSizeKB = targetSizeKB,
                EnableAnalysis = enableAnalysis
            };

            // Perform compression
            var result = await _compressionService.CompressImageAsync(imageData, options);
            
            // Set download URL
            result.DownloadUrl = $"/api/ImageCompression/download/{result.Id}";

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error compressing image");
            return StatusCode(500, new ErrorResponse { Message = "Internal server error during compression" });
        }
    }

    /// <summary>
    /// Analyze an image without compression
    /// </summary>
    /// <param name="image">Image file to analyze</param>
    /// <returns>Image analysis result</returns>
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(AnalyzeImageResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> AnalyzeImage(IFormFile image)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new ErrorResponse { Message = "No image file provided" });
            }

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();

            var analysis = await _compressionService.AnalyzeImageAsync(imageData);

            var response = new AnalyzeImageResponse
            {
                Success = true,
                Entropy = analysis.Entropy,
                MeanIntensity = analysis.MeanIntensity,
                StandardDeviation = analysis.StandardDeviation,
                DominantColors = analysis.DominantColors,
                Complexity = analysis.Complexity,
                Recommendation = analysis.Recommendation
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing image");
            return StatusCode(500, new ErrorResponse { Message = "Internal server error during analysis" });
        }
    }

    /// <summary>
    /// Get compression service information
    /// </summary>
    /// <returns>Service capabilities and configuration</returns>
    [HttpGet("info")]
    [ProducesResponseType(typeof(CompressionInfoResponse), 200)]
    public async Task<IActionResult> GetCompressionInfo()
    {
        var response = new CompressionInfoResponse
        {
            SupportedMethods = ["traditional", "ml", "hybrid"],
            SupportedFormats = ["jpeg", "jpg", "png", "webp"],
            MaxFileSizeBytes = 50 * 1024 * 1024, // 50MB
            AvailableModels = ["autoencoder", "cnn"],
            Version = "1.0.0"
        };

        return Ok(response);
    }

    /// <summary>
    /// Download compressed image by ID
    /// </summary>
    /// <param name="id">Compression result ID</param>
    /// <returns>Compressed image file</returns>
    [HttpGet("download/{id}")]
    [ProducesResponseType(typeof(FileResult), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> DownloadImage(string id)
    {
        try
        {
            // This is a placeholder - in a real app, you'd retrieve from storage
            return NotFound(new ErrorResponse { Message = "Download functionality not yet implemented" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading image {Id}", id);
            return StatusCode(500, new ErrorResponse { Message = "Internal server error during download" });
        }
    }

    /// <summary>
    /// Health check endpoint
    /// </summary>
    /// <returns>Service health status</returns>
    [HttpGet("health")]
    [ProducesResponseType(typeof(HealthResponse), 200)]
    public async Task<IActionResult> HealthCheck()
    {
        var response = new HealthResponse
        {
            Status = "Healthy",
            Version = "1.0.0",
            Timestamp = DateTime.UtcNow,
            Components = new Dictionary<string, string>
            {
                ["ImageCompression"] = "Healthy",
                ["MLService"] = "Healthy"
            },
            SystemInfo = new Dictionary<string, object>
            {
                ["MachineName"] = Environment.MachineName,
                ["ProcessorCount"] = Environment.ProcessorCount,
                ["WorkingSet"] = Environment.WorkingSet
            }
        };

        return Ok(response);
    }
}
