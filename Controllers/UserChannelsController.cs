
using Microsoft.AspNetCore.Mvc;
using Ynov_WorkShare_Server.Interfaces;
using Ynov_WorkShare_Server.Models;


namespace Ynov_WorkShare_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserChannelsController: ControllerBase
{
    private readonly IUserChannelService _iuc;

    public UserChannelsController(IUserChannelService userChannel)
    {
        _iuc = userChannel;
    }
        
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserChannel>> Get(Guid id)
    {
        return Ok(await _iuc.GetById(id));
    }
    
    [HttpPut("Update/{id:guid}/User/{userId:guid}")]
    public async Task<IActionResult> Put(Guid id, Guid userId, [FromQuery] bool isMuted)
    {
        var userChannel = await _iuc.SetIsMuted(id, userId, isMuted);
        return Ok(userChannel);
    }


    [HttpPost]
    public async Task<ActionResult<Channel>> Post(UserChannel userChannel)
    {
        var uc = await _iuc.Post(userChannel);
        return CreatedAtAction("Get", new { id = uc.Id }, uc);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _iuc.Remove(id);
        return NoContent();
    }

}