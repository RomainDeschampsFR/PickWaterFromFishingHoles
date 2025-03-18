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
using Il2CppVLB;

namespace PickWaterFromFishingHoles
{
    class Patches
    {
        public static WaterSource? waterSource;
        public static Panel_PickWater panel = InterfaceManager.GetPanel<Panel_PickWater>();
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
                            if (__instance.gameObject.GetComponent<WaterSource>() == null)
                            {
                                __instance.gameObject.AddComponent<WaterSource>();
                            }
                            __instance.gameObject.GetComponent<WaterSource>().PerformInteraction();
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
        internal static class Panel_PickWater_Enable_FromFishingHoles
        {
            private static void Postfix(Panel_PickWater __instance)
            {
                panel.m_ExecuteAll_Button.GetComponent<UIButton>().isEnabled = true;
            }
        }

        [HarmonyPatch(typeof(Panel_PickWater), nameof(Panel_PickWater.Update))]
        internal static class Panel_PickWater_Update_FromFishingHoles
        {
            private static void Postfix(Panel_PickWater __instance)
            {
                if (__instance.m_WaterSource != null && __instance.m_WaterSource.name.Contains("FishingHole") && __instance.m_ExecuteAction == PickWaterExecuteAction.TakeFromWaterSource)
                {
                    __instance.m_WaterSource.m_CurrentLiquidQuality = LiquidQuality.NonPotable;
                    __instance.m_WaterSource.m_Capacity = ItemLiquidVolume.FromLiters(1000);
                    __instance.m_WaterSource.m_CurrentLiters = ItemLiquidVolume.FromLiters(1000);
                    panel.m_ExecuteAll_Button.GetComponent<UIButton>().isEnabled = false;

                    int index = __instance.m_Label_NumUnits.mText.IndexOf('/');
                    int indexGal = __instance.m_Label_NumUnits.mText.IndexOf('g');

                    if (index != -1)
                    {
                        string unit = indexGal == -1 ? "L" : "gal";
                        __instance.m_Label_NumUnits.mText = __instance.m_Label_NumUnits.mText.Substring(0, index) + unit;
                        __instance.m_Label_NumUnits.ProcessText();
                    }
                    __instance.m_WaterSource.gameObject.SetActive(true);
                }
            }
        }
        
        [HarmonyPatch(typeof(WaterSource), nameof(WaterSource.PerformInteraction))]
        internal static class WaterSource_PerformInteraction_FromFishingHoles
        {
            private static void Prefix(WaterSource __instance)
            {
                if (__instance.name.Contains("FishingHole"))
                {
                    __instance.m_CurrentLiquidQuality = LiquidQuality.NonPotable;
                    __instance.m_Capacity = ItemLiquidVolume.FromLiters(1000);
                    __instance.m_CurrentLiters = ItemLiquidVolume.FromLiters(1000);
                }
            }
        }
    }
}
