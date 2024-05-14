using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAuth.Presentation.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class PermissionController(IPermissionService permissionService) : ControllerBase
{
    private readonly IPermissionService _permissionService = permissionService;

    [HttpGet]
    public async Task<IActionResult> GetAllPermissionsAsync()
    {
        try
        {
            IEnumerable<PermissionDTO?> permissions = await _permissionService.GetAllPermissions();

            return Ok(permissions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{uuid}")]
    public async Task<IActionResult> GetPermissionByIdAsync(Guid uuid)
    {
        try
        {
            PermissionDTO? permission = await _permissionService.GetPermissionByIdOrNull(uuid);

            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePermissionAsync(CreateUpdatePermissionDTO permissionDto)
    {
        try
        {
            PermissionDTO permission = await _permissionService.CreatePermission(permissionDto);
            return StatusCode(201, permission);
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

    [HttpPut("{uuid}")]
    public async Task<IActionResult> UpdatePermissionAsync(Guid uuid, CreateUpdatePermissionDTO permissionDto)
    {
        try
        {
            PermissionDTO permission = await _permissionService.UpdatePermission(uuid, permissionDto);

            return Ok(permission);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{uuid}")]
    public async Task<IActionResult> DeletePermissionAsync(Guid uuid)
    {
        try
        {
            await _permissionService.SoftDeletePermission(uuid);

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
