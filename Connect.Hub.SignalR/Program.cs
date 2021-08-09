using System;

namespace Connect.Hub.SignalR
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientHub client = new ClientHub();
            var mess = Console.ReadLine();
            client.SendMess(mess);
            Console.ReadKey();
        }
    }
}
