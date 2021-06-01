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
        public async Task SendMessage(Message message)
        {
            await Clients.All.GetMessage(message);
        }
    }
}
