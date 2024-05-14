using CSharpAuth.Application.DTOs;
using CSharpAuth.Application.Exceptions;
using CSharpAuth.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAuth.Presentation.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    public async Task<IActionResult> GetAllRoleAsync()
    {
        try
        {
            IEnumerable<RoleDTO?> roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("with-permissions")]
    public async Task<IActionResult> GetAllRolesWithPermissionsAsync()
    {
        try
        {
            IEnumerable<RoleWithPermissionsDTO?> roles = await _roleService.GetAllRolesWithPermissions();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{uuid}")]
    public async Task<IActionResult> GetRoleByIdAsync(Guid uuid)
    {
        try
        {
            RoleDTO? role = await _roleService.GetRoleByIdOrNull(uuid);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("{uuid}/with-permissions")]
    public async Task<IActionResult> GetRoleWithPermissionsById(Guid uuid)
    {
        try
        {
            RoleWithPermissionsDTO? role = await _roleService.GetRoleByIdWithPermissionsOrNull(uuid);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync(CreateUpdateRoleDTO roleDTO)
    {
        try
        {
            RoleDTO role = await _roleService.CreateRole(roleDTO);

            return StatusCode(201, role);
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
    public async Task<IActionResult> UpdateRoleAsync(Guid uuid, CreateUpdateRoleDTO roleDTO)
    {
        try
        {
            RoleDTO role = await _roleService.UpdateRole(uuid, roleDTO);

            return Ok(role);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpPost]
    [Route("{roleUuid}/permissions/{permissionUuid}")]
    public async Task<IActionResult> AssignPermissionToRoleAsync(Guid roleUuid, Guid permissionUuid)
    {
        try
        {
            await _roleService.AssingPermissionToRole(roleUuid, permissionUuid);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpDelete]
    [Route("{roleUuid}/permissions/{permissionUuid}")]
    public async Task<IActionResult> RemovePermissionFromRoleAsync(Guid roleUuid, Guid permissionUuid)
    {
        try
        {
            await _roleService.RemovePermissionFromRole(roleUuid, permissionUuid);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpDelete("{uuid}")]
    public async Task<IActionResult> DeleteRoleAsync(Guid uuid)
    {
        try
        {
            await _roleService.SoftDeleteRole(uuid);
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
