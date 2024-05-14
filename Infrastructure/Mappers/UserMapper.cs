using CSharpAuth.Application.DTOs;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;

namespace CSharpAuth.Infrastructure.Mappers;

public class UserMapper : IUserMapper
{
    public User MapToEntity(CreateUserDTO dto, string hashedPassword, byte[] salt)
    {
        return new User
        {
            Uuid = Guid.NewGuid(),
            Username = dto.Username,
            HashedPassword = hashedPassword,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public UserDTO MapToDTO(User user)
    {
        return new UserDTO(user.Uuid, user.Username);
    }

    public IEnumerable<UserDTO> MapToDTO(IEnumerable<User> users)
    {
        return users.Select(MapToDTO);
    }
}
