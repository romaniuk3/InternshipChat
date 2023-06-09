﻿using InternshipChat.BLL.ServiceResult;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IChatService
    {
        Task<Result> CreateChatAsync(CreateChatDTO chat);
        Task<IEnumerable<ChatInfoView>> GetAllChatsAsync();
        Task<Result<Chat>> GetChatAsync(int id);
        Task<Result<IEnumerable<Chat>>> GetUserChatsAsync(int userId);
        Task<Result> AddUserToChatAsync(int chatId, int userId);
        Task<Result<IEnumerable<ChatAttachment>>> GetChatAttachments(int chatId);
        Task<Result<ChatAttachment>> AddChatAttachment(ChatAttachment chatAttachment);
        Task<Result<IEnumerable<ChatAttachment>>> GetUserSignatureAttachments(int chatId, int userId);
        Task<Result> UpdateAttachment(int attachmentId, ChatAttachment chatAttachment);
        Task<Result> DeleteAttachment(int attachmentId);
    }
}
