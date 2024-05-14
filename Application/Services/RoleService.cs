using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services.Interfaces;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;

namespace CSharpAuth.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IRoleMapper _roleMapper;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IPermissionRepository _permissionRepository;

    public RoleService(IRoleRepository roleRepository, IRoleMapper roleMapper, IRolePermissionRepository rolePermissionRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _roleMapper = roleMapper;
        _rolePermissionRepository = rolePermissionRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<IEnumerable<RoleDTO>> GetAllRoles()
    {
        IEnumerable<Role> roles = await _roleRepository.GetAll();
        return _roleMapper.MapToDto(roles);
    }

    public async Task<IEnumerable<RoleWithPermissionsDTO>> GetAllRolesWithPermissions()
    {
        IEnumerable<Role> roles = await _roleRepository.GetAllWithPermissions();
        return _roleMapper.MapToDtoWithPermissions(roles);
    }

    public async Task<RoleDTO?> GetRoleByIdOrNull(Guid roleUuid)
    {
        Role? role = await _roleRepository.GetByIdOrNull(roleUuid);

        return role == null ? null : _roleMapper.MapToDto(role);
    }

    public async Task<RoleWithPermissionsDTO?> GetRoleByIdWithPermissionsOrNull(Guid roleUuid)
    {
        Role? role = await _roleRepository.GetByIdWithPermissionsOrNull(roleUuid);

        return role == null ? null : _roleMapper.MapToDtoWithPermissions(role);
    }

    public async Task AssingPermissionToRole(Guid roleUuid, Guid permissionUuid)
    {
        Role role = await _roleRepository.GetByIdOrNull(roleUuid) ?? throw new NotFoundException($"Role not found: {roleUuid}");
        Permission permission = await _permissionRepository.GetByIdOrNull(permissionUuid) ?? throw new NotFoundException($"Permission not found: {permissionUuid}");

        RolePermission? existingRolePermission = await _rolePermissionRepository.GetByRoleAndPermissionOrNull(roleUuid, permissionUuid);
        if (existingRolePermission != null && existingRolePermission.DeletedAt == null)
        {
            throw new InvalidOperationException($"Role permission already exists: {roleUuid} - {permissionUuid}");
        }

        RolePermission rolePermission = new()
        {
            Uuid = Guid.NewGuid(),
            RoleUuid = role.Uuid,
            PermissionUuid = permission.Uuid,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _rolePermissionRepository.Create(rolePermission);
    }

    public async Task RemovePermissionFromRole(Guid roleUuid, Guid permissionUuid)
    {
        Role role = await _roleRepository.GetByIdOrNull(roleUuid) ?? throw new NotFoundException($"Role not found: {roleUuid}");
        Permission permission = await _permissionRepository.GetByIdOrNull(permissionUuid) ?? throw new NotFoundException($"Permission not found: {permissionUuid}");

        RolePermission? existingRolePermission = await _rolePermissionRepository.GetByRoleAndPermissionOrNull(roleUuid, permissionUuid);
        if (existingRolePermission == null || existingRolePermission.DeletedAt != null)
        {
            throw new InvalidOperationException($"Role permission not found: {role.Uuid} - {permission.Uuid}");
        }
        await _rolePermissionRepository.SoftDelete(existingRolePermission.Uuid);
    }

    public async Task<RoleDTO> CreateRole(CreateUpdateRoleDTO roleDto)
    {
        Role role = _roleMapper.MapToEntity(roleDto);

        Role? existingRole = await _roleRepository.GetByNameOrNull(role.Name);
        if (existingRole != null)
        {
            throw new InvalidOperationException($"Role already exists: {roleDto.Name}");
        }

        await _roleRepository.Create(role);

        return _roleMapper.MapToDto(role);
    }
    public async Task<RoleDTO> UpdateRole(Guid roleUuid, CreateUpdateRoleDTO roleDTO)
    {
        Role existingRole = await _roleRepository.GetByIdOrNull(roleUuid) ?? throw new NotFoundException($"Role not found: {roleDTO.Name}");

        Role? existingRoleByName = await _roleRepository.GetByNameOrNull(roleDTO.Name);

        if (existingRoleByName != null && existingRoleByName.Uuid != roleUuid)
        {
            throw new InvalidOperationException($"Role already exists: {roleDTO.Name}");
        }

        existingRole.Name = roleDTO.Name;
        existingRole.Description = roleDTO.Description;

        await _roleRepository.Update(existingRole);

        return _roleMapper.MapToDto(existingRole);
    }

    public async Task SoftDeleteRole(Guid roleUuid)
    {
        if (await _roleRepository.GetByIdOrNull(roleUuid) == null)
        {
            throw new NotFoundException($"Role not found: {roleUuid}");
        }

        await _roleRepository.SoftDelete(roleUuid);
    }
}