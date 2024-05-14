using CSharpAuth.Application.DTOs;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;

namespace CSharpAuth.Infrastructure.Mappers;

public class PermissionMapper : IPermissionMapper
{
    public Permission MapToEntity(CreateUpdatePermissionDTO dto)
    {
        return new Permission
        {
            Uuid = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public Permission MapToEntity(PermissionDTO dto)
    {
        return new Permission
        {
            Uuid = dto.Uuid,
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public PermissionDTO MapToDto(Permission entity)
    {
        return new PermissionDTO(entity.Uuid, entity.Name, entity.Description);
    }

    public IEnumerable<PermissionDTO> MapToDto(IEnumerable<Permission> entities)
    {
        return entities.Select(MapToDto);
    }

}
