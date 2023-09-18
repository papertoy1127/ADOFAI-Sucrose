using System;
using System.Collections.Generic;
using System.Linq;
using ADOFAI;
using EventLib;
using EventLib.CustomEvent;
using EventLib.EventHandler;
using UnityEngine;

namespace Sucrose.CustomTile; 

[CustomEvent("CustomTileEvent", 112701, allowFirstFloor = true, allowMultiple = true)]
[EventCategory(LevelEventCategory.TrackFx)]
public class CustomTileEvent : CustomEventWrapperBase, IFloorUpdateHandler {
    [EventIcon] public static Sprite GetIcon() => Main.customTileEventIcon;
    private const float TOLERANCE = 0.0001f;

    public int startFloor => levelEvent.floor;
    public int endFloor;

    public override void OnAddEvent(int floorId) {
        FindEndFloor();
        GenerateDecos();
    }
    
    public void OnFloorUpdate() {
        FindEndFloor();
        DeleteDecos();
        GenerateDecos();
    }

    public override void OnValueChange(string key, object value) {
        FindEndFloor();
        DeleteDecos();
        GenerateDecos();
        editor.UpdateDecorationObjects();
    }

    public void DeleteDecos() {
        foreach (var e in childDecos.ToList()) {
            DeleteChildDeco(e);
        }
    }

    public void FindEndFloor() {
        var evts = editor.events.Where(e => e.floor > startFloor && e.eventType is LevelEventType.ColorTrack).ToArray();
        endFloor = evts.Any() ? evts.Min(e => e.floor) : editor.floors.Count;
    }

    public void GenerateDecos() {
        for (var i = startFloor; i < endFloor; i++) {
            var this_tile = editor.floors[i];
            bool isCW = true;
            var angle = (float) scrMisc.GetAngleMoved(this_tile.entryangle, this_tile.exitangle, true) * Mathf.Rad2Deg;
            if (angle > 180) {
                angle = 360 - angle;
                isCW = false;
            }
            if (Mathf.Abs(angle - this.angle) > TOLERANCE) continue;
            var start_angle = this_tile.entryangle * Mathf.Rad2Deg;
            var exit_angle = this_tile.exitangle * Mathf.Rad2Deg;

            var deco = new LevelEvent(new Dictionary<string, object> {
                ["eventType"] = nameof(LevelEventType.AddDecoration),
                ["floor"] = i,
                ["decorationImage"] = image,
                ["relativeTo"] = nameof(DecPlacementType.Tile),
                ["rotation"] = (rotation - (isCW ? start_angle : exit_angle) + 270) % 360,
                ["scale"] = new List<object> { scale.x, scale.y },
                ["color"] = color,
                ["depth"] = depth,
                ["imageSmoothing"] = imageSmoothing,
                ["tag"] = $"@CustomTile_{i}",
            });

            AddChildDeco(deco);
        }
    }
    
    [EventField(unit = "°", minFloat = 0, maxFloat = 180)] public float angle { get; set; } = 180;
    [EventField(PropertyType.File, fileType = FileType.Image)] public string image { get; set; } = string.Empty;
    [EventField("editor.AddDecoration.rotation", unit = "°")] public float rotation { get; set; }
    [EventField(unit = "%")] public Vector2 scale { get; set; } = new(100, 100);
    [EventField(PropertyType.Color)] public string color { get; set; } = "ffffffff";
    [EventField("editor.AddDecoration.depth")] public int depth { get; set; } = 1;
    
    [EventField("editor.imageSmoothing")] public bool imageSmoothing { get; set; } = true;
}
