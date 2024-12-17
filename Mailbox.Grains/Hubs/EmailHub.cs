using Microsoft.AspNetCore.SignalR;

namespace Mailbox.GrainInterfaces.Hubs;

public class EmailHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
}