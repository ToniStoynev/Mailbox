using Mailbox.Contracts;

namespace Mailbox.GrainInterfaces.Interfaces;

public interface IEmailGrain : IGrainWithStringKey
{
    Task SendEmail(Email email);
    
    Task<List<Email>> GetReceivedEmails();
}