using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class RolesServiceTest
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly RoleService _roleService;

    public RolesServiceTest()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockPermissionRepository = new();
        _mockRolePermissionRepository = new();

        _roleService = new(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task SoftDeleteRole_WithValidId_ShouldSoftDeleteRole()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdateRoleDTO roleDto = new("DeleteRoleAsync", "Deletar função");
        Role role = new() { Uuid = uuid, Name = roleDto.Name, Description = roleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(role);
        _mockRepository.Setup(x => x.SoftDelete(uuid)).Verifiable();

        // Act
        await _roleService.SoftDeleteRole(uuid);

        // Assert
        _mockRepository.Verify(x => x.SoftDelete(uuid), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteRole_WithInvalidId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();
        CreateUpdateRoleDTO roleDto = new("DeleteRoleAsync", "Deletar função");
        Role role = new() { Uuid = uuid, Name = roleDto.Name, Description = roleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync((Role?)null);
        _mockRepository.Setup(x => x.SoftDelete(uuid)).Verifiable();

        // Act
        async Task Act() => await _roleService.SoftDeleteRole(uuid);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }
}