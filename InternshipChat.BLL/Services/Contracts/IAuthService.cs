﻿using InternshipChat.BLL.ServiceResult;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IAuthService
    {
        Task<Result> Register(RegisterUserDTO registerUserDTO);
        Task<Result<LoginResult>> Login(LoginDto loginDto);
        Task<Result> ChangePassword(ChangePasswordModel model);
    }
}
