using Dapper;
using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DevFreela.Application.Services.Implementations
{
	public class ProjectService : IProjectService
	{
		private readonly DevFreelaDbContext _dbContext;
		private readonly string _connectionString;

		public ProjectService(DevFreelaDbContext dbContext, IConfiguration configuration)
		{
			_dbContext = dbContext;
			_connectionString = configuration.GetConnectionString("DevFreelaCs");
		}

		public int Create(NewProjectInputModel inputModel)
		{
			Project project = new(
				inputModel.Title,
				inputModel.Description,
				inputModel.IdClient,
				inputModel.IdFreelancer,
				inputModel.TotalCost
				);

			_dbContext.Projects.Add(project);
			_dbContext.SaveChanges();

			return project.Id;
		}

		public void Finish(int id)
		{
			Project project = _dbContext.Projects.FirstOrDefault(p => p.Id == id);

			project.Finish();
			_dbContext.SaveChanges();
		}

		public List<ProjectViewModel> GetAll(string query)
		{
			DbSet<Project> projects = _dbContext.Projects;

			List<ProjectViewModel> projectsViewModel = 
				projects.Select(p => new ProjectViewModel(p.Id, p.Title, p.CreatedAt)).ToList();

			return projectsViewModel;
		}

		public ProjectDetailsViewModel GetById(int id)
		{
			Project project = _dbContext.Projects
				.Include(p => p.Client)
				.Include(p => p.Freelancer)
				.FirstOrDefault(p => p.Id == id);

			if (project is null)
			{
				return null;
			}

			return new(
				project.Id,
				project.Title,
				project.Description,
				project.StartedAt,
				project.FinishedAt,
				project.TotalCost,
				project.Client.FullName,
				project.Freelancer.FullName
				);
		}

		public void Start(int id)
		{
			Project project = _dbContext.Projects.FirstOrDefault(p => p.Id == id);

			project.Start();

			using SqlConnection sqlConnection = new(_connectionString);
			sqlConnection.Open();
			string script = "UPDATE Projects SET Status = @status, StartedAt = @startedat WHERE Id = @id";
			sqlConnection.Execute(script, new {status = project.Status, startedat = project.StartedAt, id });
		}

		public void Update(UpdateProjectInputModel inputModel)
		{
			Project project = _dbContext.Projects.FirstOrDefault(p => p.Id == inputModel.Id);

			project.Update(inputModel.Title, inputModel.Description, inputModel.TotalCost);
			_dbContext.SaveChanges();
		}
	}
}
