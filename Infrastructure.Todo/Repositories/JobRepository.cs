using Core.Todo.Entities;
using Core.Todo.Repositories;
using Infrastructure.Todo.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Todo.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly TodoContext _context;
        private bool _disposed = false;
        public JobRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<Job> Create(Job entity)
        {
            await _context.Jobs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(long id)
        {
            Job job = await _context.Jobs.FindAsync(id);
            if (job is not null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Job>> GetAll() =>
            await _context.Jobs.ToListAsync();

        public async Task<Job> GetById(long id) =>
            await _context.Jobs.FindAsync(id);

        public async Task<Job> Update(Job entity)
        {
            Job job = await _context.Jobs.FindAsync(entity.Id);

            job.Description = entity.Description;
            job.Name = entity.Name;
            job.Title = entity.Title;
            job.UserId = entity.UserId;
            job.CreationDate = entity.CreationDate;
            job.EndDate = entity.EndDate;

            await _context.SaveChangesAsync();
            return job;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<List<Job>> GetAllDoneByUser(long id)
        {
            List<Job> jobs = await _context.Jobs
                .Where(j => j.UserId == id)
                .Where(j => j.Done)
                .ToListAsync();
            return jobs;
        }

        public async Task<List<Job>> GetAllNotDoneByUser(long id)
        {
            List<Job> jobs = await _context.Jobs
                .Where(j => j.UserId == id)
                .Where(j => !j.Done)
                .ToListAsync();
            return jobs;
        }

        public async Task DoneJob(Job job)
        {
            Job _job = await _context.Jobs.FindAsync(job.Id);

            _job.Done = true;

            await _context.SaveChangesAsync();
        }
    }
}
