﻿using System;
using System.Collections.Generic;
using Dalamud.Game.Command;
using Dalamud.Logging;
using KamiLib.Interfaces;

namespace KamiLib.CommandSystem;

public class CommandManager : IDisposable
{
    private string SettingsCommand => $"/{KamiLib.PluginName.ToLower()}";
    private string HelpCommand => $"/{KamiLib.PluginName.ToLower()} help";

    public readonly List<IPluginCommand> Commands = new();
    
    public CommandManager()
    {
        Commands.Add(new HelpCommands());
        
        Service.Commands.AddHandler(SettingsCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "open configuration window"
        });

        Service.Commands.AddHandler(HelpCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "display a list of all available sub-commands"
        });
    }
    
    public void Dispose()
    {
        Service.Commands.RemoveHandler(SettingsCommand);
        Service.Commands.RemoveHandler(HelpCommand);
    }

    public void AddCommand(IPluginCommand command)
    {
        Commands.Add(command);
    }
    
    public void OnCommand(string command, string arguments)
    {
        PluginLog.Debug($"Received Command `{command}` `{arguments}`");

        var commandData = Command.GetCommandData(command.ToLower(), arguments.ToLower());
        Command.ProcessCommand(commandData, Commands);
    }
}