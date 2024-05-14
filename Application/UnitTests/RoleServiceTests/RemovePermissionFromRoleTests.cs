using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class RemovePermissionFromRoleTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly RoleService _roleService;

    public RemovePermissionFromRoleTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockRolePermissionRepository = new();
        _mockPermissionRepository = new();

        _roleService = new RoleService(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task RemovePermissionFromRole_WhenRoleDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();
        Guid permissionUuid = Guid.NewGuid();

        _mockRepository.Setup(x => x.GetByIdOrNull(roleUuid)).ReturnsAsync((Role?)null);

        // Act
        async Task act() => await _roleService.RemovePermissionFromRole(roleUuid, permissionUuid);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task RemovePermissionFromRole_WhenPermissionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();
        Guid permissionUuid = Guid.NewGuid();
        Role role = new() { Uuid = roleUuid, Name = "role", CreatedAt = new DateTime(), UpdatedAt = new DateTime() };

        _mockRepository.Setup(x => x.GetByIdOrNull(roleUuid)).ReturnsAsync(role);
        _mockPermissionRepository.Setup(x => x.GetByIdOrNull(permissionUuid)).ReturnsAsync((Permission?)null);

        // Act
        async Task act() => await _roleService.RemovePermissionFromRole(roleUuid, permissionUuid);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task RemovePermissionFromRole_WhenRolePermissionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        Guid roleUuid = Guid.NewGuid();
        Guid permissionUuid = Guid.NewGuid();
        Role role = new() { Uuid = roleUuid, Name = "role", CreatedAt = new DateTime(), UpdatedAt = new DateTime() };
        Permission permission = new() { Uuid = roleUuid, Name = "permission", CreatedAt = new DateTime(), UpdatedAt = new DateTime() };

        _mockRepository.Setup(x => x.GetByIdOrNull(roleUuid)).ReturnsAsync(role);
        _mockPermissionRepository.Setup(x => x.GetByIdOrNull(permissionUuid)).ReturnsAsync(permission);
        _mockRolePermissionRepository.Setup(x => x.GetByRoleAndPermissionOrNull(roleUuid, permissionUuid)).ReturnsAsync((RolePermission?)null);

        // Act
        async Task act() => await _roleService.RemovePermissionFromRole(roleUuid, permissionUuid);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }
}
