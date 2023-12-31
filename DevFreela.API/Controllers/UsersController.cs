﻿using DevFreela.Application.Commands.CreateUser;
using DevFreela.Application.Queries.GetUser;
using DevFreela.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevFreela.API.Controllers
{
	[Route("api/users")]
	public class UsersController : ControllerBase
	{
		private readonly IMediator _mediator;
		public UsersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			GetUserQuery query = new(id);
			UserViewModel user = await _mediator.Send(query);

			return user is null ? NotFound() : Ok(user);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
		{
			int id = await _mediator.Send(command);

			return CreatedAtAction(nameof(GetById), new { id }, command);
		}

		[HttpPut("{id}/login")]
		public IActionResult Login(int id, [FromBody] LoginModel login)
		{
			return NoContent();
		}
	}
}
