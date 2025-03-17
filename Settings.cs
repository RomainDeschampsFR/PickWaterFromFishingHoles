using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModSettings;
using PickWaterFromFishingHoles;
using System.Reflection;
using UnityEngine;

namespace PickWaterFromFishingHoles
{
    class PickWaterFromFishingHolesSettings : JsonModSettings
    {
        // WATER PANEL
        [Section("Water picking panel")]
        [Name("Hold button to display water panel")]
        [Description("Hold this key when clicking on a fishing hole (Left mouse button) in order to open the water picking panel")]
        public KeyCode HoldKey = KeyCode.LeftAlt;

    }

    internal static class Settings
    {
        public static PickWaterFromFishingHolesSettings settings = new();

        public static void OnLoad()
        {
            settings.AddToModSettings("Pick Water From Fishing Holes");
        }
    }
}
