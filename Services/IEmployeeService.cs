namespace timeSheetApplication.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeModel[]> ViewEmployeesAsync();

        Task<bool> AddEmployeeAsync(IdentityUser newEmployee);

        Task<bool> RemoveEmployeeAsync(Guid id);
    }
}