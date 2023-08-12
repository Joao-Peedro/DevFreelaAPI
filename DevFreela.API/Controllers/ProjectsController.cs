using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevFreela.API.Controllers
{
	[Route("api/projects")]
	public class ProjectsController : ControllerBase
	{
		private readonly IProjectService _projectService;
		private readonly IMediator _mediator;
		public ProjectsController(IProjectService projectService, IMediator mediator)
		{
			_projectService= projectService;
			_mediator = mediator;
		}

		[HttpGet]
		public IActionResult Get(string query)
		{
			List<ProjectViewModel> projects = _projectService.GetAll(query);

			return Ok(projects);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var project = _projectService.GetById(id);

			if (project is null)
			{
				return NotFound();
			}

			return Ok(project);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateProjectCommand command)
		{
			if (command.Title.Length > 50)
			{
				return BadRequest();
			}

			int id = await _mediator.Send(command);

			return CreatedAtAction(nameof(GetById), new { id }, command);
		}

		[HttpPut("{id}")]
		public IActionResult Put([FromBody] UpdateProjectInputModel inputModel)
		{
			if (inputModel.Description.Length > 200)
			{
				return BadRequest();
			}

			_projectService.Update(inputModel);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			DeleteProjectCommand command = new(id);

			await _mediator.Send(command);

			return NoContent();
		}

		[HttpPost("{id}/comments")]
		public async Task<IActionResult> PostComment(int id,[FromBody] CreateCommentCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}

		[HttpPut("{id}/start")]
		public IActionResult Start(int id)
		{
			_projectService.Start(id);

			return NoContent();
		}

		[HttpPut("{id}/finish")]
		public IActionResult Finish(int id)
		{
			_projectService.Finish(id);

			return NoContent();
		}
	}
}
