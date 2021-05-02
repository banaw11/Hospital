using HospitalAPI.DTOs;
using HospitalAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<CreatedUserAccountDTO>> Register([FromBody] RegisterUserDTO dto)
        {
            var newUserAccount = await _accountService.RegisterUser(dto);

            return Ok(newUserAccount);
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<string>> SignIn([FromBody] LoginUserDTO dto)
        {
            var token = await _accountService.SignInUser(dto);

            return Ok(token);
        }
    }
}
