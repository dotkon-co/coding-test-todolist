using Core.Todo.Entities;
using Infrastructure.Todo.Context;
using Infrastructure.Todo.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class UserRepositoryTests
{
    private readonly DbContextOptions<TodoContext> _dbContextOptions;

    public UserRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "TodoDb")
            .Options;
    }

    [Fact]
    public async Task Create_ShouldAddUserToDatabase_AndGeneratePasswordHash()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new UserRepository(context);
        var user = new User { Name = "Test User", Email = "test@example.com", Password = "password123" };

        // Act
        var createdUser = await repository.Create(user);
        var result = await context.Users.FindAsync(createdUser.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.Name);
        Assert.NotNull(result.PasswordHash);
        Assert.NotNull(result.PasswordSalt);
    }

    [Fact]
    public async Task Delete_ShouldRemoveUserFromDatabase()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new UserRepository(context);
        var user = new User { Name = "User to be deleted", Email = "delete@example.com", Password = "password123" };
        await repository.Create(user);

        // Act
        await repository.Delete(user.Id);
        var result = await context.Users.FindAsync(user.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new UserRepository(context);
        await repository.Create(new User { Name = "User 1", Email = "user1@example.com", Password = "password123" });
        await repository.Create(new User { Name = "User 2", Email = "user2@example.com", Password = "password123" });

        // Act
        var result = await repository.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnUserById()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new UserRepository(context);
        var user = new User { Name = "Specific User", Email = "specific@example.com", Password = "password123" };
        await repository.Create(user);

        // Act
        var result = await repository.GetById(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task Update_ShouldModifyExistingUser()
    {
        // Arrange
        using var context = new TodoContext(_dbContextOptions);
        var repository = new UserRepository(context);
        var user = new User { Name = "Old Name", Email = "old@example.com", Password = "password123" };
        await repository.Create(user);

        // Act
        user.Name = "New Name";
        user.Email = "new@example.com";
        await repository.Update(user);
        var result = await context.Users.FindAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Name", result.Name);
        Assert.Equal("new@example.com", result.Email);
    }
}
