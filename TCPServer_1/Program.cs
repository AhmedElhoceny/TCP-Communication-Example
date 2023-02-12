using System.IO;
using System.Net;
using System.Net.Sockets;

var listener = new TcpListener(IPAddress.Any, 1337);
listener.Start();

_ = Task.Run(async () =>
{
    var tcpClient = await listener.AcceptTcpClientAsync();
    await HandleClientConnection(tcpClient);
});

_ = Task.Run(async () =>
{
    var tcpClient = await listener.AcceptTcpClientAsync();
    await HandleClientConnection2(tcpClient);
});

await ReadTheClientMessages();


async Task HandleClientConnection(TcpClient client)
{
    using NetworkStream ns = client.GetStream();
    using StreamWriter sw = new StreamWriter(ns);

    await sw.WriteLineAsync("Hello My Friend.........");

    await sw.FlushAsync();

}
async Task HandleClientConnection2(TcpClient client)
{
    using NetworkStream ns = client.GetStream();
    using StreamWriter sw = new StreamWriter(ns);

    await sw.WriteLineAsync("Hello My Friend Again.........");

    await sw.FlushAsync();

}
async Task ReadTheClientMessages()
{
    while (true)
    {

        using TcpClient client = new TcpClient();
        await client.ConnectAsync(IPAddress.Loopback, 1337);
        using NetworkStream ns = client.GetStream();

        //ns.ReadTimeout = 2000;

        using StreamReader sr = new StreamReader(ns);
        
        string Message = await sr.ReadToEndAsync();
        Console.WriteLine(Message);
        Task.Delay(1000).Wait();
    }

}
