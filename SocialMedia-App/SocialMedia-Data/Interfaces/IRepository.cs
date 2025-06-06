namespace SocialMedia.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<List<T>> GetAllAsync();
        public Task AddAsync(T entity);
        public void Remove(T entity);
        public Task SaveAsync();
    }
}
