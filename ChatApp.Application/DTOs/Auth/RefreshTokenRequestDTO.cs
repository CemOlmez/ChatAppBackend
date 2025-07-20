using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Auth
{
    public class RefreshTokenRequestDTO
    {
        public string RefreshToken { get; set; } = null!;
    }
}
