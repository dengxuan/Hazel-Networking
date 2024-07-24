using Hazel.Dtls;
using Hazel.Udp;
using Hazel.Udp.FewerThreads;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Hazel.Hosting
{
    internal class Server : IHostedService
    {
        private readonly DtlsConnectionListener _listener = new(10, new IPEndPoint(IPAddress.Any, 4321), logger: new ConsoleLogger(true));

        private void HandleNewConnection(NewConnectionEventArgs args)
        {
            var clientVersion = args.HandshakeData.ReadInt32();
            Console.WriteLine($"{clientVersion} New connection from {args.Connection.EndPoint}");
            // Make sure this client version is compatible with this server and/or other clients!
            args.Connection.DataReceived += HandleDataReceived;
        }

        private void HandleDataReceived(DataReceivedEventArgs args)
        {
            string message = System.Text.Encoding.UTF8.GetString(args.Message.Buffer);
            Console.WriteLine($"Received: {message}");
            // 广播消息给所有客户端
            BroadcastMessage(message, args.Sender);
        }


        private void BroadcastMessage(string message, Connection sender)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            var messageWriter = new MessageWriter(data);
            Console.WriteLine(messageWriter);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _listener.NewConnection += HandleNewConnection;
            _listener.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
