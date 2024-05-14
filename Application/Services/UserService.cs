using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services.Interfaces;
using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Mappers.Interfaces;
using CSharpAuth.Infrastructure.Repositories.Interfaces;

namespace CSharpAuth.Application.Services;

public class UserService(IUserRepository userRepository, IUserMapper userMapper, IPasswordHasherService passwordHasherService) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserMapper _userMapper = userMapper;
    private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        IEnumerable<User> users = await _userRepository.GetAll();
        return _userMapper.MapToDTO(users);
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid uuid)
    {
        User? user = await _userRepository.GetByIdOrNull(uuid);

        return user == null ? null : _userMapper.MapToDTO(user);
    }

    public async Task<UserDTO> CreateUserAsync(CreateUserDTO userDTO)
    {
        User? userExists = await _userRepository.GetByUsernameOrNull(userDTO.Username);
        if (userExists != null)
        {
            throw new InvalidOperationException("User already exists");
        }

        (string hashedPassword, byte[] salt) = _passwordHasherService.HashPassword(userDTO.Password);

        User user = _userMapper.MapToEntity(userDTO, hashedPassword, salt);

        await _userRepository.CreateAsync(user);

        return _userMapper.MapToDTO(user);
    }
}
