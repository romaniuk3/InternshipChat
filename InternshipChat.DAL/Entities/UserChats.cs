﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Entities
{
    public class UserChats
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public User? User { get; set; }
        public Chat? Chat { get; set; }
    }
}
