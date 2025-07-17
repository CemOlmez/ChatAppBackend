using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Group
{
    public class AddUserToGroupRequestDTO
    {
        public Guid GroupId { get; set; }
        public List<Guid> UserIds { get; set; } = new();
    }
}
