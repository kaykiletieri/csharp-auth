using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class GetAllRolesWithPermissionsTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly RoleService _roleService;

    public GetAllRolesWithPermissionsTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockRolePermissionRepository = new();
        _mockPermissionRepository = new();

        _roleService = new RoleService(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task GetAllRolesWithPermissions_WhenRolesExist_ReturnsRolesWithPermissions()
    {
        // Arrange
        Role role = new() { Uuid = Guid.NewGuid(), Name = "role", CreatedAt = new DateTime(), UpdatedAt = new DateTime() };
        Permission permission = new() { Uuid = Guid.NewGuid(), Name = "permission", CreatedAt = new DateTime(), UpdatedAt = new DateTime() };
        RolePermission rolePermission = new() { Uuid = Guid.NewGuid(), RoleUuid = role.Uuid, PermissionUuid = permission.Uuid, CreatedAt = new DateTime(), UpdatedAt = new DateTime() };

        _mockRepository.Setup(x => x.GetAllWithPermissions()).ReturnsAsync(new List<Role> { role });
        _mockMapper.Setup(x => x.MapToDtoWithPermissions(It.IsAny<IEnumerable<Role>>())).Returns(
            [
                new RoleWithPermissionsDTO(role.Uuid, role.Name, [new(permission.Uuid, permission.Name)])
            ]
         );

        // Act
        IEnumerable<RoleWithPermissionsDTO> result = await _roleService.GetAllRolesWithPermissions();

        // Assert
        Assert.Single(result);
        Assert.Equal(role.Uuid, result.First().Uuid);
        Assert.Equal(role.Name, result.First().Name);
        Assert.Single(result.First().Permissions);
        Assert.Equal(permission.Uuid, result.First().Permissions.First().Uuid);
        Assert.Equal(permission.Name, result.First().Permissions.First().Name);
    }

    [Fact]
    public async Task GetAllRolesWithPermissions_WhenNoRolesExist_ReturnsEmptyList()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetAllWithPermissions()).ReturnsAsync(new List<Role>());

        // Act
        IEnumerable<RoleWithPermissionsDTO> result = await _roleService.GetAllRolesWithPermissions();

        // Assert
        Assert.Empty(result);
    }
}
