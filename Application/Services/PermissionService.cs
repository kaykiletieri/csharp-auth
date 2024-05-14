using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services.Interfaces;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;

namespace CSharpAuth.Application.Services;

public class PermissionService(IPermissionRepository permissionRepository, IPermissionMapper permissionMapper) : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;
    private readonly IPermissionMapper _permissionMapper = permissionMapper;

    public async Task<IEnumerable<PermissionDTO>> GetAllPermissions()
    {
        IEnumerable<Permission> permissions = await _permissionRepository.GetAll();
        return _permissionMapper.MapToDto(permissions);
    }

    public async Task<PermissionDTO?> GetPermissionByIdOrNull(Guid uuid)
    {
        Permission? permission = await _permissionRepository.GetByIdOrNull(uuid);

        return permission == null ? null : _permissionMapper.MapToDto(permission);
    }

    public async Task<PermissionDTO> CreatePermission(CreateUpdatePermissionDTO permissionDto)
    {
        Permission permission = _permissionMapper.MapToEntity(permissionDto);

        Permission? existingPermission = await _permissionRepository.GetByNameOrNull(permission.Name);
        if (existingPermission != null)
        {
            throw new InvalidOperationException($"Permissions already exists: {permission.Name}");
        }

        await _permissionRepository.Create(permission);

        return _permissionMapper.MapToDto(permission);
    }

    public async Task<PermissionDTO> UpdatePermission(Guid uuid, CreateUpdatePermissionDTO permissionDTO)
    {
        Permission existingPermission = await _permissionRepository.GetByIdOrNull(uuid) ?? throw new NotFoundException($"Permission not found: {permissionDTO.Name}");

        Permission? existingPermissionByName = await _permissionRepository.GetByNameOrNull(permissionDTO.Name);
        if (existingPermissionByName != null && existingPermissionByName.Uuid != uuid)
        {
            throw new InvalidOperationException($"Permission already exists: {permissionDTO.Name}");
        }

        existingPermission.Name = permissionDTO.Name;
        existingPermission.Description = permissionDTO.Description;

        await _permissionRepository.Update(existingPermission);

        return _permissionMapper.MapToDto(existingPermission);
    }

    public async Task SoftDeletePermission(Guid uuid)
    {
        if (await _permissionRepository.GetByIdOrNull(uuid) == null)
        {
            throw new NotFoundException($"Permission not found: {uuid}");
        }

        await _permissionRepository.SoftDelete(uuid);
    }
}
