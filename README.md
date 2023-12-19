# Repository for Udemy course: Maestría en .NET 8 y C#: Proyectos Esenciales
## Introduction

This solution is a collection of four projects built with .NET 8 and C#.
It's designed to demonstrate various functionalities including API consumption, email sending, environment configuration, and logging.
The solution is composed of **ApiConsumer**, **EmailSender**, **Environments**, and **Logging** projects.

## Projects Overview
### ApiConsumer
Example of an API that consumes other APIs GET, POST, PUT, and DELETE methods.
You can copy the file 'ApiConsumerService.cs' to any project to have a service to make Get, Post, Put, and Delete API calls.
Remember to create the body content of the requests using StringContent as follows:
```csharp
var body = new StringContent(JsonConvert.SerializeObject(data),System.Text.Encoding.UTF8, "application/json");
```
You can also add a Bearer token to the requests adding the information to the Token parameter on ApiConsumerDto.

### EmailSender
Showcases the code to send emails using DotNet and MailKit libraries. It has the basic configuration to send emails via Gmail and Outlook. Supports attachments and HTML body content.
You can copy the interface 'IEmailService.cs', the services: 'DotNetEmailService.cs' and 'MailKitEmailService.cs', and the models: 'EmailAttachmentDto.cs', 'EmailConfiguration.cs', and 'EmailDto.cs' to any project to have a service to send emails.
Remember to copy to your appsettings:
- For Gmail
```
  "EmailConfiguration": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "UserName": "your_email@gmail.com",
    "Password": "your_app_password",
    "EnableSsl": true
  }
```
- For Outlook
```
  "EmailConfiguration": {
    "Host": "smtp-mail.outlook.com",
    "Port": 587,
    "UserName": "your_email@outlook.com",
    "Password": "your_app_password",
    "EnableSsl": true
  }
```

Finally, if you intend to use Dependancy Injection for the service, remember to add to 'Program.cs' for DotNetEmailService:
```chsarp
builder.Services.AddScoped<IEmailService,DotNetEmailService>();
```
or
```chsarp
builder.Services.AddTransient<IEmailService, DotNetEmailService>();
```

And for MailKitEmailService:
```chsarp
builder.Services.AddScoped<IEmailService,MailKitEmailService>();
```
or
```chsarp
builder.Services.AddTransient<IEmailService, MailKitEmailService>();
```

Remember:
> 
    Transient objects are always different; a new instance is provided to every controller and every service.
    Scoped objects are the same within a request, but different across different requests.
    Singleton objects are the same for every object and every request.
[Taken from Stack Overflow](https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences)


### Environments
Has the configuration to manage different environment configurations, including appsettings.json for different environments.
Remember to add your Environment profile to launchSettings.json file and the variable:
```
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Your_Environment"
      }
```

### Logging
Demonstrates error handling and logging. Has a service to log messages to a file and an endpoint to request the log files.
You can copy the file 'FileLoggerProvider.cs' to any project to have file logging capabilities.
Remember to add to Program.cs the line:
```chsarp
builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
```


## Getting Started
### Prerequisites
.NET 8 and 6 SDK
Visual Studio 2022

#### Installation
1. Clone the repository:
2. Open the solution in Visual Studio.
3. Configure the desired project as a StartUp project.
4. Run using IIS Express.

#### Usage

Run each project from Visual Studio to have an idea of how everything works.

#### Configuration

Each project has its configuration settings in the respective appsettings.json file. You can modify these settings according to your requirements.

#### License

This project is licensed under the MIT License.
