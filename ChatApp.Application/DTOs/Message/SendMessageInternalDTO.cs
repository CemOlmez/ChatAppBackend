using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Message
{
    public class SendMessageInternalDTO
    {
        public Guid SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
        public Guid? GroupId { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
    }
}
