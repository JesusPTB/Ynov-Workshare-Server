using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ynov_WorkShare_Server.Interfaces;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Controllers;

[Route("api/[controller]")]
[ApiController]

[Authorize]
public class ChannelsController: ControllerBase
{
    private readonly IChannelService _ics;

    public ChannelsController(IChannelService channelService)
    {
        _ics = channelService;
    }
        
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Channel>> Get(Guid id)
    {
        return Ok(await _ics.Get(id));
    }
    
    [HttpGet("User/{userId:guid}")]
    public async Task<ActionResult<Channel>> GetByUserId(Guid userId)
    {
        return Ok(await _ics.GetChannelsByUser(userId));
    }
        
        
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, Channel  channel)
    {
        var c = await _ics.Update(id, channel);
        return Ok(c);
    }
        
    [HttpPut("Update/{id:guid}/Admin/{adminId}")]
    public async Task<IActionResult> Put(Guid id, string adminId)
    {
        var channel = await _ics.ChangeAdmin(id, adminId);
        return Ok(channel);
    }


    [HttpPost]
    public async Task<ActionResult<Channel>> Post(Channel channel)
    {
        var c = await _ics.Post(channel);
        return CreatedAtAction("Get", new { id = c.Id }, c);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ics.Delete(id);
        return NoContent();
    }
}