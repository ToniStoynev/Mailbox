using Mailbox.GrainInterfaces.Interfaces;
using Mailbox.Contracts;
using Mailbox.GrainInterfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Mailbox.GrainInterfaces.Grains;

public class AppointmentGrain(
    [PersistentState(stateName: "appointment", storageName: "mailbox")]
    IPersistentState<List<Appointment>> appointments,
    IHubContext<EmailHub> emailHubContext)
    : Grain, IAppointmentGrain, IRemindable
{
    public async Task ScheduleAppointment(Appointment appointment)
    {
        appointments.State.Add(appointment);
        await appointments.WriteStateAsync();

        await emailHubContext.Clients.All.SendAsync("NewAppointmentScheduled", appointment);
    }

    public Task<List<Appointment>> GetScheduledAppointments(string userEmailAddress)
        => Task.FromResult(appointments.State
            .Where(app => app.Organizer == userEmailAddress || app.Participants.Contains(userEmailAddress)).ToList());
    
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await this.RegisterOrUpdateReminder("CheckUpcomingAppointments", TimeSpan.Zero, TimeSpan.FromSeconds(60));
    }

    public async Task ReceiveReminder(string reminderName, TickStatus status)
    {
        if (reminderName == "CheckUpcomingAppointments")
        {
            var now = DateTime.Now;

            // Find events that are 1 minute away
            var upcomingAppointments = appointments.State?
                .Where(a => (a.StartTime - now).TotalMinutes <= 1 && (a.StartTime - now).TotalMinutes > 0)
                .ToList();

            if (upcomingAppointments != null)
            {
                foreach (var appointment in upcomingAppointments)
                {
                    await emailHubContext.Clients.All.SendAsync("SendNotification", $"{appointment.EventName}");
                }
            }
        }
    }
}