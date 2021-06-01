using ChatRealTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRealTime.Hubs.Interfaces
{
    public interface IChat
    {
        Task SendMessage(Message message);
        Task GetMessage(Message message);
    }
}
