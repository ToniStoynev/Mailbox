using Mailbox.ApiService.Constants;
using Mailbox.ApiService.Requests;
using Mailbox.GrainInterfaces.Hubs;
using Mailbox.GrainInterfaces.Interfaces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.UseSignalR();
    siloBuilder.AddMemoryGrainStorage("mailbox");
    siloBuilder.UseInMemoryReminderService();
});

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR().AddOrleans();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/sendEmail", async (IGrainFactory grainFactory, [FromBody]SendEmailRequest sendEmailRequest) =>
{
    var emailGrain = grainFactory.GetGrain<IEmailGrain>(sendEmailRequest.To);
    await emailGrain.SendEmail(new(
        Guid.NewGuid(), 
        sendEmailRequest.From, 
        sendEmailRequest.To, 
        sendEmailRequest.Subject, 
        sendEmailRequest.Body,
        DateTime.Now));
    return Results.Ok();
});

app.MapGet("/emails", async (IGrainFactory grainFactory, string userEmailAddress) =>
{
    var emailGrain = grainFactory.GetGrain<IEmailGrain>(userEmailAddress);
    var emails = await emailGrain.GetReceivedEmails();
    return Results.Ok(emails);
});

app.MapPost("/scheduleAppointment", async (IGrainFactory grainFactory, [FromBody]ScheduleAppointmentRequest scheduleAppointmentRequest) =>
{
    var appointmentGrain = grainFactory.GetGrain<IAppointmentGrain>(AppConstants.AppointmentGrainKey);
    await appointmentGrain.ScheduleAppointment(new(
        scheduleAppointmentRequest.EventName, 
        scheduleAppointmentRequest.Organizer, 
        scheduleAppointmentRequest.Participants, 
        scheduleAppointmentRequest.StartTime, 
        scheduleAppointmentRequest.EndTime));
    
    return Results.Ok();
});

app.MapGet("/appointments", async (IGrainFactory grainFactory, string userEmailAddress) =>
{
    var appointmentGrain = grainFactory.GetGrain<IAppointmentGrain>(AppConstants.AppointmentGrainKey);
    var scheduledAppointments = await appointmentGrain.GetScheduledAppointments(userEmailAddress);
    return Results.Ok(scheduledAppointments);
});

app.MapDefaultEndpoints();
app.MapHub<EmailHub>("/emailsHub");
app.Run();

