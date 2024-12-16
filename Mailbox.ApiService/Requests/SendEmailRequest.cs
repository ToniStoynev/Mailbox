namespace Mailbox.ApiService.Requests;

public record SendEmailRequest(string To, string Subject, string Body);