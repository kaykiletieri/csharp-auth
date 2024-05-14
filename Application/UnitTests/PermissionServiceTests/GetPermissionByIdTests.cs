using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.PermissionServiceTests;

public class GetPermissionByIdTests
{
    private readonly Mock<IPermissionRepository> _mockRepository;
    private readonly Mock<IPermissionMapper> _mockMapper;
    private readonly PermissionService _permissionService;

    public GetPermissionByIdTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _permissionService = new(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetPermissionByIdAsync_WhenValidId_ReturnsPermission()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();

        Permission permissionFromRepository = new() { Uuid = uuid, Name = "GetPermissionByIdAsync", Description = "Obter permissão por id", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        PermissionDTO permissionDtoFromMapper = new(permissionFromRepository.Uuid, permissionFromRepository.Name, permissionFromRepository.Description);

        _mockRepository.Setup(x => x.GetByIdOrNull(uuid)).ReturnsAsync(permissionFromRepository);
        _mockMapper.Setup(x => x.MapToDto(permissionFromRepository)).Returns(permissionDtoFromMapper);

        // Act
        var result = await _permissionService.GetPermissionByIdOrNull(uuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(permissionDtoFromMapper, result);
    }

    [Fact]
    public async Task GetPermissionByIdAsync_WhenInvalidId_ReturnsNull()
    {
        // Arrange
        Guid uuid = Guid.NewGuid();

        IEnumerable<Permission> permissionsFromRepository =
        [
            new Permission { Uuid = Guid.NewGuid(), Name = "GetAllPermissions", Description = "Obter todas as permissões", CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) },
        ];

        _mockRepository.Setup(x => x.GetAll()).ReturnsAsync(permissionsFromRepository);

        // Act
        var result = await _permissionService.GetPermissionByIdOrNull(uuid);

        // Assert
        Assert.Null(result);
    }
}