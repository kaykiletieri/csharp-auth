using CSharpAuth.Application.DTOs;

namespace CSharpAuth.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO> CreateUserAsync(CreateUserDTO userDTO);
}
