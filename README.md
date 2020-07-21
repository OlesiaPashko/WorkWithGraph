# Console application using Microsoft Graph.

##It that can get all events from calendar or just write you token

#How set up and run

Please do next commands from console inside the app folder (but paste YOUR_APP_ID from Your Azure app registration entity):

dotnet user-secrets init
dotnet user-secrets set appId "YOUR_APP_ID"
dotnet user-secrets set scopes "User.Read;Calendars.Read"
dotnet build
dotnet run
