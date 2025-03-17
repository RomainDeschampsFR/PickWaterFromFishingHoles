using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;
using Il2CppTLD.IntBackedUnit;
using Il2Cpp;
using Il2CppVLB;


namespace PickWaterFromFishingHoles
{
	public class Main : MelonMod
	{
        public override void OnInitializeMelon()
		{
            Debug.Log($"[{Info.Name}] Version {Info.Version} loaded!");
            Settings.OnLoad();
        }
    }
}