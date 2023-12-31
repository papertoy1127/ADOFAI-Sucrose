﻿using ADOFAI;
using ERDPatch;
using EventLib.CustomEvent;
using HarmonyLib;
using Mono.WebBrowser;
using Sucrose.CustomTile;
using UnityEngine;
using UnityModManagerNet;

namespace Sucrose; 

public abstract class Main {
    public static Harmony harmony { get; private set; }
    internal static UnityModManager.ModEntry entry;
    
    internal static Sprite customTileEventIcon;
    internal static Sprite moveCustomTileEventIcon;
    internal static Texture2D logo;
    
    private static bool Load(UnityModManager.ModEntry mod_entry) {
        entry = mod_entry;
            
        entry.OnGUI = OnGUI;
        harmony = new Harmony(mod_entry.Info.Id);
        harmony.PatchAll();
        
        CustomEventManager.RegisterCustomEvent<CustomTileEvent>();
        CustomEventManager.RegisterCustomEvent<MoveCustomTileEvent>();
        RDStringPatcher.AddString("editor.CustomTileEvent.angle", "Target Angle");
        RDStringPatcher.AddString("editor.CustomTileEvent.rotation", "Rotation");
        RDStringPatcher.AddString("editor.CustomTileEvent", "Custom Sprite Tile");
        RDStringPatcher.AddString("editor.MoveCustomTileEvent", "Move Custom Tile");
        
        var stream = typeof(Main).Assembly.GetManifestResourceStream("Sucrose.Resources.CustomTile.png")!;
        var buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        var texture = new Texture2D(2, 2);
        texture.LoadImage(buffer);
        customTileEventIcon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
        stream = typeof(Main).Assembly.GetManifestResourceStream("Sucrose.Resources.MoveCustomTile.png")!;
        buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        texture = new Texture2D(2, 2);
        texture.LoadImage(buffer);
        moveCustomTileEventIcon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
        stream = typeof(Main).Assembly.GetManifestResourceStream("Sucrose.Resources.Sucrose-logo.png")!;
        buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        logo = new Texture2D(1274, 288);
        logo.LoadImage(buffer);
        
        return true;
    }

    public static string test = "";
    private static void OnGUI(UnityModManager.ModEntry mod_entry) {
        GUILayout.Label(logo, GUILayout.Width(319), GUILayout.Height(72));
        GUILayout.Space(-20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(80);
        GUILayout.Label("<size=24><b>v0.2</b> Alpha</size>");
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        /*
        test = GUILayout.TextField(test);
        try {
            GUILayout.Label(RDString.Get(test));
        } catch (Exception e) {
            GUILayout.Label(e.ToString());
        }
        */
    }
}
