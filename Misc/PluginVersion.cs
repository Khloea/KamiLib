﻿using System.Diagnostics;
using ImGuiNET;
using KamiLib.Drawing;

namespace KamiLib.Misc;


public class PluginVersion
{
    private static PluginVersion? _instance;
    public static PluginVersion Instance => _instance ??= new PluginVersion();

    private readonly string versionText;
    
    private PluginVersion()
    {
        versionText = GetVersionText();
    }
    
    private static string GetVersionText()
    {
        var callingAssembly = new StackTrace().GetFrame(3)?.GetMethod()?.DeclaringType?.Assembly;

        if (callingAssembly is not null)
        {
            var assemblyInformation = callingAssembly.FullName!.Split(',');

            return assemblyInformation[1].Replace('=', ' ');
        }

        return "Unable to Read Assembly";
    }
    
    public void DrawVersionText()
    {
        var region = ImGui.GetContentRegionAvail();

        var versionTextSize = ImGui.CalcTextSize(versionText) / 2.0f;
        var cursorStart = ImGui.GetCursorPos();
        cursorStart.X += region.X / 2.0f - versionTextSize.X;

        ImGui.SetCursorPos(cursorStart);
        ImGui.TextColored(Colors.Grey, versionText);
    }
}