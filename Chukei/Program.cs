using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Chukei;

internal class Program
{
    public static Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Exit("Usage: ./chukei.exe [path-to-settings]");
        }

        var settingsPath = args[0];
        
        if (!File.Exists(settingsPath))
        {
            Exit($"Settings file does not exist at: '{settingsPath}'.");
        }

        var settings = JsonSerializer.Deserialize(File.OpenRead(settingsPath), SettingsContext.Default.Settings);

        if (settings is null) Exit("Invalid settings file provided.");

        _ = Server.BeginListen(settings.Port);

        while (true)
        {
            var input = Console.ReadLine();
            if (input is null) continue;

            input = input.Trim();

            switch (input)
            {
                case "q":
                case "quit":
                    Server.EndListen();
                    Exit("Shutting down...");
                    break;
            }
        }
    }

    [DoesNotReturn]
    internal static void Exit(string? message = null)
    {
        if (message is null) Environment.Exit(0);

        Console.WriteLine(message);
        Environment.Exit(0);
    }
}