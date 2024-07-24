// See https://aka.ms/new-console-template for more information
using Hazel;
using Hazel.Dtls;
using Hazel.Udp;
using System.Net;

Console.WriteLine("Hello, World!");

var connection = new DtlsUnityConnection(new ConsoleLogger(true), new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 4321));
connection.DataReceived += (DataReceivedEventArgs args) =>
{
    string message = System.Text.Encoding.UTF8.GetString(args.Message.Buffer);
    Console.WriteLine($"Received: {message}");
};
connection.Disconnected += (sender, args) =>
{
    Console.WriteLine(args.Reason);
};
connection.Connect();
while (true)
{
    var msg = MessageWriter.Get(SendOption.None);
    msg.StartMessage(1);
    msg.Write("message");
    msg.EndMessage();
    connection.Send(msg);
    msg.Recycle();
    await Task.Delay(1000);
}