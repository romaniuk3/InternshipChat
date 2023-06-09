﻿using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        private readonly ChatContext _chatContext;

        public ChatRepository(ChatContext chatContext) : base(chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<Chat> GetChatById(int id)
        {
            var chat = await _chatContext.Chats
                .Include(x => x.UserChats)
                .ThenInclude(y => y.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            return chat;
        } 

        public async Task<Chat?> GetChatByName(string name)
        {
            return await _chatContext.Chats.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<ChatInfoView>> GetAllChats()
        {
            var chats = await _chatContext.Set<ChatInfoView>().ToListAsync();
            return chats;
        }

        public async Task SaveAttachment(ChatAttachment chatAttachment)
        {
            await _chatContext.ChatAttachments.AddAsync(chatAttachment);
        }

        public async Task<ChatAttachment?> GetChatAttachment(int id)
        {
            return await _chatContext.ChatAttachments.FirstOrDefaultAsync(ca => ca.Id == id);
        }

        public async Task<IEnumerable<ChatAttachment>> GetUserSignatureAttachments(int chatId, int userId)
        {
            var signatureAttachmentsInChat = await _chatContext.ChatAttachments.Where(ca => ca.ChatId == chatId && (ca.SenderId == userId || ca.ReceiverId == userId)).ToListAsync();

            return signatureAttachmentsInChat;
        }

        public async Task<IEnumerable<ChatAttachment>> GetAllChatAttachments(int chatId)
        {
            var attachments = await _chatContext.ChatAttachments.Where(ca => ca.ChatId == chatId && ca.ReceiverId == null).ToListAsync();

            return attachments;
        }

        public async Task DeleteAttachment(ChatAttachment chatAttachment)
        {
            _chatContext.ChatAttachments.Remove(chatAttachment);
        }
    }
}
