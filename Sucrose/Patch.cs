using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ADOFAI;
using EventLib;
using EventLib.Mapping;
using HarmonyLib;
using Sucrose.CustomTile;

namespace Sucrose; 

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class Patch {
    [HarmonyPatch(typeof(scnGame), "ApplyEventsToFloors", typeof(List<scrFloor>))]
    public static class FloorIconPatch {
        public static void Postfix() {
            if (scnEditor.instance == null) return;
            if (scnEditor.instance.playMode) return;
            
            for (var i = 0; i < scnEditor.instance.floors.Count; i++) {
                var floor = scnEditor.instance.floors[i];
                L.og($"floor {i} icon: {floor.floorIcon} event: {floor.eventIcon}");
                if (floor.floorIcon != FloorIcon.Vfx) continue;
                if (floor.eventIcon != LevelEventType.None) continue;
                var evnts = scnEditor.instance.events
                    .Where(e => e.floor == i)
                    .Where(e => !e.IsChildEvent())
                    .Select(e => e.eventType).Distinct().ToArray();
                if (evnts.Length != 1) continue;
                floor.eventIcon = evnts[0];
            }
        }
    }
}
