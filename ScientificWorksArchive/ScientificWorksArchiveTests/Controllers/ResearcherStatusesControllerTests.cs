using ScientificWorksArchive.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ScientificWorksArchive.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ScientificWorksArchive.Controllers.Tests;

public class ResearcherStatusesControllerTests
{
    [Fact]
    public async Task GetResearcherStatuses_ReturnsIEnumerableResearcherStatuses()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);
        await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
        await dbContext.SaveChangesAsync();

        var controller = new ResearcherStatusesController(dbContext);

        // Act
        var result = await controller.GetResearcherStatuses();

        // Assert
        var iEnumerableResult = Xunit.Assert.IsType<ActionResult<IEnumerable<ResearcherStatus>>>(result);
        Xunit.Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task GetResearcherStatus_ReturnsNotFoundResult_WhenStatusNotFound()
    {
        // Arrange
        int id = 3;

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.GetResearcherStatus(id);

        // Assert
        var actionResult = Xunit.Assert.IsType<ActionResult<ResearcherStatus>>(result);
        Xunit.Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetResearcherStatus_ReturnsStatusResult_WhenStatusFound()
    {
        // Arrange
        int id = 1;

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);
        await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
        await dbContext.SaveChangesAsync();

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.GetResearcherStatus(id);

        // Assert
        var statusResult = Xunit.Assert.IsType<ActionResult<ResearcherStatus>>(result);
        var status = Xunit.Assert.IsType<ResearcherStatus>(statusResult.Value);
        Xunit.Assert.Equal("Test status 1", status.Name);
        Xunit.Assert.Equal("Test description 1", status.Description);
        Xunit.Assert.Equal(id, status.Id);
    }

    [Fact]
    public async Task PutResearcherStatus_ReturnsBadRequestResult_WhenIdNotValid()
    {
        // Arrange
        int id = 1;
        ResearcherStatus inputStatus = new ResearcherStatus()
        {
            Id = 2,
            Name = "UpdatedName",
            Description = "UpdatedDescriptio"
        };

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);
        await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
        await dbContext.SaveChangesAsync();

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.PutResearcherStatus(id, inputStatus);

        // Assert
        var badRequestResult = Xunit.Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task PutResearcherStatus_ReturnsNoContentResult_OnSuccess()
    {
        // Arrange
        int id = 2;
        ResearcherStatus inputStatus = new ResearcherStatus()
        {
            Id = 2,
            Name = "UpdatedName",
            Description = "UpdatedDescription"
        };

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        // Seed the database with initial data
        await using (var dbContext = new ScientificWorksArchiveAPIContext(options))
        {
            await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
            await dbContext.SaveChangesAsync();
        }

        // Create a new context for the test action
        await using (var dbContext = new ScientificWorksArchiveAPIContext(options))
        {
            var controller = new ResearcherStatusesController(dbContext);

            // Act
            var result = await controller.PutResearcherStatus(id, inputStatus);

            // Assert
            var noContentResult = Xunit.Assert.IsType<NoContentResult>(result);
            var updatedStatus = await dbContext.ResearcherStatuses.FirstOrDefaultAsync(status => status.Id == id);
            Xunit.Assert.Equal(inputStatus, updatedStatus);
        }
    }

    [Fact]
    public async Task PutResearcherStatus_ReturnsNotFoundResult_WhenDbUpdateConcurrencyExceptionThrown()
    {
        // Arrange
        int id = 2;
        ResearcherStatus inputStatus = new ResearcherStatus()
        {
            Id = 2,
            Name = "UpdatedName",
            Description = "UpdatedDescription"
        };

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.PutResearcherStatus(id, inputStatus);

        // Assert
        var badRequestResult = Xunit.Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PostResearcherStatus_ReturnsCreatedAtActionResult()
    {
        // Arrange
        ResearcherStatus inputStatus = new ResearcherStatus()
        {
            Name = "NewName",
            Description = "NewDescription"
        };

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);
        await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
        await dbContext.SaveChangesAsync();

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.PostResearcherStatus(inputStatus);

        // Assert
        var actionResult = Xunit.Assert.IsType<ActionResult<ResearcherStatus>>(result);
        var createdAtActionResult = Xunit.Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnValue = Xunit.Assert.IsType<ResearcherStatus>(createdAtActionResult.Value);
        Xunit.Assert.Equal("NewName", returnValue.Name);
        Xunit.Assert.Equal("NewDescription", returnValue.Description);
        Xunit.Assert.Equal(3, dbContext.ResearcherStatuses.Count());
    }

    [Fact]
    public async Task DeleteResearcherStatus_ReturnsNotFoundResult_WhenInvalidId()
    {
        // Arrange
        int id = 3;

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);
        await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
        await dbContext.SaveChangesAsync();

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.DeleteResearcherStatus(id);

        // Assert
        var actionResult = Xunit.Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteResearcherStatus_ReturnsNoContentResult_OnSuccess()
    {
        // Arrange
        int id = 1;

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);
        await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
        await dbContext.SaveChangesAsync();

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.DeleteResearcherStatus(id);

        // Assert
        var actionResult = Xunit.Assert.IsType<NoContentResult>(result);
        Xunit.Assert.Equal(1, dbContext.ResearcherStatuses.Count());
    }

    [Fact]
    public async Task GetResearchersWithStatus_ReturnsIEnumerableResearcher_OnSuccess()
    {
        // Arrange
        int id = 1;

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);
        await dbContext.ResearcherStatuses.AddRangeAsync(GetTestStatuses());
        await dbContext.SaveChangesAsync();

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.GetResearchersWithStatus(id);

        // Assert
        var actionResult = Xunit.Assert.IsType<ActionResult<IEnumerable<Researcher>>>(result);
        Xunit.Assert.Equal(0, actionResult.Value.Count());
    }

    [Fact]
    public async Task GetResearchersWithStatus_ReturnsNotFoundResult_WhenInvalidId()
    {
        // Arrange
        int id = 1;

        var options = new DbContextOptionsBuilder<ScientificWorksArchiveAPIContext>()
           .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
           .Options;

        await using var dbContext = new ScientificWorksArchiveAPIContext(options);

        var controller = new ResearcherStatusesController(dbContext);

        // Action
        var result = await controller.GetResearchersWithStatus(id);

        // Assert
        var actionResult = Xunit.Assert.IsType<ActionResult<IEnumerable<Researcher>>>(result);
        Xunit.Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    private List<ResearcherStatus> GetTestStatuses()
    {
        var list = new List<ResearcherStatus>()
        {
            new ResearcherStatus
            {
                Id = 1,
                Name = "Test status 1",
                Description = "Test description 1",
            },
            new ResearcherStatus
            {
                Id = 2,
                Name = "Test status 2",
                Description = "Test description 2",
            }
        };
        return list;
    }
}