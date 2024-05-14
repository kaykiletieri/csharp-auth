using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class UpdateRoleTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly RoleService _roleService;

    public UpdateRoleTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockRolePermissionRepository = new();
        _mockPermissionRepository = new();

        _roleService = new RoleService(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task UpdateRole_WithValidId_ShouldReturnUpdatedRole()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();
        CreateUpdateRoleDTO roleDto = new("UpdateRoleAsync", "Atualizar função");
        Role role = new() { Uuid = roleUuid, Name = roleDto.Name, Description = roleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        RoleDTO roleDTO = new(role.Uuid, role.Name, role.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(roleUuid)).ReturnsAsync(role);
        _mockRepository.Setup(x => x.GetByNameOrNull(roleDto.Name)).ReturnsAsync((Role?)null);
        _mockRepository.Setup(x => x.Update(role)).ReturnsAsync(role);
        _mockMapper.Setup(x => x.MapToDto(role)).Returns(roleDTO);

        // Act
        var result = await _roleService.UpdateRole(roleUuid, roleDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roleDto.Name, result.Name);
        Assert.Equal(roleDto.Description, result.Description);
    }

    [Fact]
    public async Task UpdateRole_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();
        CreateUpdateRoleDTO roleDto = new("UpdateRoleAsync", "Atualizar função");
        Role role = new() { Uuid = roleUuid, Name = roleDto.Name, Description = roleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        RoleDTO roleDTO = new(role.Uuid, role.Name, role.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(roleUuid)).ReturnsAsync((Role?)null);
        _mockRepository.Setup(x => x.GetByNameOrNull(roleDto.Name)).ReturnsAsync((Role?)null);
        _mockRepository.Setup(x => x.Update(role)).ReturnsAsync(role);
        _mockMapper.Setup(x => x.MapToDto(role)).Returns(roleDTO);

        // Act
        async Task act() => await _roleService.UpdateRole(roleUuid, roleDto);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task UpdateRole_WithInvalidName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();
        CreateUpdateRoleDTO roleDto = new("UpdateRoleAsync", "Atualizar função");
        Role role = new() { Uuid = Guid.NewGuid(), Name = roleDto.Name, Description = roleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        RoleDTO roleDTO = new(role.Uuid, role.Name, role.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(roleUuid)).ReturnsAsync(role);
        _mockRepository.Setup(x => x.GetByNameOrNull(roleDto.Name)).ReturnsAsync(role);
        _mockRepository.Setup(x => x.Update(role)).ReturnsAsync(role);
        _mockMapper.Setup(x => x.MapToDto(role)).Returns(roleDTO);

        // Act
        async Task act() => await _roleService.UpdateRole(roleUuid, roleDto);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }

    [Fact]
    public async Task UpdateRole_WithSameName_ShouldReturnUpdatedRole()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdateRoleDTO roleDto = new("UpdateRoleAsync", "Atualizar função");
        Role role = new() { Uuid = uuid, Name = roleDto.Name, Description = roleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        RoleDTO roleDTO = new(role.Uuid, role.Name, role.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(role);
        _mockRepository.Setup(x => x.GetByNameOrNull(roleDto.Name)).ReturnsAsync(role);
        _mockRepository.Setup(x => x.Update(role)).ReturnsAsync(role);
        _mockMapper.Setup(x => x.MapToDto(role)).Returns(roleDTO);

        // Act
        var result = await _roleService.UpdateRole(uuid, roleDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roleDto.Name, result.Name);
        Assert.Equal(roleDto.Description, result.Description);
    }
}
