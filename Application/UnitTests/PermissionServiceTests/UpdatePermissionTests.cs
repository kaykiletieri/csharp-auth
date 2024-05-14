using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PermissionServiceTests;

public class UpdatePermissionTests
{
    private readonly Mock<IPermissionRepository> _mockRepository;
    private readonly Mock<IPermissionMapper> _mockMapper;
    private readonly PermissionService _permissionService;

    public UpdatePermissionTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _permissionService = new(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task UpdatePermission_WithValidId_ShouldReturnUpdatedPermission()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdatePermissionDTO permissionDto = new("UpdatePermissionAsync", "Atualizar função");
        Permission permission = new() { Uuid = uuid, Name = permissionDto.Name, Description = permissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        PermissionDTO permissionDTO = new(permission.Uuid, permission.Name, permission.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(permission);
        _mockRepository.Setup(x => x.GetByNameOrNull(permissionDto.Name)).ReturnsAsync((Permission?)null);
        _mockRepository.Setup(x => x.Update(permission)).ReturnsAsync(permission);
        _mockMapper.Setup(x => x.MapToDto(permission)).Returns(permissionDTO);

        //Act
        var result = await _permissionService.UpdatePermission(uuid, permissionDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(permissionDto.Name, result.Name);
        Assert.Equal(permissionDto.Description, result.Description);
    }

    [Fact]
    public async Task UpdatePermission_WithInvalidId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdatePermissionDTO permissionDto = new("UpdatePermissionAsync", "Atualizar função");
        Permission permission = new() { Uuid = uuid, Name = permissionDto.Name, Description = permissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        PermissionDTO permissionDTO = new(permission.Uuid, permission.Name, permission.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync((Permission?)null);
        _mockRepository.Setup(x => x.GetByNameOrNull(permissionDto.Name)).ReturnsAsync((Permission?)null);
        _mockRepository.Setup(x => x.Update(permission)).ReturnsAsync(permission);
        _mockMapper.Setup(x => x.MapToDto(permission)).Returns(permissionDTO);

        //Act
        async Task Act() => await _permissionService.UpdatePermission(uuid, permissionDto);

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    [Fact]
    public async Task UpdatePermission_WithInvalidName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdatePermissionDTO permissionDto = new("UpdatePermissionAsync", "Atualizar função");
        Permission permission = new() { Uuid = Guid.NewGuid(), Name = permissionDto.Name, Description = permissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        PermissionDTO permissionDTO = new(permission.Uuid, permission.Name, permission.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(permission);
        _mockRepository.Setup(x => x.GetByNameOrNull(permissionDto.Name)).ReturnsAsync(permission);
        _mockRepository.Setup(x => x.Update(permission)).ReturnsAsync(permission);
        _mockMapper.Setup(x => x.MapToDto(permission)).Returns(permissionDTO);

        //Act
        async Task Act() => await _permissionService.UpdatePermission(uuid, permissionDto);

        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
    }

    [Fact]
    public async Task UpdatePermission_WithSameName_ShouldReturnUpdatedPermission()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdatePermissionDTO permissionDto = new("UpdatePermissionAsync", "Atualizar função");
        Permission permission = new() { Uuid = uuid, Name = permissionDto.Name, Description = permissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        PermissionDTO permissionDTO = new(permission.Uuid, permission.Name, permission.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(permission);
        _mockRepository.Setup(x => x.GetByNameOrNull(permissionDto.Name)).ReturnsAsync(permission);
        _mockRepository.Setup(x => x.Update(permission)).ReturnsAsync(permission);
        _mockMapper.Setup(x => x.MapToDto(permission)).Returns(permissionDTO);

        //Act
        var result = await _permissionService.UpdatePermission(uuid, permissionDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(permissionDto.Name, result.Name);
        Assert.Equal(permissionDto.Description, result.Description);
    }
}