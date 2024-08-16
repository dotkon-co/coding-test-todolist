using Core.Todo.Entities;
using Infrastructure.Todo.Context;
using Infrastructure.Todo.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class JobRepositoryTests
{
    private readonly DbContextOptions<TodoContext> _dbContextOptions;

    public JobRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "Todo")
            .Options;
    }

    [Fact]
    public async Task Create_ShouldAddJobToDatabase()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new JobRepository(context);
        var job = new Job { Id = 1, Name = "Test Job", Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow, UserId = 1, Done = false };

        // Act
        await repository.Create(job);
        var result = await context.Jobs.FindAsync(job.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Job", result.Name);
    }

    [Fact]
    public async Task Delete_ShouldRemoveJobFromDatabase()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new JobRepository(context);
        var job = new Job { Id = 2, Name = "Job to be deleted", Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow, UserId = 1, Done = false };
        await context.Jobs.AddAsync(job);
        await context.SaveChangesAsync();

        // Act
        await repository.Delete(job.Id);
        var result = await context.Jobs.FindAsync(job.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllJobs()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new JobRepository(context);
        await context.Jobs.AddRangeAsync(new List<Job>
        {
            new Job { Id = 3, Name = "Job 1",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow, UserId = 1, Done = false },
            new Job { Id = 4, Name = "Job 2",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow, UserId = 1, Done = true }
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Update_ShouldModifyExistingJob()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new JobRepository(context);
        var job = new Job { Id = 5, Name = "Job to be updated", Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow, UserId = 1, Done = false };
        await context.Jobs.AddAsync(job);
        await context.SaveChangesAsync();

        job.Name = "Updated Job Name";

        // Act
        await repository.Update(job);
        var result = await context.Jobs.FindAsync(job.Id);

        // Assert
        Assert.Equal("Updated Job Name", result.Name);
    }

    [Fact]
    public async Task DoneJob_ShouldMarkJobAsDone()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new JobRepository(context);
        var job = new Job { Id = 6, Name = "Job to be marked as done", Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow, UserId = 1, Done = false };
        await context.Jobs.AddAsync(job);
        await context.SaveChangesAsync();

        // Act
        await repository.DoneJob(job);
        var result = await context.Jobs.FindAsync(job.Id);

        // Assert
        Assert.True(result.Done);
    }

    [Fact]
    public async Task GetAllDoneByUser_ShouldReturnOnlyDoneJobs()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new JobRepository(context);

        long userId = 1;
        await context.Jobs.AddRangeAsync(new List<Job>
        {
            new Job { Id = 1, Name = "Job 1",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow,  Done = true, UserId = userId },
            new Job { Id = 2, Name = "Job 2",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow, Done = false, UserId = userId },
            new Job { Id = 3, Name = "Job 3",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow,  Done = true, UserId = userId }
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllDoneByUser(userId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, job => Assert.True(job.Done));
    }

    [Fact]
    public async Task GetAllNotDoneByUser_ShouldReturnOnlyNotDoneJobs()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new JobRepository(context);

        long userId = 1;
        await context.Jobs.AddRangeAsync(new List<Job>
        {
            new Job { Id = 1, Name = "Job 1",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow,  Done = true, UserId = userId },
            new Job { Id = 2, Name = "Job 2",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow,  Done = false, UserId = userId },
            new Job { Id = 3, Name = "Job 3",Title = "Teste Tittle", Description = "Teste Description", CreationDate = DateTime.Today, EndDate = DateTime.UtcNow,  Done = false, UserId = userId }
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllNotDoneByUser(userId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, job => Assert.False(job.Done));
    }
}

