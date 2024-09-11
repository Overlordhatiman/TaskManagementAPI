using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;
using Xunit;
using Task = TaskManagementAPI.Models.Task;

namespace TaskManagementAPI.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskDbRepository> _taskRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TaskService>> _loggerMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskDbRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TaskService>>();
            _taskService = new TaskService(_taskRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldCallRepositoryAndReturnTask()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskDto = new TaskDTO { Title = "Test Task", Description = "Description" };
            var taskEntity = new Task { Id = Guid.NewGuid(), Title = "Test Task", Description = "Description", UserId = userId };

            _mapperMock.Setup(m => m.Map<Task>(taskDto)).Returns(taskEntity);
            _mapperMock.Setup(m => m.Map<TaskDTO>(taskEntity)).Returns(taskDto);
            _taskRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TaskManagementAPI.Models.Task>()))
            .Returns((TaskManagementAPI.Models.Task newTask) => {
                return taskEntity; // Returning the mocked task entity
            });

            // Act
            var result = await _taskService.CreateTaskAsync(userId, taskDto);

            // Assert
            Assert.Equal(taskDto, result);
            _taskRepositoryMock.Verify(r => r.AddAsync(It.Is<Task>(t => t.UserId == userId)), Times.Once);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>(), taskEntity.Title, userId), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnTaskIfFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var taskEntity = new Task { Id = taskId, UserId = userId };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(taskEntity);
            var taskDto = new TaskDTO();
            _mapperMock.Setup(m => m.Map<TaskDTO>(taskEntity)).Returns(taskDto);

            // Act
            var result = await _taskService.GetTaskByIdAsync(userId, taskId);

            // Assert
            Assert.Equal(taskDto, result);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>(), taskId, userId), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldThrowKeyNotFoundException_IfTaskNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((Task)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.GetTaskByIdAsync(userId, taskId));
            _loggerMock.Verify(l => l.LogWarning(It.IsAny<string>(), taskId, userId), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_ShouldDeleteTask_WhenTaskExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var taskEntity = new Task { Id = taskId, UserId = userId };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(taskEntity);
            _taskRepositoryMock.Setup(r => r.DeleteAsync(taskId)).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            await _taskService.DeleteTaskAsync(userId, taskId);

            // Assert
            _taskRepositoryMock.Verify(r => r.DeleteAsync(taskId), Times.Once);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>(), taskId, userId), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldUpdateTask_WhenTaskExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var taskDto = new TaskDTO { Title = "Updated Task", Description = "Updated Description" };
            var taskEntity = new Task { Id = taskId, UserId = userId };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(taskEntity);
            _taskRepositoryMock.Setup(r => r.UpdateAsync(taskEntity)).Returns(System.Threading.Tasks.Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<TaskDTO>(taskEntity)).Returns(taskDto);

            // Act
            var result = await _taskService.UpdateTaskAsync(userId, taskId, taskDto);

            // Assert
            Assert.Equal(taskDto, result);
            _taskRepositoryMock.Verify(r => r.UpdateAsync(taskEntity), Times.Once);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>(), taskId, userId), Times.Once);
        }
    }
}
