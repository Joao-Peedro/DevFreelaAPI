using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.FinishProject;
using DevFreela.Application.Commands.StartProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Queries.GetProjectById;
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
		private readonly IMediator _mediator;
		public ProjectsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> Get(string query)
		{
			GetAllProjectsQuery getAllProjectsQuery = new(query);
			List<ProjectViewModel> projects = await _mediator.Send(getAllProjectsQuery); 

			return Ok(projects);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			GetProjectByIdQuery query = new(id);
			ProjectDetailsViewModel project = await _mediator.Send(query);

			return project is null ? NotFound() : Ok(project);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateProjectCommand command)
		{
			int id = await _mediator.Send(command);

			return CreatedAtAction(nameof(GetById), new { id }, command);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectCommand command)
		{
			await _mediator.Send(command);

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
		public async Task<IActionResult> PostComment(int id, [FromBody] CreateCommentCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}

		[HttpPut("{id}/start")]
		public async Task<IActionResult> Start(int id)
		{
			StartProjectCommand command = new(id);

			await _mediator.Send(command);

			return NoContent();
		}

		[HttpPut("{id}/finish")]
		public async Task<IActionResult> Finish(int id)
		{
			FinishProjectCommand command = new(id);

			await _mediator.Send(command);

			return NoContent();
		}
	}
}
