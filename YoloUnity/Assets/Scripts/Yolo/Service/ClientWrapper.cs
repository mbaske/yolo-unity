using Grpc.Core;
using Google.Protobuf;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Yolo
{
    public class ClientWrapper
    {
        public bool IsIdle => state == State.Idle;
        public bool IsBusy => state == State.Busy;
        public bool HasNewResponse => state == State.NewResponse;

        enum State
        {
            Idle,
            Busy,
            NewResponse
        }
        State state = State.Idle;

        readonly YoloService.YoloServiceClient client;

        public ClientWrapper(YoloService.YoloServiceClient client)
        {
            this.client = client;
        }

        public void Reset()
        {
            state = State.Idle;
        }

        public async Task Detect(byte[] imageData, YoloResult result)
        {
            try
            {
                state = State.Busy;
                DetectionRequest request = new DetectionRequest { Image = ByteString.CopyFrom(imageData) };

                using (var call = client.Detect(request))
                {
                    var responseStream = call.ResponseStream;
                    while (await responseStream.MoveNext())
                    {
                        DetectionResponse response = responseStream.Current;
                        foreach (DetectionResult r in response.YoloItems)
                        {
                            result.Add(new YoloItem(r.Type, r.Confidence, r.X, r.Y, r.Width, r.Height));
                        }
                        result.ElapsedMilliseconds = response.ElapsedMilliseconds;
                        state = State.NewResponse;
                    }
                }
            }
            catch (RpcException e)
            {
                UnityEngine.Debug.LogError("RPC failed " + e);
                throw;
            }
        }
    }
}
