namespace Application.Interfaces
{
    public interface IRoleManagerWrapper
    {
        Task<bool> RoleExistsAsync(string roleName);
    }
}
