﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Group
{
    public class GroupDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsPrivate { get; set; }
    }
}
