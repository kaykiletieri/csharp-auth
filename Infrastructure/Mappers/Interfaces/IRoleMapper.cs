using CSharpAuth.Application.DTOs;
using CSharpAuth.Domain.Entities;

namespace CSharpAuth.Infrastructure.Mappers.Interfaces;

public interface IRoleMapper
{
    Role MapToEntity(CreateUpdateRoleDTO dto);
    Role MapToEntity(RoleDTO dto);
    RoleDTO MapToDto(Role entity);
    RoleWithPermissionsDTO MapToDtoWithPermissions(Role entity);
    IEnumerable<RoleDTO> MapToDto(IEnumerable<Role> entities);
    IEnumerable<RoleWithPermissionsDTO> MapToDtoWithPermissions(IEnumerable<Role> entities);

}