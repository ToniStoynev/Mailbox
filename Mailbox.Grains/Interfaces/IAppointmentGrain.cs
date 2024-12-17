using Mailbox.Contracts;

namespace Mailbox.GrainInterfaces.Interfaces;

public interface IAppointmentGrain : IGrainWithStringKey
{
    Task ScheduleAppointment(Appointment appointment);
    
    Task<List<Appointment>> GetScheduledAppointments(string userEmailAddress);
}