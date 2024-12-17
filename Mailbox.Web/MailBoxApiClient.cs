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

            await httpClient.PostAsync("/sendEmail", jsonContent);
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }
}

