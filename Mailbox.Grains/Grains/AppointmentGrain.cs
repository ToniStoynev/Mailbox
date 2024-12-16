using Mailbox.GrainInterfaces.Interfaces;
using Mailbox.Contracts;

namespace Mailbox.GrainInterfaces.Grains;

public class AppointmentGrain([PersistentState(stateName: "appointment", storageName: "mailbox")]IPersistentState<List<Appointment>> appointments) : Grain, IAppointmentGrain
{
    public async Task ScheduleAppointment(string eventName, string organizer, List<string> participants, DateTime startTime, DateTime endTime)
    {
        appointments.State.Add(new(eventName, organizer, participants, startTime, endTime));
        await appointments.WriteStateAsync();
    }
    
}