using ImageCompressionAPI.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System.Diagnostics;

namespace ImageCompressionAPI.Services;

/// <summary>
/// ML.NET-based machine learning model service for image compression optimization
/// </summary>
public class MLModelService : IMLModelService
{
    private readonly MLContext _mlContext;
    private readonly ILogger<MLModelService> _logger;
    private ITransformer? _model;
    private PredictionEngine<ImageData, CompressionPrediction>? _predictionEngine;
    private ModelInfo _modelInfo;

    public MLModelService(MLContext mlContext, ILogger<MLModelService> logger)
    {
        _mlContext = mlContext;
        _logger = logger;
        _modelInfo = new ModelInfo
        {
            Name = "ImageCompressionOptimizer",
            Version = "1.0.0",
            Description = "ML.NET model for predicting optimal image compression settings"
        };
    }

    /// <summary>
    /// Predicts optimal compression settings for an image
    /// </summary>
    public async Task<CompressionPrediction> PredictOptimalCompressionAsync(ImageData imageData)
    {
        try
        {
            if (_predictionEngine == null)
            {
                await LoadDefaultModelAsync();
            }

            var prediction = _predictionEngine!.Predict(imageData);
            
            // Apply business rules and constraints
            prediction.OptimalQuality = Math.Max(10, Math.Min(95, prediction.OptimalQuality));
            prediction.PredictedCompressionRatio = Math.Max(0.1f, Math.Min(0.9f, prediction.PredictedCompressionRatio));
            
            return prediction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting optimal compression for image");
            return GetFallbackPrediction(imageData);
        }
    }

    /// <summary>
    /// Analyzes image characteristics using computer vision techniques
    /// </summary>
    public async Task<ImageAnalysisResult> AnalyzeImageAsync(byte[] imageBytes)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            using var image = Image.Load<Rgb24>(imageBytes);
            
            var result = new ImageAnalysisResult();
            
            // Analyze image complexity (edge detection, texture analysis)
            result.Complexity = await CalculateImageComplexityAsync(image);
            
            // Analyze color distribution
            result.DominantColors = await ExtractDominantColorsAsync(image);
            
            // Estimate noise level
            result.NoiseLevel = await EstimateNoiseLevelAsync(image);
            
            // Calculate edge density
            result.EdgeDensity = await CalculateEdgeDensityAsync(image);
            
            // Determine compression strategy
            result.CompressionStrategy = DetermineCompressionStrategy(result);
            
            stopwatch.Stop();
            result.AnalysisTime = stopwatch.Elapsed;
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing image");
            stopwatch.Stop();
            
            return new ImageAnalysisResult
            {
                Complexity = 0.5f,
                NoiseLevel = 0.3f,
                EdgeDensity = 0.4f,
                CompressionStrategy = "balanced",
                AnalysisTime = stopwatch.Elapsed
            };
        }
    }

    /// <summary>
    /// Loads the ML model from file or creates a default model
    /// </summary>
    public async Task<bool> LoadModelAsync(string modelPath)
    {
        try
        {
            if (File.Exists(modelPath))
            {
                _model = _mlContext.Model.Load(modelPath, out var modelSchema);
                _predictionEngine = _mlContext.Model.CreatePredictionEngine<ImageData, CompressionPrediction>(_model);
                
                _modelInfo.LoadedAt = DateTime.UtcNow;
                _modelInfo.IsLoaded = true;
                
                _logger.LogInformation("ML model loaded successfully from {ModelPath}", modelPath);
                return true;
            }
            else
            {
                _logger.LogWarning("Model file not found at {ModelPath}, creating default model", modelPath);
                return await LoadDefaultModelAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading ML model from {ModelPath}", modelPath);
            return await LoadDefaultModelAsync();
        }
    }

    /// <summary>
    /// Gets information about the currently loaded model
    /// </summary>
    public async Task<ModelInfo> GetModelInfoAsync()
    {
        return await Task.FromResult(_modelInfo);
    }

    #region Private Methods

    /// <summary>
    /// Creates and loads a default rule-based model
    /// </summary>
    private async Task<bool> LoadDefaultModelAsync()
    {
        try
        {
            // Create a simple rule-based model for demonstration
            var pipeline = _mlContext.Transforms.Concatenate("Features", 
                nameof(ImageData.Width), 
                nameof(ImageData.Height), 
                nameof(ImageData.AspectRatio), 
                nameof(ImageData.FileSize))
                .Append(_mlContext.Regression.Trainers.Sdca(maximumNumberOfIterations: 100));

            // Create dummy training data
            var trainingData = GenerateTrainingData();
            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);
            
            _model = pipeline.Fit(dataView);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ImageData, CompressionPrediction>(_model);
            
            _modelInfo.LoadedAt = DateTime.UtcNow;
            _modelInfo.IsLoaded = true;
            
            _logger.LogInformation("Default ML model created and loaded successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating default ML model");
            return false;
        }
    }

    /// <summary>
    /// Generates synthetic training data for the default model
    /// </summary>
    private IEnumerable<CompressionTrainingData> GenerateTrainingData()
    {
        var random = new Random(42);
        var data = new List<CompressionTrainingData>();

        for (int i = 0; i < 1000; i++)
        {
            var width = random.Next(100, 4000);
            var height = random.Next(100, 4000);
            var aspectRatio = (float)width / height;
            var fileSize = random.Next(50000, 20000000);
            
            // Rule-based optimal quality calculation
            var complexity = random.NextSingle();
            var optimalQuality = 60 + (complexity * 30); // 60-90 based on complexity
            
            data.Add(new CompressionTrainingData
            {
                Width = width,
                Height = height,
                AspectRatio = aspectRatio,
                OriginalFileSize = fileSize,
                Complexity = complexity,
                OptimalQuality = (float)optimalQuality
            });
        }

        return data;
    }

    /// <summary>
    /// Provides fallback prediction when ML model fails
    /// </summary>
    private CompressionPrediction GetFallbackPrediction(ImageData imageData)
    {
        // Rule-based fallback logic
        var quality = 80f; // Default quality
        
        // Adjust based on file size
        if (imageData.FileSize > 5000000) // > 5MB
            quality = 70f;
        else if (imageData.FileSize < 500000) // < 500KB
            quality = 90f;
        
        // Adjust based on dimensions
        var pixelCount = imageData.Width * imageData.Height;
        if (pixelCount > 8000000) // > 8MP
            quality = Math.Min(quality, 75f);
        
        return new CompressionPrediction
        {
            OptimalQuality = quality,
            PredictedCompressionRatio = 0.6f,
            Confidence = 0.7f,
            RecommendedFormat = "jpeg"
        };
    }

    /// <summary>
    /// Calculates image complexity using edge detection and texture analysis
    /// </summary>
    private async Task<float> CalculateImageComplexityAsync(Image<Rgb24> image)
    {
        return await Task.Run(() =>
        {
            // Simple complexity calculation based on color variance
            var pixels = new List<Rgb24>();
            
            // Sample pixels for performance
            for (int y = 0; y < image.Height; y += 10)
            {
                for (int x = 0; x < image.Width; x += 10)
                {
                    if (x < image.Width && y < image.Height)
                    {
                        pixels.Add(image[x, y]);
                    }
                }
            }
            
            if (pixels.Count == 0) return 0.5f;
            
            // Calculate color variance as complexity metric
            var avgR = pixels.Average(p => p.R);
            var avgG = pixels.Average(p => p.G);
            var avgB = pixels.Average(p => p.B);
            
            var variance = pixels.Average(p => 
                Math.Pow(p.R - avgR, 2) + 
                Math.Pow(p.G - avgG, 2) + 
                Math.Pow(p.B - avgB, 2)) / (255 * 255 * 3);
            
            return Math.Min(1.0f, (float)variance);
        });
    }

    /// <summary>
    /// Extracts dominant colors from the image
    /// </summary>
    private async Task<List<string>> ExtractDominantColorsAsync(Image<Rgb24> image)
    {
        return await Task.Run(() =>
        {
            var colorCounts = new Dictionary<Rgb24, int>();
            
            // Sample colors for performance
            for (int y = 0; y < image.Height; y += 20)
            {
                for (int x = 0; x < image.Width; x += 20)
                {
                    if (x < image.Width && y < image.Height)
                    {
                        var color = image[x, y];
                        // Quantize color to reduce variations
                        var quantized = new Rgb24(
                            (byte)((color.R / 32) * 32),
                            (byte)((color.G / 32) * 32),
                            (byte)((color.B / 32) * 32)
                        );
                        
                        colorCounts[quantized] = colorCounts.GetValueOrDefault(quantized, 0) + 1;
                    }
                }
            }
            
            return colorCounts
                .OrderByDescending(kv => kv.Value)
                .Take(5)
                .Select(kv => $"#{kv.Key.R:X2}{kv.Key.G:X2}{kv.Key.B:X2}")
                .ToList();
        });
    }

    /// <summary>
    /// Estimates noise level in the image
    /// </summary>
    private async Task<float> EstimateNoiseLevelAsync(Image<Rgb24> image)
    {
        return await Task.Run(() =>
        {
            // Simple noise estimation using local variance
            float totalVariance = 0;
            int sampleCount = 0;
            
            for (int y = 1; y < image.Height - 1; y += 10)
            {
                for (int x = 1; x < image.Width - 1; x += 10)
                {
                    if (x < image.Width - 1 && y < image.Height - 1)
                    {
                        var center = image[x, y];
                        var neighbors = new[]
                        {
                            image[x-1, y], image[x+1, y],
                            image[x, y-1], image[x, y+1]
                        };
                        
                        var avgR = neighbors.Average(p => p.R);
                        var avgG = neighbors.Average(p => p.G);
                        var avgB = neighbors.Average(p => p.B);
                        
                        var variance = 
                            Math.Pow(center.R - avgR, 2) + 
                            Math.Pow(center.G - avgG, 2) + 
                            Math.Pow(center.B - avgB, 2);
                        
                        totalVariance += (float)variance;
                        sampleCount++;
                    }
                }
            }
            
            return sampleCount > 0 ? Math.Min(1.0f, totalVariance / sampleCount / (255 * 255 * 3)) : 0.3f;
        });
    }

    /// <summary>
    /// Calculates edge density using simple gradient detection
    /// </summary>
    private async Task<float> CalculateEdgeDensityAsync(Image<Rgb24> image)
    {
        return await Task.Run(() =>
        {
            int edgeCount = 0;
            int totalSamples = 0;
            
            for (int y = 1; y < image.Height - 1; y += 5)
            {
                for (int x = 1; x < image.Width - 1; x += 5)
                {
                    if (x < image.Width - 1 && y < image.Height - 1)
                    {
                        var center = image[x, y];
                        var right = image[x + 1, y];
                        var down = image[x, y + 1];
                        
                        // Calculate gradient magnitude
                        var gradX = Math.Abs(center.R - right.R) + Math.Abs(center.G - right.G) + Math.Abs(center.B - right.B);
                        var gradY = Math.Abs(center.R - down.R) + Math.Abs(center.G - down.G) + Math.Abs(center.B - down.B);
                        var gradient = Math.Sqrt(gradX * gradX + gradY * gradY);
                        
                        if (gradient > 100) // Threshold for edge detection
                            edgeCount++;
                        
                        totalSamples++;
                    }
                }
            }
            
            return totalSamples > 0 ? (float)edgeCount / totalSamples : 0.4f;
        });
    }

    /// <summary>
    /// Determines compression strategy based on analysis results
    /// </summary>
    private string DetermineCompressionStrategy(ImageAnalysisResult analysis)
    {
        if (analysis.Complexity > 0.7f || analysis.EdgeDensity > 0.6f)
        {
            return "high_quality"; // Preserve detail in complex images
        }
        else if (analysis.NoiseLevel > 0.5f)
        {
            return "aggressive"; // Noise can hide compression artifacts
        }
        else if (analysis.Complexity < 0.3f && analysis.EdgeDensity < 0.3f)
        {
            return "efficient"; // Simple images can be compressed more
        }
        
        return "balanced";
    }

    #endregion
}
