using ChatRealTimeConsole.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ChatRealTimeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(2000);
            string url = "http://localhost:49577/chatHub";
            var connection = new HubConnectionBuilder().WithUrl(url)
                                                       .Build();
            connection.On<Message>("GetMessage", (message) =>
            {
                Console.WriteLine($"{message.User} - {message.Content}");
            });
            connection.StartAsync().Wait();
            while (true)
            {
                var Content = Console.ReadLine();
                connection.InvokeAsync("SendMessage", new Message() { User = "Consola", Content = Content });
            }
        }
    }
}
