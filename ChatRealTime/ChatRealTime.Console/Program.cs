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

            Console.WriteLine("Escoga la sala 1 => Sala1, 2 => Sala2, 3 => Sala3");
            var room = Console.ReadLine();

            var roomSelected = SelectRoom(room);

            string url = "http://localhost:49577/chatHub";
            var connection = new HubConnectionBuilder().WithUrl(url)
                                                       .Build();

            connection.On<Message>("GetMessage", (message) =>
            {
                Console.WriteLine($"{message.User} - {message.Content} {message}");
            });
            connection.StartAsync().Wait();
            connection.InvokeAsync("SendMessage", new Message() { User = "Consola", Content = "", Room = room });
            while (true)
            {
                var Content = Console.ReadLine();
                connection.InvokeAsync("SendMessage", new Message() { User = "Consola", Content = Content, Room = room });
            }
        }

        static string SelectRoom(string room)
        {
            switch (room)
            {
                case "1":
                    room = "Sala1";
                    break;
                case "2":
                    room = "Sala2";
                    break;
                case "3":
                    room = "Sala3";
                    break;
                default:
                    break;
            }
            return room;
        }
    }
}
