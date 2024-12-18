Instructions for launching the application and demonstrating the functionalities:

Locally clone the repository and open it with an IDE (Visual Studio, JetBrains Rider)

The application uses .NET Aspire. We launch the .AppHost project (which has references and will launch two other projects - `apiService` and `webfrontend`)

After launching the .AppHost project, the home page will load in the browser (which will help us enter the username as a route parameter in the url).

We need to pass our email address in the url as a route parameter to simulate logging into our email (e.g. `http://localhost:5171/toni.stoinev@gmail.com`).

You can open another tab with another email address to be able to follow sending/receiving emails to different addresses (e.g. `http://localhost:5171/john.doe@gmail.com`)

You press the `New Email` button, which will open a modal, in which you should get all the necessary information (To, Subject and Main content), and in `To` we can find the email address for which we already have an open tab (http://localhost :5171/john.doe@gmail.com)

The sent message will appear immediately in the mailbox table of the email address to which it was sent.

The `Schedule Appointment` functionality also works in this way, and it also has a Reminder functionality - one minute before the start of the event, a new column with the text "Reminder: Your event is start soon!" appears in the Calendar table on the corresponding row of a given event
