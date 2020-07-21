# Console application using Microsoft Graph.

## It that can get all events from calendar or just write you token

# How set up and run

Please do next commands from console inside the app folder (but paste YOUR_APP_ID from Your Azure app registration entity):

1. dotnet user-secrets init
1. dotnet user-secrets set appId "YOUR_APP_ID"
1. dotnet user-secrets set scopes "User.Read;Calendars.Read"
1. dotnet build
1. dotnet run
