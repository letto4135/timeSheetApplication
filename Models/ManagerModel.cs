using System;

namespace Zeit.Models
{
	public class ManagerModel : EmployeeModel
	{
        public Guid ManagerId { get; }
        public string EmployeeId { get; set; }
	}
}
