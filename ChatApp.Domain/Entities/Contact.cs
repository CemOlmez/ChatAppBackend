using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class Contact
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }  // The user who added the contact
        public Guid ContactUserId { get; set; }  // The user who is added

        public User Owner { get; set; } = null!;
        public User ContactUser { get; set; } = null!;
    }
}
