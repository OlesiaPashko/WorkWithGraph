﻿using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WorkWithGraph
{
    class Program
    {
    	static IConfigurationRoot LoadAppSettings()
		{
    		var appConfig = new ConfigurationBuilder()
        	.AddUserSecrets<Program>()
        	.Build();

    		// Check for required settings
    		if (string.IsNullOrEmpty(appConfig["appId"]) ||
        	string.IsNullOrEmpty(appConfig["scopes"]))
    		{
        		return null;
    		}

    		return appConfig;
		}

        static void Main(string[] args)
        {
            var appConfig = LoadAppSettings();

			if (appConfig == null)
			{
    			Console.WriteLine("Missing or invalid appsettings.json...exiting");
    			return;
			}

			var appId = appConfig["appId"];
			var scopesString = appConfig["scopes"];
			var scopes = scopesString.Split(';');

			// Initialize the auth provider with values from appsettings.json
			var authProvider = new DeviceCodeAuthProvider(appId, scopes);

			// Request a token to sign in the user
			var accessToken = authProvider.GetAccessToken().Result;

			// Initialize Graph client
			GraphHelper.Initialize(authProvider);

			// Get signed in user
			var user = GraphHelper.GetMeAsync().Result;
			Console.WriteLine($"Welcome {user.DisplayName}!\n");
            
            int choice = -1;

            while (choice != 0) {
                Console.WriteLine("Please choose one of the following options:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Display access token");
                Console.WriteLine("2. List calendar events");

                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch (System.FormatException)
                {
                    // Set to invalid value
                    choice = -1;
                }

                switch(choice)
                {
                    case 0:
                        // Exit the program
                        Console.WriteLine("Goodbye...");
                        break;
                    case 1:
                        Console.WriteLine($"Access token: {accessToken}\n");
                        break;
                    case 2:
                        // List the calendar
                        ListCalendarEvents();
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static string FormatDateTimeTimeZone(Microsoft.Graph.DateTimeTimeZone value)
        {
            // Get the timezone specified in the Graph value
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(value.TimeZone);
            // Parse the date/time string from Graph into a DateTime
            var dateTime = DateTime.Parse(value.DateTime);

            // Create a DateTimeOffset in the specific timezone indicated by Graph
            var dateTimeWithTZ = new DateTimeOffset(dateTime, timeZone.BaseUtcOffset)
            .ToLocalTime();

            return dateTimeWithTZ.ToString("g");
        }

        static void ListCalendarEvents()
        {
            var events = GraphHelper.GetEventsAsync().Result;

            Console.WriteLine("Events:");

            foreach (var calendarEvent in events)
            {
                Console.WriteLine($"Subject: {calendarEvent.Subject}");
                Console.WriteLine($"  Organizer: {calendarEvent.Organizer.EmailAddress.Name}");
                Console.WriteLine($"  Start: {FormatDateTimeTimeZone(calendarEvent.Start)}");
                Console.WriteLine($"  End: {FormatDateTimeTimeZone(calendarEvent.End)}");
            }
        }
    }
}
