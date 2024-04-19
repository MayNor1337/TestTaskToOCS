using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using CFPService.Domain.Services;
using Moq;

namespace CFPService.Domain.UnitTest;

public class ApplicationServiceTest
{
    [Fact]
    public void CreateApplication_EntityWillBeAdded()
    {
        // Arrange
        var repositoryMock = new Mock<IApplicationRepository>();
        var ser = new ApplicationService(repositoryMock.Object);
        var id = Guid.NewGuid();
        var data = new ApplicationData("a", "n", "d", "o");

        // Act
        var res = ser.CreateApplication(id, data);

        // Assert
        repositoryMock.Verify(ar =>
            ar.InsertApplication(It.IsAny<ApplicationEntity>()), Times.Once);
    }

    [Fact]
    public async Task EditApplication_EntityWillBeEdit()
    {
        // Arrange
        var id = Guid.NewGuid();
        var data = new ApplicationData("a", "n", "d", "o");
        var application = new ApplicationEntity(id, data);
        var repositoryMock = new Mock<IApplicationRepository>();

        repositoryMock.Setup(x => x.GetApplication(It.IsAny<Guid>()))
            .ReturnsAsync(new  GetApplicationResult.ApplicationFound(application));
        repositoryMock.Setup(x => x.UpdateApplication(It.IsAny<ApplicationEntity>()))
            .ReturnsAsync(new GetApplicationResult.ApplicationFound(application));

        var ser = new ApplicationService(repositoryMock.Object);
        
        // Act
        var res = await ser.EditApplication(id, data);
        
        // Assert
        repositoryMock.Verify(ar =>
            ar.UpdateApplication(It.IsAny<ApplicationEntity>()), Times.Once);
    }
}