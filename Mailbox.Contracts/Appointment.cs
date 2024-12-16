namespace Mailbox.Contracts;

[GenerateSerializer, Alias(nameof(Appointment))]
public record Appointment(string EventName, string Organizer, List<string> Participants, DateTime StartTime, DateTime EndTime);