namespace Mailbox.GrainInterfaces.Interfaces;

public interface IAppointmentGrain : IGrainWithStringKey
{
    Task ScheduleAppointment(string eventName, string organizer, List<string> participants, DateTime startTime, DateTime endTime);
}