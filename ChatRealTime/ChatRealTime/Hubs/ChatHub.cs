using ChatRealTime.Hubs.Interfaces;
using ChatRealTime.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRealTime.Hubs
{
    public class ChatHub : Hub<IChat>
    {
        public static Dictionary<string, (string, string)> Users { get; set; } = new Dictionary<string, (string, string)>();
        public async Task SendMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.Content))
                await Clients.Group(message.Room).GetMessage(message);
            else if (!string.IsNullOrEmpty(message.User))
            {
                Users.Add(Context.ConnectionId, (message.User, message.Room));
                await Groups.AddToGroupAsync(Context.ConnectionId, message.Room);
                await Clients.GroupExcept(message.Room, Context.ConnectionId).SendMessage(new Message() { User = message.User, Content = "Se ha conectado!" });
            }
        }

        public async IAsyncEnumerable<int> CounterAsync()
        {
            for (int i = 1; i < 100000; i++)
            {
                yield return i;
                await Task.Delay(1000);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).GetMessage(new Message() { User = "Host", Content = "Hola, Bienvenido al chat." });
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            await Clients.GroupExcept(Users[Context.ConnectionId].Item2, connectionId)
                         .GetMessage(new Message() { User = "Host", Content = $"{Users[connectionId].Item1} Se ha desconectado." });
            Users.Remove(connectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
