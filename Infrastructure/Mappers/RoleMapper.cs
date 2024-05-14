using CSharpAuth.Application.DTOs;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;

namespace CSharpAuth.Infrastructure.Mappers;

public class RoleMapper(IPermissionMapper permissionMapper) : IRoleMapper
{
    private readonly IPermissionMapper _permissionMapper = permissionMapper;

    public Role MapToEntity(CreateUpdateRoleDTO dto)
    {
        return new Role
        {
            Uuid = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public Role MapToEntity(RoleDTO dto)
    {
        return new Role
        {
            Uuid = dto.Uuid,
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public RoleDTO MapToDto(Role entity)
    {
        return new RoleDTO(entity.Uuid, entity.Name, entity.Description);
    }

    public RoleWithPermissionsDTO MapToDtoWithPermissions(Role entity)
    {
        IEnumerable<Permission> permissionDTOs = entity.RolePermissions
            .Where(rp => rp.DeletedAt == null && rp.Permission != null && rp.Permission.DeletedAt == null)
            .Select(rp => rp.Permission)
            .Where(p => p != null)
            .Select(p => p!);

        return new RoleWithPermissionsDTO(entity.Uuid, entity.Name,
            _permissionMapper.MapToDto(permissionDTOs), entity.Description);
    }

    public IEnumerable<RoleDTO> MapToDto(IEnumerable<Role> entities)
    {
        return entities.Select(MapToDto);
    }

    public IEnumerable<RoleWithPermissionsDTO> MapToDtoWithPermissions(IEnumerable<Role> entities)
    {
        return entities.Select(MapToDtoWithPermissions);
    }
}
