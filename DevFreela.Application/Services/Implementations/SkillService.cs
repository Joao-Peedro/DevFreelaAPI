using Dapper;
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
	public class SkillService : ISkillService
	{
		private readonly DevFreelaDbContext _dbContext;
		private readonly string _connectionString;

		public SkillService(DevFreelaDbContext dbContext, IConfiguration configuration)
		{
			_dbContext = dbContext;
			_connectionString = configuration.GetConnectionString("DevFreelaCs");
		}

		public List<SkillViewModel> GetAll()
		{
			using SqlConnection sqlConnection = new(_connectionString);
			sqlConnection.Open();

			string script = "SELECT Id, Description FROM Skills";
			return sqlConnection.Query<SkillViewModel>(script).ToList();
		}
	}
}
