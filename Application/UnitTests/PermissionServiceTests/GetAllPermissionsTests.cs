using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PermissionServiceTests;

public class GetAllPermissionsTests
{
    private readonly Mock<IPermissionRepository> _mockRepository;
    private readonly Mock<IPermissionMapper> _mockMapper;
    private readonly PermissionService _permissionService;

    public GetAllPermissionsTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _permissionService = new(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllPermissions_ShouldReturnAllPermissions()
    {
        // Arrange
        IEnumerable<Permission> permissionsFromRepository =
        [
            new Permission { Uuid = Guid.NewGuid(), Name = "GetAllPermissions", Description = "Obter todas as permissões", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt =  new DateTime(2024, 04, 12, 10, 30, 0)},
            new Permission { Uuid = Guid.NewGuid(), Name = "GetPermissionByIdAsync", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt =  new DateTime(2024, 04, 12, 10, 30, 0)},
            new Permission { Uuid = Guid.NewGuid(), Name = "CreatePermissionAsync", Description = "Criar permissão", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt =  new DateTime(2024, 04, 12, 10, 30, 0)},
            new Permission { Uuid = Guid.NewGuid(), Name = "UpdatePermissionAsync", Description = "Atualizar permissão", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt =  new DateTime(2024, 04, 12, 10, 30, 0)}
        ];

        IEnumerable<PermissionDTO> permissionsDtoFromMapper =
        [
            new PermissionDTO(permissionsFromRepository.ElementAt(0).Uuid, permissionsFromRepository.ElementAt(0).Name, permissionsFromRepository.ElementAt(0).Description),
            new PermissionDTO(permissionsFromRepository.ElementAt(1).Uuid, permissionsFromRepository.ElementAt(1).Name, permissionsFromRepository.ElementAt(1).Description),
            new PermissionDTO(permissionsFromRepository.ElementAt(2).Uuid, permissionsFromRepository.ElementAt(2).Name, permissionsFromRepository.ElementAt(2).Description),
            new PermissionDTO(permissionsFromRepository.ElementAt(3).Uuid, permissionsFromRepository.ElementAt(3).Name, permissionsFromRepository.ElementAt(3).Description)
        ];

        _mockRepository.Setup(x => x.GetAll()).ReturnsAsync(permissionsFromRepository);
        _mockMapper.Setup(x => x.MapToDto(permissionsFromRepository)).Returns(permissionsDtoFromMapper);

        // Act
        var result = await _permissionService.GetAllPermissions();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(permissionsDtoFromMapper, result);
    }

    [Fact]
    public async Task GetAllPermissions_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetAll()).ReturnsAsync(new List<Permission>());

        // Act
        var result = await _permissionService.GetAllPermissions();

        // Assert
        Assert.Empty(result);
    }
}