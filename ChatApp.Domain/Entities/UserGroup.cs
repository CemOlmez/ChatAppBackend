using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class UserGroup
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }

        public User User { get; set; } = null!;
        public Group Group { get; set; } = null!;
    }
}
