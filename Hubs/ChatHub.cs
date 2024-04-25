using Microsoft.AspNetCore.SignalR;

namespace Ynov_WorkShare_Server.Hubs;

public class ChatHub : Hub
{
    /// <summary>
    ///     Send a message to all clients connected to the hub
    /// </summary>
    /// <param name="user">The user who sent the message</param>
    /// <param name="message">The message to  send</param>
    public async Task SendMessageToAll(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    /// <summary>
    ///     Add the connection to a group
    /// </summary>
    /// <param name="groupName">Group name</param>
    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    /// <summary>
    ///     Remove the connection from a group
    /// </summary>
    /// <param name="groupName">Group name</param>
    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
    }

    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }
}