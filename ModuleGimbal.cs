﻿using UnityEngine;
using System;

namespace ClawKSP
{
	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public class MGHook : MonoBehaviour
	{
		public void Start()
		{
			Controller.HookModule("ModuleGimbal", "MG");
		}
	}
	public class MG : PartModule
	{
		ModuleEngines engineModule;
		ModuleGimbal gimbalModule;
		[KSPField(isPersistant = true, guiName = "Engine Thrust", guiActive = true, guiActiveEditor = true)]
		float engineFlow;
        [KSPField(isPersistant = true, guiName = "Automatic Gimbal Lock", guiActive = true, guiActiveEditor = true), UI_Toggle(enabledText = "True", disabledText = "False")]
		public bool blockerEnabled = true;
		public override void OnStart(StartState state)
		{
			try
			{
				base.OnStart(state);
				gimbalModule = part.FindModuleImplementing<ModuleGimbal>();
				engineModule = part.FindModuleImplementing<ModuleEngines>();
				engineFlow = engineModule.fuelFlowGui;
			}
			catch (Exception ex)
			{
				Debug.LogError("PROBLEM.\n" + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public void FixedUpdate()
		{
			if (blockerEnabled)
			{
				engineFlow = engineModule.fuelFlowGui;
				if (engineFlow < 0.000001f)
				{
					gimbalModule.gimbalActive = false;
				}
				else
				{
					gimbalModule.gimbalActive = true;
				}
				Debug.Log(gimbalModule.gimbalActive);
			}
		}
	}
}