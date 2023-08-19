using DevFreela.Application.Queries.GetAllSkills;
using DevFreela.Core.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevFreela.API.Controllers
{
	[Route("api/skills")]
	public class SkillsController : ControllerBase
	{
		private readonly IMediator _mediator;
		public SkillsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			GetAllSkillsQuery query = new();

			List<SkillDTO> skills = await _mediator.Send(query);

			return Ok(skills);
		}
	}
}
