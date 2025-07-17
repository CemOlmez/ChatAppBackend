using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs
{
    public class AddContactRequestDTO
    {
        public Guid RequesterId { get; set; }
        public string TargetUsername { get; set; } = string.Empty;
    }
}
