namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<IReadOnlyList<T>> GetAsync();

        Task<T> GetByIdAsync(int id);

        Task<T> UpdateAsync(T entity);

    }


}
