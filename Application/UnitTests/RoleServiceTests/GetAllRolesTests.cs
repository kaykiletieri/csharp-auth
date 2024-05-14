using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class GetAllRolesTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly RoleService _roleService;

    public GetAllRolesTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockRolePermissionRepository = new();
        _mockPermissionRepository = new();

        _roleService = new RoleService(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task GetAllRoles_ShoulReturnAllRoles()
    {
        // Arrange
        IEnumerable<Role> rolesFromRepository =
        [
            new Role { Uuid = Guid.NewGuid(), Name = "GetAllRoles", Description = "Obter todas as funções", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) },
            new Role { Uuid = Guid.NewGuid(), Name = "GetRoleByIdAsync", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) },
            new Role { Uuid = Guid.NewGuid(), Name = "CreateRoleAsync", Description = "Criar função", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) },
            new Role { Uuid = Guid.NewGuid(), Name = "UpdateRoleAsync", Description = "Atualizar função", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) }
        ];
        IEnumerable<RoleDTO> rolesDtoFromMapper =
        [
            new RoleDTO(rolesFromRepository.ElementAt(0).Uuid, rolesFromRepository.ElementAt(0).Name, rolesFromRepository.ElementAt(0).Description),
            new RoleDTO(rolesFromRepository.ElementAt(1).Uuid, rolesFromRepository.ElementAt(1).Name, rolesFromRepository.ElementAt(1).Description),
            new RoleDTO(rolesFromRepository.ElementAt(2).Uuid, rolesFromRepository.ElementAt(2).Name, rolesFromRepository.ElementAt(2).Description),
            new RoleDTO(rolesFromRepository.ElementAt(3).Uuid, rolesFromRepository.ElementAt(3).Name, rolesFromRepository.ElementAt(3).Description)
        ];

        _mockRepository.Setup(x => x.GetAll()).ReturnsAsync(rolesFromRepository);
        _mockMapper.Setup(x => x.MapToDto(rolesFromRepository)).Returns(rolesDtoFromMapper);

        // Act
        var result = await _roleService.GetAllRoles();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rolesDtoFromMapper, result);
    }

    [Fact]
    public async Task GetAllRoles_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetAll()).ReturnsAsync(new List<Role>());

        // Act
        var result = await _roleService.GetAllRoles();

        // Assert
        Assert.Empty(result);
    }
}