using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Hub.SignalR
{
    class Program
    {
        static ClientWebSocket socket;
        static void Main(string[] args)
        {
            StartAsync();
            Console.ReadKey();
            CloseConnect();
        }
        private static async void StartAsync()
        {
            do
            {
                socket = new ClientWebSocket();
                    try
                    {
                        await socket.ConnectAsync(new Uri("wss://localhost:5001/ws"), CancellationToken.None);

                        await Send(socket, "Hello world");
                        await Receive();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR - {ex.Message}");
                    }
            } while (true);
        }
        static async Task Send(ClientWebSocket socket, string data) => await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);

        static async Task Receive()
        {
            var buffer = new ArraySegment<byte>(new byte[1024 * 4]);
            do
            {
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    ms.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                        Console.WriteLine(await reader.ReadToEndAsync());
                }
            } while (true);
        }
        private static async void CloseConnect()
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye-Bye", CancellationToken.None);
        }

        private static void Start()
        {
            ClientHub client = new ClientHub();
            var mess = Console.ReadLine();
            client.SendMess(mess);
            Console.ReadKey();
        }
    }
}
