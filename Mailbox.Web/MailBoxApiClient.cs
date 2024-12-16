using Mailbox.Contracts;

namespace Mailbox.Web;

public class MailBoxApiClient(HttpClient httpClient)
{
    public async Task<List<Email>> GetEmailsAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<Email>? emails = null;

        await foreach (var email in httpClient.GetFromJsonAsAsyncEnumerable<Email>("/emails?userEmailAddress=toni.stoinev@gmail.com", cancellationToken))
        {
            if (emails?.Count >= maxItems)
            {
                break;
            }
            if (email is not null)
            {
                emails ??= [];
                emails.Add(email);
            }
        }

        return emails ?? [];
    }
}

