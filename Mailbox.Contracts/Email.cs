namespace Mailbox.Contracts;

[GenerateSerializer, Alias(nameof(Email))]
public sealed record Email(Guid Id, string To, string Subject, string Body);