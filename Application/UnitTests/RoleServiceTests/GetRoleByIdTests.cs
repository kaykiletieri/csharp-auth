using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class GetRoleByIdTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly RoleService _roleService;

    public GetRoleByIdTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockPermissionRepository = new();
        _mockRolePermissionRepository = new();

        _roleService = new(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task GetRoleByIdAsync_WhenValidId_ReturnsRole()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();

        Role roleFromRepository = new() { Uuid = uuid, Name = "GetRoleByIdAsync", Description = "Obter função por Id", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        RoleDTO roleDtoFromMapper = new(roleFromRepository.Uuid, roleFromRepository.Name, roleFromRepository.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(roleFromRepository);
        _mockMapper.Setup(x => x.MapToDto(roleFromRepository)).Returns(roleDtoFromMapper);

        // Act
        var result = await _roleService.GetRoleByIdOrNull(uuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roleDtoFromMapper, result);
    }

    [Fact]
    public async Task GetRoleByIdAsync_WhenInvalidId_ReturnsNull()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();

        IEnumerable<Role> roleFromRepository =
        [
            new Role { Uuid = Guid.NewGuid(), Name = "GetAllRoles", Description = "Obter todas as funções", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) },
        ];

        _mockRepository.Setup(x => x.GetAll()).ReturnsAsync(roleFromRepository);

        // Act
        var result = await _roleService.GetRoleByIdOrNull(uuid);

        // Assert
        Assert.Null(result);
    }
}