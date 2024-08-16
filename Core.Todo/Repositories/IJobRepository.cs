using Core.Todo.Entities;

namespace Core.Todo.Repositories
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<List<Job>> GetAllDoneByUser(long id);
        Task<List<Job>> GetAllNotDoneByUser(long id);
        Task DoneJob(Job job);
    }
}
