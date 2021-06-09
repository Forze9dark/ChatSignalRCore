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
        public static Dictionary<string, string> Usuarios { get; set; } = new Dictionary<string, string>();
        public async Task SendMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.Content))
                await Clients.All.GetMessage(message);
            else if (!string.IsNullOrEmpty(message.User))
            {
                Usuarios.Add(Context.ConnectionId, message.User);
                await Clients.AllExcept(Context.ConnectionId).SendMessage(new Message() { User = message.User, Content = "Se ha conectado!" });
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
            _ = Usuarios.TryGetValue(connectionId, out string usuario);
            await Clients.AllExcept(connectionId).GetMessage(new Message() { User = "Host", Content = $"{usuario} Se ha desconectado." });
            Usuarios.Remove(connectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
