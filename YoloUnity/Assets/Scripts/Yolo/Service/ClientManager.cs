using UnityEngine;
using Grpc.Core;
using System;
using System.Diagnostics;

namespace Yolo
{
    public class ClientManager : IDisposable
    {
        public event EventHandler<DetectionEventArgs> RaiseDetectionEvent;
        DetectionEventArgs detectionEventArgs; // re-use

        Channel channel;
        ClientWrapper client;
        Texture2D texture;
        YoloResult result; // re-use, reference

        Stopwatch timer;
        const int minInterval = 0; // throttle requests
        bool requestEnabled => timer.Elapsed.Milliseconds >= minInterval;

        public ClientManager(ref Texture2D texture)
        {
            channel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            client = new ClientWrapper(new YoloService.YoloServiceClient(channel));

            this.texture = texture;
            result = new YoloResult();
            detectionEventArgs = new DetectionEventArgs(result);

            timer = new Stopwatch();
            timer.Start();
        }

        public void Dispose()
        {
            channel.ShutdownAsync();
        }

        public void Update()
        {
            if (client.IsIdle)
            {
                if (requestEnabled)
                {
                    timer.Restart();
                    result.Clear();
                    client.Detect(ImageConversion.EncodeToPNG(texture), result);
                }
            }
            else if (client.HasNewResponse)
            {
                UnityEngine.Debug.Log(string.Format("Detection time: {0}ms, Roundtrip time: {1}ms",
                   result.ElapsedMilliseconds, timer.Elapsed.Milliseconds));

                timer.Restart();
                client.Reset();
                OnRaiseDetectionEvent(detectionEventArgs);
            }
        }

        void OnRaiseDetectionEvent(DetectionEventArgs e)
        {
            EventHandler<DetectionEventArgs> handler = RaiseDetectionEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class DetectionEventArgs : EventArgs
    {
        public YoloResult Result { get; private set; }

        public DetectionEventArgs(YoloResult result)
        {
            Result = result;
        }
    }
}