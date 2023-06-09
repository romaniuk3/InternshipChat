﻿using InternshipChat.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.ChatDtos
{
    public class ChatAttachmentDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public FileModel Attachment { get; set; }
        public bool RequiresSignature { get; set; }
        public int? ReceiverId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileText { get; set; } = string.Empty;
        public string AttachmentUrl { get; set; } = string.Empty;
    }
}
