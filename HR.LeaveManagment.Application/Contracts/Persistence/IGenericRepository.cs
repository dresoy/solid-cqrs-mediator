namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    internal interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);

        Task<T> DeleteAsync(T entity);

        Task<T> GetAsync();

        Task<T> GetByIdAsync(int id);

        Task<T> UpdateAsync(T entity);

    }


}
