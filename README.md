
# Meet Senior back-end

This ASP.NET project is a chat platform designed for seniors of a college to communicate with juniors and share posts. The project is built using .NET 6 and includes the following features:


Contributing
If you find any bugs or issues with this project, feel free to open an issue or submit a pull request. Contributions are always welcome.
.


## Featuring

Chat 
Seniors can chat with juniors and vice versa. Each chat message is timestamped and displayed in a chat window.
Post Sharing
Seniors can create and share posts on various topics. Juniors can view these posts, comment on them, and react to them
.
Point System
For each action a junior takes toward a senior, the senior gets a point. These actions include sending a message, reacting to a post, commenting on a post, etc.
## Requirements

.NET 6 SDK
Visual Studio 2022 (or later)
Once you have installed the required software, you can clone this repository to your local machine and open the project in Visual Studio. You can then build and run the project to test it locally.

.NET 6 SDK Download link: 
https://download.visualstudio.microsoft.com/download/pr/ef7f0961-bb98-4d11-8cb2-9697a9cf55f0/ff7adb80e8d1fa056f5d51616a4fdcd1/dotnet-sdk-6.0.407-win-arm64.exe


This project uses the following dependencies:


Entity Framework Core for database access (Entity Framework 7 or higher)


```bash
  dotnet --version
  dotnet ef --version
  
```

To install Entity Framework command line: 
```bach
  dotnet tool install --global dotnet-ef
  ```


## Installation

Install all the dependecies with

```bash
  dotnet build
```

Or buil it using visual studio 2022
    
To init migration database, execute the following command:

```bash
  add-migration init
  update-database
```

These following commands are executed through Pomelo.EntityFrameworkCore library.

Configure the appsettings.json in the project root (where there is *.csproj file) with your own configuration environnement

```bash
{
  "Token": "{your_security_key}",
  "Key": "{your_key}",
  "Server": "your server",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Default": "Server={your server};user={username};password={password};Database={database name}"
  }
}
```
## Running locally

To run the application

```bash
  dotnet watch
```

Or run it via visual studio 2022
    