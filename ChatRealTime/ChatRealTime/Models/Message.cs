using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRealTime.Models
{
    public class Message
    {
        public string User { get; set; }
        public string Content { get; set; }
        public string Room { get; set; }
    }
}
