using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; } // always required
        public Guid? ReceiverId { get; set; } // for 1-on-1 messages
        public Guid? GroupId { get; set; } // for group messages

        public string Content { get; set; } = string.Empty;
        public string? FileUrl { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User Sender { get; set; } = null!;
        public User? Receiver { get; set; } // can be null if it's a group message
        public Group? Group { get; set; } // can be null if it's a 1-on-1 message
    }
}
