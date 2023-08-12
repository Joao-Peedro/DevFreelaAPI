using System;

namespace DevFreela.Application.ViewModels
{
	public class ProjectDetailsViewModel
	{
		public ProjectDetailsViewModel(int id, string title, string description,
			DateTime? startedAt, DateTime? finishedAt, decimal totalCost,
			string clientFullName, string freelancerFullName)
		{
			Id = id;
			Title = title;
			Description = description;
			StartedAt = startedAt;
			FinishedAt = finishedAt;
			TotalCost = totalCost;
			ClientFullName = clientFullName;
			FreelancerFullName = freelancerFullName;
		}

		public int Id { get; private set; }
		public string Title { get; private set; }
		public string Description { get; private set; }
		public DateTime? StartedAt { get; private set; }
		public DateTime? FinishedAt { get; private set; }
		public decimal TotalCost { get; private set; }
		public string ClientFullName { get; private set; }
		public string FreelancerFullName { get; private set; }
	}
}
