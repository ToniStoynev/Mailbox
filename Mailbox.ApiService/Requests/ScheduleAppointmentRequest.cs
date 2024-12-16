namespace Mailbox.ApiService.Requests;

public record ScheduleAppointmentRequest(string EventName, string Organizer, List<string> Participants, DateTime StartTime, DateTime EndTime);