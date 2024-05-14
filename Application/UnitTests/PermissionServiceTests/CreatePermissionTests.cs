using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PermissionServiceTests;

public class CreatePermissionTests
{
    private readonly Mock<IPermissionRepository> _mockRepository;
    private readonly Mock<IPermissionMapper> _mockMapper;
    private readonly PermissionService _permissionService;

    public CreatePermissionTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _permissionService = new(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreatePermission_WithValidName_ShouldReturnCreatedPermission()
    {
        // Arrange
        CreateUpdatePermissionDTO createPermissionDto = new("CreatePermissionAsync", "Criar função");
        Permission permission = new() { Uuid = Guid.NewGuid(), Name = createPermissionDto.Name, Description = createPermissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        PermissionDTO permissionDTO = new(permission.Uuid, permission.Name, permission.Description);

        _mockMapper.Setup(x => x.MapToEntity(createPermissionDto)).Returns(permission);
        _mockRepository.Setup(x => x.GetByNameOrNull(permission.Name)).ReturnsAsync((Permission?)null);
        _mockRepository.Setup(x => x.Create(permission)).ReturnsAsync(permission);
        _mockMapper.Setup(x => x.MapToDto(permission)).Returns(permissionDTO);

        //Act
        PermissionDTO result = await _permissionService.CreatePermission(createPermissionDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(permission.Name, result.Name);
        Assert.Equal(permission.Description, result.Description);
    }

    [Fact]
    public async Task CreatePermission_WithInvalidName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        CreateUpdatePermissionDTO createPermissionDto = new("CreatePermissionAsync", "Criar função");
        Permission permission = new() { Uuid = Guid.NewGuid(), Name = createPermissionDto.Name, Description = createPermissionDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        _mockMapper.Setup(x => x.MapToEntity(createPermissionDto)).Returns(permission);
        _mockRepository.Setup(x => x.GetByNameOrNull(permission.Name)).ReturnsAsync(permission);

        //Act
        async Task Act() => await _permissionService.CreatePermission(createPermissionDto);

        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
    }
}