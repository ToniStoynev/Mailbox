using Mailbox.GrainInterfaces.Interfaces;
using Mailbox.Contracts;
using Mailbox.GrainInterfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Mailbox.GrainInterfaces.Grains;

public class AppointmentGrain([PersistentState(stateName: "appointment", storageName: "mailbox")]IPersistentState<List<Appointment>> appointments, 
    IHubContext<EmailHub> emailHubContext) 
    : Grain, IAppointmentGrain
{
    public async Task ScheduleAppointment(Appointment appointment)
    {
        appointments.State.Add(appointment);
        await appointments.WriteStateAsync();
        
        await emailHubContext.Clients.All.SendAsync("NewAppointmentScheduled", appointment);
    }

    public Task<List<Appointment>> GetScheduledAppointments(string userEmailAddress) 
        => Task.FromResult(appointments.State.Where(app => app.Organizer == userEmailAddress || app.Participants.Contains(userEmailAddress)).ToList());
}