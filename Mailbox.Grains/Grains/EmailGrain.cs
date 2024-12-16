using Mailbox.GrainInterfaces.Interfaces;
using Mailbox.Contracts;
using Mailbox.GrainInterfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Mailbox.GrainInterfaces.Grains;

public class EmailGrain([PersistentState(stateName: "email", storageName: "mailbox")]IPersistentState<List<Email>> emails, IHubContext<EmailHub> emailHubContext) 
    : Grain, IEmailGrain
{
    
    public async Task SendEmail(Email email)
    {
        emails.State.Add(email);
        await emails.WriteStateAsync();
        await emailHubContext.Clients.All.SendAsync("NewEmailReceived", email);
    }

    public Task<List<Email>> GetReceivedEmails() => Task.FromResult(emails.State);
}