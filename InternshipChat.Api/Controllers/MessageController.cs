﻿using AutoMapper;
using InternshipChat.Api.Extensions;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public MessageController(IMapper mapper, IMessageService messageService)
        {
            _mapper = mapper;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageDTO createMessageDto)
        {
            var message = _mapper.Map<Message>(createMessageDto);
            var res = _messageService.SendMessage(message);

            return Ok(res);
        }

        [HttpGet]
        [Route("{chatId}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetAllMessages(int chatId)
        {
            var messagesResult = await _messageService.GetMessagesAsync(chatId);
            if (messagesResult.IsFailure)
            {
                return this.FromError(messagesResult.Error);
            }

            var messageDtos = _mapper.Map<IEnumerable<MessageDTO>>(messagesResult.Value);

            return Ok(messageDtos);
        }
    }
}
