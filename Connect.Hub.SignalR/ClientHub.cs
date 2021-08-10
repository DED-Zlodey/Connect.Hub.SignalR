using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Connect.Hub.SignalR
{
    class ClientHub
    {
        HubConnection connection;
        public ClientHub()
        {
            Start();
        }
        private async void Start()
        {
            connection = new HubConnectionBuilder().WithUrl(new Uri("http://srv003.il2-expert.ru/HubDevices")).WithAutomaticReconnect().Build();
            connection.On("Send", (string message) =>
            {
                Console.WriteLine($"Message from {message}");
            });
            await connection.StartAsync();
        }
        public async void SendMess(string mess)
        {
            await connection.InvokeAsync("Send", mess);
        }
    }
}
