namespace CSharpAuth.Application.DTOs;

public record CreateUserDTO(string Username, string Password);
public record UserDTOWithRole(Guid Uuid, string Username, IEnumerable<RoleDTO> Roles);
public record UserDTO(Guid Uuid, string Username);