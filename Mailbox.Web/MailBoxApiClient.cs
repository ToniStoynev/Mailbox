using System.Text;
using System.Text.Json;
using Mailbox.Contracts;

namespace Mailbox.Web;

public class MailBoxApiClient(HttpClient httpClient)
{
    public async Task<List<Email>> GetEmailsAsync(string userEmailAddress, CancellationToken cancellationToken = default)
    {
        List<Email>? emails = null;

        await foreach (var email in httpClient.GetFromJsonAsAsyncEnumerable<Email>($"/emails?userEmailAddress={userEmailAddress}", cancellationToken))
        {
            if (email is not null)
            {
                emails ??= [];
                emails.Add(email);
            }
        }

        return emails ?? [];
    }
    
    public async Task SendEmailsAsync(Email email, CancellationToken cancellationToken = default)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(email),
                Encoding.UTF8,
                "application/json");

            await httpClient.PostAsync("/sendEmail", jsonContent, cancellationToken);
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }
    
    public async Task<List<Appointment>> GetAppointmentsAsync(string userEmailAddress, CancellationToken cancellationToken = default)
    {
        List<Appointment>? appointments = null;

        await foreach (var appointment in httpClient.GetFromJsonAsAsyncEnumerable<Appointment>($"/appointments?userEmailAddress={userEmailAddress}", cancellationToken))
        {
            if (appointment is not null)
            {
                appointments ??= [];
                appointments.Add(appointment);
            }
        }

        return appointments ?? [];
    }
    
    public async Task ScheduleAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(appointment),
                Encoding.UTF8,
                "application/json");

            await httpClient.PostAsync("/scheduleAppointment", jsonContent, cancellationToken);
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }
}

