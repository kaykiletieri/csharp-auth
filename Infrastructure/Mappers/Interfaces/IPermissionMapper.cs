using CSharpAuth.Application.DTOs;
using CSharpAuth.Domain.Entities;

namespace CSharpAuth.Infrastructure.Mappers.Interfaces;

public interface IPermissionMapper
{
    Permission MapToEntity(CreateUpdatePermissionDTO dto);
    Permission MapToEntity(PermissionDTO dto);
    PermissionDTO MapToDto(Permission entity);
    IEnumerable<PermissionDTO> MapToDto(IEnumerable<Permission> entities);
}
