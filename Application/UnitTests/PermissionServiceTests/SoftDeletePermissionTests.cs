using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PermissionServiceTests;

public class SoftDeletePermissionTests
{
    private readonly Mock<IPermissionRepository> _mockRepository;
    private readonly Mock<IPermissionMapper> _mockMapper;
    private readonly PermissionService _permissionService;

    public SoftDeletePermissionTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _permissionService = new(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task SoftDeletePermission_WithValidId_ShouldSoftDeletedPermission()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdatePermissionDTO permissionDto = new("DeletePermissionAsync", "Deletar função");
        Permission permission = new() { Uuid = uuid, Name = permissionDto.Name, Description = permissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(permission);
        _mockRepository.Setup(x => x.SoftDelete(uuid)).Verifiable();

        //Act
        await _permissionService.SoftDeletePermission(uuid);

        //Assert
        _mockRepository.Verify(x => x.SoftDelete(uuid), Times.Once);
    }

    [Fact]
    public async Task SoftDeletePermission_WithInvalidId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdatePermissionDTO permissionDto = new("DeletePermissionAsync", "Deletar função");
        Permission permission = new() { Uuid = uuid, Name = permissionDto.Name, Description = permissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync((Permission?)null);
        _mockRepository.Setup(x => x.SoftDelete(uuid)).Verifiable();

        //Act
        async Task Act() => await _permissionService.SoftDeletePermission(uuid);

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }
}