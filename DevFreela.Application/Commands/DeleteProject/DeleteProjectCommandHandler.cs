﻿using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.DeleteProject
{
	public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
	{
		private readonly DevFreelaDbContext _dbContext;

		public DeleteProjectCommandHandler(DevFreelaDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
		{
			Project project = _dbContext.Projects.FirstOrDefault(p => p.Id == request.Id);

			project.Cancel();
			await _dbContext.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
