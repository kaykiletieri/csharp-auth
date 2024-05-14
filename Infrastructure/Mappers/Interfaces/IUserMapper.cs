using CSharpAuth.Application.DTOs;
using CSharpAuth.Domain.Entities;

namespace CSharpAuth.Infrastructure.Mappers.Interfaces;

public interface IUserMapper
{
    User MapToEntity(CreateUserDTO dto, string hashedPassword, byte[] salt);
    UserDTO MapToDTO(User user);
    IEnumerable<UserDTO> MapToDTO(IEnumerable<User> users);
}
