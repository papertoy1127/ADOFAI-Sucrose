using System;
using System.Collections.Generic;
using System.Linq;
using ADOFAI;
using DG.Tweening;
using EventLib;
using EventLib.CustomEvent;
using EventLib.EventHandler;
using UnityEngine;

namespace Sucrose.CustomTile; 

[CustomEvent("MoveCustomTileEvent", 112702, allowFirstFloor = true, allowMultiple = true)]
[EventCategory(LevelEventCategory.TrackFx)]
public class MoveCustomTileEvent : CustomEventWrapperBase, IFloorUpdateHandler {
    [EventIcon] public static Sprite GetIcon() => Main.moveCustomTileEventIcon;
    private const float TOLERANCE = 0.0001f;

    public LevelEvent moveEvent;

    public override void OnAddEvent(int floorId) {
        moveEvent = AddChildEvent(levelEvent.floor, LevelEventType.MoveDecorations);
        UpdateTag();

        foreach (var (key, value) in levelEvent.disabled) {
            moveEvent.disabled[key] = value;
        }
    }

    public override void OnLoadEvent(Dictionary<string, object> data, Dictionary<string, bool> disabled) {
        moveEvent = children[0];
        base.OnLoadEvent(data, disabled);
    }

    public void UpdateTag() {
        var floors = new List<int>();
        var startfloor = scnGame.IDFromTile(startTile, levelEvent.floor, editor.floors);
        var endfloor = scnGame.IDFromTile(endTile, levelEvent.floor, editor.floors);
        for (var i = startfloor; i < endfloor + 1; i += gapLength + 1) {
            floors.Add(i);
        }
        
        moveEvent["tag"] = string.Join(" ", floors.Select(f => $"@CustomTile_{f}"));
    }
    public void OnFloorUpdate() => UpdateTag();
    public override void OnValueChange(string key, object value) => UpdateTag();

    public override void OnDisableChange(string key, bool enabled) {
        moveEvent.disabled[key] = enabled;
    }

    [EventField(PropertyType.Tile, unit = "tilesFrom")]
    public Tuple<int, TileRelativeTo> startTile { get; set; } = new(0, TileRelativeTo.ThisTile);
    
    [EventField(PropertyType.Tile, unit = "tilesFrom")]
    public Tuple<int,TileRelativeTo> endTile { get; set; } = new(0, TileRelativeTo.ThisTile);
    
    [EventField]
    public int gapLength { get; set; }
    
    [EventField] public float duration {
        get => (float) moveEvent["duration"];
        set => moveEvent["duration"] = value;
    }
    
    [EventField]
    [CanBeDisabled(true)]
    public Vector2 positionOffset {
        get => (Vector2) moveEvent["positionOffset"];
        set => moveEvent["positionOffset"] = value;
    }
    
    [EventField(unit = "°")]
    [CanBeDisabled]
    public float rotationOffset {
        get => (float) moveEvent["rotationOffset"];
        set => moveEvent["rotationOffset"] = value;
    }
    
    [EventField(unit = "%")]
    [CanBeDisabled]
    public Vector2 scale {
        get => (Vector2) moveEvent["scale"];
        set => moveEvent["scale"] = value;
    }
    
    [EventField(PropertyType.Color)]
    [CanBeDisabled]
    public string color {
        get => (string) moveEvent["color"];
        set => moveEvent["color"] = value;
    }
    
    [EventField(unit = "%")]
    [CanBeDisabled]
    public float opacity {
        get => (float) moveEvent["opacity"];
        set => moveEvent["opacity"] = value;
    }
    
    [EventField]
    public float angleOffset {
        get => (float) moveEvent["angleOffset"];
        set => moveEvent["angleOffset"] = value;
    }
    /*
    [EventField("editor.AddDecoration.relativeTo")] public bool visible {
        get => (bool) moveEvent["visible"];
        set => moveEvent["visible"] = value;
    }
    
    [EventField("editor.MoveDecorations.decorationImage", PropertyType.File, fileType = FileType.Image)]
    public string decorationImage {
        get => (string) moveEvent["decorationImage"];
        set => moveEvent["decorationImage"] = value;
    }
    
    [EventField("editor.MoveDecorations.depth")]
    public float depth {
        get => (float) moveEvent["depth"];
        set => moveEvent["depth"] = value;
    }*/
    
    [EventField]
    public Ease ease {
        get => (Ease) moveEvent["ease"];
        set => moveEvent["ease"] = value;
    }
    
    [EventField]
    public string eventTag {
        get => (string) moveEvent["eventTag"];
        set => moveEvent["eventTag"] = value;
    }
}
