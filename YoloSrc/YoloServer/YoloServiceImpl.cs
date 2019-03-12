using Alturos.Yolo;
using Alturos.Yolo.Model;
using Grpc.Core;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yolo
{
    public class YoloServiceImpl : YoloService.YoloServiceBase, IDisposable
    {
        readonly YoloWrapper yoloWrapper;

        public YoloServiceImpl()
        {
            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect();
            yoloWrapper = new YoloWrapper(config);
        }

        public void Dispose()
        {
            yoloWrapper.Dispose();
        }

        public override async Task Detect(DetectionRequest request, 
            IServerStreamWriter<DetectionResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("Image received...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            IEnumerable<YoloItem> items = yoloWrapper.Detect(request.Image.ToByteArray());
            
            DetectionResponse response = new DetectionResponse();
            foreach (YoloItem item in items)
            {
                response.YoloItems.Add(new DetectionResult
                {
                    Type = item.Type,
                    Confidence = item.Confidence,
                    X = item.X,
                    Y = item.Y,
                    Width = item.Width,
                    Height = item.Height
                });
            }
            timer.Stop();
            response.ElapsedMilliseconds = timer.Elapsed.Milliseconds;
            Console.WriteLine("{0} objects detected in {1} ms", response.YoloItems.Count, timer.Elapsed.Milliseconds);

            await responseStream.WriteAsync(response);
        }
    }
}
