using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBootcampBot
{
    public static class ImageAnalyzer
    {
        public static async Task<AnalysisResult> DescribeImage(string url)
        {
            
                var client = new VisionServiceClient("<Project Oxford Subscription key here>");
                var features = new VisualFeature[] { VisualFeature.Color, VisualFeature.Description };
                var result = await client.AnalyzeImageAsync(url, features);

                return result;

        }
    }
}
