using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Message
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid? GroupId { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
