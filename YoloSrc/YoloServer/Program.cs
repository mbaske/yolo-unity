using Grpc.Core;
using System;

namespace Yolo
{
    class Program
    {
        static void Main(string[] args)
        {
            const int Port = 50052;
            YoloServiceImpl impl = new YoloServiceImpl();

            Server server = new Server
            {
                Services = { YoloService.BindService(impl) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("YoloService server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            impl.Dispose();
            server.ShutdownAsync().Wait();
        }
    }
}
