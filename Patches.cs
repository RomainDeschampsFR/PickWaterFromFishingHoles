using Il2Cpp;
using Il2CppTLD.Gear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using Il2CppTLD.IntBackedUnit;

namespace PickWaterFromFishingHoles
{
    class Patches
    {
        public static Panel_PickWater panel = new();
        public static WaterSource waterSource = new();
        public static WaterSupply waterSupply = GearItem.LoadGearItemPrefab("GEAR_WaterSupplyNotPotable").GetComponent<WaterSupply>();
        public static bool updateUnits = false;

        [HarmonyPatch(typeof(IceFishingHole), nameof(IceFishingHole.PerformInteraction))]
        internal static class IceFishingHole_PerformInteraction_OpenWaterPanel
        {
            private static bool Prefix(IceFishingHole __instance)
            {
                if (Input.GetKey(Settings.settings.HoldKey))
                {

                    if (__instance._NormalizedFrozen_k__BackingField < 0.01f)
                    {
                        if (__instance.m_LootTable.name.Contains("Fresh"))
                        {
                            waterSource = new WaterSource();
                            waterSource.m_CurrentLiquidQuality = LiquidQuality.NonPotable;
                            waterSource.m_CurrentLiters = ItemLiquidVolume.FromLiters(100);

                            panel = InterfaceManager.GetPanel<Panel_PickWater>();
                            panel.SetWaterSourceForTaking(waterSource, waterSupply);
                            panel.Enable(true);
                            panel.m_ExecuteAll_Button.GetComponent<UIButton>().isEnabled = false;
                            updateUnits = true;
                        }
                        else
                        {
                            HUDMessage.AddMessage("THIS IS NOT A FRESH WATER SOURCE",3);
                        }
                            return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Panel_PickWater), nameof(Panel_PickWater.Enable))]
        internal static class Panel_PickWater_Enable_FromFichingHoles
        {
            private static bool Prefix(Panel_PickWater __instance)
            {
                updateUnits = false;
                panel.m_ExecuteAll_Button.GetComponent<UIButton>().isEnabled = true;
                return true;
            }
        }

        [HarmonyPatch(typeof(Panel_PickWater), nameof(Panel_PickWater.Update))]
        internal static class Panel_PickWater_Update_FromFichingHoles
        {
            private static void Postfix(Panel_PickWater __instance)
            {
                if (updateUnits)
                {
                    int index = __instance.m_Label_NumUnits.mText.IndexOf('/');
                    int indexGal = __instance.m_Label_NumUnits.mText.IndexOf('g');

                    if (index != -1)
                    {
                        string unit = indexGal == -1 ? "L" : "gal";
                        __instance.m_Label_NumUnits.mText = __instance.m_Label_NumUnits.mText.Substring(0, index) + unit;
                        __instance.m_Label_NumUnits.ProcessText();
                    }
                }
            }
        }
    }
}
