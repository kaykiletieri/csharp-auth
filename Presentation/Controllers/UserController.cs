using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAuth.Presentation.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO userDTO)
    {
        try
        {
            UserDTO user = await _userService.CreateUserAsync(userDTO);

            return StatusCode(201, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
