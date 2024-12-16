namespace Mailbox.ApiService.Requests;

public record SendEmailRequest(string From, string To, string Subject, string Body);