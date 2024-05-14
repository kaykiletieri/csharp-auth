using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class GetRoleByIdWithPermissionsOrNullTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly RoleService _roleService;

    public GetRoleByIdWithPermissionsOrNullTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockRolePermissionRepository = new();
        _mockPermissionRepository = new();

        _roleService = new RoleService(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task GetRoleByIdWithPermissionsOrNull_WhenRoleExists_ReturnsRoleWithPermissions()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();
        Role role = new() { Uuid = roleUuid, Name = "role", CreatedAt = new DateTime(), UpdatedAt = new DateTime() };
        Permission permission = new() { Uuid = Guid.NewGuid(), Name = "permission", CreatedAt = new DateTime(), UpdatedAt = new DateTime() };
        RolePermission rolePermission = new() { Uuid = Guid.NewGuid(), RoleUuid = role.Uuid, PermissionUuid = permission.Uuid, CreatedAt = new DateTime(), UpdatedAt = new DateTime() };

        _mockRepository.Setup(x => x.GetByIdWithPermissionsOrNull(roleUuid)).ReturnsAsync(role);
        _mockMapper.Setup(x => x.MapToDtoWithPermissions(role)).Returns(new RoleWithPermissionsDTO(role.Uuid, role.Name, [new(permission.Uuid, permission.Name)]));

        // Act
        RoleWithPermissionsDTO? result = await _roleService.GetRoleByIdWithPermissionsOrNull(roleUuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(role.Uuid, result.Uuid);
        Assert.Equal(role.Name, result.Name);
        Assert.Single(result.Permissions);
        Assert.Equal(permission.Uuid, result.Permissions.First().Uuid);
        Assert.Equal(permission.Name, result.Permissions.First().Name);
    }

    [Fact]
    public async Task GetRoleByIdWithPermissionsOrNull_WhenRoleDoesNotExist_ReturnsNull()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();

        _mockRepository.Setup(x => x.GetByIdWithPermissionsOrNull(roleUuid)).ReturnsAsync((Role?)null);

        // Act
        RoleWithPermissionsDTO? result = await _roleService.GetRoleByIdWithPermissionsOrNull(roleUuid);

        // Assert
        Assert.Null(result);
    }
}
