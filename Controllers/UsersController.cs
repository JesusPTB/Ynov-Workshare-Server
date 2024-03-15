using Microsoft.AspNetCore.Mvc;
using Ynov_WorkShare_Server.DTO;
using Ynov_WorkShare_Server.Interfaces;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _ius;

    public UsersController(IUserService userService)
    {
        _ius = userService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        return Ok(await _ius.GetById(id));
    }
    
    [HttpGet("Channel/{id}")]
    public async Task<ActionResult<UserDto>> GetUserByChannelId(Guid channelId)
    {
        return Ok(await _ius.GetByChannel(channelId));
    }
    
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        return Ok(await _ius.GetAll());
    }
    
    [HttpGet("Email")]
    public async Task<ActionResult<UserDto>> GetUserById([FromQuery] string email)
    {
        return Ok(await _ius.GetByEmail(email));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, User user)
    {
        await _ius.Update(id, user);
        return NoContent();
    }


    [HttpPost]
    public async Task<ActionResult<UserDto>> Post(User user)
    {
        var u = await _ius.Post(user);
        return CreatedAtAction("GetUserById", new { id = u.Id }, u);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ius.Delete(id);
        return NoContent();
    }
}