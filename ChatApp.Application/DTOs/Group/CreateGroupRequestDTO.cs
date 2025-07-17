using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Group
{
    public class CreateGroupRequestDTO
    {
        public List<Guid>? UserIds { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
