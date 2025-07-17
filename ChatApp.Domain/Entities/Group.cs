using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = [];
        public ICollection<Message> Messages { get; set; } = [];
    }
}
