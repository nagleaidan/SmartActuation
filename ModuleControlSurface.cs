using System;
using UnityEngine;

namespace ClawKSP
{
	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public class MCSHook : MonoBehaviour
	{
		public void Start()
		{
			Controller.HookModule("ModuleControlSurface", "MCS");
			Controller.HookModule("ModuleAeroSurface", "MCS");
		}
	}

	public class MCS : PartModule
	{
		private ModuleControlSurface ControlSurfaceModule;
		private float ctrlSurfaceRange;
		private float vacuumRange = 1.0f;

		[KSPField(isPersistant = true, guiName = "Locked in Vacum", guiActive = true, guiActiveEditor = true), UI_Toggle(enabledText = "On", disabledText = "Off")]
		public bool blockerEnabled = true;

		public override void OnStart(StartState state)
		{
			base.OnStart(state);

			ControlSurfaceModule = part.FindModuleImplementing<ModuleControlSurface>();
			if (null == ControlSurfaceModule)
			{
				Debug.LogWarning("Did not find Control Surface Module.");
				return;
			}

			ctrlSurfaceRange = ControlSurfaceModule.ctrlSurfaceRange;
			ControlSurfaceModule.ctrlSurfaceRange = ctrlSurfaceRange * vacuumRange;
		}

		public void FixedUpdate()
		{
			ControlSurfaceModule = part.FindModuleImplementing<ModuleControlSurface>();
			if (null == ControlSurfaceModule)
			{
				return;
			}
			if (!ControlSurfaceModule.deploy&&blockerEnabled)
			{
				if (FlightGlobals.getStaticPressure(part.transform.position) < 0.001f)
				{
					if (vacuumRange > 0.001f)
					{
						vacuumRange -= 0.05f;
					}
					else
					{
						vacuumRange = 0.001f;
					}
				}
				else
				{
					if (vacuumRange < 1.0f)
					{
						vacuumRange += 0.05f;
					}
					else
					{
						vacuumRange = 1.0f;
					}
				}
			}
			else
			{
				vacuumRange = 1.0f;
			}
			ControlSurfaceModule.ctrlSurfaceRange = ctrlSurfaceRange* vacuumRange * Mathf.Sign(ControlSurfaceModule.ctrlSurfaceRange);
		}
		//public void Update(){Debug.Log("test" + blockerEnabled);} //debugger
	}
}