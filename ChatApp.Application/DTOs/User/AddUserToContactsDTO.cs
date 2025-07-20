using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.User
{
    public class AddUserToContactsDTO
    {
        public Guid RequesterId { get; set; }
        public string TargetUsername { get; set; } = string.Empty;
    }
}
