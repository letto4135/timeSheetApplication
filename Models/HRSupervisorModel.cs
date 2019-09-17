using System;

namespace Zeit.Models
{
	public class HRSupervisor : ManagerModel
	{
        public Guid HRSupervisorId { get; }
        public string EmployeeId { get; set; }
	}
}
