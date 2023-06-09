﻿using InternshipChat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories.Interfaces
{
    public interface IUserChatsRepository: IRepository<UserChats>
    {
        Task<IEnumerable<Chat>> GetAllUserChats(int userId);
    }
}
