using UnityEngine;
using System;

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
		ModuleControlSurface ControlSurfaceModule;
		float ctrlSurfaceRange;
		float vacuumRange = 1.0f;

		[KSPField(isPersistant = true, guiName = "Locked in Vacum", guiActive = true, guiActiveEditor = true), UI_Toggle(enabledText = "True", disabledText = "False")]
		public bool blockerEnabled = true;

		public override void OnStart(StartState state)
		{
			try
			{
				base.OnStart(state);
				ControlSurfaceModule = part.FindModuleImplementing<ModuleControlSurface>();
				ctrlSurfaceRange = ControlSurfaceModule.ctrlSurfaceRange;
				ControlSurfaceModule.ctrlSurfaceRange = ctrlSurfaceRange* vacuumRange;
			}
			catch (Exception ex)				
			{
				Debug.LogError("PROBLEM.\n" + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public void FixedUpdate()
		{
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
	}
}