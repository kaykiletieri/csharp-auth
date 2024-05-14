using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CSharpAuth.Application.UnitTests.RoleServiceTests;

public class CreateRoleTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IRoleMapper> _mockMapper;
    private readonly Mock<IPermissionRepository> _mockPermissionRepository;
    private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
    private readonly RoleService _roleService;

    public CreateRoleTests()
    {
        _mockRepository = new();
        _mockMapper = new();
        _mockPermissionRepository = new();
        _mockRolePermissionRepository = new();

        _roleService = new(_mockRepository.Object, _mockMapper.Object, _mockRolePermissionRepository.Object, _mockPermissionRepository.Object);
    }

    [Fact]
    public async Task CreateRole_WithValidName_ShouldReturnCreatedRole()
    {
        // Arrange
        CreateUpdateRoleDTO createRoleDto = new("CreateRoleAsync", "Criar função");
        Role role = new() { Uuid = Guid.NewGuid(), Name = createRoleDto.Name, Description = createRoleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };
        RoleDTO roleDTO = new(role.Uuid, role.Name, role.Description);

        _mockMapper.Setup(x => x.MapToEntity(createRoleDto)).Returns(role);
        _mockRepository.Setup(x => x.GetByNameOrNull(role.Name)).ReturnsAsync((Role?)null);
        _mockRepository.Setup(x => x.Create(role)).ReturnsAsync(role);
        _mockMapper.Setup(x => x.MapToDto(role)).Returns(roleDTO);


        // Act
        RoleDTO result = await _roleService.CreateRole(createRoleDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(role.Name, result.Name);
        Assert.Equal(role.Description, result.Description);
    }

    [Fact]
    public async Task CreateRole_WithInvalidName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        CreateUpdateRoleDTO createRoleDto = new("CreateRoleAsync", "Criar função");
        Role role = new() { Uuid = Guid.NewGuid(), Name = createRoleDto.Name, Description = createRoleDto.Description, CreatedAt = new DateTime(2024, 04, 12, 10, 30, 0), UpdatedAt = new DateTime(2024, 04, 12, 10, 30, 0) };

        _mockMapper.Setup(x => x.MapToEntity(createRoleDto)).Returns(role);
        _mockRepository.Setup(x => x.GetByNameOrNull(role.Name)).ReturnsAsync(role);

        // Act
        async Task Act() => await _roleService.CreateRole(createRoleDto);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
    }
}