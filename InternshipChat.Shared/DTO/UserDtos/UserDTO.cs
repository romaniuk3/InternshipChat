﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.UserDtos
{
    public class UserDTO : BaseUserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public DateTime? Birhdate { get; set; }
    }
}
